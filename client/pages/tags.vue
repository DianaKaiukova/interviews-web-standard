<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold">Tags Manager</h1>
      <button 
        @click="openCreateTag" 
        class="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg flex items-center"
      >
        <PlusIcon class="w-5 h-5 mr-2" /> Add Tag
      </button>
    </div>

    <div v-if="loading" class="text-center py-8">
      <p>Loading tags...</p>
    </div>
    
    <div v-else-if="error" class="text-center py-8 text-red-500">
      <p>{{ error }}</p>
      <button @click="fetchTags" class="mt-2 text-blue-500 hover:text-blue-700">
        Retry
      </button>
    </div>
    
    <div v-else>
      <div v-if="tags.length === 0" class="text-center py-8">
        <p>No tags found. Create your first tag!</p>
      </div>
      
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        <div 
          v-for="tag in tags" 
          :key="tag.id"
          class="border rounded-lg p-4 flex justify-between items-center"
        >
          <span
            class="font-medium flex items-center"
          >
            <span
              v-if="tag.color"
              :style="{ backgroundColor: tag.color }"
              class="w-4 h-4 rounded-full inline-block mr-2 border"
            ></span>
            {{ tag.name }}
          </span>
          <div class="flex space-x-2">
            <button @click="openEditTag(tag)" class="text-blue-500 hover:text-blue-700">
              <PencilIcon class="w-5 h-5" />
            </button>
            <button @click="deleteTag(tag.id)" class="text-red-500 hover:text-red-700">
              <TrashIcon class="w-5 h-5" />
            </button>
          </div>
        </div>
      </div>
    </div>

    <TagModal 
      v-if="showTagModal"
      :show="showTagModal"
      :tag="editingTag"
      @update:show="showTagModal = $event"
      @save="handleTagSave"
    />
  </div>
</template>

<script setup lang="ts">
import { PlusIcon, PencilIcon, TrashIcon } from '@heroicons/vue/24/outline'

const { $api } = useNuxtApp()
const tags = ref<any[]>([])
const loading = ref(true)
const error = ref<string | null>(null)

// Tag management
const showTagModal = ref(false)
const editingTag = ref<any>(null)

// Fetch tags
const fetchTags = async () => {
  loading.value = true
  error.value = null
  try {
    const data = await $api.get('/tags')
    tags.value = data
  } catch (err: any) {
    console.error('Failed to fetch tags:', err)
    error.value = `Failed to load tags: ${err.message || 'Unknown error'}`
  } finally {
    loading.value = false
  }
}

// Handle tag operations
const openCreateTag = () => {
  editingTag.value = null
  showTagModal.value = true
}

const openEditTag = (tag: any) => {
  editingTag.value = tag
  showTagModal.value = true
}

const handleTagSave = async (tagData: any) => {
  try {
    if (tagData.id) {
      await $api.put(`/tags/${tagData.id}`, tagData)
    } else {
      await $api.post('/tags', tagData)
    }
    await fetchTags()
    showTagModal.value = false
  } catch (err: any) {
    console.error('Failed to save tag:', err)
    alert('Failed to save tag. Please try again.')
  }
}

const deleteTag = async (id: number) => {
  if (confirm('Are you sure you want to delete this tag?')) {
    try {
      await $api.delete(`/tags/${id}`)
      await fetchTags()
    } catch (err: any) {
      console.error('Failed to delete tag:', err)
      alert('Failed to delete tag. Please try again.')
    }
  }
}

onMounted(() => {
  fetchTags()
})
</script>