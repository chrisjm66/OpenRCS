<script lang='ts'>
	import { getFormattedDate } from "$lib/date";

    let time = $state(getFormattedDate())

    setInterval(() => {
        time =  getFormattedDate() 
    }, 1000)
</script>


<div class='bg-gray-800 flex items-center h-full p-2 min-w-25   '>

        <p class='text-red-500 w-full text-center'>{time}</p>
    </div>
