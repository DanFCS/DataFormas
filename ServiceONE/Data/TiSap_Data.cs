using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using SAPbobsCOM;

namespace ServiceONE.Data
{
    public class TiSap_Data
    {
        // Insertar Facturas deudores a SAP --Daniel--
         public static void Insertar_Factura_OINV()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            try
            {
                int En_DN = 0;
                string NoteDedit = "";

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                      
                    string queryEn = "SELECT  \"CardCode\", \"DocNum\",  \"DocDate\", \"DocRate\", \"U_Clave\", \"U_IdDocElect\", \"DocTotal\", \"U_Tipo_Doc\" \"Tipo_identificacion\", \"ID_Factura\", \"Origen_documento\", \"U_Num_Hab\", \"DocSubType\", \"TaxCode\", \"TaxCode_Servicio\" FROM \"10031_BDDOCS\".\"FE_EN\" WHERE \"Status\" = 0 "; //Query Encabezado                    
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"DiscPrcnt\", \"TaxCode\", \"WhsCode\", \"U_Cabys\", \"OcrCode\", \"ID_Factura\", \"Origen_documento\" FROM \"10031_BDDOCS\".\"FE_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {
                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OINV = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInvoices);

                            while (dr.Read())
                            {
                                
                                myDoc_OINV = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInvoices);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                NoteDedit = dr["DocSubType"].ToString();

                                myDoc_OINV.CardCode = dr["CardCode"].ToString();
                                myDoc_OINV.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OINV.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OINV.DocRate = Convert.ToDouble(dr["DocRate"]);
                                
                                if (!DBNull.Value.Equals(dr["U_Tipo_Doc"])) { myDoc_OINV.UserFields.Fields.Item("U_Tipo_Doc").Value = Convert.ToInt32(dr["U_Tipo_Doc"]); }
                                myDoc_OINV.UserFields.Fields.Item("U_Clave").Value = dr["U_Clave"].ToString();
                                myDoc_OINV.UserFields.Fields.Item("U_IdDocElect").Value = dr["U_IdDocElect"].ToString();
                                myDoc_OINV.DocTotal = Convert.ToDouble(dr["DocTotal"]);
                               // myDoc_OINV.taxCode

                                myDoc_OINV.UserFields.Fields.Item("U_Tipo_identificacion").Value = Convert.ToInt32(dr["Tipo_identificacion"]);
                                myDoc_OINV.UserFields.Fields.Item("U_ID_Factura").Value = dr["ID_Factura"].ToString();
                                myDoc_OINV.UserFields.Fields.Item("U_Origen_doc").Value = dr["Origen_documento"].ToString();
                                myDoc_OINV.UserFields.Fields.Item("U_Num_Hab").Value = dr["U_Num_Hab"].ToString();
                                

                                if (NoteDedit == "ND") { myDoc_OINV.DocumentSubType = SAPbobsCOM.BoDocumentSubType.bod_DebitMemo; } //verifica si es Nota de Débito o no                                
                                

                                myDoc_OINV.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items; // en este caso factura articulos si fueran servicios cambia

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta                             

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OINV.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = dtr["DocNum"].ToString();
                                        myDoc_OINV.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OINV.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OINV.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OINV.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_OINV.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_OINV.Lines.UserFields.Fields.Item("U_TaxCode_Servicio").Value = dtr["TaxCode_Servicio"].ToString();
                                        myDoc_OINV.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        if (!DBNull.Value.Equals(dtr["U_Cabys"])) { myDoc_OINV.Lines.UserFields.Fields.Item("U_CABYS").Value = dtr["U_Cabys"].ToString(); }
                                        if (!DBNull.Value.Equals(dtr["OcrCode"])) { myDoc_OINV.Lines.CostingCode = dtr["OcrCode"].ToString(); }
                                        if (!DBNull.Value.Equals(dtr["ID_Factura"])) { myDoc_OINV.Lines.UserFields.Fields.Item("U_ID_Factura").Value = Convert.ToInt32(dtr["ID_Factura"]); }
                                        if (!DBNull.Value.Equals(dtr["Origen_doc"])) { myDoc_OINV.Lines.UserFields.Fields.Item("U_Origen_doc").Value = dtr["Origen_documento"].ToString(); }                                   
                                                                                
                                        myDoc_OINV.Lines.Add();

                                        String queryStatus = "UPDATE \"10031_BDDOCS\".\"FE_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1 tabla Detalle                                         
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OINV.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OINV: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey();// obtener el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10031_BDDOCS\".\"FE_EN\" SET \"DocEntry_SAP\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OINV: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

            }

        }
               
        // Insertar Nota de Créditos a SAP --Daniel--
         public static void Insertar_NotaCredito_ORIN()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 
            try
            {
                int En_DN = 0;

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    //  queryEn Trae les encabezados que el estado esta en 0 y tengan número de DocRef
                    string queryEn = "SELECT  P.\"DocNum\", P.\"DocDate\", P.\"CardCode\", P.\"Tipo_identificacion\", P.\"ID_Factura\", P.\"Origen_documento\",P.\"U_Num_Hab\", P.\"DocRate\", P.\"U_Clave\", P.\"U_IdDocElect\", P.\"DocTotal\", P.\"DocRef\", O.\"DocEntry\" FROM \"10031_BDDOCS\".\"NC_EN\" P INNER JOIN \"10028_COPPER_ES_TEST\".\"OINV\" O ON P.\"DocRef\" = O.\"DocNum\"  AND P.\"Status\" = 0";
                    string queryDet = "SELECT  D.\"DocNum\", D.\"ItemCode\", D.\"Quantity\", D.\"PriceBefDi\", D.\"DiscPrcnt\", D.\"TaxCode\", D.\"WhsCode\",  D.\"U_Cabys\", D.\"OcrCode\", I.\"LineNum\"  FROM \"10028_COPPER_ES_TEST\".\"INV1\" I LEFT JOIN  \"10031_BDDOCS\".\"NC_EN\" P ON P.\"DocRef\" = I.\"U_DocEntry_TI\" LEFT JOIN \"10031_BDDOCS\".\"NC_DET\" D  on P.\"DocNum\" = D.\"DocNum\"  WHERE  P.\"DocNum\" = ?  and I.\"ItemCode\" = D.\"ItemCode\"";

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_ORIN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);

                            while (dr.Read())
                            {
                                //if (!DBNull.Value.Equals(dr["DocRef"])) //válida que si venga el valor DocRef
                                //{                                    
                                myDoc_ORIN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);

                                En_DN = Convert.ToInt32(dr["DocNum"]);

                                myDoc_ORIN.CardCode = dr["CardCode"].ToString();
                                myDoc_ORIN.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                myDoc_ORIN.DocDate = Convert.ToDateTime(dr["DocDate"]);
                                myDoc_ORIN.DocRate = Convert.ToDouble(dr["DocRate"]);                               
                                myDoc_ORIN.UserFields.Fields.Item("U_Clave").Value = dr["U_Clave"].ToString();
                                myDoc_ORIN.UserFields.Fields.Item("U_IdDocElect").Value = dr["U_IdDocElect"].ToString();
                                myDoc_ORIN.DocTotal = Convert.ToDouble(dr["DocTotal"]);

                                myDoc_ORIN.UserFields.Fields.Item("U_Tipo_identificacion").Value = Convert.ToInt32(dr["Tipo_identificacion"]);
                                myDoc_ORIN.UserFields.Fields.Item("U_ID_Factura").Value = dr["ID_Factura"].ToString();
                                myDoc_ORIN.UserFields.Fields.Item("U_Origen_doc").Value = dr["Origen_documento"].ToString();
                                myDoc_ORIN.UserFields.Fields.Item("U_Num_Hab").Value = dr["U_Num_Hab"].ToString();

                               

                                myDoc_ORIN.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta                                                           

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_ORIN.Lines.BaseEntry = Convert.ToInt32(dr["DocEntry"]);  //validar que mo venga vacío
                                        myDoc_ORIN.Lines.BaseLine = Convert.ToInt32(dtr["LineNum"]);
                                        myDoc_ORIN.Lines.BaseType = 13;

                                        myDoc_ORIN.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = dtr["DocNum"].ToString();
                                        myDoc_ORIN.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_ORIN.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_ORIN.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_ORIN.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_ORIN.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_ORIN.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        myDoc_ORIN.Lines.UserFields.Fields.Item("U_Cabys").Value = dtr["U_Cabys"].ToString();
                                        myDoc_ORIN.Lines.CostingCode = dtr["OcrCode"].ToString();                                        
                                        

                                        // myDoc_ORIN.Lines.ActualBaseEntry
                                        //myDoc_ORIN.Lines.ActualBaseLine

                                        myDoc_ORIN.Lines.BatchNumbers.Add();
                                        myDoc_ORIN.Lines.Add();

                                        String queryStatus = "UPDATE \"10031_BDDOCS\".\"NC_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1 tabla detalle 
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();
                                    }

                                if (myDoc_ORIN.Add() != 0) //En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en ORIN: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10031_BDDOCS\".\"NC_EN\" SET \"DocEntry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }
                                //}
                                //else
                                //{                                    
                                //    string queryRef= "UPDATE \"10099_BDDOCS\".\"NC_EN\" SET \"MSG_ERR\" = 'El DocRef esta Vacio' WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el el mensaje sino viene el DocRef
                                //    OdbcCommand cm = new OdbcCommand(queryRef, conn);
                                //    cm.ExecuteNonQuery();
                                //}

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en ORIN: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }           

        // Insertar Pagos recibidos a SAP --Daniel--
         public static void Insertar_PagosRecibidos_ORCT()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 
            objRegistraLog.Graba("eNTRO  A ORCT: "  + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            try
            {
                int En_DN = 0;
                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
               
                    string queryEn = "SELECT  P.\"DocNum\", P.\"DocDate\", P.\"CardCode\", P.\"CashSum\",  P.\"CreditSum\", P.\"ID_Factura\", P.\"Origen_documento\", P.\"Trsfrsum\", O.\"DocEntry\" FROM \"10031_BDDOCS\".\"PAGOS_EN\" P  INNER JOIN \"10028_COPPER_ES_TEST\".\"OINV\" O ON P.\"Status\" = 0 AND P.\"DocNum\" = O.\"U_DocEntry_TI\" ";
                    string queryDet = "SELECT \"DocNum\", \"PaidSum\"  FROM \"10031_BDDOCS\".\"PAGOS_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Payments myDoc_ORCT = (SAPbobsCOM.Payments)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                            while (dr.Read())
                            {
                                myDoc_ORCT = (SAPbobsCOM.Payments)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                                En_DN = Convert.ToInt32(dr["DocNum"]);

                                myDoc_ORCT.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_ORCT.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_ORCT.CardCode = dr["CardCode"].ToString();
                                if (!DBNull.Value.Equals(dr["CashSum"])) { myDoc_ORCT.CashSum = Convert.ToDouble(dr["CashSum"]); }
                                myDoc_ORCT.TransferSum = Convert.ToDouble(dr["Trsfrsum"]);
                                myDoc_ORCT.UserFields.Fields.Item("U_ID_Factura").Value = Convert.ToInt32(dr["ID_Factura"]);
                                myDoc_ORCT.UserFields.Fields.Item("U_Origen_doc").Value = dr["Origen_documento"].ToString();
                                if (!DBNull.Value.Equals(dr["CreditSum"]))
                                {
                                    double credcard = myDoc_ORCT.CreditCards.CreditSum = Convert.ToDouble(dr["CreditSum"]);

                                    if (credcard != 0) //Campos quemados para la opción de tarjeta
                                    {
                                        myDoc_ORCT.CreditCards.CreditCard = 1;
                                        myDoc_ORCT.CreditCards.CreditCardNumber = "1234";
                                        myDoc_ORCT.CreditCards.CardValidUntil = Convert.ToDateTime("12-12-9999");
                                        myDoc_ORCT.CreditCards.ConfirmationNum = "153";
                                        myDoc_ORCT.CreditCards.VoucherNum = "DEFAULT";
                                    }

                                }

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 

                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdetalle = Convert.ToInt32(dtr["DocNum"]);


                                        myDoc_ORCT.Invoices.DocEntry = Convert.ToInt32(dr["DocEntry"]); //DocEntry de mi Tabla OINV                                                                                                                                                   
                                        myDoc_ORCT.Invoices.SumApplied = Convert.ToDouble(dtr["PaidSum"]);//PaidSum de mi Tabla OINV   

                                        myDoc_ORCT.Invoices.Add();

                                        String queryStatus = "UPDATE \"10031_BDDOCS\".\"PAGOS_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdetalle + ""; //Query Actualiza el campo Status a 1, Tabla Detalle
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_ORCT.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en ORCT: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {

                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10031_BDDOCS\".\"PAGOS_EN\" SET \"DocEntry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    // MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en ORCT: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }

    
        #region  Insert a SAP que no son de Buena Vista

        // Insertar transferencias a SAP --Daniel--
        public static void Insertar_Transferencia_OWTR()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0;

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT \"CardCode\", \"DOCNUM\", \"DocDate\", \"Filler\", \"ToWhsCode\" FROM \"10099_BDDOCS\".\"TRANSFER_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"ItemCode\", \"DocNum\", \"Quantity\", \"Filler\", \"ToWhsCode\" \"BatchNum\"  FROM \"10099_BDDOCS\".\"TRANSFER_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.StockTransfer myDoc_OWTR = (SAPbobsCOM.StockTransfer)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);

                            while (dr.Read())
                            {
                                myDoc_OWTR = (SAPbobsCOM.StockTransfer)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);

                                En_DN = Convert.ToInt32(dr["DOCNUM"]);
                                myDoc_OWTR.CardCode = dr["CardCode"].ToString();
                                myDoc_OWTR.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DOCNUM"]);
                                myDoc_OWTR.DocDate = Convert.ToDateTime(dr["DocDate"]);
                                myDoc_OWTR.FromWarehouse = dr["Filler"].ToString();
                                myDoc_OWTR.ToWarehouse = dr["ToWhsCode"].ToString();


                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Detalle 


                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {

                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OWTR.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OWTR.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                        myDoc_OWTR.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OWTR.Lines.FromWarehouseCode = dtr["Filler"].ToString();
                                        myDoc_OWTR.Lines.WarehouseCode = dtr["ToWhsCode"].ToString();
                                        myDoc_OWTR.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OWTR.Lines.BatchNumbers.Quantity = Convert.ToDouble(dtr["Quantity"]);

                                        myDoc_OWTR.Lines.BatchNumbers.Add();
                                        myDoc_OWTR.Lines.Add();


                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"TRANSFER_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1 tabla detalle 
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OWTR.Add() != 0) //En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OWTR: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey();// obtener el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"TRANSFER_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OWTR: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

            }

        }

        // Insertar Proveedoreos a SAP --Daniel--
        public static void Insertar_Proveedores_ORPC()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0;
                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    String queryEn = "SELECT  \"CardCode\", \"DocNum\",  \"DocDate\", \"DocRate\", \"DiscPrcnt\", \"NumAtCard\" FROM \"10099_BDDOCS\".\"NC_PROVE_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    String queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"DiscPrcnt\", \"TaxCode\", \"WhsCode\", \"BatchNum\"  FROM \"10099_BDDOCS\".\"NC_PROVE_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OPRC = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);

                            while (dr.Read())
                            {
                                myDoc_OPRC = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);

                                //En_DN = Convert.ToInt32(dr["DocNum"]);
                                //if (!DBNull.Value.Equals(dr["NumAtCard"])) //validar que no venga vacio para insertarlo
                                //{
                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OPRC.CardCode = dr["CardCode"].ToString();
                                myDoc_OPRC.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OPRC.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OPRC.DocRate = Convert.ToDouble(dr["DocRate"]);
                                myDoc_OPRC.DiscountPercent = Convert.ToDouble(dr["DiscPrcnt"]);
                                myDoc_OPRC.NumAtCard = dr["NumAtCard"].ToString();

                                myDoc_OPRC.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OPRC.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OPRC.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                        myDoc_OPRC.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OPRC.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OPRC.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_OPRC.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_OPRC.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        myDoc_OPRC.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OPRC.Lines.BatchNumbers.Quantity = Convert.ToDouble(dtr["Quantity"]);

                                        myDoc_OPRC.Lines.BatchNumbers.Add();
                                        myDoc_OPRC.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"NC_PROVE_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1, tabla Detalle
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OPRC.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en ORPC: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"NC_PROVE_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }
                                //}
                                //else // Aactualiza el Status y MSG_Err si el campo NumAtm viene vacio
                                //{
                                //    String queryStatus = "UPDATE \"10099_BDDOCS\".\"NC_PROVE_EN\" SET \"Status\" = 2, \"MSG_ERR\" = 'No se ingresaa a SAP ya que falta el campo *NumAtCard*'   WHERE \"DocNum\" =  ? "; //Query Actualiza el campo Status a 1
                                //    OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                //    cm.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query que actuliza Status a 1
                                //    cm.ExecuteNonQuery();
                                //}

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en ORPC: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }

        // Insertar Salida de Mercancia a SAP --Daniel--
        public static void Insertar_SalidaMercancia_OIGE()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0;

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT  \"DocDate\", \"DocNum\", \"Comments\" FROM \"10099_BDDOCS\".\"SALMER_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"WhsCode\", \"BatchNum\"  FROM \"10099_BDDOCS\".\"SALMER_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OIGE = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);

                            while (dr.Read())
                            {
                                myDoc_OIGE = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OIGE.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OIGE.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OIGE.Comments = dr["Comments"].ToString();

                                myDoc_OIGE.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 


                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OIGE.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                        myDoc_OIGE.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OIGE.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OIGE.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OIGE.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        myDoc_OIGE.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OIGE.Lines.BatchNumbers.Quantity = Convert.ToDouble(dtr["Quantity"]);

                                        myDoc_OIGE.Lines.BatchNumbers.Add();
                                        myDoc_OIGE.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"SALMER_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1, tabla Detalle
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OIGE.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OIGE: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {

                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"SALMER_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }


                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OIGE: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }

        // Insertar Entrada de Mercancía a SAP --Daniel--
        public static void Insertar_EntradaMercancía_OIGN()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0;
                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT  \"DocDate\", \"DocNum\", \"COMMENTS\" FROM \"10099_BDDOCS\".\"ENTMER_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"WhsCode\", \"BatchNum\", \"MnfDate\"  FROM \"10099_BDDOCS\".\"ENTMER_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OIGN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

                            while (dr.Read())
                            {
                                myDoc_OIGN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OIGN.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OIGN.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OIGN.Comments = dr["Comments"].ToString();


                                myDoc_OIGN.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdetalle = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OIGN.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                        myDoc_OIGN.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OIGN.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OIGN.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OIGN.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        myDoc_OIGN.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OIGN.Lines.BatchNumbers.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        if (!DBNull.Value.Equals(dtr["MnfDate"])) { myDoc_OIGN.Lines.BatchNumbers.ManufacturingDate = Convert.ToDateTime(dtr["MnfDate"]); }
                                        myDoc_OIGN.Lines.BatchNumbers.Add();
                                        myDoc_OIGN.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"ENTMER_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdetalle + ""; //Query Actualiza el campo Status a 1, tabla Detalle
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OIGN.Add() != 0) //En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OIGN: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                                else
                                {

                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"ENTMER_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OIGN: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt")); ;
            }

        }

        // Insertar Entrada Mercancía OC a SAP --Daniel--
        public static void Insertar_EntradaMercancía_OC_OPDN()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0;
                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT  P.\"CardCode\", P.\"DocNum\", P.\"DocDate\", P.\"DocRate\", P.\"DiscPrcnt\", P.\"NumOC\", O.\"DocEntry\" FROM \"10099_BDDOCS\".\"ENTMER_OC_EN\" P INNER JOIN \"10024_CHAMA\".\"OPOR\" O ON P.\"NumOC\" = O. \"U_DocEntry_TI\" AND P.\"Status\" = 0"; //Query Encabezado                                       
                    string queryDet = "SELECT D.\"DocNum\", D.\"ItemCode\", D.\"Quantity\", D.\"PriceBefDi\", D.\"DiscPrcnt\", D.\"TaxCode\", D.\"WhsCode\", D.\"BatchNum\", D.\"MnfDate\", I.\"LineNum\" FROM \"10024_CHAMA\".\"POR1\" I LEFT JOIN \"10099_BDDOCS\".\"ENTMER_OC_EN\" P ON P.\"NumOC\" = I.\"U_DocEntry_TI\" LEFT JOIN \"10099_BDDOCS\".\"ENTMER_OC_DET\" D on P.\"DocNum\" = D.\"DocNum\" WHERE P.\"DocNum\" = ? AND I.\"ItemCode\" = D.\"ItemCode\" ";//Query Detalle


                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OPDN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                            while (dr.Read())
                            {
                                myDoc_OPDN = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OPDN.CardCode = dr["CardCode"].ToString();
                                myDoc_OPDN.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OPDN.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OPDN.DocRate = Convert.ToDouble(dr["DocRate"]);
                                myDoc_OPDN.DiscountPercent = Convert.ToDouble(dr["DiscPrcnt"]);


                                myDoc_OPDN.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdetalle = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OPDN.Lines.BaseEntry = Convert.ToInt32(dr["DocEntry"]);  //validar que mo venga vacío
                                        myDoc_OPDN.Lines.BaseLine = Convert.ToInt32(dtr["LineNum"]);
                                        myDoc_OPDN.Lines.BaseType = 22;

                                        myDoc_OPDN.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OPDN.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                        myDoc_OPDN.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OPDN.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OPDN.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_OPDN.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_OPDN.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        myDoc_OPDN.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OPDN.Lines.BatchNumbers.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        if (!DBNull.Value.Equals(dtr["MnfDate"])) { myDoc_OPDN.Lines.BatchNumbers.ManufacturingDate = Convert.ToDateTime(dtr["MnfDate"]); }

                                        myDoc_OPDN.Lines.BatchNumbers.Add();
                                        myDoc_OPDN.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"ENTMER_OC_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdetalle + " "; //Query Actualiza el campo Status a 1, Tabla Detalle
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OPDN.Add() != 0) //En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OPDN: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"ENTMER_OC_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OPDN: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }

        // Insertar Contabilización de Stock a SAP --Daniel--
        public static void Insertar_Contabilizacion_OIQR()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 

            try
            {
                int En_DN = 0, A = 0, count = 0, C = 0, DNdetalla = 0;
                string B = "", Itm = "";

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT   \"DocDate\", \"DocNum\", \"Comments\" FROM \"10099_BDDOCS\".\"STOCK_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"CountQty\", \"WhsCode\", \"BatchNum\",( SELECT COUNT(*)  FROM  \"10099_BDDOCS\".\"STOCK_DET\" WHERE \"ItemCode\" =TI.\"ItemCode\" AND \"DocNum\" = TI.\"DocNum\") CONTEO, (SELECT  SUM(\"CountQty\")  FROM \"10099_BDDOCS\".\"STOCK_DET\" WHERE \"ItemCode\" = TI.\"ItemCode\" AND \"DocNum\" = TI.\"DocNum\") SUMA FROM \"10099_BDDOCS\".\"STOCK_DET\" TI WHERE \"DocNum\" = ? ORDER BY \"ItemCode\" "; //Query Detalle  ordenado para que no afecte al momento de hacer la insercion de lotes
                    string queryBatch = "SELECT \"CountQty\", \"BatchNum\", (SELECT SUM(\"CountQty\") FROM \"10099_BDDOCS\".\"STOCK_DET\" WHERE \"ItemCode\" = TI.\"ItemCode\" AND \"DocNum\" = TI.\"DocNum\") SUMA FROM \"10099_BDDOCS\".\"STOCK_DET\" TI WHERE \"DocNum\" = ? AND \"BatchNum\" IS NOT NULL "; //Query de Lotes se usa en el caso  que sean muchos números de lostes pero con igual Itemcode igual bodega

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.CompanyService oCS = (SAPbobsCOM.CompanyService)Data.ConexionSAP.myCompany.GetCompanyService();
                            SAPbobsCOM.InventoryPostingsService oInventoryPostingsService = (SAPbobsCOM.InventoryPostingsService)oCS.GetBusinessService(SAPbobsCOM.ServiceTypes.InventoryPostingsService);
                            SAPbobsCOM.InventoryPosting myDoc_OIQR = (SAPbobsCOM.InventoryPosting)oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

                            while (dr.Read())
                            {
                                myDoc_OIQR = (SAPbobsCOM.InventoryPosting)oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OIQR.UserFields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OIQR.CountDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OIQR.Remarks = dr["Comments"].ToString();

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta 

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {

                                        try
                                        {
                                            SAPbobsCOM.InventoryPostingLines oInventoryPostingLines = myDoc_OIQR.InventoryPostingLines;
                                            SAPbobsCOM.InventoryPostingLine myDoc_OIQR_Lines = oInventoryPostingLines.Add();

                                            //variables para validaciones
                                            DNdetalla = Convert.ToInt32(dtr["DocNum"]);
                                            Itm = dtr["ItemCode"].ToString();
                                            count = Convert.ToInt32(dtr["CONTEO"]);

                                            if (Itm != B | DNdetalla != C) //Pregunta si se cumple aunque sea un solo campo
                                            {
                                                A = 0;

                                                if (count > 1 && A == 0) //pregunta si tiene más de un mismo Itemcode
                                                {

                                                    OdbcCommand comd = new OdbcCommand(queryBatch, conn);
                                                    comd.Parameters.Add(new OdbcParameter("@num", DNdetalla));//Pasa el valor de DocNum al query Deta 

                                                    using (OdbcDataReader datr = comd.ExecuteReader())
                                                        while (datr.Read())
                                                        {
                                                            myDoc_OIQR_Lines.UserFields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                                            B = myDoc_OIQR_Lines.ItemCode = dtr["ItemCode"].ToString();
                                                            myDoc_OIQR_Lines.WarehouseCode = dtr["WhsCode"].ToString();

                                                            InventoryPostingBatchNumber myDoc_OIQR_BatchNumber = myDoc_OIQR_Lines.InventoryPostingBatchNumbers.Add();
                                                            myDoc_OIQR_Lines.CountedQuantity = Convert.ToDouble(datr["SUMA"]);
                                                            if (!DBNull.Value.Equals(datr["BatchNum"])) { myDoc_OIQR_BatchNumber.BatchNumber = datr["BatchNum"].ToString(); }
                                                            myDoc_OIQR_BatchNumber.Quantity = Convert.ToDouble(datr["CountQty"]);

                                                            A = 1;
                                                            C = Convert.ToInt32(dtr["DocNum"]);
                                                        }
                                                    SAPbobsCOM.InventoryPostingParams oInventoryPostingParams = oInventoryPostingsService.Add(myDoc_OIQR);

                                                }
                                                else
                                                {
                                                    myDoc_OIQR_Lines.UserFields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dtr["DocNum"]);
                                                    B = myDoc_OIQR_Lines.ItemCode = dtr["ItemCode"].ToString();
                                                    myDoc_OIQR_Lines.WarehouseCode = dtr["WhsCode"].ToString();

                                                    SAPbobsCOM.InventoryPostingBatchNumber myDoc_OIQR_BatchNumber = myDoc_OIQR_Lines.InventoryPostingBatchNumbers.Add();
                                                    myDoc_OIQR_Lines.CountedQuantity = Convert.ToDouble(dtr["CountQty"]);
                                                    if (!DBNull.Value.Equals(dtr["BatchNum"])) { myDoc_OIQR_BatchNumber.BatchNumber = dtr["BatchNum"].ToString(); }
                                                    myDoc_OIQR_BatchNumber.Quantity = Convert.ToDouble(dtr["CountQty"]);

                                                    SAPbobsCOM.InventoryPostingParams oInventoryPostingParams = oInventoryPostingsService.Add(myDoc_OIQR);
                                                }
                                            }

                                            //String queryStatus = "UPDATE \"10025_BDDOCS\".\"STOCK_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdetalla + " "; //Query Actualiza el campo Status a 1, Tabla detalle
                                            //OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                            ////cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                            //cm.ExecuteNonQuery();

                                        }
                                        catch (Exception ex)
                                        {
                                            conn.Close();
                                            string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                            objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OIQR: " + msgErr + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }

                                    }

                                string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey(); // Obtiene el DocEntry al crear el registro en SAP
                                int DC_SAP = Convert.ToInt32(DocEnt);
                                string queryAct = "UPDATE \"10025_BDDOCS\".\"STOCK_EN\" SET \"DocEntry_SAP\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                cmand.ExecuteNonQuery();

                                //MessageBox.Show("Se Agrego Correctamente");
                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OWTR: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
            }

        }

        // Insertar Facturas proveedores a SAP --Daniel--
        public static void Insertar_FacturaProve_OPCH()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            try
            {
                int En_DN = 0;

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT  \"CardCode\", \"DocNum\",  \"DocDate\", \"DocRate\", \"DiscPrcnt\", \"NumAtCard\",  \"DocTotal\" FROM \"10099_BDDOCS\".\"FE_PROVE_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"DiscPrcnt\", \"TaxCode\", \"WhsCode\", \"BatchNum\"  FROM \"10099_BDDOCS\".\"FE_PROVE_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {
                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_OPCH = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                            while (dr.Read())
                            {
                                myDoc_OPCH = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_OPCH.CardCode = dr["CardCode"].ToString();
                                myDoc_OPCH.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_OPCH.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_OPCH.DocRate = Convert.ToDouble(dr["DocRate"]);
                                if (!DBNull.Value.Equals(dr["NumAtCard"])) { myDoc_OPCH.NumAtCard = dr["NumAtCard"].ToString(); }
                                myDoc_OPCH.DiscountPercent = Convert.ToDouble(dr["DiscPrcnt"]);
                                myDoc_OPCH.DocTotal = Convert.ToDouble(dr["DocTotal"]);

                                //myDoc_OINV. Convert.ToDouble(dr["DiscSum"]);

                                myDoc_OPCH.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta                             

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_OPCH.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = dtr["DocNum"].ToString();
                                        myDoc_OPCH.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_OPCH.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_OPCH.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_OPCH.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_OPCH.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_OPCH.Lines.WarehouseCode = dtr["WhsCode"].ToString();
                                        //if (!DBNull.Value.Equals(dtr["U_Cabys"])) { myDoc_OINV.Lines.UserFields.Fields.Item("U_CABYS").Value = dtr["U_Cabys"].ToString(); }
                                        //if (!DBNull.Value.Equals(dtr["OcrCode"])) { myDoc_OINV.Lines.CostingCode = dtr["OcrCode"].ToString(); }
                                        //if (!DBNull.Value.Equals(dtr["OcrCode2"])) { myDoc_OINV.Lines.CostingCode2 = dtr["OcrCode2"].ToString(); }
                                        //if (!DBNull.Value.Equals(dtr["OcrCode3"])) { myDoc_OINV.Lines.CostingCode3 = dtr["OcrCode3"].ToString(); }
                                        //if (!DBNull.Value.Equals(dtr["OcrCode4"])) { myDoc_OINV.Lines.CostingCode4 = dtr["OcrCode4"].ToString(); }
                                        myDoc_OPCH.Lines.BatchNumbers.BatchNumber = dtr["BatchNum"].ToString();
                                        myDoc_OPCH.Lines.BatchNumbers.Quantity = myDoc_OPCH.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);

                                        myDoc_OPCH.Lines.BatchNumbers.Add();
                                        myDoc_OPCH.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"FE_PROVE_EN\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1 tabla Detalle                                         
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_OPCH.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en OINV: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey();// obtener el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"FE_PROVE_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en OPCH: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

            }

        }

        // Insertar Facturas Ordenes de Venta a SAP --Daniel--
        public static void Insertar_OrdenVenta_ORDR()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            try
            {
                int En_DN = 0;

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    string queryEn = "SELECT  \"CardCode\", \"DocNum\",  \"DocDate\", \"DocRate\", \"DiscPrcnt\", \"DOCTOTAL\", FROM \"10099_BDDOCS\".\"ORDER_VTA_EN\" WHERE \"Status\" = 0 "; //Query Encabezado
                    string queryDet = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"PriceBefDi\", \"DiscPrcnt\", \"TaxCode\", \"WhsCode\",  FROM \"10099_BDDOCS\".\"ORDER_VTA_DET\" WHERE \"DocNum\" = ? "; //Query Detalle

                    OdbcCommand CmD = new OdbcCommand(queryEn, conn); // llama al Query Encabezado con la conexion

                    conn.Open();

                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {
                        if (Data.ConexionSAP.Open())
                        {
                            Data.ConexionSAP.myCompany.StartTransaction();
                            SAPbobsCOM.Documents myDoc_ORDR = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oOrders);

                            while (dr.Read())
                            {
                                myDoc_ORDR = (SAPbobsCOM.Documents)Data.ConexionSAP.myCompany.GetBusinessObject(BoObjectTypes.oOrders);

                                En_DN = Convert.ToInt32(dr["DocNum"]);
                                myDoc_ORDR.CardCode = dr["CardCode"].ToString();
                                myDoc_ORDR.UserFields.Fields.Item("U_DocEntry_TI").Value = Convert.ToInt32(dr["DocNum"]);
                                if (!DBNull.Value.Equals(dr["DocDate"])) { myDoc_ORDR.DocDate = Convert.ToDateTime(dr["DocDate"]); }
                                myDoc_ORDR.DocRate = Convert.ToDouble(dr["DocRate"]);
                                myDoc_ORDR.DiscountPercent = Convert.ToDouble(dr["DiscPrcnt"]);
                                myDoc_ORDR.DocTotal = Convert.ToDouble(dr["DOCTOTAL"]);
                                //myDoc_OINV. Convert.ToDouble(dr["DiscSum"]);

                                myDoc_ORDR.DocType = BoDocumentTypes.dDocument_Items;

                                OdbcCommand cmc = new OdbcCommand(queryDet, conn);
                                cmc.Parameters.Add(new OdbcParameter("@num", En_DN));//Pasa el valor de DocNum al query Deta                             

                                // lineas de Detalle
                                using (OdbcDataReader dtr = cmc.ExecuteReader())
                                    while (dtr.Read())
                                    {
                                        int DNdeta = Convert.ToInt32(dtr["DocNum"]);

                                        myDoc_ORDR.Lines.UserFields.Fields.Item("U_DocEntry_TI").Value = dtr["DocNum"].ToString();
                                        myDoc_ORDR.Lines.ItemCode = dtr["ItemCode"].ToString();
                                        myDoc_ORDR.Lines.Quantity = Convert.ToDouble(dtr["Quantity"]);
                                        myDoc_ORDR.Lines.Price = Convert.ToDouble(dtr["PriceBefDi"]);
                                        myDoc_ORDR.Lines.DiscountPercent = Convert.ToDouble(dtr["DiscPrcnt"]);
                                        myDoc_ORDR.Lines.TaxCode = dtr["TaxCode"].ToString();
                                        myDoc_ORDR.Lines.WarehouseCode = dtr["WhsCode"].ToString();

                                        myDoc_ORDR.Lines.BatchNumbers.Add();
                                        myDoc_ORDR.Lines.Add();

                                        String queryStatus = "UPDATE \"10099_BDDOCS\".\"ORDER_VTA_DET\" SET \"Status\" = 1  WHERE \"DocNum\" =  " + DNdeta + " "; //Query Actualiza el campo Status a 1 tabla Detalle                                         
                                        //cm.Parameters.Add(new OdbcParameter("@num", dN ));//Pasa el valor de DocNum al query que actuliza Status a 1
                                        OdbcCommand cm = new OdbcCommand(queryStatus, conn);
                                        cm.ExecuteNonQuery();

                                    }

                                if (myDoc_ORDR.Add() != 0)//En caso de fallar
                                {
                                    conn.Close();
                                    string msgErr = Data.ConexionSAP.myCompany.GetLastErrorDescription();
                                    objRegistraLog.Graba("Error en el proceso de agregar de la Tabla Intermedia a SAP en ORDR: " + msgErr + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                                    Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                                }
                                else
                                {
                                    string DocEnt = Data.ConexionSAP.myCompany.GetNewObjectKey();// obtener el DocEntry al crear el registro en SAP
                                    int DC_SAP = Convert.ToInt32(DocEnt);
                                    string queryAct = "UPDATE \"10099_BDDOCS\".\"ORDER_VTA_EN\" SET \"Doc_Entry_Sap\" = " + DC_SAP + ", \"Status\" = 1 WHERE \"DocNum\" =  " + En_DN + " "; //Query Actualiza el campo Status y DocEntry_SAP, Tabla Encabezado
                                    OdbcCommand cmand = new OdbcCommand(queryAct, conn);
                                    cmand.ExecuteNonQuery();

                                    //MessageBox.Show("Se Agrego Correctamente");
                                }

                            }
                            Data.ConexionSAP.myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            conn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //conn.Close();
                objRegistraLog.Graba("Error al insertar de la Tabla Intermedia a SAP en ORDR: " + "/" + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

            }

        }

        #endregion

    }
}