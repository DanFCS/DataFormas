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
    //[RoutePrefix("api/OrdCompra")]
    public class OrdCompraController : ApiController
    {
        // GET api/<controller>
        //[Route("OC_OUT")]
        //public List<TablaInt_OPOR_out> GetAll()
        //{
        //    return OPOR_out_Data.Listar_OrdCompra();
        //}


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }


        // POST api/<controller>
        [Route("OC_OUT")]
        //public List<TablaInt_OPOR_out> consultarOrdenCompra([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OPOR_out_Data.Listar_OrdCompra(objTS);


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