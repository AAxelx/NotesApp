using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
    [BsonCollection("TaskList")]
    public class TaskList : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        public List<string> Content { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        [BsonElement("OwnerId")]
        public string OwnerId { get; set; }

        [BsonElement("SharedAccessUserIds")]
        public List<string> SharedAccessUserIds { get; set; } = new List<string>();
    }
}

