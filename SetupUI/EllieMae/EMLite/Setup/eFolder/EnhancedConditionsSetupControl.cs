// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EnhancedConditionsSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EnhancedConditionsSetupControl : UserControl
  {
    private Sessions.Session session;
    private FeaturesAclManager aclManager;
    private bool hasAddEditCopyECPermission;
    private bool hasRemoveECPermission;
    private bool hasActiveDeactiveECPermission;
    private bool hasConditionECPermission;
    private EnhancedConditionTemplate[] conditionTemplates;
    private EnhancedConditionTemplate condTemplate;
    private GridViewDataManager gvTemplatesMgr;
    private IContainer components;
    private ToolTip tooltip;
    private TableLayoutPanel tableLayoutPanel1;
    private GroupContainer gcConditionTemplates;
    private StandardIconButton btnNew;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private GridView gvConditionTemplates;
    private Button btnDeactivate;
    private Button btnActivate;
    private Panel panel1;
    private CheckBox chkExternally;
    private CheckBox chkInternally;
    private Label label3;
    private TextBox txtDaysToReceive;
    private Label label2;
    private Button btnConditionType;
    private Label label1;
    private Label lblListOfCounts;
    private VerticalSeparator verticalSeparator1;
    private CheckBox chkAssignRoleSetting;

    public EnhancedConditionsSetupControl(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.setAccessRights(this.aclManager);
      this.initTemplateList();
      this.init();
    }

    private void setAccessRights(FeaturesAclManager featuresAclManager)
    {
      this.hasAddEditCopyECPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_AddEditCopyConditions);
      this.hasRemoveECPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_DeleteConditions);
      this.hasActiveDeactiveECPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_ActivateDeactivateConditions);
      this.hasConditionECPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_ConditionTypeSettings);
      this.btnNew.Enabled = this.hasAddEditCopyECPermission;
      this.btnConditionType.Enabled = this.hasConditionECPermission;
    }

    private void init()
    {
      this.conditionTemplates = EnhancedConditionRestApiHelper.GetEnhancedConditionTemplates();
      this.loadTemplateList();
      this.disableActionButtons();
      this.gvConditionTemplates_SelectedIndexChanged((object) null, (EventArgs) null);
      bool result = false;
      bool.TryParse(this.session.ConfigurationManager.GetCompanySetting("Policies", "ENHANCEDCONDNAPPLYACCESSROLE"), out result);
      this.chkAssignRoleSetting.Checked = result;
    }

    private void initTemplateList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvConditionTemplates, (LoanDataMgr) null);
      this.gvConditionTemplates.Sort(12, SortOrder.Descending);
    }

    private void loadTemplateList()
    {
      this.gvTemplatesMgr.ClearItems();
      this.gcConditionTemplates.Text = "Conditions (0)";
      if (this.conditionTemplates == null)
        return;
      foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
      {
        bool selected = false;
        if (this.condTemplate != null)
        {
          Guid? id1 = conditionTemplate.Id;
          Guid? id2 = this.condTemplate.Id;
          selected = id1.HasValue == id2.HasValue && (!id1.HasValue || id1.GetValueOrDefault() == id2.GetValueOrDefault());
        }
        this.gvTemplatesMgr.AddItem(conditionTemplate, selected);
      }
      this.gcConditionTemplates.Text = "Conditions (" + (object) this.conditionTemplates.Length + ")";
      this.gvConditionTemplates.ReSort();
    }

    private void gvConditionTemplates_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.hasAddEditCopyECPermission)
        return;
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvConditionTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvConditionTemplates.SelectedItems.Count;
      if (count <= 0)
        return;
      this.condTemplate = (EnhancedConditionTemplate) this.gvConditionTemplates.SelectedItems[0].Tag;
      bool flag = false;
      if (this.condTemplate.Active.HasValue && this.hasActiveDeactiveECPermission)
        flag = this.condTemplate.Active.Value;
      this.btnDuplicate.Enabled = this.btnEdit.Enabled = count > 0 && this.hasAddEditCopyECPermission;
      this.btnDelete.Enabled = this.hasRemoveECPermission && !flag;
      if (!this.hasActiveDeactiveECPermission)
        return;
      this.btnActivate.Enabled = !flag;
      this.btnDeactivate.Enabled = flag;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      EnhancedConditionTemplateDialog conditionTemplateDialog = new EnhancedConditionTemplateDialog(this.session, new EnhancedConditionTemplate()
      {
        IsExternalPrint = this.chkExternally.Checked,
        IsInternalPrint = this.chkInternally.Checked,
        DaysToReceive = string.IsNullOrEmpty(this.txtDaysToReceive.Text) ? new int?() : new int?(Convert.ToInt32(this.txtDaysToReceive.Text))
      });
      int num = (int) conditionTemplateDialog.ShowDialog();
      if (conditionTemplateDialog.DialogResult != DialogResult.OK)
        return;
      this.init();
    }

    private void removeDefaultTrackingdefination(
      EnhancedConditionTemplate enhancedConditionTemplate)
    {
      if (enhancedConditionTemplate == null || enhancedConditionTemplate.customDefinitions == null || enhancedConditionTemplate.customDefinitions.trackingDefinitions == null)
        return;
      List<string> defaultTrackingOptions = Utils.GetEnhanceConditionsDefaultTrackingOptions();
      List<TrackingDefinitionContract> definitionContractList = new List<TrackingDefinitionContract>();
      foreach (TrackingDefinitionContract trackingDefinition in enhancedConditionTemplate.customDefinitions.trackingDefinitions)
      {
        if (!defaultTrackingOptions.Contains(trackingDefinition.name))
          definitionContractList.Add(trackingDefinition);
      }
      if (definitionContractList.Count <= 0)
        return;
      enhancedConditionTemplate.customDefinitions.trackingDefinitions = definitionContractList.ToArray();
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (!this.validateEditableTemplate())
        return;
      string duplicateName = this.FindDuplicateName(this.condTemplate.Title, this.conditionTemplates);
      EnhancedConditionTemplate conditionTemplate = (EnhancedConditionTemplate) this.condTemplate.Clone();
      this.removeDefaultTrackingdefination(conditionTemplate);
      conditionTemplate.Title = duplicateName;
      using (EnhancedConditionTemplateDialog conditionTemplateDialog = new EnhancedConditionTemplateDialog(this.session, conditionTemplate))
      {
        int num = (int) conditionTemplateDialog.ShowDialog((IWin32Window) this);
        if (conditionTemplateDialog.DialogResult != DialogResult.OK)
          return;
        this.init();
      }
    }

    private bool validateEditableTemplate()
    {
      bool flag = true;
      if (this.gvConditionTemplates.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      if ((EnhancedConditionTemplate) this.gvConditionTemplates.SelectedItems[0].Tag == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This Enhanced Condtion Template can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    protected virtual string FindDuplicateName(
      string sourceName,
      EnhancedConditionTemplate[] conditionTemplates)
    {
      string lower = sourceName.Trim().ToLower();
      string str = sourceName;
      if (lower.StartsWith("copy of "))
      {
        int num = lower.IndexOf("copy of ");
        str = sourceName.Substring(num + 8);
      }
      else if (lower.StartsWith("copy (") && lower.IndexOf(") of ") > -1)
      {
        int num = lower.IndexOf(") of ");
        str = sourceName.Substring(num + 5);
      }
      int num1 = 0;
      string duplicateName = string.Empty;
      string empty = string.Empty;
      while (duplicateName == string.Empty)
      {
        string strA = num1 != 0 ? "Copy (" + num1.ToString() + ") of " + str : "Copy of " + str;
        bool flag = false;
        for (int index = 0; index < conditionTemplates.Length; ++index)
        {
          if (string.Compare(strA, conditionTemplates[index].Title, true) == 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          duplicateName = strA;
        ++num1;
      }
      if (duplicateName.Length > (int) byte.MaxValue)
        duplicateName = duplicateName.Substring(0, (int) byte.MaxValue);
      return duplicateName;
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (!this.validateEditableTemplate())
        return;
      EnhancedConditionTemplate tag = (EnhancedConditionTemplate) this.gvConditionTemplates.SelectedItems[0].Tag;
      this.removeDefaultTrackingdefination(tag);
      using (EnhancedConditionTemplateDialog conditionTemplateDialog = new EnhancedConditionTemplateDialog(this.session, tag))
      {
        int num = (int) conditionTemplateDialog.ShowDialog();
        if (conditionTemplateDialog.DialogResult != DialogResult.OK)
          return;
        this.init();
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure that you want to permanently delete the condition template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      EnhancedConditionRestApiHelper.AddConditionTemplates(new EnhancedConditionTemplate[1]
      {
        new EnhancedConditionTemplate()
        {
          Id = this.condTemplate.Id
        }
      }, false, ConditionAPIActions.delete.ToString());
      this.init();
    }

    private void btnConditionType_Click(object sender, EventArgs e)
    {
      int num = (int) new ConditionTypeSettingsDialog(this.session, (EDisclosurePackage) null, "").ShowDialog();
      this.init();
    }

    private void btnActivate_Click(object sender, EventArgs e)
    {
      if (this.condTemplate == null)
        return;
      if (EnhancedConditionRestApiHelper.AddConditionTemplates(new EnhancedConditionTemplate[1]
      {
        new EnhancedConditionTemplate()
        {
          Id = this.condTemplate.Id,
          Active = new bool?(true)
        }
      }, true, ConditionAPIActions.update.ToString()) == null)
        return;
      this.init();
    }

    private void btnDeactivate_Click(object sender, EventArgs e)
    {
      if (this.condTemplate == null || Utils.Dialog((IWin32Window) this, "Deactivated conditions will not be available to add within a loan file. This will not impact loans where the condition is already added.\n\rDo you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      if (EnhancedConditionRestApiHelper.AddConditionTemplates(new EnhancedConditionTemplate[1]
      {
        new EnhancedConditionTemplate()
        {
          Id = this.condTemplate.Id,
          Active = new bool?(false)
        }
      }, true, ConditionAPIActions.update.ToString()) == null)
        return;
      this.init();
    }

    private void disableActionButtons()
    {
      this.btnEdit.Enabled = this.btnDelete.Enabled = this.btnDuplicate.Enabled = this.btnActivate.Enabled = this.btnDeactivate.Enabled = false;
    }

    private void textboxNumericOnlyValidation(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void chkAssignRoleSetting_CheckedChanged(object sender, EventArgs e)
    {
      this.session.ConfigurationManager.SetCompanySetting("Policies", "ENHANCEDCONDNAPPLYACCESSROLE", this.chkAssignRoleSetting.Checked.ToString());
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
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.tooltip = new ToolTip(this.components);
      this.btnNew = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.gcConditionTemplates = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lblListOfCounts = new Label();
      this.btnActivate = new Button();
      this.btnDeactivate = new Button();
      this.gvConditionTemplates = new GridView();
      this.panel1 = new Panel();
      this.chkAssignRoleSetting = new CheckBox();
      this.chkExternally = new CheckBox();
      this.chkInternally = new CheckBox();
      this.label3 = new Label();
      this.txtDaysToReceive = new TextBox();
      this.label2 = new Label();
      this.btnConditionType = new Button();
      this.label1 = new Label();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.gcConditionTemplates.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Enabled = false;
      this.btnNew.Location = new Point(1398, 8);
      this.btnNew.Margin = new Padding(4, 5, 4, 5);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(24, 25);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 5;
      this.btnNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnNew, "New");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(1432, 8);
      this.btnDuplicate.Margin = new Padding(4, 5, 4, 5);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(24, 25);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 4;
      this.btnDuplicate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDuplicate, "Duplicate");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(1464, 8);
      this.btnEdit.Margin = new Padding(4, 5, 4, 5);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(24, 25);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(1498, 8);
      this.btnDelete.Margin = new Padding(4, 5, 4, 5);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(24, 25);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 2;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.gcConditionTemplates, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 138f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(1814, 700);
      this.tableLayoutPanel1.TabIndex = 0;
      this.gcConditionTemplates.Controls.Add((Control) this.verticalSeparator1);
      this.gcConditionTemplates.Controls.Add((Control) this.lblListOfCounts);
      this.gcConditionTemplates.Controls.Add((Control) this.btnNew);
      this.gcConditionTemplates.Controls.Add((Control) this.btnDuplicate);
      this.gcConditionTemplates.Controls.Add((Control) this.btnEdit);
      this.gcConditionTemplates.Controls.Add((Control) this.btnDelete);
      this.gcConditionTemplates.Controls.Add((Control) this.btnActivate);
      this.gcConditionTemplates.Controls.Add((Control) this.btnDeactivate);
      this.gcConditionTemplates.Controls.Add((Control) this.gvConditionTemplates);
      this.gcConditionTemplates.Dock = DockStyle.Fill;
      this.gcConditionTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionTemplates.Location = new Point(4, 143);
      this.gcConditionTemplates.Margin = new Padding(4, 5, 4, 5);
      this.gcConditionTemplates.Name = "gcConditionTemplates";
      this.gcConditionTemplates.Size = new Size(1806, 552);
      this.gcConditionTemplates.TabIndex = 3;
      this.gcConditionTemplates.Text = "List of Conditions ";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(1536, 5);
      this.verticalSeparator1.Margin = new Padding(4, 5, 4, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 13;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.lblListOfCounts.AutoSize = true;
      this.lblListOfCounts.BackColor = Color.Transparent;
      this.lblListOfCounts.Location = new Point(162, 8);
      this.lblListOfCounts.Margin = new Padding(4, 0, 4, 0);
      this.lblListOfCounts.Name = "lblListOfCounts";
      this.lblListOfCounts.Size = new Size(0, 20);
      this.lblListOfCounts.TabIndex = 8;
      this.btnActivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnActivate.Enabled = false;
      this.btnActivate.Location = new Point(1552, 3);
      this.btnActivate.Margin = new Padding(4, 5, 4, 5);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(112, 31);
      this.btnActivate.TabIndex = 6;
      this.btnActivate.Text = "Activate";
      this.btnActivate.TextAlign = ContentAlignment.TopCenter;
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.btnDeactivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeactivate.Enabled = false;
      this.btnDeactivate.Location = new Point(1670, 3);
      this.btnDeactivate.Margin = new Padding(4, 5, 4, 5);
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.Size = new Size(112, 31);
      this.btnDeactivate.TabIndex = 7;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.TextAlign = ContentAlignment.TopCenter;
      this.btnDeactivate.UseVisualStyleBackColor = true;
      this.btnDeactivate.Click += new EventHandler(this.btnDeactivate_Click);
      this.gvConditionTemplates.AllowMultiselect = false;
      this.gvConditionTemplates.BorderStyle = BorderStyle.None;
      this.gvConditionTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "InternalId";
      gvColumn1.Tag = (object) "InternalId";
      gvColumn1.Text = "Internal ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Tag = (object) "NAME";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "InternalDescription";
      gvColumn3.Tag = (object) "INTERNALDESCRIPTION";
      gvColumn3.Text = "Internal Description";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Type";
      gvColumn4.Tag = (object) "TYPE";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Category";
      gvColumn5.Tag = (object) "CATEGORY";
      gvColumn5.Text = "Category";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "PriorTo";
      gvColumn6.Tag = (object) "PRIORTO";
      gvColumn6.Text = "Prior To";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "PrintInternal";
      gvColumn7.Tag = (object) "PRINTINTERNALLY";
      gvColumn7.Text = "Print Internal";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "PrintExternal";
      gvColumn8.Tag = (object) "PRINTEXTERNALLY";
      gvColumn8.Text = "Print External";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "DaysToReceive";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Tag = (object) "DAYSTILLDUE";
      gvColumn9.Text = "Days To Receive";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Customized";
      gvColumn10.Tag = (object) "CUSTOMIZED";
      gvColumn10.Text = "Customized";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ConditionStatus";
      gvColumn11.Tag = (object) "CONDITIONSTATUS";
      gvColumn11.Text = "Condition Status";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "LastModifiedBy";
      gvColumn12.Tag = (object) "LASTMODIFIEDBY";
      gvColumn12.Text = "Last Modified By";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "LastModifiedDateTime";
      gvColumn13.SortMethod = GVSortMethod.DateTime;
      gvColumn13.Tag = (object) "LASTMODIFIEDDATETIME";
      gvColumn13.Text = "Last Modified Date/Time";
      gvColumn13.Width = 100;
      this.gvConditionTemplates.Columns.AddRange(new GVColumn[13]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvConditionTemplates.Dock = DockStyle.Fill;
      this.gvConditionTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditionTemplates.Location = new Point(1, 26);
      this.gvConditionTemplates.Margin = new Padding(4, 5, 4, 5);
      this.gvConditionTemplates.Name = "gvConditionTemplates";
      this.gvConditionTemplates.Size = new Size(1804, 525);
      this.gvConditionTemplates.TabIndex = 1;
      this.gvConditionTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditionTemplates.SelectedIndexChanged += new EventHandler(this.gvConditionTemplates_SelectedIndexChanged);
      this.gvConditionTemplates.ItemDoubleClick += new GVItemEventHandler(this.gvConditionTemplates_ItemDoubleClick);
      this.panel1.Controls.Add((Control) this.chkExternally);
      this.panel1.Controls.Add((Control) this.chkInternally);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.txtDaysToReceive);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.btnConditionType);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.chkAssignRoleSetting);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(4, 5);
      this.panel1.Margin = new Padding(4, 5, 4, 5);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1806, 128);
      this.panel1.TabIndex = 4;
      this.chkAssignRoleSetting.AutoSize = true;
      this.chkAssignRoleSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkAssignRoleSetting.Location = new Point(1201, 97);
      this.chkAssignRoleSetting.Name = "chkAssignRoleSetting";
      this.chkAssignRoleSetting.Size = new Size(576, 24);
      this.chkAssignRoleSetting.TabIndex = 7;
      this.chkAssignRoleSetting.Text = "Apply these settings to role even when the role is not assigned to the loan file";
      this.chkAssignRoleSetting.UseVisualStyleBackColor = true;
      this.chkAssignRoleSetting.CheckedChanged += new EventHandler(this.chkAssignRoleSetting_CheckedChanged);
      this.chkExternally.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkExternally.AutoSize = true;
      this.chkExternally.Location = new Point(1670, 60);
      this.chkExternally.Margin = new Padding(4, 5, 4, 5);
      this.chkExternally.Name = "chkExternally";
      this.chkExternally.Size = new Size(103, 24);
      this.chkExternally.TabIndex = 6;
      this.chkExternally.Text = "Externally";
      this.chkExternally.UseVisualStyleBackColor = true;
      this.chkInternally.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkInternally.AutoSize = true;
      this.chkInternally.Location = new Point(1559, 58);
      this.chkInternally.Margin = new Padding(4, 5, 4, 5);
      this.chkInternally.Name = "chkInternally";
      this.chkInternally.Size = new Size(99, 24);
      this.chkInternally.TabIndex = 5;
      this.chkInternally.Text = "Internally";
      this.chkInternally.UseVisualStyleBackColor = true;
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(1394, 58);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(41, 20);
      this.label3.TabIndex = 4;
      this.label3.Text = "Print";
      this.txtDaysToReceive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtDaysToReceive.Location = new Point(1556, 17);
      this.txtDaysToReceive.Margin = new Padding(4, 5, 4, 5);
      this.txtDaysToReceive.MaxLength = 3;
      this.txtDaysToReceive.Name = "txtDaysToReceive";
      this.txtDaysToReceive.Size = new Size(100, 26);
      this.txtDaysToReceive.TabIndex = 3;
      this.txtDaysToReceive.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(1394, 17);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(128, 20);
      this.label2.TabIndex = 2;
      this.label2.Text = "Days To Receive";
      this.btnConditionType.Location = new Point(14, 51);
      this.btnConditionType.Margin = new Padding(4, 5, 4, 5);
      this.btnConditionType.Name = "btnConditionType";
      this.btnConditionType.Size = new Size(152, 35);
      this.btnConditionType.TabIndex = 1;
      this.btnConditionType.Text = "Condition Types";
      this.btnConditionType.UseVisualStyleBackColor = true;
      this.btnConditionType.Click += new EventHandler(this.btnConditionType_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 17);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(330, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Set Default values for adding new Conditions.";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (EnhancedConditionsSetupControl);
      this.Size = new Size(1814, 700);
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.gcConditionTemplates.ResumeLayout(false);
      this.gcConditionTemplates.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
