//replies einfügen sowie den comment selbst
type Comment = {
  id: number,
  content: string,
  createdAt: string,
  author: string,
  parentId: number | null,
  postId: number
};

export const mockComments: Comment[] = [
  {
    id: 1,
    content: 'Das ist ein **Top-Level**-Kommentar mit Markdown.',
    createdAt: '2026-05-11T12:00:00Z',
    author: 'Alice',
    parentId: null,
    postId: 42
  },
  {
    id: 2,
    content: 'Ich antworte auf Alice. `code` funktioniert auch.',
    createdAt: '2026-05-11T12:05:00Z',
    author: 'Bob',
    parentId: 1,
    postId: 42
  },
  {
    id: 3,
    content: 'Noch eine Antwort auf Alice.',
    createdAt: '2026-05-11T12:10:00Z',
    author: 'Charlie',
    parentId: 1,
    postId: 42
  },
  {
    id: 4,
    content: 'Ein eigenständiger Kommentar ohne Bezug.',
    createdAt: '2026-05-11T12:15:00Z',
    author: 'Diana',
    parentId: null,
    postId: 42
  },
  {
    id: 5,
    content: 'Antwort auf Bobs Kommentar (2 Ebenen tief).',
    createdAt: '2026-05-11T12:20:00Z',
    author: 'Alice',
    parentId: 2,
    postId: 42
  }
]
