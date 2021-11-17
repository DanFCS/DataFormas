using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OWTR_out
    {
        //Campos de Encabezado
        public  int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public string Filler { get; set; }
        public string ToWhsCode { get; set; }
        public List<TablaInt_WTR1_out> Detalle { get; set; }





    }
}