using ServiceONE.Data;
using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceONE.Controllers
{
    [Authorize]
   // [RoutePrefix("api/SocNegocios")]
    public class SocNegociosController : ApiController
    {
        // GET api/<controller>
        //[Route("CLI_OUT")]
        //public List<TablaInt_OCRD_out> GetAll()
        //{
        //    return OCRD_out_Data.Listar_SocNegocios();
        //}

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("CLI_OUT")]
        //public List<TablaInt_OCRD_out> consultarSocNegocios([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OCRD_out_Data.Listar_SocNegocios(objTS);


        //    }
        //}

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}