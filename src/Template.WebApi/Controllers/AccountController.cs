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
        
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]RegisterViewModel model)
        {
            //if (ModelState.IsValid && model.Password == model.ConfirmPassword)
            //{
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
            //}
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
