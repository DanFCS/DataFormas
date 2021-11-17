using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class COSTOS_out_Data
    {
        public static List<TablaInt_COSTOS_out> Listar_CostArticulos(TopSkip objParam)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_COSTOS_out> Obj_list_COSTOS_out = new List<TablaInt_COSTOS_out>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryCOSTOS = "SELECT \"ItemCode\", \"AvgPrice\" FROM \"10099_BDDOCS\".\"COSTOS\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";
                    //"SELECT \"ItemCode\", \"AvgPrice\" FROM \"10099_BDDOCS\".\"COSTOS\" WHERE \"Status\" = 0 ";

                OdbcCommand cmd = new OdbcCommand(queryCOSTOS, conn);
             
                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //String queryActStatus = "UPDATE \"10099_BDDOCS\".\"COSTOS\" SET \"Status\" = 1  WHERE \"ItemCode\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_COSTOS_out E = new TablaInt_COSTOS_out(); //Obj Encabezado

                            E.ItemCode = dr["ItemCode"].ToString();
                            E.AvgPrice = Convert.ToInt32(dr["AvgPrice"]);

                            Obj_list_COSTOS_out.Add(E); //Ingreso de los encabezados a la lista
                           

                            //cm.Parameters.Add(new OdbcParameter("@num", E.ItemCode)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 

                        }                      
                         
                    }

                    conn.Close();
                    return Obj_list_COSTOS_out;

                }
                catch (Exception ex)
                {
                    TablaInt_COSTOS_out E = new TablaInt_COSTOS_out();
                    E.Message = ex.Message;
                    Obj_list_COSTOS_out.Add(E);
                   
                    objRegistraLog.Graba("Error en el POST de la tabla COSTOS : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    return Obj_list_COSTOS_out;

                }

            }

        }
    }
}