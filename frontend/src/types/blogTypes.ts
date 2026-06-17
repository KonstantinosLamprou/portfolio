
export interface ContentBlockDto {
  id: string;
  type: string;
  data: Record<string, any>; // Entspricht JsonElement in C#
}

export interface CreateBlogRequest {
  contentType: string;
  title: string;
  slug: string;
  imgSrc: string;
  description: string;
  content: ContentBlockDto[];
}

export interface BlogApiResponse {
  id: string;
  contenttype?: 'blog' | 'project';
  title: string;
  author: string;
  slug: string;
  dateOfCreation: string; 
  description: string;
  content: object;
  imgSrc: string;
  likesCount?: number; // likes ist keine Num
  views?: number;
  commentsCount?: number; //deprecated? 
  comment?: string; // deprecated?
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