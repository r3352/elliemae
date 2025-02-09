// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMVmDataTableFieldPair
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMVmDataTableFieldPair
  {
    public int Criteria { get; set; }

    public string FieldId { get; set; }

    public string[] Values { get; set; }

    public string FormattedLine { get; set; }

    public SessionObjects SessionObjects { get; set; }

    public void Process()
    {
      FieldDefinition fieldDefinition = DDM_FieldAccess_Utils.GetFieldDefinition(this.FieldId, this.SessionObjects);
      string[] formattedValues = this.formatFieldValues(this.FieldId, this.Values, fieldDefinition);
      for (int index = 0; index < formattedValues.Length; ++index)
        formattedValues[index] = Utils.EscapeDoubleQuotesForVB(formattedValues[index]);
      string fieldId;
      switch (fieldDefinition.Format)
      {
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          fieldId = "#" + this.FieldId;
          break;
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          fieldId = "@" + this.FieldId;
          break;
        default:
          fieldId = this.FieldId;
          break;
      }
      switch (this.Criteria)
      {
        case 0:
          this.FormattedLine = string.Format("[{0}] = {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 1:
          this.FormattedLine = string.Format("[{0}] < {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 2:
          this.FormattedLine = string.Format("[{0}] <= {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 3:
          this.FormattedLine = string.Format("[{0}] > {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 4:
          this.FormattedLine = string.Format("[{0}] >= {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 5:
          this.FormattedLine = string.Format("[{0}] <> {1}", (object) fieldId, (object) formattedValues[0]);
          break;
        case 6:
          this.FormattedLine = string.Format("[{0}] >= {1} AND [{0}] <= {2}", (object) fieldId, (object) formattedValues[0], (object) formattedValues[1]);
          break;
        case 7:
          this.FormattedLine = string.Format("\"{0}\".Contains(\"|\" + [{1}] + \"|\")", (object) ("|" + string.Join("|", formattedValues) + "|"), (object) fieldId);
          break;
        case 8:
        case 19:
        case 21:
          this.FormattedLine = string.Format("[{0}].ToLower() = \"{1}\".ToLower()", (object) fieldId, (object) formattedValues[0]);
          break;
        case 9:
        case 20:
        case 22:
        case 23:
          this.FormattedLine = string.Format("\"{0}\".ToLower().Contains(\"|\" + [{1}].ToLower() + \"|\")", (object) ("|" + string.Join("|", formattedValues) + "|"), (object) fieldId);
          break;
        case 10:
          this.FormattedLine = string.Format("String.Compare([{0}], \"{1}\", True) = 0", (object) fieldId, (object) formattedValues[0]);
          break;
        case 11:
          this.FormattedLine = string.Format("String.Compare([{0}], \"{1}\", True) <> 0", (object) fieldId, (object) formattedValues[0]);
          break;
        case 12:
          this.FormattedLine = string.Format("[{0}].Contains(\"{1}\")", (object) fieldId, (object) formattedValues[0]);
          break;
        case 13:
          this.FormattedLine = string.Format("Not [{0}].Contains(\"{1}\")", (object) fieldId, (object) formattedValues[0]);
          break;
        case 14:
          this.FormattedLine = string.Format("[{0}].StartsWith(\"{1}\", StringComparison.InvariantCultureIgnoreCase)", (object) fieldId, (object) formattedValues[0]);
          break;
        case 15:
          this.FormattedLine = string.Format("[{0}].EndsWith(\"{1}\", StringComparison.InvariantCultureIgnoreCase)", (object) fieldId, (object) formattedValues[0]);
          break;
        case 18:
          this.FormattedLine = this.buildFormattedLineForListOfValues(fieldId, formattedValues);
          break;
        case 25:
          this.FormattedLine = string.Format("String.IsNullOrEmpty([{0}]) Or String.Compare(Convert.ToString([{0}]), \"1/1/0001 12:00:00 AM\", True) = 0", (object) this.FieldId);
          break;
      }
    }

    private string buildFormattedLineForListOfValues(string fieldId, string[] formattedValues)
    {
      for (int index = 0; index < formattedValues.Length; ++index)
        formattedValues[index] = string.Format("[{0}] = \"{1}\"", (object) fieldId, (object) formattedValues[index]);
      return "( " + string.Join(" OR ", formattedValues) + " )";
    }

    private string[] formatFieldValues(string fieldId, string[] values, FieldDefinition fldInfo)
    {
      return fldInfo.Format == FieldFormat.DATE ? ((IEnumerable<string>) values).Select<string, string>((Func<string, string>) (value => !string.IsNullOrEmpty(value) ? "#" + value + "#" : "")).ToArray<string>() : values;
    }
  }
}
