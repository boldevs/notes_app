import API from "./api";
import type { Note, NotePayload, PaginatedNoteResponse } from '../types/note';

export const createNote = async (payload: NotePayload) => {
  const response = await API.post('/notes', payload);
  return response.data;
};

export interface NoteQueryParams {
  search?: string;
  sortBy?: string;
  sortDesc?: boolean;
  page?: number;
  limit?: number;
}

export const getNotes = async (params: NoteQueryParams = {}): Promise<PaginatedNoteResponse> => {
  const {
    search = '',
    sortBy = 'UpdatedAt',
    sortDesc = true,
    page = 1,
    limit = 10,
  } = params;

  const response = await API.get('/notes', {
    params: {
      Search: search,
      SortBy: sortBy,
      SortDesc: sortDesc,
      Page: page,
      Limit: limit,
    },
  });

  return response.data;
};

export const updateNote = async (id: number, payload: { title: string; content: string }) => {
  await API.put(`/notes/${id}`, payload)
}

export const deleteNotebyid = async (id: number): Promise<void> => {
  await API.delete(`/notes/${id}`);
}

export const getNoteById = async (id: number): Promise<Note> => {
  const response = await API.get(`/notes/${id}`)
  return response.data
}