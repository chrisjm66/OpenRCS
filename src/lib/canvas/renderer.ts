import type { SignalDiagram } from '../../bindings';

const hitTargets: HitTarget[] = [];
const SIGNAL_RADIUS = 5;
const TRACK_STROKE = 5;

export function render(
	canvas: HTMLCanvasElement,
	ctx: CanvasRenderingContext2D,
	camera: Camera,
	diagram: SignalDiagram
) {
	hitTargets.length = 0;
	const rect = canvas.getBoundingClientRect();
	const dpr = window.devicePixelRatio || 1;

	hitTargets.length = 0;

	// Work in actual backing-buffer pixels first
	ctx.setTransform(1, 0, 0, 1, 0, 0);
	ctx.clearRect(0, 0, canvas.width, canvas.height);

	// From here onward, work in CSS pixels
	ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

	ctx.translate(rect.width / 2, rect.height / 2);
	ctx.scale(camera.zoom, camera.zoom);
	ctx.translate(-camera.x, -camera.y);

	drawTracks(diagram, ctx);
	drawSignals(diagram, ctx);
}

export function drawTracks(diagram: SignalDiagram | undefined, context: CanvasRenderingContext2D) {
	if (!diagram || !diagram.tracks) {
		console.log('null');
		return;
	} else {
		Object.entries(diagram.tracks).forEach(([id, track]) => {
			console.log(id);
			if (track && track.positions) {
				console.log(track);

				for (let i = 1; i < track.positions.length; i++) {
					const previousPosition = track.positions[i - 1];
					const currentPosition = track.positions[i];

					context.beginPath();
					context.moveTo(previousPosition.x!, previousPosition.y!);
					context.lineTo(currentPosition.x!, currentPosition.y!);
					context.lineWidth = TRACK_STROKE;
					context.strokeStyle = 'darkgrey';
					context.stroke();

					hitTargets.push({
						type: 'track',
						id: track.track_circuits.toString(),
						shape: {
							type: 'line',
							x1: previousPosition.x!,
							y1: previousPosition.y!,
							x2: currentPosition.x!,
							y2: currentPosition.y!,
							tolerance: 5
						}
					});
					// TODO perform state lookup to add headcode, color, etc.
				}
			} else {
				console.log('circuit is null');
			}
		});
	}
}

export function drawSignals(diagram: SignalDiagram | undefined, context: CanvasRenderingContext2D) {
	if (!diagram || !diagram.signals) {
		return;
	}

	Object.entries(diagram.signals).forEach(([id, signal]) => {
		if (signal && signal.position) {
			const x = signal.position.x!;
			const y = signal.position.y!;

			context.beginPath();
			context.arc(signal.position.x!, signal.position.y!, SIGNAL_RADIUS, 0, Math.PI * 2);
			context.fillStyle = 'yellow';
			context.fill();

			hitTargets.push({
				type: 'signal',
				id,
				shape: {
					type: 'circle',
					x,
					y,
					radius: 10
				}
			});
		}
	});
}

function convertScreenToWorldCoordinates(
	screenX: number,
	screenY: number,
	canvas: HTMLCanvasElement,
	camera: Camera
) {
	const rect = canvas.getBoundingClientRect();

	return {
		x: (screenX - rect.width / 2) / camera.zoom + camera.x,
		y: (screenY - rect.height / 2) / camera.zoom + camera.y
	};
}

function isPointInCircle(px: number, py: number, cx: number, cy: number, radius: number) {
	const dx = px - cx;
	const dy = py - cy;

	return dx * dx + dy * dy <= radius * radius;
}

function calculateDistanceToSegment(
	px: number,
	py: number,
	x1: number,
	y1: number,
	x2: number,
	y2: number
) {
	const dx = x2 - x1;
	const dy = y2 - y1;

	const lengthSquared = dx * dx + dy * dy;

	if (lengthSquared === 0) {
		return Math.hypot(px - x1, py - y1);
	}

	let t = ((px - x1) * dx + (py - y1) * dy) / lengthSquared;

	t = Math.max(0, Math.min(1, t));

	const x = x1 + t * dx;
	const y = y1 + t * dy;

	return Math.hypot(px - x, py - y);
}

export function isSignalHit(
	mouseX: number,
	mouseY: number,
	signalX: number,
	signalY: number,
	radius: number
) {
	const dx = mouseX - signalX;
	const dy = mouseY - signalY;

	return dx * dx + dy * dy <= radius * radius;
}

function hitTest(x: number, y: number): HitTarget | undefined {
	for (let i = hitTargets.length - 1; i >= 0; i--) {
		const target = hitTargets[i];

		switch (target.shape.type) {
			case 'circle': {
				const dx = x - target.shape.x;
				const dy = y - target.shape.y;

				if (dx * dx + dy * dy <= target.shape.radius * target.shape.radius) {
					return target;
				}

				break;
			}

			case 'line': {
				const distance = calculateDistanceToSegment(
					x,
					y,
					target.shape.x1,
					target.shape.y1,
					target.shape.x2,
					target.shape.y2
				);

				if (distance <= target.shape.tolerance) {
					return target;
				}

				break;
			}
		}
	}

	return undefined;
}

export function hitTestCanvas(
	canvas: HTMLCanvasElement,
	event: PointerEvent,
	camera: Camera
): HitTarget | undefined {
	const rect = canvas.getBoundingClientRect();

	const screenX = event.clientX - rect.left;
	const screenY = event.clientY - rect.top;

	const world = convertScreenToWorldCoordinates(screenX, screenY, canvas, camera);

	return hitTest(world.x, world.y);
}

export type Camera = {
	x: number;
	y: number;
	zoom: number;
};

type HitTarget =
	| {
			type: 'signal';
			id: string;
			shape: {
				type: 'circle';
				x: number;
				y: number;
				radius: number;
			};
	  }
	| {
			type: 'track';
			id: string;
			shape: {
				type: 'line';
				x1: number;
				y1: number;
				x2: number;
				y2: number;
				tolerance: number;
			};
	  };
