'<!-- TagAssignmentModal.vue -->'

<template>
  <Transition name="modal">
    <div v-if="show" class="fixed inset-0 z-50 overflow-y-auto">
      <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center">
        <!-- Backdrop -->
        <div class="fixed inset-0 bg-black opacity-50 z-40" @click="emit('update:show', false)"></div>
        <!-- Modal Panel -->
        <div class="z-50 relative inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full" @click.stop>
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4">
              Assign Tags to: {{ task?.title }}
            </h3>

            '<!-- Display current tags if any -->'
            <div class="flex flex-wrap gap-2 mb-4">
              <span 
                v-for="tag in task?.tags" 
                :key="tag.id"
                class="px-2 py-1 bg-blue-100 text-blue-800 rounded-full text-xs"
              >
                {{ tag.name }}
              </span>
            </div>

            '<!-- Available Tags Section -->'
            <div class="mt-4">
              <label class="block text-sm font-medium text-gray-700 mb-2">Available Tags</label>
              <div class="space-y-2 max-h-60 overflow-y-auto">
                <div 
                  v-for="tag in availableTags" 
                  :key="tag.id"
                  class="flex items-center px-4 py-2 border rounded hover:bg-gray-50 cursor-pointer"
                  @click="toggleTagSelection(tag.id)"
                >

                  '<!-- Checkbox for selecting tags -->'
                  <input 
                    type="checkbox" 
                    :checked="selectedTags.includes(tag.id)"
                    class="h-4 w-4 text-blue-600 rounded focus:ring-blue-500"
                  >
                  <span class="ml-3">{{ tag.name }}</span>
                </div>
              </div>
            </div>
          </div>
          
          '<!-- Modal Footer with Save and Cancel Buttons -->'
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button 
              type="button" 
              @click="save"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-500 text-base font-medium text-white hover:bg-blue-600 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
            >

            '<!-- Save Button -->'
              Save Tags
            </button>
            <button 
              type="button" 
              @click="emit('update:show', false)"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
            >
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'

'<!-- Importing useNuxtApp to access the API client -->'
const { $api } = useNuxtApp()

const props = defineProps({
  show: Boolean,
  task: Object
})

const emit = defineEmits(['update:show', 'save'])

'<!-- Reactive variables for available tags and selected tags -->'
const availableTags = ref<any[]>([])
const selectedTags = ref<number[]>([])

'<!-- Watch for changes in the task prop to fetch tags and set selected tags accordingly -->'
watch(() => props.task, async (newTask) => {
  if (newTask) {
    try {
      // Fetch all tags
      const tags = await $api.get('/tags')
      availableTags.value = tags
      
      // Set currently assigned tags
      selectedTags.value = newTask.tags?.map((t: any) => t.id) || []
    } catch (err) {
      console.error('Failed to fetch tags:', err)
    }
  } else {
    selectedTags.value = []
  }
}, { immediate: true })

'<!-- Function to toggle tag selection -->'
const toggleTagSelection = (tagId: number) => {
  if (selectedTags.value.includes(tagId)) {
    selectedTags.value = selectedTags.value.filter(id => id !== tagId)
  } else {
    selectedTags.value = [...selectedTags.value, tagId]
  }
}

'<!-- Function to save selected tags and emit the event -->'
const save = () => {
  emit('save', selectedTags.value)
  emit('update:show', false)
}
</script>