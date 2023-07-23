using System;

namespace NotesApp.DAL.DataAccess.Configuration.Abstractions
{
	public interface IMongoDbSettings
	{
        string DatabaseName { get; set; }

        string ConnectionString { get; set; }
    }
}

