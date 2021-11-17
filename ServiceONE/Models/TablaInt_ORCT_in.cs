using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_ORCT_in
    {
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public float CashSum { get; set; }
        public float CreditSum { get; set; }
        public float TrsfrSum { get; set; }
        public int ID_Factura { get; set; }
        public char Origen_documento { get; set; }      
        public List<TablaInt_RCT2_in> Detalle { get; set; }
    }
}