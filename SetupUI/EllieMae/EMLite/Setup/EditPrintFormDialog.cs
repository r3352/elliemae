// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditPrintFormDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditPrintFormDialog : Form
  {
    private Sessions.Session session;
    private Hashtable reqFields;
    private Hashtable existingForms;
    private PrintRequiredFieldsInfo requiredFieldInfo;
    private IContainer components;
    private GroupBox groupBox1;
    private Label label2;
    private TextBox txtFormName;
    private Button findBtn;
    private Label label8;
    private Button removeBtn;
    private Button findFieldBtn;
    private Button addBtn;
    private Button okBtn;
    private Button cancelBtn;
    private CheckBox chkPrintBlank;
    private TextBox txtCoding;
    private Button btnValidation;
    private Label label1;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageFields;
    private TabPageEx tabPageCodes;
    private GridView gridViewFields;

    public EditPrintFormDialog(
      Sessions.Session session,
      PrintRequiredFieldsInfo requiredFieldInfo,
      Hashtable existingForms)
    {
      this.session = session;
      this.requiredFieldInfo = requiredFieldInfo;
      this.existingForms = existingForms;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.chkPrintBlank.Checked = false;
      this.reqFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.requiredFieldInfo == null)
        return;
      FormInfo formItem = new FormInfo(this.requiredFieldInfo.FormID, this.requiredFieldInfo.FormType);
      this.txtFormName.Text = this.buildUIFormName(formItem);
      this.txtFormName.Tag = (object) formItem;
      this.txtCoding.Text = this.requiredFieldInfo.AdvancedCoding;
      if (this.requiredFieldInfo.FieldIDs == null)
        return;
      this.gridViewFields.BeginUpdate();
      foreach (string fieldId in this.requiredFieldInfo.FieldIDs)
      {
        if (fieldId == PrintRequiredFieldsInfo.PRINTBLANKID)
        {
          this.chkPrintBlank.Checked = true;
        }
        else
        {
          this.gridViewFields.Items.Add(new GVItem(fieldId)
          {
            SubItems = {
              (object) this.getFieldDescription(fieldId)
            }
          });
          this.reqFields.Add((object) fieldId, (object) fieldId);
        }
      }
      this.gridViewFields.EndUpdate();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewFields.Items.Count == 0 && this.txtCoding.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a required field or advanced coding.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.txtCoding.Text.Trim() != string.Empty && !this.validateAdvancedCode())
          return;
        if (this.txtFormName.Text == string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter a form name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int index = 0;
          string[] fieldIDs = !this.chkPrintBlank.Checked ? new string[this.reqFields.Keys.Count] : new string[this.reqFields.Keys.Count + 1];
          foreach (DictionaryEntry reqField in this.reqFields)
            fieldIDs[index++] = reqField.Key.ToString();
          if (this.chkPrintBlank.Checked)
            fieldIDs[index] = PrintRequiredFieldsInfo.PRINTBLANKID;
          FormInfo tag = (FormInfo) this.txtFormName.Tag;
          this.requiredFieldInfo = new PrintRequiredFieldsInfo(tag.Name, tag.Type, fieldIDs, this.txtCoding.Text);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
    }

    private void addFields(string[] ids)
    {
      this.gridViewFields.BeginUpdate();
      for (int index = 0; index < ids.Length; ++index)
      {
        if (this.reqFields.ContainsKey((object) ids[index]))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The field list already contains field '" + ids[index] + "'. This field will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.reqFields.Add((object) ids[index], (object) ids[index]);
          this.gridViewFields.Items.Add(new GVItem(ids[index])
          {
            SubItems = {
              (object) this.getFieldDescription(ids[index])
            },
            Selected = true
          });
        }
      }
      this.gridViewFields.EndUpdate();
    }

    private string getFieldDescription(string fieldID) => EncompassFields.GetDescription(fieldID);

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.gridViewFields.SelectedItems[0].Index;
        for (int nItemIndex = this.gridViewFields.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gridViewFields.Items[nItemIndex].Selected)
          {
            string text = this.gridViewFields.Items[nItemIndex].Text;
            this.gridViewFields.Items.RemoveAt(nItemIndex);
            if (this.reqFields.ContainsKey((object) text))
              this.reqFields.Remove((object) text);
          }
        }
        if (this.gridViewFields.Items.Count == 0)
          return;
        if (index > this.gridViewFields.Items.Count - 1)
          this.gridViewFields.Items[this.gridViewFields.Items.Count - 1].Selected = true;
        else
          this.gridViewFields.Items[index].Selected = true;
      }
    }

    public PrintRequiredFieldsInfo RequiredFieldInfo => this.requiredFieldInfo;

    private void findBtn_Click(object sender, EventArgs e)
    {
      using (AddPrintFormDialog addPrintFormDialog = new AddPrintFormDialog(this.session, this.existingForms))
      {
        if (addPrintFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (Session.DefaultInstance == null)
          OutputFormNameMap.LoadFormNameMap(this.session);
        FormInfo formItem = new FormInfo(OutputFormNameMap.GetFormNameToKey(addPrintFormDialog.SelectedForm.FormName, this.session), addPrintFormDialog.SelectedForm.FormType);
        this.txtFormName.Text = this.buildUIFormName(formItem);
        this.txtFormName.Tag = (object) formItem;
      }
    }

    private string buildUIFormName(FormInfo formItem)
    {
      string formKey = formItem.Name;
      int num = formKey.LastIndexOf("\\");
      if (num > -1)
        formKey = formItem.Name.Substring(num + 1);
      if (formItem.Type == OutputFormType.CustomLetters)
      {
        if (formKey.ToLower().EndsWith(".doc") || formKey.ToLower().EndsWith(".rtf"))
          formKey = formKey.Substring(0, formKey.Length - 4);
        else if (formKey.ToLower().EndsWith(".docx"))
          formKey = formKey.Substring(0, formKey.Length - 5);
      }
      else
        formKey = OutputFormNameMap.GetFormKeyToName(formKey, this.session);
      return formKey;
    }

    private void findFieldBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        arrayList.Add((object) this.gridViewFields.Items[nItemIndex].Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) arrayList.ToArray(typeof (string)), true, string.Empty, false, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.gridViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]))
          {
            this.gridViewFields.Items.Add(new GVItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                (object) this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index])
              }
            });
            if (!this.reqFields.ContainsKey((object) ruleFindFieldDialog.SelectedRequiredFields[index]))
              this.reqFields.Add((object) ruleFindFieldDialog.SelectedRequiredFields[index], (object) ruleFindFieldDialog.SelectedRequiredFields[index]);
          }
        }
        this.gridViewFields.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnValidation_Click(object sender, EventArgs e)
    {
      if (!this.validateAdvancedCode())
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The Advanced Coding is valid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private bool validateAdvancedCode()
    {
      if (this.txtCoding.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Advanced Coding field cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtCoding.Focus();
        return false;
      }
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeFieldRule("", "", (RuleCondition) PredefinedCondition.Empty, this.txtCoding.Text, new string[0], FieldFormat.STRING).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        CompilerError error = ex.Errors[0];
        if (error.LineIndexOfRegion >= 0)
          Utils.HighlightLine(this.txtCoding, error.LineIndexOfRegion, false);
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: " + error.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.groupBox1 = new GroupBox();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageFields = new TabPageEx();
      this.gridViewFields = new GridView();
      this.label8 = new Label();
      this.findFieldBtn = new Button();
      this.addBtn = new Button();
      this.removeBtn = new Button();
      this.tabPageCodes = new TabPageEx();
      this.label1 = new Label();
      this.btnValidation = new Button();
      this.txtCoding = new TextBox();
      this.chkPrintBlank = new CheckBox();
      this.label2 = new Label();
      this.txtFormName = new TextBox();
      this.findBtn = new Button();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.groupBox1.SuspendLayout();
      this.tabControlEx1.SuspendLayout();
      this.tabPageFields.SuspendLayout();
      this.tabPageCodes.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.tabControlEx1);
      this.groupBox1.Controls.Add((Control) this.chkPrintBlank);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtFormName);
      this.groupBox1.Controls.Add((Control) this.findBtn);
      this.groupBox1.Location = new Point(12, 8);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(474, 390);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.tabControlEx1.Location = new Point(10, 45);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(453, 316);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 11;
      this.tabControlEx1.TabPages.Add(this.tabPageFields);
      this.tabControlEx1.TabPages.Add(this.tabPageCodes);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabPageFields.BackColor = Color.Transparent;
      this.tabPageFields.Controls.Add((Control) this.gridViewFields);
      this.tabPageFields.Controls.Add((Control) this.label8);
      this.tabPageFields.Controls.Add((Control) this.findFieldBtn);
      this.tabPageFields.Controls.Add((Control) this.addBtn);
      this.tabPageFields.Controls.Add((Control) this.removeBtn);
      this.tabPageFields.Location = new Point(1, 23);
      this.tabPageFields.Name = "tabPageFields";
      this.tabPageFields.TabIndex = 0;
      this.tabPageFields.TabWidth = 100;
      this.tabPageFields.Text = "Required Fields";
      this.tabPageFields.Value = (object) "Required Fields";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 113;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field Description";
      gvColumn2.Width = 207;
      this.gridViewFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewFields.Location = new Point(15, 22);
      this.gridViewFields.Name = "gridViewFields";
      this.gridViewFields.Size = new Size(348, 257);
      this.gridViewFields.TabIndex = 36;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 6);
      this.label8.Name = "label8";
      this.label8.Size = new Size(301, 13);
      this.label8.TabIndex = 35;
      this.label8.Text = "Add fields that must be completed prior to print the form above.";
      this.findFieldBtn.Location = new Point(370, 50);
      this.findFieldBtn.Name = "findFieldBtn";
      this.findFieldBtn.Size = new Size(75, 23);
      this.findFieldBtn.TabIndex = 3;
      this.findFieldBtn.Text = "Find";
      this.findFieldBtn.Click += new EventHandler(this.findFieldBtn_Click);
      this.addBtn.Location = new Point(370, 22);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 23);
      this.addBtn.TabIndex = 2;
      this.addBtn.Text = "Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.removeBtn.Location = new Point(370, 78);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(75, 23);
      this.removeBtn.TabIndex = 4;
      this.removeBtn.Text = "Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.tabPageCodes.BackColor = Color.Transparent;
      this.tabPageCodes.Controls.Add((Control) this.label1);
      this.tabPageCodes.Controls.Add((Control) this.btnValidation);
      this.tabPageCodes.Controls.Add((Control) this.txtCoding);
      this.tabPageCodes.Location = new Point(1, 23);
      this.tabPageCodes.Name = "tabPageCodes";
      this.tabPageCodes.TabIndex = 0;
      this.tabPageCodes.TabWidth = 100;
      this.tabPageCodes.Text = "Advanced Coding";
      this.tabPageCodes.Value = (object) "Advanced Coding";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(224, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Write your own code using Visual Basic .NET.";
      this.btnValidation.Location = new Point(366, 259);
      this.btnValidation.Name = "btnValidation";
      this.btnValidation.Size = new Size(75, 23);
      this.btnValidation.TabIndex = 7;
      this.btnValidation.Text = "Validation";
      this.btnValidation.UseVisualStyleBackColor = true;
      this.btnValidation.Click += new EventHandler(this.btnValidation_Click);
      this.txtCoding.Location = new Point(12, 27);
      this.txtCoding.Multiline = true;
      this.txtCoding.Name = "txtCoding";
      this.txtCoding.ScrollBars = ScrollBars.Both;
      this.txtCoding.Size = new Size(429, 226);
      this.txtCoding.TabIndex = 6;
      this.chkPrintBlank.AutoSize = true;
      this.chkPrintBlank.Location = new Point(10, 367);
      this.chkPrintBlank.Name = "chkPrintBlank";
      this.chkPrintBlank.Size = new Size(152, 17);
      this.chkPrintBlank.TabIndex = 8;
      this.chkPrintBlank.Text = "Blank forms can be printed";
      this.chkPrintBlank.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 21);
      this.label2.Name = "label2";
      this.label2.Size = new Size(88, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Print Form Name:";
      this.txtFormName.Location = new Point(102, 19);
      this.txtFormName.Name = "txtFormName";
      this.txtFormName.ReadOnly = true;
      this.txtFormName.Size = new Size(289, 20);
      this.txtFormName.TabIndex = 1;
      this.txtFormName.TabStop = false;
      this.findBtn.Location = new Point(397, 18);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(65, 23);
      this.findBtn.TabIndex = 0;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.okBtn.Location = new Point(330, 404);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(410, 404);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 10;
      this.cancelBtn.Text = "Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(496, 439);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditPrintFormDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Print Form Suppression Rule";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabControlEx1.ResumeLayout(false);
      this.tabPageFields.ResumeLayout(false);
      this.tabPageFields.PerformLayout();
      this.tabPageCodes.ResumeLayout(false);
      this.tabPageCodes.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
