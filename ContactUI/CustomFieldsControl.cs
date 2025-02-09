// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFieldsControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CustomFieldsControl : UserControl, IBindingForm
  {
    private const bool ENABLED = true;
    private const bool CLEARTEXT = true;
    private bool IsDateDirty;
    private ContactType contactType;
    protected int contactId = -1;
    private object currentContact;
    private ContactCustomFieldInfo[] customFieldDefinitions;
    protected ContactCustomField[] customFieldValues;
    private CustomFieldsControl.LabelList lblFieldDescriptions = new CustomFieldsControl.LabelList();
    protected CustomFieldsControl.ComboBoxList cboFieldValues = new CustomFieldsControl.ComboBoxList();
    private List<object> additionalControls;
    private bool isReadOnly;
    private bool isUiDirty;
    private bool populatingControls = true;
    private bool deleteBackKey;
    private Panel pnlLeft;
    private Panel pnlRight;
    private Panel pnlRightData;
    private Panel pnlRightLabel;
    private Panel pnlLeftData;
    private Panel pnlLeftLabel;
    private Panel panel1;
    private Panel panel2;
    private Panel panelTPOAll;
    private Panel panelTPOLeft;
    private Panel panelTPORight;
    private int dataFieldSize;
    private ToolTip tipCustomField;
    private IContainer components;

    public event EventHandler DataChanged;

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set
      {
        if (value == this.isReadOnly)
          return;
        this.isReadOnly = value;
        this.setControlState(!this.isReadOnly, false);
      }
    }

    public bool isDirty() => this.isUiDirty;

    public void SetContactID(int contactId) => this.contactId = contactId;

    public void SetIsDirty() => this.isUiDirty = true;

    public int CurrentContactID
    {
      get => this.contactId;
      set
      {
        if (this.contactId == value)
          return;
        value = 0 > value ? -1 : value;
        this.contactId = value;
        this.customFieldValues = (ContactCustomField[]) null;
        this.setControlState(!this.isReadOnly && -1 != this.contactId, true);
        this.isUiDirty = false;
        if (0 > this.contactId)
          return;
        this.getContactInformation(this.contactId);
        this.populateFieldValues();
      }
    }

    public object CurrentContact
    {
      get => this.currentContact;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.currentContact = value;
        this.contactId = this.currentContact != null ? (!(this.currentContact is BorrowerInfo) ? ((BizPartnerInfo) value).ContactID : ((BorrowerInfo) value).ContactID) : -1;
        this.customFieldValues = (ContactCustomField[]) null;
        this.setControlState(!this.isReadOnly && -1 != this.contactId, true);
        this.isUiDirty = false;
        if (0 > this.contactId)
          return;
        this.getContactInformation(this.contactId);
        this.populateFieldValues();
      }
    }

    public ContactCustomFieldInfo[] CustomFieldInfo
    {
      get => this.customFieldDefinitions;
      set
      {
        this.customFieldDefinitions = value;
        this.populateFieldDefinitions();
      }
    }

    public CustomFieldsControl(ContactType contactType)
    {
      this.contactType = contactType;
      this.InitializeComponent();
      if (this.contactType == ContactType.TPO)
      {
        this.panelTPOLeft.Controls.Add((Control) this.pnlLeft);
        this.panelTPORight.Controls.Add((Control) this.pnlRight);
        this.pnlLeft.Dock = this.pnlRight.Dock = this.panelTPOAll.Dock = this.Dock = DockStyle.Fill;
      }
      else
      {
        this.panelTPOAll.Visible = false;
        this.pnlRight.Dock = DockStyle.Fill;
      }
    }

    public bool SaveChanges()
    {
      bool flag = false;
      if (this.isUiDirty && -1 != this.contactId)
      {
        ContactCustomField[] fields = new ContactCustomField[this.customFieldDefinitions.Length];
        for (int index = 0; index < this.customFieldDefinitions.Length; ++index)
        {
          string text = this.cboFieldValues[index].Text;
          ContactCustomFieldInfo customFieldDefinition = this.customFieldDefinitions[index];
          if (customFieldDefinition.FieldType == FieldFormat.DATE)
            Utils.FormatDateValue(text.Trim());
          fields[index] = new ContactCustomField(this.contactId, customFieldDefinition.LabelID, customFieldDefinition.OwnerID, text);
        }
        this.updateCustomFields(this.contactId, fields);
        this.isUiDirty = false;
        flag = true;
      }
      return flag;
    }

    protected void setControlState(bool enabled, bool clearText)
    {
      this.SuspendLayout();
      for (int index = 0; index < this.lblFieldDescriptions.Count; ++index)
      {
        this.cboFieldValues[index].Enabled = enabled;
        if (clearText)
          this.cboFieldValues[index].Text = string.Empty;
      }
      if (this.additionalControls != null && this.additionalControls.Count > 0)
      {
        for (int index = 0; index < this.additionalControls.Count; ++index)
        {
          Control additionalControl = (Control) this.additionalControls[index];
          if (additionalControl != null)
          {
            ComboBox tag = (ComboBox) additionalControl.Tag;
            if (tag != null)
              additionalControl.Enabled = tag.Enabled;
          }
        }
      }
      this.ResumeLayout();
    }

    protected virtual void getContactInformation(int contactId)
    {
      if (contactId > -1)
        this.customFieldValues = Session.ContactManager.GetCustomFieldsForContact(contactId, this.contactType);
      else
        this.customFieldValues = new ContactCustomField[0];
    }

    private void populateFieldDefinitions()
    {
      this.populatingControls = true;
      this.SuspendLayout();
      this.createControlArrays(this.customFieldDefinitions.Length);
      int index = 0;
      foreach (ContactCustomFieldInfo customFieldDefinition in this.customFieldDefinitions)
      {
        this.lblFieldDescriptions[index].Tag = (object) customFieldDefinition.Label;
        this.fitLabelText(this.lblFieldDescriptions[index], customFieldDefinition.Label);
        this.cboFieldValues[index].Tag = (object) customFieldDefinition;
        if (FieldFormat.DROPDOWN == customFieldDefinition.FieldType)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDown;
          this.cboFieldValues[index].MaxLength = 50;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.AddRange((object[]) this.customFieldDefinitions[index].FieldOptions);
          this.cboFieldValues[index].DrawMode = DrawMode.OwnerDrawFixed;
          this.cboFieldValues[index].DrawItem += new DrawItemEventHandler(this.cboFieldValues_DrawItem);
          this.cboFieldValues[index].EnabledChanged += new EventHandler(this.cboFieldValues_EnabledChange);
        }
        else if (FieldFormat.DROPDOWNLIST == customFieldDefinition.FieldType)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDownList;
          this.cboFieldValues[index].MaxLength = 50;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.Add((object) "");
          this.cboFieldValues[index].Items.AddRange((object[]) this.customFieldDefinitions[index].FieldOptions);
        }
        else
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.Simple;
          this.cboFieldValues[index].MaxLength = 4096;
        }
        if (this.contactType == ContactType.TPO)
        {
          this.cboFieldValues[index].MouseEnter += new EventHandler(this.cboFieldValue_MouseEnter);
          this.cboFieldValues[index].MouseLeave += new EventHandler(this.cboFieldValue_MouseLeave);
          if (customFieldDefinition.FieldType == FieldFormat.STRING || customFieldDefinition.FieldType == FieldFormat.DROPDOWN || customFieldDefinition.FieldType == FieldFormat.DROPDOWNLIST || customFieldDefinition.FieldType == FieldFormat.COMMENT)
          {
            this.cboFieldValues[index].Width = (index % 2 == 0 ? this.pnlLeftData.Width : this.pnlRightData.Width) - (customFieldDefinition.FieldType == FieldFormat.COMMENT ? 20 : 5);
            this.cboFieldValues[index].Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            if (customFieldDefinition.FieldType == FieldFormat.COMMENT)
            {
              this.cboFieldValues[index].KeyPress += new KeyPressEventHandler(this.cboCommentField_KeyPress);
              this.createCommentEditButton(this.cboFieldValues[index]);
            }
          }
          else if (customFieldDefinition.FieldType == FieldFormat.YN || customFieldDefinition.FieldType == FieldFormat.X)
          {
            this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFieldValues[index].Width = 40;
            this.cboFieldValues[index].Items.Clear();
            ComboBox.ObjectCollection items1 = this.cboFieldValues[index].Items;
            string[] strArray;
            if (customFieldDefinition.FieldType != FieldFormat.X)
              strArray = new string[3]{ "", "Y", "N" };
            else
              strArray = new string[2]{ "", "X" };
            object[] items2 = (object[]) strArray;
            items1.AddRange(items2);
            this.cboFieldValues[index].BackColor = Color.White;
          }
          else
          {
            this.cboFieldValues[index].Width = 120;
            if (customFieldDefinition.FieldType == FieldFormat.DATE)
              this.createDatePickButton(this.cboFieldValues[index]);
          }
        }
        this.dataFieldSize = this.cboFieldValues[index].Width;
        this.tipCustomField.SetToolTip((Control) this.cboFieldValues[index], FieldFormatEnumUtil.ValueToName(customFieldDefinition.FieldType));
        ++index;
      }
      this.ResumeLayout();
      this.populatingControls = false;
      this.isUiDirty = false;
    }

    protected void populateFieldValues()
    {
      this.populatingControls = true;
      this.SuspendLayout();
      Hashtable hashtable = new Hashtable(this.customFieldValues.Length);
      for (int index = 0; index < this.customFieldValues.Length; ++index)
        hashtable.Add((object) this.customFieldValues[index].FieldID, (object) this.customFieldValues[index]);
      for (int index = 0; index < this.customFieldDefinitions.Length; ++index)
      {
        ContactCustomFieldInfo customFieldDefinition = this.customFieldDefinitions[index];
        if (hashtable.Contains((object) customFieldDefinition.LabelID))
        {
          ContactCustomField contactCustomField = (ContactCustomField) hashtable[(object) customFieldDefinition.LabelID];
          this.cboFieldValues[index].Text = contactCustomField.FieldValue;
        }
      }
      this.ResumeLayout();
      this.populatingControls = false;
      this.isUiDirty = false;
    }

    protected virtual void updateCustomFields(int contactId, ContactCustomField[] fields)
    {
      try
      {
        Session.ContactManager.UpdateCustomFieldsForContact(contactId, this.contactType, fields);
      }
      catch (Exception ex)
      {
        throw new ObjectNotFoundException("Unable to update " + new ContactTypeNameProvider().GetName((object) this.contactType).ToLower() + " contact custom fields.", ObjectType.Contact, (object) contactId);
      }
    }

    private void createControlArrays(int fieldCount)
    {
      int num1 = 4;
      int num2 = 24;
      int num3 = 22;
      int num4 = num1;
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
        this.cboFieldValues[count].TabIndex = count;
        this.cboFieldValues[count].Tag = (object) (count + 1).ToString();
        this.cboFieldValues[count].KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
        this.cboFieldValues[count].TextChanged += new EventHandler(this.cboFieldValue_Changed);
        this.cboFieldValues[count].SelectedValueChanged += new EventHandler(this.cboFieldValue_Changed);
        this.cboFieldValues[count].Leave += new EventHandler(this.leaveField);
        if (count % 2 == 0)
          this.pnlLeftData.Controls.Add((Control) this.cboFieldValues[count]);
        else
          this.pnlRightData.Controls.Add((Control) this.cboFieldValues[count]);
        if (count % 2 == 0)
          num4 = num1 + count / 2 * num2;
        this.lblFieldDescriptions[count].Top = num4;
        this.lblFieldDescriptions[count].Height = num3;
        this.cboFieldValues[count].Top = num4;
        this.cboFieldValues[count].Height = num3;
      }
      this.Height = num4 + num2;
      this.adjustWidth();
    }

    private void adjustWidth()
    {
      int num1 = 0;
      int num2 = 0;
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
      this.pnlLeft.Size = new Size(num1 + this.dataFieldSize + 11, this.pnlLeft.Height);
      if (this.contactType != ContactType.TPO)
        return;
      if (this.lblFieldDescriptions.Count == 1)
      {
        this.panelTPOLeft.Dock = DockStyle.Fill;
        this.panelTPORight.Visible = false;
        foreach (Control control in (ArrangedElementCollection) this.panelTPOAll.Controls)
        {
          if (control is CollapsibleSplitter)
          {
            control.Visible = false;
            break;
          }
        }
      }
      else
        this.panelTPOLeft.Width = (this.Width - 5) / 2;
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        float width = graphics.MeasureString(text, label.Font).Width;
        if ((double) width < 400.0)
          label.Size = new Size(int.Parse(string.Concat((object) Math.Ceiling((double) width))) + 4, label.Height);
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
      FieldFormat fieldType = ((ContactCustomFieldInfo) cboFieldValue.Tag).FieldType;
      this.formatFieldValue(cboFieldValue, fieldType);
      this.isUiDirty = true;
      if (this.contactType == ContactType.TPO)
        this.cboFieldValue_MouseEnter(sender, e);
      if (this.DataChanged == null)
        return;
      this.DataChanged((object) null, (EventArgs) null);
    }

    private void cboFieldValue_MouseEnter(object sender, EventArgs e)
    {
      ComboBox window = (ComboBox) sender;
      ContactCustomFieldInfo tag = (ContactCustomFieldInfo) window.Tag;
      string empty = string.Empty;
      this.tipCustomField.Show(tag.FieldType != FieldFormat.DATE ? FieldFormatEnumUtil.ValueToName(tag.FieldType) + ":\r\n" + window.Text : "DATE (mm/dd/yyyy):\r\n" + window.Text, (IWin32Window) window, window.Left + window.Width / 2, window.Height);
    }

    private void cboFieldValue_MouseLeave(object sender, EventArgs e)
    {
      this.tipCustomField.Hide((IWin32Window) this);
    }

    private void cboFieldValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.populatingControls)
        return;
      if (this.contactType == ContactType.TPO && Keys.Delete == e.KeyCode && ((ContactCustomFieldInfo) ((Control) sender).Tag).FieldType == FieldFormat.COMMENT && Utils.Dialog((IWin32Window) this, "Are you sure you want to delete all comments in this field?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        e.Handled = true;
      }
      else
      {
        if (Keys.Back != e.KeyCode && Keys.Delete != e.KeyCode)
          return;
        this.deleteBackKey = true;
      }
    }

    private void cboCommentField_KeyPress(object sender, KeyPressEventArgs e) => e.Handled = true;

    private void CustomFieldsControl_SizeChanged(object sender, EventArgs e)
    {
      if (this.populatingControls)
        return;
      this.SuspendLayout();
      this.adjustWidth();
      this.ResumeLayout();
    }

    private void createCommentEditButton(ComboBox comboBox)
    {
      if (this.additionalControls == null)
        this.additionalControls = new List<object>();
      StandardIconButton standardIconButton = new StandardIconButton();
      standardIconButton.BackColor = Color.Transparent;
      standardIconButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      standardIconButton.MouseDownImage = (Image) null;
      standardIconButton.Name = "btnComment_" + comboBox.Name;
      standardIconButton.Size = new Size(16, 16);
      standardIconButton.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      standardIconButton.TabIndex = 0;
      standardIconButton.TabStop = false;
      standardIconButton.Tag = (object) comboBox;
      standardIconButton.Text = "...";
      standardIconButton.Click += new EventHandler(this.editCommentBtn_Click);
      standardIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      standardIconButton.Location = new Point(comboBox.Left + comboBox.Width + 2, comboBox.Top + (comboBox.Height - standardIconButton.Height) / 2);
      this.tipCustomField.SetToolTip((Control) standardIconButton, "Edit Comment...");
      comboBox.Parent.Controls.Add((Control) standardIconButton);
      this.additionalControls.Add((object) standardIconButton);
    }

    private void createDatePickButton(ComboBox comboBox)
    {
      if (this.additionalControls == null)
        this.additionalControls = new List<object>();
      DateTimePicker dateTimePicker = new DateTimePicker();
      dateTimePicker.CustomFormat = "MM/dd/yyyy";
      dateTimePicker.Format = DateTimePickerFormat.Custom;
      dateTimePicker.ValueChanged += new EventHandler(this.dateTimePicker_ValueChanged);
      dateTimePicker.Location = new Point(296, 137);
      dateTimePicker.Name = "datePicter_" + comboBox.Name;
      dateTimePicker.Size = new Size(19, 20);
      dateTimePicker.Tag = (object) comboBox;
      dateTimePicker.TabIndex = 0;
      dateTimePicker.TabStop = false;
      dateTimePicker.Width = dateTimePicker.Height;
      dateTimePicker.CloseUp += new EventHandler(this.dateTimePicker_CloseUp);
      dateTimePicker.Location = new Point(comboBox.Left + comboBox.Width + 2, comboBox.Top + (comboBox.Height - dateTimePicker.Height) / 2);
      this.tipCustomField.SetToolTip((Control) dateTimePicker, "Calendar Button");
      comboBox.Parent.Controls.Add((Control) dateTimePicker);
      this.additionalControls.Add((object) dateTimePicker);
    }

    private void leaveField(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      ContactCustomFieldInfo tag = (ContactCustomFieldInfo) comboBox.Tag;
      try
      {
        if (tag.FieldType != FieldFormat.DATE && tag.FieldType != FieldFormat.DATETIME)
          return;
        comboBox.Text = Utils.FormatDateValue(comboBox.Text.Trim());
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void dateTimePicker_ValueChanged(object sender, EventArgs e) => this.IsDateDirty = true;

    private void dateTimePicker_CloseUp(object sender, EventArgs e)
    {
      DateTimePicker dateTimePicker = (DateTimePicker) sender;
      if (dateTimePicker == null)
        return;
      ComboBox tag = (ComboBox) dateTimePicker.Tag;
      if (tag == null)
        return;
      DateTime dateTime = dateTimePicker.Value;
      if (this.IsDateDirty)
        tag.Text = dateTimePicker.Value.ToString("MM/dd/yyyy");
      this.IsDateDirty = false;
    }

    private void editCommentBtn_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == null)
        return;
      ComboBox tag = (ComboBox) standardIconButton.Tag;
      if (tag == null)
        return;
      using (CommentFieldEditorForm commentFieldEditorForm = new CommentFieldEditorForm(tag))
      {
        if (commentFieldEditorForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        tag.Text = commentFieldEditorForm.UpdatedComment;
      }
    }

    private void cboFieldValues_DrawItem(object sender, DrawItemEventArgs e)
    {
      Graphics graphics = e.Graphics;
      Rectangle bounds = e.Bounds;
      if (e.Index >= 0)
      {
        string s = ((ComboBox) sender).Items[e.Index].ToString();
        if (e.State == (DrawItemState.ComboBoxEdit | DrawItemState.Disabled | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
        {
          e.Graphics.FillRectangle((Brush) new SolidBrush(Color.White), bounds);
          graphics.DrawString(s, e.Font, Brushes.Black, (RectangleF) bounds);
          e.DrawFocusRectangle();
        }
        else if (e.State.HasFlag((Enum) DrawItemState.Selected) && !e.State.HasFlag((Enum) DrawItemState.ComboBoxEdit))
        {
          e.Graphics.FillRectangle((Brush) new SolidBrush(Color.DodgerBlue), bounds);
          graphics.DrawString(s, e.Font, Brushes.White, (RectangleF) bounds);
          e.DrawFocusRectangle();
        }
        else
        {
          e.Graphics.FillRectangle((Brush) new SolidBrush(Color.White), bounds);
          graphics.DrawString(s, e.Font, Brushes.Black, (RectangleF) bounds);
        }
      }
      graphics.Dispose();
    }

    private void cboFieldValues_EnabledChange(object sender, EventArgs e)
    {
      if (((Control) sender).Enabled)
        ((ComboBox) sender).DropDownStyle = ComboBoxStyle.DropDown;
      else
        ((ComboBox) sender).DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.panelTPOLeft = new Panel();
      this.tipCustomField = new ToolTip(this.components);
      this.pnlLeft = new Panel();
      this.panel1 = new Panel();
      this.pnlLeftData = new Panel();
      this.pnlLeftLabel = new Panel();
      this.pnlRight = new Panel();
      this.panel2 = new Panel();
      this.pnlRightData = new Panel();
      this.pnlRightLabel = new Panel();
      this.panelTPOAll = new Panel();
      this.panelTPORight = new Panel();
      CollapsibleSplitter collapsibleSplitter = new CollapsibleSplitter();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panelTPOAll.SuspendLayout();
      this.SuspendLayout();
      collapsibleSplitter.AnimationDelay = 20;
      collapsibleSplitter.AnimationStep = 20;
      collapsibleSplitter.BorderStyle3D = Border3DStyle.Flat;
      collapsibleSplitter.CollapsedPanelSize = 20;
      collapsibleSplitter.ControlToHide = (Control) this.panelTPOLeft;
      collapsibleSplitter.ExpandParentForm = false;
      collapsibleSplitter.Location = new Point(38, 0);
      collapsibleSplitter.Name = "collapsibleSplitter1";
      collapsibleSplitter.TabIndex = 20;
      collapsibleSplitter.TabStop = false;
      collapsibleSplitter.UseAnimations = false;
      collapsibleSplitter.VisualStyle = VisualStyles.Encompass;
      collapsibleSplitter.MouseClick += new MouseEventHandler(this.collapsibleSplitterTPO_MouseClick);
      this.panelTPOLeft.BackColor = Color.White;
      this.panelTPOLeft.Dock = DockStyle.Left;
      this.panelTPOLeft.Location = new Point(0, 0);
      this.panelTPOLeft.Name = "panelTPOLeft";
      this.panelTPOLeft.Size = new Size(38, 100);
      this.panelTPOLeft.TabIndex = 0;
      this.pnlLeft.Controls.Add((Control) this.panel1);
      this.pnlLeft.Controls.Add((Control) this.pnlLeftLabel);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(3, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(259, 397);
      this.pnlLeft.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.pnlLeftData);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(95, 0);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(10, 0, 0, 0);
      this.panel1.Size = new Size(164, 397);
      this.panel1.TabIndex = 2;
      this.pnlLeftData.BackColor = Color.Transparent;
      this.pnlLeftData.Dock = DockStyle.Fill;
      this.pnlLeftData.Location = new Point(10, 0);
      this.pnlLeftData.Name = "pnlLeftData";
      this.pnlLeftData.Size = new Size(154, 397);
      this.pnlLeftData.TabIndex = 1;
      this.pnlLeftLabel.BackColor = Color.Transparent;
      this.pnlLeftLabel.Dock = DockStyle.Left;
      this.pnlLeftLabel.Location = new Point(0, 0);
      this.pnlLeftLabel.Name = "pnlLeftLabel";
      this.pnlLeftLabel.Size = new Size(95, 397);
      this.pnlLeftLabel.TabIndex = 0;
      this.pnlRight.Controls.Add((Control) this.panel2);
      this.pnlRight.Controls.Add((Control) this.pnlRightLabel);
      this.pnlRight.Dock = DockStyle.Right;
      this.pnlRight.Location = new Point(386, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Padding = new Padding(20, 0, 0, 0);
      this.pnlRight.Size = new Size(351, 397);
      this.pnlRight.TabIndex = 1;
      this.panel2.Controls.Add((Control) this.pnlRightData);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(130, 0);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(10, 0, 0, 0);
      this.panel2.Size = new Size(221, 397);
      this.panel2.TabIndex = 2;
      this.pnlRightData.BackColor = Color.Transparent;
      this.pnlRightData.Dock = DockStyle.Fill;
      this.pnlRightData.Location = new Point(10, 0);
      this.pnlRightData.Name = "pnlRightData";
      this.pnlRightData.Size = new Size(211, 397);
      this.pnlRightData.TabIndex = 1;
      this.pnlRightLabel.BackColor = Color.Transparent;
      this.pnlRightLabel.Dock = DockStyle.Left;
      this.pnlRightLabel.Location = new Point(20, 0);
      this.pnlRightLabel.Name = "pnlRightLabel";
      this.pnlRightLabel.Size = new Size(110, 397);
      this.pnlRightLabel.TabIndex = 0;
      this.panelTPOAll.Controls.Add((Control) this.panelTPORight);
      this.panelTPOAll.Controls.Add((Control) collapsibleSplitter);
      this.panelTPOAll.Controls.Add((Control) this.panelTPOLeft);
      this.panelTPOAll.Location = new Point(278, 189);
      this.panelTPOAll.Name = "panelTPOAll";
      this.panelTPOAll.Size = new Size(87, 100);
      this.panelTPOAll.TabIndex = 2;
      this.panelTPORight.BackColor = Color.White;
      this.panelTPORight.Dock = DockStyle.Fill;
      this.panelTPORight.Location = new Point(45, 0);
      this.panelTPORight.Name = "panelTPORight";
      this.panelTPORight.Size = new Size(42, 100);
      this.panelTPORight.TabIndex = 21;
      this.AutoScroll = true;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.panelTPOAll);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.pnlLeft);
      this.Name = nameof (CustomFieldsControl);
      this.Padding = new Padding(3, 0, 0, 0);
      this.Size = new Size(737, 397);
      this.SizeChanged += new EventHandler(this.CustomFieldsControl_SizeChanged);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panelTPOAll.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void collapsibleSplitterTPO_MouseClick(object sender, MouseEventArgs e)
    {
      CollapsibleSplitter collapsibleSplitter = (CollapsibleSplitter) sender;
      if (collapsibleSplitter == null || e == null || e.Y > this.panelTPOAll.Height / 2 + 20 || e.Y < this.panelTPOAll.Height / 2 - 20)
        return;
      if (e.Y > this.panelTPOAll.Height / 2)
        collapsibleSplitter.SplitPosition = this.panelTPOAll.Width - collapsibleSplitter.Width;
      else
        collapsibleSplitter.SplitPosition = 20;
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
