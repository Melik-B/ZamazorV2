using Business.Models;
using Business.Services;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zamazor.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CitiesController(ICityService cityService, ICountryService countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        [Authorize]
        public IActionResult Index(int countryId)
        {
            List<CityModel> model = _cityService.Query().ToList();
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }
            CityModel city = _cityService.Query().SingleOrDefault(s => s.Id == id.Value);
            if (city == null)
            {
                return View("Hata", "Kayıt bulunamadı!");
            }
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name", city.CountryId);
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var result = _cityService.Update(city);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name", city.CountryId);
            return View(city);
        }

        public IActionResult GetCities(int countryId) // Cities/GetCities/1
        {
            var result = _cityService.List(countryId);
            var model = result.Data;
            return Json(model);
        }

        [HttpPost]
        public IActionResult PostCities(int countryId)
        {
            var result = _cityService.List(countryId);
            var model = result.Data;
            return Json(model);
        }
    }
}