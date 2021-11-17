using Newtonsoft.Json;
using ServiceONE.Data;
using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace ServiceONE.Controllers
{
    [Authorize]
    [RoutePrefix("api/Envios")]
    public class EnviosController : ApiController
    {
        [Route("CargaFactClientes")]
        public List<InfoInsert> CargaFactClientes([FromBody] List<TablaInt_OINV_in> ListaDocs)
        {
            OINV_in_Data ob = new OINV_in_Data();
            return ob.NuevaFacturaCliente(ListaDocs);
        }

        [Route("CargaNC")]
        public List<InfoInsert> CargaNC([FromBody] List<TablaInt_ORIN_in> ListaDocs)
        {
            ORIN_in_Data ob = new ORIN_in_Data();
            return ob.NuevaNC(ListaDocs);
        }
                  
        [Route("CargaPagosRecibidos")]
        public List<InfoInsert> CargaPagosRecibidos([FromBody] List<TablaInt_ORCT_in> ListaDocs)
        {
            ORCT_in_Data ob = new ORCT_in_Data();
            return ob.NuevoPagoRecibido(ListaDocs);
        }

        [Route("CargaClientes")]
        public List<InfoInsert_CL> CargaClientes([FromBody] List<TablaInt_Cliente_in> ListaDocs)
        {
            Cliente_in_Data ob = new Cliente_in_Data();
            return ob.NuevoCliente(ListaDocs);
        }

        [Route("CargaArticulos")]
        public List<InfoInsert_Art> CargaArticulos([FromBody] List<TablaInt_Articulo_in> ListaDocs)
        {
            Articulo_in_Data ob = new Articulo_in_Data();
            return ob.NuevoArticulo(ListaDocs);
        }

        

        #region Controller que NO son de Buena_Vista

        [Route("CargaTransferStock")]
        public List<InfoInsert> CargaTransferStock([FromBody] List<CS_OWTR> transfer)
        {
            OWTR_out_Data ob = new OWTR_out_Data();
            return  ob.NuevaTranferStock(transfer);
        }         
              
        [Route("CargaNcProveedores")]
        public List<InfoInsert> CargaNcProveedores([FromBody] List<TablaInt_ORPC_in> ListaDocs)
        {
            ORPC_in_Data ob = new ORPC_in_Data();
           return ob.NuevaNcProveedor(ListaDocs);
        }

        [Route("CargaSalidaMerca")]
        public List<InfoInsert> CargaSalidaMerca([FromBody] List<TablaInt_OIGE_in> ListaDocs)
        {
            OIGE_in_Data ob = new OIGE_in_Data();
            return ob.NuevaSalidaMerca(ListaDocs);
        }

        [Route("CargaEntradaMerca")]
        public List<InfoInsert> CargaEntradaMerca([FromBody] List<TablaInt_OIGN_in> ListaDocs)
        {
            OIGN_in_Data ob = new OIGN_in_Data();
           return ob.NuevaEntradaMercancia(ListaDocs);
        }

        [Route("CargaEntradaMercaOC")]
        public List<InfoInsert> CargaEntradaMercaOC([FromBody] List<TablaInt_OPDN_in> ListaDocs)
        {
            OPDN_in_Data ob = new OPDN_in_Data();
             return ob.NuevaEntradaMercanciaOC(ListaDocs);
        }

        [Route("CargaContaStock")]
        public List<InfoInsert> CargaContaStock([FromBody] List<TablaInt_OIQR_in> ListaDocs)
        {
            OIQR_in_Data ob = new OIQR_in_Data();
           return ob.NuevaContaStock(ListaDocs);
        }      
        
        [Route("CargaFactProveedor")]
        public List<InfoInsert> CargaFactProveedor([FromBody] List<TablaInt_OPCH_in> ListaDocs)
        {
            OPCH_in_Data ob = new OPCH_in_Data();
            return ob.NuevaFacturaProveedor(ListaDocs);
        }
        [Route("CargaOrdenVenta")]
        public List<InfoInsert> CargaOrdenVenta([FromBody] List<TablaInt_ORDR_in> ListaDocs)
        {
            ORDR_in_Data ob = new ORDR_in_Data();
            return ob.NuevaOrdenVenta(ListaDocs);
        }

        #endregion
    }
}