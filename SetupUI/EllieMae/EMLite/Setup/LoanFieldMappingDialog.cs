// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanFieldMappingDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanFieldMappingDialog : Form
  {
    private FieldDefinitionCollection loanFields;
    private CustomFieldsInfo loanCustomFields;
    private ContactType contactType;
    private CustomFieldMappingCollection customFieldMappingCollection;
    private BizCategory[] categories;
    private string loanFieldId = string.Empty;
    private CustomFieldDefinition categoryCustomField;
    private ContactCustomFieldInfo contactCustomField;
    private IContainer components;
    private Label lblLoanFieldId;
    private Label lblLoanFieldDescription;
    private Label lblLoanFieldType;
    private TextBox txtLoanFieldId;
    private TextBox txtLoanFieldDescription;
    private TextBox txtLoanFieldType;
    private Button btnLookUp;
    private Button btnCancel;
    private Button btnOk;
    private GroupBox grpLoanField;
    private GroupBox grpCustomField;
    private Label lblCustomFieldId;
    private Label lblCustomFieldDescription;
    private Label lblCustomFieldType;
    private TextBox txtCustomFieldId;
    private TextBox txtCustomFieldType;
    private TextBox txtCustomFieldDescription;
    private Label lblLoanFieldMapping;
    private TextBox txtLoanFieldMapping;

    public string LoanFieldId
    {
      get => this.loanFieldId;
      set
      {
        this.loanFieldId = value.Trim();
        this.txtLoanFieldId.Text = this.loanFieldId;
        this.displayLoanField();
      }
    }

    public CustomFieldDefinition CategoryCustomField
    {
      set
      {
        this.contactCustomField = (ContactCustomFieldInfo) null;
        this.categoryCustomField = value;
        this.txtCustomFieldId.Text = "Custom Field " + this.categoryCustomField.FieldNumber.ToString();
        this.txtCustomFieldDescription.Text = this.categoryCustomField.FieldDescription;
        this.txtCustomFieldType.Text = FieldFormatEnumUtil.ValueToName(this.categoryCustomField.FieldFormat);
      }
    }

    public ContactCustomFieldInfo ContactCustomField
    {
      set
      {
        this.categoryCustomField = (CustomFieldDefinition) null;
        this.contactCustomField = value;
        this.txtCustomFieldId.Text = "Custom Field " + this.contactCustomField.LabelID.ToString();
        this.txtCustomFieldDescription.Text = this.contactCustomField.Label;
        this.txtCustomFieldType.Text = FieldFormatEnumUtil.ValueToName(this.contactCustomField.FieldType);
      }
    }

    public LoanFieldMappingDialog(
      FieldDefinitionCollection loanFields,
      CustomFieldsInfo loanCustomFields,
      ContactType contactType,
      CustomFieldMappingCollection customFieldMappingCollection,
      BizCategory[] categories)
    {
      this.loanFields = loanFields;
      this.loanCustomFields = loanCustomFields;
      if (contactType == ContactType.Borrower)
        this.contactType = ContactType.Borrower;
      else if (ContactType.BizPartner == contactType)
        this.contactType = ContactType.BizPartner;
      else if (ContactType.TPO == contactType)
        this.contactType = ContactType.TPO;
      this.customFieldMappingCollection = customFieldMappingCollection;
      this.categories = categories;
      this.InitializeComponent();
    }

    public bool ValidateLoanFieldSelection(IWin32Window owner)
    {
      if (this.categoryCustomField == null && !((ContactCustomFieldInfo) null != this.contactCustomField))
        return false;
      if (string.Empty == this.loanFieldId)
        return true;
      if (!this.loanFieldDefined(this.loanFieldId))
      {
        int num = (int) Utils.Dialog(owner, "Specified Loan Field does not exist.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!this.verifyFieldFormats())
      {
        int num = (int) Utils.Dialog(owner, "Loan Field type and Custom Field type do not match.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      CustomFieldsType customFieldsType = CustomFieldsType.None;
      if (this.categoryCustomField != null)
        customFieldsType = CustomFieldsType.BizCategoryCustom;
      else if (this.contactType == ContactType.Borrower)
        customFieldsType = CustomFieldsType.Borrower;
      else if (ContactType.BizPartner == this.contactType)
        customFieldsType = CustomFieldsType.BizPartner;
      else if (ContactType.TPO == this.contactType)
        customFieldsType = CustomFieldsType.None;
      if (!this.customFieldMappingCollection.ContainsDuplicate(CustomFieldMapping.NewCustomFieldMapping(customFieldsType, this.categoryCustomField != null ? this.categoryCustomField.CategoryId : 0, this.categoryCustomField != null ? this.categoryCustomField.FieldNumber : this.contactCustomField.LabelID, this.categoryCustomField != null ? this.categoryCustomField.FieldId : 0, this.categoryCustomField != null ? this.categoryCustomField.FieldFormat : this.contactCustomField.FieldType, this.loanFieldId, false), this.loanFieldId))
        return true;
      int num1 = (int) Utils.Dialog(owner, "Loan Field is currently mapped to another Custom Field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private bool verifyFieldFormats()
    {
      FieldFormat loanFieldFormat = this.getLoanFieldFormat(this.loanFieldId);
      FieldFormat fieldFormat = this.categoryCustomField != null ? this.categoryCustomField.FieldFormat : this.contactCustomField.FieldType;
      return loanFieldFormat == fieldFormat || FieldFormat.STRING == loanFieldFormat && (FieldFormat.YN == fieldFormat || FieldFormat.X == fieldFormat || FieldFormat.ZIPCODE == fieldFormat || FieldFormat.STATE == fieldFormat || FieldFormat.PHONE == fieldFormat || FieldFormat.SSN == fieldFormat || FieldFormat.RA_STRING == fieldFormat || FieldFormat.RA_INTEGER == fieldFormat || FieldFormat.RA_DECIMAL_2 == fieldFormat || FieldFormat.RA_DECIMAL_3 == fieldFormat || FieldFormat.DROPDOWN == fieldFormat || FieldFormat.DROPDOWNLIST == fieldFormat) || (FieldFormat.RA_STRING == loanFieldFormat || FieldFormat.RA_INTEGER == loanFieldFormat || FieldFormat.RA_DECIMAL_2 == loanFieldFormat || FieldFormat.RA_DECIMAL_3 == loanFieldFormat) && fieldFormat == FieldFormat.STRING || FieldFormat.DECIMAL == loanFieldFormat && (FieldFormat.DECIMAL_1 == fieldFormat || FieldFormat.DECIMAL_2 == fieldFormat || FieldFormat.DECIMAL_3 == fieldFormat || FieldFormat.DECIMAL_4 == fieldFormat || FieldFormat.DECIMAL_5 == fieldFormat || FieldFormat.DECIMAL_6 == fieldFormat || FieldFormat.DECIMAL_7 == fieldFormat || FieldFormat.DECIMAL_10 == fieldFormat);
    }

    private bool loanFieldDefined(string loanFieldId)
    {
      return this.loanFields.Contains(loanFieldId) || this.loanCustomFields.GetField(loanFieldId) != null;
    }

    private FieldFormat getLoanFieldFormat(string loanFieldId)
    {
      FieldFormat loanFieldFormat = FieldFormat.NONE;
      if (this.loanFields.Contains(loanFieldId))
      {
        loanFieldFormat = this.loanFields[loanFieldId].Format;
      }
      else
      {
        CustomFieldInfo field = this.loanCustomFields.GetField(loanFieldId);
        if (field != null)
        {
          loanFieldFormat = field.Format;
          if (loanFieldFormat == FieldFormat.NONE && string.Empty != field.Description)
            loanFieldFormat = FieldFormat.STRING;
        }
      }
      return loanFieldFormat;
    }

    private string getLoanFieldDescription(string loanFieldId)
    {
      string fieldDescription = string.Empty;
      if (this.loanFields.Contains(loanFieldId))
      {
        fieldDescription = this.loanFields[loanFieldId].Description;
      }
      else
      {
        CustomFieldInfo field = this.loanCustomFields.GetField(loanFieldId);
        if (field != null)
          fieldDescription = field.Description;
      }
      return fieldDescription;
    }

    private bool displayLoanField()
    {
      string loanFieldId = this.txtLoanFieldId.Text.Trim();
      if (this.loanFieldDefined(loanFieldId))
      {
        this.txtLoanFieldDescription.Text = this.getLoanFieldDescription(loanFieldId);
        this.txtLoanFieldType.Text = FieldFormatEnumUtil.ValueToName(this.getLoanFieldFormat(loanFieldId));
        if (this.customFieldMappingCollection.Contains(loanFieldId))
          this.txtLoanFieldMapping.Text = this.formatCustomFieldName(this.customFieldMappingCollection.Find(loanFieldId));
        else
          this.txtLoanFieldMapping.Text = string.Empty;
        return true;
      }
      this.txtLoanFieldDescription.Text = string.Empty;
      this.txtLoanFieldType.Text = string.Empty;
      this.txtLoanFieldMapping.Text = string.Empty;
      return false;
    }

    private string formatCustomFieldName(CustomFieldMapping customFieldMapping)
    {
      string str = string.Empty;
      switch (customFieldMapping.CustomFieldsType)
      {
        case CustomFieldsType.None:
          str = "TPO Contact Custom Field " + customFieldMapping.FieldNumber.ToString();
          break;
        case CustomFieldsType.Borrower:
          str = "Borrower Contact Custom Field " + customFieldMapping.FieldNumber.ToString();
          break;
        case CustomFieldsType.BizPartner:
          str = "Business Contact Custom Field " + customFieldMapping.FieldNumber.ToString();
          break;
        case CustomFieldsType.BizCategoryCustom:
          str = this.getCategoryName(customFieldMapping.CategoryId) + " Category Custom Field " + customFieldMapping.FieldNumber.ToString();
          break;
      }
      return str;
    }

    private string getCategoryName(int categoryId)
    {
      foreach (BizCategory category in this.categories)
      {
        if (category.CategoryID == categoryId)
          return category.Name;
      }
      return string.Empty;
    }

    private void txtLoanFieldId_Leave(object sender, EventArgs e) => this.displayLoanField();

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.loanFieldId = this.txtLoanFieldId.Text;
      if (!this.ValidateLoanFieldSelection((IWin32Window) this))
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(Session.DefaultInstance, new string[1]
      {
        this.txtLoanFieldId.Text.Trim()
      }, true, string.Empty, true, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          if (ruleFindFieldDialog.SelectedRequiredFields.Length != 0)
            this.txtLoanFieldId.Text = ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1];
        }
      }
      this.displayLoanField();
    }

    private void txtLoanFieldId_TextChanged(object sender, EventArgs e) => this.displayLoanField();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblLoanFieldId = new Label();
      this.lblLoanFieldDescription = new Label();
      this.lblLoanFieldType = new Label();
      this.txtLoanFieldId = new TextBox();
      this.txtLoanFieldDescription = new TextBox();
      this.txtLoanFieldType = new TextBox();
      this.btnLookUp = new Button();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.grpLoanField = new GroupBox();
      this.lblLoanFieldMapping = new Label();
      this.txtLoanFieldMapping = new TextBox();
      this.grpCustomField = new GroupBox();
      this.lblCustomFieldId = new Label();
      this.lblCustomFieldDescription = new Label();
      this.lblCustomFieldType = new Label();
      this.txtCustomFieldId = new TextBox();
      this.txtCustomFieldType = new TextBox();
      this.txtCustomFieldDescription = new TextBox();
      this.grpLoanField.SuspendLayout();
      this.grpCustomField.SuspendLayout();
      this.SuspendLayout();
      this.lblLoanFieldId.AutoSize = true;
      this.lblLoanFieldId.Location = new Point(6, 24);
      this.lblLoanFieldId.Name = "lblLoanFieldId";
      this.lblLoanFieldId.Size = new Size(73, 13);
      this.lblLoanFieldId.TabIndex = 0;
      this.lblLoanFieldId.Text = "Loan Field ID:";
      this.lblLoanFieldDescription.AutoSize = true;
      this.lblLoanFieldDescription.Location = new Point(6, 50);
      this.lblLoanFieldDescription.Name = "lblLoanFieldDescription";
      this.lblLoanFieldDescription.Size = new Size(115, 13);
      this.lblLoanFieldDescription.TabIndex = 2;
      this.lblLoanFieldDescription.Text = "Loan Field Description:";
      this.lblLoanFieldType.AutoSize = true;
      this.lblLoanFieldType.Location = new Point(6, 76);
      this.lblLoanFieldType.Name = "lblLoanFieldType";
      this.lblLoanFieldType.Size = new Size(86, 13);
      this.lblLoanFieldType.TabIndex = 4;
      this.lblLoanFieldType.Text = "Loan Field Type:";
      this.txtLoanFieldId.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId.Location = new Point(138, 20);
      this.txtLoanFieldId.Name = "txtLoanFieldId";
      this.txtLoanFieldId.Size = new Size(215, 20);
      this.txtLoanFieldId.TabIndex = 1;
      this.txtLoanFieldId.TextChanged += new EventHandler(this.txtLoanFieldId_TextChanged);
      this.txtLoanFieldDescription.Location = new Point(138, 46);
      this.txtLoanFieldDescription.Name = "txtLoanFieldDescription";
      this.txtLoanFieldDescription.ReadOnly = true;
      this.txtLoanFieldDescription.Size = new Size(215, 20);
      this.txtLoanFieldDescription.TabIndex = 3;
      this.txtLoanFieldDescription.TabStop = false;
      this.txtLoanFieldType.Location = new Point(138, 72);
      this.txtLoanFieldType.Name = "txtLoanFieldType";
      this.txtLoanFieldType.ReadOnly = true;
      this.txtLoanFieldType.Size = new Size(215, 20);
      this.txtLoanFieldType.TabIndex = 5;
      this.txtLoanFieldType.TabStop = false;
      this.btnLookUp.Location = new Point(278, 124);
      this.btnLookUp.Name = "btnLookUp";
      this.btnLookUp.Size = new Size(75, 23);
      this.btnLookUp.TabIndex = 8;
      this.btnLookUp.TabStop = false;
      this.btnLookUp.Text = "&Look Up";
      this.btnLookUp.UseVisualStyleBackColor = true;
      this.btnLookUp.Click += new EventHandler(this.btnFind_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(290, 275);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(209, 275);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "&Select";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.grpLoanField.Controls.Add((Control) this.lblLoanFieldMapping);
      this.grpLoanField.Controls.Add((Control) this.txtLoanFieldMapping);
      this.grpLoanField.Controls.Add((Control) this.lblLoanFieldId);
      this.grpLoanField.Controls.Add((Control) this.lblLoanFieldDescription);
      this.grpLoanField.Controls.Add((Control) this.lblLoanFieldType);
      this.grpLoanField.Controls.Add((Control) this.btnLookUp);
      this.grpLoanField.Controls.Add((Control) this.txtLoanFieldId);
      this.grpLoanField.Controls.Add((Control) this.txtLoanFieldType);
      this.grpLoanField.Controls.Add((Control) this.txtLoanFieldDescription);
      this.grpLoanField.Location = new Point(12, 116);
      this.grpLoanField.Name = "grpLoanField";
      this.grpLoanField.Size = new Size(359, 153);
      this.grpLoanField.TabIndex = 1;
      this.grpLoanField.TabStop = false;
      this.lblLoanFieldMapping.AutoSize = true;
      this.lblLoanFieldMapping.Location = new Point(6, 102);
      this.lblLoanFieldMapping.Name = "lblLoanFieldMapping";
      this.lblLoanFieldMapping.Size = new Size(124, 13);
      this.lblLoanFieldMapping.TabIndex = 6;
      this.lblLoanFieldMapping.Text = "Mapped to Custom Field:";
      this.txtLoanFieldMapping.Location = new Point(138, 98);
      this.txtLoanFieldMapping.Name = "txtLoanFieldMapping";
      this.txtLoanFieldMapping.ReadOnly = true;
      this.txtLoanFieldMapping.Size = new Size(215, 20);
      this.txtLoanFieldMapping.TabIndex = 7;
      this.txtLoanFieldMapping.TabStop = false;
      this.grpCustomField.Controls.Add((Control) this.lblCustomFieldId);
      this.grpCustomField.Controls.Add((Control) this.lblCustomFieldDescription);
      this.grpCustomField.Controls.Add((Control) this.lblCustomFieldType);
      this.grpCustomField.Controls.Add((Control) this.txtCustomFieldId);
      this.grpCustomField.Controls.Add((Control) this.txtCustomFieldType);
      this.grpCustomField.Controls.Add((Control) this.txtCustomFieldDescription);
      this.grpCustomField.Location = new Point(12, 12);
      this.grpCustomField.Name = "grpCustomField";
      this.grpCustomField.Size = new Size(359, 98);
      this.grpCustomField.TabIndex = 0;
      this.grpCustomField.TabStop = false;
      this.lblCustomFieldId.AutoSize = true;
      this.lblCustomFieldId.Location = new Point(6, 24);
      this.lblCustomFieldId.Name = "lblCustomFieldId";
      this.lblCustomFieldId.Size = new Size(84, 13);
      this.lblCustomFieldId.TabIndex = 0;
      this.lblCustomFieldId.Text = "Custom Field ID:";
      this.lblCustomFieldDescription.AutoSize = true;
      this.lblCustomFieldDescription.Location = new Point(6, 50);
      this.lblCustomFieldDescription.Name = "lblCustomFieldDescription";
      this.lblCustomFieldDescription.Size = new Size(126, 13);
      this.lblCustomFieldDescription.TabIndex = 2;
      this.lblCustomFieldDescription.Text = "Custom Field Description:";
      this.lblCustomFieldType.AutoSize = true;
      this.lblCustomFieldType.Location = new Point(6, 76);
      this.lblCustomFieldType.Name = "lblCustomFieldType";
      this.lblCustomFieldType.Size = new Size(97, 13);
      this.lblCustomFieldType.TabIndex = 4;
      this.lblCustomFieldType.Text = "Custom Field Type:";
      this.txtCustomFieldId.Location = new Point(138, 20);
      this.txtCustomFieldId.Name = "txtCustomFieldId";
      this.txtCustomFieldId.ReadOnly = true;
      this.txtCustomFieldId.Size = new Size(215, 20);
      this.txtCustomFieldId.TabIndex = 1;
      this.txtCustomFieldId.TabStop = false;
      this.txtCustomFieldType.Location = new Point(138, 73);
      this.txtCustomFieldType.Name = "txtCustomFieldType";
      this.txtCustomFieldType.ReadOnly = true;
      this.txtCustomFieldType.Size = new Size(215, 20);
      this.txtCustomFieldType.TabIndex = 5;
      this.txtCustomFieldType.TabStop = false;
      this.txtCustomFieldDescription.Location = new Point(138, 47);
      this.txtCustomFieldDescription.Name = "txtCustomFieldDescription";
      this.txtCustomFieldDescription.ReadOnly = true;
      this.txtCustomFieldDescription.Size = new Size(215, 20);
      this.txtCustomFieldDescription.TabIndex = 3;
      this.txtCustomFieldDescription.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(379, 310);
      this.Controls.Add((Control) this.grpCustomField);
      this.Controls.Add((Control) this.grpLoanField);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanFieldMappingDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Loan Field for Custom Field Mapping";
      this.grpLoanField.ResumeLayout(false);
      this.grpLoanField.PerformLayout();
      this.grpCustomField.ResumeLayout(false);
      this.grpCustomField.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
