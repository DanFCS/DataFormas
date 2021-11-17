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
     [RoutePrefix("api/InventarioAlmacen")]
    public class InventarioAlmacenController : ApiController
    {
        //// GET api/<controller>
        //[Route("IALMACEN_OUT")]
        //public List<TablaInt_OITW> GetAll()
        //{
        //    return OITW_out_Data.Lista_Inventario_Almacen();
        //}

        // POST api/<controller>
       // [Route("IALMACEN_OUT")]
        //public List<TablaInt_OITW> consultarInventarioAlmacen([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OITW_out_Data.Lista_Inventario_Almacen(objTS);


        //    }
        //}


    }
}