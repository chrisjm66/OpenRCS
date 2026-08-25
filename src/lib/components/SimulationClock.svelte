<script lang="ts">
	import { getFormattedDate } from '$lib/date';

	let time = $state(getFormattedDate());

	setInterval(() => {
		time = getFormattedDate();
	}, 1000);
</script>

<div class="flex h-full min-w-25 items-center bg-gray-800 p-2">
	<p class="w-full text-center text-red-500">{time}</p>
</div>
