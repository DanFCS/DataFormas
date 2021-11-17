using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OCRD_out_Data
    {
        public static List<TablaInt_OCRD_out> Listar_SocNegocios(TopSkip objParam)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_OCRD_out> Obj_list_OCRD_out = new List<TablaInt_OCRD_out>();
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                string queryOCRD = "SELECT \"CardCode\", \"CardName\", \"CardType\" FROM \"10099_BDDOCS\".\"OCRD\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " ";

          
        OdbcCommand cmd = new OdbcCommand(queryOCRD, conn);

                try
                {
                    conn.Open();

                    using (OdbcDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"OCRD\" SET \"Status\" = 1  WHERE \"CardCode\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                            TablaInt_OCRD_out E = new TablaInt_OCRD_out(); //Obj Encabezado

                            E.CardCode = dr["CardCode"].ToString();
                            E.CardName = dr["CardName"].ToString();
                            E.CardType = dr["CardType"].ToString();

                            Obj_list_OCRD_out.Add(E); //Ingreso de los encabezados a la lista

                            //cm.Parameters.Add(new OdbcParameter("@num", E.CardCode)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 
                                                       
                        }

                    }

                    conn.Close();
                    return Obj_list_OCRD_out;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST  de la tabla OCRD : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OCRD_out;

                }

           }

        }

    }
}