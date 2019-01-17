using System;
using System.Management;

namespace ServiceLibrary.Subscription
{
    public static class HardwareInfo
    {
        public static string GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }

        public static string GetRamSerialNo()
        {
            string ram = String.Empty;
            ManagementObjectSearcher searcher = new
                  ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (ManagementObject obj in searcher.Get())
            {
                object serial = obj["SerialNumber"];
                ram += serial?.ToString().Trim() ?? String.Empty;

            }
            return ram;
        }
    }
}
