// types/comment.ts

export interface AuthorDto {
  id: string;
  name: string;
  profilePictureUrl: string | null;
}

export interface CommentResponseDto {
  id: string;
  text: string;
  createdAt: string; 
  author: AuthorDto;
  upvotes: number;
  downvotes: number;
  currentUserVote: boolean | null;
  parentCommentId: string | null; // true = up, false = down, null = kein Vote
  replies: CommentResponseDto[];
}

export interface CreateCommentRequest {
  text: string;
  contentId: number;
  parentCommentId: string | null;
}

export interface CreateVoteRequest {
  isUpvote: boolean;
  commentId: string;
}