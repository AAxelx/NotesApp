using NotesApp.Services.Models.Enums;

namespace NotesApp.Services.Models
{
    public class ServiceValueResult<T> : ServiceResult
    {
        public T? Value { get; set; }

        public ServiceValueResult(ErrorType type) : base(type)
        {
        }

        public ServiceValueResult(T value, ErrorType type = ErrorType.Ok) : base(type)
        {
            Value = value;
        }
    }
}

