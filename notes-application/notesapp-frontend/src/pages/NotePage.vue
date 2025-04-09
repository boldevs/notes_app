<template>
  <div class="space-y-6">

    <!-- Header -->
    <div class="flex items-center justify-between flex-wrap gap-4">
      <div class="md:w-1/2">
        <el-input v-model="searchQuery" placeholder="Search notes..." :prefix-icon="Search"
          class="h-8 md:h-12 rounded-lg" />
      </div>

      <div class="flex items-center gap-2">
        <!-- Sort Dropdowns -->
        <el-select v-model="sortBy" placeholder="Sort By" class="w-36" size="small">
          <el-option label="Title" value="Title" />
          <el-option label="Created Date" value="CreatedAt" />
          <el-option label="Updated Date" value="UpdatedAt" />
        </el-select>

        <el-select v-model="sortDesc" placeholder="Order" class="w-28" size="small">
          <el-option label="Descending" :value="true" />
          <el-option label="Ascending" :value="false" />
        </el-select>

        <!-- Add Button -->
        <el-tooltip content="Add Note" placement="bottom">
          <el-button class="inline-flex md:hidden" type="primary" :icon="Plus" size="small" circle
            @click="createNotes()" />
        </el-tooltip>
      </div>
    </div>


    <!-- Note Cards List -->
    <div>
      <div class="h-[calc(100vh-240px)] overflow-y-auto px-2">
        <template v-if="filteredNotes.length">
          <NoteCard v-for="note in filteredNotes" :key="note.id" :title="note.title" :preview="note.content"
            :created-at="note.createdAt" :updated-at="note.updatedAt" @edit="() => editNote(note)"
            @delete="() => deleteNote(note.id)" @view="() => viewNoteDetail(note)" />
        </template>

        <div v-else class="text-center text-gray-400 py-12">
          <el-icon size="48" class="mb-2">
            <document-remove />
          </el-icon>
          <p class="text-lg">No notes found</p>
        </div>
      </div>


      <el-pagination class="pt-4" background layout="prev, pager, next" :page-size="pagination.limit"
        :total="pagination.total" :current-page="pagination.page" @current-change="handlePageChange" />

    </div>
    <NoteForm v-model="showDialog" :initialData="newNote"
      :dialog-title="dialogMode === 'edit' ? 'Edit Note' : 'Create New Note'" @submit="saveNote" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch, type Ref } from 'vue'
import { Plus, Search } from '@element-plus/icons-vue'
import { useRouter } from 'vue-router'
import { createNote, getNotes, updateNote, deleteNotebyid } from '../utils/note'
import { } from '../utils/note'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { Note } from '../types/note'
import NoteCard from '../components/notes/NoteCard.vue'
import NoteForm from '../components/notes/NoteForm.vue'


onMounted(() => {
  loadNotes()
})

const router = useRouter()
const searchQuery = ref('')
const showDialog = ref(false)
const dialogMode = ref<'create' | 'edit'>('create')
const notes = ref<Note[]>([])
const newNote = ref<Partial<Note>>({
  id: undefined,
  title: '',
  content: ''
})

const sortBy = ref('UpdatedAt')
const sortDesc = ref(true)

const pagination = ref({
  page: 1,
  limit: 10,
  total: 0,
}) as Ref<{ page: number; limit: number; total: number }>;

const handlePageChange = (newPage: number) => {
  pagination.value.page = newPage;
  loadNotes();
};

watch(searchQuery, () => {
  pagination.value.page = 1; // reset to page 1
  loadNotes();
});

watch([sortBy, sortDesc], () => {
  loadNotes()
})

const filteredNotes = computed(() =>
  notes.value.filter(n =>
    n.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
    n.content.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
)

const loadNotes = async () => {
  try {
    const res = await getNotes({
      search: searchQuery.value,
      page: pagination.value.page,
      limit: pagination.value.limit,
      sortBy: sortBy.value,
      sortDesc: sortDesc.value,
    });


    notes.value = res.items;
    pagination.value.total = res.totalItems;
  } catch (err) {
    console.error('Failed to load notes', err);
  }
};

const saveNote = async (note: { title: string; content: string }) => {
  if (dialogMode.value === 'edit' && newNote.value.id) {
    try {
      await updateNote(newNote.value.id, note)
      await loadNotes()
      ElMessage.success('Note updated successfully!')
    } catch (error) {
      console.error('Failed to update note', error)
      ElMessage.error('Failed to update note!')
    }
  } else {
    try {
      await createNote(note)
      await loadNotes()
      ElMessage.success('Note created successfully!')
    } catch (error) {
      console.error('Failed to create note', error)
      ElMessage.error('Failed to create note!')
    }
  }

  showDialog.value = false
}

const createNotes = () => {
  newNote.value = { title: '', content: '' }
  dialogMode.value = 'create'
  showDialog.value = true
}

const editNote = (note: Note) => {
  newNote.value = { ...note } // include id
  dialogMode.value = 'edit'
  showDialog.value = true
}

const deleteNote = async (id: number) => {
  try {

    await ElMessageBox.confirm(
      'Are you sure you want to delete this note?',
      'Delete Confirmation',
      {
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel',
        type: 'warning',
      }
    )

    await deleteNotebyid(id)
    await loadNotes()

    ElMessage.success('Note deleted successfully!')
  } catch (err: any) {

    if (err === 'cancel' || err === 'close') return
    console.error('Failed to delete note', err)
    ElMessage.error('Failed to delete note!')

  }
}

const viewNoteDetail = (note: any) => {
  router.push(`/notes/${note.id}`)
}


</script>