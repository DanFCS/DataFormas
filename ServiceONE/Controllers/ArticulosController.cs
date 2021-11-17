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
    [RoutePrefix("api/Articulos")]
    public class ArticulosController : ApiController
    {
        // GET api/<controller>
        //[Route("ART_OUT")]
        //public List<TablaInt_OITM_out> GetAll()
        //{
        //    return OITM_out_Data.Listar_Articulos();
        //}
        
        // POST api/<controller>
      //  [Route("ART_OUT")]
        //public List<TablaInt_OITM_out> consultarArticulos([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OITM_out_Data.Listar_Articulos(objTS);


        //    }
        //}

    }
}