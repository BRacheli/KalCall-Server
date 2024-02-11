using KalCall.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KalCall._API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KalCall : ControllerBase
    {
        //026235529
        //147258

        private static string isPrivate = "private";
        private static  string _baseUrl = $"https://www.call2all.co.il/ym/api/";

        // GET: api/<KalCall>
        [HttpGet]
        public async Task<Root> Get()
        {

            return await Helper.GetFiles(_baseUrl, "026235529:147258", "1");
        }

        // GET api/<KalCall>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<KalCall>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<KalCall>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<KalCall>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
