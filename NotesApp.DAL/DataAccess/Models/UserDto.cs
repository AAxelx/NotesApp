using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Models
{
    [BsonCollection("User")]
    public class UserDto : IDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}

