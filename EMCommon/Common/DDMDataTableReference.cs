// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DDMDataTableReference
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class DDMDataTableReference
  {
    public int RuleType { get; set; }

    public string RuleName { get; set; }

    public string ScenarioName { get; set; }

    public int ReferenceCount { get; set; }

    public DDMDataTableReference(int ruleType, string ruleName, string scenarioName, int refCount)
    {
      this.RuleType = ruleType;
      this.RuleName = ruleName;
      this.ScenarioName = scenarioName;
      this.ReferenceCount = refCount;
    }
  }
}
