<script lang='ts'>
	import SimulationCanvas from "$lib/components/SimulationCanvas.svelte";
	import SimulationTopBar from "$lib/components/SimulationTopBar.svelte";
	import { getActiveScenario } from "$lib/simulation.svelte";
    const activeScenario = $state(getActiveScenario())

</script>

<div class="flex flex-col overflow-y-hidden overflow-x-scroll  h-screen w-screen">
    {#if activeScenario}

        <SimulationTopBar/>

        <div class='grow  h-full w-full'>
            <SimulationCanvas diagram={activeScenario.diagram}/>
        </div>
        
    {:else}
        <h1>Error: Simulation not loaded.</h1>
    {/if}
</div>