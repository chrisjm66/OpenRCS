const MAX_ZOOM = 3;
const MIN_ZOOM = 0.1;

export type Camera = {
	x: number;
	y: number;
	zoom: number;
};

export function screenToWorld(
	canvas: HTMLCanvasElement,
	camera: Camera,
	screenX: number,
	screenY: number
) {
	const rect = canvas.getBoundingClientRect();

	return {
		x: (screenX - rect.width / 2) / camera.zoom + camera.x,
		y: (screenY - rect.height / 2) / camera.zoom + camera.y
	};
}

export function panCamera(camera: Camera, deltaX: number, deltaY: number) {
	camera.x -= deltaX / camera.zoom;
	camera.y -= deltaY / camera.zoom;
}

export function zoomCameraAt(
	canvas: HTMLCanvasElement,
	camera: Camera,
	screenX: number,
	screenY: number,
	factor: number
) {
	const before = screenToWorld(canvas, camera, screenX, screenY);

	camera.zoom = Math.max(MIN_ZOOM, Math.min(MAX_ZOOM, camera.zoom * factor));

	const after = screenToWorld(canvas, camera, screenX, screenY);

	camera.x += before.x - after.x;
	camera.y += before.y - after.y;
}
