using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using Ini;


namespace ServiceONE.Data
{
    public class ConexionSAP
    {
        public static Company myCompany = null;
        private static int error;

        public static bool Open()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            try
            {
                IniFile IniFile;
                IniFile = new IniFile(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/config.ini");

                string stSERVER = IniFile.LeerINI("ConexionSap", "Server");
                string stDB = IniFile.LeerINI("ConexionSap", "CompanyDB");
                string stUSUARIO = IniFile.LeerINI("ConexionSap", "UserName");
                string stPASSWORD = IniFile.LeerINI("ConexionSap", "Password");
                string stDBUSUARIO = IniFile.LeerINI("ConexionSap", "DbUserName");
                string stDBPASSWORD = IniFile.LeerINI("ConexionSap", "DbPassword");

                bool respuesta = false;
                //MessageBox.Show("Intentando conectar a SAP");
                myCompany = new Company();

                myCompany.UseTrusted = true;
                myCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                myCompany.Server = "FCS@HANA-03.erpcloudweb.com:30013";// stSERVER;  //"NDB@CCC-DESAR-03.erpcloudweb.com:30013";
                myCompany.CompanyDB = "10028_COPPER_ES_TEST";//stDB; //"10024_CHAMA";
                myCompany.UserName = "consult12@erpcloudweb.com"; //stUSUARIO; //"manager";
                myCompany.Password = "elnida@7";//stPASSWORD; //"B1admin";
                myCompany.DbUserName = "B1admin"; // stDBUSUARIO; //"SYSTEM";
                myCompany.DbPassword = "Fcs@2022!";//stDBPASSWORD;// "Fcs2020!!!";
                myCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La;
                //myCompany.SLDServer = "HANA-03.erpcloudweb.com:30013:40000";
                //myCompany.LicenseServer = "HANA-03.erpcloudweb.com:30013:40000";

               

                error = myCompany.Connect();
                

                if (error == 0)
                {

                    respuesta = true;
                    // MessageBox.Show("Conexion exitosa");

                }
                else
                {
                    objRegistraLog.Graba("-------------------------------------ERROR_CONEXION SAP-------------------------------------------------");
                    string msgErr = "Conexion sin exito: " + myCompany.GetLastErrorDescription();
                    objRegistraLog.Graba(msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    objRegistraLog.Graba("/"+ stSERVER +" "+ stDB + " " + stUSUARIO + " " + stPASSWORD + " " + stDBUSUARIO + " " + stDBPASSWORD );

                }

                return respuesta;
            }
            catch (Exception ex)
            {
                objRegistraLog.Graba("-------------------------------------ERROR_CONEXION SAP-------------------------------------------------");
                objRegistraLog.Graba("Error al momento de conectar con SAP: " + " /" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                return false;

            }
        }
    }
}