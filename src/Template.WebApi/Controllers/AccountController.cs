using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Template.WebApi.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Template.WebApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Template.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [FromServices]
        public UserManager<ApplicationUser> UserManager { get; set; }
        [FromServices]
        public SignInManager<ApplicationUser> SignInManager { get; set; }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody]RegisterViewModel model)
        {

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
            };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(user);
            }
            else
                return HttpBadRequest(result.Errors);

        }
    }
}
