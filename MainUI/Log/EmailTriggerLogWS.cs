// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.EmailTriggerLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class EmailTriggerLogWS : UserControl
  {
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnClose;
    private Label lblSender;
    private Label lblDTTM;
    private Label label2;
    private Label label1;
    private GroupContainer groupContainer3;
    private TextBox txtBody;
    private Label label5;
    private Label lblSubject;
    private Label label4;
    private Label lblRecipients;

    public EmailTriggerLogWS(EmailTriggerLog log)
    {
      this.InitializeComponent();
      List<string> stringList = new List<string>();
      stringList.Add(log.SenderUserID);
      stringList.AddRange((IEnumerable<string>) log.RecipientUserIDs);
      Hashtable users = Session.OrganizationManager.GetUsers(stringList.ToArray(), true);
      this.lblDTTM.Text = log.Date.ToString("M/d/yyyy h:mm tt");
      this.lblSubject.Text = log.Subject;
      UserInfoSummary userInfoSummary1 = (UserInfoSummary) users[(object) log.SenderUserID];
      if (userInfoSummary1 != (UserInfoSummary) null)
        this.lblSender.Text = userInfoSummary1.FullName + " (" + userInfoSummary1.UserID + ")";
      else
        this.lblSender.Text = log.SenderUserID;
      stringList.Clear();
      foreach (string recipientUserId in log.RecipientUserIDs)
      {
        UserInfoSummary userInfoSummary2 = (UserInfoSummary) users[(object) recipientUserId];
        if (userInfoSummary2 != (UserInfoSummary) null)
          stringList.Add(userInfoSummary2.FullName + " (" + userInfoSummary2.UserID + ")");
        else
          stringList.Add(recipientUserId);
      }
      this.lblRecipients.Text = string.Join("; ", stringList.ToArray());
      this.txtBody.Text = log.Body.Replace("\n", Environment.NewLine);
      this.Dock = DockStyle.Fill;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.btnClose = new Button();
      this.lblSender = new Label();
      this.lblDTTM = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.groupContainer3 = new GroupContainer();
      this.txtBody = new TextBox();
      this.lblSubject = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.lblRecipients = new Label();
      this.groupContainer1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.lblRecipients);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.lblSubject);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.btnClose);
      this.groupContainer1.Controls.Add((Control) this.lblSender);
      this.groupContainer1.Controls.Add((Control) this.lblDTTM);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(407, 142);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Automated Email Details";
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.Location = new Point(353, 2);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(47, 22);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.lblSender.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSender.AutoEllipsis = true;
      this.lblSender.Location = new Point(121, 86);
      this.lblSender.Name = "lblSender";
      this.lblSender.Size = new Size(275, 14);
      this.lblSender.TabIndex = 3;
      this.lblDTTM.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDTTM.AutoEllipsis = true;
      this.lblDTTM.Location = new Point(121, 41);
      this.lblDTTM.Name = "lblDTTM";
      this.lblDTTM.Size = new Size(275, 14);
      this.lblDTTM.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 86);
      this.label2.Name = "label2";
      this.label2.Size = new Size(47, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Sent by:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 41);
      this.label1.Name = "label1";
      this.label1.Size = new Size(103, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Date and Time Sent:";
      this.groupContainer3.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer3.Controls.Add((Control) this.txtBody);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.Location = new Point(0, 142);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(407, 253);
      this.groupContainer3.TabIndex = 3;
      this.groupContainer3.Text = "Email Body";
      this.txtBody.BackColor = Color.White;
      this.txtBody.BorderStyle = BorderStyle.None;
      this.txtBody.Dock = DockStyle.Fill;
      this.txtBody.Location = new Point(1, 25);
      this.txtBody.Multiline = true;
      this.txtBody.Name = "txtBody";
      this.txtBody.ReadOnly = true;
      this.txtBody.ScrollBars = ScrollBars.Both;
      this.txtBody.Size = new Size(405, 227);
      this.txtBody.TabIndex = 0;
      this.lblSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubject.AutoEllipsis = true;
      this.lblSubject.Location = new Point(121, 64);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(275, 14);
      this.lblSubject.TabIndex = 6;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 64);
      this.label4.Name = "label4";
      this.label4.Size = new Size(46, 14);
      this.label4.TabIndex = 5;
      this.label4.Text = "Subject:";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 109);
      this.label5.Name = "label5";
      this.label5.Size = new Size(44, 14);
      this.label5.TabIndex = 7;
      this.label5.Text = "Sent to:";
      this.lblRecipients.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblRecipients.AutoEllipsis = true;
      this.lblRecipients.Location = new Point(121, 109);
      this.lblRecipients.Name = "lblRecipients";
      this.lblRecipients.Size = new Size(275, 14);
      this.lblRecipients.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer3);
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EmailTriggerLogWS);
      this.Size = new Size(407, 395);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
