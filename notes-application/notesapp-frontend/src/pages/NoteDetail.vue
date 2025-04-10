<template>
  <div class="max-w-3xl mx-auto mt-8 space-y-6">
    <el-card shadow="always">
      <template #header>
        <div class="text-lg font-bold">{{ selectedNote?.title }}</div>
      </template>

      <p class="whitespace-pre-wrap text-gray-700">{{ selectedNote?.content }}</p>
    </el-card>

    <el-button type="primary" @click="goBack">Back</el-button>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'
import { useNoteStore } from '../store/note'
import { storeToRefs } from 'pinia'
import { getNoteById } from '../utils/note'
import type { Note } from '../types/note'

const router = useRouter()
const note = ref<Note | null>(null)

const noteStore = useNoteStore()
const { selectedNote, isLoading } = storeToRefs(noteStore)

const fetchNote = async () => {

  const id = Number(router.params.id)
  await noteStore.fetchNoteById(id)

  if (!selectedNote.value) {
    router.push('/')
  }
}

onMounted(() => {
  fetchNote()
})

const goBack = () => {
  router.push('/')
}
</script>