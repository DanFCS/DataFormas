using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ServiceONE.Models;
using System.Text.RegularExpressions;

namespace ServiceONE.Data
{
    public class Cliente_in_Data
    {
        public List<InfoInsert_CL> NuevoCliente(List<TablaInt_Cliente_in> listaClient)
        {
            try
            {
                Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
                List<InfoInsert_CL> objJson = new List<InfoInsert_CL>();

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    conn.Open();

                    foreach (TablaInt_Cliente_in obj in listaClient)
                    {
                        try
                        {

                            string queryCons = "SELECT * FROM \"10031_BDDOCS\".\"CLIENTE\" WHERE \"CardCode\" = '" + obj.CardCode + "'";
                            OdbcCommand CD = new OdbcCommand(queryCons, conn);
                            using (OdbcDataReader dr = CD.ExecuteReader())

                                if (dr.RecordsAffected == 0) //valida si el ItemCode ya existe o no en la tabla
                                {
                                    if (obj.CardCode != "" & obj.CardName != "" & obj.CardType != "" & obj.Phone1 != "" & obj.Phone2 != "" & obj.E_Mail != "" & obj.Cellular != "" & obj.TpIdentificador != "")
                                    {
                                        Regex ValCardtype = new Regex(@"^[cC]+$");// Válido que solo sea la letra C
                                        if (ValCardtype.IsMatch(obj.CardType))
                                        {

                                            Regex Val = new Regex(@"^\d+$");// Valido que los campos phone1-2, celular sean numéricos enteros
                                            if (Val.IsMatch(obj.Phone1) & Val.IsMatch(obj.Phone2) & Val.IsMatch(obj.Cellular))
                                            {
                                                try
                                                {
                                                    int TpIdent = Convert.ToInt32(obj.TpIdentificador);

                                                    if (TpIdent <= 5 & TpIdent >= 1)// Valido que solo sea un número del 1 al 5
                                                    {

                                                        Regex ValEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");// Valido que el formato del correo sea el correcto
                                                        if (ValEmail.IsMatch(obj.E_Mail))
                                                        {
                                                            string CardType = obj.CardType.ToUpper();

                                                            string query = "INSERT INTO \"10031_BDDOCS\".\"CLIENTE\" (\"CardCode\", \"CardName\", \"CardType\", \"Phone1\", \"Phone2\", \"E_Mail\", \"Cellular\", \"U_tipo_identificacion\")" +
                                                         "values ('" + obj.CardCode + "', '" + obj.CardName + "', '" + CardType + "', '" + obj.Phone1 + "', '" + obj.Phone2 + "', '" + obj.E_Mail + "', '" + obj.Cellular + "', '" + obj.TpIdentificador + "')";
                                                            OdbcCommand CmD = new OdbcCommand(query, conn);
                                                            CmD.ExecuteReader();

                                                            string queryp = "SELECT \"DocEntry\" FROM \"10031_BDDOCS\".\"CLIENTE\" WHERE \"CardCode\" = '" + obj.CardCode + "'";
                                                            OdbcCommand Cm = new OdbcCommand(queryp, conn);
                                                            using (OdbcDataReader dtr = Cm.ExecuteReader())
                                                                while (dtr.Read())
                                                                {
                                                                    InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                                                    objinfInsrt.CardCode = obj.CardCode;
                                                                    objinfInsrt.CardName = obj.CardName;
                                                                    objinfInsrt.DocEntry = Convert.ToInt32(dtr["DocEntry"]);
                                                                    objinfInsrt.Estado = "OK";
                                                                    objJson.Add(objinfInsrt);
                                                                }

                                                        }
                                                        else
                                                        {
                                                            InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                                            objinfInsrt.CardCode = obj.CardCode;
                                                            objinfInsrt.CardName = obj.CardName;
                                                            objinfInsrt.DocEntry = -1;
                                                            objinfInsrt.Estado = "Sin insertar: El campo Email Tiene un formato no válido";
                                                            objJson.Add(objinfInsrt);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                                        objinfInsrt.CardCode = obj.CardCode;
                                                        objinfInsrt.CardName = obj.CardName;
                                                        objinfInsrt.DocEntry = -1;
                                                        objinfInsrt.Estado = "Sin insertar: El campo TpIdentificador deben de ser un número del 1 al 5";
                                                        objJson.Add(objinfInsrt);
                                                    }
                                                }
                                                catch (Exception)
                                                {

                                                    InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                                    objinfInsrt.CardCode = obj.CardCode;
                                                    objinfInsrt.CardName = obj.CardName;
                                                    objinfInsrt.DocEntry = -1;
                                                    objinfInsrt.Estado = "Sin insertar: El campo TpIdentificador deben de ser un número entero";
                                                    objJson.Add(objinfInsrt);
                                                }


                                            }
                                            else
                                            {
                                                InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                                objinfInsrt.CardCode = obj.CardCode;
                                                objinfInsrt.CardName = obj.CardName;
                                                objinfInsrt.DocEntry = -1;
                                                objinfInsrt.Estado = "Sin insertar: Los campos Phone1-2,Cellular deben de ser némeros enteros";
                                                objJson.Add(objinfInsrt);
                                            }
                                        }
                                        else
                                        {
                                            InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                            objinfInsrt.CardCode = obj.CardCode;
                                            objinfInsrt.CardName = obj.CardName;
                                            objinfInsrt.DocEntry = -1;
                                            objinfInsrt.Estado = "Sin insertar: El campos CardType deben de ser C";
                                            objJson.Add(objinfInsrt);
                                        }
                                    }
                                    else
                                    {
                                        InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                        objinfInsrt.CardCode = obj.CardCode;
                                        objinfInsrt.CardName = obj.CardName;
                                        objinfInsrt.DocEntry = -1;
                                        objinfInsrt.Estado = "Sin insertar, revisar: Todos los campos son requeridos, Tipo de valor inválido con su formato";
                                        objJson.Add(objinfInsrt);
                                    }
                                }
                                else
                                {
                                    InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                                    objinfInsrt.CardCode = obj.CardCode;
                                    objinfInsrt.CardName = obj.CardName;
                                    objinfInsrt.DocEntry = -1;
                                    objinfInsrt.Estado = "Sin Insertar: Este CardCode ya existe en la tabla";
                                    objJson.Add(objinfInsrt);
                                }
                        }
                        catch (Exception ex)
                        {

                            objRegistraLog.Graba("Error en el POST al insertar en Clientes : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                            InfoInsert_CL objinfInsrt = new InfoInsert_CL();
                            objinfInsrt.CardCode = obj.CardCode;
                            objinfInsrt.CardName = obj.CardName;
                            objinfInsrt.DocEntry = -1;
                            objinfInsrt.Estado = "No se pudo insertar el CardCode : " + ex.Message;
                            objJson.Add(objinfInsrt);
                        }


                    }// foreach (CS_OWTR transfer in listaClient)

                    conn.Close();
                    return objJson;

                }
            }
            catch (Exception exp)
            {

                Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 
                List<InfoInsert_CL> objJson = new List<InfoInsert_CL>();
                objRegistraLog.Graba("Error en el POST al insertar el Json a CLIENTES  : " + exp.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                InfoInsert_CL objinfInsrt = new InfoInsert_CL();

                objinfInsrt.Estado = "Sin Insertar registros ver formato del Json: " + exp.Message;
                objJson.Add(objinfInsrt);
                return objJson;
            }
        }
    }
}