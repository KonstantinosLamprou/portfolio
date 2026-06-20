
export interface CreateBlogRequest {
  contentType: string;
  title: string;
  slug: string;
  imgSrc: string;
  description: string;
  content: ContentBlockDto[];
}

export interface AuthorDto {
  id: string | number;
  name: string;
  profilePictureUrl?: string;
  role?: string;
}

export interface ContentBlock {
  id: string;
  type: string;
  data: any; 
}

export interface BlogApiResponse {
  id: number;
  title: string;
  slug: string;
  dateOfCreation: string; // Von API als String
  imgSrc: string;
  description: string;
  content: ContentBlock[];
  views: number;
  likesCount: number;
  commentsCount: number;
  currentUserLikeCount: number;
  author: AuthorDto;
}


export interface BlogLatestApiResponse {
  id: string;
  title: string;
  slug: string;
  dateOfCreation: string; 
  imgSrc: string;
  description: string;
  views?: number;
  likesCount?: number;
  commentsCount?: number;
}