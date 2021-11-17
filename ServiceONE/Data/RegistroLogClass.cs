using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ServiceONE.Data
{
    public class RegistroLogClass
    {

        public int Graba(string strLog)
        {
            string strDestinoLog = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + @"\Log\"; //Directory.GetCurrentDirectory() + @"\Log\";


            if (!Directory.Exists(strDestinoLog))
            {
                Directory.CreateDirectory(strDestinoLog);
            }

            string strNombreArchivoLog = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            try
            {
                StreamWriter sw = new StreamWriter(strDestinoLog + strNombreArchivoLog, true, System.Text.Encoding.UTF8);
                sw.WriteLine(strLog);
                sw.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("Exception: " + ex1.Message);
            }
            return 0;
        }
    }
}