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
        public enum RegCmdClavesRaiz
        {
            HKLM, HKCU, HKCR, HKU, HKCC
        }
        public static void ImportarRegistroDesdeString(string regCode)
        {
            var tmpFileName = "tmpReg.reg";
            var regFile = File.CreateText(tmpFileName);
            regFile.WriteLine(regCode);
            regFile.Close();
            Process.Start(new ProcessStartInfo("regedit", $"{tmpFileName} -s") { CreateNoWindow = true, UseShellExecute = false });
            File.Delete(tmpFileName);
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
            catch(Exception e)
            {
                Informacion.Error = e.Message;
            }

        }
    }


}
