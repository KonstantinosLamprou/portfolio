using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Backend.Domain.Contracts;

// Sollte ja Projekte oder Blogs liefern 
public record ContentResponseSummary(
    Guid Id,
    string Title,
    string Slug,
    DateTime DateOfCreation,
    string ImgSrc,
    string Description,

    List<ContentBlockDto> Content,

    // Metriken
    int Views,
    int LikesCount,
    int CommentsCount,

    AuthorDto Author

    );

public record ContentBlockDto(
    string Id,
    string Type,
    JsonElement Data
);


public record CreateBlogRequest(
    string Title,
    string Slug,
    string ImgSrc,
    string Description,
    // Die Blöcke kommen aus deinem Vue-Editor (z.B. Editor.js)
    List<ContentBlockDto> Content
);