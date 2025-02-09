// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMVmCondition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMVmCondition : List<DDMVmFieldPair>
  {
    public int OrdinalId { get; set; }

    public DDMVmCondition(int ordinalId) => this.OrdinalId = ordinalId;

    public string EffectiveDateConditionCode { get; set; }

    public void ProcessEffectiveDate(string effectiveDate)
    {
      if (string.IsNullOrEmpty(effectiveDate))
      {
        this.EffectiveDateConditionCode = "True";
      }
      else
      {
        string[] strArray = effectiveDate.Split('|');
        switch ((strArray.Length >= 3 ? strArray[2] : string.Empty).ToLower())
        {
          case "<":
          case "<=":
          case "=":
          case ">":
          case ">=":
            this.EffectiveDateConditionCode = string.Format("Not String.IsNullOrEmpty([{0}]) AndAlso Convert.ToDateTime([{0}]).Date {1} #{2}#.Date", (object) strArray[0], (object) strArray[2], (object) strArray[3]);
            break;
          case "between":
            if (strArray.Length > 4)
            {
              this.EffectiveDateConditionCode = string.Format("Not String.IsNullOrEmpty([{0}])  AndAlso Convert.ToDateTime([{0}]).Date >= #{1}#.Date AndAlso Convert.ToDateTime([{0}]).Date <= #{2}#.Date", (object) strArray[0], (object) strArray[3], (object) strArray[4]);
              break;
            }
            this.EffectiveDateConditionCode = "True";
            break;
          case "blank":
            this.EffectiveDateConditionCode = string.Format("String.IsNullOrEmpty([{0}])", (object) strArray[0]);
            break;
          case "blank>=":
            if (string.IsNullOrEmpty(strArray[3]))
            {
              this.EffectiveDateConditionCode = string.Format("String.IsNullOrEmpty([{0}])", (object) strArray[0]);
              break;
            }
            this.EffectiveDateConditionCode = string.Format("String.IsNullOrEmpty([{0}]) OrElse (Convert.ToDateTime([{0}]).Date >= #{1}#.Date)", (object) strArray[0], (object) strArray[3]);
            break;
          default:
            this.EffectiveDateConditionCode = "True";
            break;
        }
      }
    }
  }
}
