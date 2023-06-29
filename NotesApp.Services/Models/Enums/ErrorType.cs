using System;
namespace NotesApp.Services.Models.Enums
{
    public enum ErrorType
    {
        Ok = 0,
        NoContent = 204,
        BadRequest = 404,
        Forbidden = 403,
        InternalServerError = 500,
    }
}

