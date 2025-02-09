// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ClosingDateViolationAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ClosingDateViolationAlertPanel : AlertPanelBase
  {
    public ClosingDateViolationAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    protected override void GoToField(string fieldId)
    {
      if (fieldId == "3147")
        Session.Application.GetService<ILoanEditor>().OpenForm("Disclosure Tracking");
      else
        base.GoToField(fieldId);
    }
  }
}
