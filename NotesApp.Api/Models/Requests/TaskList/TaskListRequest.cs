using System;

namespace NotesApp.Api.Models.Requests.TaskList
{
	public class TaskListRequest
	{
        public string Title { get; set; }

        public List<string> Content { get; set; }
    }
}

