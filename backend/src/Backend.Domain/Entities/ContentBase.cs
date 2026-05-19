using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Backend.Domain.Entities
{
    public abstract class ContentBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;

        //TODO hier eine Relation aufbauen zu autor 
        public string Author { get; set; }
        public string ImgSrc { get; set; }
        public string Description { get; set; }

        // Hier ist die Magie: Eine stark typisierte Liste, aber dynamische Payload
        public List<ContentBlock> Content { get; set; } = new();

        public ICollection<Like> Likes { get; set; } = new List<Like>(); 
        public int Views { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class ContentBlock
    {
        public string Id { get; set; }
        public string Type { get; set; }

        // JsonElement erlaubt es dir, jedes beliebige JSON-Objekt hier zu speichern,
        // ohne dass C# wissen muss, ob es "level" und "text" oder "url" und "caption" enthält.
        public JsonElement Data { get; set; }
    }

    public class Blog : ContentBase { }
    public class Project : ContentBase { }

}



