// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ServicesMapping
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ServicesMapping
  {
    private static Dictionary<string, List<ServiceSetting>> settings = new Dictionary<string, List<ServiceSetting>>();

    static ServicesMapping()
    {
      string str1 = Path.Combine(SystemSettings.EpassDataDir, "Exports.xml");
      if (!File.Exists(str1))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(str1);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes("Exports/Category"))
      {
        string str2 = selectNode.Attributes["name"].Value.ToUpper() == "INVESTOR SERVICES" ? "GSE Services" : selectNode.Attributes["name"].Value;
        XmlNodeList xmlNodeList = selectNode.SelectNodes("Export");
        List<ServiceSetting> serviceSettingList = new List<ServiceSetting>();
        foreach (XmlNode node1 in xmlNodeList)
        {
          XmlNode node2 = node1.SelectSingleNode("ClientFilter");
          ServiceSetting serviceSetting = new ServiceSetting(str2, ServicesMapping.getAttributeValue("id", node1), ServicesMapping.getAttributeValue("name", node1), ServicesMapping.getAttributeValue("fileName", node1), ServicesMapping.getAttributeValue("loanfileSpecific", node1), ServicesMapping.getAttributeValue("useStandardValidationGrid", node1), ServicesMapping.getAttributeValue("useLoanTab", node1), ServicesMapping.getAttributeValue("dataServicesID", node1), ServicesMapping.getAttributeValue("minVersion", node1), ServicesMapping.getAttributeValue("maxVersion", node1), string.Equals(ServicesMapping.getAttributeValue("disableMenuItemForMultipleLoanSelection", node1), "true", StringComparison.OrdinalIgnoreCase), string.Equals(ServicesMapping.getAttributeValue("enabled", node2), "true", StringComparison.OrdinalIgnoreCase), string.Equals(ServicesMapping.getAttributeValue("toolStripSeparator", node1), "true", StringComparison.OrdinalIgnoreCase));
          if (serviceSetting.EnableClientFilter)
          {
            List<string> list = node1.SelectNodes("ClientFilter[@enabled='True']/Clients/Client").Cast<XmlNode>().Select<XmlNode, string>((Func<XmlNode, string>) (client => ServicesMapping.getAttributeValue("id", client))).ToList<string>();
            serviceSetting.EnabledClients = list.ToArray();
          }
          serviceSettingList.Add(serviceSetting);
        }
        ServicesMapping.settings.Add(str2, serviceSettingList);
      }
    }

    private static string getAttributeValue(string attributeName, XmlNode node)
    {
      try
      {
        return node == null || node.Attributes[attributeName] == null ? "" : node.Attributes[attributeName].Value;
      }
      catch
      {
        return "";
      }
    }

    public static List<string> Categories
    {
      get => new List<string>((IEnumerable<string>) ServicesMapping.settings.Keys);
    }

    public static List<ServiceSetting> GetServiceSetting(string category)
    {
      return ServicesMapping.settings.ContainsKey(category) ? ServicesMapping.settings[category] : new List<ServiceSetting>();
    }

    public static ServiceSetting GetServiceSettingFromFileName(string fileName)
    {
      foreach (string key in ServicesMapping.Categories.ToArray())
      {
        foreach (ServiceSetting settingFromFileName in ServicesMapping.settings[key].ToArray())
        {
          if (settingFromFileName.FilePath == fileName)
            return settingFromFileName;
        }
      }
      return (ServiceSetting) null;
    }

    public static ServiceSetting GetServiceSettingFromID(string id)
    {
      foreach (string key in ServicesMapping.Categories.ToArray())
      {
        foreach (ServiceSetting serviceSettingFromId in ServicesMapping.settings[key].ToArray())
        {
          if (serviceSettingFromId.ID.ToLower() == id.ToLower())
            return serviceSettingFromId;
        }
      }
      return (ServiceSetting) null;
    }
  }
}
