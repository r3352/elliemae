// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.NotificationForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class NotificationForm : Form
  {
    private const string className = "NotificationForm";
    private const int edgePadding = 10;
    private static string sw = Tracing.SwCommon;
    private UserNotification notification;
    private IContainer components;
    private GroupContainer grpMain;
    private StandardIconButton btnClose;
    private ElementControl amlIcon;
    private Label lblTitle;
    private GradientPanel pnlBody;
    private FormFader formFader;
    private Label lblFrom;
    private Label lblFromLabel;
    private Panel panel2;
    private System.Windows.Forms.LinkLabel lnkAction;
    private PictureBox pictureBox1;
    private Label lblMessage;

    public NotificationForm(UserNotification notification)
    {
      this.InitializeComponent();
      this.notification = notification;
      this.loadNotification();
      this.formFader.AttachToForm((Form) this);
      this.StartPosition = FormStartPosition.Manual;
      this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10;
      this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 10;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void loadNotification()
    {
      if (this.notification is EPassMessageNotification)
      {
        this.loadEPassMessageNotification();
      }
      else
      {
        if (!(this.notification is LoanAlertNotification))
          throw new NotSupportedException("The notification type " + this.notification.GetType().Name + " is not supported");
        this.loadLoanAlertNotification();
      }
    }

    private void loadEPassMessageNotification()
    {
      EPassMessageNotification notification = (EPassMessageNotification) this.notification;
      EPassMessageInfo message = notification.Message;
      EPassMessageAction action = notification.Message.GetAction();
      this.amlIcon.Element = (object) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Message, "");
      if ((message.LoanGuid ?? "") != "")
        this.lblTitle.Text = this.getLoanTitle(message.LoanGuid);
      else
        this.lblTitle.Text = "New Message Arrived";
      this.lblMessage.Text = notification.Message.Description;
      this.lblFrom.Text = string.Concat((object) notification.Message.GetBorrowerName());
      if (this.lblFrom.Text == "")
        this.lblFrom.Text = notification.Message.Source;
      if (notification.Message.MessageType == "FILETRANSFER")
        this.lnkAction.Text = "Open Loan Mailbox";
      else
        this.lnkAction.Text = action.Description;
    }

    private void loadLoanAlertNotification()
    {
      LoanAlertNotification notification = (LoanAlertNotification) this.notification;
      this.amlIcon.Element = (object) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Alert, "!");
      this.lblTitle.Text = this.getLoanTitle(notification.PipelineInfo);
      this.lblMessage.Text = this.getAlertMessage(notification.PipelineInfo, notification.Alert);
      this.lblFrom.Text = notification.Sender.FullName;
      this.lnkAction.Text = this.getAlertActionText(notification.Alert);
    }

    private string getAlertMessage(PipelineInfo pinfo, PipelineInfo.Alert alert)
    {
      return alert.AlertID == 0 ? this.getMilestoneAlertMessage(pinfo, alert.AlertTargetID) : alert.Event;
    }

    private string getAlertActionText(PipelineInfo.Alert alert) => "Open Loan";

    private string getMilestoneAlertMessage(PipelineInfo pinfo, string milestoneId)
    {
      Hashtable milestoneAlertMessages = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllMilestoneAlertMessages();
      PipelineInfo.MilestoneInfo milestoneById = pinfo.GetMilestoneByID(milestoneId);
      return milestoneAlertMessages.Contains((object) milestoneById.MilestoneName) ? milestoneAlertMessages[(object) milestoneById.MilestoneName].ToString() : ((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(milestoneId).DescTextAfter;
    }

    private string getLoanTitle(string guid)
    {
      string[] fields = new string[3]
      {
        "Loan.BorrowerFirstName",
        "Loan.BorrowerLastName",
        "Loan.Address1"
      };
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(new string[1]
      {
        guid
      }, fields, PipelineData.Fields, false);
      return pipeline.Length != 0 && pipeline[0] != null ? this.getLoanTitle(pipeline[0]) : throw new Exception("The loan " + guid + " does not exist or cannot be found");
    }

    private string getLoanTitle(PipelineInfo pinfo)
    {
      string loanTitle = (pinfo.GetField("BorrowerFirstName").ToString() + " " + pinfo.GetField("BorrowerLastName")).Trim();
      if (string.Concat(pinfo.GetField("Address1")) != "")
      {
        if (loanTitle != "")
          loanTitle += " / ";
        loanTitle += (string) pinfo.GetField("Address1");
      }
      return loanTitle;
    }

    private void lnkAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      IEncompassApplication service = Session.Application.GetService<IEncompassApplication>();
      if (service.IsModalDialogOpen())
      {
        if (Utils.Dialog((IWin32Window) Session.MainForm, "There are currently one or more pop-up windows open in Encompass. These windows must be closed in order to proceed." + Environment.NewLine + Environment.NewLine + "Would you like to close all open windows and proceed with the selected action?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        if (!service.CloseModalDialogs())
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "One or more pop-up windows could not be closed. The action has been aborted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      this.Close();
      Notifications.PerformAction(this.notification);
    }

    private void onMouseEnterForm(object sender, EventArgs e) => this.formFader.EnsureOpaque();

    private void onMouseLeaveForm(object sender, EventArgs e)
    {
      if (this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
        return;
      this.formFader.Resume();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NotificationForm));
      this.grpMain = new GroupContainer();
      this.amlIcon = new ElementControl();
      this.btnClose = new StandardIconButton();
      this.lblTitle = new Label();
      this.pnlBody = new GradientPanel();
      this.lblMessage = new Label();
      this.lnkAction = new System.Windows.Forms.LinkLabel();
      this.pictureBox1 = new PictureBox();
      this.panel2 = new Panel();
      this.lblFrom = new Label();
      this.lblFromLabel = new Label();
      this.formFader = new FormFader();
      this.grpMain.SuspendLayout();
      ((ISupportInitialize) this.btnClose).BeginInit();
      this.pnlBody.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.grpMain.Controls.Add((Control) this.amlIcon);
      this.grpMain.Controls.Add((Control) this.btnClose);
      this.grpMain.Controls.Add((Control) this.lblTitle);
      this.grpMain.Controls.Add((Control) this.pnlBody);
      this.grpMain.Dock = DockStyle.Fill;
      this.grpMain.Location = new Point(0, 0);
      this.grpMain.Name = "grpMain";
      this.grpMain.Size = new Size(320, 97);
      this.grpMain.TabIndex = 0;
      this.grpMain.MouseLeave += new EventHandler(this.onMouseLeaveForm);
      this.grpMain.MouseEnter += new EventHandler(this.onMouseEnterForm);
      this.amlIcon.Element = (object) null;
      this.amlIcon.Location = new Point(7, 5);
      this.amlIcon.Name = "amlIcon";
      this.amlIcon.Size = new Size(21, 16);
      this.amlIcon.TabIndex = 1;
      this.amlIcon.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.Location = new Point(298, 5);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(16, 16);
      this.btnClose.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.btnClose.TabIndex = 0;
      this.btnClose.TabStop = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTitle.AutoEllipsis = true;
      this.lblTitle.BackColor = Color.Transparent;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(30, 6);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(264, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Notification Title";
      this.pnlBody.Borders = AnchorStyles.None;
      this.pnlBody.Controls.Add((Control) this.lblMessage);
      this.pnlBody.Controls.Add((Control) this.lnkAction);
      this.pnlBody.Controls.Add((Control) this.pictureBox1);
      this.pnlBody.Controls.Add((Control) this.panel2);
      this.pnlBody.Dock = DockStyle.Fill;
      this.pnlBody.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlBody.GradientColor2 = Color.FromArgb(221, 233, 249);
      this.pnlBody.Location = new Point(1, 26);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(318, 70);
      this.pnlBody.TabIndex = 4;
      this.pnlBody.MouseLeave += new EventHandler(this.onMouseLeaveForm);
      this.pnlBody.MouseEnter += new EventHandler(this.onMouseEnterForm);
      this.lblMessage.AutoEllipsis = true;
      this.lblMessage.BackColor = Color.Transparent;
      this.lblMessage.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMessage.ForeColor = Color.FromArgb((int) byte.MaxValue, 153, 0);
      this.lblMessage.Location = new Point(8, 8);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(302, 13);
      this.lblMessage.TabIndex = 10;
      this.lblMessage.Text = "Notification Message";
      this.lnkAction.ActiveLinkColor = Color.FromArgb((int) byte.MaxValue, 128, 0);
      this.lnkAction.AutoEllipsis = true;
      this.lnkAction.BackColor = Color.Transparent;
      this.lnkAction.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lnkAction.ForeColor = Color.FromArgb(29, 110, 174);
      this.lnkAction.LinkBehavior = LinkBehavior.HoverUnderline;
      this.lnkAction.LinkColor = Color.FromArgb(29, 110, 174);
      this.lnkAction.Location = new Point(18, 46);
      this.lnkAction.Name = "lnkAction";
      this.lnkAction.Size = new Size(290, 14);
      this.lnkAction.TabIndex = 9;
      this.lnkAction.TabStop = true;
      this.lnkAction.Text = "Action Message";
      this.lnkAction.UseMnemonic = false;
      this.lnkAction.VisitedLinkColor = Color.FromArgb(29, 110, 174);
      this.lnkAction.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkAction_LinkClicked);
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(8, 51);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(7, 5);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Controls.Add((Control) this.lblFrom);
      this.panel2.Controls.Add((Control) this.lblFromLabel);
      this.panel2.Location = new Point(5, 22);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(306, 17);
      this.panel2.TabIndex = 7;
      this.lblFrom.AutoSize = true;
      this.lblFrom.BackColor = Color.Transparent;
      this.lblFrom.Location = new Point(34, 2);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new Size(80, 14);
      this.lblFrom.TabIndex = 5;
      this.lblFrom.Text = "Sender's Name";
      this.lblFromLabel.AutoSize = true;
      this.lblFromLabel.BackColor = Color.Transparent;
      this.lblFromLabel.Location = new Point(3, 2);
      this.lblFromLabel.Name = "lblFromLabel";
      this.lblFromLabel.Size = new Size(34, 14);
      this.lblFromLabel.TabIndex = 4;
      this.lblFromLabel.Text = "From:";
      this.formFader.DisplayDuration = 5000;
      this.formFader.OpacityIncrement = 8;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(320, 97);
      this.ControlBox = false;
      this.Controls.Add((Control) this.grpMain);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NotificationForm);
      this.Opacity = 0.0;
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = nameof (NotificationForm);
      this.TopMost = true;
      this.grpMain.ResumeLayout(false);
      ((ISupportInitialize) this.btnClose).EndInit();
      this.pnlBody.ResumeLayout(false);
      this.pnlBody.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
