using System;
using MongoDB.Bson;

namespace NotesApp.DAL.DataAccess.Models.Abstractions
{
	public interface ITaskList : IDocument
	{
        public ObjectId OwnerId { get; set; }
        public List<ObjectId> SharedWith { get; set; }
    }
}

