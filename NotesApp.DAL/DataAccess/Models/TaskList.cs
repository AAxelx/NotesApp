using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
	[BsonCollection("TaskList")]
    public class TaskList : ITaskList
	{
        [BsonId]
        public ObjectId Id { get; set; }

        [StringLength(30, MinimumLength = 0)]
        public string Title { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Content { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public ObjectId OwnerId { get; set; }

        public List<ObjectId> SharedWith { get; set; }
    }
}

