// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddLenderInvestorContactForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddLenderInvestorContactForm : Form
  {
    private ExternalOrgLenderContact lenderContact;
    private int displayOrder = -1;
    private UserInfo[] allInternalUsers;
    private IOrganizationManager rOrg;
    private Sessions.Session session;
    private AddLenderInvestorContactForm.NameComboBoxItem selectedItem;
    private int? OrgId;
    private string SourceTabText = string.Empty;
    private bool inInitialPageValue;
    private IContainer components;
    private Label label1;
    private CheckBox chkBoxWholesale;
    private CheckBox chkBoxNonDelegated;
    private CheckBox chkBoxDelegated;
    private Label lblName;
    private ComboBox cBoxName;
    private Label label2;
    private Label label3;
    private TextBox txtTitle;
    private Label label4;
    private TextBox txtPhone;
    private Label label5;
    private TextBox txtEmail;
    private Button btnOk;
    private Button btnCancel;
    private Label label6;
    private CheckBox chkBoxHide;

    public AddLenderInvestorContactForm() => this.InitializeComponent();

    public AddLenderInvestorContactForm(
      Sessions.Session session,
      int displayOrder,
      int? companyOrgId,
      string sourceTabText)
    {
      this.InitializeComponent();
      this.session = session;
      this.lenderContact = (ExternalOrgLenderContact) null;
      this.displayOrder = displayOrder;
      this.SourceTabText = sourceTabText;
      this.OrgId = companyOrgId;
      this.loadUsers();
      this.initialPageValue();
      if (this.lenderContact != null)
        this.Text = "Edit Lender/Investor Contact";
      if (this.OrgId.HasValue)
        return;
      this.label6.Hide();
      this.chkBoxHide.Hide();
    }

    public AddLenderInvestorContactForm(
      Sessions.Session session,
      ExternalOrgLenderContact lenderContact,
      string sourceTabText)
    {
      this.InitializeComponent();
      this.session = session;
      this.lenderContact = lenderContact;
      this.SourceTabText = sourceTabText;
      this.loadUsers();
      this.initialPageValue();
      this.OrgId = lenderContact.ExternalOrgID;
      if (!this.OrgId.HasValue)
      {
        this.label6.Hide();
        this.chkBoxHide.Hide();
      }
      if (this.lenderContact == null)
        return;
      this.Text = "Edit Lender/Investor Contact";
    }

    private void initialPageValue()
    {
      if (this.inInitialPageValue || this.lenderContact == null)
        return;
      this.inInitialPageValue = true;
      try
      {
        if (!string.IsNullOrWhiteSpace(this.lenderContact.UserID))
        {
          foreach (object obj in this.cBoxName.Items)
          {
            if (!(obj is AddLenderInvestorContactForm.NameComboBoxItem nameComboBoxItem))
              return;
            if (nameComboBoxItem.UserInfo.Userid == this.lenderContact.UserID)
            {
              this.cBoxName.SelectedItem = obj;
              this.selectedItem = nameComboBoxItem;
              break;
            }
          }
        }
        this.cBoxName.Text = this.lenderContact.Name;
        this.chkBoxWholesale.Checked = this.lenderContact.isWholesaleChannelEnabled;
        this.chkBoxDelegated.Checked = this.lenderContact.isDelegatedChannelEnabled;
        this.chkBoxNonDelegated.Checked = this.lenderContact.isNonDelegatedChannelEnabled;
        this.txtTitle.Text = this.lenderContact.Title;
        this.txtPhone.Text = this.lenderContact.Phone;
        this.txtEmail.Text = this.lenderContact.Email;
        this.chkBoxHide.Checked = this.lenderContact.isHidden;
      }
      finally
      {
        this.inInitialPageValue = false;
        this.EnableOkBtn();
      }
    }

    private void loadUsers()
    {
      if (this.rOrg == null && this.session != null)
        this.rOrg = this.session.OrganizationManager;
      if (this.allInternalUsers == null && this.rOrg != null)
        this.allInternalUsers = this.rOrg.GetAllUsers();
      if (this.allInternalUsers == null || ((IEnumerable<UserInfo>) this.allInternalUsers).Count<UserInfo>() <= 0)
        return;
      foreach (UserInfo allInternalUser in this.allInternalUsers)
      {
        if (allInternalUser.Status == UserInfo.UserStatusEnum.Enabled)
          this.cBoxName.Items.Add((object) new AddLenderInvestorContactForm.NameComboBoxItem(allInternalUser));
      }
    }

    private void OnSetcBoxNameText(string value) => this.cBoxName.Text = value;

    private void cBoxName_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cBoxName.SelectedItem is AddLenderInvestorContactForm.NameComboBoxItem selectedItem))
        return;
      UserInfo userInfo = selectedItem.UserInfo;
      if (userInfo != (UserInfo) null)
      {
        this.txtPhone.Text = userInfo.Phone;
        this.txtPhone.Enabled = false;
        this.txtEmail.Text = userInfo.Email;
        this.txtEmail.Enabled = false;
        this.txtTitle.Text = string.IsNullOrWhiteSpace(userInfo.JobTitle) ? string.Empty : userInfo.JobTitle;
        if (this.txtTitle.Text.Length > 30)
          this.txtTitle.Text = this.txtTitle.Text.Substring(0, 30);
        this.txtTitle.Enabled = false;
        if (!this.validateUser())
        {
          this.clearControls();
          this.cBoxName.SelectedIndex = -1;
        }
        else
        {
          this.selectedItem = selectedItem;
          if (this.Handle != IntPtr.Zero)
            this.BeginInvoke((Delegate) new AddLenderInvestorContactForm.SetcBoxNameTextDelegate(this.OnSetcBoxNameText), (object) selectedItem.UserInfo.firstLastName);
          else
            this.cBoxName.Text = selectedItem.UserInfo.firstLastName;
          this.EnableOkBtn();
        }
      }
      else
        this.clearControls();
    }

    private void cBoxName_TextChanged(object sender, EventArgs e)
    {
      if (this.inInitialPageValue)
        return;
      AddLenderInvestorContactForm.NameComboBoxItem selectedItem = this.cBoxName.SelectedItem as AddLenderInvestorContactForm.NameComboBoxItem;
      string source = this.cBoxName.Text;
      if (source.Contains<char>('('))
        source = source.Split('(')[0].Trim();
      if (this.selectedItem != null)
      {
        if (this.selectedItem.UserInfo.firstLastName.Equals(source))
          return;
        this.clearControls();
      }
      else
        this.clearControls();
    }

    private void clearControls()
    {
      this.txtTitle.Text = "";
      this.txtTitle.Enabled = true;
      this.txtPhone.Text = "";
      this.txtPhone.Enabled = true;
      this.txtEmail.Text = "";
      this.txtEmail.Enabled = true;
      this.selectedItem = (AddLenderInvestorContactForm.NameComboBoxItem) null;
      this.btnOk.Enabled = false;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      string name = (string) null;
      string userID = (string) null;
      string str = (string) null;
      if (this.selectedItem != null)
      {
        UserInfo userInfo = this.selectedItem.UserInfo;
        if (userInfo != (UserInfo) null)
        {
          userID = userInfo.Userid;
          name = str;
        }
      }
      else if (!string.IsNullOrEmpty(this.cBoxName.Text))
        name = this.cBoxName.Text.Trim();
      if (this.lenderContact == null)
      {
        ExternalOrgLenderContact newContact = new ExternalOrgLenderContact(userID, this.OrgId, this.OrgId.HasValue ? -1 : this.displayOrder, name, this.txtTitle.Text, this.txtEmail.Text, this.txtPhone.Text, this.chkBoxWholesale.Checked, this.chkBoxDelegated.Checked, this.chkBoxNonDelegated.Checked, hidden: this.chkBoxHide.Checked);
        int num = this.session.ConfigurationManager.AddLenderContact(newContact);
        this.lenderContact = newContact;
        this.lenderContact.ContactID = num;
        if (this.OrgId.HasValue)
          this.session.ConfigurationManager.AddTPOCompanyLenderContact(this.OrgId.Value, this.lenderContact.ContactID, this.lenderContact.Source, this.lenderContact.isHidden ? 1 : 0, this.displayOrder);
      }
      else
      {
        this.lenderContact.isWholesaleChannelEnabled = this.chkBoxWholesale.Checked;
        this.lenderContact.isDelegatedChannelEnabled = this.chkBoxDelegated.Checked;
        this.lenderContact.isNonDelegatedChannelEnabled = this.chkBoxNonDelegated.Checked;
        this.lenderContact.Name = name;
        this.lenderContact.UserID = userID;
        this.lenderContact.ExternalOrgID = this.OrgId;
        this.lenderContact.Title = this.txtTitle.Text;
        this.lenderContact.Phone = this.txtPhone.Text;
        this.lenderContact.Email = this.txtEmail.Text;
        this.lenderContact.isHidden = this.chkBoxHide.Checked;
        this.session.ConfigurationManager.UpdateLenderContact(this.lenderContact);
        if (this.OrgId.HasValue)
          this.session.ConfigurationManager.UpdateTPOCompanyLenderContact(this.OrgId.Value, this.lenderContact.ContactID, this.lenderContact.Source, this.lenderContact.isHidden ? 1 : 0, this.lenderContact.DisplayOrder);
      }
      if (this.OrgId.HasValue)
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.OrgId.Value);
      this.DialogResult = DialogResult.OK;
    }

    private bool validateUser()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.selectedItem == null && string.IsNullOrEmpty(this.cBoxName.Text.Trim()))
        flag1 = true;
      if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()))
        flag2 = true;
      if (string.IsNullOrEmpty(this.txtPhone.Text.Trim()) && (string.IsNullOrEmpty(this.txtEmail.Text.Trim()) || !Utils.ValidateEmail(this.txtEmail.Text.Trim())))
        flag3 = true;
      if (!(flag1 | flag2 | flag3))
        return true;
      string text = "The selected User Contact record must first be updated before it can be used in this Key Lender Contacts feature.";
      if (flag1)
        text += " 'Name' must be entered in the First and Last Name field(s) of the selected User Contact record";
      if (flag2)
        text += " 'Job Title' must be entered in the Job Title field of the selected User Contact record";
      if (flag3)
        text += " Email and/or Phone must be entered in the selected User Contact record";
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void EnableOkBtn()
    {
      if (this.inInitialPageValue)
        return;
      if (this.selectedItem != null && string.IsNullOrWhiteSpace(this.cBoxName.Text.Trim()))
      {
        this.btnOk.Enabled = true;
      }
      else
      {
        if (string.IsNullOrWhiteSpace(this.cBoxName.Text.Trim()))
        {
          this.btnOk.Enabled = false;
          return;
        }
        if (string.IsNullOrWhiteSpace(this.txtEmail.Text.Trim()) && string.IsNullOrWhiteSpace(this.txtPhone.Text.Trim()))
        {
          this.btnOk.Enabled = false;
          return;
        }
        if (string.IsNullOrWhiteSpace(this.txtTitle.Text.Trim()))
        {
          this.btnOk.Enabled = false;
          return;
        }
        if (!string.IsNullOrWhiteSpace(this.txtEmail.Text.Trim()) && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
        {
          this.btnOk.Enabled = false;
          return;
        }
      }
      this.btnOk.Enabled = true;
    }

    private void TextEntryChanged(object sender, EventArgs e) => this.EnableOkBtn();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddLenderInvestorContactForm));
      this.label1 = new Label();
      this.chkBoxWholesale = new CheckBox();
      this.chkBoxNonDelegated = new CheckBox();
      this.chkBoxDelegated = new CheckBox();
      this.lblName = new Label();
      this.cBoxName = new ComboBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtTitle = new TextBox();
      this.label4 = new Label();
      this.txtPhone = new TextBox();
      this.label5 = new Label();
      this.txtEmail = new TextBox();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.label6 = new Label();
      this.chkBoxHide = new CheckBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(24, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Channel:";
      this.chkBoxWholesale.AutoSize = true;
      this.chkBoxWholesale.Location = new Point(88, 19);
      this.chkBoxWholesale.Name = "chkBoxWholesale";
      this.chkBoxWholesale.Size = new Size(76, 17);
      this.chkBoxWholesale.TabIndex = 1;
      this.chkBoxWholesale.Text = "Wholesale";
      this.chkBoxWholesale.UseVisualStyleBackColor = true;
      this.chkBoxNonDelegated.AutoSize = true;
      this.chkBoxNonDelegated.Location = new Point(178, 19);
      this.chkBoxNonDelegated.Name = "chkBoxNonDelegated";
      this.chkBoxNonDelegated.Size = new Size(98, 17);
      this.chkBoxNonDelegated.TabIndex = 2;
      this.chkBoxNonDelegated.Text = "Non-Delegated";
      this.chkBoxNonDelegated.UseVisualStyleBackColor = true;
      this.chkBoxDelegated.AutoSize = true;
      this.chkBoxDelegated.Location = new Point(294, 19);
      this.chkBoxDelegated.Name = "chkBoxDelegated";
      this.chkBoxDelegated.Size = new Size(75, 17);
      this.chkBoxDelegated.TabIndex = 3;
      this.chkBoxDelegated.Text = "Delegated";
      this.chkBoxDelegated.UseVisualStyleBackColor = true;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(24, 60);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 4;
      this.lblName.Text = "Name";
      this.cBoxName.DropDownHeight = 66;
      this.cBoxName.FormattingEnabled = true;
      this.cBoxName.IntegralHeight = false;
      this.cBoxName.Location = new Point(130, 54);
      this.cBoxName.MaxDropDownItems = 4;
      this.cBoxName.MaxLength = 30;
      this.cBoxName.Name = "cBoxName";
      this.cBoxName.Size = new Size(182, 21);
      this.cBoxName.Sorted = true;
      this.cBoxName.TabIndex = 5;
      this.cBoxName.SelectedIndexChanged += new EventHandler(this.cBoxName_SelectedIndexChanged);
      this.cBoxName.TextChanged += new EventHandler(this.cBoxName_TextChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(24, 93);
      this.label2.Name = "label2";
      this.label2.Size = new Size(93, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Title / Department";
      this.label3.AutoSize = true;
      this.label3.ForeColor = Color.Red;
      this.label3.Location = new Point(113, 95);
      this.label3.Name = "label3";
      this.label3.Size = new Size(11, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "*";
      this.txtTitle.Location = new Point(130, 90);
      this.txtTitle.MaxLength = 30;
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.Size = new Size(182, 20);
      this.txtTitle.TabIndex = 8;
      this.txtTitle.TextChanged += new EventHandler(this.TextEntryChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(24, 130);
      this.label4.Name = "label4";
      this.label4.Size = new Size(38, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Phone";
      this.txtPhone.Location = new Point(130, 125);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(182, 20);
      this.txtPhone.TabIndex = 10;
      this.txtPhone.TextChanged += new EventHandler(this.TextEntryChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(24, 168);
      this.label5.Name = "label5";
      this.label5.Size = new Size(32, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Email";
      this.txtEmail.Location = new Point(130, 160);
      this.txtEmail.MaxLength = 40;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(182, 20);
      this.txtEmail.TabIndex = 12;
      this.txtEmail.TextChanged += new EventHandler(this.TextEntryChanged);
      this.btnOk.Enabled = false;
      this.btnOk.Location = new Point(217, 224);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 13;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(312, 224);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(24, 198);
      this.label6.Name = "label6";
      this.label6.Size = new Size(29, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Hide";
      this.chkBoxHide.AutoSize = true;
      this.chkBoxHide.Location = new Point(130, 198);
      this.chkBoxHide.Name = "chkBoxHide";
      this.chkBoxHide.Size = new Size(15, 14);
      this.chkBoxHide.TabIndex = 16;
      this.chkBoxHide.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(410, 259);
      this.Controls.Add((Control) this.chkBoxHide);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtPhone);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtTitle);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cBoxName);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.chkBoxDelegated);
      this.Controls.Add((Control) this.chkBoxNonDelegated);
      this.Controls.Add((Control) this.chkBoxWholesale);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddLenderInvestorContactForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = " Add Lender/Investor Contact";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class NameComboBoxItem
    {
      public UserInfo UserInfo;

      public NameComboBoxItem(UserInfo userInfo) => this.UserInfo = userInfo;

      public override string ToString()
      {
        return string.Format("{0} ({1})", (object) this.UserInfo.firstLastName, (object) this.UserInfo.Email);
      }
    }

    private delegate void SetcBoxNameTextDelegate(string value);
  }
}
