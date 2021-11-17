using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ini
{
    public class IniFile
    {
        public string ficheroINI { get; private set; }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            ficheroINI = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Seccion"></PARAM>
        /// Section name
        /// <PARAM name="Clave"></PARAM>
        /// Key Name
        /// <PARAM name="Valor"></PARAM>
        /// Value Name
        public void EscribirINI(string Seccion, string Clave, string Valor)
        {
            WritePrivateProfileString(Seccion, Clave, Valor, this.ficheroINI);
        }
        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Seccion"></PARAM>
        /// <PARAM name="Clave"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string LeerINI(string Seccion, string Clave)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Seccion, Clave, "", temp, 255, this.ficheroINI);
            return temp.ToString();
        }
    }
}
