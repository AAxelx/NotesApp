using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Services.Models;
using NotesApp.Services.Models.Enums;
namespace NotesApp.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        private readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IActionResult MapResponse(ServiceResult result)
        {
            return GetResponseByType(result.ResponseType);
        }

        protected IActionResult MapResponse<TServiceModel, TResponseModel>(ServiceValueResult<TServiceModel> result, Func<TServiceModel, TResponseModel> map)
        {
            if (result.ResponseType != ResponseType.Ok)
            {
                return GetResponseByType(result.ResponseType);
            }

            return Ok(map.Invoke(result.Value));
        }

        protected IActionResult GetResponseByType(ResponseType responseType)
        {
            switch (responseType)
            {
                case ResponseType.Ok:
                    return Ok();
                case ResponseType.NoContent:
                    return NoContent();
                case ResponseType.BadRequest:
                    return BadRequest(); ;
                case ResponseType.Forbidden:
                    return Forbid();
                case ResponseType.NotFound:
                    return NotFound();
                default:
                case ResponseType.InternalServerError:
                    return StatusCode(500);
            }
        }
    }
}

