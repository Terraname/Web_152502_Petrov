using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.API.Data;
using Web_152502_Petrov.API.Services;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;

namespace Web_152502_Petrov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CathegoriesController : ControllerBase
    {
        //private readonly AppDbContext _context;
        private readonly ICathegoryService _cathegoryService;

        //public CathegoriesController(AppDbContext context)
        //{
        //    _context = context;
        //}
        public CathegoriesController(ICathegoryService cathegoryService)
        {
            _cathegoryService = cathegoryService;
        }

        // GET: api/Cathegories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cathegory>>> GetCathegories()
        {
            var cathegories = await _cathegoryService.GetCathegoryListAsync();

            if (!cathegories.Success)
            {
                return NotFound(cathegories.ErrorMessage);
            }

            return Ok(cathegories.Data);
        }

        // GET: api/Cathegories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cathegory>> GetCathegory(int id)
        {
            var cathegories = await _cathegoryService.GetCathegoryListAsync();

            if (cathegories == null)
            {
                return NotFound();
            }
            //var cathegory = await _context.Cathegories.FindAsync(id);
            var cathegory = cathegories.Data.Find(x => x.Id == id);

            if (cathegory == null)
            {
                return NotFound();
            }

            return cathegory;
        }

        //PUT: api/Cathegories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCathegory(int id, Cathegory cathegory)
        {
            if (id != cathegory.Id)
            {
                return BadRequest();
            }

            await _cathegoryService.UpdateCathegoryAsync(id, cathegory);
            //if (id != cathegory.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(cathegory).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CathegoryExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Cathegories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cathegory>> PostCathegory(Cathegory cathegory)
        {
            var cathegoties = await _cathegoryService.GetCathegoryListAsync();
            if (!cathegoties.Success)
            {
                return Problem("Entity set 'AppDbContext.Cathegory'  is null.");
            }
            var drug0 = _cathegoryService.CreateCathegoryAsync(cathegory);
            //_context.Drugs.Add(drug);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetCathegory", new { id = cathegory.Id }, cathegory);
            //if (_context.Cathegories == null)
            //{
            //    return Problem("Entity set 'AppDbContext.Cathegories'  is null.");
            //}
            //_context.Cathegories.Add(cathegory);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCathegory", new { id = cathegory.Id }, cathegory);
        }

        // DELETE: api/Cathegories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCathegory(int id)
        {
            //var cathegories = await _cathegoryService.GetCathegoryListAsync();
            //if (cathegories == null)
            //{
            //    return NotFound();
            //}
            //var cathegory = await _cathegoryService.GetCathegoryByIdAsync(id);
            //if (cathegory == null)
            //{
            //    return NotFound();
            //}

            await _cathegoryService.DeleteCathegoryAsync(id);
            //await _context.SaveChangesAsync();

            return NoContent();
            //if (!cathegories.Success)
            //{
            //    return NotFound(cathegories.ErrorMessage);
            //}

            //return Ok(cathegories);
            //if (_context.Cathegories == null)
            //{
            //    return NotFound();
            //}
            //var cathegory = await _context.Cathegories.FindAsync(id);
            //if (cathegory == null)
            //{
            //    return NotFound();
            //}

            //_context.Cathegories.Remove(cathegory);
            //await _context.SaveChangesAsync();

            //return NoContent();
        }

        private bool CathegoryExists(int id)
        {
            var cathegory = _cathegoryService.GetCathegoryByIdAsync(id);
            if (cathegory == null)
            { return false; }
            else { return true; }
            //return (_context.Drugs?.Any(e => e.Id == id)).GetValueOrDefault();
            //return (_context.Cathegories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
