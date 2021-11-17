﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_INV1_in
    {
        public int DocNum { get; set; }
        public string ItemCode { get; set; }
        public float Quantity { get; set; }
        public float PriceBefDi { get; set; }
        public float DiscPrcnt { get; set; }
        public string TaxCode { get; set; }
        public string TaxCode_Servicio { get; set; }
        public string WhsCode { get; set; }
        public string U_Cabys { get; set; }
        public string OcrCode { get; set; }
        public string ID_Factura { get; set; }
        public string Origen_documento { get; set; }

        public float LineTotal { get; set; }
        public string OcrCode2 { get; set; }
        public string ItemName { get; set; }
        public string UnitMsr { get; set; }
        public string U_Servicio { get; set; }

    }
}