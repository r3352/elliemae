// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EDisclosureChannelControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EDisclosureChannelControl : UserControl
  {
    private GVItem itemAllApplication;
    private GVItem itemAllThreeDay;
    private GVItem itemAllLock;
    private GVItem itemAllApproval;
    private GVItem itemAllGFE;
    private GVItem itemAllTIL;
    private GVItem itemAllLE;
    private GVItem itemConditionalApplication;
    private GVItem itemConditionalThreeDay;
    private GVItem itemConditionalLock;
    private GVItem itemConditionalApproval;
    private GVItem itemChangedCircumstance;
    private GVItem itemChangedLock;
    private GVItem itemChangedAPR;
    private EDisclosureChannel channel;
    private IEnumerable<Milestone> milestoneList;
    private string condition;
    private Sessions.Session session;
    private bool isSettingsSync;
    private IContainer components;
    private CheckBox chkBroker;
    private CheckBox chkLender;
    private Label lblEntityType;
    private GroupContainer gcInitial;
    private GroupContainer gcRedisclosure;
    private Panel pnlPackages;
    private Panel pnlSpace;
    private GradientPanel gpnlRedisclosureControl;
    private GradientPanel gpnlInitialControl;
    private Label lblInitialControl;
    private Label lblRedisclosureControl;
    private ComboBox cboInitialControl;
    private ComboBox cboRedisclosureControl;
    private Panel pnlEntityType;
    private GridView gvConditional;
    private GridView gvRedisclosure;
    private GridView gvAllLoans;
    private Button btnModifyCondition;
    private CheckBox chkInformationalOnly;
    private ToolTip toolTip;

    public EDisclosureChannelControl()
      : this(Session.DefaultInstance, false)
    {
    }

    public EDisclosureChannelControl(Sessions.Session session, bool isSettingsSync)
    {
      this.session = session;
      this.isSettingsSync = isSettingsSync;
      this.InitializeComponent();
      this.initializeGridItems();
    }

    private void initializeGridItems()
    {
      this.itemAllApplication = new GVItem(new string[2]
      {
        null,
        "At Application"
      });
      this.itemAllThreeDay = new GVItem(new string[2]
      {
        null,
        "Three-day"
      });
      this.itemAllLock = new GVItem(new string[2]
      {
        null,
        "At Lock"
      });
      this.itemAllApproval = new GVItem(new string[2]
      {
        null,
        "Approval"
      });
      this.itemAllGFE = new GVItem(new string[2]
      {
        null,
        "Include GFE"
      });
      this.itemAllTIL = new GVItem(new string[2]
      {
        null,
        "Include TIL"
      });
      this.itemAllLE = new GVItem(new string[2]
      {
        null,
        "Include LE"
      });
      this.gvAllLoans.Items.Add(this.itemAllApplication);
      this.gvAllLoans.Items.Add(this.itemAllThreeDay);
      this.gvAllLoans.Items.Add(this.itemAllLock);
      this.gvAllLoans.Items.Add(this.itemAllApproval);
      this.gvAllLoans.Items.Add(this.itemAllGFE);
      this.gvAllLoans.Items.Add(this.itemAllTIL);
      this.gvAllLoans.Items.Add(this.itemAllLE);
      this.itemConditionalApplication = new GVItem(new string[2]
      {
        null,
        "At Application"
      });
      this.itemConditionalThreeDay = new GVItem(new string[2]
      {
        null,
        "Three-day"
      });
      this.itemConditionalLock = new GVItem(new string[2]
      {
        null,
        "At Lock"
      });
      this.itemConditionalApproval = new GVItem(new string[2]
      {
        null,
        "Approval"
      });
      this.gvConditional.Items.Add(this.itemConditionalApplication);
      this.gvConditional.Items.Add(this.itemConditionalThreeDay);
      this.gvConditional.Items.Add(this.itemConditionalLock);
      this.gvConditional.Items.Add(this.itemConditionalApproval);
      this.itemChangedCircumstance = new GVItem(new string[2]
      {
        null,
        "Changed Circumstance (3168)"
      });
      this.itemChangedLock = new GVItem(new string[2]
      {
        null,
        "Re-Lock (761)"
      });
      this.itemChangedAPR = new GVItem(new string[2]
      {
        null,
        "APR Change (Current APR differs from the Disclosed APR by more than APR Tolerance)"
      });
      this.gvRedisclosure.Items.Add(this.itemChangedCircumstance);
      this.gvRedisclosure.Items.Add(this.itemChangedLock);
      this.gvRedisclosure.Items.Add(this.itemChangedAPR);
      this.gvAllLoans.Dock = DockStyle.Fill;
      this.gvConditional.Dock = DockStyle.Fill;
    }

    public void LoadConfiguration(
      LoanChannel channelType,
      EDisclosureChannel channel,
      IEnumerable<Milestone> milestoneList)
    {
      this.channel = channel;
      this.milestoneList = milestoneList;
      this.chkBroker.Checked = channel.IsBroker;
      this.chkLender.Checked = channel.IsLender;
      this.condition = new LoanChannelNameProvider().GetName((object) channelType);
      if (channelType == LoanChannel.BankedWholesale)
      {
        this.chkInformationalOnly.Visible = true;
        this.chkInformationalOnly.Checked = channel.IsInformationalOnly;
      }
      switch (channel.InitialControl)
      {
        case ControlOptionType.User:
          this.cboInitialControl.SelectedIndex = 2;
          break;
        case ControlOptionType.AllLoans:
          this.cboInitialControl.SelectedIndex = 0;
          break;
        case ControlOptionType.Conditional:
          this.cboInitialControl.SelectedIndex = 1;
          break;
      }
      switch (channel.RedisclosureControl)
      {
        case ControlOptionType.User:
          this.cboRedisclosureControl.SelectedIndex = 1;
          break;
        case ControlOptionType.AllLoans:
          this.cboRedisclosureControl.SelectedIndex = 0;
          break;
      }
      this.gvAllLoans.BeginUpdate();
      this.gvConditional.BeginUpdate();
      this.gvRedisclosure.BeginUpdate();
      this.itemAllApplication.Checked = channel.AllLoans.AtApplication;
      this.itemAllThreeDay.Checked = channel.AllLoans.ThreeDay;
      this.itemAllLock.Checked = channel.AllLoans.AtLock;
      this.itemAllApproval.Checked = channel.AllLoans.Approval;
      this.itemAllGFE.Checked = channel.AllLoans.IncludeGFE;
      this.itemAllTIL.Checked = channel.AllLoans.IncludeTIL;
      this.itemAllLE.Checked = channel.AllLoans.IncludeLE;
      for (int index = 0; index <= 3; ++index)
      {
        GVItem gvItem = (GVItem) null;
        EDisclosurePackage package = (EDisclosurePackage) null;
        switch (index)
        {
          case 0:
            gvItem = this.itemConditionalApplication;
            package = channel.ConditionalApplication;
            break;
          case 1:
            gvItem = this.itemConditionalThreeDay;
            package = channel.ConditionalThreeDay;
            break;
          case 2:
            gvItem = this.itemConditionalLock;
            package = channel.ConditionalLock;
            break;
          case 3:
            gvItem = this.itemConditionalApproval;
            package = channel.ConditionalApproval;
            break;
        }
        gvItem.Checked = package.Enabled;
        gvItem.SubItems[2].CheckBoxVisible = package.Enabled;
        gvItem.SubItems[2].Checked = package.IncludeGFE;
        gvItem.SubItems[3].CheckBoxVisible = package.Enabled;
        gvItem.SubItems[3].Checked = package.IncludeTIL;
        gvItem.SubItems[4].Checked = package.IncludeLE;
        gvItem.SubItems[5].Value = (object) this.getConditionValue(package);
        package.UpdatedExceptionsList = (Dictionary<MilestoneTemplate, string>) null;
      }
      for (int index = 0; index <= 2; ++index)
      {
        GVItem gvItem = (GVItem) null;
        EDisclosurePackage edisclosurePackage = (EDisclosurePackage) null;
        switch (index)
        {
          case 0:
            gvItem = this.itemChangedCircumstance;
            edisclosurePackage = channel.ChangedCircumstance;
            break;
          case 1:
            gvItem = this.itemChangedLock;
            edisclosurePackage = channel.ChangedLock;
            break;
          case 2:
            gvItem = this.itemChangedAPR;
            edisclosurePackage = channel.ChangedAPR;
            break;
        }
        gvItem.Checked = edisclosurePackage.Enabled;
        gvItem.SubItems[2].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[2].Checked = edisclosurePackage.AtApplication;
        gvItem.SubItems[3].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[3].Checked = edisclosurePackage.ThreeDay;
        gvItem.SubItems[4].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[4].Checked = edisclosurePackage.AtLock;
        gvItem.SubItems[5].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[5].Checked = edisclosurePackage.Approval;
        gvItem.SubItems[6].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[6].Checked = edisclosurePackage.IncludeGFE;
        gvItem.SubItems[7].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[7].Checked = edisclosurePackage.IncludeTIL;
        gvItem.SubItems[8].CheckBoxVisible = edisclosurePackage.Enabled;
        gvItem.SubItems[8].Checked = edisclosurePackage.IncludeLE;
      }
      this.gvAllLoans.EndUpdate();
      this.gvConditional.EndUpdate();
      this.gvRedisclosure.EndUpdate();
    }

    private string getConditionValue(EDisclosurePackage package)
    {
      if (!package.Enabled)
        return (string) null;
      switch (package.RequirementType)
      {
        case PackageRequirementType.Fields:
          if (package.RequiredFields != null)
            return string.Join(", ", package.RequiredFields);
          break;
        case PackageRequirementType.Milestone:
          if (package.RequiredMilestone != null)
          {
            Milestone milestone = this.milestoneList.ToList<Milestone>().FirstOrDefault<Milestone>((Func<Milestone, bool>) (item => item.MilestoneID == package.RequiredMilestone));
            if (milestone != null)
              return milestone.Name;
            break;
          }
          break;
      }
      return (string) null;
    }

    private void chkBroker_Click(object sender, EventArgs e)
    {
      this.channel.IsBroker = this.chkBroker.Checked;
      this.OnConfigurationChanged();
    }

    private void chkLender_Click(object sender, EventArgs e)
    {
      this.channel.IsLender = this.chkLender.Checked;
      this.OnConfigurationChanged();
    }

    private void chkInformationalOnly_Click(object sender, EventArgs e)
    {
      this.channel.IsInformationalOnly = this.chkInformationalOnly.Checked;
      this.OnConfigurationChanged();
    }

    private void cboInitialControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag1 = false;
      bool flag2 = false;
      switch (this.cboInitialControl.SelectedIndex)
      {
        case 0:
          flag1 = true;
          break;
        case 1:
          flag2 = true;
          break;
      }
      this.gvAllLoans.Visible = flag1;
      this.gvConditional.Visible = flag2;
      this.btnModifyCondition.Visible = flag2;
    }

    private void cboInitialControl_SelectionChangeCommitted(object sender, EventArgs e)
    {
      switch (this.cboInitialControl.SelectedIndex)
      {
        case 0:
          this.channel.InitialControl = ControlOptionType.AllLoans;
          break;
        case 1:
          this.channel.InitialControl = ControlOptionType.Conditional;
          break;
        case 2:
          this.channel.InitialControl = ControlOptionType.User;
          break;
      }
      this.OnConfigurationChanged();
    }

    private void cboRedisclosureControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboRedisclosureControl.SelectedIndex == 0)
        this.gvRedisclosure.Visible = true;
      else
        this.gvRedisclosure.Visible = false;
    }

    private void cboRedisclosureControl_SelectionChangeCommitted(object sender, EventArgs e)
    {
      switch (this.cboRedisclosureControl.SelectedIndex)
      {
        case 0:
          this.channel.RedisclosureControl = ControlOptionType.AllLoans;
          break;
        case 1:
          this.channel.RedisclosureControl = ControlOptionType.User;
          break;
      }
      this.OnConfigurationChanged();
    }

    private void btnModifyCondition_Click(object sender, EventArgs e)
    {
      LoanChannel loanChannelType = LoanChannel.None;
      if (this.condition.Contains("Retail"))
      {
        this.condition = "RetailChannel";
        loanChannelType = LoanChannel.BankedRetail;
      }
      else if (this.condition.Contains("Wholesale"))
      {
        this.condition = "WholesaleChannel";
        loanChannelType = LoanChannel.BankedWholesale;
      }
      else if (this.condition.Contains("Broker"))
      {
        this.condition = "BrokerChannel";
        loanChannelType = LoanChannel.Brokered;
      }
      if (this.condition.Contains("Correspondent"))
      {
        this.condition = "CorrespondentChannel";
        loanChannelType = LoanChannel.Correspondent;
      }
      EDisclosurePackage package = (EDisclosurePackage) null;
      if (this.itemConditionalApplication.Selected)
      {
        package = this.channel.ConditionalApplication;
        this.condition += "_AtApplication";
        this.condition = this.condition + "_" + this.session.SessionObjects.ConfigurationManager.GetEDisclosureElementAttributeId((int) loanChannelType, 1);
      }
      else if (this.itemConditionalApproval.Selected)
      {
        package = this.channel.ConditionalApproval;
        this.condition += "_Approval";
        this.condition = this.condition + "_" + this.session.SessionObjects.ConfigurationManager.GetEDisclosureElementAttributeId((int) loanChannelType, 4);
      }
      else if (this.itemConditionalLock.Selected)
      {
        package = this.channel.ConditionalLock;
        this.condition += "_AtLock";
        this.condition = this.condition + "_" + this.session.SessionObjects.ConfigurationManager.GetEDisclosureElementAttributeId((int) loanChannelType, 3);
      }
      else if (this.itemConditionalThreeDay.Selected)
      {
        package = this.channel.ConditionalThreeDay;
        this.condition += "_ThreeDay";
        this.condition = this.condition + "_" + this.session.SessionObjects.ConfigurationManager.GetEDisclosureElementAttributeId((int) loanChannelType, 2);
      }
      using (ConditionalRequirementDialog requirementDialog = new ConditionalRequirementDialog(this.session, package, this.condition))
      {
        if (requirementDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        package.UpdatedExceptionsList = requirementDialog.UpdatedExceptionsList;
        this.gvConditional.SelectedItems[0].SubItems[5].Value = (object) this.getConditionValue(package);
        this.OnConfigurationChanged();
      }
    }

    private void gvAllLoans_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Item == this.itemAllApplication)
        this.channel.AllLoans.AtApplication = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllApproval)
        this.channel.AllLoans.Approval = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllGFE)
        this.channel.AllLoans.IncludeGFE = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllLock)
        this.channel.AllLoans.AtLock = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllThreeDay)
        this.channel.AllLoans.ThreeDay = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllTIL)
        this.channel.AllLoans.IncludeTIL = e.SubItem.Checked;
      else if (e.SubItem.Item == this.itemAllLE)
        this.channel.AllLoans.IncludeLE = e.SubItem.Checked;
      this.OnConfigurationChanged();
    }

    private void gvConditional_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.btnModifyCondition.Enabled)
        return;
      this.btnModifyCondition_Click(source, EventArgs.Empty);
    }

    private void gvConditional_SelectedIndexChanged(object sender, EventArgs e)
    {
      GVItem gvItem = (GVItem) null;
      if (this.gvConditional.SelectedItems.Count == 1)
        gvItem = this.gvConditional.SelectedItems[0];
      EDisclosurePackage edisclosurePackage = (EDisclosurePackage) null;
      if (gvItem == this.itemConditionalApplication)
        edisclosurePackage = this.channel.ConditionalApplication;
      else if (gvItem == this.itemConditionalApproval)
        edisclosurePackage = this.channel.ConditionalApproval;
      else if (gvItem == this.itemConditionalLock)
        edisclosurePackage = this.channel.ConditionalLock;
      else if (gvItem == this.itemConditionalThreeDay)
        edisclosurePackage = this.channel.ConditionalThreeDay;
      if (this.isSettingsSync)
        return;
      if (edisclosurePackage != null)
        this.btnModifyCondition.Enabled = edisclosurePackage.Enabled;
      else
        this.btnModifyCondition.Enabled = false;
    }

    private void gvConditional_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      EDisclosurePackage package = (EDisclosurePackage) null;
      if (e.SubItem.Item == this.itemConditionalApplication)
        package = this.channel.ConditionalApplication;
      else if (e.SubItem.Item == this.itemConditionalApproval)
        package = this.channel.ConditionalApproval;
      else if (e.SubItem.Item == this.itemConditionalLock)
        package = this.channel.ConditionalLock;
      else if (e.SubItem.Item == this.itemConditionalThreeDay)
        package = this.channel.ConditionalThreeDay;
      switch (e.SubItem.Index)
      {
        case 0:
          package.Enabled = e.SubItem.Checked;
          e.SubItem.Item.SubItems[2].CheckBoxVisible = package.Enabled;
          e.SubItem.Item.SubItems[3].CheckBoxVisible = package.Enabled;
          e.SubItem.Item.SubItems[4].CheckBoxVisible = package.Enabled;
          e.SubItem.Item.SubItems[5].Value = (object) this.getConditionValue(package);
          if (e.SubItem.Item.Selected)
          {
            this.btnModifyCondition.Enabled = package.Enabled;
            break;
          }
          break;
        case 2:
          package.IncludeGFE = e.SubItem.Checked;
          break;
        case 3:
          package.IncludeTIL = e.SubItem.Checked;
          break;
        case 4:
          package.IncludeLE = e.SubItem.Checked;
          break;
      }
      this.OnConfigurationChanged();
    }

    private void gvRedisclosure_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      EDisclosurePackage edisclosurePackage = (EDisclosurePackage) null;
      if (e.SubItem.Item == this.itemChangedAPR)
        edisclosurePackage = this.channel.ChangedAPR;
      else if (e.SubItem.Item == this.itemChangedCircumstance)
        edisclosurePackage = this.channel.ChangedCircumstance;
      else if (e.SubItem.Item == this.itemChangedLock)
        edisclosurePackage = this.channel.ChangedLock;
      switch (e.SubItem.Index)
      {
        case 0:
          edisclosurePackage.Enabled = e.SubItem.Checked;
          e.SubItem.Item.SubItems[2].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[3].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[4].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[5].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[6].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[7].CheckBoxVisible = edisclosurePackage.Enabled;
          e.SubItem.Item.SubItems[8].CheckBoxVisible = edisclosurePackage.Enabled;
          break;
        case 2:
          edisclosurePackage.AtApplication = e.SubItem.Checked;
          break;
        case 3:
          edisclosurePackage.ThreeDay = e.SubItem.Checked;
          break;
        case 4:
          edisclosurePackage.AtLock = e.SubItem.Checked;
          break;
        case 5:
          edisclosurePackage.Approval = e.SubItem.Checked;
          break;
        case 6:
          edisclosurePackage.IncludeGFE = e.SubItem.Checked;
          break;
        case 7:
          edisclosurePackage.IncludeTIL = e.SubItem.Checked;
          break;
        case 8:
          edisclosurePackage.IncludeLE = e.SubItem.Checked;
          break;
      }
      this.OnConfigurationChanged();
    }

    public event EventHandler ConfigurationChanged;

    public virtual void OnConfigurationChanged()
    {
      if (this.ConfigurationChanged == null)
        return;
      this.ConfigurationChanged((object) this, EventArgs.Empty);
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
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      this.chkBroker = new CheckBox();
      this.chkLender = new CheckBox();
      this.lblEntityType = new Label();
      this.gcInitial = new GroupContainer();
      this.btnModifyCondition = new Button();
      this.gvAllLoans = new GridView();
      this.gvConditional = new GridView();
      this.gpnlInitialControl = new GradientPanel();
      this.cboInitialControl = new ComboBox();
      this.lblInitialControl = new Label();
      this.gcRedisclosure = new GroupContainer();
      this.gvRedisclosure = new GridView();
      this.gpnlRedisclosureControl = new GradientPanel();
      this.cboRedisclosureControl = new ComboBox();
      this.lblRedisclosureControl = new Label();
      this.pnlPackages = new Panel();
      this.pnlSpace = new Panel();
      this.pnlEntityType = new Panel();
      this.chkInformationalOnly = new CheckBox();
      this.toolTip = new ToolTip(this.components);
      this.gcInitial.SuspendLayout();
      this.gpnlInitialControl.SuspendLayout();
      this.gcRedisclosure.SuspendLayout();
      this.gpnlRedisclosureControl.SuspendLayout();
      this.pnlPackages.SuspendLayout();
      this.pnlEntityType.SuspendLayout();
      this.SuspendLayout();
      this.chkBroker.AutoSize = true;
      this.chkBroker.Location = new Point(84, 8);
      this.chkBroker.Name = "chkBroker";
      this.chkBroker.Size = new Size(118, 18);
      this.chkBroker.TabIndex = 1;
      this.chkBroker.Text = "Broker Disclosures";
      this.chkBroker.UseVisualStyleBackColor = true;
      this.chkBroker.Click += new EventHandler(this.chkBroker_Click);
      this.chkLender.AutoSize = true;
      this.chkLender.Location = new Point(212, 8);
      this.chkLender.Name = "chkLender";
      this.chkLender.Size = new Size(120, 18);
      this.chkLender.TabIndex = 2;
      this.chkLender.Text = "Lender Disclosures";
      this.chkLender.UseVisualStyleBackColor = true;
      this.chkLender.Click += new EventHandler(this.chkLender_Click);
      this.lblEntityType.AutoSize = true;
      this.lblEntityType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEntityType.Location = new Point(8, 8);
      this.lblEntityType.Name = "lblEntityType";
      this.lblEntityType.Size = new Size(66, 14);
      this.lblEntityType.TabIndex = 0;
      this.lblEntityType.Text = "Entity Type";
      this.gcInitial.Controls.Add((Control) this.btnModifyCondition);
      this.gcInitial.Controls.Add((Control) this.gvAllLoans);
      this.gcInitial.Controls.Add((Control) this.gvConditional);
      this.gcInitial.Controls.Add((Control) this.gpnlInitialControl);
      this.gcInitial.Dock = DockStyle.Fill;
      this.gcInitial.HeaderForeColor = SystemColors.ControlText;
      this.gcInitial.Location = new Point(4, 0);
      this.gcInitial.Name = "gcInitial";
      this.gcInitial.Size = new Size(713, 153);
      this.gcInitial.TabIndex = 0;
      this.gcInitial.Text = "Initial Packages";
      this.btnModifyCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnModifyCondition.Enabled = false;
      this.btnModifyCondition.Location = new Point(613, 2);
      this.btnModifyCondition.Name = "btnModifyCondition";
      this.btnModifyCondition.Size = new Size(98, 22);
      this.btnModifyCondition.TabIndex = 1;
      this.btnModifyCondition.Text = "Modify Condition";
      this.btnModifyCondition.UseVisualStyleBackColor = true;
      this.btnModifyCondition.Click += new EventHandler(this.btnModifyCondition_Click);
      this.gvAllLoans.AllowColumnResize = false;
      this.gvAllLoans.AllowMultiselect = false;
      this.gvAllLoans.BorderStyle = BorderStyle.None;
      this.gvAllLoans.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colSelect";
      gvColumn1.Text = "Select";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 55;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colPackage";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Package";
      gvColumn2.Width = 277;
      this.gvAllLoans.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAllLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAllLoans.Location = new Point(12, 68);
      this.gvAllLoans.Name = "gvAllLoans";
      this.gvAllLoans.Size = new Size(332, 140);
      this.gvAllLoans.SortOption = GVSortOption.None;
      this.gvAllLoans.TabIndex = 2;
      this.gvAllLoans.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAllLoans.Visible = false;
      this.gvAllLoans.SubItemCheck += new GVSubItemEventHandler(this.gvAllLoans_SubItemCheck);
      this.gvConditional.AllowColumnResize = false;
      this.gvConditional.AllowMultiselect = false;
      this.gvConditional.BorderStyle = BorderStyle.None;
      this.gvConditional.ClearSelectionsOnEmptyRowClick = false;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colSelect";
      gvColumn3.Text = "Select";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 55;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colPackage";
      gvColumn4.Text = "Package";
      gvColumn4.Width = 100;
      gvColumn5.CheckBoxes = true;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colGFE";
      gvColumn5.Text = "Include GFE";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 75;
      gvColumn6.CheckBoxes = true;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colTIL";
      gvColumn6.Text = "Include TIL";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 75;
      gvColumn7.CheckBoxes = true;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colLE";
      gvColumn7.Text = "Include LE";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 75;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colCondition";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "Condition";
      gvColumn8.Width = 3;
      this.gvConditional.Columns.AddRange(new GVColumn[6]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvConditional.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditional.Location = new Point(356, 68);
      this.gvConditional.Name = "gvConditional";
      this.gvConditional.Size = new Size(346, 140);
      this.gvConditional.SortOption = GVSortOption.None;
      this.gvConditional.TabIndex = 3;
      this.gvConditional.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditional.Visible = false;
      this.gvConditional.SelectedIndexChanged += new EventHandler(this.gvConditional_SelectedIndexChanged);
      this.gvConditional.SubItemCheck += new GVSubItemEventHandler(this.gvConditional_SubItemCheck);
      this.gvConditional.ItemDoubleClick += new GVItemEventHandler(this.gvConditional_ItemDoubleClick);
      this.gpnlInitialControl.Borders = AnchorStyles.Bottom;
      this.gpnlInitialControl.Controls.Add((Control) this.cboInitialControl);
      this.gpnlInitialControl.Controls.Add((Control) this.lblInitialControl);
      this.gpnlInitialControl.Dock = DockStyle.Top;
      this.gpnlInitialControl.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpnlInitialControl.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpnlInitialControl.Location = new Point(1, 26);
      this.gpnlInitialControl.Name = "gpnlInitialControl";
      this.gpnlInitialControl.Padding = new Padding(8, 0, 0, 0);
      this.gpnlInitialControl.Size = new Size(711, 31);
      this.gpnlInitialControl.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpnlInitialControl.TabIndex = 0;
      this.cboInitialControl.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboInitialControl.FormattingEnabled = true;
      this.cboInitialControl.Items.AddRange(new object[3]
      {
        (object) "Company selects a package for all loans",
        (object) "Company selects a different package based on condition",
        (object) "User selects a package for each loan (no company control)"
      });
      this.cboInitialControl.Location = new Point(96, 4);
      this.cboInitialControl.Name = "cboInitialControl";
      this.cboInitialControl.Size = new Size(360, 22);
      this.cboInitialControl.TabIndex = 1;
      this.cboInitialControl.SelectedIndexChanged += new EventHandler(this.cboInitialControl_SelectedIndexChanged);
      this.cboInitialControl.SelectionChangeCommitted += new EventHandler(this.cboInitialControl_SelectionChangeCommitted);
      this.lblInitialControl.AutoSize = true;
      this.lblInitialControl.BackColor = Color.Transparent;
      this.lblInitialControl.Location = new Point(8, 8);
      this.lblInitialControl.Name = "lblInitialControl";
      this.lblInitialControl.Size = new Size(75, 14);
      this.lblInitialControl.TabIndex = 0;
      this.lblInitialControl.Text = "Control Option";
      this.gcRedisclosure.Controls.Add((Control) this.gvRedisclosure);
      this.gcRedisclosure.Controls.Add((Control) this.gpnlRedisclosureControl);
      this.gcRedisclosure.Dock = DockStyle.Bottom;
      this.gcRedisclosure.HeaderForeColor = SystemColors.ControlText;
      this.gcRedisclosure.Location = new Point(4, 157);
      this.gcRedisclosure.Name = "gcRedisclosure";
      this.gcRedisclosure.Size = new Size(713, 132);
      this.gcRedisclosure.TabIndex = 2;
      this.gcRedisclosure.Text = "Re-disclosure Packages";
      this.gvRedisclosure.AllowColumnResize = false;
      this.gvRedisclosure.AllowMultiselect = false;
      this.gvRedisclosure.BorderStyle = BorderStyle.None;
      this.gvRedisclosure.ClearSelectionsOnEmptyRowClick = false;
      gvColumn9.CheckBoxes = true;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colSelect";
      gvColumn9.Text = "Select";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 55;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "colCondition";
      gvColumn10.SpringToFit = true;
      gvColumn10.Text = "Condition";
      gvColumn10.Width = 131;
      gvColumn11.CheckBoxes = true;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "colAtApp";
      gvColumn11.Text = "At App";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 75;
      gvColumn12.CheckBoxes = true;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "colThreeDay";
      gvColumn12.Text = "Three-day";
      gvColumn12.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn12.Width = 75;
      gvColumn13.CheckBoxes = true;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "colAtLock";
      gvColumn13.Text = "At Lock";
      gvColumn13.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn13.Width = 75;
      gvColumn14.CheckBoxes = true;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "colApproval";
      gvColumn14.Text = "Approval";
      gvColumn14.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn14.Width = 75;
      gvColumn15.CheckBoxes = true;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "colGFE";
      gvColumn15.Text = "Include GFE";
      gvColumn15.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn15.Width = 75;
      gvColumn16.CheckBoxes = true;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "colTIL";
      gvColumn16.Text = "Include TIL";
      gvColumn16.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn16.Width = 75;
      gvColumn17.CheckBoxes = true;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "colLE";
      gvColumn17.Text = "Include LE";
      gvColumn17.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn17.Width = 75;
      this.gvRedisclosure.Columns.AddRange(new GVColumn[9]
      {
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15,
        gvColumn16,
        gvColumn17
      });
      this.gvRedisclosure.Dock = DockStyle.Fill;
      this.gvRedisclosure.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRedisclosure.Location = new Point(1, 57);
      this.gvRedisclosure.Name = "gvRedisclosure";
      this.gvRedisclosure.Size = new Size(711, 74);
      this.gvRedisclosure.SortOption = GVSortOption.None;
      this.gvRedisclosure.TabIndex = 1;
      this.gvRedisclosure.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvRedisclosure.Visible = false;
      this.gvRedisclosure.SubItemCheck += new GVSubItemEventHandler(this.gvRedisclosure_SubItemCheck);
      this.gpnlRedisclosureControl.Borders = AnchorStyles.Bottom;
      this.gpnlRedisclosureControl.Controls.Add((Control) this.cboRedisclosureControl);
      this.gpnlRedisclosureControl.Controls.Add((Control) this.lblRedisclosureControl);
      this.gpnlRedisclosureControl.Dock = DockStyle.Top;
      this.gpnlRedisclosureControl.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpnlRedisclosureControl.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpnlRedisclosureControl.Location = new Point(1, 26);
      this.gpnlRedisclosureControl.Name = "gpnlRedisclosureControl";
      this.gpnlRedisclosureControl.Padding = new Padding(8, 0, 0, 0);
      this.gpnlRedisclosureControl.Size = new Size(711, 31);
      this.gpnlRedisclosureControl.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpnlRedisclosureControl.TabIndex = 0;
      this.cboRedisclosureControl.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRedisclosureControl.FormattingEnabled = true;
      this.cboRedisclosureControl.Items.AddRange(new object[2]
      {
        (object) "Company selects a package for all loans",
        (object) "User selects a package for each loan (no company control)"
      });
      this.cboRedisclosureControl.Location = new Point(96, 4);
      this.cboRedisclosureControl.Name = "cboRedisclosureControl";
      this.cboRedisclosureControl.Size = new Size(360, 22);
      this.cboRedisclosureControl.TabIndex = 1;
      this.cboRedisclosureControl.SelectedIndexChanged += new EventHandler(this.cboRedisclosureControl_SelectedIndexChanged);
      this.cboRedisclosureControl.SelectionChangeCommitted += new EventHandler(this.cboRedisclosureControl_SelectionChangeCommitted);
      this.lblRedisclosureControl.AutoSize = true;
      this.lblRedisclosureControl.BackColor = Color.Transparent;
      this.lblRedisclosureControl.Location = new Point(8, 8);
      this.lblRedisclosureControl.Name = "lblRedisclosureControl";
      this.lblRedisclosureControl.Size = new Size(75, 14);
      this.lblRedisclosureControl.TabIndex = 0;
      this.lblRedisclosureControl.Text = "Control Option";
      this.pnlPackages.Controls.Add((Control) this.gcInitial);
      this.pnlPackages.Controls.Add((Control) this.pnlSpace);
      this.pnlPackages.Controls.Add((Control) this.gcRedisclosure);
      this.pnlPackages.Dock = DockStyle.Fill;
      this.pnlPackages.Location = new Point(0, 32);
      this.pnlPackages.Name = "pnlPackages";
      this.pnlPackages.Padding = new Padding(4, 0, 4, 4);
      this.pnlPackages.Size = new Size(721, 293);
      this.pnlPackages.TabIndex = 1;
      this.pnlSpace.Dock = DockStyle.Bottom;
      this.pnlSpace.Location = new Point(4, 153);
      this.pnlSpace.Name = "pnlSpace";
      this.pnlSpace.Size = new Size(713, 4);
      this.pnlSpace.TabIndex = 1;
      this.pnlEntityType.Controls.Add((Control) this.chkInformationalOnly);
      this.pnlEntityType.Controls.Add((Control) this.lblEntityType);
      this.pnlEntityType.Controls.Add((Control) this.chkBroker);
      this.pnlEntityType.Controls.Add((Control) this.chkLender);
      this.pnlEntityType.Dock = DockStyle.Top;
      this.pnlEntityType.Location = new Point(0, 0);
      this.pnlEntityType.Name = "pnlEntityType";
      this.pnlEntityType.Size = new Size(721, 32);
      this.pnlEntityType.TabIndex = 0;
      this.chkInformationalOnly.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkInformationalOnly.AutoSize = true;
      this.chkInformationalOnly.Location = new Point(518, 8);
      this.chkInformationalOnly.Name = "chkInformationalOnly";
      this.chkInformationalOnly.Size = new Size(202, 18);
      this.chkInformationalOnly.TabIndex = 3;
      this.chkInformationalOnly.Text = "Make the Package Informational Only";
      this.toolTip.SetToolTip((Control) this.chkInformationalOnly, "This option will make all packages informational. Informational packages do not provide signing option or fax coversheet.");
      this.chkInformationalOnly.UseVisualStyleBackColor = true;
      this.chkInformationalOnly.Visible = false;
      this.chkInformationalOnly.Click += new EventHandler(this.chkInformationalOnly_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlPackages);
      this.Controls.Add((Control) this.pnlEntityType);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EDisclosureChannelControl);
      this.Size = new Size(721, 325);
      this.gcInitial.ResumeLayout(false);
      this.gpnlInitialControl.ResumeLayout(false);
      this.gpnlInitialControl.PerformLayout();
      this.gcRedisclosure.ResumeLayout(false);
      this.gpnlRedisclosureControl.ResumeLayout(false);
      this.gpnlRedisclosureControl.PerformLayout();
      this.pnlPackages.ResumeLayout(false);
      this.pnlEntityType.ResumeLayout(false);
      this.pnlEntityType.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
