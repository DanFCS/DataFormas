using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class OWTR_out_Data
    {
        

        public static List<TablaInt_OWTR_out> Listar_Transf(TopSkip objParam)
        {
            int DnEnc = 0;
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            List<TablaInt_OWTR_out> Obj_list_OWTR_out = new List<TablaInt_OWTR_out>(); //list Encabezado
             //List<TablaInt_WTR1_out> Obj_list_WTR1_out = new List<TablaInt_WTR1_out>(); //list Detalle
            int miTop = objParam.TOP;
            int miSkip = objParam.SKIP;

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {               
                string queryOWTR = "SELECT \"DocNum\", \"CardCode\", \"DocDate\", \"Filler\",\"ToWhsCode\" FROM \"10099_BDDOCS\".\"OWTR\" WHERE \"Status\" = 0 LIMIT " + miTop + " OFFSET " + miSkip + " "; //Query Encabezado
                string queryWTR1 = "SELECT \"DocNum\", \"ItemCode\", \"Quantity\", \"Filler\", \"ToWhsCode\", \"BatchNum\", \"ExpDate\", \"MnfDate\"  FROM \"10099_BDDOCS\".\"WTR1\"WHERE \"DocNum\" = ?"; //Queey Detalle  

                OdbcCommand CmD = new OdbcCommand(queryOWTR, conn); // llama al Query Encabezado con la conexion                       
                
                try
                {
                    conn.Open();

                     
                    using (OdbcDataReader dr = CmD.ExecuteReader())
                    {

                        //Llenado de la lista Encabezado
                        while (dr.Read())
                        {
                            //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"OWTR\" SET \"Status\" = 1  WHERE \"DocNum\" =  ? "; //Query Actualiza el campo Status a 1
                            //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion


                            TablaInt_OWTR_out E = new TablaInt_OWTR_out(); //Obj Encabezado

                            DnEnc = Convert.ToInt32(dr["Docnum"]);
                            E.DocNum = Convert.ToInt32(dr["Docnum"]);
                            E.CardCode = dr["CardCode"].ToString();

                            //valida si la fecha no viene nula para que pueda ser convertida a string
                            if (!DBNull.Value.Equals(dr["DocDate"])) { E.DocDate = Convert.ToDateTime(dr["DocDate"].ToString()); }

                            E.Filler = dr["Filler"].ToString();
                            E.ToWhsCode = dr["ToWhsCode"].ToString();
                            E.Detalle = new List<TablaInt_WTR1_out>();

                          

                            //cm.Parameters.Add(new OdbcParameter("@num", E.DocNum)); //Pasa el valor de DocNum al query que actuliza Status a 1
                            //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 
                            
                            OdbcCommand cmc = new OdbcCommand(queryWTR1, conn);
                            cmc.Parameters.Add(new OdbcParameter("@num", DnEnc));
                            using (OdbcDataReader dtr = cmc.ExecuteReader())
                                //Llenado de la lista Detalle
                            while (dtr.Read())
                            {

                                //string queryActStatus = "UPDATE \"10099_BDDOCS\".\"WTR1\" SET \"Status\" = 1  WHERE \"DocNum\" =  ? "; //Query Actualiza el campo Status a 1
                                //OdbcCommand cm = new OdbcCommand(queryActStatus, conn);// llama al Query Actualiza con la conexion

                                TablaInt_WTR1_out D = new TablaInt_WTR1_out(); // Obj Detalle

                                D.DocNum = Convert.ToInt32(dtr["DocNum"]);
                                D.ItemCode = dtr["ItemCode"].ToString();
                                D.Quantity = Convert.ToInt32(dtr["Quantity"]);
                                D.Filler = dtr["Filler"].ToString();
                                D.ToWhsCode = dtr["ToWhsCode"].ToString();
                                D.BatchNum = dtr["BatchNum"].ToString();

                                //valida si la fecha no viene nula para que pueda ser convertida a string
                                if (!DBNull.Value.Equals(dtr["ExpDate"])) { D.ExpDate = Convert.ToDateTime(dtr["ExpDate"].ToString()); }
                                if (!DBNull.Value.Equals(dtr["MnfDate"])) { D.MnfDate = Convert.ToDateTime(dtr["MnfDate"].ToString()); }
                                    E.Detalle.Add(D);

                                    // Obj_list_WTR1_out.Add(D); //Ingreso de los detalles a la lista

                                    //cm.Parameters.Add(new OdbcParameter("@num", D.DocNum)); //Pasa el valor de DocNum al query que actuliza Status a 1
                                    //cm.ExecuteNonQuery(); //Ejecuta el query de actualizar status 
                                }

                            Obj_list_OWTR_out.Add(E); //Ingreso de los encabezados a la lista
                        }
                        //FOR para recorrer los encabezados y llenar sus detalles que le corresponde a cada uno
                        //for (int i = 0; i < Obj_list_OWTR_out.Count; i++ ) // For Encabezado
                        //{                          
                        //    for (int j = 0; j < Obj_list_WTR1_out.Count; j++) // For Detalle
                        //    {
                        //        if (Obj_list_OWTR_out.ElementAt(i).DocNum == Obj_list_WTR1_out.ElementAt(j).DocNum)
                        //        {
                        //            Obj_list_OWTR_out.ElementAt(i).Detalle.Add(Obj_list_WTR1_out.ElementAt(j));

                        //        }
                        //    }
                        //}

                                              
                        conn.Close();
                        return Obj_list_OWTR_out; //Retorno el la lita de encabezados con sus detalles ya ingresados
                    }

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el POST de las tablas OWTR-WTR1 : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return Obj_list_OWTR_out; //Retorno el la lita vacía en caso no funcione el try
                }
                              
            }

        }

        public List<InfoInsert> NuevaTranferStock(List<CS_OWTR> listaTranferencias)
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            List<InfoInsert> listInst_ok = new List<InfoInsert>();

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                conn.Open();
               
                foreach (CS_OWTR obj in listaTranferencias)
                {
                    String query = "call \"10099_BDDOCS\".\"INS_TI_OWTR_POST\" ('" + obj.CardCode + "'," + obj.DocNum + ",'" + obj.DocDate.ToString("yyyy-MM-dd") + "','" + obj.Filler + "','" + obj.ToWhsCode + "'," + obj.Status + ",'" + obj.MSG_ERR + "')";
                    OdbcCommand CmD = new OdbcCommand(query, conn);
                    try
                    {

                        CmD.ExecuteReader();
                        foreach (CS_WTR1 dl in obj.Detalle)
                        {
                            query = "insert into \"10099_BDDOCS\".\"TRANSFER_DET\" (\"ItemCode\",\"DocNum\",\"Quantity\",\"Filler\",\"ToWhsCode\",\"BatchNum\",\"Doc_Entry_Sap\",\"Status\",\"MSG_ERR\") " +
                                "values('" + dl.ItemCode + "'," + dl.DocNum + "," + dl.Quantity + ",'" + dl.Filler + "','" + dl.ToWhsCode + "','" + dl.BatchNum + "'," + dl.Doc_Entry_Sap + "," + dl.Status + ",'" + dl.MSG_ERR + "')";
                             CmD = new OdbcCommand(query, conn);
                            CmD.ExecuteReader();
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        objRegistraLog.Graba("Error en el POST al insertar el Detalle  INS_TI_OWTR_POST-TRANSFER_DET : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                        Console.WriteLine(ex.Message);
                    }
                }// foreach (CS_OWTR transfer in listaTranferencias)

                    foreach (CS_OWTR obj in listaTranferencias) //For para consultar cuales documentos fueron innsertados correctamente
                    {
                        InfoInsert objinfInsrt = new InfoInsert();
                        string queryCons = "SELECT \"DOCNUM\", \"DocEntry\" FROM \"10099_BDDOCS\".\"TRANSFER_EN\" WHERE \"DOCNUM\" = " + obj.DocNum + "";
                        OdbcCommand CmD = new OdbcCommand(queryCons, conn);
                        using (OdbcDataReader dr = CmD.ExecuteReader())
                            while (dr.Read())
                            {
                                try
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DOCNUM"]);
                                    objinfInsrt.DocEntry = Convert.ToInt32(dr["DocEntry"]);
                                    objinfInsrt.Estado = "OK";
                                    listInst_ok.Add(objinfInsrt);
                                }
                                catch (Exception exp)
                                {
                                    objinfInsrt.DocNum = Convert.ToInt32(dr["DOCNUM"]);
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
                    objRegistraLog.Graba("Error en el POST de las tablas INS_TI_OWTR_POST-TRANSFER_DET : " + e.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return listInst_ok;
                }
               
            }
        }
                       
    }
}