// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDAGenerateULIDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HMDAGenerateULIDialog : Form
  {
    private Button okBtn;
    private Button cancelBtn;
    private Label label1;
    private RadioButton reportingRadio;
    private RadioButton profileRadio;
    private IContainer components;
    private LoanData loan;
    private ToolTip fieldToolTip;
    private Panel pnlBody;
    private Sessions.Session session = Session.DefaultInstance;

    public HMDAGenerateULIDialog(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.loan = loan;
      string field1 = this.loan.GetField("HMDA.X106");
      string field2 = this.loan.GetField("HMDA.X70");
      this.reportingRadio.Enabled = !string.IsNullOrEmpty(field1);
      this.profileRadio.Enabled = !string.IsNullOrEmpty(field2);
      if (this.reportingRadio.Enabled && this.profileRadio.Enabled)
      {
        if (this.loan.GetField("HMDA.X105") == "Profile")
          this.profileRadio.Checked = true;
        else
          this.reportingRadio.Checked = true;
      }
      else
      {
        this.reportingRadio.Checked = this.reportingRadio.Enabled;
        this.profileRadio.Checked = this.profileRadio.Enabled;
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HMDAGenerateULIDialog));
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.reportingRadio = new RadioButton();
      this.profileRadio = new RadioButton();
      this.fieldToolTip = new ToolTip(this.components);
      this.pnlBody = new Panel();
      this.pnlBody.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(286, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Please Select LEI to use in generating the ULI for this loan:";
      this.reportingRadio.Location = new Point(12, 26);
      this.reportingRadio.Name = "reportingRadio";
      this.reportingRadio.Size = new Size(121, 20);
      this.reportingRadio.TabIndex = 2;
      this.reportingRadio.Text = "Reporting LEI";
      this.profileRadio.Location = new Point(12, 50);
      this.profileRadio.Name = "profileRadio";
      this.profileRadio.Size = new Size(121, 20);
      this.profileRadio.TabIndex = 3;
      this.profileRadio.Text = "HMDA Profile LEI";
      this.pnlBody.Controls.Add((Control) this.label1);
      this.pnlBody.Controls.Add((Control) this.reportingRadio);
      this.pnlBody.Controls.Add((Control) this.profileRadio);
      this.pnlBody.Location = new Point(12, 12);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(300, 80);
      this.pnlBody.TabIndex = 69;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(139, 106);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(220, 106);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(324, 142);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.pnlBody);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HMDAGenerateULIDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calculate Universal Loan ID";
      this.KeyPress += new KeyPressEventHandler(this.HMDAGenerateULIDialog_KeyPress);
      this.pnlBody.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e) => this.setLoanData(this.loan);

    private void setLoanData(LoanData loanData)
    {
      string val;
      string field;
      if (this.reportingRadio.Checked)
      {
        val = "Reporting";
        field = loanData.GetField("HMDA.X106");
      }
      else
      {
        val = "Profile";
        field = loanData.GetField("HMDA.X70");
      }
      loanData.SetCurrentField("HMDA.X105", val);
      if (loanData.Settings.HMDAInfo != null && loanData.Settings.HMDAInfo.HMDANuli && loanData.GetField("HMDA.X113") == "Y")
        loanData.SetCurrentField("HMDA.X28", loanData.GetField("364"));
      else if (!string.IsNullOrEmpty(field))
      {
        string checkDigit = D1003CalculationServant.GenerateCheckDigit(field, loanData.GetField("364"), loanData.IsLocked("HMDA.X28"));
        if (!string.IsNullOrEmpty(checkDigit))
          loanData.SetCurrentField("HMDA.X28", checkDigit);
      }
      this.loan.Calculator.FormCalculation("CALCURLALOANIDENTIFIER");
    }

    private void HMDAGenerateULIDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    internal void SetScreenDisabled()
    {
      this.setFieldToDisabled(this.pnlBody.Controls);
      this.cancelBtn.Text = "&Close";
      this.okBtn.Visible = false;
    }

    private void setFieldToDisabled(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
          case Button _:
            c.Enabled = false;
            continue;
          default:
            this.setFieldToDisabled(c.Controls);
            continue;
        }
      }
    }
  }
}
