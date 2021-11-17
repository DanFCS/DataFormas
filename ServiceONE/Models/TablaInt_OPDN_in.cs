using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OPDN_in
    {
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public float DocRate { get; set; }
        public float DiscPrcnt { get; set; }
        public string NumOC { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
        public List<TablaInt_PDN1_in> Detalle { get; set; }
    }
}