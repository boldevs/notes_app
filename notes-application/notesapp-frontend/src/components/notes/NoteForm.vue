<template>
  <el-dialog :model-value="modelValue" @update:model-value="(val: boolean) => emit('update:modelValue', val)"
    :title="dialogTitle" width="90%" class="w-full max-w-[700px]">
    <el-form ref="noteForm" :model="formData" :rules="rules" label-position="top" class="space-y-4">
      <el-form-item label="Title" prop="title">
        <el-input v-model="formData.title" placeholder="Enter note title" />
      </el-form-item>

      <el-form-item label="Content" prop="content">
        <el-input type="textarea" v-model="formData.content" :rows="5" placeholder="Write your note content..." />
      </el-form-item>
    </el-form>

    <template #footer>
      <div class="flex justify-end gap-2 flex-wrap">
        <el-button @click="cancel">Cancel</el-button>
        <el-button type="primary" :loading="isSubmitting" @click="submit">Save</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, watch, reactive, computed } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'

const props = defineProps<{
  modelValue: boolean
  initialData?: Partial<{ title: string; content: string }>
  dialogTitle?: string
}>()

const emit = defineEmits(['update:modelValue', 'submit'])

const isSubmitting = ref(false)
const noteForm = ref<FormInstance>()

const formData = reactive({
  title: '',
  content: ''
})

// Responsive dialog width based on screen
const dialogWidth = computed(() => {
  if (window.innerWidth <= 480) return '90%'
  if (window.innerWidth <= 768) return '95%'
  return '500px'
})

const rules: FormRules = {
  title: [{ required: true, message: 'Title is required', trigger: 'blur' }]
}

watch(
  () => props.initialData,
  (val) => {
    formData.title = val?.title || ''
    formData.content = val?.content || ''
  },
  { immediate: true }
)

const cancel = () => {
  emit('update:modelValue', false)
}

const submit = () => {
  if (!noteForm.value) return

  noteForm.value.validate(async (valid) => {
    if (!valid) return
    isSubmitting.value = true
    await emit('submit', { ...formData })
    isSubmitting.value = false
    emit('update:modelValue', false)
  })
}
</script>
