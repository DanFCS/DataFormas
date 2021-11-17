using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OITW
    {
        public string ItemCode { get; set; }
        public string  WhsCode { get; set; }
        public double OnHand { get; set; }
        public string Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}