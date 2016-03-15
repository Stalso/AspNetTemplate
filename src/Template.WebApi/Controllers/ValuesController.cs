using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Template.WebApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Template.DTO.ViewModels;
using Newtonsoft.Json;

namespace Template.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [FromServices]
        public ApplicationDbContext<ApplicationUser, Application, IdentityRole, string> context { get; set; }
        //// GET: api/values
        [HttpGet]
        public ActionResult Get()
        {


            //JsonResult
            var res = context.SampleEntities.Select(x => new SampleEntityViewModel()
            {
                Name = x.Name,
                Description = x.Description
            }).ToList();
            res.Add(new SampleEntityViewModelA()
            {
                Name = "Fake",
                Description = "Fake desc",
                ADesc = "Fake desc A"
            });
            return Ok(res);

        }
        // //GET: api/values
        //[HttpGet]
        // public JsonResult Get()
        // {
        //     //JsonResult
        //     var res = context.SampleEntities.Select(x => new SampleEntityViewModel()
        //     {
        //         Name = x.Name,
        //         Description = x.Description
        //     }).ToList();
        //     res.Add(new SampleEntityViewModelA()
        //     {
        //         Name = "Fake",
        //         Description = "Fake desc",
        //         ADesc = "Fake desc A"
        //     });
        //     //return Json(res, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects});
        //     return Json(res);

        // }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
