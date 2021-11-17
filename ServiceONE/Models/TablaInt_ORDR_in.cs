using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_ORDR_in
    {
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public float DocRate { get; set; }
        public float DiscPrcnt { get; set; }
        public float DOCTOTAL { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
        public List<TablaInt_RDR1_in> Detalle { get; set; }
    }
}