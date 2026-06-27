using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Contracts;
public record CreateLikeOnContent(
    string ContentType,
    string Slug
);