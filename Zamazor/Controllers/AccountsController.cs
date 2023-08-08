using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Business.Enums;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Entities;
using DataAccess.Enums;

namespace Zamazor.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IUserService _userService;

        public AccountsController(IAccountService accountService, ICountryService countryService, ICityService cityService, IUserService userService)
        {
            _accountService = accountService;
            _countryService = countryService;
            _cityService = cityService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            List<UserModel> model = _userService.Query().ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            UserModel model = _userService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (model == null)
                return View("Error", "User not found!");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            UserModel model = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (model == null)
                return View("Error", "User not found!");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateGet()
        {
            return View("CreateHtml");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(string Firstname, string Lastname, string Username, string Password, bool IsActive, Role Role, string Email, string Address, Gender Gender, Country Country, City City)
        {
            UserModel model = new UserModel()
            {
                Firstname = Firstname,
                Lastname = Lastname,
                Username = Username,
                Password = Password,
                IsActive = IsActive,
                RoleId = Role.Id,
                Email = Email,
                Address = Address,
                Gender = Gender,
                CountryId = Country.Id,
                CityId = City.Id
                
            };
            var result = _userService.Add(model);
            if (result.IsSuccessful)
                return RedirectToAction(nameof(Index));
            return View("Error", result.Message);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, result.Data.Username),
                    new Claim(ClaimTypes.Role, result.Data.RoleNameDisplay),
                    new Claim(ClaimTypes.Sid, result.Data.Id.ToString())
                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UnauthorizedAction()
        {
            return View("Error", "You are not authorized to perform this action!");
        }

        public IActionResult Register()
        {
            ViewBag.Countries = new SelectList(_countryService.Query().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Register(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Login));
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Countries = new SelectList(_countryService.Query().ToList(), "Id", "Name", model.CountryId);

            var cityResult = _cityService.List(model.CountryId ?? -1);

            ViewBag.Cities = new SelectList(cityResult.Data, "Id", "Name", model.CityId);

            return View(model);
        }
    }
}