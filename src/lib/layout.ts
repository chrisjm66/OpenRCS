import { invoke } from '@tauri-apps/api/core';
import type { Scenario, SimulationLayout } from '../bindings';

export async function getLayouts(): Promise<Scenario[]> {
	const layouts = await invoke<Scenario[]>('get_layouts');
	console.log(layouts);

	return layouts;
}
