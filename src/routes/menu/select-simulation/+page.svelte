<script lang="ts">
	import { goto } from '$app/navigation';
	import type { SimulationLayout } from '../../../bindings';
	import { getLayouts } from '../../../lib/layout';

	let layouts = $state<SimulationLayout[] | undefined>(undefined);
	let selectedSimulation = $state<SimulationLayout | undefined>();

	async function loadLayouts() {
		layouts = await getLayouts();
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
			{#if layouts != undefined}
				{#each layouts as layout}
					<button
						onclick={() => {
							selectedSimulation = layout;
						}}
						class="w-full rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
					>
						{layout.name}
					</button>
				{/each}
			{/if}
		</div>
		<div class="relative h-full w-full px-5">
			{#if selectedSimulation}
				<h1 class="text-2xl">{selectedSimulation.name}</h1>
				<h2 class="text-xl">{selectedSimulation.description}</h2>
				<button
					class="absolute bottom-5 w-max rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
					>Start Simulation</button
				>
			{/if}
		</div>
	</div>
</div>
