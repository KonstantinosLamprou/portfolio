using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Backend.Domain.Contracts;

// Listen-Ansicht 
public record ContentListResponse(
    int Id, 
    string Title,
    string Slug,
    DateTime DateOfCreation,
    string ImgSrc,
    string Description,
    int Views,
    int LikesCount,
    string[] Tags,
    int CommentsCount
);


// Für die Detail-Ansicht (wenn man den Blog anklickt)
public record ContentDetailResponse(
    int Id,
    string Title,
    string Slug,
    DateTime DateOfCreation,
    string ImgSrc,
    string Description,
    List<ContentBlockDto> Content,
    int Views,
    int LikesCount,
    int CommentsCount,
    string[] Tags,
    AuthorDto Author,
    // für: ob der aktuelle User noch liken darf
    int CurrentUserLikeCount
);

public record ContentBlockDto(
    string Id,
    string Type,
    JsonElement Data
);

// Admin Dashboard DTO 
public record CreateBlogRequest(
    string ContentType, 
    string Title,
    string Slug,
    string ImgSrc,
    string Description,
    string[] Tags,
    // Die Blöcke kommen aus deinem Vue-Editor 
    List<ContentBlockDto> Content
);

public record UpdateViewsContentResponse(
    int Id,
    int Views
);