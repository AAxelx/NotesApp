using NotesApp.Api.Helpers;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Repositories;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;
using NotesApp.Services.Services;
using NotesApp.Services.Services.Abstractions;

namespace NotesApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<ITaskListService, TaskListService>();

        builder.Configuration.AddJsonFile("appsettings.json");

        var mongoDbSettings = new MongoDbSettings();
        builder.Configuration.Bind("MongoDbSettings", mongoDbSettings);
        builder.Services.AddSingleton<IMongoDbSettings>(mongoDbSettings);

        builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        //builder.Services.AddControllers();
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new ObjectIdConverter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

