<script lang="ts">
	import { drawSignals, drawTracks } from "$lib/canvas";
	import type { SignalDiagram } from "../../bindings";

	const { diagram }: { diagram: SignalDiagram | undefined } = $props();
	let canvas: HTMLCanvasElement;

	initializeCanvas();
	drawLayout();

	function initializeCanvas() {
		$effect(() => {
			const context = canvas.getContext('2d');

			const rect = canvas.getBoundingClientRect();
			const dpr = window.devicePixelRatio || 1;

			canvas.width = rect.width * dpr;
			canvas.height = rect.height * dpr;

			context?.scale(dpr, dpr);
		});
	}

	function drawLayout() {
		$effect(() => {
			const context = canvas.getContext('2d');

			if (context == null) {
				return;
			}

			drawTracks(diagram, context)	
			drawSignals(diagram, context)
		})
	}
</script>

<canvas bind:this={canvas} class="z-10 h-full w-full bg-black"></canvas>
