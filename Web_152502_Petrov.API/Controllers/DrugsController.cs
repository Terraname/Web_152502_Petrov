using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
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
    public class DrugsController : ControllerBase
    {
        //private readonly AppDbContext _context;
        private readonly IDrugService _drugService;

        public DrugsController(IDrugService drugService)
        {
            _drugService = drugService;
        }



        //public DrugsController(IDrugService drugService)
        //{
        //    _drugService = drugService;
        //}

        //// GET: api/Pictures
        //[HttpGet("")]
        //[Route("{genre}")]
        //[Route("page{pageNo}")]
        //[Route("{genre}/page{pageNo}")]
        //public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs(string? genre, int pageNo = 1, int pageSize = 3)
        //{
        //    // check error 
        //    return Ok(await _drugService.GetDrugListAsync(genre, pageNo, pageSize));
        //}


        //// GET: api/Drugs
        //[HttpGet]
        //GET: api/Drugs
        [HttpGet("")]
       // [Route("{cathegory}")]
        [Route("page{pageNo}")]
        //[Route("{cathegory}/page{pageNo}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs(string? cathegory, int pageNo = 1, int pageSize = 3)
        {
            //var drugs = await _drugService.GetDrugListAsync(cathegoryNormalizedName, pageNo, pageSize);

            //if (!drugs.Success)
            //{
            //    return NotFound(drugs.ErrorMessage);
            //}

            //return Ok(drugs.Data.Items);

            return Ok(await _drugService.GetDrugListAsync(cathegory, pageNo, pageSize));

            //if (_context.Drugs == null)
            //{
            //    return NotFound();
            //}
            //return await _context.Drugs.ToListAsync();
        }

        // GET: api/Drugs/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Drug>> GetDrug(int id)
        {
            var drugs = await _drugService.GetDrugListAsync(null);

            if (drugs == null)
            {
                return NotFound();
            }
            //var cathegory = await _context.Cathegories.FindAsync(id);
            //var drug = drugs.Data.Find(x => x.Id == id);
            var drug = await _drugService.GetDrugByIdAsync(id);

            if (drug == null)
            {
                return NotFound();
            }

            return drug.Data;
            //if (_context.Drugs == null)
            //{
            //    return NotFound();
            //}
            //var drug = await _context.Drugs.FindAsync(id);

            //if (drug == null)
            //{
            //    return NotFound();
            //}

            //return drug;
        }

        // PUT: api/Drugs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDrug(int id, Drug drug)
        {
            if (id != drug.Id)
            {
                return BadRequest();
            }

            await _drugService.UpdateDrugAsync(id, drug);
            //_context.Entry(drug).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!DrugExists(id))
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

        // POST: api/Drugs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Drug>> PostDrug(Drug drug)
        {
            var drugs = await _drugService.GetDrugListAsync(null);
            if (!drugs.Success)
            {
                return Problem("Entity set 'AppDbContext.Drugs'  is null.");
            }
            var drug0 = _drugService.CreateDrugAsync(drug);
            //_context.Drugs.Add(drug);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrug", new { id = drug.Id }, drug);
        }

        // POST: api/Drugs/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<string>>> PostImage(
            int id,
            IFormFile formFile)
        {
            var response = await _drugService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDrug(int id)
        {
            await _drugService.DeleteDrugAsync(id);

            //if (_context.Drugs == null)
            //{
            //    return NotFound();
            //}
            //var drug = await _context.Drugs.FindAsync(id);
            //if (drug == null)
            //{
            //    return NotFound();
            //}

            //_context.Drugs.Remove(drug);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        //[AllowAnonymous]
        private bool DrugExists(int id)
        {
            var drugs = _drugService.GetDrugByIdAsync(id);
            if (drugs == null)
            { return false; }
            else { return true; }
            //return (_context.Drugs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Web_152502_Petrov.API.Data;
//using Web_152502_Petrov.API.Services;
//using Web_152502_Petrov.Domain.Entities;

//namespace Web_152502_Petrov.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DrugsController : ControllerBase
//    {
//        private readonly IDrugService _drugService;

//        public DrugsController(IDrugService drugService)
//        {
//            _drugService = drugService;
//        }

//        // GET: api/Pictures
//        [HttpGet("")]
//        [Route("{genre}")]
//        [Route("page{pageNo}")]
//        [Route("{genre}/page{pageNo}")]
//        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs(string? genre, int pageNo = 1, int pageSize = 3)
//        {
//            // check error 
//            return Ok(await _drugService.GetDrugListAsync(genre, pageNo, pageSize));
//        }

//        //// GET: api/Pictures/5
//        //[HttpGet("{id}")]
//        //public async Task<ActionResult<Picture>> GetPicture(int? id)
//        //{
//        //  if (_pictureService.Pictures == null)
//        //  {
//        //      return NotFound();
//        //  }
//        //    var picture = await _pictureService.Pictures.FindAsync(id);

//        //    if (picture == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    return picture;
//        //}

//        //// PUT: api/Pictures/5
//        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        //[HttpPut("{id}")]
//        //public async Task<IActionResult> PutPicture(int? id, Picture picture)
//        //{
//        //    if (id != picture.Id)
//        //    {
//        //        return BadRequest();
//        //    }

//        //    _pictureService.Entry(picture).State = EntityState.Modified;

//        //    try
//        //    {
//        //        await _pictureService.SaveChangesAsync();
//        //    }
//        //    catch (DbUpdateConcurrencyException)
//        //    {
//        //        if (!PictureExists(id))
//        //        {
//        //            return NotFound();
//        //        }
//        //        else
//        //        {
//        //            throw;
//        //        }
//        //    }

//        //    return NoContent();
//        //}

//        //// POST: api/Pictures
//        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        //[HttpPost]
//        //public async Task<ActionResult<Picture>> PostPicture(Picture picture)
//        //{
//        //  if (_pictureService.Pictures == null)
//        //  {
//        //      return Problem("Entity set 'AppDbContext.Pictures'  is null.");
//        //  }
//        //    _pictureService.Pictures.Add(picture);
//        //    await _pictureService.SaveChangesAsync();

//        //    return CreatedAtAction("GetPicture", new { id = picture.Id }, picture);
//        //}

//        //// DELETE: api/Pictures/5
//        //[HttpDelete("{id}")]
//        //public async Task<IActionResult> DeletePicture(int? id)
//        //{
//        //    if (_pictureService.Pictures == null)
//        //    {
//        //        return NotFound();
//        //    }
//        //    var picture = await _pictureService.Pictures.FindAsync(id);
//        //    if (picture == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    _pictureService.Pictures.Remove(picture);
//        //    await _pictureService.SaveChangesAsync();

//        //    return NoContent();
//        //}

//        //private bool PictureExists(int? id)
//        //{
//        //    return (_pictureService.Pictures?.Any(e => e.Id == id)).GetValueOrDefault();
//        //}
//    }
//}
