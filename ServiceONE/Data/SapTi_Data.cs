using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ServiceONE.Data
{
    public class SapTi_Data
    {
        // Insertar costos en la tabla intermedia
        public static bool InsertTI_Costos()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_COSTOS"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne Costos Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {                    
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en COSTOS: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OCRD en la tabla intermedia
        public static bool InsertTI_OCRD()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OCRD"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OCRD Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));

                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OCRD: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OITM en la tabla intermedia
        public static bool InsertTI_OITM()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OITM"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OITM Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OITM: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OPOR en la tabla intermedia
        public static bool InsertTI_OPOR()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error
            
            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OPOR"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OPOR Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OPOR: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OSTC en la tabla intermedia
        public static bool InsertTI_OSTC()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OSTC"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OSTC Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OSCT: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OWTR en la tabla intermedia
        public static bool InsertTI_OWTR()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OWTR"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OWTR Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OWTR: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OINV en la tabla intermedia x csadmin
        public static bool InsertTI_OINV()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OINV"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OINV Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OINV: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar ITM1 en la tabla intermedia x csadmin
        public static bool InsertTI_ITM1()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_ITM1"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne ITM1 Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en iTM1: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

        // Insertar OITW en la tabla intermedia x csadmin
        public static bool InsertTI_OITW()
        {
            Data.RegistroLogClass objRegistraLog = new Data.RegistroLogClass(); //Log en caso de error

            using (OdbcConnection conn = new OdbcConnection(Conexion.strCon))
            {
                try
                {

                    conn.Open();
                    OdbcCommand cmd = new OdbcCommand("", conn);

                    cmd.CommandText = @"call 10099_BDDOCS.INS_TI_OITW"; // llamado de storeprocedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    objRegistraLog.Graba("Prueba en ServeOne OITW Ecomm: " + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    objRegistraLog.Graba("Error en el proceso de Insertar a la Tabla Intermedia en OITW: " + ex.Message + "/ " + "Hora: " + DateTime.Now.ToString("HH:mm:ss tt"));
                    conn.Close();
                    return false;
                }
            }
        }

    }
}