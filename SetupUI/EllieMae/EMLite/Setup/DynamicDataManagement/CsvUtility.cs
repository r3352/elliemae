// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.CsvUtility
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public static class CsvUtility
  {
    public static string FormatFieldValuesForCsv(DDMDataTableFieldValue ddmDataTableFieldValue)
    {
      switch (ddmDataTableFieldValue.Criteria)
      {
        case 1:
          return string.Format("<{0}", (object) ddmDataTableFieldValue.Values);
        case 2:
          return string.Format("<={0}", (object) ddmDataTableFieldValue.Values);
        case 3:
          return string.Format(">{0}", (object) ddmDataTableFieldValue.Values);
        case 4:
          return string.Format(">={0}", (object) ddmDataTableFieldValue.Values);
        case 5:
          return string.Format("<>{0}", (object) ddmDataTableFieldValue.Values);
        case 6:
          return ddmDataTableFieldValue.Values.Replace("|", "--");
        case 10:
          return string.Format("Equals({0})", (object) ddmDataTableFieldValue.Values);
        case 11:
          return string.Format("DoesNotEqual({0})", (object) ddmDataTableFieldValue.Values);
        case 12:
          return string.Format("Contains({0})", (object) ddmDataTableFieldValue.Values);
        case 13:
          return string.Format("NotContains({0})", (object) ddmDataTableFieldValue.Values);
        case 14:
          return string.Format("BeginsWith({0})", (object) ddmDataTableFieldValue.Values);
        case 15:
          return string.Format("EndsWith({0})", (object) ddmDataTableFieldValue.Values);
        case 17:
          return string.Format("Adv({0})/Adv", (object) ddmDataTableFieldValue.Values);
        case 24:
          return "";
        case 25:
          return "NoValueInLoanFile";
        case 27:
          return "ClearValueInLoanFile";
        default:
          return ddmDataTableFieldValue.Values;
      }
    }

    public static string EscapeCommasInFieldValues(string fieldValue)
    {
      string str = fieldValue;
      return fieldValue.Contains("\"") || fieldValue.Contains(",") ? "\"" + str.Replace("\"", "\"\"") + "\"" : str;
    }

    public static void WriteRows(TextWriter writer, List<List<string>> tableInfo)
    {
      foreach (List<string> rowInfo in tableInfo)
        CsvUtility.WriteRow(writer, rowInfo);
    }

    public static void WriteRow(TextWriter writer, List<string> rowInfo)
    {
      writer.WriteLine(string.Join(",", (IEnumerable<string>) rowInfo));
    }
  }
}
