// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PopupBusinessRules
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PopupBusinessRules
  {
    private ResourceManager resources;
    private Hashtable fieldRights;
    private System.Drawing.Image asteriskImage;
    private System.Drawing.Image downArrowImage;
    private LoanData loan;
    private Hashtable dropdownList;
    private bool isSuperAdmin;
    private Sessions.Session session = Session.DefaultInstance;

    public event EventHandler DropdownSelected;

    public PopupBusinessRules(
      LoanData loan,
      ResourceManager resources,
      System.Drawing.Image asteriskImage,
      System.Drawing.Image downArrowImage,
      Sessions.Session session)
    {
      this.session = session;
      if (this.session.UserInfo.IsSuperAdministrator())
        this.isSuperAdmin = true;
      this.resources = resources;
      this.asteriskImage = asteriskImage;
      this.downArrowImage = downArrowImage;
      this.loan = loan;
      if (this.loan != null && !this.loan.IsInFindFieldForm)
        this.fieldRights = !this.loan.IsTemplate ? this.session.LoanDataMgr.GetFieldAccessList() : (Hashtable) null;
      if (this.session.LoanDataMgr == null)
        return;
      this.dropdownList = this.session.LoanDataMgr.GetDropdownFieldRuleList();
    }

    public void InitializeControls(System.Windows.Forms.Control parentControl)
    {
      this.InitializeControls(parentControl, (ToolTip) null);
    }

    public void InitializeControls(System.Windows.Forms.Control parentControl, ToolTip toolTip)
    {
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) parentControl.Controls)
      {
        if (control is System.Windows.Forms.Button)
          this.SetBusinessRules(control as System.Windows.Forms.Button);
        else if (PopupBusinessRules.IsFieldControl(control))
          this.InitializeFieldControl(control, toolTip);
        else if (control.Controls.Count > 0)
          this.InitializeControls(control, toolTip);
      }
    }

    public void InitializeFieldControl(System.Windows.Forms.Control ctrl, ToolTip toolTip)
    {
      if (ctrl is ComboBox)
        this.PopulateDropdownList(ctrl as ComboBox);
      this.PopulateFieldValue(ctrl);
      this.SetBusinessRules((object) ctrl, string.Concat(ctrl.Tag), toolTip);
    }

    public bool PopulateDropdownList(ComboBox cbo)
    {
      if (cbo.Items.Count > 0)
        return false;
      FieldDefinition fieldDefinition = this.GetFieldDefinition((System.Windows.Forms.Control) cbo);
      if (fieldDefinition == null)
        return false;
      cbo.Items.Add((object) FieldOption.Empty);
      foreach (FieldOption option in fieldDefinition.Options)
        cbo.Items.Add((object) option);
      return true;
    }

    public FieldDefinition GetFieldDefinition(System.Windows.Forms.Control ctrl)
    {
      string fieldId = string.Concat(ctrl.Tag);
      return fieldId == "" ? (FieldDefinition) null : EncompassFields.GetField(fieldId, this.loan.Settings.FieldSettings);
    }

    public bool PopulateFieldValue(System.Windows.Forms.Control ctrl)
    {
      string id = string.Concat(ctrl.Tag);
      if (id == "")
        return false;
      string field = this.loan.GetField(id);
      switch (ctrl)
      {
        case System.Windows.Forms.CheckBox _:
          if (field == "Y")
          {
            ((System.Windows.Forms.CheckBox) ctrl).Checked = true;
            break;
          }
          break;
        case DatePicker _:
          ((DatePicker) ctrl).Value = Utils.ParseDate((object) field);
          break;
        default:
          ctrl.Text = field;
          break;
      }
      return true;
    }

    public static bool IsFieldControl(System.Windows.Forms.Control c)
    {
      if (string.Concat(c.Tag) == "")
        return false;
      switch (c)
      {
        case System.Windows.Forms.TextBox _:
          return true;
        case ComboBox _:
          return true;
        case System.Windows.Forms.CheckBox _:
          return true;
        case DatePicker _:
          return true;
        default:
          return false;
      }
    }

    public bool RuleValidate(object ctrl, string id)
    {
      if (this.isSuperAdmin || this.loan != null && this.loan.IsInFindFieldForm || !(ctrl is System.Windows.Forms.TextBox))
        return true;
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) ctrl;
      if (!(textBox.Text.Trim() == ""))
      {
        if (!(id == ""))
        {
          try
          {
            if (id != null)
            {
              if (this.loan != null)
              {
                if (this.loan.Validator != null)
                  this.loan.Validator.Validate(id, textBox.Text);
              }
            }
          }
          catch (MissingPrerequisiteException ex)
          {
            StandardField field = StandardFields.GetField(ex.FieldID);
            if (field != null)
            {
              int num1 = (int) Utils.Dialog((IWin32Window) null, "The field '" + field.Description + "' (" + field.FieldID + ") is required in order to complete this field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              int num2 = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            textBox.Text = this.loan.GetField(id);
            return false;
          }
          catch (ValidationException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            textBox.Text = this.loan.GetField(id);
            return false;
          }
          return true;
        }
      }
      return true;
    }

    public BizRule.FieldAccessRight GetFieldAccessRight(string fieldID)
    {
      return this.session.LoanDataMgr.GetFieldAccessRights(fieldID);
    }

    public void SetBusinessRules(System.Windows.Forms.Button button)
    {
      string actionID = "BUTTON_" + button.Text.Trim().Replace("&", "");
      this.SetButtonAccessMode(button, actionID);
    }

    public void SetBusinessRules(System.Windows.Forms.Button[] buttons)
    {
      for (int index = 0; index < buttons.Length; ++index)
        this.SetBusinessRules(buttons[index]);
    }

    public void SetBusinessRules(object ctrl, string fieldID)
    {
      if (ctrl is System.Windows.Forms.Button)
        this.SetButtonAccessMode((System.Windows.Forms.Button) ctrl, fieldID);
      else
        this.SetBusinessRules(ctrl, fieldID, true);
    }

    public void SetBusinessRules(object ctrl, string fieldID, ToolTip toolTip)
    {
      if (ctrl is System.Windows.Forms.Button)
        this.SetButtonAccessMode((System.Windows.Forms.Button) ctrl, fieldID);
      else
        this.SetBusinessRules(ctrl, fieldID, true, toolTip);
    }

    public void SetBusinessRules(object ctrl, string fieldID, bool setDropDown)
    {
      this.SetBusinessRules(ctrl, fieldID, setDropDown, (ToolTip) null);
    }

    public void SetBusinessRules(object ctrl, string fieldID, bool setDropDown, ToolTip toolTip)
    {
      if (toolTip != null && ctrl is System.Windows.Forms.Control)
      {
        System.Windows.Forms.Control control = (System.Windows.Forms.Control) ctrl;
        EncompassFields.GetDescription(fieldID);
        toolTip.SetToolTip(control, FieldHelp.GetText(fieldID) == "" ? fieldID : fieldID + ": " + FieldHelp.GetText(fieldID));
        control.Tag = (object) fieldID;
      }
      if (this.isSuperAdmin || this.loan != null && this.loan.IsInFindFieldForm)
        return;
      if (ctrl is FieldLockButton && !fieldID.StartsWith("LOCKBUTTON_"))
        fieldID = "LOCKBUTTON_" + fieldID;
      if (this.fieldRights != null && this.fieldRights.ContainsKey((object) fieldID))
      {
        BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights(fieldID);
        System.Windows.Forms.TextBox textBox1 = (System.Windows.Forms.TextBox) null;
        switch (fieldAccessRights)
        {
          case BizRule.FieldAccessRight.Hide:
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = this.asteriskImage;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox" + fieldID;
            pictureBox.TabStop = false;
            if (ctrl is System.Windows.Forms.TextBox)
            {
              System.Windows.Forms.TextBox textBox2 = (System.Windows.Forms.TextBox) ctrl;
              pictureBox.BackColor = textBox2.BackColor;
              pictureBox.Size = new Size(textBox2.Width, textBox2.Height);
              textBox2.Controls.Add((System.Windows.Forms.Control) pictureBox);
              goto case BizRule.FieldAccessRight.ViewOnly;
            }
            else if (ctrl is System.Windows.Forms.CheckBox)
            {
              System.Windows.Forms.CheckBox checkBox = (System.Windows.Forms.CheckBox) ctrl;
              pictureBox.BackColor = checkBox.BackColor;
              pictureBox.Location = new Point(2, 3);
              pictureBox.Size = new Size(10, 10);
              checkBox.Controls.Add((System.Windows.Forms.Control) pictureBox);
              goto case BizRule.FieldAccessRight.ViewOnly;
            }
            else if (ctrl is ComboBox)
            {
              ComboBox box = (ComboBox) ctrl;
              box.Visible = false;
              textBox1 = this.createTextBox(false, (System.Windows.Forms.Control) box, pictureBox);
              box.Parent.Controls.Add((System.Windows.Forms.Control) textBox1);
              goto case BizRule.FieldAccessRight.ViewOnly;
            }
            else if (ctrl is FieldLockButton)
            {
              ((System.Windows.Forms.Control) ctrl).Visible = false;
              goto case BizRule.FieldAccessRight.ViewOnly;
            }
            else if (ctrl is DatePicker)
            {
              DatePicker box = (DatePicker) ctrl;
              box.Visible = false;
              textBox1 = this.createTextBox(false, (System.Windows.Forms.Control) box, pictureBox);
              box.Parent.Controls.Add((System.Windows.Forms.Control) textBox1);
              goto case BizRule.FieldAccessRight.ViewOnly;
            }
            else
              goto case BizRule.FieldAccessRight.ViewOnly;
          case BizRule.FieldAccessRight.ViewOnly:
            if (ctrl is System.Windows.Forms.TextBox)
            {
              System.Windows.Forms.TextBox textBox3 = (System.Windows.Forms.TextBox) ctrl;
              textBox3.ReadOnly = true;
              textBox3.TabStop = false;
              break;
            }
            if (ctrl is System.Windows.Forms.CheckBox)
            {
              System.Windows.Forms.CheckBox checkBox = (System.Windows.Forms.CheckBox) ctrl;
              checkBox.Enabled = false;
              checkBox.TabStop = false;
              break;
            }
            if (ctrl is ComboBox)
            {
              ComboBox box = (ComboBox) ctrl;
              box.Visible = false;
              box.Enabled = false;
              box.TabStop = false;
              if (textBox1 != null)
                break;
              System.Windows.Forms.TextBox textBox4 = this.createTextBox(true, (System.Windows.Forms.Control) box, (PictureBox) null);
              textBox4.Text = box.Text;
              textBox4.Tag = (object) fieldID;
              box.Parent.Controls.Add((System.Windows.Forms.Control) textBox4);
              break;
            }
            if (ctrl is FieldLockButton)
            {
              ((System.Windows.Forms.Control) ctrl).Enabled = false;
              break;
            }
            if (!(ctrl is DatePicker))
              break;
            DatePicker box1 = (DatePicker) ctrl;
            box1.Visible = false;
            box1.Enabled = false;
            box1.TabStop = false;
            if (textBox1 != null)
              break;
            System.Windows.Forms.TextBox textBox5 = this.createTextBox(true, (System.Windows.Forms.Control) box1, (PictureBox) null);
            textBox5.Text = box1.Text;
            box1.Parent.Controls.Add((System.Windows.Forms.Control) textBox5);
            break;
          default:
            switch (ctrl)
            {
              case System.Windows.Forms.TextBox _:
                this.setupDropdownBox((System.Windows.Forms.Control) ctrl, fieldID, setDropDown);
                return;
              case ComboBox _:
                if (((ComboBox) ctrl).DropDownStyle != ComboBoxStyle.Simple)
                  return;
                this.setupDropdownBox((System.Windows.Forms.Control) ctrl, fieldID, setDropDown);
                return;
              default:
                return;
            }
        }
      }
      else
      {
        switch (ctrl)
        {
          case System.Windows.Forms.TextBox _:
            this.setupDropdownBox((System.Windows.Forms.Control) ctrl, fieldID, setDropDown);
            break;
          case ComboBox _:
            if (((ComboBox) ctrl).DropDownStyle != ComboBoxStyle.Simple)
              break;
            this.setupDropdownBox((System.Windows.Forms.Control) ctrl, fieldID, setDropDown);
            break;
        }
      }
    }

    public void SetButtonAccessMode(System.Windows.Forms.Button button, string actionID)
    {
      if (this.isSuperAdmin || this.loan != null && this.loan.IsInFindFieldForm)
        return;
      if (!actionID.ToLower().StartsWith("button_"))
        actionID = "Button_" + actionID;
      if (this.fieldRights == null || !this.fieldRights.ContainsKey((object) actionID))
        return;
      switch (this.session.LoanDataMgr.GetFieldAccessRights(actionID))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    public bool IsButtonVisible(string actionID)
    {
      if (this.isSuperAdmin || this.loan != null && this.loan.IsInFindFieldForm)
        return true;
      if (!actionID.ToLower().StartsWith("button_"))
        actionID = "Button_" + actionID;
      return this.fieldRights == null || !this.fieldRights.ContainsKey((object) actionID) || this.session.LoanDataMgr.GetFieldAccessRights(actionID) != BizRule.FieldAccessRight.Hide;
    }

    private System.Windows.Forms.TextBox createTextBox(
      bool readOnly,
      System.Windows.Forms.Control box,
      PictureBox pictureBox)
    {
      System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
      textBox.Size = new Size(box.Width, box.Height);
      textBox.ReadOnly = true;
      textBox.TabStop = false;
      textBox.Visible = true;
      textBox.Location = new Point(box.Left, box.Top);
      textBox.Tag = box.Tag;
      textBox.Enter += new EventHandler(this.enterField);
      if (!readOnly)
      {
        pictureBox.Location = new Point(2, 2);
        pictureBox.Size = new Size(textBox.Width, textBox.Height);
        pictureBox.BackColor = textBox.BackColor;
        textBox.Controls.Add((System.Windows.Forms.Control) pictureBox);
      }
      return textBox;
    }

    private void enterField(object sender, EventArgs e)
    {
      if (this.session.MainScreen == null || sender == null || !(sender is System.Windows.Forms.TextBox))
        return;
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      if (textBox.Tag == null)
        return;
      this.displayFieldID(textBox.Tag.ToString());
    }

    private void displayFieldID(string fieldId)
    {
      this.session.Application.GetService<IStatusDisplay>().DisplayFieldID(fieldId);
    }

    private void setupDropdownBox(System.Windows.Forms.Control box, string fieldID, bool setDropDown)
    {
      if (this.dropdownList == null || !this.dropdownList.ContainsKey((object) fieldID))
        return;
      if (setDropDown)
      {
        if (box.Width > 19)
          box.Width -= 19;
        System.Windows.Forms.Button button = new System.Windows.Forms.Button();
        button.Name = box.Name + "_C";
        button.Top = box.Top + 1;
        button.Left = box.Left + box.Width + 2;
        button.Width = 17;
        button.Height = 17;
        button.FlatAppearance.BorderSize = 0;
        button.FlatStyle = FlatStyle.Flat;
        button.BackgroundImage = this.downArrowImage;
        button.Click += new EventHandler(this.btnDownArrow_Click);
        button.Tag = (object) box;
        box.Parent.Controls.Add((System.Windows.Forms.Control) button);
      }
      if (!((FRList) this.dropdownList[(object) fieldID]).IsLock)
        return;
      switch (box)
      {
        case System.Windows.Forms.TextBox _:
          System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) box;
          textBox.ReadOnly = true;
          textBox.TabStop = false;
          break;
        case ComboBox _:
          ComboBox comboBox = (ComboBox) box;
          if (comboBox.DropDownStyle != ComboBoxStyle.Simple)
            break;
          comboBox.Enabled = false;
          comboBox.TabStop = false;
          break;
      }
    }

    private void btnDownArrow_Click(object sender, EventArgs e)
    {
      System.Windows.Forms.Button button = (System.Windows.Forms.Button) sender;
      if (button.Tag == null)
        return;
      System.Windows.Forms.TextBox sender1 = (System.Windows.Forms.TextBox) null;
      ComboBox sender2 = (ComboBox) null;
      if (button.Tag is System.Windows.Forms.TextBox)
        sender1 = (System.Windows.Forms.TextBox) button.Tag;
      else if (button.Tag is ComboBox)
        sender2 = (ComboBox) button.Tag;
      if (sender1 == null && sender2 == null || sender1 != null && sender1.Tag == null || sender2 != null && sender2.Tag == null)
        return;
      string str = string.Empty;
      if (sender1 != null)
        str = (string) sender1.Tag;
      else if (sender2 != null)
        str = (string) sender2.Tag;
      string[] ruleDropdownOptions = this.session.LoanDataMgr.GetFieldRuleDropdownOptions(str);
      if (ruleDropdownOptions == null || ruleDropdownOptions.Length == 0)
        return;
      int w = 20;
      if (sender1 != null)
        w = Math.Max(sender1.Width, 20);
      else if (sender2 != null)
        w = Math.Max(sender2.Width, 20);
      Point position = Cursor.Position;
      int x1 = position.X;
      position = Cursor.Position;
      int y1 = position.Y;
      int x2 = x1 - w;
      int y2 = y1 + 10;
      DropdownOption[] options = new DropdownOption[ruleDropdownOptions.Length];
      for (int index = 0; index < ruleDropdownOptions.Length; ++index)
        options[index] = new DropdownOption(ruleDropdownOptions[index]);
      using (FieldRuleDropdownDialog ruleDropdownDialog = new FieldRuleDropdownDialog(options, str))
      {
        if (ruleDropdownDialog.Height + y2 > Screen.PrimaryScreen.Bounds.Height)
          y2 = y1 - ruleDropdownDialog.Height - 40;
        ruleDropdownDialog.SetListBoxWidth(w);
        ruleDropdownDialog.Location = new Point(x2, y2);
        if (ruleDropdownDialog.ShowDialog((IWin32Window) this.session.MainScreen) != DialogResult.OK)
          return;
        if (sender1 != null)
        {
          sender1.Text = ruleDropdownDialog.SelectedItem.Text;
          if (this.DropdownSelected == null)
            return;
          this.DropdownSelected((object) sender1, e);
        }
        else
        {
          if (sender2 == null)
            return;
          sender2.Text = ruleDropdownDialog.SelectedItem.Text;
          if (this.DropdownSelected == null)
            return;
          this.DropdownSelected((object) sender2, e);
        }
      }
    }

    public static bool SetFieldWithRuleCheck(string id, string val, IHtmlInput inputData)
    {
      if (inputData is LoanData && !PopupBusinessRules.RequiredFieldRuleCheck(id, inputData))
        return false;
      try
      {
        inputData.SetField(id, val);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return true;
    }

    public static bool RequiredFieldRuleCheck(string id, IHtmlInput inputData)
    {
      if (inputData is LoanData)
      {
        LoanData loanData = (LoanData) inputData;
        if (!loanData.IsTemplate && new BusinessRuleCheck().HasPrerequiredFields(loanData, id, true, (Hashtable) null))
          return false;
      }
      return true;
    }
  }
}
