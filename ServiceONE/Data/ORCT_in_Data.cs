using ServiceONE.Models;
using ServiceONE.Data;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class ORCT_in_Data
    {
        List<InfoInsert> listInst_ok = new List<InfoInsert>();

        public List<InfoInsert> NuevoPagoRecibido(List<TablaInt_ORCT_in> listaPagosRecibidos)
        {

            RegistroLogClass objRegistraLog = new RegistroLogClass(); //Log en caso de error  

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {
                    conn.Open();

                    foreach (TablaInt_ORCT_in obj in listaPagosRecibidos)
                    {
                        //Validad el formato de los campos númericos
                        if (obj.DocNum >= 1 & obj.CashSum >= 0 & obj.CreditSum >= 0 & obj.TrsfrSum >= 0) // Validad el formato de los campos númericos
                        {
                            string query = "INSERT INTO \"10031_BDDOCS\".\"PAGOS_EN\" (\"DocNum\",\"DocDate\",\"CardCode\",\"CashSum\",\"CreditSum\",\"Trsfrsum\",\"ID_Factura\",\"Origen_documento\") " +
                            "values(" + obj.DocNum + ",'" + obj.DocDate.ToString("yyyy-MM-dd") + "','" + obj.CardCode + "'," + obj.CashSum + "," + obj.CreditSum + "," + obj.TrsfrSum + "," + obj.ID_Factura + ",'" + obj.Origen_documento + "')";

                            OdbcCommand CmD = new OdbcCommand(query, conn);
                            try
                            {

                                CmD.ExecuteReader();
                                foreach (TablaInt_RCT2_in dl in obj.Detalle)
                                {                                
                                      query = "insert into \"10031_BDDOCS\".\"PAGOS_DET\" (\"DocNum\",\"PaidSum\",\"ID_Factura\",\"Origen_documento\") " +
                                    "values(" + dl.DocNum + "," + dl.PaidSum + "," + dl.ID_Factura + ",'" + dl.Origen_documento + "')";
                                    CmD = new OdbcCommand(query, conn);
                                    CmD.ExecuteReader();
                                }

                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                conn.Close();
                                objRegistraLog.Graba("Error en el POST al insertar el Detalle PAGOS_EN-PAGOS_DET : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                            }
                        }

                        else
                        {
                            ValidarCamposNumericos(obj.DocNum, obj.CashSum, obj.CreditSum, obj.TrsfrSum);

                        }

                    } // end del foreach

                    foreach (TablaInt_ORCT_in obj in listaPagosRecibidos) //For para consultar cuales documentos  fueron innsertados correctamente
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DocNum\", \"DocEntry\", \"ID_Factura\", \"Origen_documento\" FROM \"10031_BDDOCS\".\"PAGOS_EN\" WHERE \"DocNum\" = " + obj.DocNum + "";
                        OdbcCommand CmD = new OdbcCommand(queryCons, conn);
                        using (OdbcDataReader dr = CmD.ExecuteReader())
                            while (dr.Read())
                            {
                                try
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DocNum"]);
                                    objinfInsrt.DocEntry = Convert.ToInt32(dr["DocEntry"]);
                                    objinfInsrt.ID_Factura = Convert.ToInt32(dr["ID_Factura"]);
                                    objinfInsrt.Origen_doc = dr["Origen_documento"].ToString();
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
                    objRegistraLog.Graba("Error en el POST de las tablas PAGOS_EN-PAGOS_DET : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    return listInst_ok;
                }
            }
        }

        public void ValidarCamposNumericos(int iDocNum, float iCashSum, float iCredSum, float iTrsfrSum)
        {
            InfoInsert objinfInsrt = new InfoInsert();
            int D = iDocNum;
            float Ca = iCashSum, Cr = iCredSum, T = iTrsfrSum;

            if (D <= 0)
            {
                objinfInsrt.DocNum = D;
                objinfInsrt.DocEntry = -1;
                objinfInsrt.Estado = "Sin Insertar: El Campo *DocNum* con formato inválido";
                listInst_ok.Add(objinfInsrt);
            }
            if (Ca <= -1)
            {
                objinfInsrt.DocNum = D;
                objinfInsrt.DocEntry = -1;
                objinfInsrt.Estado = "Sin Insertar: El Campo *CashSum* con formato inválido";
                listInst_ok.Add(objinfInsrt);
            }
            if (Cr <= -1)
            {
                objinfInsrt.DocNum = D;
                objinfInsrt.DocEntry = -1;
                objinfInsrt.Estado = "Sin Insertar: El Campo *CreditSum* con formato inválido";
                listInst_ok.Add(objinfInsrt);
            }
            if (T <= -1)
            {
                objinfInsrt.DocNum = D;
                objinfInsrt.DocEntry = -1;
                objinfInsrt.Estado = "Sin Insertar: El Campo *TrsfrSum* con formato inválido";
                listInst_ok.Add(objinfInsrt);
            }

        }
    }
}
