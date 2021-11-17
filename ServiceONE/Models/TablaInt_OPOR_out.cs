using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OPOR_out
    {
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public decimal DocRate { get; set; }
        public decimal DiscPrcnt { get; set; }
        public List<TabaInt_POR1_out> Detalle { get; set; }
       
    }
}