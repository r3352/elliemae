// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.WelcomeEmail
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class WelcomeEmail : Form
  {
    private DateTime date;
    private UserInfo user;
    private string emailID;
    private TriggerEmailTemplate template;
    private ExternalUserInfo external;
    private ExternalOriginatorManagementData extOrg;
    private OrgInfo intOrg;
    private Sessions.Session session;
    private string newPassword;
    private IContainer components;
    private Label lblSentDate;
    private Label lblSentBy;
    private Label lblWelcomeEmail;
    private TextBox txtSendDate;
    private TextBox txtSentBy;
    private RichTextBox rtxtWelcomeEmail;
    private Button btnSend;
    private Button btnCancel;

    public event EventHandler SendEmail;

    public WelcomeEmail(
      Sessions.Session session,
      ExternalUserInfo external,
      UserInfo user,
      string emailID,
      DateTime welcomeEmailDate,
      string welcomeEmailUserName,
      string newPassword)
    {
      this.InitializeComponent();
      this.session = session;
      this.user = user;
      this.emailID = emailID;
      this.external = external;
      this.newPassword = newPassword;
      this.extOrg = this.session.ConfigurationManager.GetExternalOrganization(false, external.ExternalOrgID);
      this.intOrg = this.session.OrganizationManager.GetOrganization(user.OrgId);
      if (welcomeEmailDate != DateTime.MinValue)
        this.txtSendDate.Text = welcomeEmailDate.ToString("G");
      if (welcomeEmailUserName != "")
        this.txtSentBy.Text = welcomeEmailUserName;
      this.rtxtWelcomeEmail.Text = this.emailTemplate();
      if (((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_SendWelcomeEmail))
        return;
      this.btnSend.Enabled = false;
    }

    public TriggerEmailTemplate getTemplate => this.template;

    public DateTime DateTime => this.date;

    public string Date => this.date.Date.ToString("dd/mm/yyyy");

    public string Time => this.date.TimeOfDay.ToString("hh:mm:ss");

    private string emailTemplate()
    {
      string subject = this.intOrg.OrgName + " - New User Account";
      string str1 = "Dear " + this.external.FirstName + ",\n\nA new ";
      string str2 = "Dear " + this.external.FirstName + ",<br><br>A new ";
      string str3 = str1 + this.intOrg.OrgName + " website account has been created for you. You may now manage your ";
      string str4 = str2 + this.intOrg.OrgName + " website account has been created for you. You may now manage your ";
      string str5 = str3 + this.intOrg.OrgName + " loans electronically through our online portal using the account information included below. \n\n";
      string str6 = str4 + this.intOrg.OrgName + " loans electronically through our online portal using the account information included below. <br><br>";
      string str7;
      string str8;
      if (this.extOrg.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        str7 = str5 + "Company: " + this.extOrg.OrganizationName + "\n";
        str8 = str6 + "<ul><li>Company: " + this.extOrg.OrganizationName + "</li>";
      }
      else
      {
        str7 = str5 + "Company: " + this.extOrg.HierarchyPath.Split('\\')[1] + "\n";
        str8 = str6 + "<ul><li>Company: " + this.extOrg.HierarchyPath.Split('\\')[1] + "</li>";
        if (this.extOrg.OrganizationType == ExternalOriginatorOrgType.Branch)
        {
          str7 = str7 + "Branch: " + this.extOrg.OrganizationName + "\n";
          str8 = str8 + "<li>Branch: " + this.extOrg.OrganizationName + "</li>";
        }
        else if (this.extOrg.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
        {
          str7 = str7 + "Company Extension: " + this.extOrg.OrganizationName + "\n";
          str8 = str8 + "<li>Company Extension: " + this.extOrg.OrganizationName + "</li>";
        }
        else if (this.extOrg.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
        {
          str7 = str7 + "Branch Extension: " + this.extOrg.OrganizationName + "\n";
          str8 = str8 + "<li>Branch Extension: " + this.extOrg.OrganizationName + "</li>";
        }
      }
      ExternalUserInfo externalUserInfo = (ExternalUserInfo) null;
      if (this.extOrg.Manager != "")
      {
        externalUserInfo = this.extOrg.Manager != "" ? this.session.ConfigurationManager.GetExternalUserInfo(this.extOrg.Manager) : (ExternalUserInfo) null;
        str7 = str7 + "Company Primary Contact: " + externalUserInfo.FirstName + " " + externalUserInfo.LastName;
        str8 = str8 + "<li>Company Primary Contact: " + externalUserInfo.FirstName + " " + externalUserInfo.LastName;
      }
      string str9;
      string str10;
      if ((UserInfo) externalUserInfo != (UserInfo) null)
      {
        str9 = str7 + " (" + externalUserInfo.Email + ")\n";
        str10 = str8 + " (" + externalUserInfo.Email + ")</li>";
      }
      else
      {
        str9 = str7 + "\n";
        str10 = str8 + "</li>";
      }
      UserInfo user = this.session.OrganizationManager.GetUser(this.external.SalesRepID);
      ExternalUserURL[] externalUserInfoUrLs = this.session.ConfigurationManager.GetExternalUserInfoURLs(this.external.ExternalUserID);
      string str11 = str9 + this.intOrg.OrgName + " Account Executive : " + user.FullName + " (" + user.Email + ")\n\n";
      string str12 = str10 + "<li>" + this.intOrg.OrgName + " Account Executive : " + user.FullName + " (" + user.Email + ")</li></ul><br>";
      string str13 = str11 + "Account Details: \n";
      string str14 = str12 + "Account Details: <br>";
      string str15 = str13 + "Email Address: " + this.external.EmailForLogin + "\n";
      string str16 = str14 + "<ul><li>Email Address: " + this.external.EmailForLogin + "</li>";
      string str17 = str15 + "Password: " + this.newPassword + "\n";
      string str18 = str16 + "<li>Password: " + this.newPassword + "</li></ul><br>";
      string str19 = str17 + "You will have access to the following site(s) with your login information: \n\n";
      string str20 = str18 + "You will have access to the following site(s) with your login information: <br><br>";
      foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs)
      {
        str19 = str19 + this.session.ConfigurationManager.GetUrlLink(externalUserUrl.URLID) + "\n\n";
        str20 = str20 + this.session.ConfigurationManager.GetUrlLink(externalUserUrl.URLID) + "<br><br>";
      }
      string str21 = str19 + "Thank you, \n" + this.intOrg.OrgName + "\n\n";
      string body = str20 + "Thank you,<br>" + this.intOrg.OrgName + "<br><br>";
      this.template = new TriggerEmailTemplate(subject, body, new List<string>()
      {
        this.emailID
      }.ToArray(), new int[0], true);
      return str21;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      this.txtSendDate.Text = DateTime.Now.ToString();
      this.txtSentBy.Text = this.user.FullName;
      this.date = DateTime.Now;
      this.DialogResult = DialogResult.OK;
      if (this.SendEmail == null)
        return;
      this.SendEmail((object) this.template, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WelcomeEmail));
      this.lblSentDate = new Label();
      this.lblSentBy = new Label();
      this.lblWelcomeEmail = new Label();
      this.txtSendDate = new TextBox();
      this.txtSentBy = new TextBox();
      this.rtxtWelcomeEmail = new RichTextBox();
      this.btnSend = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblSentDate.AutoSize = true;
      this.lblSentDate.Location = new Point(4, 20);
      this.lblSentDate.Name = "lblSentDate";
      this.lblSentDate.Size = new Size(55, 13);
      this.lblSentDate.TabIndex = 0;
      this.lblSentDate.Text = "Sent Date";
      this.lblSentBy.AutoSize = true;
      this.lblSentBy.Location = new Point(4, 43);
      this.lblSentBy.Name = "lblSentBy";
      this.lblSentBy.Size = new Size(44, 13);
      this.lblSentBy.TabIndex = 1;
      this.lblSentBy.Text = "Sent By";
      this.lblWelcomeEmail.AutoSize = true;
      this.lblWelcomeEmail.Location = new Point(4, 63);
      this.lblWelcomeEmail.Name = "lblWelcomeEmail";
      this.lblWelcomeEmail.Size = new Size(80, 13);
      this.lblWelcomeEmail.TabIndex = 2;
      this.lblWelcomeEmail.Text = "Welcome Email";
      this.txtSendDate.Location = new Point(90, 17);
      this.txtSendDate.Name = "txtSendDate";
      this.txtSendDate.ReadOnly = true;
      this.txtSendDate.Size = new Size(415, 20);
      this.txtSendDate.TabIndex = 3;
      this.txtSentBy.Location = new Point(90, 40);
      this.txtSentBy.Name = "txtSentBy";
      this.txtSentBy.ReadOnly = true;
      this.txtSentBy.Size = new Size(415, 20);
      this.txtSentBy.TabIndex = 4;
      this.rtxtWelcomeEmail.Location = new Point(90, 63);
      this.rtxtWelcomeEmail.Name = "rtxtWelcomeEmail";
      this.rtxtWelcomeEmail.ReadOnly = true;
      this.rtxtWelcomeEmail.Size = new Size(415, 180);
      this.rtxtWelcomeEmail.TabIndex = 5;
      this.rtxtWelcomeEmail.Text = "";
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Location = new Point(341, 249);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 23);
      this.btnSend.TabIndex = 6;
      this.btnSend.Text = "Send";
      this.btnSend.UseVisualStyleBackColor = true;
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(422, 249);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnSend;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(509, 280);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.rtxtWelcomeEmail);
      this.Controls.Add((Control) this.txtSentBy);
      this.Controls.Add((Control) this.txtSendDate);
      this.Controls.Add((Control) this.lblWelcomeEmail);
      this.Controls.Add((Control) this.lblSentBy);
      this.Controls.Add((Control) this.lblSentDate);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (WelcomeEmail);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Welcome Email";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
