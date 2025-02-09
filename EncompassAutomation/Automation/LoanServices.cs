// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.LoanServices
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Provides access to the LoanServices.</summary>
  public class LoanServices
  {
    private const string className = "LoanServices";
    /// <summary>TraceSwitch</summary>
    protected static string sw = Tracing.SwAutomation;

    internal LoanServices()
    {
    }

    /// <summary>Triggers the process of ordering a compliance report.</summary>
    /// <remarks>This method should only be called after a loan has fully loaded in Encompass.
    /// Calling this method as a result of the EncompassApplication.LoanOpened event will throw an exception.
    /// Calling this method causes the loan to be commited. Calling this method in the Loan's BeforeCommit or Commited
    /// event will cause an infinite loop.</remarks>
    /// <example>
    ///       The following code demonstrates how to to automatically order a compliance report from a plugin.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using EllieMae.Encompass.Automation;
    /// using EllieMae.Encompass.ComponentModel;
    /// 
    /// namespace TestPlugin
    /// {
    ///     [Plugin]
    ///     public class TestPlugin
    ///     {
    ///         public TestPlugin()
    ///         {
    ///             EncompassApplication.LoanOpened += EncompassApplication_LoanOpened;
    ///         }
    /// 
    ///         private void EncompassApplication_LoanOpened(object sender, EventArgs e)
    ///         {
    ///             EncompassApplication.CurrentLoan.FieldChange += CurrentLoan_FieldChange;
    ///         }
    /// 
    ///         private void CurrentLoan_FieldChange(object source, EllieMae.Encompass.BusinessObjects.Loans.FieldChangeEventArgs e)
    ///         {
    ///             if (e.FieldID == "4002")
    ///             {
    ///                 EncompassApplication.LoanServices.OrderComplianceReport();
    ///             }
    ///         }
    ///     }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
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
