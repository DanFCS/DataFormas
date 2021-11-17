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
   [RoutePrefix("api/ListaPrecios")]
    public class ListaPreciosController : ApiController
    {
        // GET api/<controller>
        //[Route("LPrecios_OUT")]
        //public List<TablaInt_ITM1_out> GetAll()
        //{
        //    return ITM1_out_Data.Lista_Precios();
        //}


        // POST api/<controller>
      //  [Route("LPrecios_OUT")]
        //public List<TablaInt_ITM1_out> consultarListaPrecios([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return ITM1_out_Data.Lista_Precios(objTS);
                
        //    }
        //}



    }
}