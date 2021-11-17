using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class ITM1_out_Data
    {
        public static List<TablaInt_ITM1_out> Lista_Precios(TopSkip objParam)
        {
            //string itm;
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            
            List<TablaInt_ITM1_out> Obj_list_OINV_out = new List<TablaInt_ITM1_out>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;


            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryITM1 = "SELECT * FROM \"10099_BDDOCS\".\"LIST_PRECIOS\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";

                OdbcCommand cmd = new OdbcCommand(queryITM1, conn);

                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //String queryActStatus = "UPDATE \"10099_BDDOCS\".\"LIST_PRECIOS\" SET \"Status\" = 1  WHERE \"DocEntry\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_ITM1_out E = new TablaInt_ITM1_out(); //Obj Encabezado

                            E.ItemCode = dr["ItemCode"].ToString();
                            E.PriceList = Convert.ToInt32( dr["PriceList"].ToString());
                            E.Price = Convert.ToDouble(dr["Price"].ToString());
                            //E.Status = dr["SaldoPend"].ToString();
                            //E.MSG_ERR = dr["DocDate"].ToString();
                           

                            Obj_list_OINV_out.Add(E); //Ingreso de los encabezados a la lista

                            //cm.Parameters.Add(new OdbcParameter("@num", dr["DocEntry"].ToString())); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 

                        }

                    }

                    conn.Close();
                    return Obj_list_OINV_out;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST de la tabla ITM1 : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    String MSG = ex.ToString();
                    conn.Close();
                    return Obj_list_OINV_out;
                }

            }

        }
    }
}