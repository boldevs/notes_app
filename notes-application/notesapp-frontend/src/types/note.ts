export interface NotePayload {
  title: string;
  content: string;
}

export interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}


export interface PaginatedNoteResponse {
  items: Note[]
  totalItems: number
  page: number
  limit: number
}