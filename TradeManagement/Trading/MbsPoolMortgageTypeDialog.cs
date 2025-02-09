// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolMortgageTypeDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolMortgageTypeDialog : Form
  {
    private IContainer components;
    private Button btnOk;
    private Button btnCancel;
    private RadioButton rbtnFannie;
    private RadioButton rbtnFreddie;
    private RadioButton rbtnGinnie;
    private RadioButton rbtnFannieMaePE;

    public MbsPoolMortgageType PoolMortgageType { get; set; }

    public MbsPoolMortgageTypeDialog()
    {
      this.InitializeComponent();
      this.Init();
    }

    private void Init()
    {
      if (Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableFMPMandGSE"]))
        return;
      this.rbtnFannieMaePE.Visible = false;
      this.rbtnFannieMaePE.Enabled = false;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.PoolMortgageType = MbsPoolMortgageType.None;
      if (this.rbtnFannie.Checked)
        this.PoolMortgageType = MbsPoolMortgageType.FannieMae;
      if (this.rbtnFreddie.Checked)
        this.PoolMortgageType = MbsPoolMortgageType.FreddieMac;
      if (this.rbtnGinnie.Checked)
        this.PoolMortgageType = MbsPoolMortgageType.GinnieMae;
      if (this.rbtnFannieMaePE.Checked)
        this.PoolMortgageType = MbsPoolMortgageType.FannieMaePE;
      if (this.PoolMortgageType != MbsPoolMortgageType.None)
      {
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select Pool Mortgage Type.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.None;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.rbtnFannie = new RadioButton();
      this.rbtnFreddie = new RadioButton();
      this.rbtnGinnie = new RadioButton();
      this.rbtnFannieMaePE = new RadioButton();
      this.SuspendLayout();
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(35, 117);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 17;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(116, 117);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rbtnFannie.AutoSize = true;
      this.rbtnFannie.Location = new Point(44, 18);
      this.rbtnFannie.Name = "rbtnFannie";
      this.rbtnFannie.Size = new Size(105, 17);
      this.rbtnFannie.TabIndex = 21;
      this.rbtnFannie.TabStop = true;
      this.rbtnFannie.Text = "Fannie Mae Pool";
      this.rbtnFannie.UseVisualStyleBackColor = true;
      this.rbtnFreddie.AutoSize = true;
      this.rbtnFreddie.Location = new Point(44, 41);
      this.rbtnFreddie.Name = "rbtnFreddie";
      this.rbtnFreddie.Size = new Size(108, 17);
      this.rbtnFreddie.TabIndex = 22;
      this.rbtnFreddie.TabStop = true;
      this.rbtnFreddie.Text = "Freddie Mac Pool";
      this.rbtnFreddie.UseVisualStyleBackColor = true;
      this.rbtnGinnie.AutoSize = true;
      this.rbtnGinnie.Location = new Point(44, 64);
      this.rbtnGinnie.Name = "rbtnGinnie";
      this.rbtnGinnie.Size = new Size(103, 17);
      this.rbtnGinnie.TabIndex = 23;
      this.rbtnGinnie.TabStop = true;
      this.rbtnGinnie.Text = "Ginnie Mae Pool";
      this.rbtnGinnie.UseVisualStyleBackColor = true;
      this.rbtnFannieMaePE.AutoSize = true;
      this.rbtnFannieMaePE.Location = new Point(44, 87);
      this.rbtnFannieMaePE.Name = "rbtnFannieMaePE";
      this.rbtnFannieMaePE.Size = new Size(148, 17);
      this.rbtnFannieMaePE.TabIndex = 24;
      this.rbtnFannieMaePE.TabStop = true;
      this.rbtnFannieMaePE.Text = "Fannie Mae PE MBS Pool";
      this.rbtnFannieMaePE.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(224, 151);
      this.Controls.Add((Control) this.rbtnFannieMaePE);
      this.Controls.Add((Control) this.rbtnGinnie);
      this.Controls.Add((Control) this.rbtnFreddie);
      this.Controls.Add((Control) this.rbtnFannie);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MbsPoolMortgageTypeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Pool Mortgage Type";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
