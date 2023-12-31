using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Web_152502_Petrov;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Extensions;
using Web_152502_Petrov.Services.CathegoryService;
using Web_152502_Petrov.Services.DrugService;

namespace Web_152502_Petrov.Controllers
{
    public class DrugController : Controller
    {
        private readonly IDrugService drugService;
        private readonly ICathegoryService cathegoryService;

        public DrugController(IDrugService drugService, ICathegoryService cathegoryService)
        {
            this.drugService = drugService;
            this.cathegoryService = cathegoryService;
        }

       

        // GET: DrugController
        public async Task<IActionResult> Index(string? cathegory, int pageNo = 1)
        {
            //ICathegoryService _cathegory = new MemoryCathegoryService();
            var builder = new ConfigurationBuilder().AddJsonFile("appsettins.json");
            //IConfiguration config = System.Configuration.CoonfigurationManager.;
            //IConfiguration config = System.Configuration.ConfigurationManager.
            //var conf = System.Configuration.ConfigurationManager.AppSettings;
            //var b = Microsoft.Extensions.Configuration.ConfigurationManager[];
            //IDrugService _service = new MemoryDrugService(config, _cathegory,1);
            var cathegories =
            await cathegoryService.GetCathegoryListAsync();
            //if (cathegory == null) { cathegory = "soup"; }
            var productResponse =
            await drugService.GetDrugListAsync(cathegory, pageNo);
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);
            ViewBag.Cathegories = cathegories.Data;
            ViewBag.IsAdmin = false;

            if (Request.isAjaxRequest())
            {
                return PartialView("Partials/_PictureListPartial", new
                {
                    Pictures = productResponse.Data!.Items,
                    Genre = cathegory,
                    productResponse.Data.CurrentPage,
                    productResponse.Data.TotalPages,
                    ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
                    IsAdmin = false
                });
            }

            return View(productResponse.Data);
            //Data.Items
        }


        //// GET: DrugController/Details/5.
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: DrugController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: DrugController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: DrugController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: DrugController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: DrugController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: DrugController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
