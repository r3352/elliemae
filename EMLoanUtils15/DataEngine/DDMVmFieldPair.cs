// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMVmFieldPair
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMVmFieldPair
  {
    public bool RunAlways { get; set; }

    public string FieldId { get; set; }

    public string Value { get; set; }

    public string ValueStripped => this.Value.Replace("\"", "'");

    public string FormattedLine { get; set; }

    public string FeeRuleLine { get; set; }

    public bool NeedConditionWrapper { get; set; }

    public string ConditionWrapper { get; set; }

    public void ProcessCondition()
    {
      int result;
      if (this.FieldId == null || !this.FieldId.Equals("1393") || !int.TryParse(this.Value, out result) || result < 0 || (BizRule.LoanStatus) result > Enum.GetValues(typeof (BizRule.LoanStatus)).Cast<BizRule.LoanStatus>().Max<BizRule.LoanStatus>())
        return;
      switch (result)
      {
        case 0:
          this.FormattedLine = string.Format("String.IsNullOrEmpty([{0}]) OrElse [{0}].Equals(\"Active Loan\")", (object) this.FieldId);
          break;
        case 1:
          this.FormattedLine = string.Format("\"{0}\".Contains(\"|\" + [{1}] + \"|\")", (object) "|Loan Originated|Loan purchased by your institution|", (object) this.FieldId);
          break;
        case 2:
          this.FormattedLine = string.Format("Not (String.IsNullOrEmpty([{0}]) OrElse [{0}].Equals(\"Active Loan\")) AndAlso Not \"{1}\".Contains(\"|\" + [{0}] + \"|\")", (object) this.FieldId, (object) "|Loan Originated|Loan purchased by your institution|");
          break;
      }
    }
  }
}
