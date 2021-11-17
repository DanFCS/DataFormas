using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OITM_out_Data
    {
        public static List<TablaInt_OITM_out> Listar_Articulos(TopSkip objParam)
        {
            //string itm;

            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            List<TablaInt_OITM_out> Obj_list_OITM_out = new List<TablaInt_OITM_out>();
                        
            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOITM = "SELECT \"ItemCode\", \"ItemName\", \"VATLiable\", \"U_Codigo_IVA\", \"U_Cabys\", \"ManBtchNum\" FROM \"10099_BDDOCS\".\"OITM\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";

                OdbcCommand cmd = new OdbcCommand(queryOITM, conn);
               
                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"OITM\" SET \"Status\" = 1  WHERE \"ItemCode\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OITM_out E = new TablaInt_OITM_out(); //Obj Encabezado

                            E.ItemCode = dr["ItemCode"].ToString();
                            E.ItemName = dr["ItemName"].ToString();
                            E.VATLiable = dr["VATLiable"].ToString();
                            E.U_Codigo_IVA = dr["U_Codigo_IVA"].ToString();
                            E.U_Cabys = dr["U_Cabys"].ToString();
                            E.ManBtchNum = dr["ManBtchNum"].ToString();


                            Obj_list_OITM_out.Add(E); //Ingreso de los encabezados a la lista

                            //cm.Parameters.Add(new OdbcParameter("@num", E.ItemCode)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 

                        }

                    }

                    conn.Close();
                    return Obj_list_OITM_out;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST de la tabla OITM : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OITM_out;
                }

            }

        }

    }

}