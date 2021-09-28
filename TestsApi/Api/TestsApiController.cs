using System;
using System.Threading.Tasks;
using System.Web.Http;
using TestsApi.Models;

namespace TestsApi.Api
{
    public class TestsApiController : ApiController
    {
        [HttpPost]
        [Route("api/post/{dto}")]
        public async Task<IHttpActionResult> HttpPostAsync(HttpPostDto dto)
        {
            if (dto.Flag) throw new ArgumentNullException("Invalid http request content");
            
            if (dto.Guid == Guid.Empty) return BadRequest();
            
            return Ok(dto);
        }

        [HttpPut]
        [Route("api/put/{dto}")]
        public async Task<IHttpActionResult> HttpPutAsync(HttpPutDto dto)
        {
            if (dto.Flag) throw new ArgumentNullException("Invalid http request content");

            if (dto.Guid == Guid.Empty) return BadRequest();
            
            return Ok(dto);
        }

        [HttpDelete]
        [Route("api/delete/{dto}")]
        public async Task<IHttpActionResult> HttpDeleteAsync(HttpDeleteDto dto)
        {
            if (dto.Flag) throw new ArgumentNullException("Invalid http request content");

            if (dto.Guid == Guid.Empty) return BadRequest();
            
            return Ok();
        }

        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IHttpActionResult> HttpGetAsync(int id)
        {
            if (id < 0) return BadRequest();
            
            return Ok(id);
        }
    }
}