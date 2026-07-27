using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly ContractService _contractService;

        public ContractController(ContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _contractService.GetById(id);

            if (result == null)
            {
                return NotFound(new { message = $"Contract with ID {id} not found." });
            }

            return Ok(result);
        }
    }
}
