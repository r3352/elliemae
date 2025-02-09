// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.CategoryCustomFieldsControl
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class CategoryCustomFieldsControl : UserControl, IBindingForm
  {
    private const bool ENABLED = true;
    private const bool VISIBLE = true;
    private const bool CLEARTEXT = true;
    private int contactId = -1;
    private BizPartnerInfo contactInfo;
    private CustomFieldsType customFieldsType;
    private CustomFieldsDefinition customFieldsDefinition;
    private CustomFieldValueCollection customFieldValues;
    private bool isReadOnly;
    private bool isUIDirty;
    private bool populatingControls;
    private bool deleteBackKey;
    private CategoryCustomFieldsControl.LabelList lblFieldDescriptions = new CategoryCustomFieldsControl.LabelList();
    private Panel pnlLeftLabel;
    private Panel pnlRightData;
    private Panel pnlRight;
    private Panel pnlRightLabel;
    private Panel pnlLeft;
    private Panel pnlLeftData;
    private CategoryCustomFieldsControl.ComboBoxList cboFieldValues = new CategoryCustomFieldsControl.ComboBoxList();
    private Panel panel2;
    private Panel panel1;
    private int dataFieldSize;
    private Sessions.Session session;
    private IContainer components;
    private ToolTip tipCustomField;

    public event EventHandler DataChangedEvent;

    private bool isUiDirty
    {
      get => this.isUIDirty;
      set
      {
        this.isUIDirty = value;
        if (!value || this.DataChangedEvent == null)
          return;
        this.DataChangedEvent((object) null, (EventArgs) null);
      }
    }

    public string GetCustomFieldValue(string fieldDescription)
    {
      if (this.customFieldsDefinition != null && this.customFieldValues != null)
      {
        CustomFieldDefinition customFieldDefinition = this.customFieldsDefinition.CustomFieldDefinitions.Find(fieldDescription);
        if (customFieldDefinition != null)
        {
          CustomFieldValue customFieldValue = this.customFieldValues.Find(customFieldDefinition.FieldId);
          if (customFieldValue != null)
            return customFieldValue.FieldValue;
        }
      }
      return string.Empty;
    }

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set
      {
        if (value == this.isReadOnly)
          return;
        this.isReadOnly = value;
        this.setControlState(!this.isReadOnly, true, false);
      }
    }

    public bool isDirty() => this.isUiDirty;

    public bool setDirty
    {
      set => this.isUiDirty = value;
    }

    public int CurrentContactID
    {
      get => this.contactId;
      set
      {
        if (this.contactId == value)
          return;
        value = 0 > value ? -1 : value;
        this.contactId = value;
        this.contactInfo = (BizPartnerInfo) null;
        this.customFieldsDefinition = (CustomFieldsDefinition) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.setControlState(!this.isReadOnly, false, true);
        this.isUiDirty = false;
        if (0 > this.contactId)
          return;
        this.getContactInformation(this.contactId);
        this.populateCustomFields();
      }
    }

    public object CurrentContact
    {
      get => (object) this.contactInfo;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.contactId = value == null ? -1 : ((BizPartnerInfo) value).ContactID;
        this.contactInfo = (BizPartnerInfo) null;
        this.customFieldsDefinition = (CustomFieldsDefinition) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.setControlState(!this.isReadOnly, false, true);
        this.isUiDirty = false;
        if (0 > this.contactId)
          return;
        this.contactInfo = (BizPartnerInfo) value;
        this.getContactInformation(this.contactId);
        this.populateCustomFields();
      }
    }

    public void ResetCategoryID(int categoryID)
    {
      this.resetCustomFieldInfo(this.contactId, categoryID);
      this.populateCustomFields();
    }

    public CategoryCustomFieldsControl(CustomFieldsType customFieldsType)
      : this(customFieldsType, Session.DefaultInstance)
    {
    }

    public CategoryCustomFieldsControl(CustomFieldsType customFieldsType, Sessions.Session session)
    {
      this.session = session;
      this.customFieldsType = customFieldsType;
      this.InitializeComponent();
    }

    public void UpdateMortgageClauseFields(RxContactInfo rxContact)
    {
      this.getContactInformation(rxContact.ContactID);
      for (int index = 0; index < this.customFieldsDefinition.CustomFieldDefinitions.Count; ++index)
      {
        if (this.customFieldsDefinition.CustomFieldDefinitions[index].FieldNumber <= 0)
        {
          string empty = string.Empty;
          string str;
          switch (this.customFieldsDefinition.CustomFieldDefinitions[index].FieldDescription)
          {
            case "Address":
              str = rxContact.MortgageeClauseAddressLine;
              break;
            case "City":
              str = rxContact.MortgageeClauseCity;
              break;
            case "Company Name":
              str = rxContact.MortgageeClauseCompany;
              break;
            case "Contact Fax":
              str = rxContact.MortgageeClauseFax;
              break;
            case "Contact Name":
              str = rxContact.MortgageeClauseName;
              break;
            case "Contact Phone":
              str = rxContact.MortgageeClausePhone;
              break;
            case "Investor Code":
              str = rxContact.MortgageeClauseInvestorCode;
              break;
            case "Location Code":
              str = rxContact.MortgageeClauseLocationCode;
              break;
            case "Mortgagee Clause":
              str = rxContact.MortgageeClauseText;
              break;
            case "State":
              str = rxContact.MortgageeClauseState;
              break;
            case "Zip":
              str = rxContact.MortgageeClauseZipCode;
              break;
            default:
              continue;
          }
          int fieldId = this.customFieldsDefinition.CustomFieldDefinitions[index].FieldId;
          if (this.customFieldValues.Contains(fieldId))
          {
            CustomFieldValue customFieldValue = this.customFieldValues.Find(fieldId);
            if (string.Empty != str)
              customFieldValue.FieldValue = str;
            else
              customFieldValue.Delete();
          }
          else if (string.Empty != str)
          {
            FieldFormat fieldFormat = this.customFieldsDefinition.CustomFieldDefinitions[index].FieldFormat;
            CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(fieldId, rxContact.ContactID, fieldFormat);
            customFieldValue.FieldValue = str;
            this.customFieldValues.Add(customFieldValue);
          }
        }
      }
      if (!this.customFieldValues.IsDirty)
        return;
      try
      {
        this.customFieldValues = this.customFieldValues.Save();
      }
      catch (Exception ex)
      {
        throw new ObjectNotFoundException("Unable to update custom category fields for business contact '" + this.contactInfo.LastName + ", " + this.contactInfo.FirstName + "'.", ObjectType.Contact, (object) this.contactInfo.ContactID);
      }
      this.populateCustomFields();
    }

    public bool SaveChanges()
    {
      bool flag = false;
      if (this.isUiDirty && -1 != this.contactId)
      {
        for (int index = 0; index < this.customFieldsDefinition.CustomFieldDefinitions.Count; ++index)
        {
          string str = this.cboFieldValues[index].Text.Trim();
          int fieldId = ((CustomFieldDefinition) this.cboFieldValues[index].Tag).FieldId;
          if (this.customFieldValues.Contains(fieldId))
            this.customFieldValues.Find(fieldId).FieldValue = str;
          else if (string.Empty != str)
          {
            FieldFormat fieldFormat = ((CustomFieldDefinition) this.cboFieldValues[index].Tag).FieldFormat;
            CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(fieldId, this.contactId, fieldFormat);
            customFieldValue.FieldValue = str;
            this.customFieldValues.Add(customFieldValue);
          }
        }
        if (this.customFieldValues.IsDirty)
        {
          try
          {
            this.customFieldValues = this.customFieldValues.Save();
          }
          catch (Exception ex)
          {
            throw new ObjectNotFoundException("Unable to update custom category fields for business contact '" + this.contactInfo.LastName + ", " + this.contactInfo.FirstName + "'.", ObjectType.Contact, (object) this.contactInfo.ContactID);
          }
          flag = true;
          this.populateCustomFields();
        }
      }
      this.isUiDirty = false;
      return flag;
    }

    private void setControlState(bool enabled, bool visible, bool clearText)
    {
      this.SuspendLayout();
      for (int index = 0; index < this.cboFieldValues.Count; ++index)
      {
        this.cboFieldValues[index].Enabled = enabled;
        this.lblFieldDescriptions[index].Visible = visible;
        this.cboFieldValues[index].Visible = visible;
        if (clearText)
          this.cboFieldValues[index].Text = string.Empty;
      }
      this.ResumeLayout();
    }

    private void getContactInformation(int contactId)
    {
      if (this.contactInfo == null || this.contactInfo.ContactID != contactId)
        this.contactInfo = this.session.ContactManager.GetBizPartner(contactId);
      this.resetCustomFieldInfo(this.contactInfo.ContactID, this.contactInfo.CategoryID);
    }

    private void resetCustomFieldInfo(int contactId, int categoryId)
    {
      this.customFieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(this.session.SessionObjects, this.customFieldsType, categoryId);
      if (this.customFieldsDefinition == null)
        return;
      if (contactId > -1)
        this.customFieldValues = CustomFieldValueCollection.GetCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(this.contactInfo.ContactID, categoryId));
      else
        this.customFieldValues = (CustomFieldValueCollection) null;
    }

    private void populateCustomFields()
    {
      this.populatingControls = true;
      this.SuspendLayout();
      this.createControlArrays(this.customFieldsDefinition.CustomFieldDefinitions.Count);
      int index = 0;
      foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) this.customFieldsDefinition.CustomFieldDefinitions)
      {
        this.lblFieldDescriptions[index].Tag = (object) customFieldDefinition.FieldDescription;
        this.fitLabelText(this.lblFieldDescriptions[index], customFieldDefinition.FieldDescription);
        this.lblFieldDescriptions[index].Visible = true;
        this.cboFieldValues[index].Tag = (object) customFieldDefinition;
        if (FieldFormat.DROPDOWN == customFieldDefinition.FieldFormat)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDown;
          this.cboFieldValues[index].MaxLength = 50;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.AddRange((object[]) customFieldDefinition.CustomFieldOptions.GetOptionValues());
          this.cboFieldValues[index].SelectedIndex = -1;
        }
        else if (FieldFormat.DROPDOWNLIST == customFieldDefinition.FieldFormat)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDownList;
          this.cboFieldValues[index].MaxLength = 50;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.Add((object) string.Empty);
          this.cboFieldValues[index].Items.AddRange((object[]) customFieldDefinition.CustomFieldOptions.GetOptionValues());
          this.cboFieldValues[index].SelectedIndex = -1;
        }
        else
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.Simple;
          this.cboFieldValues[index].MaxLength = 4096;
          this.cboFieldValues[index].Text = string.Empty;
        }
        if (this.customFieldValues != null && this.customFieldValues.Contains(customFieldDefinition.FieldId))
          this.cboFieldValues[index].Text = this.customFieldValues.Find(customFieldDefinition.FieldId).FieldValue;
        this.dataFieldSize = this.cboFieldValues[index].Width;
        this.tipCustomField.SetToolTip((Control) this.cboFieldValues[index], FieldFormatEnumUtil.ValueToName(customFieldDefinition.FieldFormat));
        this.cboFieldValues[index].Visible = true;
        if (customFieldDefinition.FieldDescription == "Investor Code")
          this.cboFieldValues[index].MaxLength = 64;
        ++index;
      }
      this.adjustWidth();
      this.ResumeLayout();
      this.populatingControls = false;
      this.isUiDirty = false;
    }

    private void createControlArrays(int fieldCount)
    {
      if (this.lblFieldDescriptions.Count == fieldCount)
        return;
      int num1 = 2;
      int num2 = 4;
      int num3 = 24;
      int num4 = 22;
      if (this.lblFieldDescriptions.Count > fieldCount)
      {
        for (int index = this.lblFieldDescriptions.Count - 1; index >= fieldCount; --index)
        {
          if (this.pnlLeftLabel.Contains((Control) this.lblFieldDescriptions[index]))
            this.pnlLeftLabel.Controls.Remove((Control) this.lblFieldDescriptions[index]);
          else if (this.pnlRightLabel.Contains((Control) this.lblFieldDescriptions[index]))
            this.pnlRightLabel.Controls.Remove((Control) this.lblFieldDescriptions[index]);
          if (this.pnlLeftData.Contains((Control) this.cboFieldValues[index]))
            this.pnlLeftData.Controls.Remove((Control) this.cboFieldValues[index]);
          else if (this.pnlRightData.Contains((Control) this.cboFieldValues[index]))
            this.pnlRightData.Controls.Remove((Control) this.cboFieldValues[index]);
          this.lblFieldDescriptions.RemoveAt(index);
          this.cboFieldValues.RemoveAt(index);
        }
        this.Height = num2 + this.lblFieldDescriptions.Count / num1 * num3 + num3;
      }
      else
      {
        for (int count = this.lblFieldDescriptions.Count; count < fieldCount; ++count)
        {
          this.lblFieldDescriptions.Add(new Label());
          this.lblFieldDescriptions[count].TextAlign = ContentAlignment.MiddleLeft;
          if (count % 2 == 0)
            this.pnlLeftLabel.Controls.Add((Control) this.lblFieldDescriptions[count]);
          else
            this.pnlRightLabel.Controls.Add((Control) this.lblFieldDescriptions[count]);
          this.cboFieldValues.Add(new ComboBox());
          this.cboFieldValues[count].DropDownStyle = ComboBoxStyle.Simple;
          this.cboFieldValues[count].Enabled = !this.isReadOnly;
          this.cboFieldValues[count].TabIndex = count;
          this.cboFieldValues[count].Tag = (object) (count + 1).ToString();
          this.cboFieldValues[count].KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
          this.cboFieldValues[count].TextChanged += new EventHandler(this.cboFieldValue_Changed);
          this.cboFieldValues[count].SelectedValueChanged += new EventHandler(this.cboFieldValue_Changed);
          if (count % 2 == 0)
            this.pnlLeftData.Controls.Add((Control) this.cboFieldValues[count]);
          else
            this.pnlRightData.Controls.Add((Control) this.cboFieldValues[count]);
          int num5 = num2 + count / num1 * num3;
          this.lblFieldDescriptions[count].Top = num5;
          this.lblFieldDescriptions[count].Height = num4;
          this.cboFieldValues[count].Top = num5;
          this.cboFieldValues[count].Height = num4;
        }
        this.Height = num2 + this.lblFieldDescriptions.Count / num1 * num3 + num3;
        this.adjustWidth();
      }
    }

    private void adjustWidth()
    {
      int num1 = 0;
      int num2 = 0;
      this.SuspendLayout();
      for (int index = 0; index < this.lblFieldDescriptions.Count; ++index)
      {
        if (this.lblFieldDescriptions[index].Tag != null)
        {
          this.fitLabelText(this.lblFieldDescriptions[index], (string) this.lblFieldDescriptions[index].Tag);
          if (index % 2 == 0)
          {
            if (this.lblFieldDescriptions[index].Width > num1)
              num1 = this.lblFieldDescriptions[index].Width;
          }
          else if (this.lblFieldDescriptions[index].Width > num2)
            num2 = this.lblFieldDescriptions[index].Width;
        }
      }
      this.pnlLeftLabel.Width = num1;
      this.pnlRightLabel.Width = num2;
      this.pnlLeft.Size = new Size(num1 + this.dataFieldSize + 10, this.pnlLeft.Height);
      this.ResumeLayout();
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        float width = graphics.MeasureString(text, label.Font).Width;
        if ((double) width < 400.0)
          label.Size = new Size(int.Parse(string.Concat((object) Math.Ceiling((double) width))) + 2, label.Height);
        if (Utils.FitLabelText(graphics, label, text))
          this.tipCustomField.SetToolTip((Control) label, string.Empty);
        else
          this.tipCustomField.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    private void formatFieldValue(ComboBox cboFieldValue, FieldFormat fieldFormat)
    {
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
        if (FieldFormat.SSN == fieldFormat)
          return;
      }
      bool needsUpdate = false;
      int newCursorPos = 0;
      string str = Utils.FormatInput(cboFieldValue.Text, fieldFormat, ref needsUpdate, cboFieldValue.SelectionStart, ref newCursorPos);
      if (!needsUpdate)
        return;
      cboFieldValue.Text = str;
      cboFieldValue.SelectionStart = newCursorPos;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cboFieldValue_Changed(object sender, EventArgs e)
    {
      if (this.populatingControls)
        return;
      ComboBox cboFieldValue = (ComboBox) sender;
      FieldFormat fieldFormat = ((CustomFieldDefinition) cboFieldValue.Tag).FieldFormat;
      this.formatFieldValue(cboFieldValue, fieldFormat);
      this.isUiDirty = true;
    }

    private void cboFieldValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.populatingControls || Keys.Back != e.KeyCode && Keys.Delete != e.KeyCode)
        return;
      this.deleteBackKey = true;
    }

    private void CategoryCustomFieldsControl_SizeChanged(object sender, EventArgs e)
    {
      if (this.populatingControls)
        return;
      this.SuspendLayout();
      this.adjustWidth();
      this.ResumeLayout();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.tipCustomField = new ToolTip(this.components);
      this.pnlLeftLabel = new Panel();
      this.pnlRightData = new Panel();
      this.pnlRight = new Panel();
      this.pnlRightLabel = new Panel();
      this.pnlLeft = new Panel();
      this.pnlLeftData = new Panel();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.pnlRight.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.pnlLeftLabel.BackColor = Color.Transparent;
      this.pnlLeftLabel.Dock = DockStyle.Left;
      this.pnlLeftLabel.Location = new Point(0, 0);
      this.pnlLeftLabel.Name = "pnlLeftLabel";
      this.pnlLeftLabel.Size = new Size(95, 246);
      this.pnlLeftLabel.TabIndex = 0;
      this.pnlRightData.BackColor = Color.Transparent;
      this.pnlRightData.Dock = DockStyle.Fill;
      this.pnlRightData.Location = new Point(10, 0);
      this.pnlRightData.Name = "pnlRightData";
      this.pnlRightData.Padding = new Padding(10, 0, 0, 0);
      this.pnlRightData.Size = new Size(99, 246);
      this.pnlRightData.TabIndex = 1;
      this.pnlRight.Controls.Add((Control) this.panel2);
      this.pnlRight.Controls.Add((Control) this.pnlRightLabel);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(265, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Padding = new Padding(20, 0, 0, 0);
      this.pnlRight.Size = new Size(239, 246);
      this.pnlRight.TabIndex = 3;
      this.pnlRightLabel.BackColor = Color.Transparent;
      this.pnlRightLabel.Dock = DockStyle.Left;
      this.pnlRightLabel.Location = new Point(20, 0);
      this.pnlRightLabel.Name = "pnlRightLabel";
      this.pnlRightLabel.Size = new Size(110, 246);
      this.pnlRightLabel.TabIndex = 0;
      this.pnlLeft.Controls.Add((Control) this.panel1);
      this.pnlLeft.Controls.Add((Control) this.pnlLeftLabel);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(265, 246);
      this.pnlLeft.TabIndex = 2;
      this.pnlLeftData.BackColor = Color.Transparent;
      this.pnlLeftData.Dock = DockStyle.Fill;
      this.pnlLeftData.Location = new Point(10, 0);
      this.pnlLeftData.Name = "pnlLeftData";
      this.pnlLeftData.Size = new Size(160, 246);
      this.pnlLeftData.TabIndex = 1;
      this.panel1.Controls.Add((Control) this.pnlLeftData);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(95, 0);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(10, 0, 0, 0);
      this.panel1.Size = new Size(170, 246);
      this.panel1.TabIndex = 2;
      this.panel2.Controls.Add((Control) this.pnlRightData);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(130, 0);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(10, 0, 0, 0);
      this.panel2.Size = new Size(109, 246);
      this.panel2.TabIndex = 2;
      this.AutoScroll = true;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.pnlLeft);
      this.Name = nameof (CategoryCustomFieldsControl);
      this.Size = new Size(504, 246);
      this.SizeChanged += new EventHandler(this.CategoryCustomFieldsControl_SizeChanged);
      this.pnlRight.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public class LabelList : CollectionBase
    {
      public Label this[int index]
      {
        get => (Label) this.List[index];
        set => this.List[index] = (object) value;
      }

      public int Add(Label label) => this.List.Add((object) label);
    }

    public class ComboBoxList : CollectionBase
    {
      public ComboBox this[int index]
      {
        get => (ComboBox) this.List[index];
        set => this.List[index] = (object) value;
      }

      public int Add(ComboBox comboBox) => this.List.Add((object) comboBox);
    }
  }
}
