using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OIGE_in
    {
        public DateTime DocDate { get; set; }
        public int DocNum { get; set; }
        public String Comments { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
        public List<TablaInt_IGE1_in> Detalle { get; set; }
    }
}