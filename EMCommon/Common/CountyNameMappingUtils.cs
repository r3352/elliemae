// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CountyNameMappingUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CountyNameMappingUtils
  {
    private static Dictionary<string, Dictionary<string, string>> stateList = new Dictionary<string, Dictionary<string, string>>();

    static CountyNameMappingUtils()
    {
      string empty = string.Empty;
      string filename = !AssemblyResolver.IsSmartClient ? Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, SystemSettings.CountyNameMapRelPath) : AssemblyResolver.GetResourceFileFullPath(SystemSettings.CountyNameMapRelPath);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(filename);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Mappings/Mapping");
      if (xmlNodeList == null || xmlNodeList.Count <= 0)
        return;
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        string key1 = xmlNode.Attributes["StateCode"].Value;
        string key2 = xmlNode.Attributes["Zipcode"].Value;
        string str = xmlNode.Attributes["CountyLimit"].Value;
        if (!CountyNameMappingUtils.stateList.ContainsKey(key1))
          CountyNameMappingUtils.stateList.Add(key1, new Dictionary<string, string>()
          {
            {
              key2,
              str
            }
          });
        else if (!CountyNameMappingUtils.stateList[key1].ContainsKey(key2))
          CountyNameMappingUtils.stateList[key1].Add(key2, str);
        else
          CountyNameMappingUtils.stateList[key1][key2] = str;
      }
    }

    public static string GetCountyLimitCounty(string stateCode, string zipCodeCountyName)
    {
      string str = zipCodeCountyName;
      return !CountyNameMappingUtils.stateList.ContainsKey(stateCode.ToUpper()) || !CountyNameMappingUtils.stateList[stateCode.ToUpper()].ContainsKey(zipCodeCountyName.ToUpper()) ? str : CountyNameMappingUtils.stateList[stateCode][zipCodeCountyName.ToUpper()];
    }
  }
}
