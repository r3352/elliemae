// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.ImportCustomFormUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class ImportCustomFormUtil
  {
    private const string className = "ImportCustomFormsUtil";
    private static readonly string sw = Tracing.SwImportExport;
    private static Hashtable fieldMaps = new Hashtable();

    static ImportCustomFormUtil() => ImportCustomFormUtil.LoadMap();

    private static void LoadMap()
    {
      StreamReader streamReader = new StreamReader((Stream) File.Open(AssemblyResolver.GetResourceFileFullPath(SystemSettings.PointMapFileRelPath, SystemSettings.LocalAppDir), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
      string str;
      while ((str = streamReader.ReadLine()) != null)
      {
        try
        {
          if (!str.StartsWith("//"))
          {
            string[] strArray = str.Split('|');
            if (strArray.Length == 1)
            {
              if (strArray[0] == string.Empty)
                continue;
            }
            if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) strArray[0]))
              ImportCustomFormUtil.fieldMaps.Add((object) strArray[0], (object) strArray[1]);
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportCustomFormUtil.sw, TraceLevel.Error, "ImportCustomFormsUtil", "Exception in reading map file: " + ex.Message + "   \n line: " + str + "\r\n");
        }
      }
      if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1240"))
        ImportCustomFormUtil.fieldMaps.Add((object) "1240", (object) "1811");
      if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1242"))
        ImportCustomFormUtil.fieldMaps.Add((object) "1242", (object) "420");
      if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1241"))
        ImportCustomFormUtil.fieldMaps.Add((object) "1241", (object) "19");
      if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1244"))
        ImportCustomFormUtil.fieldMaps.Add((object) "1244", (object) "1172");
      if (!ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1243"))
        ImportCustomFormUtil.fieldMaps.Add((object) "1243", (object) "608");
      if (ImportCustomFormUtil.fieldMaps.ContainsKey((object) "101"))
        ImportCustomFormUtil.fieldMaps[(object) "101"] = (object) "37";
      if (ImportCustomFormUtil.fieldMaps.ContainsKey((object) "102"))
        ImportCustomFormUtil.fieldMaps[(object) "102"] = (object) "FR0104";
      if (ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1508"))
        ImportCustomFormUtil.fieldMaps[(object) "1508"] = (object) "317";
      if (ImportCustomFormUtil.fieldMaps.ContainsKey((object) "1509"))
        ImportCustomFormUtil.fieldMaps[(object) "1509"] = (object) "1406";
      streamReader.Close();
    }

    public static string GetEMFieldFromPointField(string field)
    {
      string key = string.Empty;
      if (field.IndexOf("_") > -1)
        key = field.Substring(field.IndexOf("_") + 1);
      if (ImportCustomFormUtil.fieldMaps.ContainsKey((object) key))
      {
        string str = ImportCustomFormUtil.fieldMaps[(object) key].ToString();
        int length = str.IndexOf(",");
        if (length > -1)
          str = str.Substring(0, length);
        field = "M_" + str.Replace(".", "dot");
      }
      else
        field = "";
      return field;
    }
  }
}
