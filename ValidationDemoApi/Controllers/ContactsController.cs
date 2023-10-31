using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidationDemoApi.CORE.Interfaces;
using ValidationDemoApi.CORE.Models;
using ValidationDemoApi.Mappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ValidationDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IRepository<Contact> _contactRepo;

        public ContactsController(IRepository<Contact> contactRepo)
        {
            _contactRepo = contactRepo;
        }


        // GET: api/<ContactsController>
        [HttpGet]
        public IActionResult Get()
        {
            var contacts = _contactRepo.GetAll();
            var contactsWithoutOrders = contacts.Select(x => x.MapToDto());
            return Ok(contactsWithoutOrders);
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var contact = _contactRepo.GetById(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact.MapToDto());
        }

        // POST api/<ContactsController>
        [HttpPost]
        //[Authorize]
        public IActionResult Post([FromBody] ContactDto contact)
        {
            var userId = User.Claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // make sure the email is unique
            var existingContact = _contactRepo.GetOne(c => c.Email == contact.Email);
            if (existingContact != null)
            {
                ModelState.AddModelError("Email", "Email must be unique");
                return BadRequest(ModelState);
            }


            _contactRepo.Add(contact.MapToEntity());
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Contact contact)
        {
            var oldContact = _contactRepo.GetById(id);
            if (oldContact == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _contactRepo.Update(contact);
            return NoContent();
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contact = _contactRepo.GetById(id);
            if (contact == null)
            {
                return NotFound();
            }
            _contactRepo.Delete(contact);
            return NoContent();
        }
    }
}
