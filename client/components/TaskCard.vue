'<!-- TaskCard.vue -->'

<template>
  <div class="border rounded-lg p-4 mb-4 shadow-sm hover:shadow-md transition-shadow">
    <div class="flex justify-between items-start">
      <div>
        <h3 class="font-bold text-lg">{{ task.title }}</h3>
        <p v-if="task.description" class="text-gray-600 mt-2">{{ task.description }}</p>
      </div>
      '<!-- Action buttons for editing and deleting the task -->'
      <div class="flex space-x-2">
        <button @click.stop="emit('edit', task)" class="text-blue-500 hover:text-blue-700">
          <PencilIcon class="w-5 h-5" />
        </button>
        <button @click.stop="emit('delete', task.id)" class="text-red-500 hover:text-red-700">
          <TrashIcon class="w-5 h-5" />
        </button>
      </div>
    </div>
    '<!-- Display task tags and a button to assign new tags -->'
    <div class="mt-3 flex flex-wrap gap-2">
      <span 
        v-for="tag in task.tags" 
        :key="tag.id"
        class="px-2 py-1 bg-blue-100 text-blue-800 rounded-full text-xs"
      >
      '<!-- Display each tag with a small badge style -->'
        {{ tag.name }}
      </span>
      <button 
        @click.stop="emit('assign-tags', task)" 
        class="text-xs text-gray-500 hover:text-gray-700 flex items-center"
      >
      '<!-- Button to assign new tags to the task -->'
        <PlusIcon class="w-4 h-4 mr-1" /> Add Tags
      </button>
    </div>
  </div>
</template>

'<!-- TaskCard.vue -->'
<script setup lang="ts">
import { PencilIcon, TrashIcon, PlusIcon } from '@heroicons/vue/24/outline'

'<!-- TaskCard.vue -->'
const props = defineProps({
  task: {
    type: Object as () => any,
    required: true
  }
})

'<!-- Define the emits for editing, deleting, and assigning tags to the task -->'
const emit = defineEmits(['edit', 'delete', 'assign-tags'])
</script>