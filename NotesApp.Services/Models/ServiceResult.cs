using NotesApp.Services.Models.Enums;

namespace NotesApp.Services.Models
{
    public class ServiceResult
    {
        public ResponseType ResponseType { get; set; }

        public ServiceResult(ResponseType type)
        {
            ResponseType = type;
        }
    }
}

