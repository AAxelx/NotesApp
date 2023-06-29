using System;
using NotesApp.Services.Models.Enums;

namespace NotesApp.Services.Models
{
    public class ServiceResult
    {
        public ErrorType ErrorType { get; set; }

        public ServiceResult(ErrorType type)
        {
            ErrorType = type;
        }
    }
}

