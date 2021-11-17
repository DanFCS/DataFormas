using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OINV
    {
        public string CardCode { get; set; }
        public string CARDNAME { get; set; }
        public int DocNum { get; set; }
        public double SaldoPend { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public string MSG_ERR { get; set; }
    }
}