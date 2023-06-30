using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Api.Models.Requests.TaskList
{
	public class CreateTaskListDto
	{
        public string Title { get; set; }

        public List<string> Content { get; set; }

        public List<ObjectId> SharedAccessUserIds { get; set; }
    }
}

