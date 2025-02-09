// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.EditConcessionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

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
  public class EditConcessionDialog : Form
  {
    private bool readOnly = true;
    public string concession = string.Empty;
    public string approvalDate = string.Empty;
    public string approvedBy = string.Empty;
    public string reason = string.Empty;
    private bool use10Digits;
    private IContainer components;
    private Button buttonOK;
    private Button buttonCancel;
    private TextBox textConcession;
    private Label label7;
    private Label label1;
    private Label label2;
    private TextBox textApprovedBy;
    private TextBox textConcessionReason;
    private Label label3;
    private Button btnBuyComment;
    private DatePicker textApprovalDate;

    public EditConcessionDialog(
      bool readOnly,
      string dlgTitle,
      string concession,
      string approvalDate,
      string approvedBy,
      string reason,
      bool use10Digits)
    {
      this.InitializeComponent();
      this.readOnly = readOnly;
      this.Text = dlgTitle;
      if (!string.IsNullOrEmpty(concession) && !string.IsNullOrEmpty(concession.Trim()))
      {
        double num = Utils.ParseDouble((object) concession.Trim());
        this.textConcession.Text = use10Digits ? num.ToString("N10") : num.ToString("N3");
      }
      else
        this.textConcession.Text = concession;
      this.textApprovalDate.Text = approvalDate;
      this.textApprovedBy.Text = approvedBy;
      this.textConcessionReason.Text = reason;
      this.use10Digits = use10Digits;
      if (!this.readOnly)
        return;
      this.textConcession.ReadOnly = true;
      this.textApprovalDate.ReadOnly = true;
      this.textApprovedBy.ReadOnly = true;
      this.textConcessionReason.ReadOnly = true;
      this.btnBuyComment.Enabled = false;
    }

    private void btnBuyComment_Click(object sender, EventArgs e)
    {
      using (AddCommentForm addCommentForm = new AddCommentForm(Session.UserInfo.FullName))
      {
        if (addCommentForm.ShowDialog((IWin32Window) this) != DialogResult.OK || addCommentForm.NewComments == string.Empty)
          return;
        string empty = string.Empty;
        string str1 = this.textConcessionReason.Text.Trim();
        if (str1 != string.Empty)
          str1 += "\r\n\r\n";
        string str2 = str1 + addCommentForm.NewComments;
        this.textConcessionReason.Text = str2;
        this.textConcessionReason.SelectionStart = str2.Length;
        this.textConcessionReason.ScrollToCaret();
        this.textConcessionReason.Focus();
      }
    }

    private void numField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text.Trim());
      if (!(textBox.Text.Trim() != string.Empty))
        return;
      if (this.use10Digits)
        textBox.Text = num.ToString("N10");
      else
        textBox.Text = num.ToString("N3");
    }

    private void num_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void num_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = this.use10Digits ? FieldFormat.DECIMAL_10 : FieldFormat.DECIMAL_3;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void strField_Leave(object sender, EventArgs e)
    {
      string empty = string.Empty;
      string str = string.Empty;
      switch (sender)
      {
        case TextBox _:
          TextBox textBox = (TextBox) sender;
          empty = textBox.Tag.ToString();
          str = textBox.Text.Trim();
          break;
        case DatePicker _:
          DatePicker datePicker = (DatePicker) sender;
          if (datePicker == null || ((string) datePicker.Tag ?? "") == "")
            break;
          str = datePicker.Text.Trim();
          break;
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.concession = this.textConcession.Text.Trim();
      this.approvalDate = this.textApprovalDate.Text.Trim();
      this.approvedBy = this.textApprovedBy.Text.Trim();
      this.reason = this.textConcessionReason.Text.Trim();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.textConcession = new TextBox();
      this.label7 = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.textApprovedBy = new TextBox();
      this.textConcessionReason = new TextBox();
      this.label3 = new Label();
      this.btnBuyComment = new Button();
      this.textApprovalDate = new DatePicker();
      this.SuspendLayout();
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(88, 262);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(75, 23);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "&OK";
      this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(169, 262);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "&Cancel";
      this.textConcession.Font = new Font("Arial", 8.25f);
      this.textConcession.Location = new Point(145, 17);
      this.textConcession.Name = "textConcession";
      this.textConcession.Size = new Size(170, 20);
      this.textConcession.TabIndex = 0;
      this.textConcession.Tag = (object) "";
      this.textConcession.TextAlign = HorizontalAlignment.Right;
      this.textConcession.Leave += new EventHandler(this.numField_Leave);
      this.textConcession.KeyUp += new KeyEventHandler(this.num_KeyUp);
      this.textConcession.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f);
      this.label7.Location = new Point(12, 20);
      this.label7.Name = "label7";
      this.label7.Size = new Size(91, 14);
      this.label7.TabIndex = 49;
      this.label7.Text = "Price Concession";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f);
      this.label1.Location = new Point(12, 49);
      this.label1.Name = "label1";
      this.label1.Size = new Size(76, 14);
      this.label1.TabIndex = 51;
      this.label1.Text = "Approval Date";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(12, 78);
      this.label2.Name = "label2";
      this.label2.Size = new Size(71, 14);
      this.label2.TabIndex = 53;
      this.label2.Text = "Approved By";
      this.textApprovedBy.Font = new Font("Arial", 8.25f);
      this.textApprovedBy.Location = new Point(145, 75);
      this.textApprovedBy.MaxLength = (int) byte.MaxValue;
      this.textApprovedBy.Name = "textApprovedBy";
      this.textApprovedBy.Size = new Size(170, 20);
      this.textApprovedBy.TabIndex = 2;
      this.textApprovedBy.Tag = (object) "";
      this.textApprovedBy.Leave += new EventHandler(this.strField_Leave);
      this.textConcessionReason.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textConcessionReason.Location = new Point(15, 125);
      this.textConcessionReason.Multiline = true;
      this.textConcessionReason.Name = "textConcessionReason";
      this.textConcessionReason.ScrollBars = ScrollBars.Both;
      this.textConcessionReason.Size = new Size(300, 88);
      this.textConcessionReason.TabIndex = 3;
      this.textConcessionReason.Tag = (object) "";
      this.textConcessionReason.Leave += new EventHandler(this.strField_Leave);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f);
      this.label3.Location = new Point(12, 108);
      this.label3.Name = "label3";
      this.label3.Size = new Size(107, 14);
      this.label3.TabIndex = 93;
      this.label3.Text = "Reason for Approval";
      this.btnBuyComment.BackColor = SystemColors.Control;
      this.btnBuyComment.Location = new Point(198, 219);
      this.btnBuyComment.Name = "btnBuyComment";
      this.btnBuyComment.Size = new Size(117, 22);
      this.btnBuyComment.TabIndex = 4;
      this.btnBuyComment.Text = "Add Comments";
      this.btnBuyComment.UseVisualStyleBackColor = true;
      this.btnBuyComment.Click += new EventHandler(this.btnBuyComment_Click);
      this.textApprovalDate.BackColor = SystemColors.Window;
      this.textApprovalDate.Location = new Point(146, 46);
      this.textApprovalDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textApprovalDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textApprovalDate.Name = "textApprovalDate";
      this.textApprovalDate.Size = new Size(169, 21);
      this.textApprovalDate.TabIndex = 1;
      this.textApprovalDate.Tag = (object) "";
      this.textApprovalDate.ToolTip = "";
      this.textApprovalDate.Value = new DateTime(0L);
      this.textApprovalDate.ValueChanged += new EventHandler(this.strField_Leave);
      this.textApprovalDate.Leave += new EventHandler(this.strField_Leave);
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(332, 297);
      this.Controls.Add((Control) this.textApprovalDate);
      this.Controls.Add((Control) this.btnBuyComment);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textConcessionReason);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textApprovedBy);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.textConcession);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.buttonCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditConcessionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Concession";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
