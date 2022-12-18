using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CustomerEstablishmentRequest.Models;
using System.Linq;

namespace CustomerEstablishmentRequest.Controllers
{
    [Route("api/[controller]")]
    public class InputController : Controller
    {

        [HttpGet]
        public IEnumerable<Input> GetAll()
        {
            return _context.InputItems.ToList();
        }

        [HttpGet("{id}", Name = "GetInput")]
        public IActionResult GetById(long id)
        {
            var item = _context.InputItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Input item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.InputItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetInput", new { id = item.Id }, item);
        }

        private readonly InputContext _context;

        public InputController(InputContext context)
        {
            _context = context;

        }
    }
}
