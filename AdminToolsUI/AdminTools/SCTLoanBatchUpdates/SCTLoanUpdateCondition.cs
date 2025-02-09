// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanUpdateCondition
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates
{
  public class SCTLoanUpdateCondition
  {
    private Dictionary<string, string> loanStatusList;
    private List<object[]> loanInfos;

    public SCTLoanUpdateCondition()
    {
      this.loanInfos = new List<object[]>();
      this.loanStatusList = new Dictionary<string, string>();
    }

    public void AddStatus(object[] loanInfo, string processStatus)
    {
      this.loanInfos.Add(loanInfo);
      string key = loanInfo[0].ToString();
      if (!(key != "") || this.loanStatusList.ContainsKey(key))
        return;
      this.loanStatusList.Add(key, processStatus);
    }

    public bool ProcessPendingList { get; set; }

    public SCTLoanUpdateCondition.UpdateOptions UpdateOption { get; set; }

    public string CSVFile { get; set; }

    public enum UpdateOptions
    {
      UpdateOnly,
    }
  }
}
