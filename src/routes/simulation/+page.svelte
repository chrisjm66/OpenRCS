<script lang="ts">
	import SimulationCanvas from '$lib/components/SimulationCanvas.svelte';
	import SimulationTopBar from '$lib/components/SimulationTopBar.svelte';
	import { getActiveScenario } from '$lib/simulation.svelte';
	const activeScenario = $state(getActiveScenario());
</script>

<div class="flex h-screen w-screen flex-col overflow-x-scroll overflow-y-hidden">
	{#if activeScenario}
		<SimulationTopBar />

		<SimulationCanvas diagram={activeScenario.diagram} />
	{:else}
		<h1>Error: Simulation not loaded.</h1>
	{/if}
</div>
