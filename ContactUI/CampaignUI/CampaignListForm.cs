// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignListForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using Infragistics.Win;
using Infragistics.Win.Layout;
using Infragistics.Win.UltraWinTree;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignListForm : Form
  {
    private CampaignData campaignData;
    private CampaignStatusNameProvider statusNames = new CampaignStatusNameProvider();
    private ActivityTypeNameProvider activityNames = new ActivityTypeNameProvider();
    private FlowLayoutPanel flowLayoutPanel1;
    private ActivityDateSelectionNameProvider activitySelectionNames = new ActivityDateSelectionNameProvider();
    private IContainer components;
    private System.Windows.Forms.ToolTip tipCampaignList;
    private ContextMenu ctxCampaignList;
    private MenuItem mnuNewCampaign;
    private MenuItem menuItem2;
    private MenuItem mnuOpen;
    private MenuItem mnuDuplicate;
    private MenuItem mnuDelete;
    private MenuItem menuItem6;
    private MenuItem mnuStartCampaign;
    private MenuItem mnuStopCampaign;
    private CheckBox chkIncludeStoppedCampaigns;
    private CheckBox chkIncludeNotStartedCampaigns;
    private ErrorProvider errCampaignList;
    private Label lblCampaignCount;
    private Label lblTasksDueCount;
    private MenuItem mnuExpandAdd;
    private MenuItem mnuCollapseAll;
    private MenuItem menuItem3;
    private MenuItem menuItem1;
    private MenuItem mnuManageTemplates;
    private Button btnStartCampaign;
    private Button btnStopCampaign;
    private Button btnCampaignTemplates;
    private VerticalSeparator verticalSeparator1;
    private TableContainer pnlCampaignTable;
    private StandardIconButton icnDeleteCampaign;
    private StandardIconButton icnEditCampaign;
    private StandardIconButton icnDuplicateCampaign;
    private StandardIconButton icnNewCampaign;
    private UltraTree utCampaignList;
    private UltraTreeNode utTooltipNode;

    public int ActiveCampaignIndex
    {
      get
      {
        if (this.utCampaignList.Nodes == null || this.utCampaignList.Nodes.Count <= 0 || this.utCampaignList.ActiveNode == null)
          return -1;
        return this.utCampaignList.ActiveNode.Level == 0 ? this.utCampaignList.Nodes.IndexOf(this.utCampaignList.ActiveNode.Key) : this.utCampaignList.Nodes.IndexOf(this.utCampaignList.ActiveNode.Parent.Key);
      }
    }

    public bool HidingStoppedCampaigns => !this.chkIncludeStoppedCampaigns.Checked;

    public bool HidingNotStartedCampaigns => !this.chkIncludeNotStartedCampaigns.Checked;

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
          flag = this.icnNewCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
          flag = this.icnEditCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
          flag = this.icnDuplicateCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
          flag = this.icnDeleteCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
          flag = this.btnStartCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
          flag = this.btnStopCampaign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = this.btnCampaignTemplates.Enabled;
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
          flag = this.icnNewCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
          flag = this.icnEditCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
          flag = this.icnDuplicateCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
          flag = this.icnDeleteCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
          flag = this.btnStartCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
          flag = this.btnStopCampaign.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = this.btnCampaignTemplates.Visible;
          break;
      }
      return flag;
    }

    public void TriggerContactAction(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
          this.icnNewCampaign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
          this.icnEditCampaign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
          this.icnDuplicateCampaign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
          this.icnDeleteCampaign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
          this.btnStartCampaign.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
          this.btnStopCampaign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          this.btnCampaignTemplates.PerformClick();
          break;
      }
    }

    public CampaignListForm()
    {
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaignData.ActivityUpdatedEvent += new ActivityUpdatedEventHandler(this.campaignData_ActivityUpdatedEvent);
      this.campaignData.CampaignDataChangedEvent += new CampaignDataChangedEventHandler(this.campaignData_CampaignDataChangedEvent);
      CampaignListForm.RootOnlyAlphaComparer onlyAlphaComparer = new CampaignListForm.RootOnlyAlphaComparer(0);
      this.utCampaignList.ColumnSettings.RootColumnSet.Columns["Name"].SortComparer = (IComparer) onlyAlphaComparer;
      this.utCampaignList.ColumnSettings.RootColumnSet.Columns["Status"].SortComparer = (IComparer) onlyAlphaComparer;
      this.utCampaignList.ColumnSettings.RootColumnSet.Columns["ContactType"].SortComparer = (IComparer) onlyAlphaComparer;
      this.utCampaignList.ColumnSettings.RootColumnSet.Columns["TasksDue"].SortComparer = (IComparer) new CampaignListForm.RootOnlyNumericComparer(0);
      this.PopulateCampaignListForm();
      UltraTreeNodeColumn column = this.utCampaignList.ColumnSettings.RootColumnSet.Columns["TasksDue"];
      this.utCampaignList.ColumnSettings.RootColumnSet.SortedColumns.Clear();
      this.utCampaignList.ColumnSettings.RootColumnSet.SortedColumns.Add(column);
      column.SortType = SortType.Descending;
      this.utCampaignList.RefreshSort(0);
      this.utCampaignList.TopNode = (UltraTreeNode) null;
      this.collapseAll();
    }

    public void GetCampaignData()
    {
      this.campaignData.CampaignCollectionCriteria.ContactTypes = new EllieMae.EMLite.ContactUI.ContactType[2]
      {
        EllieMae.EMLite.ContactUI.ContactType.Borrower,
        EllieMae.EMLite.ContactUI.ContactType.BizPartner
      };
      CampaignCollectionCriteria collectionCriteria = this.campaignData.CampaignCollectionCriteria;
      DateTime[] dateTimeArray = new DateTime[2];
      dateTimeArray[0] = new DateTime(2000, 1, 1, 0, 0, 0);
      DateTime today = DateTime.Today;
      int year = today.Year;
      today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = today.Day;
      dateTimeArray[1] = new DateTime(year, month, day, 23, 59, 59);
      collectionCriteria.ActivityDateRange = dateTimeArray;
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) CampaignStatus.Running);
      if (this.chkIncludeStoppedCampaigns.Checked)
        arrayList.Add((object) CampaignStatus.Stopped);
      if (this.chkIncludeNotStartedCampaigns.Checked)
        arrayList.Add((object) CampaignStatus.NotStarted);
      this.campaignData.CampaignCollectionCriteria.CampaignStatuses = (CampaignStatus[]) arrayList.ToArray(typeof (CampaignStatus));
      this.campaignData.LoadCampaignData();
      this.PopulateCampaignListForm();
    }

    public void PopulateCampaignListForm() => this.populateCampaignList();

    public void RefreshCampaignListForm() => this.refreshCampaignList();

    public bool MoveToCampaignNode(int campaignId)
    {
      if (this.utCampaignList.ActiveNode.Level != 0)
        this.utCampaignList.ActiveNode = this.utCampaignList.ActiveNode.Parent;
      UltraTreeNode activeNode = this.utCampaignList.ActiveNode;
      this.utCampaignList.PerformAction(UltraTreeAction.FirstNode, false, false);
      do
        ;
      while ((!(this.utCampaignList.ActiveNode.Tag is EllieMae.EMLite.Campaign.Campaign tag) || tag.CampaignId != campaignId) && this.utCampaignList.PerformAction(UltraTreeAction.NextNode, false, false));
      if (tag == null || tag.CampaignId != campaignId)
      {
        this.utCampaignList.ActiveNode = activeNode;
        return false;
      }
      this.campaignData.ActiveCampaign = tag;
      return true;
    }

    public void MoveToNextCampaignNode()
    {
      if (this.utCampaignList.ActiveNode.Level != 0)
        this.utCampaignList.ActiveNode = this.utCampaignList.ActiveNode.Parent;
      UltraTreeNode activeNode = this.utCampaignList.ActiveNode;
      while (UltraTreeState.NodeLast != (this.utCampaignList.CurrentState & UltraTreeState.NodeLast))
      {
        string key1 = this.utCampaignList.ActiveNode.Key;
        this.utCampaignList.PerformAction(UltraTreeAction.NextNode, false, false);
        string key2 = this.utCampaignList.ActiveNode.Key;
        if (UltraTreeState.NodeChild != (this.utCampaignList.CurrentState & UltraTreeState.NodeChild) || key1 == key2)
          break;
      }
      if (UltraTreeState.NodeChild == (this.utCampaignList.CurrentState & UltraTreeState.NodeChild))
        this.utCampaignList.ActiveNode = activeNode;
      this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) this.utCampaignList.ActiveNode.Tag;
    }

    public void MoveToPrevCampaignNode()
    {
      if (this.utCampaignList.ActiveNode.Level != 0)
        this.utCampaignList.ActiveNode = this.utCampaignList.ActiveNode.Parent;
      while (UltraTreeState.NodeFirst != (this.utCampaignList.CurrentState & UltraTreeState.NodeFirst))
      {
        string key1 = this.utCampaignList.ActiveNode.Key;
        this.utCampaignList.PerformAction(UltraTreeAction.PrevNode, false, false);
        string key2 = this.utCampaignList.ActiveNode.Key;
        if (UltraTreeState.NodeChild != (this.utCampaignList.CurrentState & UltraTreeState.NodeChild) || key1 == key2)
          break;
      }
      this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) this.utCampaignList.ActiveNode.Tag;
    }

    private void populateCampaignList()
    {
      this.lblCampaignCount.Text = "Campaigns (0) -";
      this.lblTasksDueCount.Text = "Total 0 Tasks Due";
      this.lblTasksDueCount.Left = this.lblCampaignCount.Right + 1;
      int totalTasksDue = 0;
      int activeCampaignIndex = this.ActiveCampaignIndex;
      this.utCampaignList.AfterSelect -= new AfterNodeSelectEventHandler(this.utCampaignList_AfterSelect);
      this.utCampaignList.Nodes.Clear();
      this.utCampaignList.AfterSelect += new AfterNodeSelectEventHandler(this.utCampaignList_AfterSelect);
      if (this.campaignData.Campaigns == null || 0 >= this.campaignData.Campaigns.Count)
        return;
      UltraTreeNode ultraTreeNode1 = (UltraTreeNode) null;
      UltraTreeNode ultraTreeNode2 = (UltraTreeNode) null;
      UltraTreeNode ultraTreeNode3 = (UltraTreeNode) null;
      int campaignId = this.campaignData.ActiveCampaign == null ? 0 : this.campaignData.ActiveCampaign.CampaignId;
      int campaignStepId = this.campaignData.ActiveCampaignStep == null ? 0 : this.campaignData.ActiveCampaignStep.CampaignStepId;
      foreach (EllieMae.EMLite.Campaign.Campaign campaign in (CollectionBase) this.campaignData.Campaigns)
      {
        int num1 = 0;
        UltraTreeNode ultraTreeNode4 = this.utCampaignList.Nodes.Add();
        if (ultraTreeNode1 == null)
          ultraTreeNode1 = ultraTreeNode4;
        if (campaignId == campaign.CampaignId)
          ultraTreeNode2 = ultraTreeNode4;
        ultraTreeNode4.Key = string.Format("{0}-{1}", (object) campaign.CampaignId, (object) 0);
        ultraTreeNode4.Tag = (object) campaign;
        ultraTreeNode4.Cells["Name"].Value = (object) (campaign.CampaignName + " (" + campaign.CampaignSteps.Count.ToString() + ")");
        ultraTreeNode4.Cells["Status"].Value = (object) this.statusNames.GetName((object) campaign.Status);
        ultraTreeNode4.Cells["ContactType"].Value = (object) Enum.Format(typeof (EllieMae.EMLite.ContactUI.ContactType), (object) campaign.ContactType, "G");
        ultraTreeNode4.Cells["TasksDue"].Appearance.FontData.Bold = DefaultableBoolean.True;
        UltraTreeNodeCell cell1 = ultraTreeNode4.Cells["LastActionDate"];
        DateTime lastActivityDate;
        string str1;
        if (!(DateTime.MinValue != campaign.LastActivityDate))
        {
          str1 = string.Empty;
        }
        else
        {
          lastActivityDate = campaign.LastActivityDate;
          str1 = lastActivityDate.ToShortDateString();
        }
        cell1.Value = (object) str1;
        if (campaign.CampaignSteps != null)
        {
          foreach (CampaignStep campaignStep in (CollectionBase) campaign.CampaignSteps)
          {
            UltraTreeNode ultraTreeNode5 = ultraTreeNode4.Nodes.Add();
            if (campaignStepId == campaignStep.CampaignStepId)
              ultraTreeNode3 = ultraTreeNode5;
            ultraTreeNode5.Key = string.Format("{0}-{1}", (object) campaign.CampaignId, (object) campaignStep.CampaignStepId);
            ultraTreeNode5.Tag = (object) campaignStep;
            UltraTreeNodeCell cell2 = ultraTreeNode5.Cells["Name"];
            int num2 = campaignStep.StepNumber;
            string str2 = num2.ToString() + ". " + campaignStep.StepName;
            cell2.Value = (object) str2;
            ultraTreeNode5.Cells["TaskType"].Value = (object) this.activityNames.GetName((object) campaignStep.ActivityType);
            UltraTreeNodeCell cell3 = ultraTreeNode5.Cells["TasksDue"];
            num2 = campaignStep.TasksDueCount;
            string str3 = num2.ToString();
            cell3.Value = (object) str3;
            UltraTreeNodeCell cell4 = ultraTreeNode5.Cells["Delay"];
            num2 = campaignStep.StepOffset;
            string str4 = num2.ToString() + " days";
            cell4.Value = (object) str4;
            UltraTreeNodeCell cell5 = ultraTreeNode5.Cells["LastActionDate"];
            string str5;
            if (!(DateTime.MinValue != campaignStep.LastActivityDate))
            {
              str5 = string.Empty;
            }
            else
            {
              lastActivityDate = campaignStep.LastActivityDate;
              str5 = lastActivityDate.ToShortDateString();
            }
            cell5.Value = (object) str5;
            num1 += campaignStep.TasksDueCount;
            totalTasksDue += campaignStep.TasksDueCount;
          }
        }
        ultraTreeNode4.Cells["TasksDue"].Value = (object) num1.ToString();
      }
      if (ultraTreeNode2 != null)
      {
        ultraTreeNode2.BringIntoView();
        this.utCampaignList.ActiveNode = ultraTreeNode2;
        ultraTreeNode2.Selected = true;
      }
      else if (-1 != activeCampaignIndex && 0 < this.campaignData.Campaigns.Count)
      {
        UltraTreeNode ultraTreeNode6 = (UltraTreeNode) this.utCampaignList.Nodes.GetItem(activeCampaignIndex == 0 ? 0 : (activeCampaignIndex - 1 < this.campaignData.Campaigns.Count ? activeCampaignIndex - 1 : this.campaignData.Campaigns.Count - 1));
        ultraTreeNode6.BringIntoView();
        this.utCampaignList.ActiveNode = ultraTreeNode6;
        ultraTreeNode6.Selected = true;
      }
      else
        ultraTreeNode1.BringIntoView();
      this.lblCampaignCount.Text = "Campaigns (" + this.campaignData.Campaigns.Count.ToString() + ") -";
      this.lblTasksDueCount.Text = "Total " + totalTasksDue.ToString() + " Tasks Due";
      this.lblTasksDueCount.Left = this.lblCampaignCount.Right + 1;
      this.campaignData.UpdateTotalTasksDue(totalTasksDue);
    }

    private void refreshCampaignList()
    {
      this.lblCampaignCount.Text = "Campaigns (0) -";
      this.lblTasksDueCount.Text = "Total 0 Tasks Due";
      this.lblTasksDueCount.Left = this.lblCampaignCount.Right + 1;
      int totalTasksDue = 0;
      if (0 >= this.utCampaignList.Nodes.Count)
        return;
      string str1 = string.Empty;
      string str2 = string.Empty;
      if (this.campaignData.ActiveCampaign != null)
        str1 = string.Format("{0}-{1}", (object) this.campaignData.ActiveCampaign.CampaignId, (object) 0);
      if (this.campaignData.ActiveCampaignStep != null)
        str2 = string.Format("{0}-{1}", (object) this.campaignData.ActiveCampaign.CampaignId, (object) this.campaignData.ActiveCampaignStep.CampaignStepId);
      UltraTreeNode ultraTreeNode = (UltraTreeNode) null;
      foreach (UltraTreeNode node1 in this.utCampaignList.Nodes)
      {
        int num = 0;
        EllieMae.EMLite.Campaign.Campaign tag1 = (EllieMae.EMLite.Campaign.Campaign) node1.Tag;
        node1.Cells["Status"].Value = (object) this.statusNames.GetName((object) tag1.Status);
        UltraTreeNodeCell cell1 = node1.Cells["LastActionDate"];
        DateTime lastActivityDate;
        string str3;
        if (!(DateTime.MinValue != tag1.LastActivityDate))
        {
          str3 = string.Empty;
        }
        else
        {
          lastActivityDate = tag1.LastActivityDate;
          str3 = lastActivityDate.ToShortDateString();
        }
        cell1.Value = (object) str3;
        if (str1 == node1.Key)
          ultraTreeNode = node1;
        if (node1.Nodes != null)
        {
          foreach (UltraTreeNode node2 in node1.Nodes)
          {
            CampaignStep tag2 = (CampaignStep) node2.Tag;
            node2.Cells["TasksDue"].Value = (object) tag2.TasksDueCount.ToString();
            UltraTreeNodeCell cell2 = node2.Cells["LastActionDate"];
            string str4;
            if (!(DateTime.MinValue != tag2.LastActivityDate))
            {
              str4 = string.Empty;
            }
            else
            {
              lastActivityDate = tag2.LastActivityDate;
              str4 = lastActivityDate.ToShortDateString();
            }
            cell2.Value = (object) str4;
            num += tag2.TasksDueCount;
            totalTasksDue += tag2.TasksDueCount;
            if (node1.Expanded && str2 == node2.Key)
              ultraTreeNode = node2;
          }
        }
        node1.Cells["TasksDue"].Value = (object) num.ToString();
      }
      if (ultraTreeNode != null)
      {
        ultraTreeNode.BringIntoView();
        ultraTreeNode.Selected = true;
      }
      this.lblCampaignCount.Text = "Campaigns (" + this.campaignData.Campaigns.Count.ToString() + ") -";
      this.lblTasksDueCount.Text = "Total " + totalTasksDue.ToString() + " Tasks Due";
      this.lblTasksDueCount.Left = this.lblCampaignCount.Right + 1;
      this.campaignData.UpdateTotalTasksDue(totalTasksDue);
    }

    private void adjustColumnWidths()
    {
    }

    private void expandAll()
    {
      this.utCampaignList.ExpandAll();
      this.utCampaignList.SelectedNodes.Clear();
      this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) null;
      this.setButtonStates();
    }

    private void collapseAll()
    {
      this.utCampaignList.CollapseAll();
      this.utCampaignList.SelectedNodes.Clear();
      this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) null;
      this.setButtonStates();
    }

    private void setButtonStates()
    {
      this.icnDuplicateCampaign.Enabled = false;
      this.icnEditCampaign.Enabled = false;
      this.icnDeleteCampaign.Enabled = false;
      this.btnStartCampaign.Enabled = false;
      this.btnStopCampaign.Enabled = false;
      if (this.campaignData.ActiveCampaign == null)
        return;
      this.icnDuplicateCampaign.Enabled = true;
      this.icnEditCampaign.Enabled = true;
      this.icnDeleteCampaign.Enabled = true;
      this.btnStopCampaign.Enabled = this.campaignData.ActiveCampaign.Status == CampaignStatus.Running;
      this.btnStartCampaign.Enabled = !this.btnStopCampaign.Enabled;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void icnNewCampaign_Click(object sender, EventArgs e)
    {
      this.mnuNewCampaign_Click(sender, e);
    }

    private void icnDuplicateCampaign_Click(object sender, EventArgs e)
    {
      this.mnuDuplicate_Click(sender, e);
    }

    private void icnEditCampaign_Click(object sender, EventArgs e) => this.mnuOpen_Click(sender, e);

    private void icnDeleteCampaign_Click(object sender, EventArgs e)
    {
      this.mnuDelete_Click(sender, e);
    }

    private void btnStartCampaign_Click(object sender, EventArgs e)
    {
      this.mnuStartCampaign_Click(sender, e);
    }

    private void btnStopCampaign_Click(object sender, EventArgs e)
    {
      this.mnuStopCampaign_Click(sender, e);
    }

    private void btnCampaignTemplates_Click(object sender, EventArgs e)
    {
      this.mnuManageTmplates_Click(sender, e);
    }

    private void chkIncludeStoppedCampaigns_CheckedChanged(object sender, EventArgs e)
    {
      this.GetCampaignData();
    }

    private void chkIncludeNotStartedCampaigns_CheckedChanged(object sender, EventArgs e)
    {
      this.GetCampaignData();
    }

    private void ctxCampaignList_Popup(object sender, EventArgs e)
    {
      this.mnuNewCampaign.Enabled = true;
      this.mnuOpen.Enabled = false;
      this.mnuDuplicate.Enabled = false;
      this.mnuDelete.Enabled = false;
      this.mnuStartCampaign.Enabled = false;
      this.mnuStopCampaign.Enabled = false;
      if (this.campaignData.ActiveCampaign == null)
        return;
      this.mnuOpen.Enabled = true;
      this.mnuDuplicate.Enabled = true;
      this.mnuDelete.Enabled = true;
      if (CampaignStatus.NotStarted == this.campaignData.ActiveCampaign.Status || CampaignStatus.Stopped == this.campaignData.ActiveCampaign.Status)
        this.mnuStartCampaign.Enabled = true;
      else
        this.mnuStopCampaign.Enabled = true;
    }

    private void mnuNewCampaign_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("NewCampaign"));
    }

    private void mnuOpen_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("OpenCampaign"));
    }

    private void mnuDuplicate_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("DuplicateCampaign"));
    }

    private void mnuDelete_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("DeleteCampaign"));
    }

    private void mnuExpandAdd_Click(object sender, EventArgs e) => this.expandAll();

    private void mnuCollapseAll_Click(object sender, EventArgs e) => this.collapseAll();

    private void mnuStartCampaign_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("StartCampaign"));
    }

    private void mnuStopCampaign_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("StopCampaign"));
    }

    private void mnuManageTmplates_Click(object sender, EventArgs e)
    {
      this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("ManageTemplates"));
    }

    private void campaignData_ActivityUpdatedEvent(object sender, EventArgs e)
    {
      this.populateCampaignList();
    }

    private void campaignData_CampaignDataChangedEvent(object sender, EventArgs e)
    {
      this.icnDuplicateCampaign.Enabled = false;
      this.icnEditCampaign.Enabled = false;
      this.icnDeleteCampaign.Enabled = false;
      this.btnStartCampaign.Enabled = false;
      this.btnStopCampaign.Enabled = false;
    }

    private void utCampaignList_AfterNodeLayoutItemResize(
      object sender,
      AfterNodeLayoutItemResizeEventArgs e)
    {
      this.adjustColumnWidths();
    }

    private void utCampaignList_Resize(object sender, EventArgs e) => this.adjustColumnWidths();

    private void utCampaignList_MouseDown(object sender, MouseEventArgs e)
    {
      UltraTreeNode nodeFromPoint = this.utCampaignList.GetNodeFromPoint(e.X, e.Y);
      if (nodeFromPoint != null)
      {
        this.utCampaignList.ActiveNode = nodeFromPoint;
        nodeFromPoint.Selected = true;
      }
      else
      {
        this.utCampaignList.SelectedNodes.Clear();
        this.utCampaignList.ActiveNode = (UltraTreeNode) null;
        this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) null;
        this.setButtonStates();
      }
    }

    private void utCampaignList_Click(object sender, EventArgs e)
    {
      if (this.utCampaignList.Nodes == null || this.utCampaignList.Nodes.Count <= 0)
        return;
      Point client = ((Control) this.utCampaignList).PointToClient(Control.MousePosition);
      UltraTreeNode nodeFromPoint = this.utCampaignList.GetNodeFromPoint(client);
      if (nodeFromPoint == null)
        return;
      if (nodeFromPoint.Level == 0)
      {
        this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) this.utCampaignList.ActiveNode.Tag;
        if (this.utCampaignList.UIElement.ElementFromPoint(client) is Infragistics.Win.UltraWinTree.ExpansionIndicatorUIElement)
          return;
        this.utCampaignList.ActiveNode.Expanded = !this.utCampaignList.ActiveNode.Expanded;
      }
      else
        this.campaignData.ActiveCampaignStep = (CampaignStep) this.utCampaignList.ActiveNode.Tag;
    }

    private void utCampaignList_DoubleClick(object sender, EventArgs e)
    {
      if (this.utCampaignList.Nodes == null || this.utCampaignList.Nodes.Count <= 0 || this.utCampaignList.GetNodeFromPoint(((Control) this.utCampaignList).PointToClient(Control.MousePosition)) == null)
        return;
      if (this.utCampaignList.ActiveNode.Level == 0)
        this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) this.utCampaignList.ActiveNode.Tag;
      else
        this.campaignData.ActiveCampaignStep = (CampaignStep) this.utCampaignList.ActiveNode.Tag;
      if (CampaignStatus.NotStarted == this.campaignData.ActiveCampaign.Status)
        this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("OpenCampaign"));
      else
        this.OnDisplayDetailEvent(EventArgs.Empty);
    }

    private void utCampaignList_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
    {
      if (this.utCampaignList.Nodes != null && this.utCampaignList.Nodes.Count > 0)
        return;
      ((CancelEventArgs) e).Cancel = true;
    }

    private void utCampaignList_AfterSortChange(object sender, AfterSortChangeEventArgs e)
    {
      this.utCampaignList.Nodes.Override.SortComparer = e.OriginalSortedColumns[0].SortComparer;
      this.utCampaignList.Nodes.Override.Sort = e.OriginalSortedColumns[0].SortType;
      Color window = SystemColors.Window;
      Color neutral3 = EncompassColors.Neutral3;
      Color color = neutral3;
      foreach (UltraTreeNode node1 in this.utCampaignList.Nodes)
      {
        color = neutral3 == color ? window : neutral3;
        node1.Override.NodeAppearance.BackColor = color;
        if (node1.Nodes != null)
        {
          foreach (UltraTreeNode node2 in node1.Nodes)
            node2.Override.NodeAppearance.BackColor = color;
        }
      }
    }

    private void utCampaignList_BeforeSelect(object sender, BeforeSelectEventArgs e)
    {
      foreach (UltraTreeNode selectedNode in this.utCampaignList.SelectedNodes)
        selectedNode.Override.HotTracking = DefaultableBoolean.Default;
      foreach (UltraTreeNode newSelection in e.NewSelections)
        newSelection.Override.HotTracking = DefaultableBoolean.False;
    }

    private void utCampaignList_AfterSelect(object sender, SelectEventArgs e)
    {
      if (this.utCampaignList.Nodes == null || this.utCampaignList.Nodes.Count <= 0)
        return;
      this.utCampaignList.HotTrackingNode = (UltraTreeNode) null;
      if (this.utCampaignList.ActiveNode.IsRootLevelNode)
        this.campaignData.ActiveCampaign = (EllieMae.EMLite.Campaign.Campaign) this.utCampaignList.ActiveNode.Tag;
      else
        this.campaignData.ActiveCampaignStep = (CampaignStep) this.utCampaignList.ActiveNode.Tag;
      this.setButtonStates();
    }

    private void utCampaignList_MouseMove(object sender, MouseEventArgs e)
    {
      UltraTreeNode nodeFromPoint = this.utCampaignList.GetNodeFromPoint(e.X, e.Y);
      if (nodeFromPoint == null || nodeFromPoint.Tag == null || this.utTooltipNode == nodeFromPoint)
        return;
      this.utTooltipNode = nodeFromPoint;
      if (nodeFromPoint == null)
      {
        this.tipCampaignList.SetToolTip((Control) this.utCampaignList, string.Empty);
      }
      else
      {
        using (Graphics graphics = this.CreateGraphics())
          this.tipCampaignList.SetToolTip((Control) this.utCampaignList, this.utTooltipNode.Level == 0 ? Utils.FitToolTipText(graphics, this.Font, 400f, ((EllieMae.EMLite.Campaign.Campaign) this.utTooltipNode.Tag).CampaignDesc) : Utils.FitToolTipText(graphics, this.Font, 400f, ((CampaignStep) this.utTooltipNode.Tag).StepDesc));
      }
    }

    private string fitToolTipText(Font font, string text)
    {
      string[] strArray = text.Split(' ');
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      int num = 0;
      using (Graphics graphics = this.CreateGraphics())
      {
        while (num < strArray.Length)
        {
          do
          {
            stringBuilder1.Append(strArray[num++] + " ");
          }
          while (400.0 > (double) graphics.MeasureString(stringBuilder1.ToString(), font).Width && num < strArray.Length);
          stringBuilder2.Append(stringBuilder1.ToString() + "\n");
          stringBuilder1.Length = 0;
        }
        stringBuilder2.Length = (stringBuilder2.Length -= 2);
      }
      return stringBuilder2.ToString();
    }

    public event DisplayDetailEventHandler DisplayDetailEvent;

    protected virtual void OnDisplayDetailEvent(EventArgs e)
    {
      if (this.DisplayDetailEvent == null)
        return;
      this.DisplayDetailEvent((object) this, e);
    }

    public event CampaignActionRequestEventHandler CampaignActionRequestEvent;

    protected virtual void OnCampaignActionRequestEvent(CampaignActionRequestEventArgs e)
    {
      if (this.CampaignActionRequestEvent == null)
        return;
      this.CampaignActionRequestEvent((object) this, e);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
      UltraTreeColumnSet ultraTreeColumnSet = new UltraTreeColumnSet();
      UltraTreeNodeColumn column1 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column2 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column3 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column4 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column5 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column6 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
      UltraTreeNodeColumn column7 = new UltraTreeNodeColumn();
      Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
      Override @override = new Override();
      Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
      this.ctxCampaignList = new ContextMenu();
      this.mnuNewCampaign = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.mnuOpen = new MenuItem();
      this.mnuDuplicate = new MenuItem();
      this.mnuDelete = new MenuItem();
      this.menuItem3 = new MenuItem();
      this.mnuExpandAdd = new MenuItem();
      this.mnuCollapseAll = new MenuItem();
      this.menuItem6 = new MenuItem();
      this.mnuStartCampaign = new MenuItem();
      this.mnuStopCampaign = new MenuItem();
      this.menuItem1 = new MenuItem();
      this.mnuManageTemplates = new MenuItem();
      this.errCampaignList = new ErrorProvider(this.components);
      this.tipCampaignList = new System.Windows.Forms.ToolTip(this.components);
      this.icnDeleteCampaign = new StandardIconButton();
      this.icnEditCampaign = new StandardIconButton();
      this.icnDuplicateCampaign = new StandardIconButton();
      this.icnNewCampaign = new StandardIconButton();
      this.pnlCampaignTable = new TableContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnCampaignTemplates = new Button();
      this.btnStopCampaign = new Button();
      this.btnStartCampaign = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.utCampaignList = new UltraTree();
      this.chkIncludeNotStartedCampaigns = new CheckBox();
      this.chkIncludeStoppedCampaigns = new CheckBox();
      this.lblCampaignCount = new Label();
      this.lblTasksDueCount = new Label();
      ((ISupportInitialize) this.errCampaignList).BeginInit();
      ((ISupportInitialize) this.icnDeleteCampaign).BeginInit();
      ((ISupportInitialize) this.icnEditCampaign).BeginInit();
      ((ISupportInitialize) this.icnDuplicateCampaign).BeginInit();
      ((ISupportInitialize) this.icnNewCampaign).BeginInit();
      this.pnlCampaignTable.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.utCampaignList).BeginInit();
      this.SuspendLayout();
      this.ctxCampaignList.MenuItems.AddRange(new MenuItem[13]
      {
        this.mnuNewCampaign,
        this.menuItem2,
        this.mnuOpen,
        this.mnuDuplicate,
        this.mnuDelete,
        this.menuItem3,
        this.mnuExpandAdd,
        this.mnuCollapseAll,
        this.menuItem6,
        this.mnuStartCampaign,
        this.mnuStopCampaign,
        this.menuItem1,
        this.mnuManageTemplates
      });
      this.ctxCampaignList.Popup += new EventHandler(this.ctxCampaignList_Popup);
      this.mnuNewCampaign.Index = 0;
      this.mnuNewCampaign.Text = "New Campaign";
      this.mnuNewCampaign.Click += new EventHandler(this.mnuNewCampaign_Click);
      this.menuItem2.Index = 1;
      this.menuItem2.Text = "-";
      this.mnuOpen.Enabled = false;
      this.mnuOpen.Index = 2;
      this.mnuOpen.Text = "Open";
      this.mnuOpen.Click += new EventHandler(this.mnuOpen_Click);
      this.mnuDuplicate.Enabled = false;
      this.mnuDuplicate.Index = 3;
      this.mnuDuplicate.Text = "Duplicate";
      this.mnuDuplicate.Click += new EventHandler(this.mnuDuplicate_Click);
      this.mnuDelete.Enabled = false;
      this.mnuDelete.Index = 4;
      this.mnuDelete.Text = "Delete";
      this.mnuDelete.Click += new EventHandler(this.mnuDelete_Click);
      this.menuItem3.Index = 5;
      this.menuItem3.Text = "-";
      this.mnuExpandAdd.Index = 6;
      this.mnuExpandAdd.Text = "Expand All";
      this.mnuExpandAdd.Click += new EventHandler(this.mnuExpandAdd_Click);
      this.mnuCollapseAll.Index = 7;
      this.mnuCollapseAll.Text = "Collapse All";
      this.mnuCollapseAll.Click += new EventHandler(this.mnuCollapseAll_Click);
      this.menuItem6.Index = 8;
      this.menuItem6.Text = "-";
      this.mnuStartCampaign.Enabled = false;
      this.mnuStartCampaign.Index = 9;
      this.mnuStartCampaign.Text = "Start Campaign";
      this.mnuStartCampaign.Click += new EventHandler(this.mnuStartCampaign_Click);
      this.mnuStopCampaign.Enabled = false;
      this.mnuStopCampaign.Index = 10;
      this.mnuStopCampaign.Text = "Stop Campaign";
      this.mnuStopCampaign.Click += new EventHandler(this.mnuStopCampaign_Click);
      this.menuItem1.Index = 11;
      this.menuItem1.Text = "-";
      this.mnuManageTemplates.Enabled = false;
      this.mnuManageTemplates.Index = 12;
      this.mnuManageTemplates.Text = "Manage Campaign Templates";
      this.errCampaignList.ContainerControl = (ContainerControl) this;
      this.icnDeleteCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnDeleteCampaign.BackColor = Color.Transparent;
      this.icnDeleteCampaign.Enabled = false;
      this.icnDeleteCampaign.Location = new Point(93, 3);
      this.icnDeleteCampaign.Name = "icnDeleteCampaign";
      this.icnDeleteCampaign.Size = new Size(16, 16);
      this.icnDeleteCampaign.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.icnDeleteCampaign.TabIndex = 15;
      this.icnDeleteCampaign.TabStop = false;
      this.tipCampaignList.SetToolTip((Control) this.icnDeleteCampaign, "Delete Campaign");
      this.icnDeleteCampaign.Click += new EventHandler(this.icnDeleteCampaign_Click);
      this.icnEditCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnEditCampaign.BackColor = Color.Transparent;
      this.icnEditCampaign.Enabled = false;
      this.icnEditCampaign.Location = new Point(49, 3);
      this.icnEditCampaign.Name = "icnEditCampaign";
      this.icnEditCampaign.Size = new Size(16, 16);
      this.icnEditCampaign.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.icnEditCampaign.TabIndex = 14;
      this.icnEditCampaign.TabStop = false;
      this.tipCampaignList.SetToolTip((Control) this.icnEditCampaign, "Open Campaign");
      this.icnEditCampaign.Click += new EventHandler(this.icnEditCampaign_Click);
      this.icnDuplicateCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnDuplicateCampaign.BackColor = Color.Transparent;
      this.icnDuplicateCampaign.Enabled = false;
      this.icnDuplicateCampaign.Location = new Point(71, 3);
      this.icnDuplicateCampaign.Name = "icnDuplicateCampaign";
      this.icnDuplicateCampaign.Size = new Size(16, 16);
      this.icnDuplicateCampaign.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.icnDuplicateCampaign.TabIndex = 13;
      this.icnDuplicateCampaign.TabStop = false;
      this.tipCampaignList.SetToolTip((Control) this.icnDuplicateCampaign, "Duplicate Campaign");
      this.icnDuplicateCampaign.Click += new EventHandler(this.icnDuplicateCampaign_Click);
      this.icnNewCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnNewCampaign.BackColor = Color.Transparent;
      this.icnNewCampaign.Location = new Point(27, 3);
      this.icnNewCampaign.Name = "icnNewCampaign";
      this.icnNewCampaign.Size = new Size(16, 16);
      this.icnNewCampaign.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.icnNewCampaign.TabIndex = 12;
      this.icnNewCampaign.TabStop = false;
      this.tipCampaignList.SetToolTip((Control) this.icnNewCampaign, "New Campaign");
      this.icnNewCampaign.Click += new EventHandler(this.icnNewCampaign_Click);
      this.pnlCampaignTable.Controls.Add((Control) this.flowLayoutPanel1);
      this.pnlCampaignTable.Controls.Add((Control) this.utCampaignList);
      this.pnlCampaignTable.Controls.Add((Control) this.chkIncludeNotStartedCampaigns);
      this.pnlCampaignTable.Controls.Add((Control) this.chkIncludeStoppedCampaigns);
      this.pnlCampaignTable.Controls.Add((Control) this.lblCampaignCount);
      this.pnlCampaignTable.Controls.Add((Control) this.lblTasksDueCount);
      this.pnlCampaignTable.Dock = DockStyle.Fill;
      this.pnlCampaignTable.Location = new Point(0, 0);
      this.pnlCampaignTable.Name = "pnlCampaignTable";
      this.pnlCampaignTable.Size = new Size(910, 402);
      this.pnlCampaignTable.TabIndex = 7;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCampaignTemplates);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnStopCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnStartCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnDeleteCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnDuplicateCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnEditCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnNewCampaign);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(506, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(399, 22);
      this.flowLayoutPanel1.TabIndex = 16;
      this.btnCampaignTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCampaignTemplates.BackColor = SystemColors.Control;
      this.btnCampaignTemplates.Location = new Point(283, 0);
      this.btnCampaignTemplates.Margin = new Padding(0);
      this.btnCampaignTemplates.Name = "btnCampaignTemplates";
      this.btnCampaignTemplates.Padding = new Padding(2, 0, 0, 0);
      this.btnCampaignTemplates.Size = new Size(116, 22);
      this.btnCampaignTemplates.TabIndex = 8;
      this.btnCampaignTemplates.Text = "Campaign Templates";
      this.btnCampaignTemplates.UseVisualStyleBackColor = true;
      this.btnCampaignTemplates.Click += new EventHandler(this.btnCampaignTemplates_Click);
      this.btnStopCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStopCampaign.BackColor = SystemColors.Control;
      this.btnStopCampaign.Location = new Point(194, 0);
      this.btnStopCampaign.Margin = new Padding(0);
      this.btnStopCampaign.Name = "btnStopCampaign";
      this.btnStopCampaign.Padding = new Padding(2, 0, 0, 0);
      this.btnStopCampaign.Size = new Size(89, 22);
      this.btnStopCampaign.TabIndex = 9;
      this.btnStopCampaign.Text = "Stop Campaign";
      this.btnStopCampaign.UseVisualStyleBackColor = true;
      this.btnStopCampaign.Click += new EventHandler(this.btnStopCampaign_Click);
      this.btnStartCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStartCampaign.BackColor = SystemColors.Control;
      this.btnStartCampaign.Location = new Point(121, 0);
      this.btnStartCampaign.Margin = new Padding(0);
      this.btnStartCampaign.Name = "btnStartCampaign";
      this.btnStartCampaign.Padding = new Padding(2, 0, 0, 0);
      this.btnStartCampaign.Size = new Size(73, 22);
      this.btnStartCampaign.TabIndex = 10;
      this.btnStartCampaign.Text = "Start Campaign";
      this.btnStartCampaign.UseVisualStyleBackColor = true;
      this.btnStartCampaign.Click += new EventHandler(this.btnStartCampaign_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(115, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 4, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 11;
      this.utCampaignList.BorderStyle = UIElementBorderStyle.None;
      this.utCampaignList.ColumnSettings.AutoFitColumns = AutoFitColumns.ResizeAllColumns;
      appearance1.BorderColor = SystemColors.Control;
      this.utCampaignList.ColumnSettings.CellAppearance = (AppearanceBase) appearance1;
      appearance2.BackColor = SystemColors.Control;
      appearance2.BackGradientStyle = GradientStyle.None;
      this.utCampaignList.ColumnSettings.ColumnHeaderAppearance = (AppearanceBase) appearance2;
      ultraTreeColumnSet.AllowColMoving = GridBagLayoutAllowMoving.None;
      ultraTreeColumnSet.BorderStyleCell = UIElementBorderStyle.Solid;
      column1.DataType = typeof (string);
      appearance3.TextHAlignAsString = "Left";
      column1.HeaderAppearance = (AppearanceBase) appearance3;
      column1.Key = "Name";
      column1.LayoutInfo.PreferredCellSize = new Size(368, 18);
      column1.LayoutInfo.PreferredLabelSize = new Size(368, 22);
      column1.Text = "Campaign Name";
      column2.DataType = typeof (string);
      appearance4.TextHAlignAsString = "Left";
      column2.HeaderAppearance = (AppearanceBase) appearance4;
      column2.Key = "Status";
      column2.LayoutInfo.PreferredLabelSize = new Size(70, 0);
      column2.Text = "Status";
      column3.DataType = typeof (string);
      appearance5.TextHAlignAsString = "Left";
      column3.HeaderAppearance = (AppearanceBase) appearance5;
      column3.Key = "ContactType";
      column3.LayoutInfo.PreferredCellSize = new Size(96, 18);
      column3.LayoutInfo.PreferredLabelSize = new Size(96, 0);
      column3.SortType = SortType.Descending;
      column3.Text = "Contact Type";
      column4.AllowSorting = DefaultableBoolean.False;
      column4.DataType = typeof (string);
      appearance6.TextHAlignAsString = "Left";
      column4.HeaderAppearance = (AppearanceBase) appearance6;
      column4.Key = "TaskType";
      column4.LayoutInfo.PreferredLabelSize = new Size(96, 0);
      column4.Text = "Task Type";
      column5.AllowSorting = DefaultableBoolean.False;
      column5.DataType = typeof (string);
      appearance7.TextHAlignAsString = "Left";
      column5.HeaderAppearance = (AppearanceBase) appearance7;
      column5.Key = "Delay";
      column5.LayoutInfo.PreferredCellSize = new Size(75, 0);
      column5.LayoutInfo.PreferredLabelSize = new Size(75, 0);
      column5.ShowSortIndicators = DefaultableBoolean.False;
      column5.Text = "Interval";
      appearance8.ForeColor = Color.FromArgb(238, 0, 0);
      column6.ActiveCellAppearance = (AppearanceBase) appearance8;
      appearance9.ForeColor = Color.FromArgb(238, 0, 0);
      column6.CellAppearance = (AppearanceBase) appearance9;
      column6.DataType = typeof (int);
      appearance10.TextHAlignAsString = "Right";
      column6.HeaderAppearance = (AppearanceBase) appearance10;
      column6.Key = "TasksDue";
      column6.LayoutInfo.PreferredCellSize = new Size(100, 0);
      column6.LayoutInfo.PreferredLabelSize = new Size(100, 0);
      column6.Text = "Tasks Due";
      column7.AllowSorting = DefaultableBoolean.False;
      column7.DataType = typeof (string);
      appearance11.TextHAlignAsString = "Left";
      column7.HeaderAppearance = (AppearanceBase) appearance11;
      column7.Key = "LastActionDate";
      column7.LayoutInfo.PreferredCellSize = new Size(95, 18);
      column7.LayoutInfo.PreferredLabelSize = new Size(95, 0);
      column7.ShowSortIndicators = DefaultableBoolean.False;
      column7.Text = "Last Action Date";
      ultraTreeColumnSet.Columns.Add(column1);
      ultraTreeColumnSet.Columns.Add(column2);
      ultraTreeColumnSet.Columns.Add(column3);
      ultraTreeColumnSet.Columns.Add(column4);
      ultraTreeColumnSet.Columns.Add(column5);
      ultraTreeColumnSet.Columns.Add(column6);
      ultraTreeColumnSet.Columns.Add(column7);
      ultraTreeColumnSet.HeaderStyle = HeaderStyle.XPThemed;
      this.utCampaignList.ColumnSettings.RootColumnSet = ultraTreeColumnSet;
      ((Control) this.utCampaignList).ContextMenu = this.ctxCampaignList;
      ((Control) this.utCampaignList).Dock = DockStyle.Fill;
      this.utCampaignList.HideSelection = false;
      ((Control) this.utCampaignList).Location = new Point(1, 26);
      ((Control) this.utCampaignList).Name = "utCampaignList";
      @override.HotTracking = DefaultableBoolean.True;
      appearance12.BackColor = SystemColors.Info;
      appearance12.FontData.Name = "Arial";
      appearance12.FontData.SizeInPoints = 8.25f;
      appearance12.FontData.UnderlineAsString = "False";
      appearance12.ForeColor = SystemColors.ControlText;
      @override.HotTrackingNodeAppearance = (AppearanceBase) appearance12;
      @override.NodeDoubleClickAction = NodeDoubleClickAction.None;
      @override.SelectionType = SelectType.Single;
      this.utCampaignList.Override = @override;
      ((Control) this.utCampaignList).Size = new Size(908, 350);
      ((Control) this.utCampaignList).TabIndex = 3;
      this.utCampaignList.ViewStyle = ViewStyle.OutlookExpress;
      this.utCampaignList.AfterSortChange += new AfterSortChangeEventHandler(this.utCampaignList_AfterSortChange);
      this.utCampaignList.AfterSelect += new AfterNodeSelectEventHandler(this.utCampaignList_AfterSelect);
      ((Control) this.utCampaignList).MouseMove += new MouseEventHandler(this.utCampaignList_MouseMove);
      this.utCampaignList.BeforeSortChange += new BeforeSortChangeEventHandler(this.utCampaignList_BeforeSortChange);
      ((Control) this.utCampaignList).MouseDown += new MouseEventHandler(this.utCampaignList_MouseDown);
      ((Control) this.utCampaignList).Resize += new EventHandler(this.utCampaignList_Resize);
      ((Control) this.utCampaignList).Click += new EventHandler(this.utCampaignList_Click);
      this.utCampaignList.AfterNodeLayoutItemResize += new AfterNodeLayoutItemResizeEventHandler(this.utCampaignList_AfterNodeLayoutItemResize);
      this.utCampaignList.BeforeSelect += new BeforeNodeSelectEventHandler(this.utCampaignList_BeforeSelect);
      ((Control) this.utCampaignList).DoubleClick += new EventHandler(this.utCampaignList_DoubleClick);
      this.chkIncludeNotStartedCampaigns.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkIncludeNotStartedCampaigns.BackColor = Color.Transparent;
      this.chkIncludeNotStartedCampaigns.Checked = true;
      this.chkIncludeNotStartedCampaigns.CheckState = CheckState.Checked;
      this.chkIncludeNotStartedCampaigns.Font = new Font("Arial", 8.25f);
      this.chkIncludeNotStartedCampaigns.Location = new Point(178, 380);
      this.chkIncludeNotStartedCampaigns.Name = "chkIncludeNotStartedCampaigns";
      this.chkIncludeNotStartedCampaigns.Size = new Size(173, 20);
      this.chkIncludeNotStartedCampaigns.TabIndex = 2;
      this.chkIncludeNotStartedCampaigns.Text = "Include Not Started Campaigns";
      this.chkIncludeNotStartedCampaigns.UseVisualStyleBackColor = false;
      this.chkIncludeNotStartedCampaigns.CheckedChanged += new EventHandler(this.chkIncludeNotStartedCampaigns_CheckedChanged);
      this.chkIncludeStoppedCampaigns.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkIncludeStoppedCampaigns.BackColor = Color.Transparent;
      this.chkIncludeStoppedCampaigns.Checked = true;
      this.chkIncludeStoppedCampaigns.CheckState = CheckState.Checked;
      this.chkIncludeStoppedCampaigns.Font = new Font("Arial", 8.25f);
      this.chkIncludeStoppedCampaigns.Location = new Point(13, 380);
      this.chkIncludeStoppedCampaigns.Name = "chkIncludeStoppedCampaigns";
      this.chkIncludeStoppedCampaigns.Size = new Size(159, 20);
      this.chkIncludeStoppedCampaigns.TabIndex = 1;
      this.chkIncludeStoppedCampaigns.Text = "Include Stopped Campaigns";
      this.chkIncludeStoppedCampaigns.UseVisualStyleBackColor = false;
      this.chkIncludeStoppedCampaigns.CheckedChanged += new EventHandler(this.chkIncludeStoppedCampaigns_CheckedChanged);
      this.lblCampaignCount.AutoSize = true;
      this.lblCampaignCount.BackColor = Color.Transparent;
      this.lblCampaignCount.Font = new Font("Arial", 8f, FontStyle.Bold);
      this.lblCampaignCount.Location = new Point(10, 6);
      this.lblCampaignCount.Name = "lblCampaignCount";
      this.lblCampaignCount.Size = new Size(93, 14);
      this.lblCampaignCount.TabIndex = 1;
      this.lblCampaignCount.Text = "Campaigns (0) -";
      this.lblCampaignCount.TextAlign = ContentAlignment.MiddleLeft;
      this.lblTasksDueCount.AutoSize = true;
      this.lblTasksDueCount.BackColor = Color.Transparent;
      this.lblTasksDueCount.Font = new Font("Arial", 8.25f);
      this.lblTasksDueCount.ForeColor = Color.FromArgb(238, 0, 0);
      this.lblTasksDueCount.Location = new Point(100, 6);
      this.lblTasksDueCount.Name = "lblTasksDueCount";
      this.lblTasksDueCount.Size = new Size(93, 14);
      this.lblTasksDueCount.TabIndex = 3;
      this.lblTasksDueCount.Text = "Total 0 Tasks Due";
      this.lblTasksDueCount.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(910, 402);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlCampaignTable);
      this.Font = new Font("Arial", 8.25f);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignListForm);
      this.ShowInTaskbar = false;
      ((ISupportInitialize) this.errCampaignList).EndInit();
      ((ISupportInitialize) this.icnDeleteCampaign).EndInit();
      ((ISupportInitialize) this.icnEditCampaign).EndInit();
      ((ISupportInitialize) this.icnDuplicateCampaign).EndInit();
      ((ISupportInitialize) this.icnNewCampaign).EndInit();
      this.pnlCampaignTable.ResumeLayout(false);
      this.pnlCampaignTable.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.utCampaignList).EndInit();
      this.ResumeLayout(false);
    }

    public class RootOnlyAlphaComparer : IComparer
    {
      private int maxSortLevel = -1;

      public RootOnlyAlphaComparer(int maxSortLevel) => this.maxSortLevel = maxSortLevel;

      int IComparer.Compare(object x, object y)
      {
        UltraTreeNodeCell ultraTreeNodeCell1 = x as UltraTreeNodeCell;
        UltraTreeNodeCell ultraTreeNodeCell2 = y as UltraTreeNodeCell;
        UltraTreeNode node1 = ultraTreeNodeCell1?.Node;
        UltraTreeNode node2 = ultraTreeNodeCell2?.Node;
        if (node1 == null || node1.Level > this.maxSortLevel || node2 == null || node2.Level > this.maxSortLevel)
          return 0;
        object cellValue1 = node1.GetCellValue(ultraTreeNodeCell1.Column);
        object cellValue2 = node2.GetCellValue(ultraTreeNodeCell2.Column);
        IComparable comparable1 = cellValue1 as IComparable;
        IComparable comparable2 = cellValue2 as IComparable;
        return comparable1 == null || comparable2 == null ? 0 : comparable1.CompareTo((object) comparable2);
      }
    }

    public class RootOnlyNumericComparer : IComparer
    {
      private int maxSortLevel = -1;

      public RootOnlyNumericComparer(int maxSortLevel) => this.maxSortLevel = maxSortLevel;

      int IComparer.Compare(object x, object y)
      {
        UltraTreeNodeCell ultraTreeNodeCell1 = x as UltraTreeNodeCell;
        UltraTreeNodeCell ultraTreeNodeCell2 = y as UltraTreeNodeCell;
        UltraTreeNode node1 = ultraTreeNodeCell1?.Node;
        UltraTreeNode node2 = ultraTreeNodeCell2?.Node;
        if (node1 == null || node1.Level > this.maxSortLevel || node2 == null || node2.Level > this.maxSortLevel)
          return 0;
        object cellValue1 = node1.GetCellValue(ultraTreeNodeCell1.Column);
        int num1 = 0;
        try
        {
          num1 = int.Parse(cellValue1.ToString());
        }
        catch
        {
        }
        object cellValue2 = node2.GetCellValue(ultraTreeNodeCell2.Column);
        int num2 = 0;
        try
        {
          num2 = int.Parse(cellValue2.ToString());
        }
        catch
        {
        }
        if (num1 < num2)
          return -1;
        return num1 > num2 ? 1 : 0;
      }
    }
  }
}
