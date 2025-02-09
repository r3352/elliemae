// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ePass.ePassCredentialDetail
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup.ePass
{
  public class ePassCredentialDetail : Form
  {
    private bool suspendEvent;
    private HttpWebRequest request;
    private WebResponse response;
    private Dictionary<string, string> PartnerMapping = new Dictionary<string, string>();
    private ePassCredentialSetting existingSetting;
    private string[] currentUserList = new string[0];
    private string partnerSection = "";
    private string saveLoginFieldName = "";
    private string saveLoginValue = "";
    private string encryptionType = "";
    private bool passwordModified;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label2;
    private Label lblAuth2;
    private TextBox txtAuth2;
    private Label lblAuth1;
    private Label label1;
    private TextBox txtAuth1;
    private ComboBox cbCategory;
    private TextBox txtPWD;
    private ComboBox comBoxProviders;
    private TextBox txtUID;
    private Label lblUserID;
    private Label lblPwd;
    private TextBox txtDescription;
    private Label label3;
    private Label label5;
    private StandardIconButton btnSearchUser;
    private TextBox txtUserCount;
    private Label label4;
    private CheckBox chkEmailNotification;
    private Label label7;
    private Label label6;
    private TextBox txtExpirationDays;
    private Panel pnlDynamic;
    private Panel pnlAuth1;
    private Panel pnlAuth2;
    private Panel pnlEtc;
    private Button btnSave;
    private Button btnCancel;
    private Panel pnlUID;
    private Panel pnlPWD;
    private EMHelpLink emHelpLink1;
    private Panel pnlTPO;
    private Label lblTPONumber;
    private TextBox txtTPONumber;

    public ePassCredentialDetail(ePassCredentialSetting existingSetting)
    {
      this.existingSetting = existingSetting;
      this.InitializeComponent();
      this.initialPageValue();
      if (this.isNew)
        return;
      this.initialPageValueForExistingRecord();
    }

    public ePassCredentialDetail()
      : this((ePassCredentialSetting) null)
    {
    }

    private bool isNew => this.existingSetting == null;

    private void initialPageValueForExistingRecord()
    {
      this.cbCategory.SelectedItem = (object) this.existingSetting.Category;
      this.cbCategory_SelectedIndexChanged((object) null, (EventArgs) null);
      this.cbCategory.Enabled = false;
      this.comBoxProviders.SelectedItem = (object) this.existingSetting.Title;
      this.comBoxProviders_SelectedIndexChanged((object) null, (EventArgs) null);
      this.comBoxProviders.Enabled = true;
      if (this.comBoxProviders.SelectedIndex == 0)
      {
        if (this.existingSetting.UIDName != "" || this.existingSetting.PasswordName != "" || this.existingSetting.Auth1Name != "" || this.existingSetting.Auth2Name != "")
          this.pnlEtc.Visible = true;
        else
          this.pnlEtc.Visible = false;
        if (this.existingSetting.Auth2Name != "")
        {
          this.pnlAuth2.Visible = true;
          this.lblAuth2.Text = this.existingSetting.Auth2Name;
          this.lblAuth2.Tag = (object) this.existingSetting.Auth2FieldName;
        }
        else
          this.pnlAuth2.Visible = false;
        if (this.existingSetting.Auth1Name != "")
        {
          this.lblAuth1.Text = this.existingSetting.Auth1Name;
          this.pnlAuth1.Visible = true;
          this.lblAuth1.Tag = (object) this.existingSetting.Auth1FieldName;
        }
        else
          this.pnlAuth1.Visible = false;
        if (this.existingSetting.PasswordName != "")
        {
          this.lblPwd.Text = this.existingSetting.PasswordName;
          this.lblPwd.Tag = (object) this.existingSetting.PasswordFieldName;
          this.pnlPWD.Visible = true;
        }
        else
          this.pnlPWD.Visible = false;
        if (this.existingSetting.UIDName != "")
        {
          this.lblUserID.Text = this.existingSetting.UIDName;
          this.lblUserID.Tag = (object) this.existingSetting.UIDFieldName;
          this.pnlUID.Visible = true;
        }
        else
          this.pnlUID.Visible = false;
        if (this.existingSetting.TPOName != "")
        {
          this.lblTPONumber.Text = this.existingSetting.TPOName;
          this.lblTPONumber.Tag = (object) this.existingSetting.TPOFieldName;
          this.pnlTPO.Visible = true;
        }
        else
          this.pnlTPO.Visible = false;
      }
      this.suspendEvent = true;
      this.txtDescription.Text = this.existingSetting.Description;
      this.txtUID.Text = this.existingSetting.UIDValue;
      this.txtPWD.Text = this.existingSetting.PasswordValue;
      this.txtAuth1.Text = this.existingSetting.Auth1Value;
      this.txtAuth2.Text = this.existingSetting.Auth2Value;
      this.txtTPONumber.Text = this.existingSetting.TPOFieldValue;
      if (this.existingSetting.ValidDuration > 0)
      {
        this.chkEmailNotification.Checked = true;
        this.txtExpirationDays.ReadOnly = false;
        this.txtExpirationDays.Text = string.Concat((object) this.existingSetting.ValidDuration);
      }
      else
      {
        this.chkEmailNotification.Checked = false;
        this.txtExpirationDays.Text = "";
        this.txtExpirationDays.ReadOnly = true;
      }
      this.currentUserList = Session.ConfigurationManager.GetUserIDListByePassCredentialID(this.existingSetting.CredentialID).ToArray();
      this.txtUserCount.Text = string.Concat((object) Session.ConfigurationManager.GetUserIDListByePassCredentialID(this.existingSetting.CredentialID).Count);
      this.suspendEvent = false;
    }

    private void initialPageValue()
    {
      this.suspendEvent = true;
      this.comBoxProviders.Items.Clear();
      this.cbCategory.Items.Clear();
      this.cbCategory.Items.Add((object) "Select a service category");
      this.request = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassai/getallservices.asp");
      try
      {
        this.response = this.request.GetResponse();
        Stream responseStream = this.response.GetResponseStream();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(responseStream);
        foreach (XmlNode selectNode in xmlDocument.SelectNodes("SERVICES/S/@Title"))
          this.cbCategory.Items.Add((object) selectNode.InnerText);
      }
      catch
      {
        this.response.Close();
      }
      this.comBoxProviders.Items.Add((object) "Select from My Providers list");
      this.cbCategory.SelectedIndex = 0;
      this.comBoxProviders.SelectedIndex = 0;
      this.suspendEvent = false;
    }

    public bool RequirePasswordEncryption
    {
      get => ePassCredentialSetting.RequirePasswordEncryption(this.encryptionType);
    }

    private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.comBoxProviders.Items.Clear();
      this.comBoxProviders.Items.Add((object) "Select from My Providers list");
      if (this.cbCategory.SelectedIndex == 0)
      {
        this.comBoxProviders.Enabled = false;
        this.comBoxProviders.SelectedIndex = 0;
      }
      else
      {
        this.request = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassai/GetAllManagedPartners.asp");
        byte[] bytes = new ASCIIEncoding().GetBytes("Service=" + this.cbCategory.Text.Replace(" ", "%20"));
        this.request.Method = "POST";
        this.request.ContentType = "application/x-www-form-urlencoded";
        this.request.ContentLength = (long) bytes.Length;
        try
        {
          Stream requestStream = this.request.GetRequestStream();
          requestStream.Write(bytes, 0, bytes.Length);
          requestStream.Close();
          this.response = this.request.GetResponse();
          Stream responseStream = this.response.GetResponseStream();
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(responseStream);
          foreach (XmlNode selectNode in xmlDocument.SelectNodes("PARTNERS/P"))
          {
            string key = selectNode.Attributes["Title"].Value;
            string str = selectNode.Attributes["ID"].Value;
            if (!this.PartnerMapping.ContainsKey(key))
              this.PartnerMapping.Add(key, str);
            this.comBoxProviders.Items.Add((object) key);
          }
          this.comBoxProviders.Enabled = true;
        }
        catch (Exception ex)
        {
          this.response.Close();
        }
        this.comBoxProviders.SelectedIndex = 0;
      }
    }

    private void comBoxProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.isNew)
        this.resetCredentialFields();
      this.request = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassai/GetCredentialInfo.asp");
      ASCIIEncoding asciiEncoding = new ASCIIEncoding();
      if (!this.PartnerMapping.ContainsKey(this.comBoxProviders.Text))
        return;
      string s = "MapID=" + this.PartnerMapping[this.comBoxProviders.Text];
      byte[] bytes = asciiEncoding.GetBytes(s);
      this.request.Method = "POST";
      this.request.ContentType = "application/x-www-form-urlencoded";
      this.request.ContentLength = (long) bytes.Length;
      try
      {
        Stream requestStream = this.request.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        this.response = this.request.GetResponse();
        Stream responseStream = this.response.GetResponseStream();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(responseStream);
        foreach (XmlNode selectNode in xmlDocument.SelectNodes("CredentialInfos/C"))
        {
          string str1 = selectNode.Attributes["UserNameLabelName"].Value;
          string str2 = "";
          try
          {
            str2 = selectNode.Attributes["UserNameFieldName"].Value;
          }
          catch
          {
          }
          string str3 = selectNode.Attributes["PwdLabelName"].Value;
          string str4 = "";
          try
          {
            str4 = selectNode.Attributes["PwdFieldName"].Value;
          }
          catch
          {
          }
          string str5 = selectNode.Attributes["AuthLabel1Name"].Value;
          string str6 = "";
          try
          {
            str6 = selectNode.Attributes["AuthField1Name"].Value;
          }
          catch
          {
          }
          string str7 = selectNode.Attributes["AuthLabel2Name"].Value;
          string str8 = "";
          try
          {
            str8 = selectNode.Attributes["AuthField2Name"].Value;
          }
          catch
          {
          }
          try
          {
            this.partnerSection = selectNode.Attributes["PartnerSection"].Value;
          }
          catch
          {
          }
          try
          {
            this.saveLoginFieldName = selectNode.Attributes["SaveLoginFldName"].Value;
          }
          catch
          {
          }
          try
          {
            this.saveLoginValue = selectNode.Attributes["SaveLoginFldValues"].Value;
          }
          catch
          {
          }
          try
          {
            this.encryptionType = selectNode.Attributes["EncryptionType"].Value;
          }
          catch
          {
          }
          if (str1 != "" || str3 != "" || str5 != "" || str7 != "")
            this.pnlEtc.Visible = true;
          else
            this.pnlEtc.Visible = false;
          if (str7 != "")
          {
            this.pnlAuth2.Visible = true;
            this.lblAuth2.Text = str7;
            this.lblAuth2.Tag = (object) str8;
          }
          else
            this.pnlAuth2.Visible = false;
          if (str5 != "")
          {
            this.lblAuth1.Text = str5;
            this.pnlAuth1.Visible = true;
            this.lblAuth1.Tag = (object) str6;
          }
          else
            this.pnlAuth1.Visible = false;
          if (str3 != "")
          {
            this.lblPwd.Text = str3;
            this.lblPwd.Tag = (object) str4;
            this.pnlPWD.Visible = true;
          }
          else
            this.pnlPWD.Visible = false;
          if (str1 != "")
          {
            this.lblUserID.Text = str1;
            this.lblUserID.Tag = (object) str2;
            this.pnlUID.Visible = true;
          }
          else
            this.pnlUID.Visible = false;
          if (string.Compare(this.comBoxProviders.Text, "Freddie Mac's Loan Product Advisor System to System", false) == 0)
          {
            this.lblTPONumber.Text = "TPO Number";
            this.lblTPONumber.Tag = (object) "TPOAccountIdentifier";
            this.pnlTPO.Visible = true;
          }
          else
            this.pnlTPO.Visible = false;
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        this.response.Close();
      }
    }

    private void resetCredentialFields()
    {
      this.encryptionType = "None";
      this.lblUserID.Text = "";
      this.lblUserID.Tag = (object) "";
      this.lblPwd.Text = "";
      this.lblPwd.Tag = (object) "";
      this.lblAuth1.Text = "";
      this.lblAuth1.Tag = (object) "";
      this.lblAuth2.Text = "";
      this.lblAuth2.Tag = (object) "";
      this.txtUID.Text = "";
      this.txtPWD.Text = "";
      this.txtAuth1.Text = "";
      this.txtAuth2.Text = "";
      this.pnlUID.Visible = false;
      this.pnlPWD.Visible = false;
      this.pnlAuth1.Visible = false;
      this.pnlAuth2.Visible = false;
      this.pnlEtc.Visible = false;
      this.currentUserList = new string[0];
      this.txtUserCount.Text = "";
      this.partnerSection = "";
      this.saveLoginFieldName = "";
      this.saveLoginValue = "";
      this.encryptionType = "";
      this.passwordModified = false;
      this.pnlTPO.Visible = false;
      this.lblTPONumber.Text = "";
      this.lblTPONumber.Tag = (object) "";
      this.txtTPONumber.Text = "";
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.cbCategory.SelectedIndex == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a category.");
      }
      else if (this.comBoxProviders.SelectedIndex == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a provider.");
      }
      else if (!this.pnlEtc.Visible)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "This provider is not configured for this service.");
      }
      else if (this.txtDescription.Text == "")
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "Please provide information for Description.");
      }
      else if (this.txtUID.Text == "")
      {
        int num5 = (int) Utils.Dialog((IWin32Window) this, "Please provide information for " + this.lblUserID.Text + ".");
      }
      else if (this.txtPWD.Text == "")
      {
        int num6 = (int) Utils.Dialog((IWin32Window) this, "Please provide information for " + this.lblPwd.Text + ".");
      }
      else if (this.txtUserCount.Text == "" || this.txtUserCount.Text == "0")
      {
        int num7 = (int) Utils.Dialog((IWin32Window) this, "Please select a user to use this credential setting.");
      }
      else
      {
        if (this.isNew)
        {
          if (this.RequirePasswordEncryption)
            this.txtPWD.Text = this.getEncryptedPassword(this.txtPWD.Text, this.encryptionType);
          this.existingSetting = new ePassCredentialSetting(this.cbCategory.Text, this.comBoxProviders.Text, this.lblUserID.Text, this.txtUID.Text, this.lblPwd.Text, this.txtPWD.Text, this.lblAuth1.Text, this.txtAuth1.Text, this.lblAuth2.Text, this.txtAuth2.Text, this.txtDescription.Text, this.txtExpirationDays.ReadOnly ? -1 : int.Parse(this.txtExpirationDays.Text), string.Concat(this.lblUserID.Tag), string.Concat(this.lblPwd.Tag), string.Concat(this.lblAuth1.Tag), string.Concat(this.lblAuth2.Tag), this.partnerSection, this.saveLoginFieldName, this.saveLoginValue, this.encryptionType, this.lblTPONumber.Text, string.Concat(this.lblTPONumber.Tag), this.txtTPONumber.Text);
        }
        else
        {
          if (this.RequirePasswordEncryption && this.passwordModified)
            this.txtPWD.Text = this.getEncryptedPassword(this.txtPWD.Text, this.encryptionType);
          this.existingSetting.Title = this.comBoxProviders.SelectedItem.ToString();
          this.existingSetting.UIDValue = this.txtUID.Text;
          this.existingSetting.UIDFieldName = string.Concat(this.lblUserID.Tag);
          this.existingSetting.PasswordValue = this.txtPWD.Text;
          this.existingSetting.PasswordFieldName = string.Concat(this.lblPwd.Tag);
          this.existingSetting.Auth1Value = this.txtAuth1.Text;
          this.existingSetting.Auth1FieldName = string.Concat(this.lblAuth1.Tag);
          this.existingSetting.Auth2Value = this.txtAuth2.Text;
          this.existingSetting.Auth2FieldName = string.Concat(this.lblAuth2.Tag);
          this.existingSetting.Description = this.txtDescription.Text;
          this.existingSetting.PartnerSection = this.partnerSection;
          this.existingSetting.SaveLoginFieldName = this.saveLoginFieldName;
          this.existingSetting.SaveLoginValue = this.saveLoginValue;
          this.existingSetting.EncryptionType = this.encryptionType;
          this.existingSetting.TPOName = this.lblTPONumber.Text;
          this.existingSetting.TPOFieldName = string.Concat(this.lblTPONumber.Tag);
          this.existingSetting.TPOFieldValue = this.txtTPONumber.Text;
          this.existingSetting.ValidDuration = this.txtExpirationDays.ReadOnly || !Utils.IsInt((object) this.txtExpirationDays.Text) ? -1 : int.Parse(this.txtExpirationDays.Text);
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private string getEncryptedPassword(string password, string encryptionType)
    {
      Encoding utF8 = Encoding.UTF8;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.ToUniversalTime();
      string s = dateTime.Ticks.ToString();
      string base64String1 = Convert.ToBase64String(utF8.GetBytes(s));
      string base64String2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
      ePassAService ePassAservice = new ePassAService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.ePassAServiceUrl);
      try
      {
        return ePassAservice.GetePassAService(base64String1, base64String2, encryptionType);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return "";
      }
    }

    public ePassCredentialSetting UpdatedSetting => this.existingSetting;

    public string[] SelectedUserIDs => this.currentUserList;

    private void chkEmailNotification_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkEmailNotification.Checked)
      {
        this.txtExpirationDays.ReadOnly = false;
      }
      else
      {
        this.txtExpirationDays.ReadOnly = true;
        this.txtExpirationDays.Text = "";
      }
    }

    private void btnSearchUser_Click(object sender, EventArgs e)
    {
      ePassCredentialUsers passCredentialUsers = new ePassCredentialUsers(this.existingSetting == null ? -1 : this.existingSetting.CredentialID, this.comBoxProviders.Text, new List<string>((IEnumerable<string>) this.currentUserList));
      if (DialogResult.OK != passCredentialUsers.ShowDialog((IWin32Window) this))
        return;
      this.currentUserList = passCredentialUsers.SelectedUserIDs.ToArray();
      this.txtUserCount.Text = string.Concat((object) this.currentUserList.Length);
    }

    private void txtPWD_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.passwordModified = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ePassCredentialDetail));
      this.groupContainer1 = new GroupContainer();
      this.pnlDynamic = new Panel();
      this.pnlEtc = new Panel();
      this.label4 = new Label();
      this.label7 = new Label();
      this.txtUserCount = new TextBox();
      this.label6 = new Label();
      this.txtExpirationDays = new TextBox();
      this.btnSearchUser = new StandardIconButton();
      this.chkEmailNotification = new CheckBox();
      this.label5 = new Label();
      this.pnlAuth2 = new Panel();
      this.lblAuth2 = new Label();
      this.txtAuth2 = new TextBox();
      this.pnlTPO = new Panel();
      this.lblTPONumber = new Label();
      this.txtTPONumber = new TextBox();
      this.pnlAuth1 = new Panel();
      this.lblAuth1 = new Label();
      this.txtAuth1 = new TextBox();
      this.pnlPWD = new Panel();
      this.lblPwd = new Label();
      this.txtPWD = new TextBox();
      this.pnlUID = new Panel();
      this.lblUserID = new Label();
      this.txtUID = new TextBox();
      this.txtDescription = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.comBoxProviders = new ComboBox();
      this.cbCategory = new ComboBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      this.pnlDynamic.SuspendLayout();
      this.pnlEtc.SuspendLayout();
      ((ISupportInitialize) this.btnSearchUser).BeginInit();
      this.pnlAuth2.SuspendLayout();
      this.pnlTPO.SuspendLayout();
      this.pnlAuth1.SuspendLayout();
      this.pnlPWD.SuspendLayout();
      this.pnlUID.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.pnlDynamic);
      this.groupContainer1.Controls.Add((Control) this.txtDescription);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.comBoxProviders);
      this.groupContainer1.Controls.Add((Control) this.cbCategory);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(427, 247);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Account Information";
      this.pnlDynamic.BackColor = Color.Transparent;
      this.pnlDynamic.Controls.Add((Control) this.pnlEtc);
      this.pnlDynamic.Controls.Add((Control) this.pnlAuth2);
      this.pnlDynamic.Controls.Add((Control) this.pnlTPO);
      this.pnlDynamic.Controls.Add((Control) this.pnlAuth1);
      this.pnlDynamic.Controls.Add((Control) this.pnlPWD);
      this.pnlDynamic.Controls.Add((Control) this.pnlUID);
      this.pnlDynamic.Location = new Point(4, 97);
      this.pnlDynamic.Name = "pnlDynamic";
      this.pnlDynamic.Size = new Size(414, 148);
      this.pnlDynamic.TabIndex = 18;
      this.pnlEtc.Controls.Add((Control) this.label4);
      this.pnlEtc.Controls.Add((Control) this.label7);
      this.pnlEtc.Controls.Add((Control) this.txtUserCount);
      this.pnlEtc.Controls.Add((Control) this.label6);
      this.pnlEtc.Controls.Add((Control) this.txtExpirationDays);
      this.pnlEtc.Controls.Add((Control) this.btnSearchUser);
      this.pnlEtc.Controls.Add((Control) this.chkEmailNotification);
      this.pnlEtc.Controls.Add((Control) this.label5);
      this.pnlEtc.Dock = DockStyle.Fill;
      this.pnlEtc.Location = new Point(0, 115);
      this.pnlEtc.Name = "pnlEtc";
      this.pnlEtc.Size = new Size(414, 33);
      this.pnlEtc.TabIndex = 2;
      this.pnlEtc.Visible = false;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(79, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Selected Users";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(300, 47);
      this.label7.Name = "label7";
      this.label7.Size = new Size(32, 13);
      this.label7.TabIndex = 17;
      this.label7.Text = "days.";
      this.label7.Visible = false;
      this.txtUserCount.Location = new Point(160, 3);
      this.txtUserCount.Name = "txtUserCount";
      this.txtUserCount.ReadOnly = true;
      this.txtUserCount.Size = new Size(100, 20);
      this.txtUserCount.TabIndex = 11;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(179, 47);
      this.label6.Name = "label6";
      this.label6.Size = new Size(66, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "password in ";
      this.label6.Visible = false;
      this.txtExpirationDays.Location = new Point(247, 45);
      this.txtExpirationDays.Name = "txtExpirationDays";
      this.txtExpirationDays.ReadOnly = true;
      this.txtExpirationDays.Size = new Size(47, 20);
      this.txtExpirationDays.TabIndex = 15;
      this.txtExpirationDays.TextAlign = HorizontalAlignment.Right;
      this.txtExpirationDays.Visible = false;
      this.btnSearchUser.BackColor = Color.Transparent;
      this.btnSearchUser.Location = new Point(266, 6);
      this.btnSearchUser.MouseDownImage = (Image) null;
      this.btnSearchUser.Name = "btnSearchUser";
      this.btnSearchUser.Size = new Size(16, 16);
      this.btnSearchUser.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSearchUser.TabIndex = 12;
      this.btnSearchUser.TabStop = false;
      this.btnSearchUser.Click += new EventHandler(this.btnSearchUser_Click);
      this.chkEmailNotification.Location = new Point(160, 25);
      this.chkEmailNotification.Name = "chkEmailNotification";
      this.chkEmailNotification.Size = new Size(176, 23);
      this.chkEmailNotification.TabIndex = 14;
      this.chkEmailNotification.Text = "Send email reminder to change";
      this.chkEmailNotification.UseVisualStyleBackColor = true;
      this.chkEmailNotification.Visible = false;
      this.chkEmailNotification.CheckedChanged += new EventHandler(this.chkEmailNotification_CheckedChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 29);
      this.label5.Name = "label5";
      this.label5.Size = new Size(52, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Reminder";
      this.label5.Visible = false;
      this.pnlAuth2.Controls.Add((Control) this.lblAuth2);
      this.pnlAuth2.Controls.Add((Control) this.txtAuth2);
      this.pnlAuth2.Dock = DockStyle.Top;
      this.pnlAuth2.Location = new Point(0, 92);
      this.pnlAuth2.Name = "pnlAuth2";
      this.pnlAuth2.Size = new Size(414, 23);
      this.pnlAuth2.TabIndex = 1;
      this.pnlAuth2.Visible = false;
      this.lblAuth2.AutoSize = true;
      this.lblAuth2.Location = new Point(7, 6);
      this.lblAuth2.Name = "lblAuth2";
      this.lblAuth2.Size = new Size(35, 13);
      this.lblAuth2.TabIndex = 3;
      this.lblAuth2.Text = "Auth2";
      this.txtAuth2.Location = new Point(160, 3);
      this.txtAuth2.Name = "txtAuth2";
      this.txtAuth2.Size = new Size(244, 20);
      this.txtAuth2.TabIndex = 7;
      this.pnlTPO.Controls.Add((Control) this.lblTPONumber);
      this.pnlTPO.Controls.Add((Control) this.txtTPONumber);
      this.pnlTPO.Dock = DockStyle.Top;
      this.pnlTPO.Location = new Point(0, 69);
      this.pnlTPO.Name = "pnlTPO";
      this.pnlTPO.Size = new Size(414, 23);
      this.pnlTPO.TabIndex = 5;
      this.pnlTPO.Visible = false;
      this.lblTPONumber.AutoSize = true;
      this.lblTPONumber.Location = new Point(7, 6);
      this.lblTPONumber.Name = "lblTPONumber";
      this.lblTPONumber.Size = new Size(69, 13);
      this.lblTPONumber.TabIndex = 3;
      this.lblTPONumber.Text = "TPO Number";
      this.txtTPONumber.Location = new Point(160, 3);
      this.txtTPONumber.Name = "txtTPONumber";
      this.txtTPONumber.Size = new Size(244, 20);
      this.txtTPONumber.TabIndex = 6;
      this.pnlAuth1.Controls.Add((Control) this.lblAuth1);
      this.pnlAuth1.Controls.Add((Control) this.txtAuth1);
      this.pnlAuth1.Dock = DockStyle.Top;
      this.pnlAuth1.Location = new Point(0, 46);
      this.pnlAuth1.Name = "pnlAuth1";
      this.pnlAuth1.Size = new Size(414, 23);
      this.pnlAuth1.TabIndex = 0;
      this.pnlAuth1.Visible = false;
      this.lblAuth1.AutoSize = true;
      this.lblAuth1.Location = new Point(7, 6);
      this.lblAuth1.Name = "lblAuth1";
      this.lblAuth1.Size = new Size(35, 13);
      this.lblAuth1.TabIndex = 2;
      this.lblAuth1.Text = "Auth1";
      this.txtAuth1.Location = new Point(160, 3);
      this.txtAuth1.Name = "txtAuth1";
      this.txtAuth1.Size = new Size(244, 20);
      this.txtAuth1.TabIndex = 5;
      this.pnlPWD.Controls.Add((Control) this.lblPwd);
      this.pnlPWD.Controls.Add((Control) this.txtPWD);
      this.pnlPWD.Dock = DockStyle.Top;
      this.pnlPWD.Location = new Point(0, 23);
      this.pnlPWD.Name = "pnlPWD";
      this.pnlPWD.Size = new Size(414, 23);
      this.pnlPWD.TabIndex = 4;
      this.pnlPWD.Visible = false;
      this.lblPwd.AutoSize = true;
      this.lblPwd.Location = new Point(7, 6);
      this.lblPwd.Name = "lblPwd";
      this.lblPwd.Size = new Size(33, 13);
      this.lblPwd.TabIndex = 1;
      this.lblPwd.Text = "PWD";
      this.txtPWD.Location = new Point(160, 3);
      this.txtPWD.Name = "txtPWD";
      this.txtPWD.Size = new Size(244, 20);
      this.txtPWD.TabIndex = 4;
      this.txtPWD.UseSystemPasswordChar = true;
      this.txtPWD.TextChanged += new EventHandler(this.txtPWD_TextChanged);
      this.pnlUID.Controls.Add((Control) this.lblUserID);
      this.pnlUID.Controls.Add((Control) this.txtUID);
      this.pnlUID.Dock = DockStyle.Top;
      this.pnlUID.Location = new Point(0, 0);
      this.pnlUID.Name = "pnlUID";
      this.pnlUID.Size = new Size(414, 23);
      this.pnlUID.TabIndex = 3;
      this.pnlUID.Visible = false;
      this.lblUserID.AutoSize = true;
      this.lblUserID.Location = new Point(7, 6);
      this.lblUserID.Name = "lblUserID";
      this.lblUserID.Size = new Size(26, 13);
      this.lblUserID.TabIndex = 0;
      this.lblUserID.Text = "UID";
      this.txtUID.Location = new Point(160, 3);
      this.txtUID.Name = "txtUID";
      this.txtUID.Size = new Size(244, 20);
      this.txtUID.TabIndex = 3;
      this.txtDescription.Location = new Point(164, 77);
      this.txtDescription.MaxLength = 100;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(244, 20);
      this.txtDescription.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 80);
      this.label3.Name = "label3";
      this.label3.Size = new Size(60, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Description";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(77, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Provider Name";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Service Category";
      this.comBoxProviders.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comBoxProviders.Enabled = false;
      this.comBoxProviders.FormattingEnabled = true;
      this.comBoxProviders.Location = new Point(164, 53);
      this.comBoxProviders.Name = "comBoxProviders";
      this.comBoxProviders.Size = new Size(244, 21);
      this.comBoxProviders.TabIndex = 1;
      this.comBoxProviders.SelectedIndexChanged += new EventHandler(this.comBoxProviders_SelectedIndexChanged);
      this.cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbCategory.FormattingEnabled = true;
      this.cbCategory.Location = new Point(164, 30);
      this.cbCategory.Name = "cbCategory";
      this.cbCategory.Size = new Size(244, 21);
      this.cbCategory.TabIndex = 0;
      this.cbCategory.SelectedIndexChanged += new EventHandler(this.cbCategory_SelectedIndexChanged);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(283, 265);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 20;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(364, 265);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Services Password Management";
      this.emHelpLink1.Location = new Point(12, 272);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 59;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(450, 300);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ePassCredentialDetail);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Account Details";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.pnlDynamic.ResumeLayout(false);
      this.pnlEtc.ResumeLayout(false);
      this.pnlEtc.PerformLayout();
      ((ISupportInitialize) this.btnSearchUser).EndInit();
      this.pnlAuth2.ResumeLayout(false);
      this.pnlAuth2.PerformLayout();
      this.pnlTPO.ResumeLayout(false);
      this.pnlTPO.PerformLayout();
      this.pnlAuth1.ResumeLayout(false);
      this.pnlAuth1.PerformLayout();
      this.pnlPWD.ResumeLayout(false);
      this.pnlPWD.PerformLayout();
      this.pnlUID.ResumeLayout(false);
      this.pnlUID.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
