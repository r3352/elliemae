// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerEditor : Form
  {
    private const string className = "TriggerEditor";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private RuleConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private Button removeFieldBtn;
    private Button addFieldBtn;
    private ColumnHeader headerCondition;
    private ListView listViewItems;
    private Button editBtn;
    private System.ComponentModel.Container components;
    private ListViewSortManager sortMngr;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel panelCondition;
    private EMHelpLink emHelpLink1;
    private ColumnHeader headerType;
    private ColumnHeader headerAction;
    private FieldSettings fieldSettings;
    private Label label6;
    private Panel panelChannel;
    private TriggerInfo trigger;
    private ChannelConditionControl channelControl;
    private ColumnHeader headerCondItem;
    private Sessions.Session session;
    private Label label4;
    private TextBox commentsTxt;
    private bool hasNoAccess;

    public TriggerEditor(TriggerInfo trigger, Sessions.Session session)
    {
      this.trigger = trigger;
      this.session = session;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.sortMngr = new ListViewSortManager(this.listViewItems, new System.Type[4]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMngr.Sort(0);
      this.channelControl = new ChannelConditionControl();
      if (this.trigger != null)
        this.channelControl.ChannelValue = this.trigger.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.Triggers);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_FieldTriggers);
      this.addFieldBtn.Enabled = this.editBtn.Enabled = this.removeFieldBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.editBtn = new Button();
      this.listViewItems = new ListView();
      this.headerType = new ColumnHeader();
      this.headerCondition = new ColumnHeader();
      this.headerCondItem = new ColumnHeader();
      this.headerAction = new ColumnHeader();
      this.addFieldBtn = new Button();
      this.removeFieldBtn = new Button();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(837, 582);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(917, 582);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Location = new Point(31, 33);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(549, 20);
      this.textBoxName.TabIndex = 1;
      this.editBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.editBtn.Location = new Point(671, 374);
      this.editBtn.Name = "editBtn";
      this.editBtn.Size = new Size(75, 23);
      this.editBtn.TabIndex = 9;
      this.editBtn.Text = "&Edit";
      this.editBtn.Click += new EventHandler(this.editBtn_Click);
      this.listViewItems.Columns.AddRange(new ColumnHeader[4]
      {
        this.headerType,
        this.headerCondition,
        this.headerCondItem,
        this.headerAction
      });
      this.listViewItems.FullRowSelect = true;
      this.listViewItems.GridLines = true;
      this.listViewItems.HideSelection = false;
      this.listViewItems.Location = new Point(31, 344);
      this.listViewItems.MultiSelect = false;
      this.listViewItems.Name = "listViewItems";
      this.listViewItems.Size = new Size(634, 219);
      this.listViewItems.TabIndex = 7;
      this.listViewItems.UseCompatibleStateImageBehavior = false;
      this.listViewItems.View = View.Details;
      this.listViewItems.DoubleClick += new EventHandler(this.editBtn_Click);
      this.headerType.Text = "Type";
      this.headerType.Width = 79;
      this.headerCondition.Text = "Activation";
      this.headerCondition.Width = 161;
      this.headerCondItem.Text = "Activation Source";
      this.headerCondItem.Width = 160;
      this.headerAction.Text = "Action";
      this.headerAction.Width = 200;
      this.addFieldBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addFieldBtn.Location = new Point(671, 346);
      this.addFieldBtn.Name = "addFieldBtn";
      this.addFieldBtn.Size = new Size(75, 23);
      this.addFieldBtn.TabIndex = 8;
      this.addFieldBtn.Text = "&Add";
      this.addFieldBtn.Click += new EventHandler(this.addFieldBtn_Click);
      this.removeFieldBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removeFieldBtn.Location = new Point(671, 402);
      this.removeFieldBtn.Name = "removeFieldBtn";
      this.removeFieldBtn.Size = new Size(75, 23);
      this.removeFieldBtn.TabIndex = 10;
      this.removeFieldBtn.Text = "&Remove";
      this.removeFieldBtn.Click += new EventHandler(this.removeFieldBtn_Click);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(16, 318);
      this.label3.Name = "label3";
      this.label3.Size = new Size(173, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Add and apply field events";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 185);
      this.label2.Name = "label2";
      this.label2.Size = new Size(215, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this trigger";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(150, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Trigger Name";
      this.panelCondition.Location = new Point(32, 210);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(548, 92);
      this.panelCondition.TabIndex = 5;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(13, 66);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(31, 91);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(548, 79);
      this.panelChannel.TabIndex = 3;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Field Triggers";
      this.emHelpLink1.Location = new Point(16, 585);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(625, 12);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(628, 33);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(364, 137);
      this.commentsTxt.TabIndex = 12;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1015, 618);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.panelChannel);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.addFieldBtn);
      this.Controls.Add((Control) this.removeFieldBtn);
      this.Controls.Add((Control) this.editBtn);
      this.Controls.Add((Control) this.listViewItems);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (TriggerEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add/Edit Trigger";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public TriggerInfo Trigger => this.trigger;

    private void initForm()
    {
      this.listViewItems.Items.Clear();
      if (this.trigger == null)
        return;
      foreach (TriggerEvent e in this.trigger.Events)
        this.listViewItems.Items.Add(this.createListViewItem(e));
      this.ruleCondForm.SetCondition((BizRuleInfo) this.trigger);
      this.textBoxName.Text = this.trigger.RuleName;
      if (this.trigger.CommentsTxt.Contains("\n") && !this.trigger.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.trigger.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.trigger.CommentsTxt;
    }

    private ListViewItem createListViewItem(TriggerEvent e)
    {
      ListViewItem listViewItem = new ListViewItem("");
      listViewItem.SubItems.Add("");
      listViewItem.SubItems.Add("");
      listViewItem.SubItems.Add("");
      listViewItem.Tag = (object) e;
      this.refreshListViewItem(listViewItem);
      return listViewItem;
    }

    private void refreshListViewItem(ListViewItem item)
    {
      TriggerEvent tag = (TriggerEvent) item.Tag;
      TriggerCondition condition = tag.Conditions[0];
      item.SubItems[0].Text = this.getConditionActivationType(condition);
      item.SubItems[1].Text = this.getConditionDescription(condition);
      item.SubItems[2].Text = this.getConditionActivationDescription(condition);
      item.SubItems[3].Text = this.getActionDescription(tag.Action);
    }

    private string getConditionActivationType(TriggerCondition cond)
    {
      switch (cond)
      {
        case TriggerFieldCondition _:
          return "Field";
        case TriggerMilestoneCompletionCondition _:
          return "Milestone";
        case TriggerRateLockCondition _:
          return "Rate Lock";
        default:
          return "TPO Action";
      }
    }

    private string getConditionDescription(TriggerCondition cond)
    {
      return cond is TriggerFieldCondition ? new TriggerEventConditionNameProvider().GetName((object) cond.ConditionType) : new TriggerActivationNameProvider().GetNameForConditionType(cond.ConditionType);
    }

    private string getConditionActivationDescription(TriggerCondition cond)
    {
      switch (cond)
      {
        case TriggerFieldCondition _:
          return this.getFieldDescription(((TriggerFieldCondition) cond).FieldID);
        case TriggerMilestoneCompletionCondition _:
          return this.getMilestoneDescription(((TriggerMilestoneCompletionCondition) cond).MilestoneID);
        default:
          return "";
      }
    }

    private string getMilestoneDescription(string milestoneId)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(milestoneId);
      return milestoneById == null ? "Unknown Milestone" : milestoneById.Name;
    }

    private string getActionDescription(TriggerAction action)
    {
      switch (action)
      {
        case TriggerCopyAction _:
          return this.getCopyActionDescription((TriggerCopyAction) action);
        case TriggerAssignmentAction _:
          return this.getAssignmentActionDescription((TriggerAssignmentAction) action);
        case TriggerCompleteTasksAction _:
          return this.getCompleteTasksActionDescription((TriggerCompleteTasksAction) action);
        case TriggerEmailAction _:
          return this.getEmailActionDescription((TriggerEmailAction) action);
        case TriggerAdvancedCodeAction _:
          return "Execute advanced code";
        case TriggerMoveLoanFolderAction _:
          return this.getLoanFolderDescription((TriggerMoveLoanFolderAction) action);
        case TriggerApplyLoanTemplateAction _:
          return this.getLoanTemplateDescription((TriggerApplyLoanTemplateAction) action);
        case TriggerSpecialFeatureCodesAction _:
          return this.getSpecialFeatureCodesDescription((TriggerSpecialFeatureCodesAction) action);
        default:
          return "Unknown";
      }
    }

    private string getLoanTemplateDescription(TriggerApplyLoanTemplateAction action)
    {
      return "Loan Template: " + action.LoanTemplateName;
    }

    private string getSpecialFeatureCodesDescription(TriggerSpecialFeatureCodesAction action)
    {
      string empty = string.Empty;
      string str = action.SpecialFeatureCodes.First<KeyValuePair<string, string>>().Value;
      foreach (KeyValuePair<string, string> keyValuePair in action.SpecialFeatureCodes.Skip<KeyValuePair<string, string>>(1))
        str = str + " | " + keyValuePair.Value;
      return "Add Special Feature Codes: " + str;
    }

    private string getLoanFolderDescription(TriggerMoveLoanFolderAction action)
    {
      return "Move to: " + action.LoanFolderName;
    }

    private string getEmailActionDescription(TriggerEmailAction action)
    {
      return "Send: " + (object) action.Templates.Count + " email(s)";
    }

    private string getCopyActionDescription(TriggerCopyAction action)
    {
      return "Copy to: " + this.getListDescription(action.TargetFieldIDs);
    }

    private string getCompleteTasksActionDescription(TriggerCompleteTasksAction action)
    {
      return "Complete tasks: " + this.getListDescription(action.TaskNames);
    }

    private string getAssignmentActionDescription(TriggerAssignmentAction action)
    {
      string[] items = new string[action.Assignments.Length];
      for (int index = 0; index < action.Assignments.Length; ++index)
        items[index] = action.Assignments[index].FieldID;
      return "Assign to: " + this.getListDescription(items);
    }

    private string getFieldConditionDescription(TriggerFieldCondition cond)
    {
      FieldDefinition field = EncompassFields.GetField(cond.FieldID);
      switch (cond)
      {
        case TriggerValueChangeCondition _:
          return "Any change";
        case TriggerFixedValueCondition _:
          return field != null && field.Options.RequireValueFromList ? "Value set to " + field.Options.ValueToText(((TriggerFixedValueCondition) cond).Value) : "Value set to " + ((TriggerFixedValueCondition) cond).Value;
        case TriggerRangeCondition _:
          return this.getRangeConditionDescription((TriggerRangeCondition) cond);
        case TriggerValueListCondition _:
          return this.getValueListConditionDescription(field, (TriggerValueListCondition) cond);
        default:
          return "Unknown";
      }
    }

    private string getValueListConditionDescription(
      FieldDefinition fieldDef,
      TriggerValueListCondition cond)
    {
      if (cond.Values.Length == 1)
        return "Value is " + this.fieldValueToText(fieldDef, cond.Values[0]);
      return fieldDef != null && fieldDef.Options.RequireValueFromList ? "Value is any of: " + this.getFieldOptionListDescription(fieldDef, cond.Values) : "Value is any of: " + this.getListDescription(cond.Values);
    }

    private string getRangeConditionDescription(TriggerRangeCondition cond)
    {
      if (cond.Minimum != "" && cond.Maximum != "")
        return "Value between " + cond.Minimum + " and " + cond.Maximum;
      return cond.Minimum != "" ? "Value >= " + cond.Minimum : "Value <= " + cond.Maximum;
    }

    private string fieldValueToText(FieldDefinition fieldDef, string value)
    {
      string text = fieldDef == null || !fieldDef.Options.RequireValueFromList ? value : fieldDef.Options.ValueToText(value);
      if (text == "")
        text = "<Empty>";
      return text;
    }

    private string getFieldOptionListDescription(FieldDefinition fieldDef, string[] items)
    {
      List<string> stringList = new List<string>();
      foreach (string str in items)
        stringList.Add(fieldDef.Options.ValueToText(str));
      return this.getListDescription(stringList.ToArray());
    }

    private string getListDescription(string[] items)
    {
      string[] strArray = (string[]) items.Clone();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] == "")
          strArray[index] = "<Empty>";
      }
      if (strArray.Length < 4)
        return string.Join(", ", strArray);
      return string.Join(", ", strArray, 0, 3) + ", ... (" + (object) (strArray.Length - 3) + " more)";
    }

    private void addFieldBtn_Click(object sender, EventArgs e)
    {
      using (TriggerEventEditor triggerEventEditor = new TriggerEventEditor(this.fieldSettings, (TriggerEvent) null, this.session))
      {
        if (triggerEventEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.listViewItems.Items.Add(this.createListViewItem(triggerEventEditor.TriggerEvent));
      }
    }

    private void removeFieldBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewItems.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Select an event to be removed from the list", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.listViewItems.SelectedItems[0].Remove();
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      if (this.listViewItems.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the event to edit from the list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (TriggerEventEditor triggerEventEditor = new TriggerEventEditor(this.fieldSettings, (TriggerEvent) this.listViewItems.SelectedItems[0].Tag, this.session))
        {
          if (triggerEventEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.listViewItems.SelectedItems[0].Remove();
          ListViewItem listViewItem = this.createListViewItem(triggerEventEditor.TriggerEvent);
          this.listViewItems.Items.Add(listViewItem);
          listViewItem.Selected = true;
        }
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Triggers)).GetRules(this.ruleCondForm.IsGeneralRule, true);
        string str = this.textBoxName.Text.Trim();
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(str, rules[index].RuleName, true) == 0)
          {
            bool flag = false;
            if (this.trigger == null)
              flag = true;
            else if (this.trigger.RuleID != rules[index].RuleID || this.trigger.RuleID == 0)
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
        if (this.listViewItems.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "You must add one or more events to save this trigger.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          TriggerEvent[] collection = new TriggerEvent[this.listViewItems.Items.Count];
          for (int index = 0; index < this.listViewItems.Items.Count; ++index)
            collection[index] = (TriggerEvent) this.listViewItems.Items[index].Tag;
          this.trigger = this.trigger == null ? new TriggerInfo(str) : new TriggerInfo(this.trigger.RuleID, str);
          this.trigger.Events.AddRange((IEnumerable<TriggerEvent>) collection);
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.trigger);
          this.trigger.Condition2 = this.channelControl.ChannelValue;
          this.trigger.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private string getFieldDescription(string fieldID)
    {
      string str = EncompassFields.GetDescription(fieldID, this.fieldSettings);
      if (str == string.Empty)
        str = "Unknown field";
      return fieldID + " (" + str + ")";
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.emHelpLink1.HelpTag);
    }
  }
}
