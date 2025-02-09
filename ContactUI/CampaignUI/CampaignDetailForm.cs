// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignDetailForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignDetailForm : Form
  {
    private const bool FORCE_REFRESH = true;
    private CampaignData campaignData;
    private FlowLayoutPanel flowLayoutPanel1;
    private CampaignListForm frmCampaignList;
    private IContainer components;
    private Button btnNextCampaign;
    private Button btnPrevCampaign;
    private Button btnBackToList;
    private BorderPanel pnlTabControl;
    private Label lblCampaignName;
    private TabControl tabCampaignDetail;
    private TabPage tbpCampaignSteps;
    private TabPage tbpCampaignHistory;
    private Label lblCampaignDescription;
    private ToolTip tipCampaignDetail;
    private TabPage tbpCampaignContacts;
    private CampaignStepsTab frmStepsTab;
    private CampaignContactsTab frmContactsTab;
    private Button btnStartStopCampaign;
    private CampaignHistoryTab frmHistoryTab;
    private FormattedLabel lblCampaignXofY;
    private Label lblContactType;
    private FormattedLabel lblCampaignInfo;
    private Label lblDescriptionHdr;
    private GradientPanel pnlPageHeader;
    private GradientPanel pnlPageSubHeader;

    public CampaignDetailForm(CampaignListForm frmCampaignList)
    {
      this.InitializeComponent();
      this.frmCampaignList = frmCampaignList;
      this.campaignData = CampaignData.GetCampaignData();
      this.frmStepsTab = new CampaignStepsTab();
      this.frmStepsTab.TopLevel = false;
      this.frmStepsTab.Visible = true;
      this.frmStepsTab.Dock = DockStyle.Fill;
      this.tbpCampaignSteps.Controls.Add((Control) this.frmStepsTab);
      this.frmStepsTab.UpdateTaskCount += new CampaignDetailForm.UpdateTaskCount(this.updateTaskCount);
      this.frmContactsTab = new CampaignContactsTab();
      this.frmContactsTab.TopLevel = false;
      this.frmContactsTab.Visible = true;
      this.frmContactsTab.Dock = DockStyle.Fill;
      this.tbpCampaignContacts.Controls.Add((Control) this.frmContactsTab);
      this.frmContactsTab.UpdateTaskCount += new CampaignDetailForm.UpdateTaskCount(this.updateTaskCount);
      this.frmHistoryTab = new CampaignHistoryTab();
      this.frmHistoryTab.TopLevel = false;
      this.frmHistoryTab.Visible = true;
      this.frmHistoryTab.Dock = DockStyle.Fill;
      this.tbpCampaignHistory.Controls.Add((Control) this.frmHistoryTab);
      this.tabCampaignDetail.SelectedIndex = 0;
    }

    private void updateTaskCount() => this.lblCampaignName_Resize((object) null, (EventArgs) null);

    public void CloseCampaignDetailForm()
    {
      this.tabCampaignDetail.SelectedIndex = 0;
      this.OnDisplayListEvent(EventArgs.Empty);
    }

    public void PopulateCampaignDetailForm(bool forceRefresh)
    {
      this.populateNavigationPanel();
      this.populateCampaignSummaryPanel();
      this.populateCampaignStepsTab();
      this.populateCampaignContactsTab();
      this.populateCampaignHistoryTab(forceRefresh);
    }

    private void populateNavigationPanel()
    {
      int activeCampaignIndex = this.frmCampaignList.ActiveCampaignIndex;
      this.lblCampaignXofY.Text = "<b>" + (activeCampaignIndex + 1).ToString() + "</b> of <b>" + this.campaignData.Campaigns.Count.ToString() + "</b>";
      this.btnPrevCampaign.Enabled = activeCampaignIndex != 0;
      this.btnNextCampaign.Enabled = this.campaignData.Campaigns.Count != activeCampaignIndex + 1;
    }

    private void populateCampaignSummaryPanel()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      this.fitLabelText(this.lblCampaignName, activeCampaign.CampaignName);
      this.lblCampaignInfo.Text = "<b>" + this.lblCampaignName.Text + "</b> (" + new CampaignStatusNameProvider().GetName((object) activeCampaign.Status) + ") - <c value=\"238, 0, 0\">" + activeCampaign.TasksDue.ToString() + " Tasks Due</c>";
      this.tipCampaignDetail.SetToolTip((Control) this.lblCampaignInfo, this.tipCampaignDetail.GetToolTip((Control) this.lblCampaignName));
      this.lblContactType.Text = "Contact Type: " + new ContactTypeNameProvider().GetName((object) activeCampaign.ContactType);
      this.fitLabelText(this.lblCampaignDescription, activeCampaign.CampaignDesc);
      this.btnStartStopCampaign.Text = activeCampaign.Status == CampaignStatus.Running ? "Stop Campaign" : "Start Campaign";
    }

    private void populateCampaignStepsTab() => this.frmStepsTab.PopulateControls();

    private void populateCampaignContactsTab() => this.frmContactsTab.PopulateControls();

    private void populateCampaignHistoryTab(bool forceRefresh)
    {
      this.frmHistoryTab.PopulateControls(forceRefresh);
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if (Utils.FitLabelText(graphics, label, text))
          this.tipCampaignDetail.SetToolTip((Control) label, string.Empty);
        else
          this.tipCampaignDetail.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnBackToList_Click(object sender, EventArgs e) => this.CloseCampaignDetailForm();

    private void btnPrevCampaign_Click(object sender, EventArgs e)
    {
      this.OnPrevCampaignEvent(EventArgs.Empty);
    }

    private void btnNextCampaign_Click(object sender, EventArgs e)
    {
      this.OnNextCampaignEvent(EventArgs.Empty);
    }

    private void btnStartStopCampaign_Click(object sender, EventArgs e)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      if (string.Compare(((Control) sender).Text, "Start Campaign", true) == 0)
      {
        if (CampaignStatus.NotStarted != activeCampaign.Status && CampaignStatus.Stopped != activeCampaign.Status)
          return;
        this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("StartCampaign"));
        this.PopulateCampaignDetailForm(false);
      }
      else
      {
        if (activeCampaign.Status != CampaignStatus.Running)
          return;
        this.OnCampaignActionRequestEvent(new CampaignActionRequestEventArgs("StopCampaign"));
        this.PopulateCampaignDetailForm(false);
      }
    }

    private void tabCampaignDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabCampaignDetail.SelectedIndex == 0)
        this.populateCampaignStepsTab();
      else if (1 == this.tabCampaignDetail.SelectedIndex)
        this.populateCampaignContactsTab();
      else
        this.populateCampaignHistoryTab(false);
    }

    private void lblCampaignName_Resize(object sender, EventArgs e)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      this.fitLabelText(this.lblCampaignName, activeCampaign.CampaignName);
      this.lblCampaignInfo.Text = "<b>" + this.lblCampaignName.Text + "</b>(" + new CampaignStatusNameProvider().GetName((object) activeCampaign.Status) + ") -<c value=\"238, 0, 0\">" + activeCampaign.TasksDue.ToString() + " Tasks Due</c>";
      this.tipCampaignDetail.SetToolTip((Control) this.lblCampaignInfo, this.tipCampaignDetail.GetToolTip((Control) this.lblCampaignName));
    }

    private void lblCampaignDescription_Resize(object sender, EventArgs e)
    {
      this.fitLabelText(this.lblCampaignDescription, this.campaignData.ActiveCampaign.CampaignDesc);
    }

    public event DisplayListEventHandler DisplayListEvent;

    protected virtual void OnDisplayListEvent(EventArgs e)
    {
      if (this.DisplayListEvent == null)
        return;
      this.DisplayListEvent((object) this, e);
    }

    public event PrevCampaignEventHandler PrevCampaignEvent;

    protected virtual void OnPrevCampaignEvent(EventArgs e)
    {
      if (this.PrevCampaignEvent == null)
        return;
      this.PrevCampaignEvent((object) this, (EventArgs) null);
    }

    public event NextCampaignEventHandler NextCampaignEvent;

    protected virtual void OnNextCampaignEvent(EventArgs e)
    {
      if (this.NextCampaignEvent == null)
        return;
      this.NextCampaignEvent((object) this, (EventArgs) null);
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
      this.pnlTabControl = new BorderPanel();
      this.tabCampaignDetail = new TabControl();
      this.tbpCampaignSteps = new TabPage();
      this.tbpCampaignContacts = new TabPage();
      this.tbpCampaignHistory = new TabPage();
      this.tipCampaignDetail = new ToolTip(this.components);
      this.pnlPageSubHeader = new GradientPanel();
      this.lblCampaignDescription = new Label();
      this.lblDescriptionHdr = new Label();
      this.lblCampaignXofY = new FormattedLabel();
      this.lblContactType = new Label();
      this.pnlPageHeader = new GradientPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnNextCampaign = new Button();
      this.btnPrevCampaign = new Button();
      this.btnBackToList = new Button();
      this.btnStartStopCampaign = new Button();
      this.lblCampaignInfo = new FormattedLabel();
      this.lblCampaignName = new Label();
      this.pnlTabControl.SuspendLayout();
      this.tabCampaignDetail.SuspendLayout();
      this.pnlPageSubHeader.SuspendLayout();
      this.pnlPageHeader.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlTabControl.Controls.Add((Control) this.tabCampaignDetail);
      this.pnlTabControl.Dock = DockStyle.Fill;
      this.pnlTabControl.Location = new Point(0, 62);
      this.pnlTabControl.Name = "pnlTabControl";
      this.pnlTabControl.Padding = new Padding(2, 2, 0, 0);
      this.pnlTabControl.Size = new Size(1000, 538);
      this.pnlTabControl.TabIndex = 2;
      this.tabCampaignDetail.Controls.Add((Control) this.tbpCampaignSteps);
      this.tabCampaignDetail.Controls.Add((Control) this.tbpCampaignContacts);
      this.tabCampaignDetail.Controls.Add((Control) this.tbpCampaignHistory);
      this.tabCampaignDetail.Dock = DockStyle.Fill;
      this.tabCampaignDetail.Location = new Point(3, 3);
      this.tabCampaignDetail.Name = "tabCampaignDetail";
      this.tabCampaignDetail.SelectedIndex = 0;
      this.tabCampaignDetail.Size = new Size(996, 534);
      this.tabCampaignDetail.TabIndex = 0;
      this.tabCampaignDetail.SelectedIndexChanged += new EventHandler(this.tabCampaignDetail_SelectedIndexChanged);
      this.tbpCampaignSteps.BackColor = Color.White;
      this.tbpCampaignSteps.Location = new Point(4, 23);
      this.tbpCampaignSteps.Name = "tbpCampaignSteps";
      this.tbpCampaignSteps.Padding = new Padding(0, 2, 2, 2);
      this.tbpCampaignSteps.Size = new Size(988, 507);
      this.tbpCampaignSteps.TabIndex = 0;
      this.tbpCampaignSteps.Text = "   Campaign Steps   ";
      this.tbpCampaignContacts.BackColor = Color.White;
      this.tbpCampaignContacts.Location = new Point(4, 22);
      this.tbpCampaignContacts.Name = "tbpCampaignContacts";
      this.tbpCampaignContacts.Padding = new Padding(0, 2, 2, 2);
      this.tbpCampaignContacts.Size = new Size(988, 508);
      this.tbpCampaignContacts.TabIndex = 1;
      this.tbpCampaignContacts.Text = "   Campaign Contacts   ";
      this.tbpCampaignHistory.BackColor = Color.White;
      this.tbpCampaignHistory.Location = new Point(4, 22);
      this.tbpCampaignHistory.Name = "tbpCampaignHistory";
      this.tbpCampaignHistory.Padding = new Padding(0, 2, 2, 2);
      this.tbpCampaignHistory.Size = new Size(988, 508);
      this.tbpCampaignHistory.TabIndex = 2;
      this.tbpCampaignHistory.Text = "   Campaign History   ";
      this.pnlPageSubHeader.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.pnlPageSubHeader.Controls.Add((Control) this.lblCampaignDescription);
      this.pnlPageSubHeader.Controls.Add((Control) this.lblDescriptionHdr);
      this.pnlPageSubHeader.Controls.Add((Control) this.lblCampaignXofY);
      this.pnlPageSubHeader.Controls.Add((Control) this.lblContactType);
      this.pnlPageSubHeader.Dock = DockStyle.Top;
      this.pnlPageSubHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlPageSubHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlPageSubHeader.Location = new Point(0, 32);
      this.pnlPageSubHeader.Name = "pnlPageSubHeader";
      this.pnlPageSubHeader.Size = new Size(1000, 30);
      this.pnlPageSubHeader.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlPageSubHeader.TabIndex = 5;
      this.lblCampaignDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCampaignDescription.BackColor = Color.Transparent;
      this.lblCampaignDescription.ImageAlign = ContentAlignment.TopLeft;
      this.lblCampaignDescription.Location = new Point(208, 8);
      this.lblCampaignDescription.Name = "lblCampaignDescription";
      this.lblCampaignDescription.Size = new Size(705, 16);
      this.lblCampaignDescription.TabIndex = 3;
      this.lblCampaignDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignDescription.Resize += new EventHandler(this.lblCampaignDescription_Resize);
      this.lblDescriptionHdr.BackColor = Color.Transparent;
      this.lblDescriptionHdr.Location = new Point(144, 8);
      this.lblDescriptionHdr.Name = "lblDescriptionHdr";
      this.lblDescriptionHdr.Size = new Size(64, 16);
      this.lblDescriptionHdr.TabIndex = 4;
      this.lblDescriptionHdr.Text = "Description:";
      this.lblDescriptionHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignXofY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCampaignXofY.BackColor = Color.Transparent;
      this.lblCampaignXofY.Location = new Point(925, 8);
      this.lblCampaignXofY.Name = "lblCampaignXofY";
      this.lblCampaignXofY.Size = new Size(64, 16);
      this.lblCampaignXofY.TabIndex = 7;
      this.lblCampaignXofY.Text = "<b>999</b> of<b> 999</b>";
      this.lblCampaignXofY.Visible = false;
      this.lblContactType.BackColor = Color.Transparent;
      this.lblContactType.Location = new Point(10, 8);
      this.lblContactType.Name = "lblContactType";
      this.lblContactType.Size = new Size(124, 16);
      this.lblContactType.TabIndex = 4;
      this.lblContactType.Text = "Contact Type: Borrower";
      this.lblContactType.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlPageHeader.BackColorGlassyStyle = true;
      this.pnlPageHeader.Controls.Add((Control) this.flowLayoutPanel1);
      this.pnlPageHeader.Controls.Add((Control) this.lblCampaignInfo);
      this.pnlPageHeader.Controls.Add((Control) this.lblCampaignName);
      this.pnlPageHeader.Dock = DockStyle.Top;
      this.pnlPageHeader.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.pnlPageHeader.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.pnlPageHeader.Location = new Point(0, 0);
      this.pnlPageHeader.Name = "pnlPageHeader";
      this.pnlPageHeader.Size = new Size(1000, 32);
      this.pnlPageHeader.Style = GradientPanel.PanelStyle.PageHeader;
      this.pnlPageHeader.TabIndex = 4;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNextCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPrevCampaign);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnBackToList);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnStartStopCampaign);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(615, 5);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(380, 22);
      this.flowLayoutPanel1.TabIndex = 9;
      this.btnNextCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNextCampaign.BackColor = SystemColors.Control;
      this.btnNextCampaign.Location = new Point(285, 0);
      this.btnNextCampaign.Margin = new Padding(0);
      this.btnNextCampaign.Name = "btnNextCampaign";
      this.btnNextCampaign.Padding = new Padding(2, 0, 0, 0);
      this.btnNextCampaign.Size = new Size(95, 22);
      this.btnNextCampaign.TabIndex = 6;
      this.btnNextCampaign.Text = "Next Campaign";
      this.btnNextCampaign.UseVisualStyleBackColor = true;
      this.btnNextCampaign.Click += new EventHandler(this.btnNextCampaign_Click);
      this.btnPrevCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrevCampaign.BackColor = SystemColors.Control;
      this.btnPrevCampaign.Location = new Point(187, 0);
      this.btnPrevCampaign.Margin = new Padding(0);
      this.btnPrevCampaign.Name = "btnPrevCampaign";
      this.btnPrevCampaign.Padding = new Padding(2, 0, 0, 0);
      this.btnPrevCampaign.Size = new Size(98, 22);
      this.btnPrevCampaign.TabIndex = 5;
      this.btnPrevCampaign.Text = "Prev. Campaign";
      this.btnPrevCampaign.UseVisualStyleBackColor = true;
      this.btnPrevCampaign.Click += new EventHandler(this.btnPrevCampaign_Click);
      this.btnBackToList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBackToList.BackColor = SystemColors.Control;
      this.btnBackToList.Location = new Point(108, 0);
      this.btnBackToList.Margin = new Padding(0);
      this.btnBackToList.Name = "btnBackToList";
      this.btnBackToList.Padding = new Padding(2, 0, 0, 0);
      this.btnBackToList.Size = new Size(79, 22);
      this.btnBackToList.TabIndex = 4;
      this.btnBackToList.Text = "Back to List";
      this.btnBackToList.UseVisualStyleBackColor = true;
      this.btnBackToList.Click += new EventHandler(this.btnBackToList_Click);
      this.btnStartStopCampaign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStartStopCampaign.BackColor = SystemColors.Control;
      this.btnStartStopCampaign.Location = new Point(12, 0);
      this.btnStartStopCampaign.Margin = new Padding(0);
      this.btnStartStopCampaign.Name = "btnStartStopCampaign";
      this.btnStartStopCampaign.Padding = new Padding(2, 0, 0, 0);
      this.btnStartStopCampaign.Size = new Size(96, 22);
      this.btnStartStopCampaign.TabIndex = 8;
      this.btnStartStopCampaign.Text = "Start Campaign";
      this.btnStartStopCampaign.UseVisualStyleBackColor = true;
      this.btnStartStopCampaign.Click += new EventHandler(this.btnStartStopCampaign_Click);
      this.lblCampaignInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCampaignInfo.AutoSize = false;
      this.lblCampaignInfo.BackColor = Color.Transparent;
      this.lblCampaignInfo.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCampaignInfo.Location = new Point(10, 8);
      this.lblCampaignInfo.Name = "lblCampaignInfo";
      this.lblCampaignInfo.Size = new Size(593, 16);
      this.lblCampaignInfo.TabIndex = 4;
      this.lblCampaignInfo.Text = "<b>Campaign Name</b>(Not Started)<c value=\"239,0,0\">999 Tasks Due</c>";
      this.lblCampaignName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCampaignName.BackColor = Color.Transparent;
      this.lblCampaignName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCampaignName.ForeColor = Color.Transparent;
      this.lblCampaignName.ImageAlign = ContentAlignment.TopLeft;
      this.lblCampaignName.Location = new Point(10, 8);
      this.lblCampaignName.Name = "lblCampaignName";
      this.lblCampaignName.Size = new Size(430, 16);
      this.lblCampaignName.TabIndex = 2;
      this.lblCampaignName.Text = "Campaign Name";
      this.lblCampaignName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignName.Visible = false;
      this.lblCampaignName.Resize += new EventHandler(this.lblCampaignName_Resize);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1000, 600);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlTabControl);
      this.Controls.Add((Control) this.pnlPageSubHeader);
      this.Controls.Add((Control) this.pnlPageHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignDetailForm);
      this.pnlTabControl.ResumeLayout(false);
      this.tabCampaignDetail.ResumeLayout(false);
      this.pnlPageSubHeader.ResumeLayout(false);
      this.pnlPageSubHeader.PerformLayout();
      this.pnlPageHeader.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void UpdateTaskCount();
  }
}
