using MongoDB.Bson.Serialization.Attributes;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
    [BsonCollection("User")]
    public class User : IDocument
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}

