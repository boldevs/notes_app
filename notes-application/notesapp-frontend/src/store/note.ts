import { defineStore } from "pinia";
import { ref } from "vue";
import type { Note, NotePayload, PaginatedNoteResponse } from "../types/note";
import {
  getNotes,
  createNote,
  updateNote,
  deleteNotebyid,
  getNoteById,
} from "../utils/note";

export const useNoteStore = defineStore("note", () => {
  const notes = ref<Note[]>([]);
  const totalItems = ref(0);
  const isLoading = ref(false);
  const selectedNote = ref<Note | null>(null);

  const fetchNotes = async (params: any) => {
    isLoading.value = true;
    try {
      const res: PaginatedNoteResponse = await getNotes(params);
      notes.value = res.items;
      totalItems.value = res.totalItems;
    } catch (err) {
      console.error("Failed to fetch notes", err);
    } finally {
      isLoading.value = false;
    }
  };

  const fetchNoteById = async (id: number) => {
    isLoading.value = true;
    try {
      const res = await getNoteById(id);
      selectedNote.value = res;
    } catch (err) {
      console.error(`Failed to fetch note ID ${id}`, err);
      selectedNote.value = null;
    } finally {
      isLoading.value = false;
    }
  };

  const addNote = async (payload: NotePayload) => {
    await createNote(payload);
  };

  const editNote = async (id: number, payload: NotePayload) => {
    await updateNote(id, payload);
  };

  const removeNote = async (id: number) => {
    await deleteNotebyid(id);
  };

  return {
    notes,
    totalItems,
    isLoading,
    selectedNote,
    fetchNotes,
    fetchNoteById,
    addNote,
    editNote,
    removeNote,
  }
});
