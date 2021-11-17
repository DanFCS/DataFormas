using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class ORPC_in_Data
    {
        public List<InfoInsert> NuevaNcProveedor(List<TablaInt_ORPC_in> listaNCsProveedor)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            List<InfoInsert> listInst_ok = new List<InfoInsert>();

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {
                                   
                conn.Open();

                foreach (TablaInt_ORPC_in obj in listaNCsProveedor)
                {
                    string query = "insert into \"10099_BDDOCS\".\"NC_PROVE_EN\" (\"CardCode\",\"DocNum\",\"DocDate\",\"DocRate\",\"DiscPrcnt\",\"NumAtCard\") " +
                                 "values('" + obj.CardCode + "'," + obj.DocNum + ",'" + obj.DocDate.ToString("yyyy-MM-dd") + "'," + obj.DocRate + "," + obj.DiscPrcnt + ",'" + obj.NumAtCard + "')";
                    OdbcCommand CmD = new OdbcCommand(query, conn);
                    try
                    {


                        CmD.ExecuteReader();
                        foreach (TablaInt_RPC1_in dl in obj.Detalle)
                        {
                             query = "insert into \"10099_BDDOCS\".\"NC_PROVE_DET\" (\"DocNum\",\"ItemCode\",\"Quantity\",\"PriceBefDi\",\"DiscPrcnt\",\"TaxCode\",\"WhsCode\",\"BatchNum\") " +
                                "values(" + dl.DocNum + ",'" + dl.ItemCode + "'," + dl.Quantity + "," + dl.PriceBefDi + "," + dl.DiscPrcnt + ",'" + dl.TaxCode + "','" + dl.WhsCode + "','" + dl.BatchNum + "')";
                            CmD = new OdbcCommand(query, conn);
                            CmD.ExecuteReader();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        objRegistraLog.Graba("Error en el POST al insertar el Detalle NC_PROVE_EN-NC_PROVE_EN : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                    }
                }// foreach (CS_OWTR transfer in listaTranferencias)

                    foreach (TablaInt_ORPC_in obj in listaNCsProveedor)
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DocNum\", \"DocEntry\" FROM \"10099_BDDOCS\".\"NC_PROVE_EN\" WHERE \"DocNum\" = " + obj.DocNum + "";
                        OdbcCommand CmD = new OdbcCommand(queryCons, conn);
                        using (OdbcDataReader dr = CmD.ExecuteReader())
                            while (dr.Read())
                            {
                                try
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DocNum"]);
                                    objinfInsrt.DocEntry = Convert.ToInt32(dr["DocEntry"]);
                                    objinfInsrt.Estado = "OK";
                                    listInst_ok.Add(objinfInsrt);
                                }
                                catch (Exception exp)
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DocNum"]);
                                    objinfInsrt.DocEntry = -1;
                                    objinfInsrt.Estado = "Sin Insertar " + exp.Message;
                                    listInst_ok.Add(objinfInsrt);
                                }

                            }
                    }

                    conn.Close();
                    return listInst_ok;

                }
                catch (Exception e)
                {
                    objRegistraLog.Graba("Error en el POST de las tablas NC_PROVE_EN-NC_PROVE_EN : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return listInst_ok;
                }
               
            }
        }
    }
}