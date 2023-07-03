using MongoDB.Bson;

namespace NotesApp.Api.Models.TaskList
{
	public class TaskListResponse
	{
        public string Id { get; set; }

        public string Title { get; set; }

        public List<string> Content { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public string OwnerId { get; set; }

        public List<string> SharedAccessUserIds { get; set; }
    }
}