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
			class="bg-primary text-primary-foreground hover:bg-accent hover:text-accent-foreground rounded px-4 py-2 font-medium transition-colors"
			onclick={() => goto('/menu/select-simulation')}
		>
			Start a Simulation
		</button>
		<button
			class="bg-primary text-primary-foreground hover:bg-accent hover:text-accent-foreground rounded px-4 py-2 font-medium transition-colors"
		>
			Load a Simulation
		</button>
		<button
			class="bg-primary text-primary-foreground hover:bg-accent hover:text-accent-foreground rounded px-4 py-2 font-medium transition-colors"
		>
			Scenario Editor
		</button>
		<button
			class="bg-primary text-primary-foreground hover:bg-accent hover:text-accent-foreground rounded px-4 py-2 font-medium transition-colors"
		>
			Settings
		</button>

		<button
			class="bg-primary text-primary-foreground hover:bg-accent hover:text-accent-foreground rounded px-4 py-2 font-medium transition-colors"
			onclick={() => exit()}
		>
			Exit
		</button>
	</div>
