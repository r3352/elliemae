// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DiagnosticsEnablementForm
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
  public class DiagnosticsEnablementForm : Form
  {
    private IContainer components;
    private Button btnOK;
    private TextBox txtCaseNumber;
    private Label label2;
    private Button btnCancel;
    private Label label1;
    private CheckBox chkAutoSubmit;

    public DiagnosticsEnablementForm() => this.InitializeComponent();

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtCaseNumber.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Enter your ICE Mortgage Technology Customer Support Case Number in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          DiagnosticSession.CaseNumber = this.txtCaseNumber.Text;
          DiagnosticSession.DiagnosticsMode = this.chkAutoSubmit.Checked ? DiagnosticsMode.AutoSubmit : DiagnosticsMode.PreserveLocal;
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You must exit and restart Encompass to begin gathering diagnostic data.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Error while setting up diagnostics: " + (object) ex);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DiagnosticsEnablementForm));
      this.btnOK = new Button();
      this.txtCaseNumber = new TextBox();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.chkAutoSubmit = new CheckBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(365, 122);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtCaseNumber.Location = new Point(84, 81);
      this.txtCaseNumber.MaxLength = 50;
      this.txtCaseNumber.Name = "txtCaseNumber";
      this.txtCaseNumber.Size = new Size(156, 20);
      this.txtCaseNumber.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 84);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 14);
      this.label2.TabIndex = 12;
      this.label2.Text = "Case Number";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(440, 122);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(508, 59);
      this.label1.TabIndex = 10;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.chkAutoSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAutoSubmit.AutoSize = true;
      this.chkAutoSubmit.Checked = true;
      this.chkAutoSubmit.CheckState = CheckState.Checked;
      this.chkAutoSubmit.Location = new Point(10, 125);
      this.chkAutoSubmit.Name = "chkAutoSubmit";
      this.chkAutoSubmit.Size = new Size(352, 18);
      this.chkAutoSubmit.TabIndex = 2;
      this.chkAutoSubmit.Text = "Submit data to ICE Mortgage Technology when you exit Encompass.";
      this.chkAutoSubmit.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(524, 153);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtCaseNumber);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkAutoSubmit);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DiagnosticsEnablementForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Diagnostics";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
