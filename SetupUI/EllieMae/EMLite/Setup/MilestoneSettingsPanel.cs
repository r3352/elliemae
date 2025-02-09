// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneSettingsPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.MSWorksheet;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneSettingsPanel : SettingsUserControl
  {
    private const string className = "MilestoneSettingsPanel";
    private static readonly string sw = Tracing.SwCommon;
    private Sessions.Session session;
    private bool allowMultiSelect;
    private bool isArchived;
    private RoleInfo[] roles;
    private InputFormInfo[] inputForms;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones;
    private MilestoneTemplate currentTemplate;
    private string autoLoanNumberingID = "";
    private RuleConditionControl ruleCondForm;
    private InputFormRuleInfo inputFormRule;
    private MilestoneTemplate defaultTemplate;
    private bool editFreeRoleTemplate;
    private bool editMilestoneTemplate;
    private bool editSettingsTemplate;
    private bool editeDisSettingsTemplate;
    private Hashtable defaultSettingsMT;
    private EllieMae.EMLite.Workflow.Milestone customLoanNumber;
    private Dictionary<string, Dictionary<MilestoneTemplate, string>> exceptionsList = new Dictionary<string, Dictionary<MilestoneTemplate, string>>();
    private Dictionary<string, List<string>> exceptionsListRemove = new Dictionary<string, List<string>>();
    private Dictionary<string, string> exceptionsTemplate = new Dictionary<string, string>();
    private EDisclosureSetup setup;
    private IContainer components;
    private TabControl tabMilestones;
    private TabPage tpMilestones;
    private TabPage tpTemplates;
    private GroupContainer gcMilestones;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnNewMilestone;
    private StandardIconButton btnEditMilestone;
    private StandardIconButton btnMoveMilestoneUp;
    private StandardIconButton btnMoveMilestoneDown;
    private GridView gvMilestones;
    private Button btnArchive;
    private FlowLayoutPanel flowLayoutPanel2;
    private ToolTip toolTip1;
    private StandardIconButton btnResetTemplate;
    private StandardIconButton btnSaveTemplate;
    private RadioButton rdbArchived;
    private RadioButton rdbCurrent;
    private Label lblglobalSettings;
    private GroupContainer grpTemplates;
    private GridView gvTemplates;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton btnDeleteTemplate;
    private FlowLayoutPanel flowLayoutPanel4;
    private StandardIconButton btnAddTemplate;
    private StandardIconButton btnDuplicateTemplate;
    private StandardIconButton btnMoveTemplateDown;
    private StandardIconButton btnMoveTemplateUp;
    protected Button btnStatus;
    private Panel panel1;
    private CollapsibleSplitter collapsibleSplitter1;
    private Button btnGlobalSettings;
    private Panel panel2;
    private GradientPanel gradientPanel1;
    private GroupContainer groupTemplateDetails;
    private TabControl tabTemplates;
    private TabPage tabDetails;
    private TabPage tabMilestoneDetails;
    private TabPage tabSettings;
    private TabPage tabRoles;
    private Label label1;
    private TextBox txtComments;
    private TextBox txtName;
    private Label label3;
    private Panel panelCondition;
    private Panel panelChannel;
    private Label label2;
    private Label label6;
    private GroupContainer groupContainer1;
    private GridView gvSequentialMilestones;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton stdRemoveMilestone;
    private StandardIconButton btnMoveTemplateMilestoneUp;
    private StandardIconButton btnMoveTemplateMilestoneDown;
    private StandardIconButton stdAddMilestoneToTemplate;
    private Label lblAutoLoanNumber;
    private GroupContainer groupContainer2;
    private FlowLayoutPanel flowLayoutPanel5;
    private StandardIconButton btnRemoveFreeRole;
    private StandardIconButton stdAddFreeRoleToTemplate;
    private GridView gvRoles;
    private MilestoneDropdownBox cmbLoanNumbering;
    protected Label lblDisconnectedFromPersona;
    protected Label lblLinkedWithPersona;
    private ImageList imageList1;
    private System.Windows.Forms.LinkLabel linkAutoLoanNumber;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel3;
    private Panel panel4;
    private Label label4;
    private FlowLayoutPanel flowEDisclosure;
    private GroupContainer grpBankerRetail;
    private Label lblRA;
    private Label lblRAL;
    private Label lblRAA;
    private MilestoneDropdownBox mdbRA;
    private MilestoneDropdownBox mdbRAL;
    private MilestoneDropdownBox mdbRTD;
    private MilestoneDropdownBox mdbRAA;
    private GroupContainer grpBankerWholesale;
    private MilestoneDropdownBox mdbWA;
    private MilestoneDropdownBox mdbWAL;
    private MilestoneDropdownBox mdbWTD;
    private MilestoneDropdownBox mdbWAA;
    private Label lblWA;
    private Label lblWAL;
    private Label lblWTD;
    private Label lblWAA;
    private GroupContainer grpBroker;
    private MilestoneDropdownBox mdbBA;
    private MilestoneDropdownBox mdbBAL;
    private MilestoneDropdownBox mdbBTD;
    private MilestoneDropdownBox mdbBAA;
    private Label lblBA;
    private Label lblBAL;
    private Label lblBTD;
    private Label lblBAA;
    private GroupContainer grpCorrespondent;
    private MilestoneDropdownBox mdbCA;
    private MilestoneDropdownBox mdbCAL;
    private MilestoneDropdownBox mdbCTD;
    private MilestoneDropdownBox mdbCAA;
    private Label lblCA;
    private Label lblCAL;
    private Label lblCTD;
    private Label lblCAA;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private Panel pnlRAA;
    private Panel pnlRTD;
    private Label lblRTD;
    private Panel pnlRA;
    private Panel pnlRAL;
    private Panel pnlWA;
    private Panel pnlWAA;
    private Panel pnlWAL;
    private Panel pnlWTD;
    private Panel pnlCA;
    private Panel pnlCAA;
    private Panel pnlCAL;
    private Panel pnlCTD;
    private Panel pnlBAA;
    private Panel pnlBTD;
    private Panel pnlBAL;
    private Panel pnlBA;
    private Label label5;
    private Label label7;
    private GridView gvChannels;

    public MilestoneSettingsPanel(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      if (this.session.EncompassEdition == EncompassEdition.Broker)
        this.brokerVersion();
      this.allowMultiSelect = allowMultiSelect;
      this.gvMilestones.AllowMultiselect = this.allowMultiSelect;
      this.ResetVariables();
      if (this.inputFormRule != null)
        this.fillChannels(this.inputFormRule.Condition2);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.ruleCondForm.Dock = DockStyle.Fill;
      this.ruleCondForm.Location = new Point(3, 16);
      this.ruleCondForm.Name = nameof (ruleCondForm);
      this.ruleCondForm.Size = new Size(562, 29);
      this.ruleCondForm.TabIndex = 0;
      this.ruleCondForm.InitControl(BpmCategory.Milestones);
      this.ruleCondForm.ResizeComboCondition();
      this.ruleCondForm.ChangesMadeToConditions += new EventHandler(this.changesMade);
      this.refreshMilestones(-1);
      this.setDirtyFlagValue(false);
      if (setupContainer != null)
        return;
      this.linkAutoLoanNumber.Visible = this.linkLabel1.Visible = false;
    }

    public void ResetVariables()
    {
      try
      {
        this.roles = this.session.SessionObjects.BpmManager.GetAllRoleFunctions();
        this.inputForms = this.session.SessionObjects.FormManager.GetAllFormInfos();
        this.defaultTemplate = this.session.SessionObjects.BpmManager.GetDefaultMilestoneTemplate();
        this.defaultSettingsMT = this.session.SessionObjects.BpmManager.GetMilestoneTemplateDefaultSettings();
        ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ResetMilestonesCache();
        this.allMilestones = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
        this.customLoanNumber = this.getCustomSetting("Policies.LoanNumber");
        this.setup = this.session.ConfigurationManager.GetEDisclosureSetup();
      }
      catch
      {
      }
    }

    private void initialPageControl(MilestoneTemplate template)
    {
      this.txtName.Text = template.Name;
      FieldRuleInfo templateConditions = this.session.SessionObjects.BpmManager.GetMilestoneTemplateConditions(template.TemplateID);
      if (templateConditions.Condition2 != null)
        this.fillChannels(templateConditions.Condition2);
      this.ruleCondForm.SetCondition((BizRuleInfo) templateConditions);
      this.txtComments.Text = template.Comment;
      this.tableLayoutPanel1.Enabled = true;
      foreach (Control control in (ArrangedElementCollection) this.tableLayoutPanel1.Controls)
        control.Enabled = !template.IsDefaultTemplate;
      this.populateTemplateMilestones(template);
      this.populateTemplateFreeRole(template);
      Point point = this.populateTemplateSettings(template);
      this.exceptionsListRemove.Clear();
      this.tabDetails.Visible = this.tabMilestoneDetails.Visible = this.tabSettings.Visible = this.tabRoles.Visible = false;
      switch (this.tabTemplates.SelectedIndex)
      {
        case 0:
          this.tabDetails.Visible = true;
          break;
        case 1:
          this.tabMilestoneDetails.Visible = true;
          break;
        case 2:
          if (this.session.EncompassEdition == EncompassEdition.Broker)
          {
            this.tabRoles.Visible = true;
            break;
          }
          this.tabSettings.Visible = true;
          this.tabSettings.AutoScrollPosition = new Point(point.X, Math.Abs(point.Y));
          break;
        case 3:
          this.tabRoles.Visible = true;
          break;
        default:
          this.tabDetails.Visible = true;
          break;
      }
    }

    private void refreshMilestones(int selectedIndex)
    {
      this.gvMilestones.Items.Clear();
      this.allMilestones = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
      this.allMilestones.ToList<EllieMae.EMLite.Workflow.Milestone>().ForEach((Action<EllieMae.EMLite.Workflow.Milestone>) (item =>
      {
        if (item.Archived != this.isArchived)
          return;
        this.gvMilestones.Items.Add(this.createGVItemForMilestone(item));
      }));
      if (this.isArchived || selectedIndex == -1)
        return;
      this.gvMilestones.Items[selectedIndex].Selected = true;
      this.gvMilestones.EnsureVisible(selectedIndex);
    }

    private GVItem createGVItemForMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      GVItem itemForMilestone = new GVItem();
      itemForMilestone.Tag = (object) ms;
      this.populateGVItemForMilestone(itemForMilestone);
      return itemForMilestone;
    }

    private void populateGVItemForMilestone(GVItem item)
    {
      EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) item.Tag;
      MilestoneLabel milestoneLabel = new MilestoneLabel(tag);
      if (tag.Archived && milestoneLabel.DisplayName.Contains("Archived"))
        milestoneLabel.DisplayName = milestoneLabel.DisplayName.Substring(0, milestoneLabel.DisplayName.Length - 10);
      item.Value = (object) milestoneLabel;
      item.SubItems.Add((object) tag.TPOConnectStatus);
      item.SubItems.Add((object) tag.ConsumerStatus);
      item.SubItems.Add((object) this.getRoleName(tag.RoleID));
      item.SubItems.Add((object) this.getFormName(tag.SummaryFormID));
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    private string getFormName(string formId)
    {
      InputFormInfo inputFormInfo = ((IEnumerable<InputFormInfo>) this.inputForms).FirstOrDefault<InputFormInfo>((Func<InputFormInfo, bool>) (x => string.Compare(x.FormID, formId, true) == 0));
      return inputFormInfo != (InputFormInfo) null ? inputFormInfo.Name : "";
    }

    private void gvMilestones_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.rdbArchived.Checked)
        return;
      this.openEditor((EllieMae.EMLite.Workflow.Milestone) e.Item.Tag);
    }

    private void updateBizRule()
    {
      if (this.session.IsBankerEdition())
        return;
      Session.BPM.GetBpmManager(BizRuleType.LoanAccess).SyncBrokerRules();
    }

    private void gvMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      EllieMae.EMLite.Workflow.Milestone milestone = (EllieMae.EMLite.Workflow.Milestone) null;
      if (this.gvMilestones.SelectedItems.Count > 0)
      {
        milestone = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.SelectedItems[0].Tag;
        if (this.rdbArchived.Checked)
          this.btnArchive.Enabled = this.gvMilestones.SelectedItems.Count > 0;
        else
          this.btnArchive.Enabled = this.gvMilestones.SelectedItems.Count > 0 && this.gvMilestones.SelectedItems[0].Index > 0 && this.gvMilestones.SelectedItems[0].Index != this.gvMilestones.Items.Count - 1;
        this.btnMoveMilestoneUp.Enabled = this.gvMilestones.SelectedItems.Count == 1 && this.gvMilestones.SelectedItems[0].Index > 1 && this.gvMilestones.SelectedItems[0].Index != this.gvMilestones.Items.Count - 1;
        this.btnMoveMilestoneDown.Enabled = this.gvMilestones.SelectedItems.Count == 1 && this.gvMilestones.SelectedItems[0].Index != 0 && this.gvMilestones.SelectedItems[0].Index < this.gvMilestones.Items.Count - 2;
        this.btnEditMilestone.Enabled = this.gvMilestones.SelectedItems.Count == 1;
      }
      else
        this.btnMoveMilestoneUp.Enabled = this.btnMoveMilestoneDown.Enabled = this.btnEditMilestone.Enabled = false;
    }

    private void btnEditMilestone_Click(object sender, EventArgs e)
    {
      if (this.gvMilestones.SelectedItems.Count == 0)
        return;
      this.openEditor((EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.SelectedItems[0].Tag);
    }

    private void openEditor(EllieMae.EMLite.Workflow.Milestone ms)
    {
      using (MilestonePropertiesForm milestonePropertiesForm = new MilestonePropertiesForm(this.session, ms))
      {
        if (milestonePropertiesForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ResetMilestones();
        this.refreshMilestones(this.gvMilestones.SelectedItems[0].Index);
        if (this.session.IsBrokerEdition())
          this.brokerVersionSaveMilestone(true);
        this.rebuildMilestoneSessionInfo();
      }
    }

    private void btnNewMilestone_Click(object sender, EventArgs e)
    {
      using (MilestonePropertiesForm milestonePropertiesForm = new MilestonePropertiesForm(this.session, (EllieMae.EMLite.Workflow.Milestone) null))
      {
        if (milestonePropertiesForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int num = this.gvMilestones.SelectedItems.Count > 0 ? this.gvMilestones.SelectedItems[0].Index + 1 : this.gvMilestones.Items.Count - 1;
        ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ResetMilestones();
        this.refreshMilestones(-1);
        if (this.gvMilestones.Items.Count - 2 != num)
        {
          this.moveMilestone(this.gvMilestones.Items.Count - 2, num);
        }
        else
        {
          this.gvMilestones.Items[num].Selected = true;
          this.gvMilestones.EnsureVisible(num);
        }
        if (this.session.IsBrokerEdition())
          this.brokerVersionSaveMilestone(false);
        this.rebuildMilestoneSessionInfo();
      }
    }

    private void btnMoveMilestoneUp_Click(object sender, EventArgs e)
    {
      if (this.gvMilestones.SelectedItems.Count > 0)
        this.moveMilestone(this.gvMilestones.SelectedItems[0].Index, this.gvMilestones.SelectedItems[0].Index - 1);
      this.gvMilestones.EnsureVisible(this.gvMilestones.SelectedItems[0].Index - 1);
    }

    private void btnMoveMilestoneDown_Click(object sender, EventArgs e)
    {
      if (this.gvMilestones.SelectedItems.Count > 0)
        this.moveMilestone(this.gvMilestones.SelectedItems[0].Index, this.gvMilestones.SelectedItems[0].Index + 1);
      this.gvMilestones.EnsureVisible(this.gvMilestones.SelectedItems[0].Index + 1);
    }

    private void moveMilestone(int fromIndex, int toIndex)
    {
      try
      {
        EllieMae.EMLite.Workflow.Milestone tag1 = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[fromIndex].Tag;
        EllieMae.EMLite.Workflow.Milestone tag2 = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[toIndex].Tag;
        if (tag2.Name.Equals("Completion"))
        {
          tag2 = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[this.gvMilestones.Items.Count - 2].Tag;
          --toIndex;
        }
        EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(tag2.MilestoneID);
        if (milestoneById == null)
        {
          this.refreshMilestones(toIndex);
        }
        else
        {
          if (!tag1.Equals((object) milestoneById))
            this.session.SessionObjects.BpmManager.ChangeMilestoneSortIndex(tag1, milestoneById);
          ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ClearMilestoneCache();
          this.refreshMilestones(toIndex);
          this.updateBizRule();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneSettingsPanel.sw, nameof (MilestoneSettingsPanel), TraceLevel.Error, "Error saving milestone order: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while updating the milestone sequence: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private List<string> getOrderedMilestoneIDs()
    {
      List<string> msids = new List<string>();
      this.gvMilestones.Items.ToList<GVItem>().ForEach((Action<GVItem>) (item => msids.Add(((EllieMae.EMLite.Workflow.Milestone) item.Tag).MilestoneID)));
      return msids;
    }

    private void btnArchive_Click(object sender, EventArgs e)
    {
      if (this.gvMilestones.SelectedItems.Count == 0)
        return;
      EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.SelectedItems[0].Tag;
      if (!tag.Archived)
      {
        string templatebyMilestoneId = this.session.SessionObjects.BpmManager.GetMilestoneTemplatebyMilestoneID(tag.MilestoneID);
        if (!templatebyMilestoneId.Equals(""))
        {
          using (ArchiveTemplatesList archiveTemplatesList = new ArchiveTemplatesList(this.session, templatebyMilestoneId))
          {
            if (archiveTemplatesList.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
          }
        }
      }
      else
        this.rdbCurrent.Checked = true;
      try
      {
        this.session.SessionObjects.BpmManager.SetMilestoneArchiveFlag(tag.MilestoneID, !tag.Archived);
        tag.Archived = !tag.Archived;
        ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ResetMilestones();
        this.refreshMilestones(-1);
        this.refreshTemplates(false, true);
        this.updateBizRule();
        if (!this.gvMilestones.Items.ContainsTag((object) tag))
          return;
        this.gvMilestones.Items.GetItemByTag((object) tag).Selected = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneSettingsPanel.sw, nameof (MilestoneSettingsPanel), TraceLevel.Error, "Error setting mileston archive flag: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while updating the milestone: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void tabMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabMilestones.SelectedTab == this.tpTemplates)
      {
        this.btnGlobalSettings.Visible = true;
        this.refreshTemplates(true, true);
      }
      if (this.tabMilestones.SelectedTab != this.tpMilestones)
        return;
      this.btnGlobalSettings.Visible = false;
      this.refreshMilestones(-1);
    }

    private void refreshTemplates(bool reload, bool selectFirst)
    {
      IEnumerable<MilestoneTemplate> milestoneTemplates = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false);
      this.session.StartupInfo.MilestoneTemplate = this.session.SessionObjects.BpmManager.GetMilestoneTemplateConditions();
      this.session.BPM.PreloadMilestoneTemplateConditions(this.session.StartupInfo.MilestoneTemplate);
      if (reload)
      {
        this.gvTemplates.Items.Clear();
        foreach (MilestoneTemplate milestoneTemplate in milestoneTemplates.ToList<MilestoneTemplate>())
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) milestoneTemplate;
          this.gvTemplates.Items.Add(gvItem);
          this.populateGVItemFormMilestoneTemplate(gvItem);
        }
        if (selectFirst && this.gvTemplates.Items.Count > 0)
          this.gvTemplates.Items[0].Selected = true;
      }
      this.btnSaveTemplate.Enabled = this.btnResetTemplate.Enabled = false;
    }

    private void populateGVItemFormMilestoneTemplate(GVItem item)
    {
      MilestoneTemplate tag = (MilestoneTemplate) item.Tag;
      item.SubItems[0].Text = (item.Index + 1).ToString();
      item.SubItems[1].Text = tag.Name;
      item.SubItems[2].Text = tag.TemplateID;
      FieldRuleInfo templateConditions = this.session.SessionObjects.BpmManager.GetMilestoneTemplateConditions(tag.TemplateID);
      item.SubItems[3].Text = templateConditions.Condition != BizRule.Condition.AdvancedCoding ? (templateConditions.Condition != BizRule.Condition.LoanPurpose ? (templateConditions.Condition != BizRule.Condition.LoanType ? "No Condition" : templateConditions.Condition.ToString() + " = " + BizRule.LoanTypeStrings[Convert.ToInt32(templateConditions.ConditionState)]) : templateConditions.Condition.ToString() + " = " + BizRule.LoanPurposeStrings[Convert.ToInt32(templateConditions.ConditionState)]) : templateConditions.ConditionState.ToString();
      string[] array = templateConditions.Condition2.Split(',');
      if (array.Length == 5)
      {
        item.SubItems[4].Text = "All Selected";
      }
      else
      {
        item.SubItems[4].Text = "";
        foreach (string str in array)
        {
          GVSubItem subItem = item.SubItems[4];
          subItem.Text = subItem.Text + BizRule.ChannelStatusString[Utils.ParseInt((object) array[Array.IndexOf<string>(array, str)])] + ", ";
        }
        item.SubItems[4].Text = item.SubItems[4].Text.Substring(0, item.SubItems[4].Text.Length - 2);
      }
      item.SubItems[5].Text = tag.Active ? "Active" : "Inactive";
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.currentTemplate != null)
      {
        GVItem itemByTag = this.gvTemplates.Items.GetItemByTag((object) this.currentTemplate);
        if (itemByTag != null)
        {
          int index = itemByTag.Index;
          if (!this.validateTemplateChanges())
          {
            this.gvTemplates.SelectedIndexChanged -= new EventHandler(this.gvTemplates_SelectedIndexChanged);
            this.gvTemplates.Items[index].Selected = true;
            this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
            return;
          }
          this.populateGVItemFormMilestoneTemplate(itemByTag);
          itemByTag.Tag = (object) this.currentTemplate;
        }
      }
      MilestoneTemplate template = (MilestoneTemplate) null;
      if (this.gvTemplates.SelectedItems.Count > 0)
      {
        template = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
        this.gvTemplates.EnsureVisible(this.gvTemplates.SelectedItems[0].Index + 1);
      }
      if (template == null)
        return;
      this.initialPageControl(template);
      this.editeDisSettingsTemplate = this.editMilestoneTemplate = this.editFreeRoleTemplate = this.editSettingsTemplate = false;
      this.btnDeleteTemplate.Enabled = template != null && !template.IsDefaultTemplate;
      this.btnDuplicateTemplate.Enabled = template != null;
      this.btnMoveTemplateUp.Enabled = template != null && this.gvTemplates.SelectedItems[0].Index > 0 && !template.IsDefaultTemplate;
      this.btnMoveTemplateDown.Enabled = template != null && this.gvTemplates.SelectedItems[0].Index < this.gvTemplates.Items.Count - 2;
      if (!template.IsDefaultTemplate)
      {
        if (template.Active)
          this.btnStatus.Text = "Deac&tivate";
        else
          this.btnStatus.Text = "&Activate";
        this.btnStatus.Enabled = true;
      }
      else
        this.btnStatus.Enabled = false;
    }

    private void populateTemplateMilestones(MilestoneTemplate template)
    {
      this.currentTemplate = template;
      this.autoLoanNumberingID = this.currentTemplate.AutoLoanNumberingMilestoneID;
      this.exceptionsTemplate = new Dictionary<string, string>((IDictionary<string, string>) this.currentTemplate.EDisclosureMilestoneSettings);
      this.groupTemplateDetails.Text = template.Name;
      this.gvSequentialMilestones.Items.Clear();
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
        this.gvSequentialMilestones.Items.Add(this.createGVItemForTemplateMilestone(sequentialMilestone));
      this.setDirtyFlagValue(false);
      this.editMilestoneTemplate = false;
    }

    private void populateTemplateFreeRole(MilestoneTemplate template)
    {
      List<EllieMae.EMLite.Workflow.Milestone> milestones = this.getMilestones(template.SequentialMilestones.ToList<TemplateMilestone>());
      List<TemplateFreeRole> templateFreeRoleList = new List<TemplateFreeRole>((IEnumerable<TemplateFreeRole>) template.FreeRoles.ToArray<TemplateFreeRole>());
      this.gvRoles.Items.Clear();
      milestones.ForEach((Action<EllieMae.EMLite.Workflow.Milestone>) (x =>
      {
        RoleInfo roleById = this.getRoleByID(x.RoleID);
        EllieMae.EMLite.Workflow.Milestone milestoneById = this.getMilestoneByID(x.MilestoneID);
        if (roleById == null || milestoneById == null)
          return;
        MilestoneLabel milestoneLabel = new MilestoneLabel(milestoneById);
        this.gvRoles.Items.Add(new GVItem(roleById.RoleName)
        {
          Tag = (object) milestoneById,
          SubItems = {
            (object) milestoneLabel
          }
        });
      }));
      this.gvRoles.Items.Add(new GVItem("----- Roles Not Tied to a Milestone -----")
      {
        Tag = (object) "Free Role Placeholder"
      });
      templateFreeRoleList.ForEach((Action<TemplateFreeRole>) (x =>
      {
        RoleInfo roleById = this.getRoleByID(x.RoleID);
        if (roleById == null)
          return;
        this.gvRoles.Items.Add(new GVItem(roleById.RoleName)
        {
          Tag = (object) roleById
        });
      }));
    }

    private Point populateTemplateSettings(MilestoneTemplate template)
    {
      this.setAutoLoanNumber(template);
      Point edisclosureUi = this.createEDisclosureUI();
      this.setDirtyFlagValue(false);
      this.editSettingsTemplate = false;
      this.editeDisSettingsTemplate = false;
      return edisclosureUi;
    }

    private EllieMae.EMLite.Workflow.Milestone[] getActiveMilestones()
    {
      List<EllieMae.EMLite.Workflow.Milestone> milestones = new List<EllieMae.EMLite.Workflow.Milestone>();
      this.allMilestones.ToList<EllieMae.EMLite.Workflow.Milestone>().ForEach((Action<EllieMae.EMLite.Workflow.Milestone>) (item =>
      {
        if (item.Archived)
          return;
        milestones.Add(item);
      }));
      return milestones.ToArray();
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByID(string milestoneId)
    {
      return this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.MilestoneID, milestoneId, true) == 0));
    }

    private GVItem createGVItemForTemplateMilestone(TemplateMilestone templateMs)
    {
      return this.createGVItemForTemplateMilestone(templateMs.MilestoneID, templateMs.DaysToComplete);
    }

    private GVItem createGVItemForTemplateMilestone(string milestoneId, int daysToComplete)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneById = this.getMilestoneByID(milestoneId);
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new MilestoneLabel(milestoneById)
          },
          [1] = {
            Value = (object) milestoneById.TPOConnectStatus
          },
          [2] = {
            Text = this.getRoleName(milestoneById.RoleID)
          },
          [3] = {
            Value = (object) daysToComplete
          }
        },
        Tag = (object) milestoneById
      };
    }

    private void gvSequentialMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setTemplateMilestoneControlStates();
    }

    private void setTemplateMilestoneControlStates()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.gvSequentialMilestones.SelectedItems.Count > 0)
      {
        flag1 = this.gvSequentialMilestones.Items[1].Selected;
        flag2 = this.gvSequentialMilestones.Items[this.gvSequentialMilestones.Items.Count - 2].Selected;
      }
      this.getSelectedTemplateMilestones();
      this.btnMoveTemplateMilestoneUp.Enabled = this.gvSequentialMilestones.SelectedItems.Count == 1 && this.gvSequentialMilestones.SelectedItems[0].Index > 1 && this.gvSequentialMilestones.SelectedItems[0].Index != this.gvSequentialMilestones.Items.Count - 1;
      this.btnMoveTemplateMilestoneDown.Enabled = this.gvSequentialMilestones.SelectedItems.Count == 1 && this.gvSequentialMilestones.SelectedItems[0].Index != 0 && this.gvSequentialMilestones.SelectedItems[0].Index < this.gvSequentialMilestones.Items.Count - 2;
      this.stdRemoveMilestone.Enabled = this.gvSequentialMilestones.SelectedItems.Count == 1 && this.gvSequentialMilestones.SelectedItems[0].Index != 0 && this.gvSequentialMilestones.SelectedItems[0].Index < this.gvSequentialMilestones.Items.Count - 1;
    }

    private EllieMae.EMLite.Workflow.Milestone[] getSelectedTemplateMilestones()
    {
      List<EllieMae.EMLite.Workflow.Milestone> milestones = new List<EllieMae.EMLite.Workflow.Milestone>();
      this.gvSequentialMilestones.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (item => milestones.Add((EllieMae.EMLite.Workflow.Milestone) item.Tag)));
      return milestones.ToArray();
    }

    private void btnMoveTemplateMilestoneUp_Click(object sender, EventArgs e)
    {
      GridView sequentialMilestones = this.gvSequentialMilestones;
      for (int nItemIndex = 0; nItemIndex < sequentialMilestones.Items.Count; ++nItemIndex)
      {
        if (sequentialMilestones.Items[nItemIndex].Selected)
        {
          GVItem gvItem = sequentialMilestones.Items[nItemIndex];
          sequentialMilestones.Items.RemoveAt(nItemIndex);
          sequentialMilestones.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
          sequentialMilestones.EnsureVisible(nItemIndex - 1);
          break;
        }
      }
      this.editMilestoneTemplate = true;
      this.setDirtyFlagValue(true);
    }

    private void btnMoveTemplateMilestoneDown_Click(object sender, EventArgs e)
    {
      GridView sequentialMilestones = this.gvSequentialMilestones;
      for (int nItemIndex = sequentialMilestones.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (sequentialMilestones.Items[nItemIndex].Selected)
        {
          GVItem gvItem = sequentialMilestones.Items[nItemIndex];
          sequentialMilestones.Items.RemoveAt(nItemIndex);
          sequentialMilestones.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
          sequentialMilestones.EnsureVisible(nItemIndex + 1);
          break;
        }
      }
      this.editMilestoneTemplate = true;
      this.setDirtyFlagValue(true);
    }

    private void gvSequentialMilestones_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    private void btnSaveTemplate_Click(object sender, EventArgs e)
    {
      this.gvSequentialMilestones.StopEditing();
      MilestoneTemplate tag = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
      GVItem selectedItem = this.gvTemplates.SelectedItems[0];
      if (selectedItem == null)
        return;
      int index = selectedItem.Index;
      if (this.saveCurrentTemplate())
      {
        this.gvTemplates.Items.Remove(selectedItem);
        GVItem gvItem = new GVItem();
        gvItem.Tag = (object) tag;
        this.gvTemplates.Items.Insert(index, gvItem);
        this.populateGVItemFormMilestoneTemplate(gvItem);
        this.refreshTemplates(false, false);
      }
      this.gvTemplates.Items[index].Selected = true;
    }

    private bool saveCurrentTemplate()
    {
      string templateName = this.txtName.Text.Trim();
      if (this.currentTemplate.Name != templateName)
      {
        if (templateName == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name for the template in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        MilestoneTemplate milestoneTemplateByName = this.session.SessionObjects.BpmManager.GetMilestoneTemplateByName(templateName);
        if (milestoneTemplateByName != null && string.Compare(this.currentTemplate.TemplateID, milestoneTemplateByName.TemplateID, true) != 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The name '" + templateName + "' is already in use by another Milestone Template. You must select a unique name for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        this.currentTemplate.Name = templateName;
      }
      if (this.currentTemplate.Comment != this.txtComments.Text.Trim())
        this.currentTemplate.Comment = this.txtComments.Text.Trim();
      if (!this.ruleCondForm.ValidateCondition())
        return false;
      FieldRuleInfo rule = new FieldRuleInfo(this.txtName.Text.Trim());
      this.ruleCondForm.ApplyCondition((BizRuleInfo) rule);
      rule.Condition2 = this.getChannelValue();
      if (this.editMilestoneTemplate)
      {
        this.currentTemplate.SequentialMilestones.Clear();
        for (int nItemIndex = 0; nItemIndex < this.gvSequentialMilestones.Items.Count; ++nItemIndex)
          this.currentTemplate.SequentialMilestones.Append((EllieMae.EMLite.Workflow.Milestone) this.gvSequentialMilestones.Items[nItemIndex].Tag, Utils.ParseInt((object) this.gvSequentialMilestones.Items[nItemIndex].SubItems[3].Text, 0));
      }
      if (this.editFreeRoleTemplate)
      {
        this.currentTemplate.FreeRoles.Clear();
        for (int nItemIndex = this.gvRoles.Items.GetItemByTag((object) "Free Role Placeholder").Index + 1; nItemIndex < this.gvRoles.Items.Count; ++nItemIndex)
          this.currentTemplate.FreeRoles.Insert(new TemplateFreeRole(((RoleSummaryInfo) this.gvRoles.Items[nItemIndex].Tag).RoleID));
      }
      if (this.editSettingsTemplate)
        this.currentTemplate.AutoLoanNumberingMilestoneID = this.autoLoanNumberingID;
      if (this.editeDisSettingsTemplate)
        this.currentTemplate.EDisclosureMilestoneSettings = this.exceptionsTemplate;
      try
      {
        this.session.SessionObjects.BpmManager.UpdateMilestoneTemplate(this.currentTemplate, (BizRuleInfo) rule);
        if (this.editeDisSettingsTemplate)
        {
          this.session.SessionObjects.BpmManager.UpdateMilestoneTemplateEDisclosureExceptions(this.exceptionsList, false);
          this.session.SessionObjects.BpmManager.RemoveMilestoneTemplateEDisclosureExceptions(this.exceptionsListRemove);
        }
        this.setDirtyFlagValue(false);
        this.editeDisSettingsTemplate = this.editFreeRoleTemplate = this.editMilestoneTemplate = this.editSettingsTemplate = false;
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneSettingsPanel.sw, nameof (MilestoneSettingsPanel), TraceLevel.Error, "Error saving milestone template: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error saving template: " + ex.Message);
        return false;
      }
    }

    private void onMilestoneTemplateEditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e.EditorControl.Text == "")
      {
        e.EditorControl.Text = e.SubItem.Text;
      }
      else
      {
        int num1;
        try
        {
          num1 = Convert.ToInt32(e.EditorControl.Text);
        }
        catch (OverflowException ex)
        {
          num1 = 999;
        }
        if (num1 > 999)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You can't assign days to be higher than 999.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          num1 = 999;
        }
        if (!(e.EditorControl.Text != e.SubItem.Text))
          return;
        e.EditorControl.Text = num1.ToString();
        this.editMilestoneTemplate = true;
        this.setDirtyFlagValue(true);
      }
    }

    private void onMilestoneTemplateEditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      TextBoxFormatter.Attach((TextBox) e.EditorControl, TextBoxContentRule.NonNegativeInteger);
    }

    private void btnResetTemplate_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the current template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.initialPageControl(this.currentTemplate);
      this.setDirtyFlagValue(false);
    }

    private void btnAddTemplate_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      this.createNewTemplate();
    }

    private void createNewTemplate()
    {
      MilestoneTemplate milestoneTemplate = this.session.SessionObjects.BpmManager.GetDefaultMilestoneTemplate();
      if (milestoneTemplate != null)
        this.duplicateTemplate(milestoneTemplate, this.getNewtemplateName("New Template"), 0);
      this.tabDetails.Visible = this.tabMilestoneDetails.Visible = this.tabSettings.Visible = this.tabRoles.Visible = false;
      this.tabTemplates.SelectedIndex = 0;
      this.tabDetails.Visible = true;
      this.txtName.Focus();
    }

    private void btnStatus_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      if (this.btnStatus.Text.Contains("Activate"))
      {
        this.gvTemplates.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (item => this.setMilestoneTemplateActiveFlag(item, true)));
        this.btnStatus.Text = "Deac&tivate";
      }
      else if (this.btnStatus.Text.Contains("Deac"))
      {
        this.gvTemplates.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (item => this.setMilestoneTemplateActiveFlag(item, false)));
        this.btnStatus.Text = "&Activate";
      }
      this.refreshTemplates(false, false);
    }

    private void setMilestoneTemplateActiveFlag(GVItem item, bool active)
    {
      MilestoneTemplate tag = (MilestoneTemplate) item.Tag;
      this.session.SessionObjects.BpmManager.SetMilestoneTemplateActiveFlag(tag.TemplateID, active);
      tag.Active = active;
      item.Tag = (object) tag;
      this.populateGVItemFormMilestoneTemplate(item);
    }

    private void btnDuplicateTemplate_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      MilestoneTemplate tag = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
      this.duplicateTemplate(tag, this.getNewtemplateName("Copy of " + tag.Name), tag.SortIndex + 1);
    }

    private void duplicateTemplate(MilestoneTemplate template, string name, int sortIndex)
    {
      try
      {
        MilestoneTemplate template1 = new MilestoneTemplate();
        template1.Name = name;
        if (sortIndex > 0)
          template1.SortIndex = sortIndex;
        template1.Clear();
        foreach (TemplateMilestone msTemplate in template)
          template1.Add(msTemplate);
        FieldRuleInfo templateConditions = this.session.SessionObjects.BpmManager.GetMilestoneTemplateConditions(template.TemplateID);
        template1.Comment = template.Comment;
        foreach (TemplateFreeRole freeRole in template.FreeRoles)
          template1.FreeRoles.Insert(freeRole);
        this.session.SessionObjects.BpmManager.CreateMilestoneTemplate(template1, (BizRuleInfo) templateConditions);
        this.refreshTemplates(true, true);
      }
      catch (Exception ex)
      {
        if (!ex.Message.Contains("Cannot insert duplicate key in object"))
          return;
        int num = (int) MessageBox.Show((IWin32Window) null, "A duplicate milestone template could not be created because a template with the same name already exists. Please rename the selected milestone template and then try to create a duplicate again. When renaming the selected template, the template name cannot exceed 37 characters.", "Error - Template Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void gvTemplates_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    private void gvTemplates_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }

    private void gvTemplates_DragDrop(object sender, DragEventArgs e)
    {
      Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
      int index = ((GVItem) e.Data.GetData(typeof (GVItem))).Index;
      GVItem gvItem = this.gvTemplates.Items[index];
      int rowIndex = this.gvTemplates.HitTest(client).RowIndex;
      if (e.Effect != DragDropEffects.Move || rowIndex == -1)
        return;
      if (rowIndex == index)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a template to its own position");
      }
      else if (rowIndex < this.gvTemplates.Items.Count - 1 && index < this.gvTemplates.Items.Count - 1)
      {
        this.changeTemplateSortIndex(index, rowIndex);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot move the default template from its position.");
      }
    }

    private void btnMoveTemplateUp_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      this.changeTemplateSortIndex(this.gvTemplates.SelectedItems[0].Index, this.gvTemplates.SelectedItems[0].Index - 1);
    }

    private void btnMoveTemplateDown_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      this.changeTemplateSortIndex(this.gvTemplates.SelectedItems[0].Index, this.gvTemplates.SelectedItems[0].Index + 1);
    }

    private void changeTemplateSortIndex(int oldIndex, int newIndex)
    {
      this.session.SessionObjects.BpmManager.ChangeMilestoneTemplateSortIndex((MilestoneTemplate) this.gvTemplates.Items[oldIndex].Tag, (MilestoneTemplate) this.gvTemplates.Items[newIndex].Tag);
      this.refreshTemplates(true, false);
      this.gvTemplates.Items[newIndex].Selected = true;
      this.gvTemplates.EnsureVisible(newIndex);
    }

    private bool validateTemplateChanges()
    {
      if (!this.btnSaveTemplate.Enabled)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Do you want to save your changes to the template '" + this.currentTemplate.Name + "'?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          this.setDirtyFlagValue(false);
          return true;
        default:
          return this.saveCurrentTemplate();
      }
    }

    private void btnDeleteTemplate_Click(object sender, EventArgs e)
    {
      MilestoneTemplate tag = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
      if (tag.IsDefaultTemplate)
        return;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete the '" + tag.Name + "' template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      try
      {
        this.session.SessionObjects.BpmManager.DeleteMilestoneTemplate(tag.TemplateID);
        this.refreshTemplates(true, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneSettingsPanel.sw, nameof (MilestoneSettingsPanel), TraceLevel.Error, "Error deleting template: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while attmepting to delete milestone template: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void CurrentArchived_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdbArchived.Checked)
      {
        this.isArchived = true;
        this.btnArchive.Text = "&Move to Current";
        this.btnArchive.Enabled = false;
        this.btnNewMilestone.Visible = this.btnEditMilestone.Visible = this.btnMoveMilestoneUp.Visible = this.btnMoveMilestoneDown.Visible = this.verticalSeparator1.Visible = false;
        this.gvMilestones.AllowDrop = false;
      }
      else if (this.rdbCurrent.Checked)
      {
        this.isArchived = false;
        this.btnArchive.Text = "&Archive";
        this.btnArchive.Enabled = false;
        this.btnNewMilestone.Visible = this.btnEditMilestone.Visible = this.btnMoveMilestoneUp.Visible = this.btnMoveMilestoneDown.Visible = this.verticalSeparator1.Visible = true;
        this.gvMilestones.AllowDrop = true;
      }
      this.refreshMilestones(-1);
    }

    private void gvMilestones_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    private void gvMilestones_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }

    private void gvMilestones_DragDrop(object sender, DragEventArgs e)
    {
      Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
      int index = ((GVItem) e.Data.GetData(typeof (GVItem))).Index;
      GVItem gvItem = this.gvMilestones.Items[index];
      int rowIndex = this.gvMilestones.HitTest(client).RowIndex;
      if (e.Effect != DragDropEffects.Move)
        return;
      if (rowIndex == index)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a milestone to its own position");
      }
      else if (rowIndex > 0 && rowIndex < this.gvMilestones.Items.Count - 1 && index > 0 && index < this.gvMilestones.Items.Count - 1)
      {
        this.moveMilestone(index, rowIndex);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot move \"Started\" and \"Completion\" milestones.");
      }
    }

    public override void Save()
    {
      if (!this.saveCurrentTemplate())
        return;
      this.setDirtyFlagValue(false);
    }

    private void stdRemoveMilestone_Click(object sender, EventArgs e)
    {
      if (this.gvSequentialMilestones.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a milestone to remove.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) this.gvSequentialMilestones.SelectedItems[0].Tag;
        if (tag.Name.Equals("Started") || tag.Name.Equals("Completion"))
          return;
        string str = "";
        if (this.exceptionsTemplate.ContainsValue(tag.MilestoneID))
          str += "\n\t-When to send eDisclosure packages";
        if (tag.MilestoneID == this.cmbLoanNumbering.MilestoneID)
          str += "\n\t-When to start auto loan numbering";
        if (str != "" && MessageBox.Show((IWin32Window) this, "An issue occurred for the following actions:\n" + str + "\n\nThe milestone you are deleting is being used as a trigger milestone for the actions above. If you proceed, your default milestone will be used. If your default milestone is not included in the milestone template, the system default milestone will be used." + "\n\nAre you sure you want to proceed?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
          return;
        if (this.gvRoles.Items.GetItemByTag(this.gvSequentialMilestones.SelectedItems[0].Tag) != null)
          this.gvRoles.Items.Remove(this.gvRoles.Items.GetItemByTag(this.gvSequentialMilestones.SelectedItems[0].Tag));
        this.gvSequentialMilestones.Items.Remove(this.gvSequentialMilestones.SelectedItems[0]);
        if (tag.MilestoneID == this.cmbLoanNumbering.MilestoneID)
        {
          this.autoLoanNumberingID = "";
          this.editSettingsTemplate = true;
        }
        foreach (KeyValuePair<string, string> keyValuePair in new Dictionary<string, string>((IDictionary<string, string>) this.exceptionsTemplate))
        {
          if (keyValuePair.Value == tag.MilestoneID)
          {
            this.exceptionsTemplate.Remove(keyValuePair.Key);
            if (!this.exceptionsListRemove.ContainsKey(this.currentTemplate.TemplateID))
            {
              this.exceptionsListRemove.Add(this.currentTemplate.TemplateID, new List<string>()
              {
                tag.MilestoneID
              });
            }
            else
            {
              List<string> stringList = this.exceptionsListRemove[this.currentTemplate.TemplateID];
              stringList.Add(tag.MilestoneID);
              this.exceptionsListRemove[this.currentTemplate.TemplateID] = stringList;
            }
          }
        }
        this.setAutoLoanNumber(this.currentTemplate);
        this.editeDisSettingsTemplate = this.editMilestoneTemplate = this.editFreeRoleTemplate = true;
        this.setDirtyFlagValue(true);
      }
    }

    private void collapsibleSplitter1_Click(object sender, EventArgs e)
    {
      if (this.collapsibleSplitter1.IsCollapsed)
      {
        this.collapsibleSplitter1.Dock = DockStyle.Bottom;
        this.gvTemplates.Dock = DockStyle.Fill;
      }
      else
      {
        this.gvTemplates.Dock = DockStyle.Top;
        this.collapsibleSplitter1.Dock = DockStyle.Top;
      }
    }

    private void btnGlobalSettings_Click(object sender, EventArgs e)
    {
      int num = (int) new GlobalSettings(this.session).ShowDialog((IWin32Window) this);
    }

    private void gvTemplates_SizeChanged(object sender, EventArgs e)
    {
      this.gvTemplates.Columns[0].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.1);
      this.gvTemplates.Columns[1].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.2);
      this.gvTemplates.Columns[2].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.1);
      this.gvTemplates.Columns[3].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.2);
      this.gvTemplates.Columns[4].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.2);
      this.gvTemplates.Columns[5].Width = Convert.ToInt32((double) this.gvTemplates.Width * 0.1);
    }

    private void fillChannels(string value)
    {
      string[] strArray1 = new string[5]
      {
        "No channel selected",
        "Banked – Retail",
        "Banked – Wholesale",
        "Brokered",
        "Correspondent"
      };
      if (this.gvChannels.Items.Count <= 0)
      {
        foreach (string text in strArray1)
          this.gvChannels.Items.Add(new GVItem(text));
      }
      for (int nItemIndex = 0; nItemIndex < this.gvChannels.Items.Count; ++nItemIndex)
        this.gvChannels.Items[nItemIndex].Checked = false;
      string[] strArray2 = value.Split(',');
      for (int index = 0; index < strArray2.Length; ++index)
      {
        if (Utils.ParseInt((object) strArray2[index]) >= 0)
          this.gvChannels.Items[Utils.ParseInt((object) strArray2[index])].Checked = true;
      }
    }

    private string getChannelValue()
    {
      string channelValue = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.gvChannels.Items.Count; ++nItemIndex)
      {
        if (this.gvChannels.Items[nItemIndex].Checked)
        {
          if (channelValue != string.Empty)
            channelValue += ",";
          channelValue += (string) (object) nItemIndex;
        }
      }
      if (channelValue == string.Empty)
        channelValue = "0";
      return channelValue;
    }

    private void tabMilestones_Deselecting(object sender, TabControlCancelEventArgs e)
    {
      if (this.tabMilestones.SelectedTab == this.tpTemplates)
      {
        if (this.validateTemplateChanges())
          return;
        e.Cancel = true;
      }
      else
      {
        if (this.tabMilestones.SelectedTab != this.tpMilestones)
          return;
        ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).ClearMilestoneCache();
      }
    }

    private RoleInfo getRoleByID(int roleID)
    {
      return ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleID));
    }

    private List<EllieMae.EMLite.Workflow.Milestone> getMilestones(
      List<TemplateMilestone> tempMilestones)
    {
      List<EllieMae.EMLite.Workflow.Milestone> result = new List<EllieMae.EMLite.Workflow.Milestone>();
      tempMilestones.ForEach((Action<TemplateMilestone>) (x => result.Add(this.getMilestoneByID(x.MilestoneID))));
      return result;
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByName(string milestoneName)
    {
      return this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.Name, milestoneName, true) == 0));
    }

    private string getNewtemplateName(string newName)
    {
      IEnumerable<MilestoneTemplate> milestoneTemplates = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false);
      int num = 1;
      string returnName = newName;
      List<MilestoneTemplate> all = milestoneTemplates.ToList<MilestoneTemplate>().FindAll((Predicate<MilestoneTemplate>) (x => x.Name.Contains(newName)));
      while (all.Exists((Predicate<MilestoneTemplate>) (x => x.Name == returnName)))
      {
        returnName = newName + " (" + (object) num + ")";
        ++num;
      }
      return returnName;
    }

    private void changesMade(object sender, EventArgs e) => this.setDirtyFlagValue(true);

    private void stdAddMilestoneToTemplate_Click(object sender, EventArgs e)
    {
      List<EllieMae.EMLite.Workflow.Milestone> usedMilestones = new List<EllieMae.EMLite.Workflow.Milestone>();
      this.gvSequentialMilestones.Items.ToList<GVItem>().ForEach((Action<GVItem>) (item => usedMilestones.Add((EllieMae.EMLite.Workflow.Milestone) item.Tag)));
      AddMilestoneToTemplate milestoneToTemplate = new AddMilestoneToTemplate(this.session, ((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) this.getActiveMilestones()).Select<EllieMae.EMLite.Workflow.Milestone, EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, EllieMae.EMLite.Workflow.Milestone>) (n => n)).Except<EllieMae.EMLite.Workflow.Milestone>((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) usedMilestones).ToList<EllieMae.EMLite.Workflow.Milestone>(), this.roles);
      int num1 = (int) milestoneToTemplate.ShowDialog();
      if (milestoneToTemplate.DialogResult != DialogResult.OK)
        return;
      this.setDirtyFlagValue(true);
      this.editMilestoneTemplate = true;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneToTemplate.MilestonesAdded)
      {
        GVItem templateMilestone = this.createGVItemForTemplateMilestone(milestone.MilestoneID, milestone.DefaultDays);
        if (this.gvSequentialMilestones.SelectedItems.Count > 0 && this.gvSequentialMilestones.SelectedItems[0].Index != 0)
          this.gvSequentialMilestones.Items.Insert(this.gvSequentialMilestones.SelectedItems[0].Index, templateMilestone);
        else
          this.gvSequentialMilestones.Items.Insert(this.gvSequentialMilestones.Items.Count - 1, templateMilestone);
        int index = this.gvRoles.Items.GetItemByTag((object) "Free Role Placeholder").Index;
        for (int nItemIndex = index + 1; nItemIndex < this.gvRoles.Items.Count; ++nItemIndex)
        {
          if (this.gvRoles.Items[nItemIndex].Tag is RoleInfo && ((RoleSummaryInfo) this.gvRoles.Items[nItemIndex].Tag).RoleID == milestone.RoleID)
          {
            this.gvRoles.Items.Remove(this.gvRoles.Items[nItemIndex]);
            this.editFreeRoleTemplate = true;
          }
        }
        MilestoneLabel milestoneLabel = new MilestoneLabel(milestone);
        RoleInfo roleById = this.getRoleByID(milestone.RoleID);
        if (roleById != null)
          this.gvRoles.Items.Insert(index, new GVItem(roleById.RoleName)
          {
            Tag = (object) milestone,
            SubItems = {
              (object) milestoneLabel
            }
          });
      }
      if (milestoneToTemplate.MilestonesAdded.Contains(this.getMilestoneByID(this.customLoanNumber.MilestoneID)) && this.lblAutoLoanNumber.ImageIndex != 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "This milestone is the customer default of one or more settings and will be used going forward.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.autoLoanNumberingID = "";
      }
      this.setAutoLoanNumber(this.currentTemplate);
    }

    private void stdAddFreeRoleToTemplate_Click(object sender, EventArgs e)
    {
      List<RoleInfo> usedRoles = new List<RoleInfo>();
      this.gvRoles.Items.ToList<GVItem>().ForEach((Action<GVItem>) (item =>
      {
        if (item.Tag is EllieMae.EMLite.Workflow.Milestone)
          usedRoles.Add(this.getRoleByID(((EllieMae.EMLite.Workflow.Milestone) item.Tag).RoleID));
        if (!(item.Tag is RoleInfo))
          return;
        usedRoles.Add((RoleInfo) item.Tag);
      }));
      AddFreeRoleToTemplate freeRoleToTemplate = new AddFreeRoleToTemplate(this.session, ((IEnumerable<RoleInfo>) this.roles).Select<RoleInfo, RoleInfo>((Func<RoleInfo, RoleInfo>) (n => n)).Except<RoleInfo>((IEnumerable<RoleInfo>) usedRoles).ToList<RoleInfo>());
      int num = (int) freeRoleToTemplate.ShowDialog();
      if (freeRoleToTemplate.DialogResult != DialogResult.OK)
        return;
      this.setDirtyFlagValue(true);
      this.editFreeRoleTemplate = true;
      foreach (RoleInfo roleInfo in freeRoleToTemplate.RolesAdded)
        this.gvRoles.Items.Add(new GVItem(roleInfo.RoleName)
        {
          Tag = (object) roleInfo
        });
      this.gvRoles.Items[this.gvRoles.Items.Count - 1].Selected = true;
    }

    private void setDirtyFlagValue(bool value)
    {
      this.setDirtyFlag(value);
      this.btnSaveTemplate.Enabled = this.btnResetTemplate.Enabled = value;
    }

    private void btnRemoveFreeRole_Click(object sender, EventArgs e)
    {
      if (this.gvRoles.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a role to remove.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.gvRoles.Items.Remove(this.gvRoles.SelectedItems[0]);
        this.gvRoles.Items[0].Selected = true;
        this.editFreeRoleTemplate = true;
        this.setDirtyFlagValue(true);
      }
    }

    private void gvSequentialMilestones_DragDrop(object sender, DragEventArgs e)
    {
      Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
      int index = ((GVItem) e.Data.GetData(typeof (GVItem))).Index;
      GVItem gvItem = this.gvSequentialMilestones.Items[index];
      int rowIndex = this.gvSequentialMilestones.HitTest(client).RowIndex;
      if (e.Effect != DragDropEffects.Move || rowIndex == -1)
        return;
      if (rowIndex == index)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a milestone to its own position");
      }
      else if (rowIndex == this.gvSequentialMilestones.Items.Count - 1 || index == this.gvSequentialMilestones.Items.Count - 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot move the \"Completion\" milestone from its position.");
      }
      else if (rowIndex == 0 || index == 0)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "You cannot move the  \"Started\" milestone from its position.");
      }
      else
      {
        this.gvSequentialMilestones.Items.Remove(gvItem);
        this.gvSequentialMilestones.Items.Insert(rowIndex, gvItem);
        this.editMilestoneTemplate = true;
        this.setDirtyFlagValue(true);
      }
    }

    private void gvSequentialMilestones_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }

    private void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvRoles.Items.GetItemByTag((object) "Free Role Placeholder") == null)
        return;
      int index = this.gvRoles.Items.GetItemByTag((object) "Free Role Placeholder").Index;
      if (this.gvRoles.SelectedItems.Count <= 0)
        return;
      this.btnRemoveFreeRole.Enabled = this.gvRoles.SelectedItems[0].Index > index;
    }

    private EllieMae.EMLite.Workflow.Milestone getCustomSetting(string setting)
    {
      string currentSetting = string.Concat(this.session.ServerManager.GetServerSetting(setting));
      return currentSetting != "" ? this.allMilestones.ToList<EllieMae.EMLite.Workflow.Milestone>().First<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (item => item.MilestoneID == currentSetting)) : (EllieMae.EMLite.Workflow.Milestone) null;
    }

    private void setAutoLoanNumber(MilestoneTemplate template)
    {
      this.cmbLoanNumbering.SelectedIndexChanged -= new EventHandler(this.cmbLoanNumbering_SelectedIndexChanged);
      this.cmbLoanNumbering.PopulateAllMilestones((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) this.gvSequentialMilestones.Items.Select<GVItem, EllieMae.EMLite.Workflow.Milestone>((Func<GVItem, EllieMae.EMLite.Workflow.Milestone>) (gv => (EllieMae.EMLite.Workflow.Milestone) gv.Tag)).ToList<EllieMae.EMLite.Workflow.Milestone>(), true, false);
      this.cmbLoanNumbering.SetDefaultMilestoneID((string) this.defaultSettingsMT[(object) "POLICIES.LOANNUMBER"]);
      if (this.autoLoanNumberingID != "")
      {
        this.cmbLoanNumbering.MilestoneID = this.autoLoanNumberingID;
        this.lblAutoLoanNumber.ImageIndex = 1;
      }
      else if (this.customLoanNumber != null && this.gvSequentialMilestones.Items.ToList<GVItem>().FirstOrDefault<GVItem>((Func<GVItem, bool>) (item => ((EllieMae.EMLite.Workflow.Milestone) item.Tag).MilestoneID == this.customLoanNumber.MilestoneID)) != null)
      {
        this.cmbLoanNumbering.MilestoneID = this.customLoanNumber.MilestoneID;
        this.lblAutoLoanNumber.ImageIndex = 0;
      }
      else
      {
        this.cmbLoanNumbering.MilestoneID = (string) this.defaultSettingsMT[(object) "POLICIES.LOANNUMBER"];
        this.lblAutoLoanNumber.ImageIndex = 0;
      }
      this.cmbLoanNumbering.SelectedIndexChanged += new EventHandler(this.cmbLoanNumbering_SelectedIndexChanged);
    }

    private Point createEDisclosureUI()
    {
      List<EllieMae.EMLite.Workflow.Milestone> list = this.gvSequentialMilestones.Items.Select<GVItem, EllieMae.EMLite.Workflow.Milestone>((Func<GVItem, EllieMae.EMLite.Workflow.Milestone>) (gv => (EllieMae.EMLite.Workflow.Milestone) gv.Tag)).ToList<EllieMae.EMLite.Workflow.Milestone>();
      Point autoScrollPosition = this.tabSettings.AutoScrollPosition;
      this.tabSettings.AutoScrollPosition = new Point(0, 0);
      this.hideAllControls();
      if (this.setupContainer != null)
      {
        this.linkLabel1.Visible = this.label4.Visible = this.flowEDisclosure.Visible = true;
        if (!this.fillEDisclosureDefault((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) list))
        {
          this.linkLabel1.Visible = this.label4.Visible = this.flowEDisclosure.Visible = false;
          this.reSizeEdisControls();
          return new Point(0, 0);
        }
        this.linkLabel1.Visible = this.label4.Visible = this.flowEDisclosure.Visible = true;
      }
      foreach (KeyValuePair<string, string> valuepair in this.exceptionsTemplate)
        this.fillEDisclosureData(valuepair, (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) list);
      this.reSizeEdisControls();
      this.tabSettings.AutoScrollPosition = new Point(autoScrollPosition.X, Math.Abs(autoScrollPosition.Y));
      return autoScrollPosition;
    }

    private void cmbLoanNumbering_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.lblAutoLoanNumber.ImageIndex = 1;
      this.autoLoanNumberingID = this.cmbLoanNumbering.MilestoneID;
      this.editSettingsTemplate = true;
      this.setDirtyFlagValue(true);
    }

    private void linkAutoLoanNumber_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      this.setupContainer.TransferToAutoLoanNumbering();
    }

    private void gvSequentialMilestones_SizeChanged(object sender, EventArgs e)
    {
      this.gvSequentialMilestones.Columns[0].Width = Convert.ToInt32((double) this.gvSequentialMilestones.Width * 0.4);
      this.gvSequentialMilestones.Columns[1].Width = Convert.ToInt32((double) this.gvSequentialMilestones.Width * 0.4);
      this.gvSequentialMilestones.Columns[2].Width = Convert.ToInt32((double) this.gvSequentialMilestones.Width * 0.17);
    }

    private void panel3_SizeChanged(object sender, EventArgs e)
    {
      if (this.ParentForm != null && this.ParentForm.WindowState == FormWindowState.Minimized || this.panel3.Width >= 580)
        return;
      this.txtName.Width = this.panel3.Size.Width - 70;
      this.panelChannel.Width = this.panel3.Size.Width - 70;
      this.panelCondition.Width = this.panel3.Size.Width - 10;
    }

    private void panel4_SizeChanged(object sender, EventArgs e)
    {
      if (this.panel4.Height >= 340)
        return;
      this.txtComments.Height = this.panel4.Height - 80;
    }

    private bool fillEDisclosureDefault(IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList)
    {
      bool flag = false;
      if (this.setup.RetailChannel.InitialControl == ControlOptionType.Conditional)
      {
        if (this.setup.RetailChannel.ConditionalApplication.Enabled && this.setup.RetailChannel.ConditionalApplication.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlRAA.Visible = this.grpBankerRetail.Visible = true;
          this.assignValues(this.mdbRAA, this.setup.RetailChannel.ConditionalApplication.RequiredMilestone, milestoneList);
          this.lblRAA.ImageIndex = 0;
        }
        if (this.setup.RetailChannel.ConditionalThreeDay.Enabled && this.setup.RetailChannel.ConditionalThreeDay.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlRTD.Visible = this.grpBankerRetail.Visible = true;
          this.assignValues(this.mdbRTD, this.setup.RetailChannel.ConditionalThreeDay.RequiredMilestone, milestoneList);
          this.lblRTD.ImageIndex = 0;
        }
        if (this.setup.RetailChannel.ConditionalLock.Enabled && this.setup.RetailChannel.ConditionalLock.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlRAL.Visible = this.grpBankerRetail.Visible = true;
          this.assignValues(this.mdbRAL, this.setup.RetailChannel.ConditionalLock.RequiredMilestone, milestoneList);
          this.lblRAL.ImageIndex = 0;
        }
        if (this.setup.RetailChannel.ConditionalApproval.Enabled && this.setup.RetailChannel.ConditionalApproval.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlRA.Visible = this.grpBankerRetail.Visible = true;
          this.assignValues(this.mdbRA, this.setup.RetailChannel.ConditionalApproval.RequiredMilestone, milestoneList);
          this.lblRA.ImageIndex = 0;
        }
        flag = this.grpBankerRetail.Visible ? this.grpBankerRetail.Visible : flag;
      }
      if (this.setup.WholesaleChannel.InitialControl == ControlOptionType.Conditional)
      {
        if (this.setup.WholesaleChannel.ConditionalApplication.Enabled && this.setup.WholesaleChannel.ConditionalApplication.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlWAA.Visible = this.grpBankerWholesale.Visible = true;
          this.assignValues(this.mdbWAA, this.setup.WholesaleChannel.ConditionalApplication.RequiredMilestone, milestoneList);
          this.lblWAA.ImageIndex = 0;
        }
        if (this.setup.WholesaleChannel.ConditionalThreeDay.Enabled && this.setup.WholesaleChannel.ConditionalThreeDay.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlWTD.Visible = this.grpBankerWholesale.Visible = true;
          this.assignValues(this.mdbWTD, this.setup.WholesaleChannel.ConditionalThreeDay.RequiredMilestone, milestoneList);
          this.lblWTD.ImageIndex = 0;
        }
        if (this.setup.WholesaleChannel.ConditionalLock.Enabled && this.setup.WholesaleChannel.ConditionalLock.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlWAL.Visible = this.grpBankerWholesale.Visible = true;
          this.assignValues(this.mdbWAL, this.setup.WholesaleChannel.ConditionalLock.RequiredMilestone, milestoneList);
          this.lblWAL.ImageIndex = 0;
        }
        if (this.setup.WholesaleChannel.ConditionalApproval.Enabled && this.setup.WholesaleChannel.ConditionalApproval.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlWA.Visible = this.grpBankerWholesale.Visible = true;
          this.assignValues(this.mdbWA, this.setup.WholesaleChannel.ConditionalApproval.RequiredMilestone, milestoneList);
          this.lblWA.ImageIndex = 0;
        }
        flag = this.grpBankerWholesale.Visible ? this.grpBankerWholesale.Visible : flag;
      }
      if (this.setup.BrokerChannel.InitialControl == ControlOptionType.Conditional)
      {
        if (this.setup.BrokerChannel.ConditionalApplication.Enabled && this.setup.BrokerChannel.ConditionalApplication.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlBAA.Visible = this.grpBroker.Visible = true;
          this.assignValues(this.mdbBAA, this.setup.BrokerChannel.ConditionalApplication.RequiredMilestone, milestoneList);
          this.lblBAA.ImageIndex = 0;
        }
        if (this.setup.BrokerChannel.ConditionalThreeDay.Enabled && this.setup.BrokerChannel.ConditionalThreeDay.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlBTD.Visible = this.grpBroker.Visible = true;
          this.assignValues(this.mdbBTD, this.setup.BrokerChannel.ConditionalThreeDay.RequiredMilestone, milestoneList);
          this.lblBTD.ImageIndex = 0;
        }
        if (this.setup.BrokerChannel.ConditionalLock.Enabled && this.setup.BrokerChannel.ConditionalLock.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlBAL.Visible = this.grpBroker.Visible = true;
          this.assignValues(this.mdbBAL, this.setup.BrokerChannel.ConditionalLock.RequiredMilestone, milestoneList);
          this.lblBAL.ImageIndex = 0;
        }
        if (this.setup.BrokerChannel.ConditionalApproval.Enabled && this.setup.BrokerChannel.ConditionalApproval.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlBA.Visible = this.grpBroker.Visible = true;
          this.assignValues(this.mdbBA, this.setup.BrokerChannel.ConditionalApproval.RequiredMilestone, milestoneList);
          this.lblBA.ImageIndex = 0;
        }
        flag = this.grpBroker.Visible ? this.grpBroker.Visible : flag;
      }
      if (this.setup.CorrespondentChannel.InitialControl == ControlOptionType.Conditional)
      {
        if (this.setup.CorrespondentChannel.ConditionalApplication.Enabled && this.setup.CorrespondentChannel.ConditionalApplication.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlCAA.Visible = this.grpCorrespondent.Visible = true;
          this.assignValues(this.mdbCAA, this.setup.CorrespondentChannel.ConditionalApplication.RequiredMilestone, milestoneList);
          this.lblCAA.ImageIndex = 0;
        }
        if (this.setup.CorrespondentChannel.ConditionalThreeDay.Enabled && this.setup.CorrespondentChannel.ConditionalThreeDay.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlCTD.Visible = this.grpCorrespondent.Visible = true;
          this.assignValues(this.mdbCTD, this.setup.CorrespondentChannel.ConditionalThreeDay.RequiredMilestone, milestoneList);
          this.lblCTD.ImageIndex = 0;
        }
        if (this.setup.CorrespondentChannel.ConditionalLock.Enabled && this.setup.CorrespondentChannel.ConditionalLock.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlCAL.Visible = this.grpCorrespondent.Visible = true;
          this.assignValues(this.mdbCAL, this.setup.CorrespondentChannel.ConditionalLock.RequiredMilestone, milestoneList);
          this.lblCAL.ImageIndex = 0;
        }
        if (this.setup.CorrespondentChannel.ConditionalApproval.Enabled && this.setup.CorrespondentChannel.ConditionalApproval.RequirementType == PackageRequirementType.Milestone)
        {
          this.pnlCA.Visible = this.grpCorrespondent.Visible = true;
          this.assignValues(this.mdbCA, this.setup.CorrespondentChannel.ConditionalApproval.RequiredMilestone, milestoneList);
          this.lblCA.ImageIndex = 0;
        }
        flag = this.grpCorrespondent.Visible ? this.grpCorrespondent.Visible : flag;
      }
      return flag;
    }

    private void fillEDisclosureData(
      KeyValuePair<string, string> valuepair,
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList)
    {
      switch (valuepair.Key.Split('_')[0])
      {
        case "RetailChannel":
          if (this.setup.RetailChannel.InitialControl != ControlOptionType.Conditional)
            break;
          switch (valuepair.Key.Split('_')[1])
          {
            case "AtApplication":
              if (!this.setup.RetailChannel.ConditionalApplication.Enabled || this.setup.RetailChannel.ConditionalApplication.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlRAA.Visible = this.grpBankerRetail.Visible = true;
              this.assignValues(this.mdbRAA, valuepair.Value, milestoneList);
              this.lblRAA.ImageIndex = 1;
              return;
            case "ThreeDay":
              if (!this.setup.RetailChannel.ConditionalThreeDay.Enabled || this.setup.RetailChannel.ConditionalThreeDay.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlRTD.Visible = this.grpBankerRetail.Visible = true;
              this.assignValues(this.mdbRTD, valuepair.Value, milestoneList);
              this.lblRTD.ImageIndex = 1;
              return;
            case "AtLock":
              if (!this.setup.RetailChannel.ConditionalLock.Enabled || this.setup.RetailChannel.ConditionalLock.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlRAL.Visible = this.grpBankerRetail.Visible = true;
              this.assignValues(this.mdbRAL, valuepair.Value, milestoneList);
              this.lblRAL.ImageIndex = 1;
              return;
            case "Approval":
              if (!this.setup.RetailChannel.ConditionalApproval.Enabled || this.setup.RetailChannel.ConditionalApproval.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlRA.Visible = this.grpBankerRetail.Visible = true;
              this.assignValues(this.mdbRA, valuepair.Value, milestoneList);
              this.lblRA.ImageIndex = 1;
              return;
            default:
              return;
          }
        case "WholesaleChannel":
          if (this.setup.WholesaleChannel.InitialControl != ControlOptionType.Conditional)
            break;
          switch (valuepair.Key.Split('_')[1])
          {
            case "AtApplication":
              if (!this.setup.WholesaleChannel.ConditionalApplication.Enabled || this.setup.WholesaleChannel.ConditionalApplication.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlWAA.Visible = this.grpBankerWholesale.Visible = true;
              this.assignValues(this.mdbWAA, valuepair.Value, milestoneList);
              this.lblWAA.ImageIndex = 1;
              return;
            case "ThreeDay":
              if (!this.setup.WholesaleChannel.ConditionalThreeDay.Enabled || this.setup.WholesaleChannel.ConditionalThreeDay.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlWTD.Visible = this.grpBankerWholesale.Visible = true;
              this.assignValues(this.mdbWTD, valuepair.Value, milestoneList);
              this.lblWTD.ImageIndex = 1;
              return;
            case "AtLock":
              if (!this.setup.WholesaleChannel.ConditionalLock.Enabled || this.setup.WholesaleChannel.ConditionalLock.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlWAL.Visible = this.grpBankerWholesale.Visible = true;
              this.assignValues(this.mdbWAL, valuepair.Value, milestoneList);
              this.lblWAL.ImageIndex = 1;
              return;
            case "Approval":
              if (!this.setup.WholesaleChannel.ConditionalApproval.Enabled || this.setup.WholesaleChannel.ConditionalApproval.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlWA.Visible = this.grpBankerWholesale.Visible = true;
              this.assignValues(this.mdbWA, valuepair.Value, milestoneList);
              this.lblWA.ImageIndex = 1;
              return;
            default:
              return;
          }
        case "BrokerChannel":
          if (this.setup.BrokerChannel.InitialControl != ControlOptionType.Conditional)
            break;
          switch (valuepair.Key.Split('_')[1])
          {
            case "AtApplication":
              if (!this.setup.BrokerChannel.ConditionalApplication.Enabled || this.setup.BrokerChannel.ConditionalApplication.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlBAA.Visible = this.grpBroker.Visible = true;
              this.assignValues(this.mdbBAA, valuepair.Value, milestoneList);
              this.lblBAA.ImageIndex = 1;
              return;
            case "ThreeDay":
              if (!this.setup.BrokerChannel.ConditionalThreeDay.Enabled || this.setup.BrokerChannel.ConditionalThreeDay.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlBTD.Visible = this.grpBroker.Visible = true;
              this.assignValues(this.mdbBTD, valuepair.Value, milestoneList);
              this.lblBTD.ImageIndex = 1;
              return;
            case "AtLock":
              if (!this.setup.BrokerChannel.ConditionalLock.Enabled || this.setup.BrokerChannel.ConditionalLock.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlBAL.Visible = this.grpBroker.Visible = true;
              this.assignValues(this.mdbBAL, valuepair.Value, milestoneList);
              this.lblBAL.ImageIndex = 1;
              return;
            case "Approval":
              if (!this.setup.BrokerChannel.ConditionalApproval.Enabled || this.setup.BrokerChannel.ConditionalApproval.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlBA.Visible = this.grpBroker.Visible = true;
              this.assignValues(this.mdbBA, valuepair.Value, milestoneList);
              this.lblBA.ImageIndex = 1;
              return;
            default:
              return;
          }
        case "CorrespondentChannel":
          if (this.setup.CorrespondentChannel.InitialControl != ControlOptionType.Conditional)
            break;
          switch (valuepair.Key.Split('_')[1])
          {
            case "AtApplication":
              if (!this.setup.CorrespondentChannel.ConditionalApplication.Enabled || this.setup.CorrespondentChannel.ConditionalApplication.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlCAA.Visible = this.grpCorrespondent.Visible = true;
              this.assignValues(this.mdbCAA, valuepair.Value, milestoneList);
              this.lblCAA.ImageIndex = 1;
              return;
            case "ThreeDay":
              if (!this.setup.CorrespondentChannel.ConditionalThreeDay.Enabled || this.setup.CorrespondentChannel.ConditionalThreeDay.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlCTD.Visible = this.grpCorrespondent.Visible = true;
              this.assignValues(this.mdbCTD, valuepair.Value, milestoneList);
              this.lblCTD.ImageIndex = 1;
              return;
            case "AtLock":
              if (!this.setup.CorrespondentChannel.ConditionalLock.Enabled || this.setup.CorrespondentChannel.ConditionalLock.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlCAL.Visible = this.grpCorrespondent.Visible = true;
              this.assignValues(this.mdbCAL, valuepair.Value, milestoneList);
              this.lblCAL.ImageIndex = 1;
              return;
            case "Approval":
              if (!this.setup.CorrespondentChannel.ConditionalApproval.Enabled || this.setup.CorrespondentChannel.ConditionalApproval.RequirementType != PackageRequirementType.Milestone)
                return;
              this.pnlCA.Visible = this.grpCorrespondent.Visible = true;
              this.assignValues(this.mdbCA, valuepair.Value, milestoneList);
              this.lblCA.ImageIndex = 1;
              return;
            default:
              return;
          }
      }
    }

    private void assignValues(
      MilestoneDropdownBox c,
      string value,
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList)
    {
      c.PopulateAllMilestones(milestoneList, true, false);
      c.SetDefaultMilestoneID((string) this.defaultSettingsMT[(object) "EDISCLOSURE"]);
      c.SelectedIndexChanged -= new EventHandler(this.eDis_SelectedIndexChanged);
      c.MilestoneID = value;
      if (c.MilestoneID == null)
        c.MilestoneID = (string) this.defaultSettingsMT[(object) "EDISCLOSURE"];
      c.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
    }

    private void hideAllControls()
    {
      this.grpBankerRetail.Visible = this.grpBankerWholesale.Visible = this.grpBroker.Visible = this.grpCorrespondent.Visible = this.pnlRAA.Visible = this.pnlRTD.Visible = this.pnlRAL.Visible = this.pnlRA.Visible = this.pnlWAA.Visible = this.pnlWTD.Visible = this.pnlWAL.Visible = this.pnlWA.Visible = this.pnlBAA.Visible = this.pnlBTD.Visible = this.pnlBAL.Visible = this.pnlBA.Visible = this.pnlCAA.Visible = this.pnlCTD.Visible = this.pnlCAL.Visible = this.pnlCA.Visible = false;
    }

    private void reSizeEdisControls()
    {
      Point point = new Point(15, 32);
      Size size = new Size(this.flowEDisclosure.Size.Width, 60);
      foreach (Control control1 in (ArrangedElementCollection) this.flowEDisclosure.Controls)
      {
        if (control1 is GroupContainer)
        {
          foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (control2 is Panel && control2.Visible)
            {
              control2.Location = point;
              point.Y += 25;
            }
          }
          control1.Size = new Size(control1.Size.Width, point.Y + 5);
          size.Height += control1.Size.Height;
        }
        point = new Point(15, 32);
      }
      this.flowEDisclosure.Size = size;
      if (this.flowEDisclosure.Size.Height < this.tabSettings.Size.Height - 62)
      {
        this.lblLinkedWithPersona.Location = new Point(this.lblLinkedWithPersona.Location.X, this.tabSettings.Size.Height - 40);
        this.lblDisconnectedFromPersona.Location = new Point(this.lblDisconnectedFromPersona.Location.X, this.lblLinkedWithPersona.Location.Y + 15);
      }
      else
      {
        this.lblLinkedWithPersona.Location = new Point(this.lblLinkedWithPersona.Location.X, this.flowEDisclosure.Size.Height + 50);
        this.lblDisconnectedFromPersona.Location = new Point(this.lblDisconnectedFromPersona.Location.X, this.lblLinkedWithPersona.Location.Y + 15);
      }
      this.lblDisconnectedFromPersona.BringToFront();
      this.lblLinkedWithPersona.BringToFront();
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (!this.validateTemplateChanges())
        return;
      this.setupContainer.TransferToEDisclosurePackages();
    }

    private void eDis_SelectedIndexChanged(object sender, EventArgs e)
    {
      string key = ((Control) sender).Tag.ToString();
      foreach (Control control in (ArrangedElementCollection) ((Control) sender).Parent.Controls)
      {
        if (control is Label)
          ((Label) control).ImageIndex = 1;
      }
      Dictionary<MilestoneTemplate, string> dictionary = new Dictionary<MilestoneTemplate, string>();
      dictionary.Add(this.currentTemplate, ((MilestoneDropdownBox) sender).MilestoneID);
      if (this.exceptionsList.ContainsKey(key))
        this.exceptionsList[key] = dictionary;
      else
        this.exceptionsList.Add(key, dictionary);
      if (this.exceptionsTemplate.ContainsKey(key))
        this.exceptionsTemplate[key] = ((MilestoneDropdownBox) sender).MilestoneID;
      else
        this.exceptionsTemplate.Add(key, ((MilestoneDropdownBox) sender).MilestoneID);
      this.editeDisSettingsTemplate = true;
      this.setDirtyFlagValue(true);
    }

    private void brokerVersion()
    {
      this.lblglobalSettings.Text = "Use the options below to manage the milestones included in this template, set up trigger milestones, and add roles to the template.";
      this.label7.Text = "";
      this.lblglobalSettings.Location = this.label7.Location;
      this.btnAddTemplate.Visible = this.btnDuplicateTemplate.Visible = this.btnMoveTemplateDown.Visible = this.btnMoveTemplateUp.Visible = this.btnDeleteTemplate.Visible = this.btnStatus.Visible = false;
      this.btnAddTemplate.Enabled = this.btnDuplicateTemplate.Enabled = this.btnMoveTemplateDown.Enabled = this.btnMoveTemplateUp.Enabled = this.btnDeleteTemplate.Enabled = this.btnStatus.Enabled = false;
      this.tabTemplates.Controls.Remove((Control) this.tabSettings);
      this.flowLayoutPanel4.Size = new Size(170, 22);
      this.flowLayoutPanel4.Location = new Point(1020, 1);
    }

    private void brokerVersionSaveMilestone(bool edit)
    {
      if (this.gvMilestones.SelectedItems.Count == 0)
        return;
      EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.SelectedItems[0].Tag;
      MilestoneTemplate milestoneTemplate = this.session.SessionObjects.BpmManager.GetDefaultMilestoneTemplate();
      FieldRuleInfo templateConditions = this.session.SessionObjects.BpmManager.GetMilestoneTemplateConditions(milestoneTemplate.TemplateID);
      if (!edit)
      {
        milestoneTemplate.SequentialMilestones.InsertInBetween(this.gvMilestones.SelectedItems[0].Index, tag);
        if (tag.RoleID != -1 && milestoneTemplate.ContainsRole(tag.RoleID))
          milestoneTemplate.FreeRoles.Remove(milestoneTemplate.FreeRoles.GetFreeRole(tag.RoleID));
      }
      else if (milestoneTemplate.Contains(tag.MilestoneID))
        milestoneTemplate.SequentialMilestones.GetMilestone(tag.MilestoneID).DaysToComplete = tag.DefaultDays;
      this.session.SessionObjects.BpmManager.UpdateMilestoneTemplate(milestoneTemplate, (BizRuleInfo) templateConditions);
      this.updateBizRule();
    }

    public bool IsOnMilestoneConfigurationTab
    {
      get => this.tabMilestones.SelectedTab == this.tpMilestones;
    }

    public string[] SelectedMilestones
    {
      get
      {
        List<string> selectedMilestones = new List<string>();
        this.gvMilestones.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => selectedMilestones.Add(((EllieMae.EMLite.Workflow.Milestone) x.Tag).Name)));
        return selectedMilestones.ToArray();
      }
      set
      {
        this.tabMilestones.SelectedTab = this.tpMilestones;
        this.gvMilestones.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => x.Selected = false));
        ((IEnumerable<string>) value).ToList<string>().ForEach((Action<string>) (selectedName =>
        {
          GVItem gvItem1 = this.gvMilestones.Items.FirstOrDefault<GVItem>((Func<GVItem, bool>) (gvItem => ((EllieMae.EMLite.Workflow.Milestone) gvItem.Tag).Name == selectedName));
          if (gvItem1 == null)
            return;
          gvItem1.Selected = true;
        }));
      }
    }

    public string[] SelectedMilestoneTemplates
    {
      get
      {
        List<string> selectedMilestoneTemplates = new List<string>();
        this.gvTemplates.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => selectedMilestoneTemplates.Add(((MilestoneTemplate) x.Tag).Name)));
        return selectedMilestoneTemplates.ToArray();
      }
      set
      {
        this.tabMilestones.SelectedTab = this.tpTemplates;
        this.gvTemplates.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => x.Selected = false));
        ((IEnumerable<string>) value).ToList<string>().ForEach((Action<string>) (selectedName =>
        {
          GVItem gvItem1 = this.gvTemplates.Items.FirstOrDefault<GVItem>((Func<GVItem, bool>) (gvItem => ((MilestoneTemplate) gvItem.Tag).Name == selectedName));
          if (gvItem1 == null)
            return;
          gvItem1.Selected = true;
        }));
      }
    }

    private void tabSettings_Enter(object sender, EventArgs e)
    {
      this.flowEDisclosure.Parent.Visible = true;
      this.createEDisclosureUI();
    }

    private void gvChannels_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.changesMade(source, (EventArgs) e);
    }

    private void rebuildMilestoneSessionInfo()
    {
      this.session.StartupInfo.Milestones = (List<EllieMae.EMLite.Workflow.Milestone>) this.session.SessionObjects.BpmManager.GetMilestones(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneSettingsPanel));
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      this.tabMilestones = new TabControl();
      this.tpMilestones = new TabPage();
      this.gcMilestones = new GroupContainer();
      this.rdbArchived = new RadioButton();
      this.rdbCurrent = new RadioButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnNewMilestone = new StandardIconButton();
      this.btnEditMilestone = new StandardIconButton();
      this.btnMoveMilestoneUp = new StandardIconButton();
      this.btnMoveMilestoneDown = new StandardIconButton();
      this.gvMilestones = new GridView();
      this.btnArchive = new Button();
      this.tpTemplates = new TabPage();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.grpTemplates = new GroupContainer();
      this.groupTemplateDetails = new GroupContainer();
      this.tabTemplates = new TabControl();
      this.tabDetails = new TabPage();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel3 = new Panel();
      this.label1 = new Label();
      this.label6 = new Label();
      this.label2 = new Label();
      this.txtName = new TextBox();
      this.panelChannel = new Panel();
      this.gvChannels = new GridView();
      this.panelCondition = new Panel();
      this.panel4 = new Panel();
      this.label3 = new Label();
      this.txtComments = new TextBox();
      this.tabMilestoneDetails = new TabPage();
      this.groupContainer1 = new GroupContainer();
      this.gvSequentialMilestones = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.stdRemoveMilestone = new StandardIconButton();
      this.btnMoveTemplateMilestoneUp = new StandardIconButton();
      this.btnMoveTemplateMilestoneDown = new StandardIconButton();
      this.stdAddMilestoneToTemplate = new StandardIconButton();
      this.tabSettings = new TabPage();
      this.label5 = new Label();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.label4 = new Label();
      this.imageList1 = new ImageList(this.components);
      this.flowEDisclosure = new FlowLayoutPanel();
      this.grpBankerRetail = new GroupContainer();
      this.pnlRAA = new Panel();
      this.lblRAA = new Label();
      this.mdbRAA = new MilestoneDropdownBox();
      this.pnlRTD = new Panel();
      this.lblRTD = new Label();
      this.mdbRTD = new MilestoneDropdownBox();
      this.pnlRAL = new Panel();
      this.lblRAL = new Label();
      this.mdbRAL = new MilestoneDropdownBox();
      this.pnlRA = new Panel();
      this.lblRA = new Label();
      this.mdbRA = new MilestoneDropdownBox();
      this.grpBankerWholesale = new GroupContainer();
      this.pnlWAA = new Panel();
      this.mdbWAA = new MilestoneDropdownBox();
      this.lblWAA = new Label();
      this.pnlWTD = new Panel();
      this.mdbWTD = new MilestoneDropdownBox();
      this.lblWTD = new Label();
      this.pnlWAL = new Panel();
      this.mdbWAL = new MilestoneDropdownBox();
      this.lblWAL = new Label();
      this.pnlWA = new Panel();
      this.mdbWA = new MilestoneDropdownBox();
      this.lblWA = new Label();
      this.grpBroker = new GroupContainer();
      this.pnlBAA = new Panel();
      this.mdbBAA = new MilestoneDropdownBox();
      this.lblBAA = new Label();
      this.pnlBTD = new Panel();
      this.mdbBTD = new MilestoneDropdownBox();
      this.lblBTD = new Label();
      this.pnlBAL = new Panel();
      this.mdbBAL = new MilestoneDropdownBox();
      this.lblBAL = new Label();
      this.pnlBA = new Panel();
      this.mdbBA = new MilestoneDropdownBox();
      this.lblBA = new Label();
      this.grpCorrespondent = new GroupContainer();
      this.pnlCAA = new Panel();
      this.mdbCAA = new MilestoneDropdownBox();
      this.lblCAA = new Label();
      this.pnlCTD = new Panel();
      this.mdbCTD = new MilestoneDropdownBox();
      this.lblCTD = new Label();
      this.pnlCAL = new Panel();
      this.mdbCAL = new MilestoneDropdownBox();
      this.lblCAL = new Label();
      this.pnlCA = new Panel();
      this.mdbCA = new MilestoneDropdownBox();
      this.lblCA = new Label();
      this.linkAutoLoanNumber = new System.Windows.Forms.LinkLabel();
      this.lblDisconnectedFromPersona = new Label();
      this.lblLinkedWithPersona = new Label();
      this.cmbLoanNumbering = new MilestoneDropdownBox();
      this.lblAutoLoanNumber = new Label();
      this.tabRoles = new TabPage();
      this.groupContainer2 = new GroupContainer();
      this.flowLayoutPanel5 = new FlowLayoutPanel();
      this.btnRemoveFreeRole = new StandardIconButton();
      this.stdAddFreeRoleToTemplate = new StandardIconButton();
      this.gvRoles = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnResetTemplate = new StandardIconButton();
      this.btnSaveTemplate = new StandardIconButton();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.flowLayoutPanel4 = new FlowLayoutPanel();
      this.btnAddTemplate = new StandardIconButton();
      this.btnDuplicateTemplate = new StandardIconButton();
      this.btnMoveTemplateDown = new StandardIconButton();
      this.btnMoveTemplateUp = new StandardIconButton();
      this.btnDeleteTemplate = new StandardIconButton();
      this.btnStatus = new Button();
      this.btnGlobalSettings = new Button();
      this.gvTemplates = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.label7 = new Label();
      this.lblglobalSettings = new Label();
      this.verticalSeparator2 = new VerticalSeparator();
      this.toolTip1 = new ToolTip(this.components);
      this.tabMilestones.SuspendLayout();
      this.tpMilestones.SuspendLayout();
      this.gcMilestones.SuspendLayout();
      ((ISupportInitialize) this.btnNewMilestone).BeginInit();
      ((ISupportInitialize) this.btnEditMilestone).BeginInit();
      ((ISupportInitialize) this.btnMoveMilestoneUp).BeginInit();
      ((ISupportInitialize) this.btnMoveMilestoneDown).BeginInit();
      this.tpTemplates.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grpTemplates.SuspendLayout();
      this.groupTemplateDetails.SuspendLayout();
      this.tabTemplates.SuspendLayout();
      this.tabDetails.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panelChannel.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tabMilestoneDetails.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdRemoveMilestone).BeginInit();
      ((ISupportInitialize) this.btnMoveTemplateMilestoneUp).BeginInit();
      ((ISupportInitialize) this.btnMoveTemplateMilestoneDown).BeginInit();
      ((ISupportInitialize) this.stdAddMilestoneToTemplate).BeginInit();
      this.tabSettings.SuspendLayout();
      this.flowEDisclosure.SuspendLayout();
      this.grpBankerRetail.SuspendLayout();
      this.pnlRAA.SuspendLayout();
      this.pnlRTD.SuspendLayout();
      this.pnlRAL.SuspendLayout();
      this.pnlRA.SuspendLayout();
      this.grpBankerWholesale.SuspendLayout();
      this.pnlWAA.SuspendLayout();
      this.pnlWTD.SuspendLayout();
      this.pnlWAL.SuspendLayout();
      this.pnlWA.SuspendLayout();
      this.grpBroker.SuspendLayout();
      this.pnlBAA.SuspendLayout();
      this.pnlBTD.SuspendLayout();
      this.pnlBAL.SuspendLayout();
      this.pnlBA.SuspendLayout();
      this.grpCorrespondent.SuspendLayout();
      this.pnlCAA.SuspendLayout();
      this.pnlCTD.SuspendLayout();
      this.pnlCAL.SuspendLayout();
      this.pnlCA.SuspendLayout();
      this.tabRoles.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.flowLayoutPanel5.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveFreeRole).BeginInit();
      ((ISupportInitialize) this.stdAddFreeRoleToTemplate).BeginInit();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnResetTemplate).BeginInit();
      ((ISupportInitialize) this.btnSaveTemplate).BeginInit();
      this.flowLayoutPanel4.SuspendLayout();
      ((ISupportInitialize) this.btnAddTemplate).BeginInit();
      ((ISupportInitialize) this.btnDuplicateTemplate).BeginInit();
      ((ISupportInitialize) this.btnMoveTemplateDown).BeginInit();
      ((ISupportInitialize) this.btnMoveTemplateUp).BeginInit();
      ((ISupportInitialize) this.btnDeleteTemplate).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tabMilestones.Controls.Add((Control) this.tpMilestones);
      this.tabMilestones.Controls.Add((Control) this.tpTemplates);
      this.tabMilestones.Dock = DockStyle.Fill;
      this.tabMilestones.ItemSize = new Size(100, 28);
      this.tabMilestones.Location = new Point(0, 0);
      this.tabMilestones.Name = "tabMilestones";
      this.tabMilestones.Padding = new Point(11, 3);
      this.tabMilestones.SelectedIndex = 0;
      this.tabMilestones.ShowToolTips = true;
      this.tabMilestones.Size = new Size(1200, 978);
      this.tabMilestones.TabIndex = 0;
      this.tabMilestones.SelectedIndexChanged += new EventHandler(this.tabMilestones_SelectedIndexChanged);
      this.tabMilestones.Deselecting += new TabControlCancelEventHandler(this.tabMilestones_Deselecting);
      this.tpMilestones.Controls.Add((Control) this.gcMilestones);
      this.tpMilestones.Location = new Point(4, 32);
      this.tpMilestones.Name = "tpMilestones";
      this.tpMilestones.Padding = new Padding(0, 1, 1, 1);
      this.tpMilestones.Size = new Size(1192, 942);
      this.tpMilestones.TabIndex = 0;
      this.tpMilestones.Text = "Milestones";
      this.tpMilestones.UseVisualStyleBackColor = true;
      this.gcMilestones.Controls.Add((Control) this.rdbArchived);
      this.gcMilestones.Controls.Add((Control) this.rdbCurrent);
      this.gcMilestones.Controls.Add((Control) this.verticalSeparator1);
      this.gcMilestones.Controls.Add((Control) this.btnNewMilestone);
      this.gcMilestones.Controls.Add((Control) this.btnEditMilestone);
      this.gcMilestones.Controls.Add((Control) this.btnMoveMilestoneUp);
      this.gcMilestones.Controls.Add((Control) this.btnMoveMilestoneDown);
      this.gcMilestones.Controls.Add((Control) this.gvMilestones);
      this.gcMilestones.Controls.Add((Control) this.btnArchive);
      this.gcMilestones.Dock = DockStyle.Fill;
      this.gcMilestones.HeaderForeColor = SystemColors.ControlText;
      this.gcMilestones.Location = new Point(0, 1);
      this.gcMilestones.Name = "gcMilestones";
      this.gcMilestones.Size = new Size(1191, 940);
      this.gcMilestones.TabIndex = 37;
      this.rdbArchived.AutoSize = true;
      this.rdbArchived.BackColor = Color.Transparent;
      this.rdbArchived.Location = new Point(131, 6);
      this.rdbArchived.Name = "rdbArchived";
      this.rdbArchived.Size = new Size(123, 18);
      this.rdbArchived.TabIndex = 40;
      this.rdbArchived.Text = "Archived Milestones";
      this.rdbArchived.UseVisualStyleBackColor = false;
      this.rdbArchived.CheckedChanged += new EventHandler(this.CurrentArchived_CheckedChanged);
      this.rdbCurrent.AutoSize = true;
      this.rdbCurrent.BackColor = Color.Transparent;
      this.rdbCurrent.Checked = true;
      this.rdbCurrent.Location = new Point(10, 6);
      this.rdbCurrent.Name = "rdbCurrent";
      this.rdbCurrent.Size = new Size(115, 18);
      this.rdbCurrent.TabIndex = 39;
      this.rdbCurrent.TabStop = true;
      this.rdbCurrent.Text = "Current Milestones";
      this.rdbCurrent.UseVisualStyleBackColor = false;
      this.rdbCurrent.CheckedChanged += new EventHandler(this.CurrentArchived_CheckedChanged);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(1105, 6);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 38;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnNewMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewMilestone.BackColor = Color.Transparent;
      this.btnNewMilestone.Location = new Point(1017, 5);
      this.btnNewMilestone.MouseDownImage = (Image) null;
      this.btnNewMilestone.Name = "btnNewMilestone";
      this.btnNewMilestone.Size = new Size(16, 17);
      this.btnNewMilestone.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewMilestone.TabIndex = 37;
      this.btnNewMilestone.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNewMilestone, "Add Milestone");
      this.btnNewMilestone.Click += new EventHandler(this.btnNewMilestone_Click);
      this.btnEditMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditMilestone.BackColor = Color.Transparent;
      this.btnEditMilestone.Enabled = false;
      this.btnEditMilestone.Location = new Point(1039, 5);
      this.btnEditMilestone.MouseDownImage = (Image) null;
      this.btnEditMilestone.Name = "btnEditMilestone";
      this.btnEditMilestone.Size = new Size(16, 17);
      this.btnEditMilestone.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditMilestone.TabIndex = 36;
      this.btnEditMilestone.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditMilestone, "Edit Milestone");
      this.btnEditMilestone.Click += new EventHandler(this.btnEditMilestone_Click);
      this.btnMoveMilestoneUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveMilestoneUp.BackColor = Color.Transparent;
      this.btnMoveMilestoneUp.Enabled = false;
      this.btnMoveMilestoneUp.Location = new Point(1061, 5);
      this.btnMoveMilestoneUp.MouseDownImage = (Image) null;
      this.btnMoveMilestoneUp.Name = "btnMoveMilestoneUp";
      this.btnMoveMilestoneUp.Size = new Size(16, 17);
      this.btnMoveMilestoneUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveMilestoneUp.TabIndex = 35;
      this.btnMoveMilestoneUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveMilestoneUp, "Move Milestone Up");
      this.btnMoveMilestoneUp.Click += new EventHandler(this.btnMoveMilestoneUp_Click);
      this.btnMoveMilestoneDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveMilestoneDown.BackColor = Color.Transparent;
      this.btnMoveMilestoneDown.Enabled = false;
      this.btnMoveMilestoneDown.Location = new Point(1083, 5);
      this.btnMoveMilestoneDown.MouseDownImage = (Image) null;
      this.btnMoveMilestoneDown.Name = "btnMoveMilestoneDown";
      this.btnMoveMilestoneDown.Size = new Size(16, 17);
      this.btnMoveMilestoneDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveMilestoneDown.TabIndex = 34;
      this.btnMoveMilestoneDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveMilestoneDown, "Move Milestone Down");
      this.btnMoveMilestoneDown.Click += new EventHandler(this.btnMoveMilestoneDown_Click);
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.AllowMultiselect = false;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 282;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "Column16";
      gvColumn17.Text = "TPO Connect Status";
      gvColumn17.Width = 176;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "consumerStatus";
      gvColumn18.Text = "Consumer Status";
      gvColumn18.Width = 176;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column4";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 176;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column5";
      gvColumn3.Text = "Field Summary Form";
      gvColumn3.Width = 199;
      this.gvMilestones.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn17,
        gvColumn18,
        gvColumn2,
        gvColumn3
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.HotItemTracking = false;
      this.gvMilestones.Location = new Point(1, 26);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(1189, 913);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 19;
      this.gvMilestones.SelectedIndexChanged += new EventHandler(this.gvMilestones_SelectedIndexChanged);
      this.gvMilestones.ItemDoubleClick += new GVItemEventHandler(this.gvMilestones_ItemDoubleClick);
      this.gvMilestones.ItemDrag += new GVItemEventHandler(this.gvMilestones_ItemDrag);
      this.gvMilestones.DragDrop += new DragEventHandler(this.gvMilestones_DragDrop);
      this.gvMilestones.DragEnter += new DragEventHandler(this.gvMilestones_DragEnter);
      this.btnArchive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnArchive.AutoSize = true;
      this.btnArchive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnArchive.BackColor = SystemColors.Control;
      this.btnArchive.Enabled = false;
      this.btnArchive.Location = new Point(1111, 2);
      this.btnArchive.MinimumSize = new Size(73, 23);
      this.btnArchive.Name = "btnArchive";
      this.btnArchive.Size = new Size(73, 24);
      this.btnArchive.TabIndex = 33;
      this.btnArchive.Text = "&Archive";
      this.btnArchive.UseVisualStyleBackColor = true;
      this.btnArchive.Click += new EventHandler(this.btnArchive_Click);
      this.tpTemplates.Controls.Add((Control) this.panel1);
      this.tpTemplates.Location = new Point(4, 32);
      this.tpTemplates.Name = "tpTemplates";
      this.tpTemplates.Padding = new Padding(0, 1, 1, 1);
      this.tpTemplates.Size = new Size(1192, 942);
      this.tpTemplates.TabIndex = 1;
      this.tpTemplates.Text = "Milestone Templates";
      this.tpTemplates.UseVisualStyleBackColor = true;
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 1);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1191, 940);
      this.panel1.TabIndex = 40;
      this.panel2.Controls.Add((Control) this.grpTemplates);
      this.panel2.Controls.Add((Control) this.gradientPanel1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1191, 940);
      this.panel2.TabIndex = 1;
      this.grpTemplates.AutoScroll = true;
      this.grpTemplates.AutoSize = true;
      this.grpTemplates.Controls.Add((Control) this.groupTemplateDetails);
      this.grpTemplates.Controls.Add((Control) this.collapsibleSplitter1);
      this.grpTemplates.Controls.Add((Control) this.flowLayoutPanel4);
      this.grpTemplates.Controls.Add((Control) this.gvTemplates);
      this.grpTemplates.Dock = DockStyle.Fill;
      this.grpTemplates.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplates.Location = new Point(0, 31);
      this.grpTemplates.Name = "grpTemplates";
      this.grpTemplates.Size = new Size(1191, 909);
      this.grpTemplates.TabIndex = 38;
      this.grpTemplates.Text = "Templates";
      this.groupTemplateDetails.Controls.Add((Control) this.tabTemplates);
      this.groupTemplateDetails.Controls.Add((Control) this.flowLayoutPanel2);
      this.groupTemplateDetails.Dock = DockStyle.Fill;
      this.groupTemplateDetails.HeaderForeColor = SystemColors.ControlText;
      this.groupTemplateDetails.Location = new Point(1, 183);
      this.groupTemplateDetails.Name = "groupTemplateDetails";
      this.groupTemplateDetails.Size = new Size(1189, 725);
      this.groupTemplateDetails.TabIndex = 4;
      this.groupTemplateDetails.Text = "Template Information";
      this.tabTemplates.Controls.Add((Control) this.tabDetails);
      this.tabTemplates.Controls.Add((Control) this.tabMilestoneDetails);
      this.tabTemplates.Controls.Add((Control) this.tabSettings);
      this.tabTemplates.Controls.Add((Control) this.tabRoles);
      this.tabTemplates.Dock = DockStyle.Fill;
      this.tabTemplates.Location = new Point(1, 26);
      this.tabTemplates.Name = "tabTemplates";
      this.tabTemplates.SelectedIndex = 0;
      this.tabTemplates.Size = new Size(1187, 698);
      this.tabTemplates.TabIndex = 2;
      this.tabDetails.Controls.Add((Control) this.tableLayoutPanel1);
      this.tabDetails.Location = new Point(4, 23);
      this.tabDetails.Name = "tabDetails";
      this.tabDetails.Padding = new Padding(3);
      this.tabDetails.Size = new Size(1179, 671);
      this.tabDetails.TabIndex = 0;
      this.tabDetails.Text = "Details";
      this.tabDetails.UseVisualStyleBackColor = true;
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.AutoScrollMinSize = new Size(900, 280);
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Controls.Add((Control) this.panel3, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel4, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(3, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Size = new Size(1173, 665);
      this.tableLayoutPanel1.TabIndex = 51;
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Controls.Add((Control) this.label6);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Controls.Add((Control) this.txtName);
      this.panel3.Controls.Add((Control) this.panelChannel);
      this.panel3.Controls.Add((Control) this.panelCondition);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(3, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(580, 659);
      this.panel3.TabIndex = 0;
      this.panel3.SizeChanged += new EventHandler(this.panel3_SizeChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 13);
      this.label1.TabIndex = 45;
      this.label1.Text = "Template Name";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(8, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(194, 13);
      this.label6.TabIndex = 47;
      this.label6.Text = "Channels this template applies to";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 183);
      this.label2.Name = "label2";
      this.label2.Size = new Size(161, 13);
      this.label2.TabIndex = 46;
      this.label2.Text = "Conditions for this template";
      this.txtName.Location = new Point(8, 28);
      this.txtName.MaxLength = 38;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(511, 20);
      this.txtName.TabIndex = 43;
      this.txtName.TextChanged += new EventHandler(this.changesMade);
      this.panelChannel.Controls.Add((Control) this.gvChannels);
      this.panelChannel.Location = new Point(8, 78);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(511, 95);
      this.panelChannel.TabIndex = 48;
      this.gvChannels.AlternatingColors = false;
      gvColumn4.CheckBoxes = true;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Column";
      gvColumn4.Width = 200;
      this.gvChannels.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gvChannels.Dock = DockStyle.Fill;
      this.gvChannels.GridLines = GVGridLines.None;
      this.gvChannels.HeaderHeight = 0;
      this.gvChannels.HeaderVisible = false;
      this.gvChannels.Location = new Point(0, 0);
      this.gvChannels.Name = "gvChannels";
      this.gvChannels.Size = new Size(511, 95);
      this.gvChannels.TabIndex = 0;
      this.gvChannels.SubItemCheck += new GVSubItemEventHandler(this.gvChannels_SubItemCheck);
      this.panelCondition.Location = new Point(8, 200);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(500, 92);
      this.panelCondition.TabIndex = 44;
      this.panel4.Controls.Add((Control) this.label3);
      this.panel4.Controls.Add((Control) this.txtComments);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(589, 3);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(581, 659);
      this.panel4.TabIndex = 1;
      this.panel4.SizeChanged += new EventHandler(this.panel4_SizeChanged);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(14, 12);
      this.label3.Name = "label3";
      this.label3.Size = new Size(103, 13);
      this.label3.TabIndex = 49;
      this.label3.Text = "Notes/Comments";
      this.txtComments.Location = new Point(14, 31);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Both;
      this.txtComments.Size = new Size(400, 252);
      this.txtComments.TabIndex = 50;
      this.txtComments.TextChanged += new EventHandler(this.changesMade);
      this.tabMilestoneDetails.Controls.Add((Control) this.groupContainer1);
      this.tabMilestoneDetails.Location = new Point(4, 23);
      this.tabMilestoneDetails.Name = "tabMilestoneDetails";
      this.tabMilestoneDetails.Size = new Size(1179, 675);
      this.tabMilestoneDetails.TabIndex = 1;
      this.tabMilestoneDetails.Text = "Milestones";
      this.tabMilestoneDetails.UseVisualStyleBackColor = true;
      this.groupContainer1.AutoScroll = true;
      this.groupContainer1.Controls.Add((Control) this.gvSequentialMilestones);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1179, 675);
      this.groupContainer1.TabIndex = 9;
      this.groupContainer1.Text = "Milestone Template";
      this.gvSequentialMilestones.AllowDrop = true;
      this.gvSequentialMilestones.AllowMultiselect = false;
      this.gvSequentialMilestones.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Milestone";
      gvColumn5.Width = 326;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column17";
      gvColumn8.Text = "TPO Connect Status";
      gvColumn8.Width = 146;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.Text = "Role";
      gvColumn6.Width = 200;
      gvColumn7.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column3";
      gvColumn7.Text = "Days";
      gvColumn7.Width = 100;
      this.gvSequentialMilestones.Columns.AddRange(new GVColumn[4]
      {
        gvColumn5,
        gvColumn8,
        gvColumn6,
        gvColumn7
      });
      this.gvSequentialMilestones.Dock = DockStyle.Fill;
      this.gvSequentialMilestones.DropTarget = GVDropTarget.Item;
      this.gvSequentialMilestones.Location = new Point(1, 26);
      this.gvSequentialMilestones.Name = "gvSequentialMilestones";
      this.gvSequentialMilestones.Size = new Size(1177, 648);
      this.gvSequentialMilestones.SortOption = GVSortOption.None;
      this.gvSequentialMilestones.TabIndex = 1;
      this.gvSequentialMilestones.SelectedIndexChanged += new EventHandler(this.gvSequentialMilestones_SelectedIndexChanged);
      this.gvSequentialMilestones.ItemDrag += new GVItemEventHandler(this.gvSequentialMilestones_ItemDrag);
      this.gvSequentialMilestones.EditorOpening += new GVSubItemEditingEventHandler(this.onMilestoneTemplateEditorOpening);
      this.gvSequentialMilestones.EditorClosing += new GVSubItemEditingEventHandler(this.onMilestoneTemplateEditorClosing);
      this.gvSequentialMilestones.SizeChanged += new EventHandler(this.gvSequentialMilestones_SizeChanged);
      this.gvSequentialMilestones.DragDrop += new DragEventHandler(this.gvSequentialMilestones_DragDrop);
      this.gvSequentialMilestones.DragEnter += new DragEventHandler(this.gvSequentialMilestones_DragEnter);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.stdRemoveMilestone);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveTemplateMilestoneUp);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveTemplateMilestoneDown);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdAddMilestoneToTemplate);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(1078, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(91, 22);
      this.flowLayoutPanel1.TabIndex = 2;
      this.stdRemoveMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdRemoveMilestone.BackColor = Color.Transparent;
      this.stdRemoveMilestone.Location = new Point(75, 3);
      this.stdRemoveMilestone.Margin = new Padding(5, 3, 0, 3);
      this.stdRemoveMilestone.MouseDownImage = (Image) null;
      this.stdRemoveMilestone.Name = "stdRemoveMilestone";
      this.stdRemoveMilestone.Size = new Size(16, 17);
      this.stdRemoveMilestone.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdRemoveMilestone.TabIndex = 43;
      this.stdRemoveMilestone.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdRemoveMilestone, "Remove Milestone from this Template");
      this.stdRemoveMilestone.Click += new EventHandler(this.stdRemoveMilestone_Click);
      this.btnMoveTemplateMilestoneUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveTemplateMilestoneUp.BackColor = Color.Transparent;
      this.btnMoveTemplateMilestoneUp.Enabled = false;
      this.btnMoveTemplateMilestoneUp.Location = new Point(54, 3);
      this.btnMoveTemplateMilestoneUp.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveTemplateMilestoneUp.MouseDownImage = (Image) null;
      this.btnMoveTemplateMilestoneUp.Name = "btnMoveTemplateMilestoneUp";
      this.btnMoveTemplateMilestoneUp.Size = new Size(16, 17);
      this.btnMoveTemplateMilestoneUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveTemplateMilestoneUp.TabIndex = 39;
      this.btnMoveTemplateMilestoneUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveTemplateMilestoneUp, "Move the milestone up in this template");
      this.btnMoveTemplateMilestoneUp.Click += new EventHandler(this.btnMoveTemplateMilestoneUp_Click);
      this.btnMoveTemplateMilestoneDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveTemplateMilestoneDown.BackColor = Color.Transparent;
      this.btnMoveTemplateMilestoneDown.Enabled = false;
      this.btnMoveTemplateMilestoneDown.Location = new Point(33, 3);
      this.btnMoveTemplateMilestoneDown.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveTemplateMilestoneDown.MouseDownImage = (Image) null;
      this.btnMoveTemplateMilestoneDown.Name = "btnMoveTemplateMilestoneDown";
      this.btnMoveTemplateMilestoneDown.Size = new Size(16, 17);
      this.btnMoveTemplateMilestoneDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveTemplateMilestoneDown.TabIndex = 38;
      this.btnMoveTemplateMilestoneDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveTemplateMilestoneDown, "Move the milestone down in this template");
      this.btnMoveTemplateMilestoneDown.Click += new EventHandler(this.btnMoveTemplateMilestoneDown_Click);
      this.stdAddMilestoneToTemplate.BackColor = Color.Transparent;
      this.stdAddMilestoneToTemplate.Location = new Point(9, 3);
      this.stdAddMilestoneToTemplate.MouseDownImage = (Image) null;
      this.stdAddMilestoneToTemplate.Name = "stdAddMilestoneToTemplate";
      this.stdAddMilestoneToTemplate.Size = new Size(16, 16);
      this.stdAddMilestoneToTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdAddMilestoneToTemplate.TabIndex = 44;
      this.stdAddMilestoneToTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdAddMilestoneToTemplate, "Add milestone to this template");
      this.stdAddMilestoneToTemplate.Click += new EventHandler(this.stdAddMilestoneToTemplate_Click);
      this.tabSettings.AutoScroll = true;
      this.tabSettings.Controls.Add((Control) this.label5);
      this.tabSettings.Controls.Add((Control) this.linkLabel1);
      this.tabSettings.Controls.Add((Control) this.label4);
      this.tabSettings.Controls.Add((Control) this.flowEDisclosure);
      this.tabSettings.Controls.Add((Control) this.linkAutoLoanNumber);
      this.tabSettings.Controls.Add((Control) this.lblDisconnectedFromPersona);
      this.tabSettings.Controls.Add((Control) this.lblLinkedWithPersona);
      this.tabSettings.Controls.Add((Control) this.cmbLoanNumbering);
      this.tabSettings.Controls.Add((Control) this.lblAutoLoanNumber);
      this.tabSettings.Location = new Point(4, 23);
      this.tabSettings.Name = "tabSettings";
      this.tabSettings.Size = new Size(1179, 675);
      this.tabSettings.TabIndex = 2;
      this.tabSettings.Text = "Settings";
      this.tabSettings.UseVisualStyleBackColor = true;
      this.tabSettings.Enter += new EventHandler(this.tabSettings_Enter);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(4, 10);
      this.label5.Name = "label5";
      this.label5.Size = new Size(595, 14);
      this.label5.TabIndex = 27;
      this.label5.Text = "Select a trigger milestone for each action. The action will start at the completion of the selected milestone.";
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new Point(457, 72);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(33, 14);
      this.linkLabel1.TabIndex = 26;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "View";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.label4.ImageAlign = ContentAlignment.MiddleRight;
      this.label4.ImageList = this.imageList1;
      this.label4.Location = new Point(36, 72);
      this.label4.Name = "label4";
      this.label4.Size = new Size(205, 22);
      this.label4.TabIndex = 16;
      this.label4.Text = "eDisclosure Initial Package";
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "link.png");
      this.imageList1.Images.SetKeyName(1, "link-broken.png");
      this.flowEDisclosure.AutoSize = true;
      this.flowEDisclosure.Controls.Add((Control) this.grpBankerRetail);
      this.flowEDisclosure.Controls.Add((Control) this.grpBankerWholesale);
      this.flowEDisclosure.Controls.Add((Control) this.grpBroker);
      this.flowEDisclosure.Controls.Add((Control) this.grpCorrespondent);
      this.flowEDisclosure.FlowDirection = FlowDirection.TopDown;
      this.flowEDisclosure.Location = new Point(95, 97);
      this.flowEDisclosure.Name = "flowEDisclosure";
      this.flowEDisclosure.Size = new Size(385, 581);
      this.flowEDisclosure.TabIndex = 15;
      this.grpBankerRetail.Controls.Add((Control) this.pnlRAA);
      this.grpBankerRetail.Controls.Add((Control) this.pnlRTD);
      this.grpBankerRetail.Controls.Add((Control) this.pnlRAL);
      this.grpBankerRetail.Controls.Add((Control) this.pnlRA);
      this.grpBankerRetail.HeaderForeColor = SystemColors.ControlText;
      this.grpBankerRetail.Location = new Point(0, 0);
      this.grpBankerRetail.Margin = new Padding(0);
      this.grpBankerRetail.Name = "grpBankerRetail";
      this.grpBankerRetail.Size = new Size(385, 145);
      this.grpBankerRetail.TabIndex = 0;
      this.grpBankerRetail.Text = "Banker - Retail";
      this.grpBankerRetail.Visible = false;
      this.pnlRAA.Controls.Add((Control) this.lblRAA);
      this.pnlRAA.Controls.Add((Control) this.mdbRAA);
      this.pnlRAA.Location = new Point(15, 32);
      this.pnlRAA.Name = "pnlRAA";
      this.pnlRAA.Size = new Size(347, 27);
      this.pnlRAA.TabIndex = 14;
      this.lblRAA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblRAA.ImageIndex = 0;
      this.lblRAA.ImageList = this.imageList1;
      this.lblRAA.Location = new Point(3, 6);
      this.lblRAA.Name = "lblRAA";
      this.lblRAA.Size = new Size(128, 14);
      this.lblRAA.TabIndex = 0;
      this.lblRAA.Text = "At Application";
      this.mdbRAA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbRAA.Location = new Point(132, 3);
      this.mdbRAA.Name = "mdbRAA";
      this.mdbRAA.Size = new Size(209, 22);
      this.mdbRAA.TabIndex = 10;
      this.mdbRAA.Tag = (object) "RetailChannel_AtApplication";
      this.mdbRAA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.pnlRTD.Controls.Add((Control) this.lblRTD);
      this.pnlRTD.Controls.Add((Control) this.mdbRTD);
      this.pnlRTD.Location = new Point(15, 55);
      this.pnlRTD.Name = "pnlRTD";
      this.pnlRTD.Size = new Size(347, 27);
      this.pnlRTD.TabIndex = 15;
      this.lblRTD.ImageAlign = ContentAlignment.MiddleRight;
      this.lblRTD.ImageIndex = 0;
      this.lblRTD.ImageList = this.imageList1;
      this.lblRTD.Location = new Point(3, 6);
      this.lblRTD.Name = "lblRTD";
      this.lblRTD.Size = new Size(128, 14);
      this.lblRTD.TabIndex = 1;
      this.lblRTD.Text = "Three-Day";
      this.mdbRTD.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbRTD.Location = new Point(132, 3);
      this.mdbRTD.Name = "mdbRTD";
      this.mdbRTD.Size = new Size(209, 22);
      this.mdbRTD.TabIndex = 11;
      this.mdbRTD.Tag = (object) "RetailChannel_ThreeDay";
      this.mdbRTD.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.pnlRAL.Controls.Add((Control) this.lblRAL);
      this.pnlRAL.Controls.Add((Control) this.mdbRAL);
      this.pnlRAL.Location = new Point(15, 81);
      this.pnlRAL.Name = "pnlRAL";
      this.pnlRAL.Size = new Size(347, 27);
      this.pnlRAL.TabIndex = 16;
      this.lblRAL.ImageAlign = ContentAlignment.MiddleRight;
      this.lblRAL.ImageIndex = 0;
      this.lblRAL.ImageList = this.imageList1;
      this.lblRAL.Location = new Point(3, 6);
      this.lblRAL.Name = "lblRAL";
      this.lblRAL.Size = new Size(128, 14);
      this.lblRAL.TabIndex = 2;
      this.lblRAL.Text = "At Lock";
      this.mdbRAL.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbRAL.Location = new Point(132, 3);
      this.mdbRAL.Name = "mdbRAL";
      this.mdbRAL.Size = new Size(209, 22);
      this.mdbRAL.TabIndex = 12;
      this.mdbRAL.Tag = (object) "RetailChannel_AtLock";
      this.mdbRAL.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.pnlRA.Controls.Add((Control) this.lblRA);
      this.pnlRA.Controls.Add((Control) this.mdbRA);
      this.pnlRA.Location = new Point(15, 107);
      this.pnlRA.Name = "pnlRA";
      this.pnlRA.Size = new Size(347, 27);
      this.pnlRA.TabIndex = 13;
      this.lblRA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblRA.ImageIndex = 0;
      this.lblRA.ImageList = this.imageList1;
      this.lblRA.Location = new Point(3, 6);
      this.lblRA.Name = "lblRA";
      this.lblRA.Size = new Size(128, 14);
      this.lblRA.TabIndex = 3;
      this.lblRA.Text = "Approval";
      this.mdbRA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbRA.Location = new Point(132, 3);
      this.mdbRA.Name = "mdbRA";
      this.mdbRA.Size = new Size(209, 22);
      this.mdbRA.TabIndex = 13;
      this.mdbRA.Tag = (object) "RetailChannel_Approval";
      this.mdbRA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.grpBankerWholesale.Controls.Add((Control) this.pnlWAA);
      this.grpBankerWholesale.Controls.Add((Control) this.pnlWTD);
      this.grpBankerWholesale.Controls.Add((Control) this.pnlWAL);
      this.grpBankerWholesale.Controls.Add((Control) this.pnlWA);
      this.grpBankerWholesale.HeaderForeColor = SystemColors.ControlText;
      this.grpBankerWholesale.Location = new Point(0, 145);
      this.grpBankerWholesale.Margin = new Padding(0);
      this.grpBankerWholesale.Name = "grpBankerWholesale";
      this.grpBankerWholesale.Size = new Size(385, 145);
      this.grpBankerWholesale.TabIndex = 1;
      this.grpBankerWholesale.Text = "Banker - Wholesale";
      this.grpBankerWholesale.Visible = false;
      this.pnlWAA.Controls.Add((Control) this.mdbWAA);
      this.pnlWAA.Controls.Add((Control) this.lblWAA);
      this.pnlWAA.Location = new Point(15, 32);
      this.pnlWAA.Name = "pnlWAA";
      this.pnlWAA.Size = new Size(347, 27);
      this.pnlWAA.TabIndex = 19;
      this.mdbWAA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbWAA.Location = new Point(132, 3);
      this.mdbWAA.Name = "mdbWAA";
      this.mdbWAA.Size = new Size(209, 22);
      this.mdbWAA.TabIndex = 14;
      this.mdbWAA.Tag = (object) "WholesaleChannel_AtApplication";
      this.mdbWAA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblWAA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblWAA.ImageIndex = 0;
      this.lblWAA.ImageList = this.imageList1;
      this.lblWAA.Location = new Point(3, 6);
      this.lblWAA.Name = "lblWAA";
      this.lblWAA.Size = new Size(128, 14);
      this.lblWAA.TabIndex = 0;
      this.lblWAA.Text = "At Application";
      this.pnlWTD.Controls.Add((Control) this.mdbWTD);
      this.pnlWTD.Controls.Add((Control) this.lblWTD);
      this.pnlWTD.Location = new Point(15, 55);
      this.pnlWTD.Name = "pnlWTD";
      this.pnlWTD.Size = new Size(347, 27);
      this.pnlWTD.TabIndex = 20;
      this.mdbWTD.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbWTD.Location = new Point(132, 3);
      this.mdbWTD.Name = "mdbWTD";
      this.mdbWTD.Size = new Size(209, 22);
      this.mdbWTD.TabIndex = 15;
      this.mdbWTD.Tag = (object) "WholesaleChannel_ThreeDay";
      this.mdbWTD.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblWTD.ImageAlign = ContentAlignment.MiddleRight;
      this.lblWTD.ImageIndex = 0;
      this.lblWTD.ImageList = this.imageList1;
      this.lblWTD.Location = new Point(3, 6);
      this.lblWTD.Name = "lblWTD";
      this.lblWTD.Size = new Size(128, 14);
      this.lblWTD.TabIndex = 1;
      this.lblWTD.Text = "Three-Day";
      this.pnlWAL.Controls.Add((Control) this.mdbWAL);
      this.pnlWAL.Controls.Add((Control) this.lblWAL);
      this.pnlWAL.Location = new Point(15, 81);
      this.pnlWAL.Name = "pnlWAL";
      this.pnlWAL.Size = new Size(347, 27);
      this.pnlWAL.TabIndex = 21;
      this.mdbWAL.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbWAL.Location = new Point(132, 3);
      this.mdbWAL.Name = "mdbWAL";
      this.mdbWAL.Size = new Size(209, 22);
      this.mdbWAL.TabIndex = 16;
      this.mdbWAL.Tag = (object) "WholesaleChannel_AtLock";
      this.mdbWAL.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblWAL.ImageAlign = ContentAlignment.MiddleRight;
      this.lblWAL.ImageIndex = 0;
      this.lblWAL.ImageList = this.imageList1;
      this.lblWAL.Location = new Point(3, 6);
      this.lblWAL.Name = "lblWAL";
      this.lblWAL.Size = new Size(128, 14);
      this.lblWAL.TabIndex = 2;
      this.lblWAL.Text = "At Lock";
      this.pnlWA.Controls.Add((Control) this.mdbWA);
      this.pnlWA.Controls.Add((Control) this.lblWA);
      this.pnlWA.Location = new Point(15, 107);
      this.pnlWA.Name = "pnlWA";
      this.pnlWA.Size = new Size(347, 27);
      this.pnlWA.TabIndex = 18;
      this.mdbWA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbWA.Location = new Point(132, 3);
      this.mdbWA.Name = "mdbWA";
      this.mdbWA.Size = new Size(209, 22);
      this.mdbWA.TabIndex = 17;
      this.mdbWA.Tag = (object) "WholesaleChannel_Approval";
      this.mdbWA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblWA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblWA.ImageIndex = 0;
      this.lblWA.ImageList = this.imageList1;
      this.lblWA.Location = new Point(3, 6);
      this.lblWA.Name = "lblWA";
      this.lblWA.Size = new Size(128, 14);
      this.lblWA.TabIndex = 3;
      this.lblWA.Text = "Approval";
      this.grpBroker.Controls.Add((Control) this.pnlBAA);
      this.grpBroker.Controls.Add((Control) this.pnlBTD);
      this.grpBroker.Controls.Add((Control) this.pnlBAL);
      this.grpBroker.Controls.Add((Control) this.pnlBA);
      this.grpBroker.HeaderForeColor = SystemColors.ControlText;
      this.grpBroker.Location = new Point(0, 290);
      this.grpBroker.Margin = new Padding(0);
      this.grpBroker.Name = "grpBroker";
      this.grpBroker.Size = new Size(385, 145);
      this.grpBroker.TabIndex = 2;
      this.grpBroker.Text = "Broker";
      this.grpBroker.Visible = false;
      this.pnlBAA.Controls.Add((Control) this.mdbBAA);
      this.pnlBAA.Controls.Add((Control) this.lblBAA);
      this.pnlBAA.Location = new Point(15, 32);
      this.pnlBAA.Name = "pnlBAA";
      this.pnlBAA.Size = new Size(347, 27);
      this.pnlBAA.TabIndex = 23;
      this.mdbBAA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbBAA.Location = new Point(132, 3);
      this.mdbBAA.Name = "mdbBAA";
      this.mdbBAA.Size = new Size(209, 22);
      this.mdbBAA.TabIndex = 18;
      this.mdbBAA.Tag = (object) "BrokerChannel_AtApplication";
      this.mdbBAA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblBAA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblBAA.ImageIndex = 0;
      this.lblBAA.ImageList = this.imageList1;
      this.lblBAA.Location = new Point(3, 6);
      this.lblBAA.Name = "lblBAA";
      this.lblBAA.Size = new Size(128, 14);
      this.lblBAA.TabIndex = 0;
      this.lblBAA.Text = "At Application";
      this.pnlBTD.Controls.Add((Control) this.mdbBTD);
      this.pnlBTD.Controls.Add((Control) this.lblBTD);
      this.pnlBTD.Location = new Point(15, 55);
      this.pnlBTD.Name = "pnlBTD";
      this.pnlBTD.Size = new Size(347, 27);
      this.pnlBTD.TabIndex = 24;
      this.mdbBTD.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbBTD.Location = new Point(132, 3);
      this.mdbBTD.Name = "mdbBTD";
      this.mdbBTD.Size = new Size(209, 22);
      this.mdbBTD.TabIndex = 19;
      this.mdbBTD.Tag = (object) "BrokerChannel_ThreeDay";
      this.mdbBTD.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblBTD.ImageAlign = ContentAlignment.MiddleRight;
      this.lblBTD.ImageIndex = 0;
      this.lblBTD.ImageList = this.imageList1;
      this.lblBTD.Location = new Point(3, 6);
      this.lblBTD.Name = "lblBTD";
      this.lblBTD.Size = new Size(128, 14);
      this.lblBTD.TabIndex = 1;
      this.lblBTD.Text = "Three-Day";
      this.pnlBAL.Controls.Add((Control) this.mdbBAL);
      this.pnlBAL.Controls.Add((Control) this.lblBAL);
      this.pnlBAL.Location = new Point(15, 81);
      this.pnlBAL.Name = "pnlBAL";
      this.pnlBAL.Size = new Size(347, 27);
      this.pnlBAL.TabIndex = 25;
      this.mdbBAL.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbBAL.Location = new Point(132, 3);
      this.mdbBAL.Name = "mdbBAL";
      this.mdbBAL.Size = new Size(209, 22);
      this.mdbBAL.TabIndex = 20;
      this.mdbBAL.Tag = (object) "BrokerChannel_AtLock";
      this.mdbBAL.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblBAL.ImageAlign = ContentAlignment.MiddleRight;
      this.lblBAL.ImageIndex = 0;
      this.lblBAL.ImageList = this.imageList1;
      this.lblBAL.Location = new Point(3, 6);
      this.lblBAL.Name = "lblBAL";
      this.lblBAL.Size = new Size(128, 14);
      this.lblBAL.TabIndex = 2;
      this.lblBAL.Text = "At Lock";
      this.pnlBA.Controls.Add((Control) this.mdbBA);
      this.pnlBA.Controls.Add((Control) this.lblBA);
      this.pnlBA.Location = new Point(15, 107);
      this.pnlBA.Name = "pnlBA";
      this.pnlBA.Size = new Size(347, 27);
      this.pnlBA.TabIndex = 22;
      this.mdbBA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbBA.Location = new Point(132, 3);
      this.mdbBA.Name = "mdbBA";
      this.mdbBA.Size = new Size(209, 22);
      this.mdbBA.TabIndex = 21;
      this.mdbBA.Tag = (object) "BrokerChannel_Approval";
      this.mdbBA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblBA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblBA.ImageIndex = 0;
      this.lblBA.ImageList = this.imageList1;
      this.lblBA.Location = new Point(3, 6);
      this.lblBA.Name = "lblBA";
      this.lblBA.Size = new Size(128, 14);
      this.lblBA.TabIndex = 3;
      this.lblBA.Text = "Approval";
      this.grpCorrespondent.Controls.Add((Control) this.pnlCAA);
      this.grpCorrespondent.Controls.Add((Control) this.pnlCTD);
      this.grpCorrespondent.Controls.Add((Control) this.pnlCAL);
      this.grpCorrespondent.Controls.Add((Control) this.pnlCA);
      this.grpCorrespondent.HeaderForeColor = SystemColors.ControlText;
      this.grpCorrespondent.Location = new Point(0, 435);
      this.grpCorrespondent.Margin = new Padding(0);
      this.grpCorrespondent.Name = "grpCorrespondent";
      this.grpCorrespondent.Size = new Size(385, 145);
      this.grpCorrespondent.TabIndex = 3;
      this.grpCorrespondent.Text = "Correspondent";
      this.grpCorrespondent.Visible = false;
      this.pnlCAA.Controls.Add((Control) this.mdbCAA);
      this.pnlCAA.Controls.Add((Control) this.lblCAA);
      this.pnlCAA.Location = new Point(15, 32);
      this.pnlCAA.Name = "pnlCAA";
      this.pnlCAA.Size = new Size(347, 27);
      this.pnlCAA.TabIndex = 27;
      this.mdbCAA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbCAA.Location = new Point(132, 3);
      this.mdbCAA.Name = "mdbCAA";
      this.mdbCAA.Size = new Size(209, 22);
      this.mdbCAA.TabIndex = 22;
      this.mdbCAA.Tag = (object) "CorrespondentChannel_AtApplication";
      this.mdbCAA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblCAA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblCAA.ImageIndex = 0;
      this.lblCAA.ImageList = this.imageList1;
      this.lblCAA.Location = new Point(3, 6);
      this.lblCAA.Name = "lblCAA";
      this.lblCAA.Size = new Size(128, 14);
      this.lblCAA.TabIndex = 0;
      this.lblCAA.Text = "At Application";
      this.pnlCTD.Controls.Add((Control) this.mdbCTD);
      this.pnlCTD.Controls.Add((Control) this.lblCTD);
      this.pnlCTD.Location = new Point(15, 55);
      this.pnlCTD.Name = "pnlCTD";
      this.pnlCTD.Size = new Size(347, 27);
      this.pnlCTD.TabIndex = 28;
      this.mdbCTD.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbCTD.Location = new Point(132, 3);
      this.mdbCTD.Name = "mdbCTD";
      this.mdbCTD.Size = new Size(209, 22);
      this.mdbCTD.TabIndex = 23;
      this.mdbCTD.Tag = (object) "CorrespondentChannel_ThreeDay";
      this.mdbCTD.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblCTD.ImageAlign = ContentAlignment.MiddleRight;
      this.lblCTD.ImageIndex = 0;
      this.lblCTD.ImageList = this.imageList1;
      this.lblCTD.Location = new Point(3, 6);
      this.lblCTD.Name = "lblCTD";
      this.lblCTD.Size = new Size(128, 14);
      this.lblCTD.TabIndex = 1;
      this.lblCTD.Text = "Three-Day";
      this.pnlCAL.Controls.Add((Control) this.mdbCAL);
      this.pnlCAL.Controls.Add((Control) this.lblCAL);
      this.pnlCAL.Location = new Point(15, 81);
      this.pnlCAL.Name = "pnlCAL";
      this.pnlCAL.Size = new Size(347, 27);
      this.pnlCAL.TabIndex = 29;
      this.mdbCAL.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbCAL.Location = new Point(132, 3);
      this.mdbCAL.Name = "mdbCAL";
      this.mdbCAL.Size = new Size(209, 22);
      this.mdbCAL.TabIndex = 24;
      this.mdbCAL.Tag = (object) "CorrespondentChannel_AtLock";
      this.mdbCAL.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblCAL.ImageAlign = ContentAlignment.MiddleRight;
      this.lblCAL.ImageIndex = 0;
      this.lblCAL.ImageList = this.imageList1;
      this.lblCAL.Location = new Point(3, 6);
      this.lblCAL.Name = "lblCAL";
      this.lblCAL.Size = new Size(128, 14);
      this.lblCAL.TabIndex = 2;
      this.lblCAL.Text = "At Lock";
      this.pnlCA.Controls.Add((Control) this.mdbCA);
      this.pnlCA.Controls.Add((Control) this.lblCA);
      this.pnlCA.Location = new Point(15, 107);
      this.pnlCA.Name = "pnlCA";
      this.pnlCA.Size = new Size(347, 27);
      this.pnlCA.TabIndex = 26;
      this.mdbCA.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mdbCA.Location = new Point(132, 3);
      this.mdbCA.Name = "mdbCA";
      this.mdbCA.Size = new Size(209, 22);
      this.mdbCA.TabIndex = 25;
      this.mdbCA.Tag = (object) "CorrespondentChannel_Approval";
      this.mdbCA.SelectedIndexChanged += new EventHandler(this.eDis_SelectedIndexChanged);
      this.lblCA.ImageAlign = ContentAlignment.MiddleRight;
      this.lblCA.ImageIndex = 0;
      this.lblCA.ImageList = this.imageList1;
      this.lblCA.Location = new Point(3, 6);
      this.lblCA.Name = "lblCA";
      this.lblCA.Size = new Size(128, 14);
      this.lblCA.TabIndex = 3;
      this.lblCA.Text = "Approval";
      this.linkAutoLoanNumber.AutoSize = true;
      this.linkAutoLoanNumber.Location = new Point(457, 46);
      this.linkAutoLoanNumber.Name = "linkAutoLoanNumber";
      this.linkAutoLoanNumber.Size = new Size(33, 14);
      this.linkAutoLoanNumber.TabIndex = 13;
      this.linkAutoLoanNumber.TabStop = true;
      this.linkAutoLoanNumber.Text = "View";
      this.linkAutoLoanNumber.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkAutoLoanNumber_LinkClicked);
      this.lblDisconnectedFromPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDisconnectedFromPersona.AutoSize = true;
      this.lblDisconnectedFromPersona.Image = (Image) Resources.link_broken;
      this.lblDisconnectedFromPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnectedFromPersona.Location = new Point(3, 1326);
      this.lblDisconnectedFromPersona.Name = "lblDisconnectedFromPersona";
      this.lblDisconnectedFromPersona.Size = new Size(479, 14);
      this.lblDisconnectedFromPersona.TabIndex = 12;
      this.lblDisconnectedFromPersona.Text = "        Trigger milestone selected by admin (here or using the related setting in Encompass Settings)";
      this.lblDisconnectedFromPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblLinkedWithPersona.AutoSize = true;
      this.lblLinkedWithPersona.Image = (Image) Resources.link;
      this.lblLinkedWithPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Location = new Point(3, 1346);
      this.lblLinkedWithPersona.Name = "lblLinkedWithPersona";
      this.lblLinkedWithPersona.Size = new Size(292, 14);
      this.lblLinkedWithPersona.TabIndex = 11;
      this.lblLinkedWithPersona.Text = "        Trigger milestone selected by system (system default)";
      this.lblLinkedWithPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.cmbLoanNumbering.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbLoanNumbering.Location = new Point(241, 38);
      this.cmbLoanNumbering.Name = "cmbLoanNumbering";
      this.cmbLoanNumbering.Size = new Size(209, 22);
      this.cmbLoanNumbering.TabIndex = 9;
      this.cmbLoanNumbering.SelectedIndexChanged += new EventHandler(this.cmbLoanNumbering_SelectedIndexChanged);
      this.lblAutoLoanNumber.ImageAlign = ContentAlignment.MiddleRight;
      this.lblAutoLoanNumber.ImageIndex = 0;
      this.lblAutoLoanNumber.ImageList = this.imageList1;
      this.lblAutoLoanNumber.Location = new Point(36, 38);
      this.lblAutoLoanNumber.Name = "lblAutoLoanNumber";
      this.lblAutoLoanNumber.Size = new Size(205, 22);
      this.lblAutoLoanNumber.TabIndex = 7;
      this.lblAutoLoanNumber.Text = "When to Start Loan Numbering";
      this.tabRoles.Controls.Add((Control) this.groupContainer2);
      this.tabRoles.Location = new Point(4, 23);
      this.tabRoles.Name = "tabRoles";
      this.tabRoles.Size = new Size(1179, 675);
      this.tabRoles.TabIndex = 3;
      this.tabRoles.Text = "Roles";
      this.tabRoles.UseVisualStyleBackColor = true;
      this.groupContainer2.Controls.Add((Control) this.flowLayoutPanel5);
      this.groupContainer2.Controls.Add((Control) this.gvRoles);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1179, 675);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Assigned Roles";
      this.flowLayoutPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel5.BackColor = Color.Transparent;
      this.flowLayoutPanel5.Controls.Add((Control) this.btnRemoveFreeRole);
      this.flowLayoutPanel5.Controls.Add((Control) this.stdAddFreeRoleToTemplate);
      this.flowLayoutPanel5.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel5.Location = new Point(1128, 3);
      this.flowLayoutPanel5.Name = "flowLayoutPanel5";
      this.flowLayoutPanel5.Size = new Size(47, 22);
      this.flowLayoutPanel5.TabIndex = 3;
      this.btnRemoveFreeRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveFreeRole.BackColor = Color.Transparent;
      this.btnRemoveFreeRole.Location = new Point(31, 3);
      this.btnRemoveFreeRole.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveFreeRole.MouseDownImage = (Image) null;
      this.btnRemoveFreeRole.Name = "btnRemoveFreeRole";
      this.btnRemoveFreeRole.Size = new Size(16, 17);
      this.btnRemoveFreeRole.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFreeRole.TabIndex = 43;
      this.btnRemoveFreeRole.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveFreeRole, "Remove Free Role from this template");
      this.btnRemoveFreeRole.Click += new EventHandler(this.btnRemoveFreeRole_Click);
      this.stdAddFreeRoleToTemplate.BackColor = Color.Transparent;
      this.stdAddFreeRoleToTemplate.Location = new Point(7, 3);
      this.stdAddFreeRoleToTemplate.MouseDownImage = (Image) null;
      this.stdAddFreeRoleToTemplate.Name = "stdAddFreeRoleToTemplate";
      this.stdAddFreeRoleToTemplate.Size = new Size(16, 16);
      this.stdAddFreeRoleToTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdAddFreeRoleToTemplate.TabIndex = 44;
      this.stdAddFreeRoleToTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdAddFreeRoleToTemplate, "Add Free Role to this template");
      this.stdAddFreeRoleToTemplate.Click += new EventHandler(this.stdAddFreeRoleToTemplate_Click);
      this.gvRoles.AllowMultiselect = false;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = "Role";
      gvColumn9.Width = 200;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column2";
      gvColumn10.Text = "Milestone";
      gvColumn10.Width = 300;
      this.gvRoles.Columns.AddRange(new GVColumn[2]
      {
        gvColumn9,
        gvColumn10
      });
      this.gvRoles.Dock = DockStyle.Fill;
      this.gvRoles.Location = new Point(1, 26);
      this.gvRoles.Name = "gvRoles";
      this.gvRoles.Size = new Size(1177, 648);
      this.gvRoles.SortOption = GVSortOption.None;
      this.gvRoles.TabIndex = 0;
      this.gvRoles.SelectedIndexChanged += new EventHandler(this.gvRoles_SelectedIndexChanged);
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnResetTemplate);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSaveTemplate);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(1133, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(52, 22);
      this.flowLayoutPanel2.TabIndex = 1;
      this.btnResetTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnResetTemplate.BackColor = Color.Transparent;
      this.btnResetTemplate.Location = new Point(36, 3);
      this.btnResetTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnResetTemplate.MouseDownImage = (Image) null;
      this.btnResetTemplate.Name = "btnResetTemplate";
      this.btnResetTemplate.Size = new Size(16, 17);
      this.btnResetTemplate.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetTemplate.TabIndex = 44;
      this.btnResetTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnResetTemplate, "Reset Template");
      this.btnResetTemplate.Click += new EventHandler(this.btnResetTemplate_Click);
      this.btnSaveTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSaveTemplate.BackColor = Color.Transparent;
      this.btnSaveTemplate.Enabled = false;
      this.btnSaveTemplate.Location = new Point(15, 3);
      this.btnSaveTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnSaveTemplate.MouseDownImage = (Image) null;
      this.btnSaveTemplate.Name = "btnSaveTemplate";
      this.btnSaveTemplate.Size = new Size(16, 17);
      this.btnSaveTemplate.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveTemplate.TabIndex = 43;
      this.btnSaveTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveTemplate, "Save Template");
      this.btnSaveTemplate.Click += new EventHandler(this.btnSaveTemplate_Click);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.groupTemplateDetails;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 176);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 3;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.collapsibleSplitter1.Click += new EventHandler(this.collapsibleSplitter1_Click);
      this.flowLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel4.BackColor = Color.Transparent;
      this.flowLayoutPanel4.Controls.Add((Control) this.btnAddTemplate);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnDuplicateTemplate);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnMoveTemplateDown);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnMoveTemplateUp);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnDeleteTemplate);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnStatus);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnGlobalSettings);
      this.flowLayoutPanel4.Location = new Point(820, 1);
      this.flowLayoutPanel4.Name = "flowLayoutPanel4";
      this.flowLayoutPanel4.Size = new Size(369, 21);
      this.flowLayoutPanel4.TabIndex = 2;
      this.btnAddTemplate.BackColor = Color.Transparent;
      this.btnAddTemplate.Location = new Point(3, 3);
      this.btnAddTemplate.MouseDownImage = (Image) null;
      this.btnAddTemplate.Name = "btnAddTemplate";
      this.btnAddTemplate.Size = new Size(16, 16);
      this.btnAddTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddTemplate.TabIndex = 17;
      this.btnAddTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddTemplate, "Add Template");
      this.btnAddTemplate.Click += new EventHandler(this.btnAddTemplate_Click);
      this.btnDuplicateTemplate.BackColor = Color.Transparent;
      this.btnDuplicateTemplate.Location = new Point(25, 3);
      this.btnDuplicateTemplate.MouseDownImage = (Image) null;
      this.btnDuplicateTemplate.Name = "btnDuplicateTemplate";
      this.btnDuplicateTemplate.Size = new Size(16, 16);
      this.btnDuplicateTemplate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicateTemplate.TabIndex = 18;
      this.btnDuplicateTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicateTemplate, "Duplicate Template");
      this.btnDuplicateTemplate.Click += new EventHandler(this.btnDuplicateTemplate_Click);
      this.btnMoveTemplateDown.BackColor = Color.Transparent;
      this.btnMoveTemplateDown.Location = new Point(47, 3);
      this.btnMoveTemplateDown.MouseDownImage = (Image) null;
      this.btnMoveTemplateDown.Name = "btnMoveTemplateDown";
      this.btnMoveTemplateDown.Size = new Size(16, 16);
      this.btnMoveTemplateDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveTemplateDown.TabIndex = 20;
      this.btnMoveTemplateDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveTemplateDown, "Move Template Down");
      this.btnMoveTemplateDown.Click += new EventHandler(this.btnMoveTemplateDown_Click);
      this.btnMoveTemplateUp.BackColor = Color.Transparent;
      this.btnMoveTemplateUp.Location = new Point(69, 3);
      this.btnMoveTemplateUp.MouseDownImage = (Image) null;
      this.btnMoveTemplateUp.Name = "btnMoveTemplateUp";
      this.btnMoveTemplateUp.Size = new Size(16, 16);
      this.btnMoveTemplateUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveTemplateUp.TabIndex = 19;
      this.btnMoveTemplateUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveTemplateUp, "Move Template Up");
      this.btnMoveTemplateUp.Click += new EventHandler(this.btnMoveTemplateUp_Click);
      this.btnDeleteTemplate.BackColor = Color.Transparent;
      this.btnDeleteTemplate.Location = new Point(91, 3);
      this.btnDeleteTemplate.MouseDownImage = (Image) null;
      this.btnDeleteTemplate.Name = "btnDeleteTemplate";
      this.btnDeleteTemplate.Size = new Size(16, 16);
      this.btnDeleteTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTemplate.TabIndex = 0;
      this.btnDeleteTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteTemplate, "Delete Template");
      this.btnDeleteTemplate.Click += new EventHandler(this.btnDeleteTemplate_Click);
      this.btnStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStatus.BackColor = SystemColors.Control;
      this.btnStatus.Location = new Point(113, 0);
      this.btnStatus.Margin = new Padding(3, 0, 3, 3);
      this.btnStatus.Name = "btnStatus";
      this.btnStatus.Size = new Size(80, 22);
      this.btnStatus.TabIndex = 38;
      this.btnStatus.Text = "Deac&tivate";
      this.btnStatus.UseVisualStyleBackColor = true;
      this.btnStatus.Click += new EventHandler(this.btnStatus_Click);
      this.btnGlobalSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGlobalSettings.Location = new Point(199, 0);
      this.btnGlobalSettings.Margin = new Padding(3, 0, 3, 3);
      this.btnGlobalSettings.Name = "btnGlobalSettings";
      this.btnGlobalSettings.Size = new Size(166, 22);
      this.btnGlobalSettings.TabIndex = 11;
      this.btnGlobalSettings.Text = "View Global Template Settings";
      this.btnGlobalSettings.UseVisualStyleBackColor = true;
      this.btnGlobalSettings.Visible = false;
      this.btnGlobalSettings.Click += new EventHandler(this.btnGlobalSettings_Click);
      this.gvTemplates.AllowDrop = true;
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column1";
      gvColumn11.Text = "Order";
      gvColumn11.Width = 50;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column2";
      gvColumn12.Text = "Name";
      gvColumn12.Width = 200;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column23";
      gvColumn13.Text = "ID";
      gvColumn13.Width = 100;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column3";
      gvColumn14.Text = "Condition";
      gvColumn14.Width = 300;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column14";
      gvColumn15.Text = "Channel";
      gvColumn15.Width = 300;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column4";
      gvColumn16.Text = "Status";
      gvColumn16.Width = 100;
      this.gvTemplates.Columns.AddRange(new GVColumn[6]
      {
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15,
        gvColumn16
      });
      this.gvTemplates.Dock = DockStyle.Top;
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(1189, 150);
      this.gvTemplates.SortOption = GVSortOption.None;
      this.gvTemplates.TabIndex = 1;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.ItemDrag += new GVItemEventHandler(this.gvTemplates_ItemDrag);
      this.gvTemplates.SizeChanged += new EventHandler(this.gvTemplates_SizeChanged);
      this.gvTemplates.DragDrop += new DragEventHandler(this.gvTemplates_DragDrop);
      this.gvTemplates.DragEnter += new DragEventHandler(this.gvTemplates_DragEnter);
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label7);
      this.gradientPanel1.Controls.Add((Control) this.lblglobalSettings);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1191, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 39;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(3, 8);
      this.label7.Name = "label7";
      this.label7.Size = new Size(165, 14);
      this.label7.TabIndex = 49;
      this.label7.Text = "Template order is important.";
      this.lblglobalSettings.AutoEllipsis = true;
      this.lblglobalSettings.BackColor = Color.Transparent;
      this.lblglobalSettings.Location = new Point(163, 7);
      this.lblglobalSettings.Name = "lblglobalSettings";
      this.lblglobalSettings.Size = new Size(650, 17);
      this.lblglobalSettings.TabIndex = 48;
      this.lblglobalSettings.Text = "The system applies the first template it finds in this list that best matches the data in the loan. Use the arrows to manage the list order.";
      this.lblglobalSettings.TextAlign = ContentAlignment.MiddleCenter;
      this.verticalSeparator2.Location = new Point(139, 3);
      this.verticalSeparator2.Margin = new Padding(3, 3, 0, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 45;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabMilestones);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MilestoneSettingsPanel);
      this.Size = new Size(1200, 978);
      this.tabMilestones.ResumeLayout(false);
      this.tpMilestones.ResumeLayout(false);
      this.gcMilestones.ResumeLayout(false);
      this.gcMilestones.PerformLayout();
      ((ISupportInitialize) this.btnNewMilestone).EndInit();
      ((ISupportInitialize) this.btnEditMilestone).EndInit();
      ((ISupportInitialize) this.btnMoveMilestoneUp).EndInit();
      ((ISupportInitialize) this.btnMoveMilestoneDown).EndInit();
      this.tpTemplates.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.grpTemplates.ResumeLayout(false);
      this.groupTemplateDetails.ResumeLayout(false);
      this.tabTemplates.ResumeLayout(false);
      this.tabDetails.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panelChannel.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.tabMilestoneDetails.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.stdRemoveMilestone).EndInit();
      ((ISupportInitialize) this.btnMoveTemplateMilestoneUp).EndInit();
      ((ISupportInitialize) this.btnMoveTemplateMilestoneDown).EndInit();
      ((ISupportInitialize) this.stdAddMilestoneToTemplate).EndInit();
      this.tabSettings.ResumeLayout(false);
      this.tabSettings.PerformLayout();
      this.flowEDisclosure.ResumeLayout(false);
      this.grpBankerRetail.ResumeLayout(false);
      this.pnlRAA.ResumeLayout(false);
      this.pnlRTD.ResumeLayout(false);
      this.pnlRAL.ResumeLayout(false);
      this.pnlRA.ResumeLayout(false);
      this.grpBankerWholesale.ResumeLayout(false);
      this.pnlWAA.ResumeLayout(false);
      this.pnlWTD.ResumeLayout(false);
      this.pnlWAL.ResumeLayout(false);
      this.pnlWA.ResumeLayout(false);
      this.grpBroker.ResumeLayout(false);
      this.pnlBAA.ResumeLayout(false);
      this.pnlBTD.ResumeLayout(false);
      this.pnlBAL.ResumeLayout(false);
      this.pnlBA.ResumeLayout(false);
      this.grpCorrespondent.ResumeLayout(false);
      this.pnlCAA.ResumeLayout(false);
      this.pnlCTD.ResumeLayout(false);
      this.pnlCAL.ResumeLayout(false);
      this.pnlCA.ResumeLayout(false);
      this.tabRoles.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.flowLayoutPanel5.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveFreeRole).EndInit();
      ((ISupportInitialize) this.stdAddFreeRoleToTemplate).EndInit();
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnResetTemplate).EndInit();
      ((ISupportInitialize) this.btnSaveTemplate).EndInit();
      this.flowLayoutPanel4.ResumeLayout(false);
      ((ISupportInitialize) this.btnAddTemplate).EndInit();
      ((ISupportInitialize) this.btnDuplicateTemplate).EndInit();
      ((ISupportInitialize) this.btnMoveTemplateDown).EndInit();
      ((ISupportInitialize) this.btnMoveTemplateUp).EndInit();
      ((ISupportInitialize) this.btnDeleteTemplate).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class DragDropData
    {
      public readonly GridView SourceView;
      public readonly EllieMae.EMLite.Workflow.Milestone[] Milestones;

      public DragDropData(GridView sourceView, EllieMae.EMLite.Workflow.Milestone[] milestones)
      {
        this.SourceView = sourceView;
        this.Milestones = milestones;
      }
    }
  }
}
