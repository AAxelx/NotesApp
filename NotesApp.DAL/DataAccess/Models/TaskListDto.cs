using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
	[BsonCollection("TaskList")]
    public class TaskListDto : IDocument
	{
        [BsonId]
        [Required]
        public ObjectId? Id { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        public List<string> Content { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public ObjectId OwnerId { get; set; }

        public List<ObjectId> SharedAccessUserIds { get; set; }
    }
}

