// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockVoidWS
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
  public class LockVoidWS : UserControl
  {
    private LockVoidLog voidLog;
    private LockRequestLog requestLog;
    private Hashtable loanSnapshot;
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
    private TextBox txtVoidedDttm;
    private TextBox txtVoidedBy;
    private Panel panelComments;
    private Panel panelHeaderVoid;

    public LockVoidWS(Sessions.Session session, LockVoidLog voidLog)
    {
      this.voidLog = voidLog != null ? voidLog : throw new ArgumentNullException(nameof (voidLog));
      this.requestLog = Session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(voidLog.RequestGUID);
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
        this.txtRequestedDttm.Text = this.voidLog.Date.ToString("MM/dd/yyyy") + " " + this.requestLog.TimeRequested;
      this.txtRequestedBy.Text = this.voidLog.VoidedByFullName;
      this.groupContainerAll.Text = "Lock Voided by " + this.voidLog.VoidedByFullName;
      if (this.voidLog.Date != DateTime.MinValue)
        this.txtVoidedDttm.Text = this.voidLog.Date.ToString("MM/dd/yyyy") + " " + this.voidLog.TimeVoided;
      this.txtVoidedBy.Text = this.voidLog.VoidedByFullName;
      this.txtComments.Text = this.voidLog.Comments;
    }

    private void clearAlertBtn_Click(object sender, EventArgs e)
    {
      if (this.voidLog != null)
        this.voidLog.AlertLoanOfficer = false;
      this.checkAlertStatus();
      int num = (int) Utils.Dialog((IWin32Window) this, "All Lock Alerts have been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void checkAlertStatus() => this.clearAlertBtn.Enabled = this.voidLog.AlertLoanOfficer;

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
      this.panelComments = new Panel();
      this.panelHeaderVoid = new Panel();
      this.txtRequestedBy = new TextBox();
      this.txtVoidedBy = new TextBox();
      this.label4 = new Label();
      this.txtVoidedDttm = new TextBox();
      this.label1 = new Label();
      this.txtRequestedDttm = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.groupContainerAll = new GroupContainer();
      this.clearAlertBtn = new Button();
      this.panelInside = new Panel();
      this.borderPanelComment.SuspendLayout();
      this.panelComments.SuspendLayout();
      this.panelHeaderVoid.SuspendLayout();
      this.groupContainerAll.SuspendLayout();
      this.panelInside.SuspendLayout();
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
      this.panelSnapshot.Location = new Point(0, 175);
      this.panelSnapshot.Name = "panelSnapshot";
      this.panelSnapshot.Size = new Size(540, 515);
      this.panelSnapshot.TabIndex = 15;
      this.borderPanelComment.Borders = AnchorStyles.Right;
      this.borderPanelComment.Controls.Add((Control) this.panelComments);
      this.borderPanelComment.Controls.Add((Control) this.panelHeaderVoid);
      this.borderPanelComment.Location = new Point(0, 0);
      this.borderPanelComment.Name = "borderPanelComment";
      this.borderPanelComment.Size = new Size(540, 238);
      this.borderPanelComment.TabIndex = 0;
      this.panelComments.Controls.Add((Control) this.txtComments);
      this.panelComments.Controls.Add((Control) this.label12);
      this.panelComments.Location = new Point(0, 65);
      this.panelComments.Name = "panelComments";
      this.panelComments.Size = new Size(534, 106);
      this.panelComments.TabIndex = 49;
      this.panelHeaderVoid.Controls.Add((Control) this.txtRequestedBy);
      this.panelHeaderVoid.Controls.Add((Control) this.txtVoidedBy);
      this.panelHeaderVoid.Controls.Add((Control) this.label4);
      this.panelHeaderVoid.Controls.Add((Control) this.txtVoidedDttm);
      this.panelHeaderVoid.Controls.Add((Control) this.label1);
      this.panelHeaderVoid.Controls.Add((Control) this.txtRequestedDttm);
      this.panelHeaderVoid.Controls.Add((Control) this.label2);
      this.panelHeaderVoid.Controls.Add((Control) this.label3);
      this.panelHeaderVoid.Location = new Point(0, 5);
      this.panelHeaderVoid.Name = "panelHeaderVoid";
      this.panelHeaderVoid.Size = new Size(534, 57);
      this.panelHeaderVoid.TabIndex = 47;
      this.txtRequestedBy.BackColor = Color.WhiteSmoke;
      this.txtRequestedBy.Location = new Point(120, 33);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(143, 20);
      this.txtRequestedBy.TabIndex = 46;
      this.txtVoidedBy.BackColor = Color.WhiteSmoke;
      this.txtVoidedBy.Location = new Point(373, 33);
      this.txtVoidedBy.Name = "txtVoidedBy";
      this.txtVoidedBy.ReadOnly = true;
      this.txtVoidedBy.Size = new Size(150, 20);
      this.txtVoidedBy.TabIndex = 44;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(0, 36);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 13);
      this.label4.TabIndex = 45;
      this.label4.Text = "Requested By";
      this.txtVoidedDttm.BackColor = Color.WhiteSmoke;
      this.txtVoidedDttm.Location = new Point(373, 7);
      this.txtVoidedDttm.Name = "txtVoidedDttm";
      this.txtVoidedDttm.ReadOnly = true;
      this.txtVoidedDttm.Size = new Size(150, 20);
      this.txtVoidedDttm.TabIndex = 39;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(266, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 41;
      this.label1.Text = "Voided Date/Time";
      this.txtRequestedDttm.BackColor = Color.WhiteSmoke;
      this.txtRequestedDttm.Location = new Point(119, 7);
      this.txtRequestedDttm.Name = "txtRequestedDttm";
      this.txtRequestedDttm.ReadOnly = true;
      this.txtRequestedDttm.Size = new Size(143, 20);
      this.txtRequestedDttm.TabIndex = 40;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 13);
      this.label2.TabIndex = 42;
      this.label2.Text = "Requested Date/Time";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(266, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(55, 13);
      this.label3.TabIndex = 43;
      this.label3.Text = "Voided By";
      this.groupContainerAll.Controls.Add((Control) this.clearAlertBtn);
      this.groupContainerAll.Controls.Add((Control) this.panelInside);
      this.groupContainerAll.Dock = DockStyle.Fill;
      this.groupContainerAll.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerAll.Location = new Point(0, 0);
      this.groupContainerAll.Name = "groupContainerAll";
      this.groupContainerAll.Size = new Size(600, 813);
      this.groupContainerAll.TabIndex = 46;
      this.groupContainerAll.Text = "Lock Voided by ";
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
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainerAll);
      this.Name = nameof (LockVoidWS);
      this.Size = new Size(600, 813);
      this.borderPanelComment.ResumeLayout(false);
      this.panelComments.ResumeLayout(false);
      this.panelComments.PerformLayout();
      this.panelHeaderVoid.ResumeLayout(false);
      this.panelHeaderVoid.PerformLayout();
      this.groupContainerAll.ResumeLayout(false);
      this.panelInside.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
