<script lang='ts'>
	import { getActiveScenario } from "$lib/simulation.svelte";
    const activeScenario = $state(getActiveScenario())

</script>

<div>
    <h1>sim here</h1>
    {#if activeScenario}
    <p>{activeScenario.name}</p>
    {:else}
        <p>no scenario</p>
    {/if}
</div>