// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockCancellationWS
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
  public class LockCancellationWS : UserControl
  {
    private LockCancellationLog cancellationLog;
    private LockRequestLog requestLog;
    private Hashtable loanSnapshot;
    private LockCancellationLog[] cancellationLocks;
    private LockConfirmLog[] confirmLocks;
    private LockDenialLog[] denialLocks;
    private Sessions.Session session;
    private IContainer components;
    private Label label12;
    private TextBox txtComments;
    private Panel panelSnapshot;
    private ToolTip toolTipField;
    private BorderPanel borderPanelComment;
    private GroupContainer groupContainerAll;
    private Panel panelInside;
    private Button clearAlertBtn;
    private TextBox txtRequestedBy;
    private Label label4;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox txtRequestedDttm;
    private TextBox txtCancelledDttm;
    private TextBox txtCancelledBy;
    private Panel panelComments;
    private Panel panelHeaderRequest;
    private TextBox txtRequestedBy2;
    private Label label5;
    private TextBox txtRequestedDttm2;
    private Label label6;
    private Panel panelHeaderCancellation;

    public LockCancellationWS(Sessions.Session session, LockCancellationLog cancellationLog)
    {
      this.cancellationLog = cancellationLog != null ? cancellationLog : throw new ArgumentNullException(nameof (cancellationLog));
      this.requestLog = Session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(cancellationLog.RequestGUID);
      this.init(session);
    }

    public LockCancellationWS(Sessions.Session session, LockRequestLog requestLog)
    {
      if (requestLog == null)
        throw new ArgumentNullException(nameof (requestLog));
      this.cancellationLog = (LockCancellationLog) null;
      this.requestLog = requestLog;
      this.init(session);
    }

    private void init(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initForm();
      this.checkAlertStatus();
    }

    private void initForm()
    {
      this.loanSnapshot = this.requestLog.GetLockRequestSnapshot();
      LoanSnapshotForm loanSnapshotForm = new LoanSnapshotForm(this.session, this.requestLog, false, Session.LoanDataMgr.LoanData);
      this.panelSnapshot.Controls.Add((Control) loanSnapshotForm);
      loanSnapshotForm.RemoveLeftBorder();
      loanSnapshotForm.RefreshSnapshotForm(this.loanSnapshot, Session.LoanDataMgr.LoanData);
      loanSnapshotForm.BringToFront();
      if (this.requestLog.Date != DateTime.MinValue)
      {
        TextBox txtRequestedDttm2 = this.txtRequestedDttm2;
        TextBox txtRequestedDttm = this.txtRequestedDttm;
        DateTime date = this.requestLog.Date;
        string str1;
        string str2 = str1 = date.ToString("MM/dd/yyyy") + " " + this.requestLog.TimeRequested;
        txtRequestedDttm.Text = str1;
        string str3 = str2;
        txtRequestedDttm2.Text = str3;
      }
      this.txtRequestedBy2.Text = this.txtRequestedBy.Text = this.requestLog.RequestedFullName;
      if (this.loanSnapshot.ContainsKey((object) "2144"))
        this.txtComments.Text = (string) this.loanSnapshot[(object) "2144"];
      if (this.cancellationLog == null)
      {
        this.groupContainerAll.Text += this.requestLog.RequestedFullName;
        this.panelHeaderCancellation.Visible = false;
        this.panelHeaderRequest.Visible = true;
        this.panelHeaderRequest.Top = 0;
        this.panelComments.Top = this.panelHeaderRequest.Top + this.panelHeaderRequest.Height;
        this.panelSnapshot.Top = this.panelComments.Top + this.panelComments.Height;
      }
      else
      {
        this.groupContainerAll.Text = "Lock Cancelled by " + this.cancellationLog.CancelledByFullName;
        if (this.cancellationLog.Date != DateTime.MinValue)
          this.txtCancelledDttm.Text = this.cancellationLog.Date.ToString("MM/dd/yyyy") + " " + this.cancellationLog.TimeCancelled.ToString();
        this.txtCancelledBy.Text = this.cancellationLog.CancelledByFullName;
        this.panelHeaderRequest.Visible = false;
        this.panelHeaderCancellation.Visible = true;
        this.panelHeaderCancellation.Top = 0;
        this.panelComments.Top = this.panelHeaderCancellation.Top + this.panelHeaderCancellation.Height;
        this.panelSnapshot.Top = this.panelComments.Top + this.panelComments.Height;
      }
      this.panelSnapshot.Height = loanSnapshotForm.Height + 20;
    }

    private void clearAlertBtn_Click(object sender, EventArgs e)
    {
      LogList logList = this.session.LoanData.GetLogList();
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
      this.checkAlertStatus();
      int num = (int) Utils.Dialog((IWin32Window) this, "All Lock Alerts have been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void checkAlertStatus()
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
      this.txtComments = new TextBox();
      this.panelSnapshot = new Panel();
      this.toolTipField = new ToolTip(this.components);
      this.borderPanelComment = new BorderPanel();
      this.groupContainerAll = new GroupContainer();
      this.clearAlertBtn = new Button();
      this.panelInside = new Panel();
      this.txtRequestedBy = new TextBox();
      this.label4 = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtRequestedDttm = new TextBox();
      this.txtCancelledDttm = new TextBox();
      this.txtCancelledBy = new TextBox();
      this.panelHeaderCancellation = new Panel();
      this.panelHeaderRequest = new Panel();
      this.txtRequestedBy2 = new TextBox();
      this.label5 = new Label();
      this.txtRequestedDttm2 = new TextBox();
      this.label6 = new Label();
      this.panelComments = new Panel();
      this.borderPanelComment.SuspendLayout();
      this.groupContainerAll.SuspendLayout();
      this.panelInside.SuspendLayout();
      this.panelHeaderCancellation.SuspendLayout();
      this.panelHeaderRequest.SuspendLayout();
      this.panelComments.SuspendLayout();
      this.SuspendLayout();
      this.label12.AutoSize = true;
      this.label12.Location = new Point(0, 6);
      this.label12.Name = "label12";
      this.label12.Size = new Size(56, 13);
      this.label12.TabIndex = 35;
      this.label12.Text = "Comments";
      this.txtComments.BackColor = Color.WhiteSmoke;
      this.txtComments.Location = new Point(3, 23);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ReadOnly = true;
      this.txtComments.ScrollBars = ScrollBars.Both;
      this.txtComments.Size = new Size(520, 80);
      this.txtComments.TabIndex = 36;
      this.txtComments.Tag = (object) "";
      this.panelSnapshot.Location = new Point(0, 210);
      this.panelSnapshot.Name = "panelSnapshot";
      this.panelSnapshot.Size = new Size(540, 515);
      this.panelSnapshot.TabIndex = 15;
      this.borderPanelComment.Borders = AnchorStyles.Right;
      this.borderPanelComment.Controls.Add((Control) this.panelComments);
      this.borderPanelComment.Controls.Add((Control) this.panelHeaderRequest);
      this.borderPanelComment.Controls.Add((Control) this.panelHeaderCancellation);
      this.borderPanelComment.Location = new Point(0, 0);
      this.borderPanelComment.Name = "borderPanelComment";
      this.borderPanelComment.Size = new Size(540, 238);
      this.borderPanelComment.TabIndex = 0;
      this.groupContainerAll.Controls.Add((Control) this.clearAlertBtn);
      this.groupContainerAll.Controls.Add((Control) this.panelInside);
      this.groupContainerAll.Dock = DockStyle.Fill;
      this.groupContainerAll.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerAll.Location = new Point(0, 0);
      this.groupContainerAll.Name = "groupContainerAll";
      this.groupContainerAll.Size = new Size(600, 813);
      this.groupContainerAll.TabIndex = 46;
      this.groupContainerAll.Text = "Lock Cancellation Requested by ";
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
      this.txtRequestedBy.BackColor = Color.WhiteSmoke;
      this.txtRequestedBy.Location = new Point(120, 33);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(143, 20);
      this.txtRequestedBy.TabIndex = 46;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(0, 36);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 13);
      this.label4.TabIndex = 45;
      this.label4.Text = "Requested By";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(266, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(108, 13);
      this.label1.TabIndex = 41;
      this.label1.Text = "Cancelled Date/Time";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 13);
      this.label2.TabIndex = 42;
      this.label2.Text = "Requested Date/Time";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(266, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 13);
      this.label3.TabIndex = 43;
      this.label3.Text = "Cancelled By";
      this.txtRequestedDttm.BackColor = Color.WhiteSmoke;
      this.txtRequestedDttm.Location = new Point(119, 7);
      this.txtRequestedDttm.Name = "txtRequestedDttm";
      this.txtRequestedDttm.ReadOnly = true;
      this.txtRequestedDttm.Size = new Size(143, 20);
      this.txtRequestedDttm.TabIndex = 40;
      this.txtCancelledDttm.BackColor = Color.WhiteSmoke;
      this.txtCancelledDttm.Location = new Point(373, 7);
      this.txtCancelledDttm.Name = "txtCancelledDttm";
      this.txtCancelledDttm.ReadOnly = true;
      this.txtCancelledDttm.Size = new Size(150, 20);
      this.txtCancelledDttm.TabIndex = 39;
      this.txtCancelledBy.BackColor = Color.WhiteSmoke;
      this.txtCancelledBy.Location = new Point(373, 33);
      this.txtCancelledBy.Name = "txtCancelledBy";
      this.txtCancelledBy.ReadOnly = true;
      this.txtCancelledBy.Size = new Size(150, 20);
      this.txtCancelledBy.TabIndex = 44;
      this.panelHeaderCancellation.Controls.Add((Control) this.txtRequestedBy);
      this.panelHeaderCancellation.Controls.Add((Control) this.txtCancelledBy);
      this.panelHeaderCancellation.Controls.Add((Control) this.label4);
      this.panelHeaderCancellation.Controls.Add((Control) this.txtCancelledDttm);
      this.panelHeaderCancellation.Controls.Add((Control) this.label1);
      this.panelHeaderCancellation.Controls.Add((Control) this.txtRequestedDttm);
      this.panelHeaderCancellation.Controls.Add((Control) this.label2);
      this.panelHeaderCancellation.Controls.Add((Control) this.label3);
      this.panelHeaderCancellation.Location = new Point(0, 34);
      this.panelHeaderCancellation.Name = "panelHeaderCancellation";
      this.panelHeaderCancellation.Size = new Size(534, 57);
      this.panelHeaderCancellation.TabIndex = 47;
      this.panelHeaderRequest.Controls.Add((Control) this.txtRequestedBy2);
      this.panelHeaderRequest.Controls.Add((Control) this.label5);
      this.panelHeaderRequest.Controls.Add((Control) this.txtRequestedDttm2);
      this.panelHeaderRequest.Controls.Add((Control) this.label6);
      this.panelHeaderRequest.Location = new Point(0, 0);
      this.panelHeaderRequest.Name = "panelHeaderRequest";
      this.panelHeaderRequest.Size = new Size(536, 30);
      this.panelHeaderRequest.TabIndex = 48;
      this.txtRequestedBy2.BackColor = Color.WhiteSmoke;
      this.txtRequestedBy2.Location = new Point(373, 6);
      this.txtRequestedBy2.Name = "txtRequestedBy2";
      this.txtRequestedBy2.ReadOnly = true;
      this.txtRequestedBy2.Size = new Size(150, 20);
      this.txtRequestedBy2.TabIndex = 50;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(274, 9);
      this.label5.Name = "label5";
      this.label5.Size = new Size(74, 13);
      this.label5.TabIndex = 49;
      this.label5.Text = "Requested By";
      this.txtRequestedDttm2.BackColor = Color.WhiteSmoke;
      this.txtRequestedDttm2.Location = new Point(120, 6);
      this.txtRequestedDttm2.Name = "txtRequestedDttm2";
      this.txtRequestedDttm2.ReadOnly = true;
      this.txtRequestedDttm2.Size = new Size(143, 20);
      this.txtRequestedDttm2.TabIndex = 47;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(0, 9);
      this.label6.Name = "label6";
      this.label6.Size = new Size(113, 13);
      this.label6.TabIndex = 48;
      this.label6.Text = "Requested Date/Time";
      this.panelComments.Controls.Add((Control) this.txtComments);
      this.panelComments.Controls.Add((Control) this.label12);
      this.panelComments.Location = new Point(0, 113);
      this.panelComments.Name = "panelComments";
      this.panelComments.Size = new Size(534, 106);
      this.panelComments.TabIndex = 49;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainerAll);
      this.Name = nameof (LockCancellationWS);
      this.Size = new Size(600, 813);
      this.borderPanelComment.ResumeLayout(false);
      this.groupContainerAll.ResumeLayout(false);
      this.panelInside.ResumeLayout(false);
      this.panelHeaderCancellation.ResumeLayout(false);
      this.panelHeaderCancellation.PerformLayout();
      this.panelHeaderRequest.ResumeLayout(false);
      this.panelHeaderRequest.PerformLayout();
      this.panelComments.ResumeLayout(false);
      this.panelComments.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
