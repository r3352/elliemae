// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.RemoveLoansOptionsDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class RemoveLoansOptionsDialog : Form
  {
    private List<LoanToTradeAssignmentBase> selectedAssignments;
    private string investorName;
    private bool isMultiEdit;
    private bool isCommentEditReady;
    private int origBtnTop;
    private int margin = 10;
    private TradeType theTradeType;
    public bool isVoidedFlag;
    private IContainer components;
    private PictureBox pictureBox1;
    private Label label1;
    private CheckBox chkReject;
    private Button btnYes;
    private Button btnNo;
    private Panel pnlSingle;
    private Panel pnlButton;
    private Label lblComment;
    private TextBox txtComment;
    private Panel pnlMultiple;
    private PictureBox pictureBox2;
    private Label label3;
    private CheckBox chkRejectMultiple;
    private Panel pnlGrid;
    private TableContainer tableComments;
    private GridView gvComments;
    private Panel pnlGroup;

    public RemoveLoansOptionsDialog(
      string investorName,
      List<LoanToTradeAssignmentBase> selectedAssignments,
      TradeType tradeType)
    {
      this.InitializeComponent();
      this.selectedAssignments = selectedAssignments;
      this.investorName = investorName ?? "";
      this.origBtnTop = this.btnYes.Top;
      this.theTradeType = tradeType;
      this.initScreen(tradeType);
    }

    public bool MarkAsRejected
    {
      get => this.isMultiEdit ? this.chkRejectMultiple.Checked : this.chkReject.Checked;
    }

    public bool AllowCommentEdit
    {
      get
      {
        switch (this.theTradeType)
        {
          case TradeType.LoanTrade:
          case TradeType.MbsPool:
            return !string.IsNullOrEmpty(this.investorName);
          default:
            return false;
        }
      }
    }

    public IList<LoanToTradeAssignmentBase> Comments
    {
      get
      {
        if (this.AllowCommentEdit)
        {
          this.selectedAssignments.Clear();
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvComments.Items)
            this.selectedAssignments.Add((LoanToTradeAssignmentBase) gvItem.Tag);
        }
        return (IList<LoanToTradeAssignmentBase>) this.selectedAssignments;
      }
    }

    private void initScreen(TradeType tradeType)
    {
      string str = string.Empty;
      switch (tradeType)
      {
        case TradeType.LoanTrade:
        case TradeType.CorrespondentTrade:
          str = "Trade";
          break;
        case TradeType.MbsPool:
          str = "MBS Pool";
          break;
      }
      this.label1.Text = this.label3.Text = "Are you sure you want to remove the selected loans from the " + str;
      if (this.selectedAssignments.Count > 1)
        this.isMultiEdit = true;
      this.alignButtons(ContentAlignment.MiddleRight);
      if (this.isMultiEdit)
      {
        this.hideControl((Control) this.pnlSingle);
        if (this.isCommentEditReady)
        {
          this.hideControl((Control) this.pnlMultiple);
          this.showControl((Control) this.pnlGrid);
          this.Height = this.pnlGrid.Height + this.pnlButton.Height + this.margin * 3;
          this.Width = this.pnlGrid.Width;
          this.pnlGrid.Dock = DockStyle.Fill;
          this.Text = "Rejected Loan Comments";
          this.btnYes.Text = "OK";
          this.btnNo.Text = "Cancel";
          this.alignButtons(ContentAlignment.MiddleRight);
          this.initGVComments();
        }
        else
        {
          this.hideControl((Control) this.pnlGrid);
          this.showControl((Control) this.pnlMultiple);
          this.Height = this.pnlMultiple.Height + this.pnlButton.Height + this.margin * 3;
          this.Width = this.pnlMultiple.Width;
          this.pnlMultiple.Dock = DockStyle.Fill;
          this.Text = "Remove Loans from " + str;
          this.btnYes.Text = "Yes";
          this.btnNo.Text = "No";
        }
      }
      else
      {
        this.showControl((Control) this.pnlSingle);
        this.hideControl((Control) this.pnlMultiple);
        this.hideControl((Control) this.pnlGrid);
        this.Height = this.pnlSingle.Height + this.pnlButton.Height + this.margin * 3;
        this.Width = this.pnlSingle.Width + this.margin;
        this.pnlSingle.Dock = DockStyle.Fill;
        this.txtComment.Enabled = false;
        this.Text = "Remove Loan from " + str;
        this.btnYes.Text = "Yes";
        this.btnNo.Text = "No";
      }
      if (!this.AllowCommentEdit)
      {
        this.chkReject.Visible = false;
        this.chkRejectMultiple.Visible = false;
        this.txtComment.Visible = false;
        this.lblComment.Visible = false;
        if (this.isMultiEdit)
        {
          if (tradeType == TradeType.CorrespondentTrade)
          {
            this.chkRejectMultiple.Visible = true;
            this.chkRejectMultiple.Text = "Flag selected loans as \"voided\"";
          }
          else
            this.Height -= this.chkRejectMultiple.Height + this.margin;
        }
        else if (tradeType == TradeType.CorrespondentTrade)
        {
          this.chkReject.Visible = true;
          this.chkReject.Text = "Flag selected loan as \"voided\"";
        }
        else
          this.Height -= this.chkReject.Height + this.txtComment.Height + this.margin;
      }
      else
      {
        this.chkReject.Text = this.chkReject.Text.Replace("%INVESTOR%", this.investorName);
        this.chkRejectMultiple.Text = this.chkRejectMultiple.Text.Replace("%INVESTOR%", this.investorName);
      }
    }

    private void initGVComments()
    {
      this.gvComments.Items.Clear();
      foreach (LoanToTradeAssignmentBase selectedAssignment in this.selectedAssignments)
        this.gvComments.Items.Add(this.createAssignment(selectedAssignment));
      this.tableComments.Text = this.tableComments.Text.Replace("###", this.selectedAssignments.Count.ToString());
      this.gvComments.Sort(0, SortOrder.Ascending);
    }

    private GVItem createAssignment(LoanToTradeAssignmentBase assignment)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) assignment.PipelineInfo.LoanNumber
          },
          [1] = {
            Value = (object) assignment.PipelineInfo.LastName
          },
          [2] = {
            Value = (object) assignment.RejectedReason
          }
        },
        Tag = (object) assignment
      };
    }

    private void alignButtons(ContentAlignment alignment)
    {
      if (alignment == ContentAlignment.MiddleCenter)
      {
        this.btnYes.Left = (this.pnlButton.Width - this.btnYes.Width) / 2 + this.margin;
        this.btnYes.Top = this.origBtnTop;
        this.btnNo.Left = this.btnYes.Left + this.btnYes.Width + this.margin;
        this.btnNo.Top = this.origBtnTop;
      }
      else
      {
        this.btnYes.Left = this.pnlButton.Right - this.btnNo.Width - this.btnYes.Width - this.margin * 2;
        this.btnYes.Top = this.origBtnTop;
        this.btnNo.Left = this.pnlButton.Right - this.margin - this.btnNo.Width;
        this.btnNo.Top = this.origBtnTop;
      }
    }

    private void hideShowControl(Control c, bool isHidden)
    {
      c.Visible = !isHidden;
      foreach (Control control in (ArrangedElementCollection) c.Controls)
      {
        control.Visible = !isHidden;
        if (control.HasChildren)
          this.hideShowControl(control, isHidden);
      }
    }

    private void hideControl(Control c) => this.hideShowControl(c, true);

    private void showControl(Control c) => this.hideShowControl(c, false);

    private void gvComments_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void gvComments_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e.EditorControl.Text.Length > 50)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Maximum of 50 characters allowed in the Comments.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.EditorControl.Text = e.EditorControl.Text.Substring(0, 50);
        e.Cancel = true;
      }
      else
        ((LoanToTradeAssignmentBase) e.SubItem.Item.Tag).RejectedReason = e.EditorControl.Text;
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      Dictionary<string, object> settings = new Dictionary<string, object>();
      if (this.chkRejectMultiple.Checked || this.chkReject.Checked)
        settings.Add("Policies.VoidedFlagEnabled", (object) true);
      else
        settings.Add("Policies.VoidedFlagEnabled", (object) false);
      Session.ServerManager.UpdateServerSettings((IDictionary) settings, true, false);
      if (this.isMultiEdit)
      {
        if (!this.chkRejectMultiple.Checked || this.theTradeType == TradeType.CorrespondentTrade)
          this.DialogResult = DialogResult.Yes;
        else if (this.isCommentEditReady)
        {
          this.DialogResult = DialogResult.Yes;
        }
        else
        {
          this.isCommentEditReady = true;
          this.initScreen(this.theTradeType);
        }
      }
      else
      {
        if (this.chkReject.Checked)
          this.selectedAssignments[0].RejectedReason = this.txtComment.Text;
        this.DialogResult = DialogResult.Yes;
      }
    }

    private void btnNo_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.No;

    private void chkReject_CheckedChanged(object sender, EventArgs e)
    {
      this.txtComment.Enabled = this.chkReject.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RemoveLoansOptionsDialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.chkReject = new CheckBox();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.pnlSingle = new Panel();
      this.lblComment = new Label();
      this.txtComment = new TextBox();
      this.pnlButton = new Panel();
      this.pnlMultiple = new Panel();
      this.pictureBox2 = new PictureBox();
      this.label3 = new Label();
      this.chkRejectMultiple = new CheckBox();
      this.pnlGrid = new Panel();
      this.tableComments = new TableContainer();
      this.gvComments = new GridView();
      this.pnlGroup = new Panel();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.pnlSingle.SuspendLayout();
      this.pnlButton.SuspendLayout();
      this.pnlMultiple.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.pnlGrid.SuspendLayout();
      this.tableComments.SuspendLayout();
      this.SuspendLayout();
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(7, 5);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(51, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(333, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "Are you sure you want to remove the selected loan from the trade?";
      this.chkReject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkReject.CheckAlign = ContentAlignment.TopLeft;
      this.chkReject.Location = new Point(51, 32);
      this.chkReject.Name = "chkReject";
      this.chkReject.Size = new Size(391, 23);
      this.chkReject.TabIndex = 2;
      this.chkReject.Text = "Flag selected loan as rejected by investor \"%INVESTOR%\"";
      this.chkReject.TextAlign = ContentAlignment.TopLeft;
      this.chkReject.UseVisualStyleBackColor = true;
      this.chkReject.CheckedChanged += new EventHandler(this.chkReject_CheckedChanged);
      this.btnYes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnYes.Location = new Point(405, 12);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 22);
      this.btnYes.TabIndex = 3;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.btnNo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.Cancel;
      this.btnNo.Location = new Point(486, 12);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 22);
      this.btnNo.TabIndex = 4;
      this.btnNo.Text = "&No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.pnlSingle.Controls.Add((Control) this.lblComment);
      this.pnlSingle.Controls.Add((Control) this.txtComment);
      this.pnlSingle.Controls.Add((Control) this.pictureBox1);
      this.pnlSingle.Controls.Add((Control) this.label1);
      this.pnlSingle.Controls.Add((Control) this.chkReject);
      this.pnlSingle.Location = new Point(12, 7);
      this.pnlSingle.Name = "pnlSingle";
      this.pnlSingle.Size = new Size(453, 89);
      this.pnlSingle.TabIndex = 5;
      this.lblComment.AutoSize = true;
      this.lblComment.Location = new Point(49, 63);
      this.lblComment.Name = "lblComment";
      this.lblComment.Size = new Size(57, 14);
      this.lblComment.TabIndex = 4;
      this.lblComment.Text = "Comments";
      this.txtComment.Location = new Point(114, 60);
      this.txtComment.MaxLength = 50;
      this.txtComment.Name = "txtComment";
      this.txtComment.Size = new Size(330, 20);
      this.txtComment.TabIndex = 3;
      this.pnlButton.Controls.Add((Control) this.btnYes);
      this.pnlButton.Controls.Add((Control) this.btnNo);
      this.pnlButton.Dock = DockStyle.Bottom;
      this.pnlButton.Location = new Point(0, 492);
      this.pnlButton.Name = "pnlButton";
      this.pnlButton.Size = new Size(571, 45);
      this.pnlButton.TabIndex = 6;
      this.pnlMultiple.Controls.Add((Control) this.pictureBox2);
      this.pnlMultiple.Controls.Add((Control) this.label3);
      this.pnlMultiple.Controls.Add((Control) this.chkRejectMultiple);
      this.pnlMultiple.Location = new Point(12, 100);
      this.pnlMultiple.Name = "pnlMultiple";
      this.pnlMultiple.Size = new Size(453, 85);
      this.pnlMultiple.TabIndex = 7;
      this.pictureBox2.Image = (Image) componentResourceManager.GetObject("pictureBox2.Image");
      this.pictureBox2.Location = new Point(7, 5);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(32, 32);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 0;
      this.pictureBox2.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(51, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(339, 14);
      this.label3.TabIndex = 1;
      this.label3.Text = "Are you sure you want to remove the selected loans from the trade?";
      this.chkRejectMultiple.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkRejectMultiple.CheckAlign = ContentAlignment.TopLeft;
      this.chkRejectMultiple.Location = new Point(51, 34);
      this.chkRejectMultiple.Name = "chkRejectMultiple";
      this.chkRejectMultiple.Size = new Size(391, 46);
      this.chkRejectMultiple.TabIndex = 2;
      this.chkRejectMultiple.Text = "Flag selected loans as rejected by investor \"%INVESTOR%\".  If rejected, you'll next add a comment for each loan describing why the loans were rejected.";
      this.chkRejectMultiple.TextAlign = ContentAlignment.TopLeft;
      this.chkRejectMultiple.UseVisualStyleBackColor = true;
      this.pnlGrid.Controls.Add((Control) this.tableComments);
      this.pnlGrid.Location = new Point(13, 200);
      this.pnlGrid.Name = "pnlGrid";
      this.pnlGrid.Size = new Size(549, 286);
      this.pnlGrid.TabIndex = 8;
      this.tableComments.Borders = AnchorStyles.None;
      this.tableComments.Controls.Add((Control) this.gvComments);
      this.tableComments.Dock = DockStyle.Fill;
      this.tableComments.Location = new Point(0, 0);
      this.tableComments.Name = "tableComments";
      this.tableComments.Size = new Size(549, 286);
      this.tableComments.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableComments.TabIndex = 0;
      this.tableComments.Text = "Rejected Loan Comments (###)";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Loan Number";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Borrower Last Name";
      gvColumn2.Width = 120;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Comments";
      gvColumn3.Width = 327;
      this.gvComments.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvComments.Dock = DockStyle.Fill;
      this.gvComments.Location = new Point(0, 25);
      this.gvComments.Name = "gvComments";
      this.gvComments.Size = new Size(549, 261);
      this.gvComments.TabIndex = 0;
      this.gvComments.SelectedIndexChanged += new EventHandler(this.gvComments_SelectedIndexChanged);
      this.gvComments.EditorClosing += new GVSubItemEditingEventHandler(this.gvComments_EditorClosing);
      this.pnlGroup.AutoSize = true;
      this.pnlGroup.Dock = DockStyle.Fill;
      this.pnlGroup.Location = new Point(0, 0);
      this.pnlGroup.Name = "pnlGroup";
      this.pnlGroup.Size = new Size(571, 492);
      this.pnlGroup.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(571, 537);
      this.Controls.Add((Control) this.pnlGrid);
      this.Controls.Add((Control) this.pnlMultiple);
      this.Controls.Add((Control) this.pnlSingle);
      this.Controls.Add((Control) this.pnlGroup);
      this.Controls.Add((Control) this.pnlButton);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RemoveLoansOptionsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Remove Loan(s) from Trade";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.pnlSingle.ResumeLayout(false);
      this.pnlSingle.PerformLayout();
      this.pnlButton.ResumeLayout(false);
      this.pnlMultiple.ResumeLayout(false);
      this.pnlMultiple.PerformLayout();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.pnlGrid.ResumeLayout(false);
      this.tableComments.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
