// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.Alerts.AtLockDisclosureRequirementsAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log.Alerts
{
  public class AtLockDisclosureRequirementsAlertPanel : AlertPanelWithDataCompletionFields
  {
    private IContainer components;

    public override bool AllowClearAlert => false;

    public AtLockDisclosureRequirementsAlertPanel(PipelineInfo.Alert alert)
    {
      this.InitializeComponent();
      this.SetAlert(alert);
      Session.LoanData?.Calculator?.CalcAtLockDisclosureDate();
      this.SetDataCompletionDateControls("Disclosure Ready Date", Utils.ParseDate((object) Session.LoanData.GetField("5003")));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
