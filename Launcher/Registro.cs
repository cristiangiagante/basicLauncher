using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Launcher
{
    public static class Registro
    {
        public enum ClavesRaiz
        {
            HKEY_LOCAL_MACHINE, HKEY_CURRENT_CONFIG, HKEY_CLASSES_ROOT, HKEY_CURRENT_USER, HKEY_USERS, HKEY_PERFORMANCE_DATA
        }

        public static string Error { get; set; }
        /// <summary>
        /// Create a registry key and value from a string registry file model
        /// </summary>
        /// <param name="regCode">
        /// $"REGEDIT4{Environment.NewLine}"+
        /// $"[HKEY_CURRENT_USER\\Software\\Blizzard Entertainment\\Warcraft III]{Environment.NewLine}" +
        /// $"\"Battle.net Gateways\"=hex(7):31,30,30,31,00,30,30,00,31,39,38,2e,35,30,2e,31,\\{Environment.NewLine}"+
        /// $"38,35,2e,39,33,00,2d,33,00,54,45,53,54,00,00"
        /// </param>
        public static void ImportarRegistroDesdeString(string regCode)
        {
            var tmpFileName = "tmpReg.reg";
            var regFile = File.CreateText(tmpFileName);
            regFile.WriteLine(regCode);
            regFile.Close();
            try
            {
                Process proc = Process.Start(new ProcessStartInfo("reg", $"import {tmpFileName}") { CreateNoWindow = false, UseShellExecute = true });
                proc.WaitForExit();

            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            finally
            {
                File.Delete(tmpFileName);
            }

        }
        public static void CrearClaveDeRegistro(ClavesRaiz clave, string path, string nombreRegistro, object contenido, RegistryValueKind tipoRegistro)
        {
            RegistryKey rk;
            try
            {
                switch (clave)
                {
                    case ClavesRaiz.HKEY_LOCAL_MACHINE:
                        rk = Registry.LocalMachine;
                        break;
                    case ClavesRaiz.HKEY_CURRENT_CONFIG:
                        rk = Registry.CurrentConfig;
                        break;
                    case ClavesRaiz.HKEY_CLASSES_ROOT:
                        rk = Registry.ClassesRoot;
                        break;
                    case ClavesRaiz.HKEY_CURRENT_USER:
                        rk = Registry.CurrentUser;
                        break;
                    case ClavesRaiz.HKEY_USERS:
                        rk = Registry.Users;
                        break;
                    case ClavesRaiz.HKEY_PERFORMANCE_DATA:
                        rk = Registry.PerformanceData;
                        break;
                    default:
                        rk = Registry.CurrentUser;
                        break;
                }
                RegistryKey reg = rk.OpenSubKey(path, true);
                reg.SetValue(nombreRegistro, contenido, tipoRegistro);
            }
            catch (Exception e)
            {
                Informacion.Error = e.Message;
            }

        }
    }


}
