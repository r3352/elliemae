// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockDenialWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LockDenialWS : UserControl
  {
    private LockDenialLog denialLog;
    private LockRequestLog requestLog;
    private Hashtable loanSnapshot;
    private LockDenialLog[] denialLocks;
    private LockConfirmLog[] confirmLocks;
    private LockCancellationLog[] cancellationLocks;
    private Sessions.Session session;
    private IContainer components;
    private Label label12;
    private TextBox textBoxComments;
    private Panel panelSnapshot;
    private TextBox boxDeniedBy;
    private Label label3;
    private Label label2;
    private Label label1;
    private TextBox boxDeniedDttm;
    private TextBox boxRequestedDttm;
    private ToolTip toolTipField;
    private BorderPanel borderPanelComment;
    private GroupContainer groupContainerAll;
    private Panel panelInside;
    private Button clearAlertBtn;
    private TextBox txtRequestedBy;
    private Label label4;

    public LockDenialWS(Sessions.Session session, LockDenialLog denialLog)
    {
      this.session = session;
      this.denialLog = denialLog;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.requestLog = Session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(this.denialLog.RequestGUID);
      this.initForm();
      this.checkAleartStatus();
    }

    private void initForm()
    {
      if (!this.requestLog.IsLockExtension)
        this.groupContainerAll.Text += this.denialLog.DeniedByFullName;
      else
        this.groupContainerAll.Text = "Extension Denied by " + this.denialLog.DeniedByFullName;
      this.loanSnapshot = this.requestLog.GetLockRequestSnapshot();
      LoanSnapshotForm loanSnapshotForm = new LoanSnapshotForm(this.session, this.requestLog, false, Session.LoanDataMgr.LoanData);
      this.panelSnapshot.Controls.Add((Control) loanSnapshotForm);
      loanSnapshotForm.RemoveLeftBorder();
      loanSnapshotForm.RefreshSnapshotForm(this.loanSnapshot, Session.LoanDataMgr.LoanData);
      loanSnapshotForm.BringToFront();
      if (this.requestLog.Date != DateTime.MinValue)
        this.boxRequestedDttm.Text = this.requestLog.Date.ToString("MM/dd/yyyy") + " " + this.requestLog.TimeRequested;
      this.txtRequestedBy.Text = this.requestLog.RequestedFullName;
      if (this.denialLog.Date != DateTime.MinValue)
        this.boxDeniedDttm.Text = this.denialLog.Date.ToString("MM/dd/yyyy") + " " + this.denialLog.TimeDenied.ToString();
      this.boxDeniedBy.Text = this.denialLog.DeniedByFullName;
      this.textBoxComments.Text = this.denialLog.Comments;
      this.panelSnapshot.Height = loanSnapshotForm.Height + 20;
    }

    private void clearAlertBtn_Click(object sender, EventArgs e)
    {
      LogList logList = Session.LoanData.GetLogList();
      if (this.confirmLocks == null)
        this.confirmLocks = (LockConfirmLog[]) logList.GetAllRecordsOfType(typeof (LockConfirmLog));
      foreach (LockConfirmLog confirmLock in this.confirmLocks)
        confirmLock.AlertLoanOfficer = false;
      if (this.denialLocks == null)
        this.denialLocks = (LockDenialLog[]) logList.GetAllRecordsOfType(typeof (LockDenialLog));
      foreach (LockDenialLog denialLock in this.denialLocks)
        denialLock.AlertLoanOfficer = false;
      if (this.cancellationLocks == null)
        this.cancellationLocks = (LockCancellationLog[]) logList.GetAllRecordsOfType(typeof (LockCancellationLog));
      foreach (LockCancellationLog cancellationLock in this.cancellationLocks)
        cancellationLock.AlertLoanOfficer = false;
      this.checkAleartStatus();
      int num = (int) Utils.Dialog((IWin32Window) this, "All Lock Alerts have been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void checkAleartStatus()
    {
      LogList logList = Session.LoanData.GetLogList();
      if (this.confirmLocks == null)
        this.confirmLocks = (LockConfirmLog[]) logList.GetAllRecordsOfType(typeof (LockConfirmLog));
      foreach (LockConfirmLog confirmLock in this.confirmLocks)
      {
        if (confirmLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      if (this.denialLocks == null)
        this.denialLocks = (LockDenialLog[]) logList.GetAllRecordsOfType(typeof (LockDenialLog));
      foreach (LockDenialLog denialLock in this.denialLocks)
      {
        if (denialLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      if (this.cancellationLocks == null)
        this.cancellationLocks = (LockCancellationLog[]) logList.GetAllRecordsOfType(typeof (LockCancellationLog));
      foreach (LockCancellationLog cancellationLock in this.cancellationLocks)
      {
        if (cancellationLock.AlertLoanOfficer)
        {
          this.clearAlertBtn.Enabled = true;
          return;
        }
      }
      this.clearAlertBtn.Enabled = false;
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
      this.label12 = new Label();
      this.textBoxComments = new TextBox();
      this.panelSnapshot = new Panel();
      this.boxDeniedBy = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.boxDeniedDttm = new TextBox();
      this.boxRequestedDttm = new TextBox();
      this.toolTipField = new ToolTip(this.components);
      this.borderPanelComment = new BorderPanel();
      this.groupContainerAll = new GroupContainer();
      this.clearAlertBtn = new Button();
      this.panelInside = new Panel();
      this.label4 = new Label();
      this.txtRequestedBy = new TextBox();
      this.borderPanelComment.SuspendLayout();
      this.groupContainerAll.SuspendLayout();
      this.panelInside.SuspendLayout();
      this.SuspendLayout();
      this.label12.AutoSize = true;
      this.label12.Location = new Point(7, 54);
      this.label12.Name = "label12";
      this.label12.Size = new Size(56, 13);
      this.label12.TabIndex = 35;
      this.label12.Text = "Comments";
      this.textBoxComments.BackColor = Color.WhiteSmoke;
      this.textBoxComments.Location = new Point(10, 71);
      this.textBoxComments.Multiline = true;
      this.textBoxComments.Name = "textBoxComments";
      this.textBoxComments.ReadOnly = true;
      this.textBoxComments.ScrollBars = ScrollBars.Both;
      this.textBoxComments.Size = new Size(520, 80);
      this.textBoxComments.TabIndex = 36;
      this.textBoxComments.Tag = (object) "2144";
      this.panelSnapshot.Location = new Point(0, 156);
      this.panelSnapshot.Name = "panelSnapshot";
      this.panelSnapshot.Size = new Size(540, 603);
      this.panelSnapshot.TabIndex = 15;
      this.boxDeniedBy.BackColor = Color.WhiteSmoke;
      this.boxDeniedBy.Location = new Point(377, 31);
      this.boxDeniedBy.Name = "boxDeniedBy";
      this.boxDeniedBy.ReadOnly = true;
      this.boxDeniedBy.Size = new Size(153, 20);
      this.boxDeniedBy.TabIndex = 10;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(276, 34);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Denied By";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Requested Date/Time";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(276, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Denied Date/Time";
      this.boxDeniedDttm.BackColor = Color.WhiteSmoke;
      this.boxDeniedDttm.Location = new Point(377, 5);
      this.boxDeniedDttm.Name = "boxDeniedDttm";
      this.boxDeniedDttm.ReadOnly = true;
      this.boxDeniedDttm.Size = new Size(153, 20);
      this.boxDeniedDttm.TabIndex = 4;
      this.boxRequestedDttm.BackColor = Color.WhiteSmoke;
      this.boxRequestedDttm.Location = new Point((int) sbyte.MaxValue, 5);
      this.boxRequestedDttm.Name = "boxRequestedDttm";
      this.boxRequestedDttm.ReadOnly = true;
      this.boxRequestedDttm.Size = new Size(143, 20);
      this.boxRequestedDttm.TabIndex = 6;
      this.borderPanelComment.Borders = AnchorStyles.Right;
      this.borderPanelComment.Controls.Add((Control) this.txtRequestedBy);
      this.borderPanelComment.Controls.Add((Control) this.label4);
      this.borderPanelComment.Controls.Add((Control) this.textBoxComments);
      this.borderPanelComment.Controls.Add((Control) this.label1);
      this.borderPanelComment.Controls.Add((Control) this.label12);
      this.borderPanelComment.Controls.Add((Control) this.label2);
      this.borderPanelComment.Controls.Add((Control) this.label3);
      this.borderPanelComment.Controls.Add((Control) this.boxRequestedDttm);
      this.borderPanelComment.Controls.Add((Control) this.boxDeniedDttm);
      this.borderPanelComment.Controls.Add((Control) this.boxDeniedBy);
      this.borderPanelComment.Location = new Point(0, 0);
      this.borderPanelComment.Name = "borderPanelComment";
      this.borderPanelComment.Size = new Size(540, 157);
      this.borderPanelComment.TabIndex = 0;
      this.groupContainerAll.Controls.Add((Control) this.clearAlertBtn);
      this.groupContainerAll.Controls.Add((Control) this.panelInside);
      this.groupContainerAll.Dock = DockStyle.Fill;
      this.groupContainerAll.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerAll.Location = new Point(0, 0);
      this.groupContainerAll.Name = "groupContainerAll";
      this.groupContainerAll.Size = new Size(600, 813);
      this.groupContainerAll.TabIndex = 46;
      this.groupContainerAll.Text = "Lock Denied by ";
      this.clearAlertBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.clearAlertBtn.Location = new Point(518, 2);
      this.clearAlertBtn.Name = "clearAlertBtn";
      this.clearAlertBtn.Size = new Size(75, 22);
      this.clearAlertBtn.TabIndex = 1;
      this.clearAlertBtn.Text = "Clear Alert";
      this.clearAlertBtn.UseVisualStyleBackColor = true;
      this.clearAlertBtn.Click += new EventHandler(this.clearAlertBtn_Click);
      this.panelInside.AutoScroll = true;
      this.panelInside.Controls.Add((Control) this.panelSnapshot);
      this.panelInside.Controls.Add((Control) this.borderPanelComment);
      this.panelInside.Dock = DockStyle.Fill;
      this.panelInside.Location = new Point(1, 26);
      this.panelInside.Name = "panelInside";
      this.panelInside.Size = new Size(598, 786);
      this.panelInside.TabIndex = 0;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 34);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 13);
      this.label4.TabIndex = 37;
      this.label4.Text = "Requested By";
      this.txtRequestedBy.BackColor = Color.WhiteSmoke;
      this.txtRequestedBy.Location = new Point((int) sbyte.MaxValue, 34);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(143, 20);
      this.txtRequestedBy.TabIndex = 38;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainerAll);
      this.Name = nameof (LockDenialWS);
      this.Size = new Size(600, 813);
      this.borderPanelComment.ResumeLayout(false);
      this.borderPanelComment.PerformLayout();
      this.groupContainerAll.ResumeLayout(false);
      this.panelInside.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
