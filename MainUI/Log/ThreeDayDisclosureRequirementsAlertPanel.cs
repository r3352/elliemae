// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ThreeDayDisclosureRequirementsAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ThreeDayDisclosureRequirementsAlertPanel : AlertPanelWithDataCompletionFields
  {
    private IContainer components;

    public override bool AllowClearAlert => false;

    public ThreeDayDisclosureRequirementsAlertPanel(PipelineInfo.Alert alert)
    {
      this.InitializeComponent();
      this.SetAlert(alert);
      this.SetAlertDate(Utils.ParseDate((object) Session.LoanData.GetField("3142")));
      Session.LoanData?.Calculator?.CalcDisclosureReadyDate();
      this.SetDataCompletionDateControls("Disclosure Ready Date", Utils.ParseDate((object) Session.LoanData.GetField("5001")));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpFields.SuspendLayout();
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (ThreeDayDisclosureRequirementsAlertPanel);
      this.Size = new Size(972, 462);
      this.grpFields.ResumeLayout(false);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.pnlFields.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
