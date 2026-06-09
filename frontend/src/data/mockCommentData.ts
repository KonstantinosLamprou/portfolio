// mockData.ts
import type {  CommentResponseDto } from '@/types/comment';

export const mockComments: CommentResponseDto[] = [
  {
    id: "c1",
    text: "Nice portfolio bro, can I get inspiration from your site ?",
    createdAt: "2026-05-04T10:00:00Z",
    author: {
      id: "u1",
      name: "Duc Tran",
      profilePictureUrl: "https://i.pravatar.cc/150?u=u1"
    },
    parentCommentId: null,
    upvotes: 0,
    downvotes: 0,
    currentUserVote: null,
    replies: [
      {
        id: "r1",
        text: "Sure thing! The source code will be on my GitHub soon.",
        createdAt: "2026-05-04T12:00:00Z",
        author: {
          id: "u_nelson",
          name: "Nelson Lai",
          profilePictureUrl: "https://i.pravatar.cc/150?u=nelson"
        },
        parentCommentId: "c1",
        upvotes: 3,
        downvotes: 0,
        currentUserVote: true,
      }
    ]
  },
  {
    id: "c2",
    text: "I love your work Nelson Lai",
    createdAt: "2026-03-15T08:30:00Z",
    author: {
      id: "u2",
      name: "Olanrewaju Toyyib",
      profilePictureUrl: "https://i.pravatar.cc/150?u=u2"
    },
    parentCommentId: null,
    upvotes: 2,
    downvotes: 0,
    currentUserVote: null,
    replies: []
  }
];