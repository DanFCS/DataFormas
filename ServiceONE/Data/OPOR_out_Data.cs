using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OPOR_out_Data
    {
        public static List<TablaInt_OPOR_out> Listar_OrdCompra(TopSkip objParam)
        {       

            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_OPOR_out> Obj_list_OPOR_out = new List<TablaInt_OPOR_out>(); //list Encabezado   
            int DnEnc = 0;
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOPOR = "SELECT \"DocNum\", \"CardCode\", \"DocDate\", \"DocRate\",\"DiscPrcnt\" FROM \"10099_BDDOCS\".\"OPOR\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " "; //Query Encabezado
                string queryPOR1 = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"DiscPrcnt\", \"TaxCode\" , \"WhsCode\" FROM \"10099_BDDOCS\".\"POR1\" WHERE \"DocNum\" =?  "; //Queey Detalle


                OdbcCommand CmD = new OdbcCommand(queryOPOR, conn); // llama al Query Encabezado con la conexion
               

                try
                {
                    conn.Open();
                   
                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        //Llenado de la lista Encabezado
                        while (dr.Read())
                        {
                            //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"OPOR\" SET \"Status\" = 1  WHERE \"DocNum\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OPOR_out E = new TablaInt_OPOR_out(); //Obj Encabezado

                            DnEnc = Convert.ToInt32(dr["DocNum"]);
                            E.DocNum = Convert.ToInt32(dr["DocNum"]);
                            E.CardCode = dr["CardCode"].ToString();

                            //valida si la fecha no viene nula para que pueda ser convertida a string
                            if (!DBNull.Value.Equals(dr["DocDate"])) { E.DocDate = Convert.ToDateTime(dr["DocDate"].ToString()); }

                            E.DocRate = Convert.ToInt32(dr["DocRate"]);
                            E.DiscPrcnt = Convert.ToInt32(dr["DiscPrcnt"]);
                            E.Detalle = new List<TabaInt_POR1_out>();



                            //cm.Parameters.Add(new OdbcParameter("@num", DnEnc)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status

                            OdbcCommand cmc = new OdbcCommand(queryPOR1, conn);                            
                            cmc.Parameters.Add(new OdbcParameter("@num", DnEnc));//Pasa el valor de DocNum al query Deta  
                            using (OdbcDataReader dtr = cmc.ExecuteReader())
                                //Llenado de la lista Detalle
                                while (dtr.Read())
                                {
                                    //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"POR1\" SET \"Status\" = 1  WHERE \"DocNum\" =  ? "; //Query Actualiza el campo Status a 1
                                    //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                                    TabaInt_POR1_out D = new TabaInt_POR1_out(); // Obj Detalle

                                    D.DocNum = Convert.ToInt32(dtr["DocNum"]);
                                    D.ItemCode = dtr["ItemCode"].ToString();
                                    D.Quantity = Convert.ToInt32(dtr["Quantity"]);
                                    D.PriceBefDi = Convert.ToInt32(dtr["PriceBefDi"]);
                                    D.DiscPrcnt = Convert.ToInt32(dtr["DiscPrcnt"]);
                                    D.TaxCode = dtr["TaxCode"].ToString();
                                    D.WhsCode = dtr["WhsCode"].ToString();
                                    E.Detalle.Add(D);                                   

                                    //cm.Parameters.Add(new OdbcParameter("@num", D.DocNum)); //Pasa el valor de DocNum al query que actuliza Status a 1
                                    //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 
                                }
                            

                            Obj_list_OPOR_out.Add(E); //Ingreso de los encabezados a la lista
                            
                        }
                    
                        conn.Close();
                        return Obj_list_OPOR_out; //Retorno el la lista de encabezados con sus detalles ya ingresados
                    }

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST de las tablas OPOR-POR1 : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    return Obj_list_OPOR_out; //Retorno el la lita vacía en caso no funcione el try
                }

            }
        }
    }
}