// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PipelineElement
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class PipelineElement : Element
  {
    private Control parentControl;
    private PipelineElementData data;

    public PipelineElement(Control parentControl, PipelineElementData data)
    {
      this.parentControl = parentControl;
      this.data = data;
    }

    public PipelineElementData CurrentData => this.data;

    public Control ParentControl => this.parentControl;

    public override string ToString()
    {
      return this.data != null ? string.Concat(this.data.GetValue()) : base.ToString();
    }

    protected LoanData GetLoanData()
    {
      if (this.data == null)
        return Session.LoanData;
      using (CursorActivator.Wait())
      {
        using (ILoan loan = Session.LoanManager.OpenLoan(this.data.PipelineInfo.GUID))
        {
          if (loan != null)
            return loan.GetLoanData(false);
          int num = (int) Utils.Dialog((IWin32Window) this.parentControl, "The loan \"" + this.data.PipelineInfo.LoanDisplayString + "\" has been deleted or is no longer accessible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return (LoanData) null;
        }
      }
    }

    protected bool OpenLoan()
    {
      return this.data == null || Session.Application.GetService<ILoanConsole>().OpenLoan(this.data.PipelineInfo.GUID);
    }
  }
}
