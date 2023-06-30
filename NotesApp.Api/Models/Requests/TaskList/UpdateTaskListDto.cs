using System;
using MongoDB.Bson;

namespace NotesApp.Api.Models.Requests.TaskList
{
	public class UpdateTaskListDto
	{
        public string Title { get; set; }

        public List<string> Content { get; set; }

        public ObjectId OwnerId { get; set; }

        public List<ObjectId> SharedAccessUserIds { get; set; }
    }
}

