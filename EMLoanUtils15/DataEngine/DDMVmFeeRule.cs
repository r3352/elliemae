// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMVmFeeRule
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMVmFeeRule : DDMVmRuleBase
  {
    public ByPassCondition _byPassMode;

    public string LineId { get; set; }

    public string Name { get; set; }

    public List<DDMVmCondition> Conditions { get; set; }

    public List<DDMVmAction> Actions { get; set; }

    public int ByPassMode => (int) this._byPassMode;

    public string ByPassAdvancedCode { get; set; }

    public DDMVmFeeRule()
    {
      this.Conditions = new List<DDMVmCondition>();
      this.Actions = new List<DDMVmAction>();
      this._byPassMode = ByPassCondition.None;
    }

    public DDMVmFeeRule(string lineId, string name)
      : this()
    {
      this.LineId = lineId;
      this.Name = name;
    }

    public void EnableByPassCondition(ByPassCondition cond, bool reset = false)
    {
      if (reset)
        this._byPassMode = cond;
      else
        this._byPassMode |= cond;
    }
  }
}
