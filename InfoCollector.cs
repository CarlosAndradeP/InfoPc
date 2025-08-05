using System;
using System.Management;
using Microsoft.Win32;

namespace InfoPC
{
    public static class InfoCollector
    {
        public static string GetInfo()
        {
            return $@"Nome do Computador: {Environment.MachineName}
Usuário: {Environment.UserName}
Sistema Operacional: {Environment.OSVersion}
Arquitetura: {(Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits")}
Diretório do Sistema: {Environment.SystemDirectory}
Processadores: {Environment.ProcessorCount}
ID da Máquina: {GetMachineGuid()}
Serial do HD: {GetDiskSerial()}
Chave do Windows: {GetWindowsKey()}
Chave do Office: {GetOfficeKey()}";
        }

        public static string GetMachineGuid()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    if (key != null)
                        return key.GetValue("MachineGuid").ToString();
                }
            }
            catch { }
            return "Não encontrado";
        }

        public static string GetDiskSerial()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (obj["SerialNumber"] != null)
                        return obj["SerialNumber"].ToString().Trim();
                }
            }
            catch { }
            return "Não encontrado";
        }

        public static string GetWindowsKey()
        {
            try
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object digitalProductId = key.GetValue("DigitalProductId");
                        if (digitalProductId != null)
                            return DecodeProductKey((byte[])digitalProductId);
                    }
                }
            }
            catch { }
            return "Não encontrado";
        }

        // Decodifica a chave do Windows
        public static string DecodeProductKey(byte[] digitalProductId)
        {
            const string keyChars = "BCDFGHJKMPQRTVWXY2346789";
            int keyStartIndex = 52;
            int keyEndIndex = keyStartIndex + 15;
            char[] decodedChars = new char[29];
            for (int i = 28; i >= 0; i--)
            {
                if ((i + 1) % 6 == 0)
                {
                    decodedChars[i] = '-';
                }
                else
                {
                    int acc = 0;
                    for (int j = 14; j >= 0; j--)
                    {
                        acc = acc * 256 ^ digitalProductId[j + keyStartIndex];
                        digitalProductId[j + keyStartIndex] = (byte)(acc / 24);
                        acc %= 24;
                    }
                    decodedChars[i] = keyChars[acc];
                }
            }
            return new string(decodedChars);
        }

        public static string GetOfficeKey()
        {
            try
            {
                string[] officePaths = {
                    @"SOFTWARE\Microsoft\Office\16.0\Registration",
                    @"SOFTWARE\Microsoft\Office\15.0\Registration",
                    @"SOFTWARE\Microsoft\Office\14.0\Registration"
                };
                foreach (var path in officePaths)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
                    {
                        if (key != null)
                        {
                            foreach (var subKeyName in key.GetSubKeyNames())
                            {
                                using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                                {
                                    object digitalProductId = subKey.GetValue("DigitalProductId");
                                    if (digitalProductId != null)
                                        return DecodeProductKey((byte[])digitalProductId);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return "Não encontrado";
        }
    }
}
