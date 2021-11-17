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
    [RoutePrefix("api/ListImpuestos")]
    public class ListImpuestosController : ApiController
    {
        // GET api/<controller>
        //[Route("IMP_OUT")]
        //public List<TablaInt_OSTC_out> GetAll()
        //{
        //    return OSTC_out_Data.Listar_ListaImpuestos();
        //}

        // POST api/<controller>
        //[Route("IMP_OUT")]
        //public List<TablaInt_OSTC_out> consultarListaImpuestos([FromBody] TopSkip objTS)
        //{
        //    if (objTS == null)
        //    { throw new HttpResponseException(HttpStatusCode.BadRequest); }

        //    else
        //    {
        //        return OSTC_out_Data.Listar_ListaImpuestos(objTS);


        //    }
        //}

    }
}