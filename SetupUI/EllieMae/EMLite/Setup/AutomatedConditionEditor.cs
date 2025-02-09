// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutomatedConditionEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutomatedConditionEditor : Form
  {
    private const string className = "AutomatedConditionEditor";
    private AutomatedConditionRuleInfo ruleInfo;
    private Sessions.Session session;
    private ChannelConditionControl channelControl;
    private RuleConditionControl ruleCondForm;
    private bool hasNoAccess;
    private IContainer components;
    private Panel panelChannel;
    private Label label6;
    private Panel panelCondition;
    private Label label3;
    private Label label2;
    private Label label1;
    private Button okBtn;
    private Button addBtn;
    private Button removeBtn;
    private TextBox textBoxName;
    private EMHelpLink emHelpLink1;
    private Button cancelBtn;
    private GridView gridViewConditions;
    private Label label4;
    private TextBox commentsTxt;

    public AutomatedConditionEditor(AutomatedConditionRuleInfo ruleInfo, Sessions.Session session)
    {
      this.ruleInfo = ruleInfo;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(session);
      this.session = session;
      this.channelControl = new ChannelConditionControl();
      if (this.ruleInfo != null)
        this.channelControl.ChannelValue = this.ruleInfo.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.AutomatedConditions);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_AutomatedConditions);
      this.addBtn.Enabled = this.removeBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    private void initForm()
    {
      this.gridViewConditions.Items.Clear();
      if (this.ruleInfo == null)
        return;
      this.textBoxName.Text = this.ruleInfo.RuleName;
      if (this.ruleInfo.CommentsTxt.Contains("\n") && !this.ruleInfo.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt;
      this.ruleCondForm.SetCondition((BizRuleInfo) this.ruleInfo);
      this.gridViewConditions.BeginUpdate();
      this.gridViewConditions.Items.Clear();
      if (this.ruleInfo.Conditions != null && this.ruleInfo.Conditions.Length != 0)
      {
        for (int index = 0; index < this.ruleInfo.Conditions.Length; ++index)
          this.gridViewConditions.Items.Add(new GVItem((object) this.ruleInfo.Conditions[index].ConditionType)
          {
            SubItems = {
              (object) this.ruleInfo.Conditions[index].ConditionName
            },
            Tag = (object) this.ruleInfo.Conditions[index]
          });
      }
      this.gridViewConditions.EndUpdate();
    }

    public AutomatedConditionRuleInfo AutomatedCondition => this.ruleInfo;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.AutomatedConditions)).GetAllRules();
        for (int index = 0; index < allRules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), allRules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.ruleInfo == null)
              flag = true;
            else if (this.ruleInfo.RuleID != allRules[index].RuleID || this.ruleInfo.RuleID == 0)
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
        if (this.gridViewConditions.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a condition.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          List<EllieMae.EMLite.ClientServer.AutomatedCondition> automatedConditionList = new List<EllieMae.EMLite.ClientServer.AutomatedCondition>();
          for (int nItemIndex = 0; nItemIndex < this.gridViewConditions.Items.Count; ++nItemIndex)
            automatedConditionList.Add((EllieMae.EMLite.ClientServer.AutomatedCondition) this.gridViewConditions.Items[nItemIndex].Tag);
          this.ruleInfo = this.ruleInfo == null ? new AutomatedConditionRuleInfo(this.textBoxName.Text.Trim()) : new AutomatedConditionRuleInfo(this.ruleInfo.RuleID, this.textBoxName.Text.Trim());
          this.ruleInfo.Conditions = automatedConditionList.ToArray();
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.ruleInfo);
          this.ruleInfo.Condition2 = this.channelControl.ChannelValue;
          this.ruleInfo.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      using (SelectConditionForm selectConditionForm = new SelectConditionForm(this.session))
      {
        if (selectConditionForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        string conditionTypeString = selectConditionForm.SelectedConditionTypeString;
        ConditionType selectedConditionType = selectConditionForm.SelectedConditionType;
        List<ConditionTemplate> selectedConditions = selectConditionForm.GetSelectedConditions();
        this.gridViewConditions.BeginUpdate();
        foreach (ConditionTemplate conditionTemplate in selectedConditions)
        {
          EllieMae.EMLite.ClientServer.AutomatedCondition automatedCondition = new EllieMae.EMLite.ClientServer.AutomatedCondition(selectedConditionType, conditionTemplate.Name);
          this.gridViewConditions.Items.Add(new GVItem(conditionTypeString)
          {
            SubItems = {
              (object) conditionTemplate.Name
            },
            Tag = (object) automatedCondition
          });
        }
        this.gridViewConditions.EndUpdate();
      }
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewConditions.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a condition first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.gridViewConditions.SelectedItems[0].Index;
        for (int nItemIndex = this.gridViewConditions.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gridViewConditions.Items[nItemIndex].Selected)
            this.gridViewConditions.Items.RemoveAt(nItemIndex);
        }
        if (this.gridViewConditions.Items.Count == 0)
          return;
        if (index > this.gridViewConditions.Items.Count - 1)
          this.gridViewConditions.Items[this.gridViewConditions.Items.Count - 1].Selected = true;
        else
          this.gridViewConditions.Items[index].Selected = true;
      }
    }

    private void gridViewConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.removeBtn.Enabled = this.gridViewConditions.SelectedItems.Count > 0;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        this.cancelBtn.PerformClick();
      }
      else
      {
        if (e.KeyCode != Keys.F1)
          return;
        this.ShowHelp();
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (AutomatedConditionEditor));
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
      this.panelChannel = new Panel();
      this.label6 = new Label();
      this.panelCondition = new Panel();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.okBtn = new Button();
      this.addBtn = new Button();
      this.removeBtn = new Button();
      this.textBoxName = new TextBox();
      this.cancelBtn = new Button();
      this.gridViewConditions = new GridView();
      this.emHelpLink1 = new EMHelpLink();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.SuspendLayout();
      this.panelChannel.Location = new Point(29, 87);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(548, 79);
      this.panelChannel.TabIndex = 3;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(13, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelCondition.Location = new Point(29, 209);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(548, 92);
      this.panelCondition.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(13, 315);
      this.label3.Name = "label3";
      this.label3.Size = new Size(173, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Add and apply field events";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(13, 184);
      this.label2.Name = "label2";
      this.label2.Size = new Size(302, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this Automated Conditions";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(13, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(233, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Automated Conditions Name";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(833, 577);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 12;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.addBtn.Location = new Point(587, 340);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 23);
      this.addBtn.TabIndex = 38;
      this.addBtn.Text = "&Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.removeBtn.Location = new Point(587, 369);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(75, 23);
      this.removeBtn.TabIndex = 40;
      this.removeBtn.Text = "&Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.textBoxName.Location = new Point(28, 28);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(549, 20);
      this.textBoxName.TabIndex = 1;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(913, 577);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 13;
      this.cancelBtn.Text = "Cancel";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "headerType";
      gvColumn1.Text = "Condition Type";
      gvColumn1.Width = 170;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "headerName";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Condition Name";
      gvColumn2.Width = 377;
      this.gridViewConditions.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewConditions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewConditions.Location = new Point(28, 340);
      this.gridViewConditions.Name = "gridViewConditions";
      this.gridViewConditions.Size = new Size(549, 227);
      this.gridViewConditions.TabIndex = 7;
      this.gridViewConditions.SelectedIndexChanged += new EventHandler(this.gridViewConditions_SelectedIndexChanged);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Automated Conditions";
      this.emHelpLink1.Location = new Point(13, 584);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(621, 7);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(624, 28);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(364, 138);
      this.commentsTxt.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1007, 612);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.gridViewConditions);
      this.Controls.Add((Control) this.panelChannel);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.removeBtn);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutomatedConditionEditor);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Automated Conditions Editor";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
