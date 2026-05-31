
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