// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlFieldDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlFieldDialog : Form
  {
    private FieldSettings fieldSettings;
    private string fieldID;
    private string fieldName;
    private bool allowSignatures;
    private bool isCCLoanSource;
    private Sessions.Session session;
    private IContainer components;
    private RadioButton rdoSignature;
    private RadioButton rdoCommon;
    private ComboBox cboField;
    private RadioButton rdoOther;
    private TextBox txtField;
    private Button btnInsert;
    private Button btnCancel;
    private Label lblInsert;
    private StandardIconButton btnSearch;

    public HtmlFieldDialog(bool allowSignatures)
      : this(allowSignatures, Session.DefaultInstance)
    {
    }

    public HtmlFieldDialog(bool allowSignatures, Sessions.Session session, bool isConsumerConnect = false)
    {
      this.InitializeComponent();
      this.session = session;
      this.allowSignatures = allowSignatures;
      this.applySecurity();
      this.isCCLoanSource = isConsumerConnect;
      this.initFieldList();
    }

    public string FieldID => this.fieldID;

    public string FieldName => this.fieldName;

    private void applySecurity()
    {
      this.rdoSignature.Visible = this.allowSignatures;
      this.rdoSignature.Checked = this.allowSignatures;
      this.rdoCommon.Checked = !this.allowSignatures;
    }

    private void initFieldList()
    {
      for (int index = 0; index <= 42; ++index)
      {
        FieldOption fieldOption = (FieldOption) null;
        switch (index)
        {
          case 0:
            fieldOption = new FieldOption("Borrower First Name", "4000");
            break;
          case 1:
            fieldOption = new FieldOption("Borrower Last Name", "4002");
            break;
          case 2:
            fieldOption = new FieldOption("Borrower First And Middle Name", "36");
            break;
          case 3:
            fieldOption = new FieldOption("Borrower Last Name And Suffix", "37");
            break;
          case 4:
            fieldOption = new FieldOption("Recipient Full Name", "Recipient Full Name");
            break;
          case 5:
            fieldOption = new FieldOption("Co-Borrower First Name", "4004");
            break;
          case 6:
            fieldOption = new FieldOption("Co-Borrower Last Name", "4006");
            break;
          case 7:
            fieldOption = new FieldOption("Co-Borrower First And Middle Name", "68");
            break;
          case 8:
            fieldOption = new FieldOption("Co-Borrower Last Name and Suffix", "69");
            break;
          case 9:
            fieldOption = new FieldOption("Borrower Home Phone", "66");
            break;
          case 10:
            fieldOption = new FieldOption("Borrower Work Phone", "FE0117");
            break;
          case 11:
            fieldOption = new FieldOption("Co-Borrower Home Phone", "98");
            break;
          case 12:
            fieldOption = new FieldOption("Co-Borrower Work Phone", "FE0217");
            break;
          case 13:
            fieldOption = new FieldOption("Loan Number", "364");
            break;
          case 14:
            fieldOption = new FieldOption("Agency Case Number", "1040");
            break;
          case 15:
            fieldOption = new FieldOption("Subject Property Address", "11");
            break;
          case 16:
            fieldOption = new FieldOption("Subject Property City", "12");
            break;
          case 17:
            fieldOption = new FieldOption("Subject Property State", "14");
            break;
          case 18:
            fieldOption = new FieldOption("Subject Property Zip", "15");
            break;
          case 19:
            fieldOption = new FieldOption("Subject Property County", "13");
            break;
          case 20:
            fieldOption = new FieldOption("Current User Name", "CurrentUserName");
            break;
          case 21:
            fieldOption = new FieldOption("Current User Phone", "CurrentUserPhone");
            break;
          case 22:
            fieldOption = new FieldOption("Current User Email", "CurrentUserEmail");
            break;
          case 23:
            fieldOption = new FieldOption("Loan Officer Name", "317");
            break;
          case 24:
            fieldOption = new FieldOption("Loan Officer Phone", "1406");
            break;
          case 25:
            fieldOption = new FieldOption("Loan Officer Email", "1407");
            break;
          case 26:
            fieldOption = new FieldOption("Loan Processor Name", "362");
            break;
          case 27:
            fieldOption = new FieldOption("Loan Processor Phone", "1408");
            break;
          case 28:
            fieldOption = new FieldOption("Loan Processor Email", "1409");
            break;
          case 29:
            fieldOption = new FieldOption("Organization Name", "315");
            break;
          case 30:
            fieldOption = new FieldOption("Organization Address", "319");
            break;
          case 31:
            fieldOption = new FieldOption("Organization City", "313");
            break;
          case 32:
            fieldOption = new FieldOption("Organization State", "321");
            break;
          case 33:
            fieldOption = new FieldOption("Organization Zip", "323");
            break;
          case 34:
            fieldOption = new FieldOption("Lender Name", "1264");
            break;
          case 35:
            fieldOption = new FieldOption("Lender Address", "1257");
            break;
          case 36:
            fieldOption = new FieldOption("Lender City", "1258");
            break;
          case 37:
            fieldOption = new FieldOption("Lender State", "1259");
            break;
          case 38:
            fieldOption = new FieldOption("Lender Zip", "1260");
            break;
          case 39:
            fieldOption = new FieldOption("Lender Phone", "1262");
            break;
          case 40:
            fieldOption = new FieldOption("Informational Documents", "Informational Documents");
            break;
          case 41:
            fieldOption = new FieldOption("Sign and Return Documents", "Sign and Return Documents");
            break;
          case 42:
            fieldOption = new FieldOption("Needed Documents", "Needed Documents");
            break;
        }
        if (index != 4 || this.isCCLoanSource)
          this.cboField.Items.Add((object) fieldOption);
      }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      string[] existingFields = (string[]) null;
      if (this.txtField.Text != string.Empty)
        existingFields = new string[1]{ this.txtField.Text };
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, existingFields, true, string.Empty, true, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.txtField.Text = ruleFindFieldDialog.SelectedRequiredFields[0];
      }
    }

    private void btnInsert_Click(object sender, EventArgs e)
    {
      string fieldId;
      if (this.rdoSignature.Checked)
        fieldId = "Signature";
      else if (this.rdoCommon.Checked)
      {
        FieldOption selectedItem = (FieldOption) this.cboField.SelectedItem;
        if (selectedItem == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select a common field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        fieldId = selectedItem.Value;
      }
      else if (this.rdoOther.Checked)
      {
        fieldId = this.txtField.Text.Trim();
        if (fieldId == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a field identifier.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (this.fieldSettings == null)
          this.fieldSettings = this.session.LoanManager.GetFieldSettings();
        if (EncompassFields.GetField(fieldId, this.fieldSettings) == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a valid field identifier.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      this.fieldName = string.Empty;
      LoanReportFieldDef fieldById = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.AllFields).GetFieldByID(fieldId);
      if (fieldById != null)
        this.fieldName = fieldById.Name;
      this.fieldID = fieldId;
      this.DialogResult = DialogResult.OK;
    }

    private void rdoButtons_CheckedChanged(object sender, EventArgs e)
    {
      this.txtField.Enabled = this.rdoOther.Checked;
      this.btnSearch.Enabled = this.rdoOther.Checked;
      this.cboField.Enabled = this.rdoCommon.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoSignature = new RadioButton();
      this.rdoCommon = new RadioButton();
      this.cboField = new ComboBox();
      this.rdoOther = new RadioButton();
      this.txtField = new TextBox();
      this.btnInsert = new Button();
      this.btnCancel = new Button();
      this.lblInsert = new Label();
      this.btnSearch = new StandardIconButton();
      ((ISupportInitialize) this.btnSearch).BeginInit();
      this.SuspendLayout();
      this.rdoSignature.AutoSize = true;
      this.rdoSignature.Location = new Point(68, 12);
      this.rdoSignature.Name = "rdoSignature";
      this.rdoSignature.Size = new Size(71, 18);
      this.rdoSignature.TabIndex = 1;
      this.rdoSignature.TabStop = true;
      this.rdoSignature.Text = "Signature";
      this.rdoSignature.UseVisualStyleBackColor = true;
      this.rdoSignature.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.rdoCommon.AutoSize = true;
      this.rdoCommon.Location = new Point(68, 40);
      this.rdoCommon.Name = "rdoCommon";
      this.rdoCommon.Size = new Size((int) sbyte.MaxValue, 18);
      this.rdoCommon.TabIndex = 2;
      this.rdoCommon.Text = "Commonly Used Field";
      this.rdoCommon.UseVisualStyleBackColor = true;
      this.rdoCommon.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.cboField.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboField.FormattingEnabled = true;
      this.cboField.Location = new Point(204, 38);
      this.cboField.Name = "cboField";
      this.cboField.Size = new Size(284, 22);
      this.cboField.TabIndex = 3;
      this.rdoOther.AutoSize = true;
      this.rdoOther.Location = new Point(68, 68);
      this.rdoOther.Name = "rdoOther";
      this.rdoOther.Size = new Size(77, 18);
      this.rdoOther.TabIndex = 4;
      this.rdoOther.Text = "Other Field";
      this.rdoOther.UseVisualStyleBackColor = true;
      this.rdoOther.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.txtField.Location = new Point(204, 68);
      this.txtField.Name = "txtField";
      this.txtField.Size = new Size(128, 20);
      this.txtField.TabIndex = 5;
      this.btnInsert.Location = new Point(336, 108);
      this.btnInsert.Name = "btnInsert";
      this.btnInsert.Size = new Size(75, 22);
      this.btnInsert.TabIndex = 6;
      this.btnInsert.Text = "Insert";
      this.btnInsert.UseVisualStyleBackColor = true;
      this.btnInsert.Click += new EventHandler(this.btnInsert_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(414, 108);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblInsert.AutoSize = true;
      this.lblInsert.Location = new Point(12, 13);
      this.lblInsert.Name = "lblInsert";
      this.lblInsert.Size = new Size(34, 14);
      this.lblInsert.TabIndex = 0;
      this.lblInsert.Text = "Insert";
      this.btnSearch.BackColor = Color.Transparent;
      this.btnSearch.Location = new Point(336, 70);
      this.btnSearch.MouseDownImage = (Image) null;
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(16, 16);
      this.btnSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSearch.TabIndex = 8;
      this.btnSearch.TabStop = false;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(500, 140);
      this.Controls.Add((Control) this.btnSearch);
      this.Controls.Add((Control) this.lblInsert);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnInsert);
      this.Controls.Add((Control) this.txtField);
      this.Controls.Add((Control) this.rdoOther);
      this.Controls.Add((Control) this.cboField);
      this.Controls.Add((Control) this.rdoCommon);
      this.Controls.Add((Control) this.rdoSignature);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HtmlFieldDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Insert Field";
      ((ISupportInitialize) this.btnSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
