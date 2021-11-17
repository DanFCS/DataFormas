using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ini;

namespace ServiceONE.Data
{
    public class Conexion
    {
       

        public static string conexionhana()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            string connectionString = "";

            try
            {
                
                IniFile IniFil;
                IniFil = new IniFile(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/config.ini"); //"Application.StartupPath" (Este se usan con el using Forms)
                                                                                                                     //el archivo config.ini queda alojado en el la carpeta bin del publicado

                string stDNS = IniFil.LeerINI("ConexionHanaDesa", "DSN");
                string stSERVER = IniFil.LeerINI("ConexionHanaDesa", "SERVERNODE");
                string stUSUARIO = IniFil.LeerINI("ConexionHanaDesa", "UID");
                string stPASSWORD = IniFil.LeerINI("ConexionHanaDesa", "PWD");
                string stDB = IniFil.LeerINI("ConexionHanaDesa", "DATABASENAME");

                return connectionString = "DSN=" + stDNS + "; SERVERNODE=" + stSERVER + ";  UID=" + stUSUARIO + "; PWD=" + stPASSWORD + "; DATABASENAME=" + stDB + "";
            }
            catch (Exception ex)
            {

                objRegistraLog.Graba(ex + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                return connectionString = "";
            }
            

        }

        public static string strCon = conexionhana();
        //public static string connectionString = "DSN=HDBODBC; SERVERNODE=HANA-03.erpcloudweb.com:30015;  UID=b1admin; PWD=Tab951Fcs; DATABASENAME=FCS";
    }
}

