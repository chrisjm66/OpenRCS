import { invoke } from '@tauri-apps/api/core';
import type { SimulationLayout } from '../bindings';

export async function getLayouts(): Promise<SimulationLayout[]> {
	const layouts = await invoke<SimulationLayout[]>('get_layouts');
	console.log(layouts);

	return layouts;
}
