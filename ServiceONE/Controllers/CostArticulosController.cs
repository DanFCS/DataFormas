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
    [RoutePrefix("api/CostArticulos")]
    public class CostArticulosController : ApiController
    {
        // GET api/<controller>
        //[Route("COSTO_OUT")]
        //public List<TablaInt_COSTOS_out> GetAll()
        //{
        //    return COSTOS_out_Data.Listar_CostArticulos();
        //}

        // POST api/<controller>
        //[Route("COSTO_OUT")]
        //public List<TablaInt_COSTOS_out> consultarCostArticulos([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return COSTOS_out_Data.Listar_CostArticulos(objTS);


        //    }
        //}

    }
}