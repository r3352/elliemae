// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.VADiscountChargeViolationAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class VADiscountChargeViolationAlertPanel : AlertPanelBase
  {
    private IContainer components;

    public VADiscountChargeViolationAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.Size = new Size(1026, 390);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1028, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(948, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(948, 20);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (VADiscountChargeViolationAlertPanel);
      this.Size = new Size(1028, 543);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
