using System;
using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
	public class TaskListLiteDto
	{
        public ObjectId Id { get; set; }
        public string Title { get; set; }
    }
}

