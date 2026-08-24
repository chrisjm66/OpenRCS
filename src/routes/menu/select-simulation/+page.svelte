<script lang="ts">
	import { goto } from '$app/navigation';
	import { initializeSimulation } from '$lib/simulation.svelte';
	import type { Scenario } from '../../../bindings';
	import { getLayouts } from '../../../lib/layout';

	let scenarios = $state<Scenario[] | undefined>(undefined);
	let selectedScenario = $state<Scenario | undefined>();

	async function loadLayouts() {
		scenarios = await getLayouts();
	}

	loadLayouts();
</script>

<div class="h-full w-full">
	<h1 class="text-2xl font-bold text-white">Select a Simulation</h1>
	<button
		class="text-muted-foreground transition-colors hover:text-primary-foreground"
		onclick={() => goto('/menu')}
	>
		Back to menu
	</button>

	<div class="mt-10 flex h-full w-full flex-row">
		<div class="h-full w-50">
			{#if scenarios != undefined}
				{#each scenarios as scenario}
					<button
						onclick={() => {
							selectedScenario = scenario;
						}}
						class="w-full rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
					>
						{scenario.name}
					</button>
				{/each}
			{/if}
		</div>
		<div class="relative h-full w-full px-5">
			{#if selectedScenario}
				<h1 class="text-2xl">{selectedScenario.name}</h1>
				<p class="text-lg">{selectedScenario.description}</p>
				<button
					class="absolute bottom-5 w-max rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
					onclick={() => initializeSimulation(selectedScenario)}
					>Start Simulation</button
				>
			{/if}
		</div>
	</div>
</div>
