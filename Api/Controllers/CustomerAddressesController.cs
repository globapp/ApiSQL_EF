#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetcustomerAddress(string id)
        {
            return await _context.customerAddress.Where(s => s.Idcustomer.Equals(id)).OrderByDescending(s => s.CreatedDate).ToListAsync();
        }



        //public async Task<ActionResult<IEnumerable<CustomerAddress>>> Search(string id)
        //{
        //    return await _context.customerAddress.Where(s => s.Idcustomer.Contains(id)).OrderByDescending(s => s.CreatedDate).ToListAsync();
        //}


        // GET: api/CustomerAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetCustomerAddress(string id)
        {
            var customerAddress = await _context.customerAddress.FindAsync(id);

            if (customerAddress == null)
            {
                return NotFound();
            }

            return customerAddress;
        }

        // PUT: api/CustomerAddresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAddress(string id, CustomerAddress customerAddress)
        {
            if (id != customerAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerAddresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerAddress>> PostCustomerAddress(CustomerAddress customerAddress)
        {
            _context.customerAddress.Add(customerAddress);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerAddressExists(customerAddress.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomerAddress", new { id = customerAddress.Id }, customerAddress);
        }

        // DELETE: api/CustomerAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAddress(string id)
        {
            var customerAddress = await _context.customerAddress.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            _context.customerAddress.Remove(customerAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerAddressExists(string id)
        {
            return _context.customerAddress.Any(e => e.Id == id);
        }
    }
}
