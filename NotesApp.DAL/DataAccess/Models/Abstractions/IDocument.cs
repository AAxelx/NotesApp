using System;
using MongoDB.Bson;

namespace NotesApp.DAL.DataAccess.Models.Abstractions
{
	public interface IDocument
	{
        public ObjectId Id { get; set; }
    }
}

