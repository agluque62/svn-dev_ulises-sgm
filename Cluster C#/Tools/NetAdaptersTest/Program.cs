using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

using NLog;

namespace NetAdaptersTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAdaptersState();
            Console.ReadKey();
        }
        static int GetAdaptersState()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            int adapters = 0;

            foreach (NetworkInterface adapter in nics)
            {
                if ((adapter.OperationalStatus == OperationalStatus.Up) && (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    UnicastIPAddressInformationCollection ips = adapter.GetIPProperties().UnicastAddresses;

                    foreach (UnicastIPAddressInformation ip in ips)
                    {
                        string strIp = ip.Address.ToString();

                        Console.WriteLine($"Adapter #{adapter.GetIPProperties().GetIPv4Properties().Index}, Ip {strIp}");
                        //if (strIp == _Settings.AdapterIp1)
                        //{
                        //    adapters |= 1;
                        //    _Adapter1Index = adapter.GetIPProperties().GetIPv4Properties().Index;
                        //}
                        //if (strIp == _Settings.AdapterIp2)
                        //{
                        //    adapters |= 2;
                        //    _Adapter2Index = adapter.GetIPProperties().GetIPv4Properties().Index;
                        //}

                        if (adapters == 3)
                        {
                            return adapters;
                        }
                    }
                }
            }
            return adapters;
        }
    }
}
