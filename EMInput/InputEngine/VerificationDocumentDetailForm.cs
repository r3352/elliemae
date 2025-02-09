// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationDocumentDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationDocumentDetailForm : Form
  {
    private VerificationTimelineType editMode = VerificationTimelineType.Employment;
    private VerificationDocument document;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private GroupContainer groupContainer1;
    private DatePicker dpExpirationDate;
    private DatePicker dpCurrentDate;
    private Label label7;
    private Label label5;
    private Label label2;
    private ComboBox cboDocName;

    public VerificationDocumentDetailForm(
      VerificationDocument document,
      VerificationTimelineType editMode)
    {
      this.editMode = editMode;
      this.document = document;
      this.InitializeComponent();
      switch (editMode)
      {
        case VerificationTimelineType.Employment:
          this.groupContainer1.Text = "Employment Status Document";
          break;
        case VerificationTimelineType.Income:
          this.groupContainer1.Text = "Income Document";
          break;
        case VerificationTimelineType.Asset:
          this.groupContainer1.Text = "Asset Document";
          break;
        case VerificationTimelineType.Obligation:
          this.groupContainer1.Text = "Monthly Obligation Document";
          break;
      }
      this.initTitleField();
      if (this.document != null)
      {
        this.cboDocName.Text = this.document.DocName;
        DatePicker dpCurrentDate = this.dpCurrentDate;
        DateTime dateTime;
        string str1;
        if (!(this.document.CurrentDate == DateTime.MinValue))
        {
          dateTime = this.document.CurrentDate;
          str1 = dateTime.ToString("MM/dd/yyyy");
        }
        else
          str1 = "";
        dpCurrentDate.Text = str1;
        DatePicker dpExpirationDate = this.dpExpirationDate;
        string str2;
        if (!(this.document.ExpirationDate == DateTime.MinValue))
        {
          dateTime = this.document.ExpirationDate;
          str2 = dateTime.ToString("MM/dd/yyyy");
        }
        else
          str2 = "";
        dpExpirationDate.Text = str2;
      }
      this.setButtonStatus();
    }

    private void initTitleField()
    {
      foreach (DocumentTemplate documentTemplate in Session.LoanDataMgr.SystemConfiguration.DocumentTrackingSetup)
        this.cboDocName.Items.Add((object) documentTemplate.Name);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.cboDocName.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The document name cannot be blank!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.dpExpirationDate.Text.Trim() != string.Empty && this.dpCurrentDate.Text.Trim() != string.Empty && Utils.ParseDate((object) this.dpExpirationDate.Text) < Utils.ParseDate((object) this.dpCurrentDate.Text))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The expiration date cannot be earlier than the current date!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.document.DocName = this.cboDocName.Text.Trim();
        this.document.CurrentDate = this.dpCurrentDate.Text == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) this.dpCurrentDate.Text);
        this.document.ExpirationDate = this.dpExpirationDate.Text == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) this.dpExpirationDate.Text);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void setButtonStatus()
    {
      this.btnOK.Enabled = this.cboDocName.Text.Trim() != string.Empty && this.dpCurrentDate.Text != string.Empty;
    }

    private void dpCurrentDate_ValueChanged(object sender, EventArgs e) => this.setButtonStatus();

    private void dpExpirationDate_ValueChanged(object sender, EventArgs e)
    {
      this.setButtonStatus();
    }

    public VerificationDocument CurrentVerificationDocument => this.document;

    private void cboDocName_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setButtonStatus();
    }

    private void cboDocName_TextChanged(object sender, EventArgs e) => this.setButtonStatus();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupContainer1 = new GroupContainer();
      this.dpExpirationDate = new DatePicker();
      this.dpCurrentDate = new DatePicker();
      this.label7 = new Label();
      this.label5 = new Label();
      this.label2 = new Label();
      this.cboDocName = new ComboBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(311, 115);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(389, 115);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.cboDocName);
      this.groupContainer1.Controls.Add((Control) this.dpExpirationDate);
      this.groupContainer1.Controls.Add((Control) this.dpCurrentDate);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(470, 109);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Employment Status Document";
      this.dpExpirationDate.BackColor = SystemColors.Window;
      this.dpExpirationDate.Location = new Point(101, 78);
      this.dpExpirationDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpExpirationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpExpirationDate.Name = "dpExpirationDate";
      this.dpExpirationDate.Size = new Size(85, 21);
      this.dpExpirationDate.TabIndex = 3;
      this.dpExpirationDate.Tag = (object) "3859";
      this.dpExpirationDate.ToolTip = "";
      this.dpExpirationDate.Value = new DateTime(0L);
      this.dpExpirationDate.ValueChanged += new EventHandler(this.dpExpirationDate_ValueChanged);
      this.dpCurrentDate.BackColor = SystemColors.Window;
      this.dpCurrentDate.Location = new Point(101, 55);
      this.dpCurrentDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpCurrentDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCurrentDate.Name = "dpCurrentDate";
      this.dpCurrentDate.Size = new Size(85, 21);
      this.dpCurrentDate.TabIndex = 2;
      this.dpCurrentDate.Tag = (object) "3859";
      this.dpCurrentDate.ToolTip = "";
      this.dpCurrentDate.Value = new DateTime(0L);
      this.dpCurrentDate.ValueChanged += new EventHandler(this.dpCurrentDate_ValueChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 81);
      this.label7.Name = "label7";
      this.label7.Size = new Size(79, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Expiration Date";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 59);
      this.label5.Name = "label5";
      this.label5.Size = new Size(67, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Current Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(87, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Document Name";
      this.cboDocName.FormattingEnabled = true;
      this.cboDocName.Location = new Point(101, 32);
      this.cboDocName.Name = "cboDocName";
      this.cboDocName.Size = new Size(363, 21);
      this.cboDocName.TabIndex = 0;
      this.cboDocName.SelectedIndexChanged += new EventHandler(this.cboDocName_SelectedIndexChanged);
      this.cboDocName.TextChanged += new EventHandler(this.cboDocName_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(470, 145);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (VerificationDocumentDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
