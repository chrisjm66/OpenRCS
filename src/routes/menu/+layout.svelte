<script lang="ts">
	const { children } = $props();
</script>

<div class="p-5 h-screen w-screen">
	{@render children()}
</div>
