<!-- filepath: d:\Applications\Infeon\Additional_task\final\interviews-web-standard\client\components\TagModal.vue -->
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
              {{ formData.id ? 'Edit Tag' : 'Create Tag' }}
            </h3>
            <!-- Form for creating or editing a tag -->
            <div class="mb-4">
              <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
              <input 
                type="text" 
                id="name" 
                v-model="formData.name"
                required
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
            </div>
            <div class="mb-4">
              <label for="color" class="block text-sm font-medium text-gray-700">Color</label>
              <input
                type="color"
                id="color"
                v-model="formData.color"
                class="mt-1 block w-16 h-10 p-0 border-0 bg-transparent"
              >
            </div>
          </div>
          <!-- Modal Footer with Save and Cancel Buttons -->
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button 
              type="button" 
              @click="save"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-green-500 text-base font-medium text-white hover:bg-green-600 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
            >
              {{ formData.id ? 'Update' : 'Create' }}
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

const props = defineProps({
  show: Boolean,
  tag: Object
})

const emit = defineEmits(['update:show', 'save'])

const formData = ref({
  id: null as number | null,
  name: '',
  color: '#3b82f6'
})

watch(() => props.tag, (newTag) => {
  if (newTag) {
    formData.value = { 
      id: newTag.id ?? null, 
      name: newTag.name ?? '', 
      color: newTag.color ?? '#3b82f6' 
    }
  } else {
    formData.value = { id: null, name: '', color: '#3b82f6' }
  }
}, { immediate: true })

const save = () => {
  emit('save', { ...formData.value })
  emit('update:show', false)
}
</script>