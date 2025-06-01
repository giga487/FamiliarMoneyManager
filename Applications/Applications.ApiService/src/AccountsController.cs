using Applications.ApiService.src.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Applications.ApiService.src
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AccountsController(ApplicationContext context)
        {
            _context = context;
        }

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] Account account)
        //{
        //    var existingAccount = await _context.Accounts.FirstOrDefaultAsync(t => t.ClientId == account.ClientId);

        //    if (existingAccount is null)
        //    {
        //        return BadRequest("Account not found.");
        //    }


        //}


        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(t => t.ClientId == account.ClientId);

            if (_context.Accounts.Where(t => t.ClientId == account.ClientId).Any())
            {
                return Conflict("An account with this ClientId already exists.");
            }

            _context.Accounts.Add(account);

            try
            {
                int result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok("Changes saved successfully.");
                }
                else
                {
                    return BadRequest("No changes were made.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return StatusCode(500, "An error occurred while saving changes.");
            }

        }

        [HttpGet("AccountsList")]
        public async Task<IEnumerable<Account>> AccountsList()
        {
            //var accountNameList = _context.Accounts.Select(t => t.ClientId); // Ensure the database is created and the ID is set correctly.
            return await _context.Accounts.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return account;
        }
    }



}
