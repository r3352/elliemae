// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.BatchEmail
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class BatchEmail : Form
  {
    private Sessions.Session session;
    private List<ExternalUserInfo> usersWithWelcomeEmail;
    private List<ExternalUserInfo> usersWithNoWelcomeEmail;
    private List<ExternalUserInfo> users;
    private HashSet<string> userSetWithUrl;
    private IContainer components;
    private GridView gridView1;
    private Button btn1;
    private Button btn2;
    private Button exporttBtn;
    private Label label1;
    private Label label2;

    public BatchEmail(Sessions.Session session, List<ExternalUserInfo> users)
    {
      this.session = session;
      this.InitializeComponent();
      this.users = users;
      this.hideElements();
      this.sendWelcomeEmail(users);
    }

    public BatchEmail(
      Sessions.Session session,
      List<ExternalUserInfo> usersWithWelcomeEmail,
      List<ExternalUserInfo> usersWithNoWelcomeEmail)
    {
      this.session = session;
      this.usersWithWelcomeEmail = usersWithWelcomeEmail;
      this.usersWithNoWelcomeEmail = usersWithNoWelcomeEmail;
      this.InitializeComponent();
      usersWithWelcomeEmail.ForEach((Action<ExternalUserInfo>) (x => this.gridView1.Items.Add(new GVItem(new string[4]
      {
        x.FirstName,
        x.LastName,
        TPOUtils.returnRoles(x.Roles),
        x.EmailForLogin
      })
      {
        Tag = (object) x
      })));
    }

    private void btn1_Click(object sender, EventArgs e)
    {
      this.sendWelcomeEmail(this.usersWithNoWelcomeEmail);
    }

    private void btn2_Click(object sender, EventArgs e)
    {
      this.usersWithNoWelcomeEmail.AddRange((IEnumerable<ExternalUserInfo>) this.usersWithWelcomeEmail);
      this.sendWelcomeEmail(this.usersWithNoWelcomeEmail);
    }

    public void sendWelcomeEmail(List<ExternalUserInfo> users)
    {
      this.users = users;
      HashSet<string> externalUserInfoUrLs = this.session.ConfigurationManager.GetMultipleExternalUserInfoURLs(string.Join("','", users.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (eu => eu.ExternalUserID)).ToArray<string>()));
      List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
      DialogResult dialogResult = DialogResult.None;
      foreach (ExternalUserInfo user in users)
      {
        if (!externalUserInfoUrLs.Contains(user.ExternalUserID))
          externalUserInfoList.Add(user);
      }
      if (externalUserInfoUrLs.Count > 0)
      {
        this.userSetWithUrl = externalUserInfoUrLs;
        dialogResult = new ProgressDialog("Send Welcome Email", new AsynchronousProcess(this.sendEmail), (object) null, false).ShowDialog((IWin32Window) this);
      }
      if (dialogResult != DialogResult.OK && dialogResult != DialogResult.None)
        return;
      if (externalUserInfoList.Any<ExternalUserInfo>())
      {
        this.showUsersWithNoUrls(externalUserInfoList, externalUserInfoUrLs);
      }
      else
      {
        this.setSmallFormSize();
        this.label2.Visible = false;
        this.gridView1.Visible = false;
        this.btn1.Visible = false;
        this.exporttBtn.Visible = false;
        this.btn2.Visible = true;
        this.btn2.Text = "Close";
        this.btn2.Click -= new EventHandler(this.btn2_Click);
        this.btn2.Click += new EventHandler(this.buttonCancel_Click);
        this.label1.Visible = true;
        this.label1.Image = (Image) Resources.check_mark_green;
        this.label1.ImageAlign = ContentAlignment.MiddleLeft;
        this.label1.Text = "       " + (object) externalUserInfoUrLs.Count + " Welcome email(s) were sent.";
      }
    }

    private DialogResult sendEmail(object state, IProgressFeedback feedback)
    {
      int num1 = 0;
      try
      {
        feedback.Status = "Send Welcome Email...";
        feedback.ResetCounter(this.userSetWithUrl.Count);
        feedback.Status = "Sending Welcome Emails...";
        foreach (string str in this.userSetWithUrl)
        {
          string userID = str;
          ExternalUserInfo externalUserInfo = this.users.First<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (e1 => userID.Equals(e1.ExternalUserID)));
          string newPassword = this.resetPassword(externalUserInfo);
          WelcomeEmail welcomeEmail = new WelcomeEmail(this.session, externalUserInfo, this.session.UserInfo, externalUserInfo.Email, new DateTime(), this.session.UserInfo.FullName, newPassword);
          TPOClientUtils.sendEmailTriggerTemplate(welcomeEmail.getTemplate, this.session);
          DateTime now = DateTime.Now;
          this.session.ConfigurationManager.SendWelcomeEmailUserInfo(externalUserInfo.ExternalUserID, welcomeEmail.DateTime, this.session.UserInfo.FullName);
          this.session.ConfigurationManager.ResetExternalUserInfoPassword(externalUserInfo.ExternalUserID, newPassword, now, true);
          ++num1;
          feedback.Increment(1);
          feedback.Details = "Completed " + (object) num1 + " of " + (object) this.userSetWithUrl.Count;
          if (feedback.Cancel)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Operation cancelled after sending welcome email to " + (object) num1 + " contact(s).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return DialogResult.Cancel;
          }
        }
        int num3 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Successfully send welcome email to " + (object) num1 + " contact(s).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "An error has occurred while sending welcome emails: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
    }

    private void setSmallFormSize()
    {
      this.Size = new Size(400, 150);
      this.label1.Location = new Point(90, 35);
      this.btn2.Location = new Point(270, 70);
    }

    private void setOriginalFormSize()
    {
      this.Size = new Size(658, 557);
      this.btn2.Location = new Point(555, 484);
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private string resetPassword(ExternalUserInfo externalUserInfo)
    {
      return externalUserInfo.GenerateNewPassword();
    }

    private void hideElements()
    {
      this.setSmallFormSize();
      this.gridView1.Visible = false;
      this.exporttBtn.Visible = false;
      this.btn1.Visible = false;
      this.btn2.Visible = false;
      this.label1.Visible = false;
      this.label2.Visible = false;
    }

    private void showUsersWithNoUrls(
      List<ExternalUserInfo> usersWithNoUrls,
      HashSet<string> userWithUrls)
    {
      this.setOriginalFormSize();
      this.gridView1.Visible = true;
      this.gridView1.Items.Clear();
      foreach (ExternalUserInfo usersWithNoUrl in usersWithNoUrls)
        this.gridView1.Items.Add(new GVItem(new string[4]
        {
          usersWithNoUrl.FirstName,
          usersWithNoUrl.LastName,
          TPOUtils.returnRoles(usersWithNoUrl.Roles),
          usersWithNoUrl.EmailForLogin
        })
        {
          Tag = (object) usersWithNoUrl
        });
      this.gridView1.Refresh();
      this.btn1.Visible = false;
      this.exporttBtn.Visible = true;
      this.btn2.Visible = true;
      this.btn2.Text = "Close";
      this.btn2.Click -= new EventHandler(this.btn2_Click);
      this.btn2.Click += new EventHandler(this.buttonCancel_Click);
      this.label1.Visible = true;
      this.label1.Text = "       " + (object) userWithUrls.Count + " Welcome email(s) were sent.";
      this.label1.Image = (Image) Resources.check_mark_green;
      this.label1.ImageAlign = ContentAlignment.MiddleLeft;
      this.label2.Visible = true;
      this.label2.Text = "Note: The following TPO contacts did not receive a Welcome Email \nbecause there is no TPO WebCenter site URLs assigned to them.";
    }

    private void exporttBtn_Click(object sender, EventArgs e)
    {
      ExcelHandler excelExport = new ExcelHandler();
      excelExport.Headers = new string[4]
      {
        "First Name",
        "Last Name",
        "Role(s)",
        "Login Email"
      };
      this.gridView1.Items.ToList<GVItem>().ForEach((Action<GVItem>) (x =>
      {
        List<string> stringList = new List<string>();
        ExternalUserInfo tag = (ExternalUserInfo) x.Tag;
        stringList.Add(tag.FirstName);
        stringList.Add(tag.LastName);
        stringList.Add(TPOUtils.returnRoles(tag.Roles));
        stringList.Add(tag.EmailForLogin);
        excelExport.AddDataRow(stringList.ToArray());
      }));
      excelExport.Export();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.btn1 = new Button();
      this.btn2 = new Button();
      this.label2 = new Label();
      this.gridView1 = new GridView();
      this.label1 = new Label();
      this.exporttBtn = new Button();
      this.SuspendLayout();
      this.btn1.Location = new Point(444, 461);
      this.btn1.Name = "btn1";
      this.btn1.Size = new Size(95, 23);
      this.btn1.TabIndex = 3;
      this.btn1.Text = "Don't Include";
      this.btn1.UseVisualStyleBackColor = true;
      this.btn1.Click += new EventHandler(this.btn1_Click);
      this.btn2.Location = new Point(555, 461);
      this.btn2.Name = "btn2";
      this.btn2.Size = new Size(75, 23);
      this.btn2.TabIndex = 4;
      this.btn2.Text = "Include";
      this.btn2.UseVisualStyleBackColor = true;
      this.btn2.Click += new EventHandler(this.btn2_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(9, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(452, 26);
      this.label2.TabIndex = 7;
      this.label2.Text = "Note: Sending them another Welcome Email will result in their password being \r\nreset and included in their Welcome Email.";
      this.gridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "First Name";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Role(s)";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Login Email";
      gvColumn4.Width = 150;
      this.gridView1.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(12, 101);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(618, 333);
      this.gridView1.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(350, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "The following TPO contacts have been previously sent Welcome Emails.";
      this.exporttBtn.Image = (Image) Resources.excel;
      this.exporttBtn.ImageAlign = ContentAlignment.MiddleLeft;
      this.exporttBtn.Location = new Point(555, 62);
      this.exporttBtn.Name = "exporttBtn";
      this.exporttBtn.Size = new Size(75, 23);
      this.exporttBtn.TabIndex = 5;
      this.exporttBtn.Text = "Export";
      this.exporttBtn.UseVisualStyleBackColor = true;
      this.exporttBtn.Click += new EventHandler(this.exporttBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(642, 505);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.exporttBtn);
      this.Controls.Add((Control) this.btn2);
      this.Controls.Add((Control) this.btn1);
      this.Controls.Add((Control) this.gridView1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BatchEmail);
      this.ShowIcon = false;
      this.Text = "Send Welcome Email";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
