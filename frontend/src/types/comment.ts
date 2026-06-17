// types/comment.ts

export interface AuthorDto {
  id: string;
  name: string;
  profilePictureUrl: string | null;
  UserRole: string;
}

// User Role ist ein Enum im Backend, wie handle ich das hier? 

export interface CommentResponseDto {
  id: string;
  text: string;
  createdAt: string; 
  author: AuthorDto;
  upvotes: number;
  downvotes: number;
  parentCommentId?: string;
  replies?: CommentResponseDto[];
}

export interface CommentReplyDto {
  id: string;
  text: string;
  createdAt: string; 
  author: AuthorDto;
  upvotes: number;
  downvotes: number;
  currentUserVote: boolean | null;
  parentCommentId: string; 
}

export interface CreateCommentRequest {
  Text: string;
  ContentType: string;
  ContentId: number;
  ParentCommentId?: string;
}


export interface CreateVoteRequest {
  isUpvote: boolean;
  commentId: string;
}