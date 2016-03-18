using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Template.WebApi.Models;
using Microsoft.AspNet.Cors;
using Template.Domain;
using Template.Domain.Repositories;
using Template.DTO.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Template.WebApi.Controllers
{
    //[Authorize(Policy = "API")]
    //[EnableCors("AllowAllOrigins")]
    [Authorize]
    [Route("api/[controller]")]
    public class PrValuesController : Controller
    {
        [FromServices]
        public IUnitOfWork<string> UnitOfWork { get; set; }
        // GET: api/prvalues
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = (await UnitOfWork.SampleEntityRepository.GetAsync(q => q.Select(x => new SampleEntityViewModel()
            {
                Name = x.Name,
                Description = x.Description
            }))).ToList();
            res.Add(new SampleEntityViewModelA()
            {
                Name = "Fake",
                Description = "Fake desc",
                ADesc = "Fake desc A"
            });
            return Ok(res);
        }

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
