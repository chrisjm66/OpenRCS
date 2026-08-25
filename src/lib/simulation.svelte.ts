import { invoke } from '@tauri-apps/api/core';
import type { Scenario, SimulationLayout } from '../bindings';
import { goto } from '$app/navigation';

let activeScenario = $state<Scenario>();
let availableScenarios = $state<Scenario[]>();

export function getAvailableScenarios() {
	return availableScenarios;
}

export function getActiveScenario() {
	return activeScenario;
}

export async function loadScenarios(): Promise<void> {
	const scenarios = await invoke<Scenario[]>('get_layouts');

	availableScenarios = scenarios;
}

export async function initializeSimulation(scenarioId: string | undefined) {
	if (!scenarioId) {
		console.error('Error: no scenarioid provided');
		return;
	}

	if (availableScenarios) {
		for (let scenario of availableScenarios) {
			if (scenario.id == scenarioId) {
				activeScenario = scenario;
				break;
			}
		}
	}
	goto('/simulation');
}
