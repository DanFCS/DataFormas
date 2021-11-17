using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ServiceONE.Data;
using Ini;


namespace ServiceONE.Controllers
{    
    [AllowAnonymous]
    [RoutePrefix("api/login")]    
    public class LoginController : ApiController
    {       

        [HttpGet] //verifica que el controlador responda
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet] //Activa la Tarea Calendarizada 
        [Route("ActiveMyScheduler")]
        public IHttpActionResult MyScheduler()
        {
            try
            {
                IniFile IniFile;
                IniFile = new IniFile(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/config.ini");

                string stHiniciao = IniFile.LeerINI("MyScheduler", "HoraInicio");
                string stMinicio = IniFile.LeerINI("MyScheduler", "MinutosInicio");
                string stCxM = IniFile.LeerINI("MyScheduler", "CadaXMinutos");

                int hora = Convert.ToInt32(stHiniciao);
                int minutos = Convert.ToInt32(stMinicio);
                int cadamin = Convert.ToInt32(stCxM);

                Data.MyScheduler.IntervalInMinutes(hora, minutos, cadamin,
               () => {

                   //Inserts de la Hana a SAP
                   //SapTi_Data.InsertTI_Costos();
                   //SapTi_Data.InsertTI_OCRD();
                   //SapTi_Data.InsertTI_OITM();
                   //SapTi_Data.InsertTI_OPOR();
                   //SapTi_Data.InsertTI_OSTC();
                   //SapTi_Data.InsertTI_OWTR();

                   //Inserts de la tabla Intermedia a SAP

                   //  TiSap_Data.Insertar_Factura_OINV();
                    // TiSap_Data.Insertar_NotaCredito_ORIN();
                     TiSap_Data.Insertar_PagosRecibidos_ORCT();

                   #region Funciones que no son de Buenas Vista

                   //TiSap_Data.Insertar_FacturaProve_OPCH();
                   //TiSap_Data.Insertar_Transferencia_OWTR();
                   //TiSap_Data.Insertar_Proveedores_ORPC();
                   //TiSap_Data.Insertar_SalidaMercancia_OIGE();
                   //TiSap_Data.Insertar_EntradaMercancía_OIGN();
                   //TiSap_Data.Insertar_EntradaMercancía_OC_OPDN();
                   //TiSap_Data.Insertar_Contabilizacion_OIQR();


                   #endregion


               });

                return Ok(true);

            }
            catch (Exception)
            {

                return Ok(false);
            }
           
        }

        [HttpGet] //verifica si hay usuarios autentificado
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost] //Método para autentificarse y envio del token para usar los métodos autorizados = [Authorize]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            IniFile IniFil;
            IniFil = new IniFile(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/config.ini");
            //el archivo config.ini queda alojado en el la carpeta  del publicado

            string stUser = IniFil.LeerINI("LoginApi", "User");
            string stPass = IniFil.LeerINI("LoginApi", "Pass");


            bool isCredentialValid = (login.Username == stUser && login.Password == stPass);
            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Username);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
 }