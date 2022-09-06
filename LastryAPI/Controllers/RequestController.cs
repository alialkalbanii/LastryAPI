using LastryAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace LastryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;



        public RequestController(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }


        [HttpPost("PostRequest"), Authorize(Roles = "User")]
        public async Task<ActionResult<List<Request>>> AddHeAddRequest(RequestDto requestDto)
        {
            Request requestToAdd = new Request();
            requestToAdd.requestDetails = requestDto.requestDetails;
            requestToAdd.creatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            requestToAdd.systemreq = requestDto.requestSystem;
            _context.Requests.Add(requestToAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetAllRequests"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Request>>> Get()
        {
            return Ok(await _context.Requests.ToListAsync());

        }

        [HttpGet("GetAllRequestsFor"), Authorize(Roles = "User")]
        public async Task<ActionResult<List<Request>>> GetRequestsForId()
        {
            var requests = await _context.Requests.Where(t => t.creatorId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync();
            return Ok(requests);

        }

        [HttpPut("ChangeStatus"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<String>> EditRequestStatus(EditRequestDto request)
        {
            var x = _context.Requests.Where(d => d.requestId == request.Id).FirstOrDefault();
            if (x == null)
            {
                return BadRequest("Request was not found");
            }
            x.requestStatus = request.Status;
            await _context.SaveChangesAsync();

            return Ok("Saved");
        }

    }
}
