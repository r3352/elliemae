// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AlertPanel : AlertPanelBase
  {
    public AlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    public override bool AllowClearAlert => this.Alert.AlertID == 43 || this.Alert.AlertID == 71;

    protected override void OnClearAlert(EventArgs e)
    {
      if (this.Alert.AlertID == 71)
        Session.LoanData.SetCurrentField("NewVestingNboAlert", "N");
      else
        Session.LoanData.SetCurrentField("4062", "Y");
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      base.OnClearAlert(e);
    }

    private void InitializeComponent()
    {
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.Size = new Size(1086, 331);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1088, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(1008, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(1008, 20);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (AlertPanel);
      this.Size = new Size(1088, 484);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
