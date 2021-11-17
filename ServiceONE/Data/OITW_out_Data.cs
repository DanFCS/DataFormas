using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OITW_out_Data
    {
        public static List<TablaInt_OITW> Lista_Inventario_Almacen(TopSkip objParam)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            //string itm;
            List<TablaInt_OITW> Obj_list_OITW_out = new List<TablaInt_OITW>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;


            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOITW = "SELECT * FROM \"10099_BDDOCS\".\"INVENT\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";

                OdbcCommand cmd = new OdbcCommand(queryOITW, conn);

                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //String queryActStatus = "UPDATE \"10099_BDDOCS\".\"INVENT\" SET \"Status\" = 1  WHERE \"DocEntry\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OITW E = new TablaInt_OITW(); //Obj Encabezado

                            E.ItemCode = dr["ItemCode"].ToString();
                            E.WhsCode = dr["WhsCode"].ToString();
                            E.OnHand = Convert.ToDouble(dr["OnHand"].ToString());
                            E.Status = dr["Status"].ToString();
                            E.MSG_ERR = dr["MSG_ERR"].ToString();


                            Obj_list_OITW_out.Add(E); //Ingreso de los encabezados a la lista

                            //cm.Parameters.Add(new OdbcParameter("@num", dr["DocEntry"].ToString())); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 

                        }

                    }

                    conn.Close();
                    return Obj_list_OITW_out;

                }
                catch (Exception ex)
                {
                    //TablaInt_OITW E = new TablaInt_OITW();
                    //E.MSG_ERR = ex.Message;
                    //String MSG = ex.Message;
                    //Obj_list_OINV_out.Add(E);

                    objRegistraLog.Graba("Error en el POTS de la tabla OITW : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OITW_out;
                }

            }

        }
    }
}