using StreamScan.Constants;
using StreamScanCommon.Infos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace StreamScan.Helpers
{
    /// <summary>
    /// Helper utilisé par la DalMachines afin de définir une propriété sur une machine
    /// ou extraire les propriétés d'une machine.
    /// </summary>
    public class HMachines
    {
        /// <summary>
        /// Applique à la propriété de la machine donnée la propriété donnée
        /// </summary>
        /// <param name="machine">Machine à laquelle on applique la propriété</param>
        /// <param name="idProperty">L'ID de la propriété à laquelle on applique la valeur</param>
        /// <param name="value">La valeur de la propriété à appliquer</param>
        /// <returns></returns>
        public static Info SetMachineProperty(Info machine, int idProperty, string value)
        {
            switch (idProperty)
            {
                case CSystemProperties.STREAMX_VERSION:
                    machine.InfosStreamX.Version = value;
                    break;
                case CSystemProperties.STREAMX_FILENAME:
                    machine.InfosStreamX.Filename = value;
                    break;
                case CSystemProperties.STREAMX_FIRSTINSTALLATION:
                    machine.InfosStreamX.FirstInstallation = DateTime.Parse(value);
                    break;
                case CSystemProperties.STREAMX_LASTINSTALLATION:
                    machine.InfosStreamX.LastInstallation = DateTime.Parse(value);
                    break;
                case CSystemProperties.HARDWARE_MANUFACTURERNAME:
                    machine.InfosMachine.ManufacturerName = value;
                    break;
                case CSystemProperties.HARDWARE_PRODUCTNAME:
                    machine.InfosMachine.ProductName = value;
                    break;
                case CSystemProperties.HARDWARE_PRODUCTTYPE:
                    machine.InfosMachine.ProductType = value;
                    break;
                case CSystemProperties.SOFTWARE_OS:
                    machine.InfosMachine.ProductNameOs = value;
                    break;
                case CSystemProperties.SOFTWARE_CPUARCHITECTURE:
                    machine.InfosMachine.CpuArchitecture = value;
                    break;
                case CSystemProperties.SOFTWARE_EDITION:
                    machine.InfosMachine.Edition = value;
                    break;
                case CSystemProperties.SOFTWARE_PLATFORMID:
                    machine.InfosMachine.PlatformID = value;
                    break;
                case CSystemProperties.SOFTWARE_SERVICEPACK:
                    machine.InfosMachine.ServicePack = value;
                    break;
                case CSystemProperties.SOFTWARE_VERSION:
                    machine.InfosMachine.Version = value;
                    break;
                case CSystemProperties.NETWORK_HOSTNAME:
                    machine.InfosReseau.HostName = value;
                    break;
                case CSystemProperties.NETWORK_IP:
                    machine.InfosReseau.IpAddress = value;
                    break;
                case CSystemProperties.NETWORK_CARDNAME:
                    machine.InfosReseau.CardName = value;
                    break;
                case CSystemProperties.NETWORK_DOMAINNAME:
                    machine.InfosReseau.DomainName = value;
                    break;
                case CSystemProperties.NETWORK_GATEWAY:
                    machine.InfosReseau.Gateway = value;
                    break;
                case CSystemProperties.NETWORK_SUBNETMASK:
                    machine.InfosReseau.SubnetMask = value;
                    break;
            }
            return machine;
        }

        /// <summary>
        /// Transforme l'object Info donné en Dictionary&lt;int, Object&gt;.
        /// La clé est l'ID de la propriété et la valeur la propriété
        /// </summary>
        /// <param name="machine">La machine (Info) à transformer</param>
        /// <returns></returns>
        public static Dictionary<int, Object> GetMachineProperties(Info machine)
        {
            Dictionary<int, Object> properties = new Dictionary<int, object>
            {
                { CSystemProperties.STREAMX_VERSION,           machine.InfosStreamX.Version },
                { CSystemProperties.STREAMX_FILENAME,          machine.InfosStreamX.Filename },
                { CSystemProperties.STREAMX_FIRSTINSTALLATION, machine.InfosStreamX.FirstInstallation },
                { CSystemProperties.STREAMX_LASTINSTALLATION,  machine.InfosStreamX.LastInstallation },
                { CSystemProperties.HARDWARE_MANUFACTURERNAME, machine.InfosMachine.ManufacturerName },
                { CSystemProperties.HARDWARE_PRODUCTNAME,      machine.InfosMachine.ProductName },
                { CSystemProperties.HARDWARE_PRODUCTTYPE,      machine.InfosMachine.ProductType },
                { CSystemProperties.SOFTWARE_OS,               machine.InfosMachine.ProductNameOs },
                { CSystemProperties.SOFTWARE_CPUARCHITECTURE,  machine.InfosMachine.CpuArchitecture },
                { CSystemProperties.SOFTWARE_EDITION,          machine.InfosMachine.Edition },
                { CSystemProperties.SOFTWARE_PLATFORMID,       machine.InfosMachine.PlatformID },
                { CSystemProperties.SOFTWARE_SERVICEPACK,      machine.InfosMachine.ServicePack },
                { CSystemProperties.SOFTWARE_VERSION,          machine.InfosMachine.Version },
                { CSystemProperties.NETWORK_HOSTNAME,          machine.InfosReseau.HostName },
                { CSystemProperties.NETWORK_IP,                machine.InfosReseau.IpAddress },
                { CSystemProperties.NETWORK_CARDNAME,          machine.InfosReseau.CardName },
                { CSystemProperties.NETWORK_DOMAINNAME,        machine.InfosReseau.DomainName },
                { CSystemProperties.NETWORK_GATEWAY,           machine.InfosReseau.Gateway },
                { CSystemProperties.NETWORK_SUBNETMASK,        machine.InfosReseau.SubnetMask }
            };
            return properties;
        }

        public static string ObjectToXml(Object obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(obj.GetType());
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    xml = sww.ToString();
                }
            }
            return xml;
        }
    }
}