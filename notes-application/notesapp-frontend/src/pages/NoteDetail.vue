<template>
  <div class="max-w-3xl mx-auto mt-8 space-y-6">
    <div class="h-[calc(100vh-260px)] overflow-y-auto">
      <el-card shadow="always">
      <template #header>
        <div class="text-lg font-bold">{{ selectedNote?.title }}</div>
      </template>

      <p class="whitespace-pre-wrap text-gray-700">{{ selectedNote?.content }}</p>
    </el-card>

    </div>
    
    <el-button type="primary" @click="goBack">Back</el-button>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { onMounted } from 'vue'
import { useNoteStore } from '../store/note'
import { storeToRefs } from 'pinia'

const rout = useRoute()
const router = useRouter()

const noteStore = useNoteStore()
const { selectedNote, isLoading } = storeToRefs(noteStore)

const fetchNote = async () => {

  const id = Number(rout.params.id)
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