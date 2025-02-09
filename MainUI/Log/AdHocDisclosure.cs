// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AdHocDisclosure
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AdHocDisclosure : Form
  {
    private DisclosureTrackingLog newLog;
    private IContainer components;
    private Button btnNo;
    private Button btnYes;
    private Label label1;
    private Panel panel2;
    private CheckBox chkGFE;
    private Label label3;
    private CheckBox chkTIL;
    private CheckBox chkSafeHarbor;

    public AdHocDisclosure() => this.InitializeComponent();

    private void btnYes_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      Session.LoanData.Calculator.CalcPaymentSchedule();
      this.newLog = new DisclosureTrackingLog(DateTime.Now, Session.LoanData, this.chkGFE.Checked, this.chkTIL.Checked, true, this.chkSafeHarbor.Checked);
      this.newLog.DisclosedBy = Session.UserInfo.Userid;
      this.newLog.DisclosedByFullName = Session.UserInfo.FullName;
      this.newLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
      this.newLog.BorrowerPairID = Session.LoanData.CurrentBorrowerPair.Id;
      if (this.chkGFE.Checked)
        this.newLog.AddDisclosedFormItem("2010 GFE", DisclosureTrackingFormItem.FormType.StandardForm);
      if (this.chkTIL.Checked)
      {
        foreach (string tilFormName in DisclosureTrackingConsts.TILFormNames)
          this.newLog.AddDisclosedFormItem(tilFormName, DisclosureTrackingFormItem.FormType.StandardForm);
      }
      if (this.chkSafeHarbor.Checked)
      {
        foreach (string safeHarborFormName in DisclosureTrackingConsts.NewSafeHarborFormNames)
          this.newLog.AddDisclosedFormItem(safeHarborFormName, DisclosureTrackingFormItem.FormType.StandardForm);
      }
      Cursor.Current = Cursors.Default;
      this.DialogResult = DialogResult.Yes;
    }

    public DisclosureTrackingLog Log => this.newLog;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnNo = new Button();
      this.btnYes = new Button();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.chkTIL = new CheckBox();
      this.chkGFE = new CheckBox();
      this.label3 = new Label();
      this.chkSafeHarbor = new CheckBox();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(259, 98);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 25);
      this.btnNo.TabIndex = 5;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(178, 98);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 25);
      this.btnYes.TabIndex = 4;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(328, 14);
      this.label1.TabIndex = 9;
      this.label1.Text = "Do you want to record a disclosure record in Disclosure Tracking?";
      this.panel2.Controls.Add((Control) this.chkSafeHarbor);
      this.panel2.Controls.Add((Control) this.chkTIL);
      this.panel2.Controls.Add((Control) this.chkGFE);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Location = new Point(18, 37);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(316, 55);
      this.panel2.TabIndex = 13;
      this.chkTIL.AutoSize = true;
      this.chkTIL.Location = new Point(222, 7);
      this.chkTIL.Name = "chkTIL";
      this.chkTIL.Size = new Size(78, 18);
      this.chkTIL.TabIndex = 2;
      this.chkTIL.Text = "REGZ - TIL";
      this.chkTIL.UseVisualStyleBackColor = true;
      this.chkGFE.AutoSize = true;
      this.chkGFE.Location = new Point(108, 7);
      this.chkGFE.Name = "chkGFE";
      this.chkGFE.Size = new Size(73, 18);
      this.chkGFE.TabIndex = 1;
      this.chkGFE.Text = "2010 GFE";
      this.chkGFE.UseVisualStyleBackColor = true;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(-2, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(101, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Disclosure includes";
      this.chkSafeHarbor.AutoSize = true;
      this.chkSafeHarbor.Location = new Point(108, 31);
      this.chkSafeHarbor.Name = "chkSafeHarbor";
      this.chkSafeHarbor.Size = new Size(85, 18);
      this.chkSafeHarbor.TabIndex = 3;
      this.chkSafeHarbor.Text = "Safe Harbor";
      this.chkSafeHarbor.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(352, 137);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AdHocDisclosure);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
