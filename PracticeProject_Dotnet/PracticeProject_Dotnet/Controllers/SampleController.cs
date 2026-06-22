using Microsoft.AspNetCore.Mvc;


namespace PracticeProject_Dotnet.Controllers
{
    [ApiController]
    [Route("api/SampleController")]
    public class SampleController : ControllerBase
    {
        [HttpGet("sample")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult SampleGetApi()
        {
            return Ok("This is the response");
        }

        [HttpGet("sampleWithCustomRouteConstraint/{id}")]
        public IActionResult SampleGetApiWithRouteConstraint(int id)
        {
            return Ok($"Here is the value {id}");
        }
    }
}