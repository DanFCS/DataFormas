using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OINV_out_Data
    {
        public static List<TablaInt_OINV> Listar_Facturas(TopSkip objParam)
        {
            //string itm;
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_OINV> Obj_list_OINV_out = new List<TablaInt_OINV>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOITM = "SELECT * FROM \"10099_BDDOCS\".\"FAC_OPEN\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";

                OdbcCommand cmd = new OdbcCommand(queryOITM, conn);

                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"FAC_OPEN\" SET \"Status\" = 1  WHERE \"DocEntry\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OINV E = new TablaInt_OINV(); //Obj Encabezado

                            E.CardCode = dr["CardCode"].ToString();
                            E.CARDNAME = dr["CARDNAME"].ToString();
                            E.DocNum =  Convert.ToInt32( dr["DocNum"].ToString());
                            E.SaldoPend = Convert.ToDouble( dr["SaldoPend"].ToString());
                            E.DocDate = Convert.ToDateTime( dr["DocDate"].ToString());
                            E.Status = dr["Status"].ToString();
                            E.MSG_ERR= dr["MSG_ERR"].ToString();


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
                    objRegistraLog.Graba("Error en el POST de la tabla OINV : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OINV_out;
                }

            }

        }
    }
}