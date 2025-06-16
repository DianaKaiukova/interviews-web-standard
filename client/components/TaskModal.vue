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
              {{ formData.id ? 'Edit Task' : 'Create Task' }}
            </h3>
            
            <!-- Form for creating or editing a task -->
            <div class="mb-4">
              <label for="title" class="block text-sm font-medium text-gray-700">Title</label>
              <input 
                type="text" 
                id="title" 
                v-model="formData.title"
                required
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
            </div>
            
            <!-- Description field for the task -->
            <div class="mb-4">
              <label for="description" class="block text-sm font-medium text-gray-700">Description</label>
              <textarea 
                id="description" 
                v-model="formData.description"
                rows="3"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              ></textarea>
            </div>
          </div>
          
          <!-- Modal Footer with Save and Cancel Buttons -->
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button 
              type="button" 
              @click="save"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-500 text-base font-medium text-white hover:bg-blue-600 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
            >
            <!-- Save Button -->
              {{ formData.id ? '' : '' }}
              Save
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

<!-- TaskModal.vue -->
<script setup lang="ts">
import { ref, watch } from 'vue'

'<!-- This is a Vue 3 component for a modal dialog to create or edit tasks. -->'
const props = defineProps({
  show: Boolean,
  task: Object
})

'<!-- Define the type for the task prop -->'
const emit = defineEmits(['update:show', 'save'])

'<!-- Reactive form data for the task being created or edited -->'
const formData = ref({
  id: null as number | null,
  title: '',
  description: ''
})

'<! Watch for changes in the task prop and update formData accordingly -->'
watch(() => props.task, (newTask) => {
  if (newTask) {
    formData.value = {
      id: newTask.id ?? null,
      title: newTask.title ?? '',
      description: newTask.description ?? ''
    }
  } else {
    formData.value = { id: null, title: '', description: '' }
  }
}, { immediate: true })

'<!-- Function to save the task and emit the save event -->'
const save = () => {
  emit('save', { ...formData.value })
  emit('update:show', false)
}
</script>

<!-- TaskModal.vue -->
<style scoped>
.modal-enter-active, .modal-leave-active {
  transition: opacity 0.3s;
}
.modal-enter-from, .modal-leave-to {
  opacity: 0;
}
</style>