using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assessment
{
    public class PeopleController : ODataController
    {
        private readonly PeopleContext _personDbContext;

        public PeopleController(PeopleContext context)
        {
            _personDbContext = context;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_personDbContext.People);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            var person = _personDbContext.People.Find(key);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Person> person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await _personDbContext.People.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            person.Patch(entity);
            try
            {
                await _personDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri]int key, [FromBody] Person update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            _personDbContext.Entry(update).State = EntityState.Modified;
            try
            {
                await _personDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        public async Task<IActionResult> Post([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            person.Id = 0;
            var entry = _personDbContext.Add(person);
            await _personDbContext.SaveChangesAsync();
            return Created(person);
        }

        public async Task<ActionResult> Delete([FromODataUri] int key)
        {
            var person = await _personDbContext.People.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }
            _personDbContext.People.Remove(person);
            await _personDbContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        private bool PersonExists(int key)
        {
            return _personDbContext.People.Find(key) != null;
        }
    }
}