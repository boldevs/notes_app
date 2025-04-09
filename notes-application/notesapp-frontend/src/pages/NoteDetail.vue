<template>
  <div class="max-w-3xl mx-auto mt-8 space-y-6">
    <el-card shadow="always">
      <template #header>
        <div class="text-lg font-bold">{{ note?.title }}</div>
      </template>

      <p class="whitespace-pre-wrap text-gray-700">{{ note?.content }}</p>
    </el-card>

    <el-button type="primary" @click="goBack">Back</el-button>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'
import { getNoteById } from '../utils/note'
import type { Note } from '../types/note'

const route = useRoute()
const router = useRouter()
const note = ref<Note | null>(null)

const fetchNote = async () => {
  const id = Number(route.params.id)
  try {
    const result = await getNoteById(id)
    note.value = result
  } catch (err) {
    console.error('Failed to fetch note', err)
    router.push('/') // fallback to home if note not found
  }
}

onMounted(() => {
  fetchNote()
})
const goBack = () => {
  router.push('/')
}
</script>