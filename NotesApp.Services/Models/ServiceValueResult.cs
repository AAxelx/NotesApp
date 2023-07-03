using NotesApp.Services.Models.Enums;

namespace NotesApp.Services.Models
{
    public class ServiceValueResult<T> : ServiceResult
    {
        public T? Value { get; set; }

        public ServiceValueResult(ResponseType type) : base(type)
        {
        }

        public ServiceValueResult(T value, ResponseType type = ResponseType.Ok) : base(type)
        {
            Value = value;
        }
    }
}

