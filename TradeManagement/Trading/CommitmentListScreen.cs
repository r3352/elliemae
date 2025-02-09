// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CommitmentListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CommitmentListScreen : TradeAssignmentByTradeListScreenBase
  {
    private IContainer components;
    private TableContainer grpCommitments;
    private GridView gvEligible;
    private FlowLayoutPanel flpSecurityTrades;
    private Button btnOpenDialog;
    private Button btnUnassign;
    private StandardIconButton siBtnExcel;
    private CommitmentsPoolAllocateAmountControl commitmentsPoolAllocateAmountControl1;

    public CommitmentListScreen()
    {
      this.InitializeComponent();
      GridViewLayoutManager viewLayoutManager = new GridViewLayoutManager(this.gvEligible, (TableLayout) null, this.getDemoTableLayout(), (TableLayout) null);
    }

    protected override TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.FulfilledAmount", "Allocated Commitment Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.Name", "Commitment ID", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.ContractNumber", "Contract #", HorizontalAlignment.Left, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentAmount", "Commitment Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.OutstandingBalance", "Outstanding Balance", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.PairOffAmount", "Total Pair-Off Amount", HorizontalAlignment.Right, 130));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentSummary.CompletionPercent", "Completion Percent", HorizontalAlignment.Left, 110));
      return demoTableLayout;
    }

    private void btnOpenDialog_Click(object sender, EventArgs e)
    {
      using (GSECmtAssignmentDialog assignmentDialog = new GSECmtAssignmentDialog())
      {
        assignmentDialog.RefreshData(this.GetCurrentAssignments(), this.GetEligibleAssignments());
        if (assignmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadAssignments(assignmentDialog.GetCurrentAssignments(), assignmentDialog.GetEligibleAssignments());
        this.setModified(assignmentDialog.DataModified);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpCommitments = new TableContainer();
      this.gvEligible = new GridView();
      this.flpSecurityTrades = new FlowLayoutPanel();
      this.btnOpenDialog = new Button();
      this.btnUnassign = new Button();
      this.siBtnExcel = new StandardIconButton();
      this.commitmentsPoolAllocateAmountControl1 = new CommitmentsPoolAllocateAmountControl();
      this.grpCommitments.SuspendLayout();
      this.flpSecurityTrades.SuspendLayout();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      this.SuspendLayout();
      this.grpCommitments.Controls.Add((Control) this.gvEligible);
      this.grpCommitments.Controls.Add((Control) this.flpSecurityTrades);
      this.grpCommitments.Dock = DockStyle.Fill;
      this.grpCommitments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpCommitments.Location = new Point(0, 0);
      this.grpCommitments.Margin = new Padding(0);
      this.grpCommitments.Name = "grpCommitments";
      this.grpCommitments.Size = new Size(769, 138);
      this.grpCommitments.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpCommitments.TabIndex = 5;
      this.grpCommitments.Text = "Commitments";
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(767, 111);
      this.gvEligible.TabIndex = 4;
      this.flpSecurityTrades.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpSecurityTrades.BackColor = Color.Transparent;
      this.flpSecurityTrades.Controls.Add((Control) this.btnOpenDialog);
      this.flpSecurityTrades.Controls.Add((Control) this.btnUnassign);
      this.flpSecurityTrades.Controls.Add((Control) this.siBtnExcel);
      this.flpSecurityTrades.FlowDirection = FlowDirection.RightToLeft;
      this.flpSecurityTrades.Location = new Point(209, 2);
      this.flpSecurityTrades.Margin = new Padding(0);
      this.flpSecurityTrades.Name = "flpSecurityTrades";
      this.flpSecurityTrades.Size = new Size(558, 22);
      this.flpSecurityTrades.TabIndex = 3;
      this.btnOpenDialog.Location = new Point(458, 0);
      this.btnOpenDialog.Margin = new Padding(0);
      this.btnOpenDialog.Name = "btnOpenDialog";
      this.btnOpenDialog.Size = new Size(100, 23);
      this.btnOpenDialog.TabIndex = 0;
      this.btnOpenDialog.Text = "Allocate Cmt";
      this.btnOpenDialog.UseVisualStyleBackColor = true;
      this.btnOpenDialog.Click += new EventHandler(this.btnOpenDialog_Click);
      this.btnUnassign.Location = new Point(358, 0);
      this.btnUnassign.Margin = new Padding(0);
      this.btnUnassign.Name = "btnUnassign";
      this.btnUnassign.Size = new Size(100, 23);
      this.btnUnassign.TabIndex = 1;
      this.btnUnassign.Text = "De-allocate Cmt";
      this.btnUnassign.UseVisualStyleBackColor = true;
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Enabled = false;
      this.siBtnExcel.Location = new Point(340, 4);
      this.siBtnExcel.Margin = new Padding(3, 4, 2, 3);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 10;
      this.siBtnExcel.TabStop = false;
      this.commitmentsPoolAllocateAmountControl1.BackColor = SystemColors.Control;
      this.commitmentsPoolAllocateAmountControl1.Dock = DockStyle.Bottom;
      this.commitmentsPoolAllocateAmountControl1.Location = new Point(0, 138);
      this.commitmentsPoolAllocateAmountControl1.Name = "commitmentsPoolAllocateAmountControl1";
      this.commitmentsPoolAllocateAmountControl1.Size = new Size(769, 18);
      this.commitmentsPoolAllocateAmountControl1.TabIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpCommitments);
      this.Controls.Add((Control) this.commitmentsPoolAllocateAmountControl1);
      this.Name = nameof (CommitmentListScreen);
      this.Size = new Size(769, 156);
      this.grpCommitments.ResumeLayout(false);
      this.flpSecurityTrades.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.ResumeLayout(false);
    }
  }
}
