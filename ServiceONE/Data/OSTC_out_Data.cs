using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OSTC_out_Data
    {
        public static List<TablaInt_OSTC_out> Listar_ListaImpuestos(TopSkip objParam)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_OSTC_out> Obj_list_OSTC_out = new List<TablaInt_OSTC_out>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOSTC = "SELECT \"Code\", \"Name\", \"Rate\" FROM \"10099_BDDOCS\".\"OSTC\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + "";
                
                OdbcCommand cmd = new OdbcCommand(queryOSTC, conn);

                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //String queryActStatus = "UPDATE \"10099_BDDOCS\".\"OSTC\" SET \"Status\" = 1  WHERE \"Code\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OSTC_out E = new TablaInt_OSTC_out(); //Obj Encabezado

                            E.Code = dr["Code"].ToString();
                            E.Name = dr["Name"].ToString();
                            E.Rate = Convert.ToInt32(dr["Rate"]);

                            Obj_list_OSTC_out.Add(E); //Ingreso de los encabezados a la lista

                            //cm.Parameters.Add(new OdbcParameter("@num", E.Code)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 

                        }

                    }

                    conn.Close();
                    return Obj_list_OSTC_out;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST de la tabla OSTC : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OSTC_out;

                }

            }

        }
    }
}