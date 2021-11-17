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
   [RoutePrefix("api/Facturas")]
    public class FacturasController : ApiController
    {
        //// GET api/<controller>
        //[Route("FAC_OUT")]
        //public List<TablaInt_OINV> GetAll()
        //{
        //    return OINV_out_Data.Listar_Facturas();
        //}

        // POST api/<controller>
       // [Route("FAC_OUT")]
        //public List<TablaInt_OINV> consultarFacturas([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OINV_out_Data.Listar_Facturas(objTS);


        //    }
        //}

    }
}