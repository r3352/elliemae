// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.MaventLicenseTypesUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class MaventLicenseTypesUtils
  {
    private static Hashtable maventLicenseTable = new Hashtable();
    private static Hashtable licenseKeyToNameTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable licenseNameToKeyTable = CollectionsUtil.CreateCaseInsensitiveHashtable();

    static MaventLicenseTypesUtils()
    {
      string empty1 = string.Empty;
      string path = !AssemblyResolver.IsSmartClient ? Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, SystemSettings.MaventLicenseTypesRelPath) : AssemblyResolver.GetResourceFileFullPath(SystemSettings.MaventLicenseTypesRelPath);
      if (!File.Exists(path))
        return;
      string empty2 = string.Empty;
      string end;
      try
      {
        StreamReader streamReader = new StreamReader((Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
        end = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch (Exception ex)
      {
        return;
      }
      if (end == string.Empty)
        return;
      string[] strArray1 = end.Split('\r');
      if (strArray1.Length == 0)
        return;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        string[] strArray2 = strArray1[index].Split('\t');
        if (strArray2.Length >= 3)
        {
          string str = strArray2[0].Replace("\n", "");
          string key1 = strArray2[1];
          string licenseName = strArray2[2].Replace("\n", "");
          if (string.Compare(str, "state", true) != 0)
          {
            if (!MaventLicenseTypesUtils.maventLicenseTable.ContainsKey((object) str))
              MaventLicenseTypesUtils.maventLicenseTable.Add((object) str, (object) new List<string>());
            List<string> stringList = (List<string>) MaventLicenseTypesUtils.maventLicenseTable[(object) str];
            if (!stringList.Contains(licenseName))
              stringList.Add(licenseName);
            if (!MaventLicenseTypesUtils.licenseKeyToNameTable.ContainsKey((object) key1))
              MaventLicenseTypesUtils.licenseKeyToNameTable.Add((object) key1, (object) licenseName);
            string key2 = MaventLicenseTypesUtils.buildStateNamePlusLicenseName(str, licenseName);
            if (!MaventLicenseTypesUtils.licenseNameToKeyTable.ContainsKey((object) key2))
              MaventLicenseTypesUtils.licenseNameToKeyTable.Add((object) key2, (object) key1);
          }
        }
      }
    }

    public static string[] GetLicenseTypes(string stateAbbrevation)
    {
      return MaventLicenseTypesUtils.maventLicenseTable.ContainsKey((object) stateAbbrevation) ? ((List<string>) MaventLicenseTypesUtils.maventLicenseTable[(object) stateAbbrevation]).ToArray() : (string[]) null;
    }

    public static string GetLicenseName(string licenseKey)
    {
      return MaventLicenseTypesUtils.licenseKeyToNameTable.ContainsKey((object) licenseKey) ? MaventLicenseTypesUtils.licenseKeyToNameTable[(object) licenseKey].ToString() : string.Empty;
    }

    public static string GetLicenseKey(string stateName, string licenseName)
    {
      string key = MaventLicenseTypesUtils.buildStateNamePlusLicenseName(stateName, licenseName);
      return MaventLicenseTypesUtils.licenseNameToKeyTable.ContainsKey((object) key) ? MaventLicenseTypesUtils.licenseNameToKeyTable[(object) key].ToString() : string.Empty;
    }

    private static string buildStateNamePlusLicenseName(string stateName, string licenseName)
    {
      return stateName + "_" + licenseName;
    }
  }
}
