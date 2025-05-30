<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold">Task Manager</h1>
      <button 
        @click="openCreateTask" 
        class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg flex items-center"
      >
        <PlusIcon class="w-5 h-5 mr-2" /> Add Task
      </button>
    </div>

    <div v-if="loading" class="text-center py-8">
      <p>Loading tasks...</p>
    </div>
    
    <div v-else-if="error" class="text-center py-8 text-red-500">
      <p>{{ error }}</p>
      <button @click="fetchTasks" class="mt-2 text-blue-500 hover:text-blue-700">
        Retry
      </button>
    </div>
    
    <div v-else>
      <div v-if="tasks.length === 0" class="text-center py-8">
        <p>No tasks found. Create your first task!</p>
      </div>
      
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <TaskCard 
          v-for="task in tasks" 
          :key="task.id" 
          :task="task"
          @edit="openEditTask"
          @delete="deleteTask"
          @assign-tags="openTagAssignment"
        />
      </div>
    </div>

    <TaskModal 
      v-if="showTaskModal"
      :show="showTaskModal"
      :task="editingTask"
      @update:show="showTaskModal = $event"
      @save="handleTaskSave"
    />

    <TagAssignmentModal 
      v-if="showTagAssignmentModal"
      :show="showTagAssignmentModal"
      :task="selectedTask"
      @update:show="showTagAssignmentModal = $event"
      @save="assignTags"
    />
  </div>
</template>

<script setup lang="ts">
import { PlusIcon } from '@heroicons/vue/24/outline'

const { $api } = useNuxtApp()
const config = useRuntimeConfig()
const tasks = ref<any[]>([])
const loading = ref(true)
const error = ref<string | null>(null)

// Task management
const showTaskModal = ref(false)
const editingTask = ref<any>(null)
const selectedTask = ref<any>(null)
const showTagAssignmentModal = ref(false)

// Fetch tasks
const fetchTasks = async () => {
  loading.value = true
  error.value = null
  try {
    console.log('Fetching tasks from:', `${config.public.apiBase}/api/tasks`)
    const start = Date.now()
    const data = await $api.get('/tasks')
    console.log(`Request took ${Date.now() - start}ms`)
    tasks.value = data
  } catch (err: any) {
    console.error('API error details:', {
      message: err.message,
      stack: err.stack,
      response: err.response?._data
    })
    error.value = `Failed to load tasks: ${err.message || 'Network error'}`
  } finally {
    loading.value = false
  }
}

// Handle task operations
const openCreateTask = () => {
  editingTask.value = null
  showTaskModal.value = true
}

const openEditTask = (task: any) => {
  editingTask.value = task
  showTaskModal.value = true
}

const openTagAssignment = (task: any) => {
  selectedTask.value = task
  showTagAssignmentModal.value = true
}

const handleTaskSave = async (taskData: any) => {
  try {
    console.log('Submitting to /tasks:', taskData)

    if (taskData.id) {
      await $api.put(`/tasks/${taskData.id}`, taskData)
    } else {
      await $api.post('/tasks', taskData)
    }

    await fetchTasks()
    showTaskModal.value = false
  } catch (err: any) {
    console.error('Failed to save task:', err)
    if (err?.response?._data) {
      console.error('API Error:', err.response._data)
    }
    alert('Failed to save task. Please try again.' + (err ? `\n${err}` : ''))
  }
}

const deleteTask = async (id: number) => {
  if (confirm('Are you sure you want to delete this task?')) {
    try {
      await $api.delete(`/tasks/${id}`)
      await fetchTasks()
    } catch (err: any) {
      console.error('Failed to delete task:', err)
      alert('Failed to delete task. Please try again.')
    }
  }
}

const assignTags = async (tagIds: number[]) => {
  try {
    await $api.post(`/tasks/${selectedTask.value.id}/tags`, tagIds)
    await fetchTasks()
    showTagAssignmentModal.value = false
  } catch (err: any) {
    console.error('Failed to assign tags:', err)
    alert('Failed to assign tags. Please try again.')
  }
}

onMounted(() => {
  fetchTasks()
})
</script>