// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplatesPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplatesPage : Form, IGroupSecurityPage
  {
    public EventHandler DirtyFlagChagned;
    private string userID = "";
    private AclGroup[] groupList;
    private IContainer components;
    private LoanTemplateLegend legend;
    private ResourceBasePanel loanProgramsPanel;
    private ResourceBasePanel closingCostsPanel;
    private ResourceBasePanel documentSetsPanel;
    private ResourceBasePanel inputFormSetsPanel;
    private ResourceBasePanel miscDataTemplatesPanel;
    private ResourceBasePanel loanTemplateSetsPanel;
    private ResourceBasePanel settlementServiceSetsPanel;
    private ResourceBasePanel affiliateSetsPanel;
    private ResourceBasePanel taskSetsPanel;
    private PanelEx pnlExBottom;
    private Splitter splitterMiddleBottom;
    private PanelEx pnlExMiddle;
    private Splitter splitterTopMiddle;
    private PanelEx pnlExTop;
    private PanelEx pnlExTopRight;
    private Splitter splitterTop;
    private PanelEx pnlExTopLeft;
    private PanelEx pnlExMiddleLeft;
    private PanelEx pnlExBottomRight;
    private Splitter splitterBottom;
    private PanelEx pnlExBottomLeft;
    private PanelEx pnlExMiddleRight;
    private Panel pnlRow4;
    private Panel pnlRow4Column1;
    private Splitter splitterRow3Row4;
    private Splitter splitterRow4;
    private Panel pnlRow4Column2;
    private Splitter splitterRow4Row5;
    private Panel pnlRow5;
    private Panel pnlRow5Column1;
    private Splitter splitterMiddle;

    public LoanTemplatesPage(Sessions.Session session, int groupId, EventHandler dirtyFlagChanged)
    {
      this.DirtyFlagChagned = dirtyFlagChanged;
      this.InitializeComponent();
      this.loanProgramsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.LoanPrograms, this.DirtyFlagChagned);
      this.closingCostsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.ClosingCosts, this.DirtyFlagChagned);
      this.documentSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.DocumentSets, this.DirtyFlagChagned);
      this.inputFormSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.InputFormSets, this.DirtyFlagChagned);
      this.miscDataTemplatesPanel = new ResourceBasePanel(session, groupId, GroupResourceType.MiscDataTemplates, this.DirtyFlagChagned);
      this.loanTemplateSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.LoanTemplateSets, this.DirtyFlagChagned);
      this.taskSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.TaskSets, this.DirtyFlagChagned);
      this.settlementServiceSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.SettlementServiceProviders, this.DirtyFlagChagned);
      this.affiliateSetsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.AffiliatedBusinessArrangements, this.DirtyFlagChagned);
      this.init();
    }

    public LoanTemplatesPage(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      EventHandler dirtyFlagChanged)
    {
      this.userID = userID;
      this.groupList = groups;
      this.DirtyFlagChagned = dirtyFlagChanged;
      this.InitializeComponent();
      this.loanProgramsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.LoanPrograms, this.DirtyFlagChagned);
      this.closingCostsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.ClosingCosts, this.DirtyFlagChagned);
      this.documentSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.DocumentSets, this.DirtyFlagChagned);
      this.inputFormSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.InputFormSets, this.DirtyFlagChagned);
      this.miscDataTemplatesPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.MiscDataTemplates, this.DirtyFlagChagned);
      this.loanTemplateSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.LoanTemplateSets, this.DirtyFlagChagned);
      this.taskSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.TaskSets, this.DirtyFlagChagned);
      this.settlementServiceSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.SettlementServiceProviders, this.DirtyFlagChagned);
      this.affiliateSetsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.AffiliatedBusinessArrangements, this.DirtyFlagChagned);
      this.init();
    }

    private void init()
    {
      this.loanProgramsPanel.Visible = true;
      this.loanProgramsPanel.Dock = DockStyle.Fill;
      this.loanProgramsPanel.BackColor = this.BackColor;
      this.pnlExTopLeft.Controls.Add((Control) this.loanProgramsPanel);
      this.closingCostsPanel.Visible = true;
      this.closingCostsPanel.Dock = DockStyle.Fill;
      this.closingCostsPanel.BackColor = this.BackColor;
      this.pnlExTopRight.Controls.Add((Control) this.closingCostsPanel);
      this.documentSetsPanel.Visible = true;
      this.documentSetsPanel.Dock = DockStyle.Fill;
      this.documentSetsPanel.BackColor = this.BackColor;
      this.pnlExMiddleLeft.Controls.Add((Control) this.documentSetsPanel);
      this.inputFormSetsPanel.Visible = true;
      this.inputFormSetsPanel.Dock = DockStyle.Fill;
      this.inputFormSetsPanel.BackColor = this.BackColor;
      this.pnlExMiddleRight.Controls.Add((Control) this.inputFormSetsPanel);
      this.miscDataTemplatesPanel.Visible = true;
      this.miscDataTemplatesPanel.Dock = DockStyle.Fill;
      this.miscDataTemplatesPanel.BackColor = this.BackColor;
      this.pnlExBottomLeft.Controls.Add((Control) this.miscDataTemplatesPanel);
      this.loanTemplateSetsPanel.Visible = true;
      this.loanTemplateSetsPanel.Dock = DockStyle.Fill;
      this.loanTemplateSetsPanel.BackColor = this.BackColor;
      this.pnlExBottomRight.Controls.Add((Control) this.loanTemplateSetsPanel);
      this.taskSetsPanel.Visible = true;
      this.taskSetsPanel.Dock = DockStyle.Fill;
      this.taskSetsPanel.BackColor = this.BackColor;
      this.pnlRow4Column1.Controls.Add((Control) this.taskSetsPanel);
      this.settlementServiceSetsPanel.Visible = true;
      this.settlementServiceSetsPanel.Dock = DockStyle.Fill;
      this.settlementServiceSetsPanel.BackColor = this.BackColor;
      this.pnlRow4Column2.Controls.Add((Control) this.settlementServiceSetsPanel);
      this.affiliateSetsPanel.Visible = true;
      this.affiliateSetsPanel.Dock = DockStyle.Fill;
      this.affiliateSetsPanel.BackColor = this.BackColor;
      this.pnlRow5Column1.Controls.Add((Control) this.affiliateSetsPanel);
      this.adjustSizes();
    }

    private void adjustSizes()
    {
      this.pnlExTop.Height = this.pnlExMiddle.Height = this.pnlExBottom.Height = this.pnlRow4.Height = (this.Height - this.legend.Height) / 5;
      this.pnlExTopLeft.Width = this.pnlExMiddleLeft.Width = this.pnlExBottomLeft.Width = this.pnlRow4Column1.Width = this.pnlRow5Column1.Width = (this.Width - this.splitterTop.Width) / 2;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlExBottom = new PanelEx();
      this.pnlExBottomRight = new PanelEx();
      this.splitterBottom = new Splitter();
      this.pnlExBottomLeft = new PanelEx();
      this.splitterMiddleBottom = new Splitter();
      this.pnlExMiddle = new PanelEx();
      this.pnlExMiddleRight = new PanelEx();
      this.splitterMiddle = new Splitter();
      this.pnlExMiddleLeft = new PanelEx();
      this.splitterTopMiddle = new Splitter();
      this.pnlExTop = new PanelEx();
      this.pnlExTopRight = new PanelEx();
      this.splitterTop = new Splitter();
      this.pnlExTopLeft = new PanelEx();
      this.pnlRow4 = new Panel();
      this.pnlRow4Column2 = new Panel();
      this.splitterRow4 = new Splitter();
      this.pnlRow4Column1 = new Panel();
      this.splitterRow3Row4 = new Splitter();
      this.splitterRow4Row5 = new Splitter();
      this.pnlRow5 = new Panel();
      this.legend = new LoanTemplateLegend();
      this.pnlRow5Column1 = new Panel();
      this.pnlExBottom.SuspendLayout();
      this.pnlExMiddle.SuspendLayout();
      this.pnlExTop.SuspendLayout();
      this.pnlRow4.SuspendLayout();
      this.pnlRow5.SuspendLayout();
      this.SuspendLayout();
      this.pnlExBottom.Controls.Add((Control) this.pnlExBottomRight);
      this.pnlExBottom.Controls.Add((Control) this.splitterBottom);
      this.pnlExBottom.Controls.Add((Control) this.pnlExBottomLeft);
      this.pnlExBottom.Dock = DockStyle.Top;
      this.pnlExBottom.Location = new Point(0, 374);
      this.pnlExBottom.Name = "pnlExBottom";
      this.pnlExBottom.Size = new Size(400, 184);
      this.pnlExBottom.TabIndex = 1;
      this.pnlExBottomRight.Dock = DockStyle.Fill;
      this.pnlExBottomRight.Location = new Point(203, 0);
      this.pnlExBottomRight.Name = "pnlExBottomRight";
      this.pnlExBottomRight.Size = new Size(197, 184);
      this.pnlExBottomRight.TabIndex = 2;
      this.splitterBottom.Location = new Point(200, 0);
      this.splitterBottom.Name = "splitterBottom";
      this.splitterBottom.Size = new Size(3, 184);
      this.splitterBottom.TabIndex = 1;
      this.splitterBottom.TabStop = false;
      this.pnlExBottomLeft.Dock = DockStyle.Left;
      this.pnlExBottomLeft.Location = new Point(0, 0);
      this.pnlExBottomLeft.Name = "pnlExBottomLeft";
      this.pnlExBottomLeft.Size = new Size(200, 184);
      this.pnlExBottomLeft.TabIndex = 0;
      this.splitterMiddleBottom.Dock = DockStyle.Top;
      this.splitterMiddleBottom.Location = new Point(0, 371);
      this.splitterMiddleBottom.Name = "splitterMiddleBottom";
      this.splitterMiddleBottom.Size = new Size(400, 3);
      this.splitterMiddleBottom.TabIndex = 2;
      this.splitterMiddleBottom.TabStop = false;
      this.pnlExMiddle.Controls.Add((Control) this.pnlExMiddleRight);
      this.pnlExMiddle.Controls.Add((Control) this.splitterMiddle);
      this.pnlExMiddle.Controls.Add((Control) this.pnlExMiddleLeft);
      this.pnlExMiddle.Dock = DockStyle.Top;
      this.pnlExMiddle.Location = new Point(0, 187);
      this.pnlExMiddle.Name = "pnlExMiddle";
      this.pnlExMiddle.Size = new Size(400, 184);
      this.pnlExMiddle.TabIndex = 3;
      this.pnlExMiddleRight.Dock = DockStyle.Fill;
      this.pnlExMiddleRight.Location = new Point(203, 0);
      this.pnlExMiddleRight.Name = "pnlExMiddleRight";
      this.pnlExMiddleRight.Size = new Size(197, 184);
      this.pnlExMiddleRight.TabIndex = 2;
      this.splitterMiddle.Location = new Point(200, 0);
      this.splitterMiddle.Name = "splitterMiddle";
      this.splitterMiddle.Size = new Size(3, 184);
      this.splitterMiddle.TabIndex = 1;
      this.splitterMiddle.TabStop = false;
      this.pnlExMiddleLeft.Dock = DockStyle.Left;
      this.pnlExMiddleLeft.Location = new Point(0, 0);
      this.pnlExMiddleLeft.Name = "pnlExMiddleLeft";
      this.pnlExMiddleLeft.Size = new Size(200, 184);
      this.pnlExMiddleLeft.TabIndex = 0;
      this.splitterTopMiddle.Dock = DockStyle.Top;
      this.splitterTopMiddle.Location = new Point(0, 184);
      this.splitterTopMiddle.Name = "splitterTopMiddle";
      this.splitterTopMiddle.Size = new Size(400, 3);
      this.splitterTopMiddle.TabIndex = 4;
      this.splitterTopMiddle.TabStop = false;
      this.pnlExTop.Controls.Add((Control) this.pnlExTopRight);
      this.pnlExTop.Controls.Add((Control) this.splitterTop);
      this.pnlExTop.Controls.Add((Control) this.pnlExTopLeft);
      this.pnlExTop.Dock = DockStyle.Top;
      this.pnlExTop.Location = new Point(0, 0);
      this.pnlExTop.Name = "pnlExTop";
      this.pnlExTop.Size = new Size(400, 184);
      this.pnlExTop.TabIndex = 5;
      this.pnlExTopRight.Dock = DockStyle.Fill;
      this.pnlExTopRight.Location = new Point(203, 0);
      this.pnlExTopRight.Name = "pnlExTopRight";
      this.pnlExTopRight.Size = new Size(197, 184);
      this.pnlExTopRight.TabIndex = 2;
      this.splitterTop.Location = new Point(200, 0);
      this.splitterTop.Name = "splitterTop";
      this.splitterTop.Size = new Size(3, 184);
      this.splitterTop.TabIndex = 1;
      this.splitterTop.TabStop = false;
      this.pnlExTopLeft.Dock = DockStyle.Left;
      this.pnlExTopLeft.Location = new Point(0, 0);
      this.pnlExTopLeft.Name = "pnlExTopLeft";
      this.pnlExTopLeft.Size = new Size(200, 184);
      this.pnlExTopLeft.TabIndex = 0;
      this.pnlRow4.Controls.Add((Control) this.pnlRow4Column2);
      this.pnlRow4.Controls.Add((Control) this.splitterRow4);
      this.pnlRow4.Controls.Add((Control) this.pnlRow4Column1);
      this.pnlRow4.Dock = DockStyle.Top;
      this.pnlRow4.Location = new Point(0, 561);
      this.pnlRow4.Name = "pnlRow4";
      this.pnlRow4.Size = new Size(400, 184);
      this.pnlRow4.TabIndex = 6;
      this.pnlRow4Column2.Dock = DockStyle.Fill;
      this.pnlRow4Column2.Location = new Point(203, 0);
      this.pnlRow4Column2.Name = "pnlRow4Column2";
      this.pnlRow4Column2.Size = new Size(197, 184);
      this.pnlRow4Column2.TabIndex = 2;
      this.splitterRow4.Location = new Point(200, 0);
      this.splitterRow4.Name = "splitterRow4";
      this.splitterRow4.Size = new Size(3, 184);
      this.splitterRow4.TabIndex = 1;
      this.splitterRow4.TabStop = false;
      this.pnlRow4Column1.Dock = DockStyle.Left;
      this.pnlRow4Column1.Location = new Point(0, 0);
      this.pnlRow4Column1.Name = "pnlRow4Column1";
      this.pnlRow4Column1.Size = new Size(200, 184);
      this.pnlRow4Column1.TabIndex = 0;
      this.splitterRow3Row4.Dock = DockStyle.Top;
      this.splitterRow3Row4.Location = new Point(0, 558);
      this.splitterRow3Row4.Name = "splitterRow3Row4";
      this.splitterRow3Row4.Size = new Size(400, 3);
      this.splitterRow3Row4.TabIndex = 7;
      this.splitterRow3Row4.TabStop = false;
      this.splitterRow4Row5.Dock = DockStyle.Top;
      this.splitterRow4Row5.Location = new Point(0, 745);
      this.splitterRow4Row5.Name = "splitterRow4Row5";
      this.splitterRow4Row5.Size = new Size(400, 3);
      this.splitterRow4Row5.TabIndex = 8;
      this.splitterRow4Row5.TabStop = false;
      this.pnlRow5.Controls.Add((Control) this.pnlRow5Column1);
      this.pnlRow5.Dock = DockStyle.Fill;
      this.pnlRow5.Location = new Point(0, 748);
      this.pnlRow5.Name = "pnlRow5";
      this.pnlRow5.Size = new Size(400, 184);
      this.pnlRow5.TabIndex = 9;
      this.legend.BackColor = Color.Transparent;
      this.legend.Dock = DockStyle.Bottom;
      this.legend.Location = new Point(0, 932);
      this.legend.Name = "legend";
      this.legend.Size = new Size(400, 49);
      this.legend.TabIndex = 0;
      this.pnlRow5Column1.Dock = DockStyle.Left;
      this.pnlRow5Column1.Location = new Point(0, 0);
      this.pnlRow5Column1.Name = "pnlRow5Column1";
      this.pnlRow5Column1.Size = new Size(200, 184);
      this.pnlRow5Column1.TabIndex = 0;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(400, 981);
      this.Controls.Add((Control) this.pnlRow5);
      this.Controls.Add((Control) this.splitterRow4Row5);
      this.Controls.Add((Control) this.pnlRow4);
      this.Controls.Add((Control) this.splitterRow3Row4);
      this.Controls.Add((Control) this.pnlExBottom);
      this.Controls.Add((Control) this.splitterMiddleBottom);
      this.Controls.Add((Control) this.pnlExMiddle);
      this.Controls.Add((Control) this.splitterTopMiddle);
      this.Controls.Add((Control) this.pnlExTop);
      this.Controls.Add((Control) this.legend);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (LoanTemplatesPage);
      this.BackColorChanged += new EventHandler(this.LoanTemplatesPage_BackColorChanged);
      this.SizeChanged += new EventHandler(this.LoanTemplatesPage_SizeChanged);
      this.pnlExBottom.ResumeLayout(false);
      this.pnlExMiddle.ResumeLayout(false);
      this.pnlExTop.ResumeLayout(false);
      this.pnlRow4.ResumeLayout(false);
      this.pnlRow5.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public virtual void SetGroup(int groupId)
    {
      this.loanProgramsPanel.SetGroup(groupId);
      this.closingCostsPanel.SetGroup(groupId);
      this.documentSetsPanel.SetGroup(groupId);
      this.inputFormSetsPanel.SetGroup(groupId);
      this.loanTemplateSetsPanel.SetGroup(groupId);
      this.miscDataTemplatesPanel.SetGroup(groupId);
      this.taskSetsPanel.SetGroup(groupId);
      this.settlementServiceSetsPanel.SetGroup(groupId);
      this.affiliateSetsPanel.SetGroup(groupId);
    }

    private void LoanTemplatesPage_BackColorChanged(object sender, EventArgs e)
    {
      this.loanProgramsPanel.BackColor = this.BackColor;
      this.closingCostsPanel.BackColor = this.BackColor;
      this.documentSetsPanel.BackColor = this.BackColor;
      this.inputFormSetsPanel.BackColor = this.BackColor;
      this.loanTemplateSetsPanel.BackColor = this.BackColor;
      this.miscDataTemplatesPanel.BackColor = this.BackColor;
      this.taskSetsPanel.BackColor = this.BackColor;
      this.settlementServiceSetsPanel.BackColor = this.BackColor;
      this.affiliateSetsPanel.BackColor = this.BackColor;
    }

    public void SaveData()
    {
      this.loanProgramsPanel.SaveData();
      this.closingCostsPanel.SaveData();
      this.documentSetsPanel.SaveData();
      this.inputFormSetsPanel.SaveData();
      this.loanTemplateSetsPanel.SaveData();
      this.miscDataTemplatesPanel.SaveData();
      this.taskSetsPanel.SaveData();
      this.settlementServiceSetsPanel.SaveData();
      this.affiliateSetsPanel.SaveData();
    }

    public void ResetData()
    {
      this.loanProgramsPanel.ResetDate();
      this.closingCostsPanel.ResetDate();
      this.documentSetsPanel.ResetDate();
      this.inputFormSetsPanel.ResetDate();
      this.loanTemplateSetsPanel.ResetDate();
      this.miscDataTemplatesPanel.ResetDate();
      this.taskSetsPanel.ResetDate();
      this.settlementServiceSetsPanel.ResetDate();
      this.affiliateSetsPanel.ResetDate();
    }

    public bool HasBeenModified()
    {
      bool flag = false;
      if (this.loanProgramsPanel.HasBeenModified())
        flag = true;
      else if (this.closingCostsPanel.HasBeenModified())
        flag = true;
      else if (this.documentSetsPanel.HasBeenModified())
        flag = true;
      else if (this.inputFormSetsPanel.HasBeenModified())
        flag = true;
      else if (this.loanTemplateSetsPanel.HasBeenModified())
        flag = true;
      else if (this.miscDataTemplatesPanel.HasBeenModified())
        flag = true;
      else if (this.taskSetsPanel.HasBeenModified())
        flag = true;
      else if (this.settlementServiceSetsPanel.HasBeenModified())
        flag = true;
      else if (this.affiliateSetsPanel.HasBeenModified())
        flag = true;
      return flag;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.loanProgramsPanel.ResetDate();
      this.closingCostsPanel.ResetDate();
      this.documentSetsPanel.ResetDate();
      this.inputFormSetsPanel.ResetDate();
      this.loanTemplateSetsPanel.ResetDate();
      this.miscDataTemplatesPanel.ResetDate();
      this.taskSetsPanel.ResetDate();
      this.settlementServiceSetsPanel.ResetDate();
      this.affiliateSetsPanel.ResetDate();
    }

    private void LoanTemplatesPage_SizeChanged(object sender, EventArgs e) => this.adjustSizes();
  }
}
