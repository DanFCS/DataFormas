using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceONE.Models
{
    public class TablaInt_OINV_in
    {
       
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public float DocRate { get; set; }        
        public int U_Tipo_Doc { get; set; }
        public string U_Clave { get; set; }
        public string U_IdDocElect { get; set; }
        public float DocTotal { get; set; }
        public string DocSubType { get; set; }        
        public int Tipo_identificacion { get; set; }
        public int ID_Factura { get; set; }
        public string Origen_documento { get; set; }
        public string U_Num_Hab { get; set; }

        public string TipoDocElec { get; set; }
        public int GroupNum { get; set; }
        public float DiscSum { get; set; }
        public float VatSum { get; set; }
        public string DocCurrCode { get; set; }
        public string CardName { get; set; }
        public string MailAdress { get; set; }
        public string E_mail { get; set; }
    
        public List<TablaInt_INV1_in> Detalle { get; set; }

    

    }
}