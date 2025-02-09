// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FieldRuleDialog : Form
  {
    private const string className = "FieldRuleDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private RuleConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private Button removeFieldBtn;
    private Button addFieldBtn;
    private Button editBtn;
    private System.ComponentModel.Container components;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel panelCondition;
    private ArrayList existingFields;
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private FieldSettings fieldSettings;
    private GridView gridViewFields;
    private ChannelConditionControl channelControl;
    private Panel panelSection4;
    private Panel panelDialog;
    private TextBox commentsTxt;
    private Label label4;
    private bool hasNoAccess;
    private string[] preConfiguredRuleNames = new string[1]
    {
      "Manner in Which Title will be Held"
    };
    private FieldRuleInfo fieldRule;

    public FieldRuleDialog(Sessions.Session session, FieldRuleInfo fieldRule)
    {
      this.fieldRule = fieldRule;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.channelControl = new ChannelConditionControl();
      if (this.fieldRule != null)
        this.channelControl.ChannelValue = this.fieldRule.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.FieldRules);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_FieldDataEntry);
      this.addFieldBtn.Enabled = this.removeFieldBtn.Enabled = this.editBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (this.hasNoAccess)
      {
        this.textBoxName.Enabled = false;
        this.ruleCondForm.DisableControls();
        this.channelControl.DisableControls();
      }
      if (this.FieldRule == null || !((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(this.FieldRule.RuleName))
        return;
      this.addFieldBtn.Enabled = false;
      this.removeFieldBtn.Enabled = false;
      this.textBoxName.ReadOnly = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public FieldRuleInfo FieldRule => this.fieldRule;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.editBtn = new Button();
      this.addFieldBtn = new Button();
      this.removeFieldBtn = new Button();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.gridViewFields = new GridView();
      this.panelSection4 = new Panel();
      this.panelDialog = new Panel();
      this.commentsTxt = new TextBox();
      this.label4 = new Label();
      this.panelSection4.SuspendLayout();
      this.panelDialog.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(890, 575);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(971, 575);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(677, 20);
      this.textBoxName.TabIndex = 1;
      this.editBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.editBtn.Location = new Point(613, 53);
      this.editBtn.Name = "editBtn";
      this.editBtn.Size = new Size(75, 23);
      this.editBtn.TabIndex = 9;
      this.editBtn.Text = "&Edit";
      this.editBtn.Click += new EventHandler(this.editBtn_Click);
      this.addFieldBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addFieldBtn.Location = new Point(613, 25);
      this.addFieldBtn.Name = "addFieldBtn";
      this.addFieldBtn.Size = new Size(75, 23);
      this.addFieldBtn.TabIndex = 8;
      this.addFieldBtn.Text = "&Add";
      this.addFieldBtn.Click += new EventHandler(this.addFieldBtn_Click);
      this.removeFieldBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removeFieldBtn.Location = new Point(613, 81);
      this.removeFieldBtn.Name = "removeFieldBtn";
      this.removeFieldBtn.Size = new Size(75, 23);
      this.removeFieldBtn.TabIndex = 10;
      this.removeFieldBtn.Text = "&Remove";
      this.removeFieldBtn.Click += new EventHandler(this.removeFieldBtn_Click);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(-3, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(162, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Add and apply field rules";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 171);
      this.label2.Name = "label2";
      this.label2.Size = new Size(196, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3.Is there a condition for this rule";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.panelCondition.Location = new Point(32, 196);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(677, 94);
      this.panelCondition.TabIndex = 5;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Field Data Entry";
      this.emHelpLink1.Location = new Point(19, 582);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(235, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2.Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(32, 78);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(677, 79);
      this.panelChannel.TabIndex = 3;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "ID";
      gvColumn1.Width = 67;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 124;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Rule Type";
      gvColumn3.Width = 105;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Details";
      gvColumn4.Width = 166;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Pre-Required Fields";
      gvColumn5.Width = 131;
      this.gridViewFields.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gridViewFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewFields.Location = new Point(13, 25);
      this.gridViewFields.Name = "gridViewFields";
      this.gridViewFields.Size = new Size(595, 207);
      this.gridViewFields.TabIndex = 7;
      this.gridViewFields.DoubleClick += new EventHandler(this.gridViewFields_DoubleClick);
      this.panelSection4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panelSection4.Controls.Add((Control) this.gridViewFields);
      this.panelSection4.Controls.Add((Control) this.label3);
      this.panelSection4.Controls.Add((Control) this.addFieldBtn);
      this.panelSection4.Controls.Add((Control) this.editBtn);
      this.panelSection4.Controls.Add((Control) this.removeFieldBtn);
      this.panelSection4.Location = new Point(19, 341);
      this.panelSection4.Name = "panelSection4";
      this.panelSection4.Size = new Size(690, 235);
      this.panelSection4.TabIndex = 32;
      this.panelDialog.AutoScroll = true;
      this.panelDialog.Controls.Add((Control) this.commentsTxt);
      this.panelDialog.Controls.Add((Control) this.label4);
      this.panelDialog.Controls.Add((Control) this.cancelBtn);
      this.panelDialog.Controls.Add((Control) this.okBtn);
      this.panelDialog.Controls.Add((Control) this.panelSection4);
      this.panelDialog.Controls.Add((Control) this.textBoxName);
      this.panelDialog.Controls.Add((Control) this.panelChannel);
      this.panelDialog.Controls.Add((Control) this.label6);
      this.panelDialog.Controls.Add((Control) this.panelCondition);
      this.panelDialog.Controls.Add((Control) this.emHelpLink1);
      this.panelDialog.Controls.Add((Control) this.label1);
      this.panelDialog.Controls.Add((Control) this.label2);
      this.panelDialog.Location = new Point(0, 0);
      this.panelDialog.Name = "panelDialog";
      this.panelDialog.Size = new Size(1105, 610);
      this.panelDialog.TabIndex = 33;
      this.commentsTxt.Location = new Point(730, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(334, (int) sbyte.MaxValue);
      this.commentsTxt.TabIndex = 12;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(727, 14);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Notes/Comments";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(1105, 611);
      this.Controls.Add((Control) this.panelDialog);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Field Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panelSection4.ResumeLayout(false);
      this.panelSection4.PerformLayout();
      this.panelDialog.ResumeLayout(false);
      this.panelDialog.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.existingFields = new ArrayList();
      this.gridViewFields.Items.Clear();
      if (this.fieldRule == null)
        return;
      foreach (DictionaryEntry fieldRule in this.fieldRule.FieldRules)
      {
        string str = fieldRule.Key.ToString();
        string col2 = this.getFieldDescription(str);
        if (col2 == string.Empty)
          col2 = "Invalid";
        object ruleObject = fieldRule.Value;
        string[] requiredField = (string[]) this.fieldRule.RequiredFields[(object) str];
        ArrayList arrayList = new ArrayList();
        arrayList.Add((object) requiredField);
        arrayList.Add(ruleObject);
        GVItem gvItem = this.createGVItem(str, col2, requiredField, ruleObject);
        gvItem.Tag = (object) arrayList;
        this.gridViewFields.Items.Add(gvItem);
        this.existingFields.Add((object) str);
      }
      this.ruleCondForm.SetCondition((BizRuleInfo) this.fieldRule);
      this.textBoxName.Text = this.fieldRule.RuleName;
      if (this.fieldRule.CommentsTxt.Contains("\n") && !this.fieldRule.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.fieldRule.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.fieldRule.CommentsTxt;
    }

    private void addFieldBtn_Click(object sender, EventArgs e)
    {
      using (EditFieldDialog editFieldDialog = new EditFieldDialog(this.session, "", "", (string[]) null, (object) null, this.existingFields, this.ruleCondForm.IsGeneralRule, this.textBoxName.Text))
      {
        if (editFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        string[] requiredFields = editFieldDialog.RequiredFields;
        object ruleObject = editFieldDialog.RuleObject;
        ArrayList arrayList = new ArrayList();
        arrayList.Add((object) requiredFields);
        arrayList.Add(ruleObject);
        GVItem gvItem = this.createGVItem(editFieldDialog.FieldID, editFieldDialog.FieldDescription, requiredFields, ruleObject);
        gvItem.Tag = (object) arrayList;
        gvItem.Selected = true;
        this.gridViewFields.Items.Add(gvItem);
        this.existingFields.Add((object) editFieldDialog.FieldID);
      }
    }

    private GVItem createGVItem(
      string col1,
      string col2,
      string[] requiredFields,
      object ruleObject)
    {
      GVItem gvItem = new GVItem(col1);
      gvItem.SubItems.Add((object) col2);
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if (requiredFields != null && requiredFields.Length != 0)
      {
        for (int index = 0; index < requiredFields.Length; ++index)
        {
          if (!(requiredFields[index] == string.Empty))
            str3 = !(str3 == string.Empty) ? str3 + ", " + requiredFields[index] : requiredFields[index];
        }
      }
      if (str1 == string.Empty)
      {
        switch (ruleObject)
        {
          case FRRange _:
            FRRange frRange = (FRRange) ruleObject;
            str1 = "Range";
            str2 = !(frRange.LowerBound == string.Empty) || !(frRange.UpperBound == string.Empty) ? (!(frRange.LowerBound != string.Empty) || !(frRange.UpperBound == string.Empty) ? (!(frRange.LowerBound == string.Empty) || !(frRange.UpperBound != string.Empty) ? "Min " + frRange.LowerBound + ", Max " + frRange.UpperBound : "Max " + frRange.UpperBound) : "Min " + frRange.LowerBound) : string.Empty;
            break;
          case FRList _:
            FRList frList = (FRList) ruleObject;
            str1 = !frList.IsLock ? "Dropdown List (Editable)" : "Dropdown List";
            for (int index = 0; index < frList.List.Length; ++index)
            {
              if (!(frList.List[index] == string.Empty))
                str2 = !(str2 == string.Empty) ? str2 + ", " + frList.List[index] : frList.List[index];
            }
            break;
          default:
            str1 = "Advanced Coding";
            str2 = ruleObject.ToString();
            break;
        }
      }
      if (str1 == string.Empty && str3 != string.Empty)
        str1 = "Pre-Required Fields";
      gvItem.SubItems.Add((object) str1);
      gvItem.SubItems.Add((object) str2);
      gvItem.SubItems.Add((object) str3);
      return gvItem;
    }

    private void removeFieldBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.existingFields.Remove((object) this.gridViewFields.SelectedItems[0].Text);
        int index = this.gridViewFields.SelectedItems[0].Index;
        this.gridViewFields.Items.Remove(this.gridViewFields.SelectedItems[0]);
        if (this.gridViewFields.Items.Count == 0)
          return;
        if (index > this.gridViewFields.Items.Count - 1)
          this.gridViewFields.Items[this.gridViewFields.Items.Count - 1].Selected = true;
        else
          this.gridViewFields.Items[index].Selected = true;
      }
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewFields.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string text1 = this.gridViewFields.SelectedItems[0].Text;
        string text2 = this.gridViewFields.SelectedItems[0].SubItems[1].Text;
        ArrayList arrayList1 = (ArrayList) null;
        try
        {
          arrayList1 = (ArrayList) this.gridViewFields.SelectedItems[0].Tag;
        }
        catch (Exception ex)
        {
          Tracing.Log(FieldRuleDialog.sw, TraceLevel.Error, nameof (FieldRuleDialog), "editBtn_Click: can't cast GVItem tag to ArrayList. Error: " + ex.Message);
        }
        string[] requiredFields1 = (string[]) null;
        object ruleObject1 = (object) null;
        if (arrayList1 != null)
        {
          if (arrayList1.Count > 0)
            requiredFields1 = (string[]) arrayList1[0];
          if (arrayList1.Count > 1)
            ruleObject1 = arrayList1[1];
        }
        using (EditFieldDialog editFieldDialog = new EditFieldDialog(this.session, text1, text2, requiredFields1, ruleObject1, this.existingFields, this.ruleCondForm.IsGeneralRule, this.FieldRule.RuleName))
        {
          int num2 = (int) editFieldDialog.ShowDialog((IWin32Window) this);
          if (editFieldDialog.DialogResult == DialogResult.OK)
          {
            this.existingFields.Remove((object) text1);
            string[] requiredFields2 = editFieldDialog.RequiredFields;
            object ruleObject2 = editFieldDialog.RuleObject;
            ArrayList arrayList2 = new ArrayList();
            arrayList2.Add((object) requiredFields2);
            arrayList2.Add(ruleObject2);
            GVItem gvItem = this.createGVItem(editFieldDialog.FieldID, editFieldDialog.FieldDescription, requiredFields2, ruleObject2);
            gvItem.Tag = (object) arrayList2;
            gvItem.Selected = true;
            this.gridViewFields.Items.Remove(this.gridViewFields.SelectedItems[0]);
            this.gridViewFields.Items.Add(gvItem);
            this.existingFields.Add((object) editFieldDialog.FieldID);
          }
          editFieldDialog.Dispose();
        }
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules)).GetRules(this.ruleCondForm.IsGeneralRule);
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), rules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.fieldRule == null)
              flag = true;
            else if (this.fieldRule.RuleID != rules[index].RuleID || this.fieldRule.RuleID == 0)
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
        if (!this.ruleCondForm.IsGeneralRule)
        {
          string str1 = "";
          for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
          {
            string str2 = this.gridViewFields.Items[nItemIndex].Text.Trim();
            if (((ArrayList) this.gridViewFields.Items[nItemIndex].Tag)[1] is string)
              str1 = !(str1 == "") ? str1 + ", " + str2 : str2;
          }
          if (str1 != string.Empty)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Advanced Coding is only available to general condition. Please remove\r\n the following fields or change them to different rule type:\r\n\r\n" + str1, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (this.gridViewFields.Items.Count == 0)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Please add a field rule.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          Hashtable hashtable1 = new Hashtable();
          Hashtable hashtable2 = new Hashtable();
          for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
          {
            string key = this.gridViewFields.Items[nItemIndex].Text.Trim();
            ArrayList tag = (ArrayList) this.gridViewFields.Items[nItemIndex].Tag;
            if (!hashtable1.ContainsKey((object) key))
              hashtable1.Add((object) key, tag[0]);
            if (!hashtable2.ContainsKey((object) key))
              hashtable2.Add((object) key, tag[1]);
          }
          this.fieldRule = this.fieldRule == null ? new FieldRuleInfo(this.textBoxName.Text.Trim()) : new FieldRuleInfo(this.fieldRule.RuleID, this.textBoxName.Text.Trim());
          this.fieldRule.RequiredFields = hashtable1;
          this.fieldRule.FieldRules = hashtable2;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.fieldRule);
          this.fieldRule.Condition2 = this.channelControl.ChannelValue;
          this.fieldRule.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F1)
      {
        this.ShowHelp();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.cancelBtn.PerformClick();
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (FieldRuleDialog));
    }

    private void gridViewFields_DoubleClick(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.editBtn_Click((object) null, (EventArgs) null);
    }
  }
}
