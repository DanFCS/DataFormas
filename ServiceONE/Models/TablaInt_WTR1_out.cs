using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_WTR1_out
    {
        //Campos de Detalle
        
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public int DocNum { get; set; }
        public string Filler { get; set; }
        public string ToWhsCode { get; set; }
        public string BatchNum { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime MnfDate { get; set; }

    }
}