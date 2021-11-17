using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OIGN_in
    {
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public String Comments { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public String MSG_ERR { get; set; }
        public List<TablaInt_IGN1_in> Detalle { get; set; }
    }
}