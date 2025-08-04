using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemberRequestAPI.Data;
using MemberRequestAPI.Models;

namespace MemberRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MemberRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/MemberRequest
        [HttpPost]
        public async Task<ActionResult<MemberRequest>> CreateMemberRequest(MemberRequest memberRequest)
        {
            _context.MemberRequests.Add(memberRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateMemberRequest), new { id = memberRequest.RequestId }, memberRequest);
        }

        // GET: api/MemberRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberRequest>>> GetMemberRequests()
        {
            return await _context.MemberRequests.ToListAsync();
        }

        // GET: api/MemberRequest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberRequest>> GetMemberRequest(int id)
        {
            var memberRequest = await _context.MemberRequests.FindAsync(id);
            if (memberRequest == null)
            {
                return NotFound();
            }
            return memberRequest;
        }
    }
}
