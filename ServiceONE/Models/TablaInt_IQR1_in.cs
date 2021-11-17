using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_IQR1_in
    {
        public int DocNum { get; set; }
        public string ItemCode { get; set; }
        public float CountQty { get; set; }
        public string WhsCode { get; set; }
        public string BatchNum { get; set; }
        public int Doc_Entry_Sap { get; set; }
        public int Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}