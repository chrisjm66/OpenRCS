<script lang="ts">
	import { hitTestCanvas, panCamera, render, zoomCameraAt, type Camera } from '$lib/canvas';
	import type { SignalDiagram } from '../../bindings';
	import { onMount } from 'svelte';

	const { diagram }: { diagram: SignalDiagram | undefined } = $props();

	let canvas: HTMLCanvasElement;
	let container: HTMLDivElement;

	let camera: Camera = $state({
		x: 0,
		y: 0,
		zoom: 1
	});

	let frameRequested = false;

	let panning = false;
	let lastPointerX = 0;
	let lastPointerY = 0;

	function requestRender() {
		if (frameRequested) return;

		frameRequested = true;

		requestAnimationFrame(() => {
			frameRequested = false;

			const context = canvas.getContext('2d');

			if (context && diagram) {
				render(canvas, context, camera, diagram);
			}
		});
	}

	function resizeCanvas() {
		const rect = container.getBoundingClientRect();
		const dpr = window.devicePixelRatio || 1;

		const width = Math.round(rect.width * dpr);
		const height = Math.round(rect.height * dpr);

		if (canvas.width === width && canvas.height === height) {
			return;
		}

		canvas.width = width;
		canvas.height = height;

		requestRender();
	}

	function handlePointerDown(event: PointerEvent) {
		// Right mouse button = pan
		if (event.button === 2) {
			panning = true;

			lastPointerX = event.clientX;
			lastPointerY = event.clientY;

			canvas.setPointerCapture(event.pointerId);

			return;
		}

		// Left click = hit detection
		if (event.button === 0) {
			const hit = hitTestCanvas(canvas, event, camera);

			if (hit) {
				console.log('Hit:', hit);
			}
		}
	}

	function handlePointerMove(event: PointerEvent) {
		if (!panning) return;

		const dx = event.clientX - lastPointerX;
		const dy = event.clientY - lastPointerY;

		panCamera(camera, dx, dy);

		lastPointerX = event.clientX;
		lastPointerY = event.clientY;

		requestRender();
	}

	function handlePointerUp(event: PointerEvent) {
		if (!panning) return;

		panning = false;

		if (canvas.hasPointerCapture(event.pointerId)) {
			canvas.releasePointerCapture(event.pointerId);
		}
	}

	function handleWheel(event: WheelEvent) {
		event.preventDefault();

		const rect = canvas.getBoundingClientRect();

		const x = event.clientX - rect.left;
		const y = event.clientY - rect.top;

		// Trackpad pinch
		if (event.ctrlKey) {
			zoomCameraAt(canvas, camera, x, y, Math.exp(-event.deltaY * 0.01));

			requestRender();
			return;
		}

		// Likely two-finger trackpad scrolling
		const likelyTrackpad =
			event.deltaMode === WheelEvent.DOM_DELTA_PIXEL && Math.abs(event.deltaX) > 0;

		if (likelyTrackpad) {
			panCamera(camera, -event.deltaX, -event.deltaY);

			requestRender();
			return;
		}

		// Mouse wheel zoom
		zoomCameraAt(canvas, camera, x, y, Math.exp(-event.deltaY * 0.0015));

		requestRender();
	}

	function handleContextMenu(event: MouseEvent) {
		event.preventDefault();
	}

	$effect(() => {
		if (diagram) {
			requestRender();
		}
	});

	onMount(() => {
		resizeCanvas();

		const observer = new ResizeObserver(() => {
			resizeCanvas();
		});

		// Important: observe the container, NOT the canvas.
		observer.observe(container);

		return () => {
			observer.disconnect();
		};
	});
</script>

<div bind:this={container} class="relative h-full w-full grow overflow-hidden">
	<canvas
		bind:this={canvas}
		onpointerdown={handlePointerDown}
		onpointermove={handlePointerMove}
		onpointerup={handlePointerUp}
		onpointercancel={handlePointerUp}
		oncontextmenu={handleContextMenu}
		onwheel={handleWheel}
		class="absolute inset-0 h-full w-full bg-black"
	></canvas>
</div>
