using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OIGN_in_Data
    {
        public List<InfoInsert> NuevaEntradaMercancia(List<TablaInt_OIGN_in> listaEntradaMercancia)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            List<InfoInsert> listInst_ok = new List<InfoInsert>();

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();

                    foreach (TablaInt_OIGN_in obj in listaEntradaMercancia)
                    {
                        string query = "insert into \"10099_BDDOCS\".\"ENTMER_EN\" (\"DocNum\",\"DocDate\",\"Comments\") " +
                                    "values(" + obj.DocNum + ",'" + obj.DocDate.ToString("yyyy-MM-dd") + "','" + obj.Comments + "')";
                        OdbcCommand CmD = new OdbcCommand(query, conn);
                        try
                        {


                            CmD.ExecuteReader();
                            foreach (TablaInt_IGN1_in dl in obj.Detalle)
                            {
                                query = "insert into \"10099_BDDOCS\".\"ENTMER_DET\" (\"DocNum\",\"ItemCode\",\"Quantity\",\"PriceBefDi\",\"WhsCode\",\"BatchNum\") " +
                                     "values(" + dl.DocNum + ",'" + dl.ItemCode + "'," + dl.Quantity + "," + dl.PriceBefDi + ",'" + dl.WhsCode + "','" + dl.BatchNum + "')";
                                CmD = new OdbcCommand(query, conn);
                                CmD.ExecuteReader();
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            objRegistraLog.Graba("Error en el POST al insertar el Detalle ENTMER_EN-ENTMER_DET : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                        }
                    }// foreach (CS_OWTR transfer in listaTranferencias)

                    foreach (TablaInt_OIGN_in obj in listaEntradaMercancia) //For para consultar cuales documentos fueron innsertados correctamente
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DocNum\", \"DocEntry\" FROM \"10099_BDDOCS\".\"ENTMER_EN\" WHERE \"DocNum\" = " + obj.DocNum + "";
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
                    conn.Close();
                    objRegistraLog.Graba("Error en el POST de las tablas ENTMER_EN-ENTMER_DET : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    return listInst_ok;

                }

            }
        }
    }
}