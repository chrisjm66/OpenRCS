<script lang="ts">
	const { children } = $props();
</script>

<div class="h-screen w-screen p-5">
	{@render children()}
</div>
