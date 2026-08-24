<script lang="ts">
	import { goto } from '$app/navigation';
	import { redirect } from '@sveltejs/kit';
	import { exit } from '@tauri-apps/plugin-process';
</script>

<div>
	<h1 class="text-2xl font-bold text-white">OpenRCS</h1>
	<h2 class="text-xl text-white">Open Source Rail Control Simulator</h2>
</div>

<div class="mt-20 flex max-w-xs flex-col gap-3">
	<button
		class="rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
		onclick={() => goto('/menu/select-simulation')}
	>
		Start a Simulation
	</button>
	<button
		class="rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
	>
		Load a Simulation
	</button>
	<button
		class="rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
	>
		Scenario Editor
	</button>
	<button
		class="rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
	>
		Settings
	</button>

	<button
		class="rounded bg-primary px-4 py-2 font-medium text-primary-foreground transition-colors hover:bg-accent hover:text-accent-foreground"
		onclick={() => exit(0)}
	>
		Exit
	</button>
</div>
