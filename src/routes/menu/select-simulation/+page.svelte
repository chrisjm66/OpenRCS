<script lang='ts'>
	import { goto } from "$app/navigation";
	import { getLayouts } from "../../../lib/layout";

    const layouts = getLayouts()
</script>


	<h1 class="text-2xl font-bold text-white">Select a Simulation</h1>
    <button class="text-muted-foreground hover:text-primary-foreground transition-colors" onclick={() => goto('/menu')}>
        Back to menu
    </button>

    <div class='mt-10 h-full w-full'>
        <h1>yooo</h1>
    </div>