using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_ITM1_out
    {
        public string ItemCode { get; set; }
        public int PriceList { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}