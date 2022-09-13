using application.Data;
using application.Models;
using application.Models.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SPaPSContext _comtext;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            SPaPSContext _context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._comtext = _context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            IdentityUser user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var createUser = await userManager.CreateAsync(user, model.Password);

            if (!createUser.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            user = await userManager.FindByEmailAsync(model.Email);

            Client client = new Client()
            {   
                UserId = user.Id,
                Name = model.Name,
                Address = model.Address,
                IdNo = model.IdNo,
                ClientTypeId = model.ClientTypeId,
                CityId = model.CityId,
                CountryId = model.CountryId
            };

           await _comtext.Clients.AddAsync(client);
           await _comtext.SaveChangesAsync();
                

            return View();
        }
    }
}
