using Newtonsoft.Json;
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
    [RoutePrefix("api/Transferecia")]
    public class TransferenciaController : ApiController
    {
        // Funcion por csadmin
        //[Route("TRANSFER_OUT")]
        //public List<TablaInt_OWTR_out> Get()
        //{
        //    return OWTR_out_Data.Listar_Transf();
        //}

        // Funcion por Daniel

        // POST api/<controller>
        [Route("TRANSFER_OUT")]
        public List<TablaInt_OWTR_out> consultar([FromBody] TopSkip objTS)
        {
            if (objTS == null)
            { throw new HttpResponseException(HttpStatusCode.BadRequest); }

            else
            {
                return OWTR_out_Data.Listar_Transf(objTS);


            }
        }


    }
}