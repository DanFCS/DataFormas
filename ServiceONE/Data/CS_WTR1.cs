using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class CS_WTR1
    {
        public string ItemCode { get; set; }
        public int DocNum { get; set; }
        public float Quantity { get; set; }
        public string Filler { get; set; }
        public string ToWhsCode { get; set; }
        public string BatchNum { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}