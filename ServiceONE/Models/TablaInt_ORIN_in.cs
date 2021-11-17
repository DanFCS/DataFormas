using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_ORIN_in
    { 

        public string CardCode { get; set; }
        public int Tipo_identificacion { get; set; }
        public int DocNum { get; set; }
        public int ID_Factura { get; set; }
        public string Origen_documento { get; set; }       
        public DateTime DocDate { get; set; }
        public float DocRate { get; set; }        
        public string U_Clave { get; set; }
        public string U_IdDocElect { get; set; }
        public string U_Num_Hab { get; set; }
        public float DocTotal { get; set; }
        public int DocRef { get; set; }
        
        
        public List<TablaInt_RIN1_in> Detalle { get; set; }
    }
}