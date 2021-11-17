using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TabaInt_POR1_out
    {
        public int DocNum { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }        
        public decimal PriceBefDi { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string TaxCode { get; set; }
        public string WhsCode { get; set; }
    }
}