// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
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
  public class PrintFormRuleDialog : Form
  {
    private const string className = "PrintFormRuleDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private ChannelConditionControl channelControl;
    private RuleConditionControl ruleCondForm;
    private PrintFormRuleInfo printRuleInfo;
    private Hashtable existingForms;
    private bool hasNoAccess;
    private IContainer components;
    private Label label6;
    private EMHelpLink emHelpLink1;
    private Panel panelCondition;
    private Label label3;
    private Label label2;
    private Label label1;
    private Button okBtn;
    private Button btnAdd;
    private Button btnRemove;
    private Panel panelChannel;
    private TextBox textBoxName;
    private Button cancelBtn;
    private Button btnEdit;
    private GridView gridViewForms;
    private Label label4;
    private TextBox commentsTxt;

    public PrintFormRuleDialog(Sessions.Session session, PrintFormRuleInfo printRuleInfo)
    {
      this.printRuleInfo = printRuleInfo;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.channelControl = new ChannelConditionControl();
      if (this.printRuleInfo != null)
        this.channelControl.ChannelValue = this.printRuleInfo.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.PrintForms);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_LoanFormPrinting);
      this.btnAdd.Enabled = this.btnEdit.Enabled = this.btnRemove.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    private void initForm()
    {
      this.existingForms = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.printRuleInfo == null)
        return;
      foreach (PrintRequiredFieldsInfo formRule in this.printRuleInfo.FormRules)
      {
        this.gridViewForms.Items.Add(this.buildViewItem(formRule, false));
        if (!this.existingForms.ContainsKey((object) formRule.FormID))
          this.existingForms.Add((object) formRule.FormID, (object) "");
      }
      this.ruleCondForm.SetCondition((BizRuleInfo) this.printRuleInfo);
      this.textBoxName.Text = this.printRuleInfo.RuleName;
      if (this.printRuleInfo.CommentsTxt.Contains("\n") && !this.printRuleInfo.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.printRuleInfo.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.printRuleInfo.CommentsTxt;
    }

    public PrintFormRuleInfo PrintRuleInfo => this.printRuleInfo;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.PrintForms)).GetRules(this.ruleCondForm.IsGeneralRule);
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), rules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.printRuleInfo == null)
              flag = true;
            else if (this.printRuleInfo.RuleID != rules[index].RuleID || this.printRuleInfo.RuleID == 0)
              flag = true;
            if (flag)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The rule name that you entered is already in use. Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.textBoxName.Focus();
              return;
            }
          }
        }
        if (!this.ruleCondForm.ValidateCondition())
          return;
        if (this.gridViewForms.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a print form rule.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          PrintRequiredFieldsInfo[] requiredFieldsInfoArray = new PrintRequiredFieldsInfo[this.gridViewForms.Items.Count];
          for (int nItemIndex = 0; nItemIndex < this.gridViewForms.Items.Count; ++nItemIndex)
            requiredFieldsInfoArray[nItemIndex] = (PrintRequiredFieldsInfo) this.gridViewForms.Items[nItemIndex].Tag;
          this.printRuleInfo = this.printRuleInfo == null ? new PrintFormRuleInfo(this.textBoxName.Text.Trim()) : new PrintFormRuleInfo(this.printRuleInfo.RuleID, this.textBoxName.Text.Trim());
          this.printRuleInfo.FormRules = requiredFieldsInfoArray;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.printRuleInfo);
          this.printRuleInfo.Condition2 = this.channelControl.ChannelValue;
          this.printRuleInfo.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (EditPrintFormDialog editPrintFormDialog = new EditPrintFormDialog(this.session, (PrintRequiredFieldsInfo) null, this.existingForms))
      {
        if (editPrintFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (!this.existingForms.ContainsKey((object) editPrintFormDialog.RequiredFieldInfo.FormID))
        {
          this.gridViewForms.Items.Add(this.buildViewItem(editPrintFormDialog.RequiredFieldInfo, true));
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The form list already contains form '" + editPrintFormDialog.RequiredFieldInfo.FormID + "'. This form will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        if (this.existingForms.ContainsKey((object) editPrintFormDialog.RequiredFieldInfo.FormID))
          return;
        this.existingForms.Add((object) editPrintFormDialog.RequiredFieldInfo.FormID, (object) "");
      }
    }

    private GVItem buildViewItem(PrintRequiredFieldsInfo requiredFieldInfo, bool selected)
    {
      return new GVItem(this.buildUIFormName(requiredFieldInfo.FormID, requiredFieldInfo.FormType))
      {
        SubItems = {
          (object) this.buildFieldIDs(requiredFieldInfo.FieldIDs, requiredFieldInfo.AdvancedCoding)
        },
        Tag = (object) requiredFieldInfo,
        Selected = selected
      };
    }

    private string buildUIFormName(string formItem, OutputFormType formType)
    {
      string formKey = formItem;
      int num = formKey.LastIndexOf("\\");
      if (num > -1)
        formKey = formItem.Substring(num + 1);
      if (formType == OutputFormType.CustomLetters)
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

    private string buildFieldIDs(string[] ids, string advancedCoding)
    {
      string empty = string.Empty;
      if (ids != null)
      {
        for (int index = 0; index < ids.Length; ++index)
        {
          if (!(ids[index] == PrintRequiredFieldsInfo.PRINTBLANKID))
          {
            if (empty != string.Empty)
              empty += ", ";
            empty += ids[index];
            if (index >= 2)
              break;
          }
        }
      }
      if (advancedCoding != string.Empty)
      {
        if (empty != string.Empty)
          empty += ", ";
        empty += "Advanced Coding...";
      }
      if (ids != null && ids.Length > 3 && advancedCoding == string.Empty)
        empty += "...";
      return empty;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gridViewForms.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a print form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        PrintRequiredFieldsInfo tag = (PrintRequiredFieldsInfo) this.gridViewForms.SelectedItems[0].Tag;
        int index = this.gridViewForms.SelectedItems[0].Index;
        this.gridViewForms.Items.RemoveAt(index);
        if (this.existingForms.ContainsKey((object) tag.FormID))
          this.existingForms.Remove((object) tag.FormID);
        if (this.gridViewForms.Items.Count == 0)
          return;
        if (index > this.gridViewForms.Items.Count - 1)
          this.gridViewForms.Items[this.gridViewForms.Items.Count - 1].Selected = true;
        else
          this.gridViewForms.Items[index].Selected = true;
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      if (this.gridViewForms.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a print form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        PrintRequiredFieldsInfo tag = (PrintRequiredFieldsInfo) this.gridViewForms.SelectedItems[0].Tag;
        using (EditPrintFormDialog editPrintFormDialog = new EditPrintFormDialog(this.session, tag, this.existingForms))
        {
          if (editPrintFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          if (this.existingForms.ContainsKey((object) tag.FormID))
            this.existingForms.Remove((object) tag.FormID);
          this.gridViewForms.SelectedItems[0].Text = this.buildUIFormName(editPrintFormDialog.RequiredFieldInfo.FormID, editPrintFormDialog.RequiredFieldInfo.FormType);
          this.gridViewForms.SelectedItems[0].SubItems[1].Text = this.buildFieldIDs(editPrintFormDialog.RequiredFieldInfo.FieldIDs, editPrintFormDialog.RequiredFieldInfo.AdvancedCoding);
          this.gridViewForms.SelectedItems[0].Tag = (object) editPrintFormDialog.RequiredFieldInfo;
          if (this.existingForms.ContainsKey((object) editPrintFormDialog.RequiredFieldInfo.FormID))
            return;
          this.existingForms.Add((object) editPrintFormDialog.RequiredFieldInfo.FormID, (object) "");
        }
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
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
      this.label6 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.panelCondition = new Panel();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.okBtn = new Button();
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.panelChannel = new Panel();
      this.textBoxName = new TextBox();
      this.cancelBtn = new Button();
      this.btnEdit = new Button();
      this.gridViewForms = new GridView();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.SuspendLayout();
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(17, 66);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Loan Form Printing";
      this.emHelpLink1.Location = new Point(17, 586);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 14;
      this.panelCondition.Location = new Point(33, 215);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(719, 92);
      this.panelCondition.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(17, 322);
      this.label3.Name = "label3";
      this.label3.Size = new Size(234, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Add and apply print suppression rules";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(17, 185);
      this.label2.Name = "label2";
      this.label2.Size = new Size(200, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this rule";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(17, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(1016, 583);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 15;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.Location = new Point(677, 351);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 8;
      this.btnAdd.Text = "&Add";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.Location = new Point(677, 407);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 10;
      this.btnRemove.Text = "&Remove";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.panelChannel.Location = new Point(32, 92);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(719, 79);
      this.panelChannel.TabIndex = 3;
      this.textBoxName.Location = new Point(32, 32);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(720, 20);
      this.textBoxName.TabIndex = 1;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(1096, 583);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 16;
      this.cancelBtn.Text = "Cancel";
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.Location = new Point(677, 379);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(75, 23);
      this.btnEdit.TabIndex = 9;
      this.btnEdit.Text = "&Edit";
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Form Name";
      gvColumn1.Width = 285;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Pre-Required Fields";
      gvColumn2.Width = 347;
      this.gridViewForms.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewForms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewForms.Location = new Point(32, 351);
      this.gridViewForms.Name = "gridViewForms";
      this.gridViewForms.Size = new Size(634, 219);
      this.gridViewForms.TabIndex = 7;
      this.gridViewForms.DoubleClick += new EventHandler(this.btnEdit_Click);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(780, 11);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(783, 32);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(364, 139);
      this.commentsTxt.TabIndex = 13;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(1191, 618);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.gridViewForms);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.panelChannel);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.btnEdit);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (PrintFormRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Print Suppression Business Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
