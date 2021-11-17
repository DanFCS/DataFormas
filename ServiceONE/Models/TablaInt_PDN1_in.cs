using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_PDN1_in
    {
        public int DocNum { get; set; }
        public string ItemCode { get; set; }
        public float Quantity { get; set; }
        public float PriceBefDi { get; set; }
        public float DiscPrcnt { get; set; }
        public string TaxCode { get; set; }
        public string WhsCode { get; set; }
        public string BatchNum { get; set; }
        public DateTime MnfDate { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}