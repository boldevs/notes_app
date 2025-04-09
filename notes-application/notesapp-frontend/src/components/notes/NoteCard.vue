<template>
  <el-card shadow="hover" class="mb-4" :body-style="{ padding: '16px' }">
    <div class="flex justify-between items-start">
      <!-- Note Content -->
      <div>
        <h3 class="text-lg font-semibold">{{ title }}</h3>
        <p class="text-sm text-gray-500">{{ preview }}</p>
        <p class="text-xs text-gray-400 mt-1">
          <span v-if="createdAt === updatedAt">
            Created: {{ formatDate(createdAt) }}
          </span>
          <span v-else>
            Updated: {{ formatDate(updatedAt) }}
          </span>
        </p>

      </div>

      <!-- Icon Dropdown -->
      <el-dropdown trigger="click" @command="handleAction">
        <el-icon class="cursor-pointer text-gray-600 hover:text-gray-800">
          <More />
        </el-icon>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item command="view">View Detail</el-dropdown-item>
            <el-dropdown-item command="edit">Edit Note</el-dropdown-item>
            <el-dropdown-item command="delete" divided>Delete</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </div>
  </el-card>
</template>

<script setup lang="ts">
import { More } from '@element-plus/icons-vue'
import { defineEmits, defineProps } from 'vue'

const props = defineProps<{
  title: string
  preview: string
  createdAt: string
  updatedAt: string
}>()

const emit = defineEmits(['edit', 'delete', 'view'])

const handleAction = (action: string) => {
  if (action === 'edit') emit('edit')
  if (action === 'delete') emit('delete')
  if (action === 'view') emit('view')
}

const formatDate = (iso: string) => {
  const date = new Date(iso + 'Z')
  return date.toLocaleString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: true,
  })
}


</script>
