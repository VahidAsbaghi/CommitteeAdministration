using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;

namespace ImmigrationSolution
{
    public class DeviceInfo
    {
        [DllImport("DeviceInfo.dll")]
        public static extern void GetDeviceInfo(IntPtr macAddressInfo);

        public string GetDeviceId()
        {
            var drive=string.Empty;
            if (drive == string.Empty)
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                //C:\ -> C
                drive = drive.Substring(0, drive.Length - 2);
            }

            string volumeSerial = getVolumeSerial(drive);
            string cpuID = getCPUID();
            var macAddresses = GetMacAddress();
            var returnString= cpuID+';'+volumeSerial+';'+string.Join(";",GetMacAddress());
            //Mix them up and remove some useless 0's
           
            return returnString;
        }
        private List<string> GetMacAddress()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                ("Select MACAddress,PNPDeviceID FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL AND PNPDeviceID IS NOT NULL");
            ManagementObjectCollection mObject = searcher.Get();

            return (from ManagementObject obj in mObject let pnp = obj["PNPDeviceID"].ToString() select obj["MACAddress"].ToString() into mac select mac.Replace(":", string.Empty) into mac where mac[0] != '0' select mac).ToList();
            //var macInfo=new MAC_ADDRESS_INFO();
            //macInfo.Address = "";
            //IntPtr ptr= Marshal.AllocHGlobal(Marshal.SizeOf(macInfo));
            //Marshal.StructureToPtr<MAC_ADDRESS_INFO>(macInfo, ptr, false);
            //GetDeviceInfo(ptr);
            //var macAddress=Marshal.PtrToStructure<MAC_ADDRESS_INFO>(ptr);
            //return macAddress.Address;
        }

        private string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }
    }
   
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MAC_ADDRESS_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Address;
       
    }
}
