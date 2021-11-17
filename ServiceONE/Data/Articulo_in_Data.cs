using ServiceONE.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ServiceONE.Data
{
    public class Articulo_in_Data
    {
        public List<InfoInsert_Art> NuevoArticulo(List<TablaInt_Articulo_in> listaArt)
        {
            try
            {
                Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
                List<InfoInsert_Art> objJson = new List<InfoInsert_Art>();

                using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
                {
                    conn.Open();

                    foreach (TablaInt_Articulo_in obj in listaArt)
                    {
                        try
                        {

                            string queryCons = "SELECT * FROM \"10031_BDDOCS\".\"ARTICULOS\" WHERE \"ItemCode\" = '" + obj.ItemCode + "'";
                            OdbcCommand CD = new OdbcCommand(queryCons, conn);
                            using (OdbcDataReader dr = CD.ExecuteReader())

                                if (dr.RecordsAffected == 0) //válida si el ItemCode ya existe o no en la tabla
                                {

                                    if (obj.ItemCode != "" & obj.ItemName != "" & obj.Lit_Viable != "" & obj.U_Cabys != "" & obj.Cod_Impuesto != 0 & obj.GrupoCode > 0)// valida si algún campo viene  vacio
                                    {

                                        Regex Val = new Regex(@"^[ynYN]+$");
                                        if (Val.IsMatch(obj.Lit_Viable)) // válida que solo sea la letra Y o N
                                        {
                                                                                     
                                                string LitViable = obj.Lit_Viable.ToUpper();

                                                string query = "insert into \"10031_BDDOCS\".\"ARTICULOS\" (\"ItemCode\", \"ItemName\", \"Cod_impuesto\", \"Lit_viable\", \"U_Cabys\", \"GrupoCode\")" +
                                                "values ('" + obj.ItemCode + "', '" + obj.ItemName + "', " + obj.Cod_Impuesto + ", '" + LitViable + "', '" + obj.U_Cabys + "', " + obj.GrupoCode + ")";
                                                OdbcCommand CmD = new OdbcCommand(query, conn);
                                                CmD.ExecuteReader();

                                                string queryp = "SELECT \"DocEntry\" FROM \"10031_BDDOCS\".\"ARTICULOS\" WHERE \"ItemCode\" = '" + obj.ItemCode + "'";
                                                OdbcCommand Cm = new OdbcCommand(queryp, conn);
                                                using (OdbcDataReader dtr = Cm.ExecuteReader())
                                                    while (dtr.Read())
                                                    {
                                                        InfoInsert_Art objinfInsrt = new InfoInsert_Art();
                                                        objinfInsrt.ItemCode = obj.ItemCode;
                                                        objinfInsrt.ItemName = obj.ItemName;
                                                        objinfInsrt.DocEntry = Convert.ToInt32(dtr["DocEntry"]);
                                                        objinfInsrt.Estado = "OK";
                                                        objJson.Add(objinfInsrt);
                                                    }                                    
                                                                                      
                                            
                                        }
                                        else
                                        {
                                            InfoInsert_Art objinfInsrt = new InfoInsert_Art();
                                            objinfInsrt.ItemCode = obj.ItemCode;
                                            objinfInsrt.ItemName = obj.ItemName;
                                            objinfInsrt.DocEntry = -1;
                                            objinfInsrt.Estado = "Sin insertar: el Campo Lit_viable  solo admite letras Y o N";
                                            objJson.Add(objinfInsrt);
                                        }


                                    }
                                    else
                                    {
                                        InfoInsert_Art objinfInsrt = new InfoInsert_Art();
                                        objinfInsrt.ItemCode = obj.ItemCode;
                                        objinfInsrt.ItemName = obj.ItemName;
                                        objinfInsrt.DocEntry = -1;
                                        objinfInsrt.Estado = "Sin insertar, revisar: Todos los campos son requeridos,  los Campos numéricos no pueden ser 0, Tipo de valor inválido con su formato";
                                        objJson.Add(objinfInsrt);
                                    }
                                }

                                else
                                {
                                    InfoInsert_Art objinfInsrt = new InfoInsert_Art();
                                    objinfInsrt.ItemCode = obj.ItemCode;
                                    objinfInsrt.ItemName = obj.ItemName;
                                    objinfInsrt.DocEntry = -1;
                                    objinfInsrt.Estado = "Sin Insertar: Este ItemCode ya existe en la tabla";
                                    objJson.Add(objinfInsrt);
                                }

                        }
                        catch (Exception ex)
                        {

                            objRegistraLog.Graba("Error en el POST al insertar en ARTICULOS : " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                            InfoInsert_Art objinfInsrt = new InfoInsert_Art();
                            objinfInsrt.ItemCode = obj.ItemCode;
                            objinfInsrt.ItemName = obj.ItemName;
                            objinfInsrt.DocEntry = -1;
                            objinfInsrt.Estado = "No se pudo insertar el ItemCode : " + ex.Message;
                            objJson.Add(objinfInsrt);
                        }


                    }// foreach END

                    conn.Close();
                    return objJson;

                }
            }
            catch (Exception exp)
            {
                Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error 
                List<InfoInsert_Art> objJson = new List<InfoInsert_Art>();
                objRegistraLog.Graba("Error en el POST al insertar el Json a ARTICULOS  : " + exp.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                InfoInsert_Art objinfInsrt = new InfoInsert_Art();

                objinfInsrt.Estado = "Sin Insertar registros ver formato del Json: " + exp.Message;
                objJson.Add(objinfInsrt);
                return objJson;
            }
        }
    }
}