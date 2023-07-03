using System;
namespace NotesApp.Services.Models.Enums
{
    public enum ResponseType
    {
        Ok = 0,
        NoContent = 204,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}

