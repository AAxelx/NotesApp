using MongoDB.Bson;

namespace NotesApp.Api.Models.TaskList
{
	public class TaskList
	{
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public List<string> Content { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public ObjectId OwnerId { get; set; }

        public List<ObjectId> SharedAccessUserIds { get; set; }
    }
}