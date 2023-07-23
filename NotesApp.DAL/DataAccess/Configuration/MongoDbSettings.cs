using NotesApp.DAL.DataAccess.Configuration.Abstractions;

namespace NotesApp.DAL.DataAccess.Configuration
{
	public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }

        public string ConnectionString { get; set; }
	}
}

