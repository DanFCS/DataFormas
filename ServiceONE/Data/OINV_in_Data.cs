using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OINV_in_Data
    {

        public List<InfoInsert> NuevaFacturaCliente(List<TablaInt_OINV_in> listaFacts)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
             List<InfoInsert> listInst_ok = new List<InfoInsert>();

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();

                    foreach (TablaInt_OINV_in obj in listaFacts)
                    {

                        string query = "insert into \"10031_BDDOCS\".\"FE_EN\" (\"CardCode\",\"DocNum\",\"DocDate\",\"DocRate\",\"U_Tipo_Doc\",\"U_Clave\",\"U_IdDocElect\",\"DocTotal\", \"Tipo_identificacion\", \"ID_Factura\", \"Origen_documento\", \"U_Num_Hab\", \"DocSubType\", \"U_Tipo_Doc_Elec\", \"GroupNum\", \"DiscSum\", \"VatSum\", \"DocCurrCode\", \"CardName\", \"MailAdress\", \"E_mail\") " +
                        "values ('" + obj.CardCode + "'," + obj.DocNum + ",'" + obj.DocDate.ToString("yyyy-MM-dd") + "'," + obj.DocRate + "," + obj.U_Tipo_Doc + ",'" + obj.U_Clave + "','" + obj.U_IdDocElect + "'," + obj.DocTotal + ", " + obj.Tipo_identificacion + "," + obj.ID_Factura + ",'" + obj.Origen_documento + "','" + obj.U_Num_Hab + "','" + obj.DocSubType + "','" + obj.TipoDocElec + "', " + obj.GroupNum + ", " + obj.DiscSum + ", " + obj.VatSum + ",'" + obj.CardName + "','" + obj.DocCurrCode + "','" + obj.MailAdress + "','" + obj.E_mail + "')";
                        OdbcCommand CmD = new OdbcCommand(query, conn);
                        try
                          {


                            CmD.ExecuteReader();
                            foreach (TablaInt_INV1_in dl in obj.Detalle)
                            {
                                query = "insert into \"10031_BDDOCS\".\"FE_DET\" (\"DocNum\",\"ItemCode\",\"Quantity\",\"PriceBefDi\",\"DiscPrcnt\", \"TaxCode\", \"TaxCode_Servicio\", \"WhsCode\",\"U_Cabys\",\"OcrCode\",\"ID_Factura\", \"Origen_documento\", \"LineTotal\", \"OcrCode2\", \"ItemName\", \"UnitMsr\" ,\"U_Servicio\")" +
                                 "values(" + dl.DocNum + ",'" + dl.ItemCode + "'," + dl.Quantity + "," + dl.PriceBefDi + "," + dl.DiscPrcnt + ",'" + dl.TaxCode + "','" + dl.TaxCode_Servicio + "','" + dl.WhsCode + "','" + dl.U_Cabys + "','" + dl.OcrCode + "','" + dl.ID_Factura + "','" + dl.Origen_documento + "'," + dl.LineTotal + ",'" + dl.OcrCode2 + "','" + dl.ItemName + "','" + dl.UnitMsr + "','" + dl.U_Servicio + "')";
                                CmD = new OdbcCommand(query, conn);
                                CmD.ExecuteReader();
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            objRegistraLog.Graba("Error en el POST al insertar el Detalle FE_EN-FE_DET : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                        }
                    }// foreach (CS_OWTR transfer in listaTranferencias)

                    foreach (TablaInt_OINV_in obj in listaFacts) //For para consultar cuales documentos fueron innsertados correctamente
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DocNum\", \"DocEntry\", \"U_IdDocElect\", \"ID_Factura\", \"Origen_documento\" FROM \"10031_BDDOCS\".\"FE_EN\" WHERE \"DocNum\" = " + obj.DocNum + "";
                        OdbcCommand CmD = new OdbcCommand(queryCons, conn);
                        using (OdbcDataReader dr = CmD.ExecuteReader())
                            while (dr.Read())
                            {
                                try
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DocNum"]);
                                    objinfInsrt.DocEntry = Convert.ToInt32(dr["DocEntry"]);
                                    objinfInsrt.IdDocElect= dr["U_IdDocElect"].ToString();
                                    objinfInsrt.ID_Factura= Convert.ToInt32(dr["ID_Factura"]);
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
                    objRegistraLog.Graba("Error en el POST de las tablas FE_EN-FE_DET : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                     return listInst_ok;
                }

            }
        }
    }
}