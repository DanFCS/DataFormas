using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class ORIN_in_Data
    {
        public List<InfoInsert> NuevaNC(List<TablaInt_ORIN_in> listaNCs)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
              List<InfoInsert> listInst_ok = new List<InfoInsert>();

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {
                   

                    conn.Open();

                    foreach (TablaInt_ORIN_in obj in listaNCs)
                    {
                       string query = "INSERT INTO \"10031_BDDOCS\".\"NC_EN\" (\"CardCode\", \"Tipo_identificacion\", \"DocNum\", \"ID_Factura\", \"Origen_documento\", \"DocDate\", \"DocRate\", \"U_Clave\", \"U_IdDocElect\", \"U_Num_Hab\", \"DocTotal\", \"DocRef\") " +
                         "values ('" + obj.CardCode + "'," + obj.Tipo_identificacion + "," + obj.DocNum + "," + obj.ID_Factura + ",'" + obj.Origen_documento + "','" + obj.DocDate.ToString("yyyy-MM-dd") + "'," + obj.DocRate + ",'" + obj.U_Clave + "','" + obj.U_IdDocElect + "','" + obj.U_Num_Hab + "'," + obj.DocTotal + "," + obj.DocRef + ")";
                        OdbcCommand CmD = new OdbcCommand(query, conn);
                        try
                        {


                            CmD.ExecuteReader();
                            foreach (TablaInt_RIN1_in dl in obj.Detalle)
                            {
                                query = "INSERT INTO \"10031_BDDOCS\".\"NC_DET\" (\"DocNum\",\"ItemCode\",\"Quantity\",\"PriceBefDi\",\"DiscPrcnt\",\"TaxCode\",\"TaxCode_Servicio\",\"WhsCode\",\"U_Cabys\",\"OcrCode\") " +
                               "values(" + dl.DocNum + ",'" + dl.ItemCode + "'," + dl.Quantity + "," + dl.PriceBefDi + "," + dl.DiscPrcnt + ",'" + dl.TaxCode + "','" + dl.TaxCode_Servicio + "','" + dl.WhsCode + "','" + dl.U_Cabys + "','" + dl.OcrCode + "')";
                                CmD = new OdbcCommand(query, conn);
                                CmD.ExecuteReader();
                            }

                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            Console.WriteLine(ex.Message);
                            objRegistraLog.Graba("Error en el POST al insertar el Detalles NC_EN-NC_DET : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                        }
                    }// foreach (CS_OWTR transfer in listaTranferencias)


                    foreach (TablaInt_ORIN_in obj in listaNCs) //For para consultar cuales documentos fueron innsertados correctamente
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DocNum\", \"DocEntry\", \"U_IdDocElect\", \"ID_Factura\", \"Origen_documento\" FROM \"10031_BDDOCS\".\"NC_EN\" WHERE \"DocNum\" = " + obj.DocNum + "";
                        OdbcCommand CmD = new OdbcCommand(queryCons, conn);
                        using (OdbcDataReader dr = CmD.ExecuteReader())
                            while (dr.Read())
                            {
                                try
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DocNum"]);
                                    objinfInsrt.DocEntry = Convert.ToInt32(dr["DocEntry"]);
                                    objinfInsrt.IdDocElect = dr["U_IdDocElect"].ToString();
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
                    objRegistraLog.Graba("Error en el POST de las tablas NC_EN-NC_DET : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                     return listInst_ok;
                }
            }

        }
    }
}