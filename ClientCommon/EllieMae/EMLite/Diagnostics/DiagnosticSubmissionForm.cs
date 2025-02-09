// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DiagnosticSubmissionForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public class DiagnosticSubmissionForm : Form
  {
    private DiagnosticSession session;
    private IContainer components;
    private Label label1;
    private Button btnCancel;
    private Label label2;
    private TextBox txtCaseNumber;
    private Button btnSubmit;
    private CheckBox chkDisableDiagnostics;
    private Label label4;
    private TextBox txtMessage;
    private Label label6;

    public DiagnosticSubmissionForm(DiagnosticSession session)
    {
      this.session = session;
      this.InitializeComponent();
      this.txtCaseNumber.Text = DiagnosticSession.CaseNumber;
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      if (this.txtCaseNumber.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Enter your Support Case Number in the space provided. If you do not have a case number, contact ICE Mortgage Technology Customer Support at 800-777-1718 to obtain one.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          DiagnosticSession.CaseNumber = this.txtCaseNumber.Text;
          this.session.SetVariable("Message", this.txtMessage.Text);
          if (this.chkDisableDiagnostics.Checked)
            DiagnosticSession.DiagnosticsMode = DiagnosticsMode.Disabled;
          this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An error has occurred: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DiagnosticSubmissionForm));
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.label2 = new Label();
      this.txtCaseNumber = new TextBox();
      this.btnSubmit = new Button();
      this.chkDisableDiagnostics = new CheckBox();
      this.label4 = new Label();
      this.txtMessage = new TextBox();
      this.label6 = new Label();
      this.SuspendLayout();
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(368, 49);
      this.label1.TabIndex = 0;
      this.label1.Text = "Your diagnostic session results are now ready to be submitted to ICE Mortgage Technology Customer Support for review. Verify that the Client ID and Case Number listed below are correct and then click Submit.";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(303, 180);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 72);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Case Number";
      this.txtCaseNumber.Location = new Point(97, 69);
      this.txtCaseNumber.MaxLength = 50;
      this.txtCaseNumber.Name = "txtCaseNumber";
      this.txtCaseNumber.Size = new Size(157, 20);
      this.txtCaseNumber.TabIndex = 2;
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(228, 180);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(75, 22);
      this.btnSubmit.TabIndex = 5;
      this.btnSubmit.Text = "&Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.chkDisableDiagnostics.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkDisableDiagnostics.AutoSize = true;
      this.chkDisableDiagnostics.Checked = true;
      this.chkDisableDiagnostics.CheckState = CheckState.Checked;
      this.chkDisableDiagnostics.Location = new Point(10, 183);
      this.chkDisableDiagnostics.Name = "chkDisableDiagnostics";
      this.chkDisableDiagnostics.Size = new Size(146, 18);
      this.chkDisableDiagnostics.TabIndex = 4;
      this.chkDisableDiagnostics.Text = "Turn off diagnostic mode";
      this.chkDisableDiagnostics.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 94);
      this.label4.Name = "label4";
      this.label4.Size = new Size(51, 14);
      this.label4.TabIndex = 8;
      this.label4.Text = "Message";
      this.txtMessage.AcceptsReturn = true;
      this.txtMessage.Location = new Point(97, 91);
      this.txtMessage.MaxLength = 500;
      this.txtMessage.Multiline = true;
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.ScrollBars = ScrollBars.Vertical;
      this.txtMessage.Size = new Size(279, 70);
      this.txtMessage.TabIndex = 3;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.ForeColor = Color.FromArgb(238, 0, 0);
      this.label6.Location = new Point(77, 71);
      this.label6.Name = "label6";
      this.label6.Size = new Size(13, 16);
      this.label6.TabIndex = 10;
      this.label6.Text = "*";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(387, 211);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtMessage);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnSubmit);
      this.Controls.Add((Control) this.txtCaseNumber);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkDisableDiagnostics);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (DiagnosticSubmissionForm);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Diagnostics Submission";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
