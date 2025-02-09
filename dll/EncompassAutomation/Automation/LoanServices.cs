// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.LoanServices
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  public class LoanServices
  {
    private const string className = "LoanServices";
    protected static string sw = Tracing.SwAutomation;

    internal LoanServices()
    {
    }

    public void OrderComplianceReport()
    {
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      try
      {
        service.OrderComplianceReport();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanServices.sw, nameof (LoanServices), TraceLevel.Error, ex.ToString());
        throw new Exception("A loan must aready be open to order a compliance report");
      }
    }
  }
}
