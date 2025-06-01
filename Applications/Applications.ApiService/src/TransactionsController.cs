using Applications.ApiService.src.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Applications.ApiService.src
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TransactionsController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetZero()
        {
            return Ok(0);
        }


    }


}
