// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FastLoanLoadResponse
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.DataEngine;
using Encompass.Diagnostics.Logging.Schema;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FastLoanLoadResponse : IRemoteCallLogDecorator
  {
    public FastLoanLoadResponse.FastLoanLoadResult Result { get; set; }

    public ILoan Proxy { get; set; }

    public LoanProperty[] LoanProperties { get; set; }

    public LoanData LoanData { get; set; }

    public ILoanSpecificConfigurationInfo LoanSpecificConfigInfo { get; set; }

    public string Status { get; set; }

    public LoanConfigurationParameters ConfigParameters { get; set; }

    public FieldRuleInfo[] FieldRulesInfo { get; set; }

    public TriggerInfo[] TriggersInfo { get; set; }

    public PrintSelectionRuleInfo[] PrintSelectionRulesInfo { get; set; }

    public CustomFieldsInfo CustomFields { get; set; }

    public DDMFeeRule[] DDMFeeRules { get; set; }

    public RoleInfo[] AllRoles { get; set; }

    public DDMFieldRule[] DDMFieldRules { get; set; }

    public PipelineInfo PipelineInfo { get; set; }

    public void Decorate(Log log)
    {
      if (!(this.Proxy is IRemoteCallLogDecorator proxy))
        return;
      proxy.Decorate(log);
    }

    public enum FastLoanLoadResult
    {
      FailedToLock,
      LoadFailed,
      Successful,
    }
  }
}
