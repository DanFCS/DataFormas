using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_ORPC_in
    {
        public string CardCode { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public float DocRate { get; set; }
        public float DiscPrcnt { get; set; }
        public string NumAtCard { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
        public List<TablaInt_RPC1_in> Detalle { get; set; }
    }
}