// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.HUDToleranceResolutionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class HUDToleranceResolutionDialog : Form
  {
    private IContainer components;
    private TextBox txtReason;
    private Label label1;
    private DialogButtons dlgButtons;
    private Label label2;
    private DatePicker dpDateResolved;

    public HUDToleranceResolutionDialog()
    {
      this.InitializeComponent();
      this.dpDateResolved.Value = DateTime.Today;
      this.dlgButtons.OKButton.Enabled = false;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.txtReason.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first provide an explanation of how the HUD-1 tolerance violation was resolved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.dpDateResolved.Value == DateTime.MinValue)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must provide the date on which the violation was resolved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string field = Session.LoanData.GetField("3171");
        string val = this.dpDateResolved.Value.ToString("MM/dd/yyyy");
        Session.LoanData.SetField("3171", val);
        if (field == val)
          Session.LoanData.TriggerCalculation("3171", val);
        Session.LoanData.SetField("3172", this.txtReason.Text.Trim());
        Session.LoanData.SetField("3173", Session.UserID);
        foreach (string[] tolereanceAlertField in HUDGFE2010Fields.HudTolereanceAlertFields)
          Session.LoanData.SetField(tolereanceAlertField[1], Session.LoanData.GetField(tolereanceAlertField[0]));
        this.DialogResult = DialogResult.OK;
      }
    }

    private void txtReason_TextChanged(object sender, EventArgs e)
    {
      this.dlgButtons.OKButton.Enabled = this.txtReason.Text.Trim() != "";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtReason = new TextBox();
      this.label1 = new Label();
      this.dlgButtons = new DialogButtons();
      this.label2 = new Label();
      this.dpDateResolved = new DatePicker();
      this.SuspendLayout();
      this.txtReason.Location = new Point(10, 70);
      this.txtReason.Multiline = true;
      this.txtReason.Name = "txtReason";
      this.txtReason.ScrollBars = ScrollBars.Vertical;
      this.txtReason.Size = new Size(362, 99);
      this.txtReason.TabIndex = 0;
      this.txtReason.TextChanged += new EventHandler(this.txtReason_TextChanged);
      this.label1.Location = new Point(9, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(362, 32);
      this.label1.TabIndex = 2;
      this.label1.Text = "Explain the means by which you are resolving the HUD-1 tolerance violation for this loan in the space below.";
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 170);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(383, 44);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 49);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Date Resolved:";
      this.dpDateResolved.BackColor = SystemColors.Window;
      this.dpDateResolved.Location = new Point(93, 46);
      this.dpDateResolved.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpDateResolved.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpDateResolved.Name = "dpDateResolved";
      this.dpDateResolved.Size = new Size(148, 22);
      this.dpDateResolved.TabIndex = 2;
      this.dpDateResolved.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(383, 214);
      this.Controls.Add((Control) this.dpDateResolved);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtReason);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HUDToleranceResolutionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "HUD-1 Tolerance Resolution";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
