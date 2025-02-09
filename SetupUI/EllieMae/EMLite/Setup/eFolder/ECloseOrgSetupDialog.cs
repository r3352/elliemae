// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ECloseOrgSetupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ECloseOrgSetupDialog : Form
  {
    private eCloseSetupControl formParent;
    private Sessions.Session session;
    private eCloseRestClient _client;
    private string originalZipCode = string.Empty;
    private bool invokeCheckedItemEvent;
    private IContainer components;
    private Label lblLenderAddress;
    private Label lblLenderCity;
    private Label lblLenderState;
    private Label lblLenderZip;
    private TextBox txtLenderAddress;
    private GroupContainer gcLender;
    private TextBox txtLenderState;
    private TextBox txtLenderCity;
    private TextBox txtLenderName;
    private Label lblLenderName;
    private TextBox txtLenderZip;
    private GroupContainer gcContact;
    private TextBox txtContactEmail;
    private Label lblContactEmail;
    private TextBox txtContactPhone;
    private TextBox txtContactTitle;
    private TextBox txtContactLastName;
    private TextBox txtContactFirstName;
    private Label lblContactFirstName;
    private Label lblContactMiddleName;
    private TextBox txtContactMiddleName;
    private Label lblContactLastName;
    private Label lblContactPhone;
    private Label lblContactTitle;
    private GroupContainer gcStreamlineAccess;
    private Label lblAccessTypes;
    private Label lblAccessQuestions;
    private Label lblInvalidAttempts2;
    private TextBox txtInvalidAttempts;
    private Label lblDaysAfterClosing2;
    private TextBox txtDaysAfterClosing;
    private Label lblDaysSent2;
    private TextBox txtDaysSent;
    private Label lblAccessLink;
    private CheckBox chkStreamlinedAccess;
    private Button btnSave;
    private Button btnCancel;
    private CheckedListBox lstAccessQuestions;
    private CheckedListBox lstAccessTypes;
    private CheckedListBox lstAccessWhen;
    private Label lblInvalidAttempts1;
    private Label lblDaysAfterClosing1;
    private Label lblDaysSent1;
    private Label lblAccessWhen;

    public ECloseOrgSetupDialog(Sessions.Session session, eCloseSetupControl form)
    {
      this.formParent = form;
      this.session = session;
      this._client = form._client;
      this.InitializeComponent();
      this.populateFields();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure to Exit ? Any changes will be loss", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.HasValidationError() || Utils.Dialog((IWin32Window) this, "Do you want to save the changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      string lenderOrganization = this._client.CreateLenderOrganization(this.getJsonObject());
      if (!lenderOrganization.Contains("Error"))
      {
        this.formParent.btnGetSetup.Visible = false;
        this.formParent.gcFulfillment.Text = "Encompass eClose - Setup Complete (" + lenderOrganization + ")";
        int num = (int) MessageBox.Show("eClose Organization Setup successfully completed", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.Close();
      }
      else
      {
        int num1 = (int) MessageBox.Show(lenderOrganization, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void txtLenderZip_Enter(object sender, EventArgs e)
    {
      this.originalZipCode = this.txtLenderZip.Text.Trim();
    }

    private void txtLenderZip_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.ZIPCODE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtLenderZip_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return;
      string str = textBox.Text.Trim();
      if (str.Length < 5 || this.originalZipCode == str && this.txtLenderState.Text.Trim() != string.Empty && this.txtLenderCity.Text.Trim() != string.Empty)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(str.Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(str.Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      this.txtLenderCity.Text = zipCodeInfo.City;
      this.txtLenderState.Text = zipCodeInfo.State.ToUpper();
    }

    private void chkStreamlinedAccess_CheckeChanged(object sender, EventArgs e)
    {
      if (this.chkStreamlinedAccess.Checked)
      {
        this.lstAccessWhen.Enabled = true;
        this.lstAccessTypes.Enabled = true;
        this.lstAccessQuestions.Enabled = true;
        this.txtDaysSent.Enabled = true;
        this.txtDaysAfterClosing.Enabled = true;
        this.txtInvalidAttempts.Enabled = true;
        this.txtDaysAfterClosing.Text = "7";
        this.txtDaysSent.Text = "14";
        this.txtInvalidAttempts.Text = "10";
        List<int> intList = new List<int>();
        foreach (object obj in (ListBox.ObjectCollection) this.lstAccessQuestions.Items)
        {
          switch (this.lstAccessQuestions.GetItemText(obj).ToLower())
          {
            case "borrower first name":
              intList.Add(this.lstAccessQuestions.Items.IndexOf(obj));
              continue;
            case "property state":
              intList.Add(this.lstAccessQuestions.Items.IndexOf(obj));
              continue;
            case "property zipcode":
              intList.Add(this.lstAccessQuestions.Items.IndexOf(obj));
              continue;
            default:
              continue;
          }
        }
        this.invokeCheckedItemEvent = intList.Count <= 0;
        foreach (int index in intList)
          this.lstAccessQuestions.SetItemChecked(index, true);
        this.invokeCheckedItemEvent = true;
      }
      else
      {
        this.txtDaysAfterClosing.Text = string.Empty;
        this.txtDaysSent.Text = string.Empty;
        this.txtInvalidAttempts.Text = string.Empty;
        this.invokeCheckedItemEvent = false;
        for (int index = 0; index < this.lstAccessQuestions.Items.Count; ++index)
          this.lstAccessQuestions.SetItemChecked(index, false);
        for (int index = 0; index < this.lstAccessTypes.Items.Count; ++index)
          this.lstAccessTypes.SetItemChecked(index, false);
        for (int index = 0; index < this.lstAccessWhen.Items.Count; ++index)
          this.lstAccessWhen.SetItemChecked(index, false);
        this.invokeCheckedItemEvent = true;
        this.lstAccessQuestions.ClearSelected();
        this.lstAccessTypes.ClearSelected();
        this.lstAccessWhen.ClearSelected();
        this.lstAccessWhen.Enabled = false;
        this.lstAccessTypes.Enabled = false;
        this.lstAccessQuestions.Enabled = false;
        this.txtDaysSent.Enabled = false;
        this.txtDaysAfterClosing.Enabled = false;
        this.txtInvalidAttempts.Enabled = false;
      }
    }

    private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!(sender is CheckedListBox) || !this.invokeCheckedItemEvent)
        return;
      CheckedListBox checkedListBox1 = (CheckedListBox) sender;
      if (checkedListBox1 == null)
        return;
      CheckedListBox checkedListBox2 = checkedListBox1;
      Point client = checkedListBox1.PointToClient(Cursor.Position);
      int x = client.X;
      client = checkedListBox1.PointToClient(Cursor.Position);
      int y = client.Y;
      if (checkedListBox2.IndexFromPoint(x, y) > -1)
        return;
      e.NewValue = e.CurrentValue;
    }

    private bool HasValidationError()
    {
      bool flag = false;
      string empty = string.Empty;
      if (string.IsNullOrEmpty(this.txtLenderName.Text))
      {
        empty += "\n• You must enter a Lender Name";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtLenderAddress.Text))
      {
        empty += "\n• You must enter a Lender Address";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtLenderCity.Text))
      {
        empty += "\n• You must enter a Lender City";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtLenderState.Text))
      {
        empty += "\n• You must enter a Lender State";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtLenderZip.Text))
      {
        empty += "\n• You must enter a Lender Zip";
        flag = true;
      }
      if (!string.IsNullOrEmpty(this.txtLenderZip.Text) && (this.txtLenderZip.Text.Length != 5 || this.txtLenderZip.Text.Contains("-")))
      {
        empty += "\n• Zip must be 5 digits and do not allows '-'";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtContactFirstName.Text))
      {
        empty += "\n• You must enter a First Name";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtContactLastName.Text))
      {
        empty += "\n• You must enter a Last Name";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtContactTitle.Text))
      {
        empty += "\n• You must enter a Title";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtContactEmail.Text))
      {
        empty += "\n• You must enter a Email";
        flag = true;
      }
      if (!string.IsNullOrEmpty(this.txtContactEmail.Text) && !Regex.IsMatch(this.txtContactEmail.Text, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
      {
        empty += "\n• Given Email is NOT valid";
        flag = true;
      }
      if (string.IsNullOrEmpty(this.txtContactPhone.Text))
      {
        empty += "\n• You must enter Phone";
        flag = true;
      }
      if (!string.IsNullOrEmpty(this.txtContactPhone.Text) && (this.txtContactPhone.Text.Length != 10 || this.txtContactPhone.Text.Contains("-")))
      {
        empty += "\n• Phone must be 10 digits and do not allows '-'";
        flag = true;
      }
      if (this.chkStreamlinedAccess.Checked)
      {
        if (this.lstAccessWhen.CheckedIndices.Count == 0)
        {
          empty += "\n• An option must be checked for When to Allow Streamline Access";
          flag = true;
        }
        if (this.lstAccessTypes.CheckedIndices.Count == 0)
        {
          empty += "\n• An option must be checked for Allowed Streamline Access";
          flag = true;
        }
        if (this.lstAccessQuestions.CheckedIndices.Count < 3)
        {
          empty += "\n• You must select at least 3 Knowledge Based Questions";
          flag = true;
        }
        if (string.IsNullOrEmpty(this.txtDaysSent.Text) || string.IsNullOrEmpty(this.txtDaysAfterClosing.Text) || string.IsNullOrEmpty(this.txtInvalidAttempts.Text))
        {
          empty += "\n• Values must be entered for Streamlined Access Options";
          flag = true;
        }
      }
      if (flag)
      {
        int num = (int) MessageBox.Show("Error: \n" + empty, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    private JObject getJsonObject()
    {
      string empty = string.Empty;
      JObject jsonObject = new JObject();
      jsonObject.Add("address", (JToken) new JObject()
      {
        {
          "city",
          (JToken) this.txtLenderCity.Text.Trim()
        },
        {
          "postalCode",
          (JToken) this.txtLenderZip.Text.Trim()
        },
        {
          "state",
          (JToken) this.txtLenderState.Text.Trim()
        },
        {
          "street",
          (JToken) this.txtLenderAddress.Text.Trim()
        }
      });
      jsonObject.Add("contact", (JToken) new JObject()
      {
        {
          "email",
          (JToken) this.txtContactEmail.Text.Trim()
        },
        {
          "firstName",
          (JToken) this.txtContactFirstName.Text.Trim()
        },
        {
          "lastName",
          (JToken) this.txtContactLastName.Text.Trim()
        },
        {
          "middleName",
          (JToken) this.txtContactMiddleName.Text.Trim()
        },
        {
          "phone",
          (JToken) this.txtContactPhone.Text.Trim()
        },
        {
          "title",
          (JToken) this.txtContactTitle.Text.Trim()
        }
      });
      jsonObject.Add("name", (JToken) this.txtLenderName.Text.Trim());
      JObject jobject1 = new JObject();
      jobject1.Add("enableStreamlinedAccess", (JToken) this.chkStreamlinedAccess.Checked);
      JObject jobject2 = new JObject();
      if (this.chkStreamlinedAccess.Checked)
      {
        JArray jarray1 = new JArray();
        foreach (object obj in (ListBox.ObjectCollection) this.lstAccessQuestions.Items)
        {
          if (this.lstAccessQuestions.GetItemCheckState(this.lstAccessQuestions.Items.IndexOf(obj)).ToString() == "Checked")
          {
            switch (obj.ToString().ToLower())
            {
              case "borrower first name":
                jarray1.Add((JToken) "BORROWER_FIRST");
                continue;
              case "closing date":
                jarray1.Add((JToken) "CLOSING_DATE");
                continue;
              case "loan amount":
                jarray1.Add((JToken) "LOAN_AMOUNT");
                continue;
              case "property state":
                jarray1.Add((JToken) "PROPERTY_STATE");
                continue;
              case "property zipcode":
                jarray1.Add((JToken) "PROPERTY_ZIP");
                continue;
              default:
                continue;
            }
          }
        }
        jobject2.Add("authenticationQuestions", (JToken) jarray1);
        CheckState itemCheckState = this.lstAccessTypes.GetItemCheckState(this.lstAccessTypes.Items.IndexOf((object) "Launch Borrower E-Signing"));
        string str1 = itemCheckState.ToString();
        jobject2.Add("borrowerEsignAuthType", (JToken) (str1 == "Checked" ? "SOFT_AUTH" : "FULL_AUTH"));
        foreach (object obj in (ListBox.ObjectCollection) this.lstAccessTypes.Items)
        {
          itemCheckState = this.lstAccessTypes.GetItemCheckState(this.lstAccessTypes.Items.IndexOf(obj));
          string str2 = itemCheckState.ToString();
          switch (obj.ToString().ToLower())
          {
            case "download documents":
              jobject2.Add("documentDownloadAuthType", (JToken) (str2 == "Checked" ? "SOFT_AUTH" : "FULL_AUTH"));
              continue;
            case "upload documents":
              jobject2.Add("documentUploadAuthType", (JToken) (str2 == "Checked" ? "SOFT_AUTH" : "FULL_AUTH"));
              continue;
            default:
              continue;
          }
        }
        jobject2.Add("invalidAttemptCount", (JToken) (string.IsNullOrEmpty(this.txtInvalidAttempts.Text) ? 0 : int.Parse(this.txtInvalidAttempts.Text.Trim())));
        JArray jarray2 = new JArray();
        foreach (object obj in (ListBox.ObjectCollection) this.lstAccessWhen.Items)
        {
          if (this.lstAccessWhen.GetItemCheckState(this.lstAccessWhen.Items.IndexOf(obj)).ToString() == "Checked")
          {
            switch (obj.ToString().ToLower())
            {
              case "closing":
                jarray2.Add((JToken) "ECLOSING");
                continue;
              case "post-closing":
                jarray2.Add((JToken) "POSTCLOSE");
                continue;
              default:
                continue;
            }
          }
        }
        jobject2.Add("streamlinedAccessTypes", (JToken) jarray2);
        jobject2.Add("urlExpiryAfterClosingInDays", (JToken) (string.IsNullOrEmpty(this.txtDaysAfterClosing.Text.Trim()) ? 0 : int.Parse(this.txtDaysAfterClosing.Text.Trim())));
        jobject2.Add("urlExpiryInDays", (JToken) (string.IsNullOrEmpty(this.txtDaysSent.Text.Trim()) ? 0 : int.Parse(this.txtDaysSent.Text.Trim())));
      }
      jobject1.Add("streamlinedAccessSetting", (JToken) jobject2);
      jsonObject.Add("settings", (JToken) jobject1);
      jsonObject.Add("spaceId", (JToken) "simplifile");
      return jsonObject;
    }

    private void populateFields()
    {
      this.lstAccessWhen.Enabled = false;
      this.lstAccessTypes.Enabled = false;
      this.lstAccessQuestions.Enabled = false;
      this.txtDaysSent.Enabled = false;
      this.txtDaysAfterClosing.Enabled = false;
      this.txtInvalidAttempts.Enabled = false;
      this.txtContactFirstName.Text = this.session.UserInfo.FirstName;
      this.txtContactMiddleName.Text = this.session.UserInfo.MiddleName;
      this.txtContactLastName.Text = this.session.UserInfo.LastName;
      this.txtContactTitle.Text = this.session.UserInfo.JobTitle;
      string str1 = this.session.UserInfo.Phone;
      if (!string.IsNullOrEmpty(str1))
      {
        str1 = str1.Replace("-", "").Replace(" ", "");
        if (str1.Length > 10)
          str1 = str1.Substring(0, 10);
      }
      this.txtContactPhone.Text = str1;
      this.txtContactEmail.Text = this.session.UserInfo.Email;
      UserInfo user = this.session.OrganizationManager.GetUser(this.session.UserID);
      if (!(user != (UserInfo) null))
        return;
      OrgInfo avaliableOrganization = this.session.OrganizationManager.GetFirstAvaliableOrganization(user.OrgId, true);
      if (avaliableOrganization == null)
        return;
      this.txtLenderName.Text = avaliableOrganization.CompanyName;
      this.txtLenderAddress.Text = avaliableOrganization.CompanyAddress.Street1;
      this.txtLenderCity.Text = avaliableOrganization.CompanyAddress.City;
      this.txtLenderState.Text = avaliableOrganization.CompanyAddress.State;
      string str2 = avaliableOrganization.CompanyAddress.Zip;
      if (!string.IsNullOrEmpty(str2))
      {
        str2 = str2.Replace("-", "").Replace(" ", "");
        if (str2.Length > 5)
          str2 = str2.Substring(0, 5);
      }
      this.txtLenderZip.Text = str2;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblLenderAddress = new Label();
      this.lblLenderCity = new Label();
      this.lblLenderState = new Label();
      this.lblLenderZip = new Label();
      this.txtLenderAddress = new TextBox();
      this.gcLender = new GroupContainer();
      this.txtLenderZip = new TextBox();
      this.txtLenderState = new TextBox();
      this.txtLenderCity = new TextBox();
      this.txtLenderName = new TextBox();
      this.lblLenderName = new Label();
      this.gcContact = new GroupContainer();
      this.txtContactEmail = new TextBox();
      this.lblContactEmail = new Label();
      this.txtContactPhone = new TextBox();
      this.txtContactTitle = new TextBox();
      this.txtContactLastName = new TextBox();
      this.txtContactFirstName = new TextBox();
      this.lblContactFirstName = new Label();
      this.lblContactMiddleName = new Label();
      this.txtContactMiddleName = new TextBox();
      this.lblContactLastName = new Label();
      this.lblContactPhone = new Label();
      this.lblContactTitle = new Label();
      this.gcStreamlineAccess = new GroupContainer();
      this.lblAccessTypes = new Label();
      this.lblAccessQuestions = new Label();
      this.lblInvalidAttempts2 = new Label();
      this.txtInvalidAttempts = new TextBox();
      this.lblDaysAfterClosing2 = new Label();
      this.txtDaysAfterClosing = new TextBox();
      this.lblDaysSent2 = new Label();
      this.txtDaysSent = new TextBox();
      this.lblAccessLink = new Label();
      this.chkStreamlinedAccess = new CheckBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.lblAccessWhen = new Label();
      this.lblDaysSent1 = new Label();
      this.lblDaysAfterClosing1 = new Label();
      this.lblInvalidAttempts1 = new Label();
      this.lstAccessWhen = new CheckedListBox();
      this.lstAccessTypes = new CheckedListBox();
      this.lstAccessQuestions = new CheckedListBox();
      this.gcLender.SuspendLayout();
      this.gcContact.SuspendLayout();
      this.gcStreamlineAccess.SuspendLayout();
      this.SuspendLayout();
      this.lblLenderAddress.AutoSize = true;
      this.lblLenderAddress.Location = new Point(12, 68);
      this.lblLenderAddress.Name = "lblLenderAddress";
      this.lblLenderAddress.Size = new Size(49, 14);
      this.lblLenderAddress.TabIndex = 2;
      this.lblLenderAddress.Text = "Address";
      this.lblLenderCity.AutoSize = true;
      this.lblLenderCity.Location = new Point(12, 96);
      this.lblLenderCity.Name = "lblLenderCity";
      this.lblLenderCity.Size = new Size(25, 14);
      this.lblLenderCity.TabIndex = 4;
      this.lblLenderCity.Text = "City";
      this.lblLenderState.AutoSize = true;
      this.lblLenderState.Location = new Point(12, 124);
      this.lblLenderState.Name = "lblLenderState";
      this.lblLenderState.Size = new Size(32, 14);
      this.lblLenderState.TabIndex = 6;
      this.lblLenderState.Text = "State";
      this.lblLenderZip.AutoSize = true;
      this.lblLenderZip.Location = new Point(12, 152);
      this.lblLenderZip.Name = "lblLenderZip";
      this.lblLenderZip.Size = new Size(22, 14);
      this.lblLenderZip.TabIndex = 8;
      this.lblLenderZip.Text = "Zip";
      this.txtLenderAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLenderAddress.Location = new Point(88, 64);
      this.txtLenderAddress.Name = "txtLenderAddress";
      this.txtLenderAddress.Size = new Size(334, 20);
      this.txtLenderAddress.TabIndex = 3;
      this.gcLender.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcLender.Controls.Add((Control) this.txtLenderZip);
      this.gcLender.Controls.Add((Control) this.txtLenderState);
      this.gcLender.Controls.Add((Control) this.txtLenderCity);
      this.gcLender.Controls.Add((Control) this.txtLenderName);
      this.gcLender.Controls.Add((Control) this.lblLenderName);
      this.gcLender.Controls.Add((Control) this.lblLenderAddress);
      this.gcLender.Controls.Add((Control) this.txtLenderAddress);
      this.gcLender.Controls.Add((Control) this.lblLenderCity);
      this.gcLender.Controls.Add((Control) this.lblLenderZip);
      this.gcLender.Controls.Add((Control) this.lblLenderState);
      this.gcLender.HeaderForeColor = SystemColors.ControlText;
      this.gcLender.Location = new Point(12, 12);
      this.gcLender.Name = "gcLender";
      this.gcLender.Size = new Size(438, 212);
      this.gcLender.TabIndex = 0;
      this.gcLender.Text = "Lender Information";
      this.txtLenderZip.Location = new Point(88, 148);
      this.txtLenderZip.Name = "txtLenderZip";
      this.txtLenderZip.Size = new Size(96, 20);
      this.txtLenderZip.TabIndex = 9;
      this.txtLenderZip.Enter += new EventHandler(this.txtLenderZip_Enter);
      this.txtLenderZip.KeyUp += new KeyEventHandler(this.txtLenderZip_KeyUp);
      this.txtLenderZip.Leave += new EventHandler(this.txtLenderZip_Leave);
      this.txtLenderState.Location = new Point(88, 120);
      this.txtLenderState.Name = "txtLenderState";
      this.txtLenderState.Size = new Size(54, 20);
      this.txtLenderState.TabIndex = 7;
      this.txtLenderCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLenderCity.Location = new Point(88, 92);
      this.txtLenderCity.Name = "txtLenderCity";
      this.txtLenderCity.Size = new Size(334, 20);
      this.txtLenderCity.TabIndex = 5;
      this.txtLenderName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLenderName.Location = new Point(88, 36);
      this.txtLenderName.Name = "txtLenderName";
      this.txtLenderName.Size = new Size(334, 20);
      this.txtLenderName.TabIndex = 1;
      this.lblLenderName.AutoSize = true;
      this.lblLenderName.Location = new Point(12, 40);
      this.lblLenderName.Name = "lblLenderName";
      this.lblLenderName.Size = new Size(71, 14);
      this.lblLenderName.TabIndex = 0;
      this.lblLenderName.Text = "Lender Name";
      this.gcContact.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcContact.Controls.Add((Control) this.txtContactEmail);
      this.gcContact.Controls.Add((Control) this.lblContactEmail);
      this.gcContact.Controls.Add((Control) this.txtContactPhone);
      this.gcContact.Controls.Add((Control) this.txtContactTitle);
      this.gcContact.Controls.Add((Control) this.txtContactLastName);
      this.gcContact.Controls.Add((Control) this.txtContactFirstName);
      this.gcContact.Controls.Add((Control) this.lblContactPhone);
      this.gcContact.Controls.Add((Control) this.lblContactFirstName);
      this.gcContact.Controls.Add((Control) this.lblContactMiddleName);
      this.gcContact.Controls.Add((Control) this.txtContactMiddleName);
      this.gcContact.Controls.Add((Control) this.lblContactLastName);
      this.gcContact.Controls.Add((Control) this.lblContactTitle);
      this.gcContact.HeaderForeColor = SystemColors.ControlText;
      this.gcContact.Location = new Point(464, 12);
      this.gcContact.Name = "gcContact";
      this.gcContact.Size = new Size(412, 212);
      this.gcContact.TabIndex = 1;
      this.gcContact.Text = "Lender Contact Information";
      this.txtContactEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContactEmail.Location = new Point(88, 148);
      this.txtContactEmail.Name = "txtContactEmail";
      this.txtContactEmail.Size = new Size(308, 20);
      this.txtContactEmail.TabIndex = 9;
      this.lblContactEmail.AutoSize = true;
      this.lblContactEmail.Location = new Point(12, 152);
      this.lblContactEmail.Name = "lblContactEmail";
      this.lblContactEmail.Size = new Size(31, 14);
      this.lblContactEmail.TabIndex = 8;
      this.lblContactEmail.Text = "Email";
      this.txtContactPhone.Location = new Point(88, 176);
      this.txtContactPhone.Name = "txtContactPhone";
      this.txtContactPhone.Size = new Size(136, 20);
      this.txtContactPhone.TabIndex = 11;
      this.txtContactPhone.MaxLength = 10;
      this.txtContactPhone.KeyPress += new KeyPressEventHandler(this.TxtBox_KeyPress);
      this.txtContactTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContactTitle.Location = new Point(88, 120);
      this.txtContactTitle.Name = "txtContactTitle";
      this.txtContactTitle.Size = new Size(308, 20);
      this.txtContactTitle.TabIndex = 7;
      this.txtContactLastName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContactLastName.Location = new Point(88, 92);
      this.txtContactLastName.Name = "txtContactLastName";
      this.txtContactLastName.Size = new Size(308, 20);
      this.txtContactLastName.TabIndex = 5;
      this.txtContactFirstName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContactFirstName.Location = new Point(88, 36);
      this.txtContactFirstName.Name = "txtContactFirstName";
      this.txtContactFirstName.Size = new Size(308, 20);
      this.txtContactFirstName.TabIndex = 1;
      this.lblContactFirstName.AutoSize = true;
      this.lblContactFirstName.Location = new Point(12, 40);
      this.lblContactFirstName.Name = "lblContactFirstName";
      this.lblContactFirstName.Size = new Size(58, 14);
      this.lblContactFirstName.TabIndex = 0;
      this.lblContactFirstName.Text = "First Name";
      this.lblContactMiddleName.AutoSize = true;
      this.lblContactMiddleName.Location = new Point(12, 68);
      this.lblContactMiddleName.Name = "lblContactMiddleName";
      this.lblContactMiddleName.Size = new Size(67, 14);
      this.lblContactMiddleName.TabIndex = 2;
      this.lblContactMiddleName.Text = "Middle Name";
      this.txtContactMiddleName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContactMiddleName.Location = new Point(88, 64);
      this.txtContactMiddleName.Name = "txtContactMiddleName";
      this.txtContactMiddleName.Size = new Size(308, 20);
      this.txtContactMiddleName.TabIndex = 3;
      this.lblContactLastName.AutoSize = true;
      this.lblContactLastName.Location = new Point(12, 96);
      this.lblContactLastName.Name = "lblContactLastName";
      this.lblContactLastName.Size = new Size(58, 14);
      this.lblContactLastName.TabIndex = 4;
      this.lblContactLastName.Text = "Last Name";
      this.lblContactPhone.AutoSize = true;
      this.lblContactPhone.Location = new Point(12, 180);
      this.lblContactPhone.Name = "lblContactPhone";
      this.lblContactPhone.Size = new Size(37, 14);
      this.lblContactPhone.TabIndex = 10;
      this.lblContactPhone.Text = "Phone";
      this.lblContactTitle.AutoSize = true;
      this.lblContactTitle.Location = new Point(12, 124);
      this.lblContactTitle.Name = "lblContactTitle";
      this.lblContactTitle.Size = new Size(26, 14);
      this.lblContactTitle.TabIndex = 6;
      this.lblContactTitle.Text = "Title";
      this.gcStreamlineAccess.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcStreamlineAccess.Controls.Add((Control) this.lstAccessQuestions);
      this.gcStreamlineAccess.Controls.Add((Control) this.lstAccessTypes);
      this.gcStreamlineAccess.Controls.Add((Control) this.lstAccessWhen);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblInvalidAttempts1);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblDaysAfterClosing1);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblDaysSent1);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblAccessWhen);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblAccessTypes);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblAccessQuestions);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblInvalidAttempts2);
      this.gcStreamlineAccess.Controls.Add((Control) this.txtInvalidAttempts);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblDaysAfterClosing2);
      this.gcStreamlineAccess.Controls.Add((Control) this.txtDaysAfterClosing);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblDaysSent2);
      this.gcStreamlineAccess.Controls.Add((Control) this.txtDaysSent);
      this.gcStreamlineAccess.Controls.Add((Control) this.lblAccessLink);
      this.gcStreamlineAccess.Controls.Add((Control) this.chkStreamlinedAccess);
      this.gcStreamlineAccess.HeaderForeColor = SystemColors.ControlText;
      this.gcStreamlineAccess.Location = new Point(12, 236);
      this.gcStreamlineAccess.Name = "gcStreamlineAccess";
      this.gcStreamlineAccess.Size = new Size(864, 166);
      this.gcStreamlineAccess.TabIndex = 2;
      this.lblAccessTypes.AutoSize = true;
      this.lblAccessTypes.Location = new Point(216, 36);
      this.lblAccessTypes.Name = "lblAccessTypes";
      this.lblAccessTypes.Size = new Size(143, 14);
      this.lblAccessTypes.TabIndex = 3;
      this.lblAccessTypes.Text = "Allowed Streamline Access:";
      this.lblAccessQuestions.AutoSize = true;
      this.lblAccessQuestions.Location = new Point(420, 36);
      this.lblAccessQuestions.Name = "lblAccessQuestions";
      this.lblAccessQuestions.Size = new Size(151, 14);
      this.lblAccessQuestions.TabIndex = 5;
      this.lblAccessQuestions.Text = "Knowledge Based Questions:";
      this.lblInvalidAttempts2.AutoSize = true;
      this.lblInvalidAttempts2.Location = new Point(728, 108);
      this.lblInvalidAttempts2.Name = "lblInvalidAttempts2";
      this.lblInvalidAttempts2.Size = new Size(120, 14);
      this.lblInvalidAttempts2.TabIndex = 16;
      this.lblInvalidAttempts2.Text = "invalid access attempts";
      this.txtInvalidAttempts.Location = new Point(684, 104);
      this.txtInvalidAttempts.Name = "txtInvalidAttempts";
      this.txtInvalidAttempts.Size = new Size(36, 20);
      this.txtInvalidAttempts.TabIndex = 15;
      this.txtInvalidAttempts.KeyPress += new KeyPressEventHandler(this.TxtBox_KeyPress);
      this.lblDaysAfterClosing2.AutoSize = true;
      this.lblDaysAfterClosing2.Location = new Point(728, 84);
      this.lblDaysAfterClosing2.Name = "lblDaysAfterClosing2";
      this.lblDaysAfterClosing2.Size = new Size(118, 14);
      this.lblDaysAfterClosing2.TabIndex = 13;
      this.lblDaysAfterClosing2.Text = "days after closing date";
      this.txtDaysAfterClosing.Location = new Point(684, 80);
      this.txtDaysAfterClosing.Name = "txtDaysAfterClosing";
      this.txtDaysAfterClosing.Size = new Size(36, 20);
      this.txtDaysAfterClosing.TabIndex = 12;
      this.txtDaysAfterClosing.KeyPress += new KeyPressEventHandler(this.TxtBox_KeyPress);
      this.lblDaysSent2.AutoSize = true;
      this.lblDaysSent2.Location = new Point(728, 60);
      this.lblDaysSent2.Name = "lblDaysSent2";
      this.lblDaysSent2.Size = new Size(105, 14);
      this.lblDaysSent2.TabIndex = 10;
      this.lblDaysSent2.Text = "days after sent date";
      this.txtDaysSent.Location = new Point(684, 56);
      this.txtDaysSent.Name = "txtDaysSent";
      this.txtDaysSent.Size = new Size(36, 20);
      this.txtDaysSent.TabIndex = 9;
      this.txtDaysSent.KeyPress += new KeyPressEventHandler(this.TxtBox_KeyPress);
      this.lblAccessLink.AutoSize = true;
      this.lblAccessLink.Location = new Point(624, 36);
      this.lblAccessLink.Name = "lblAccessLink";
      this.lblAccessLink.Size = new Size(162, 14);
      this.lblAccessLink.TabIndex = 7;
      this.lblAccessLink.Text = "Streamlined Access Options:";
      this.chkStreamlinedAccess.AutoSize = true;
      this.chkStreamlinedAccess.BackColor = Color.Transparent;
      this.chkStreamlinedAccess.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkStreamlinedAccess.Location = new Point(8, 5);
      this.chkStreamlinedAccess.Name = "chkStreamlinedAccess";
      this.chkStreamlinedAccess.Size = new Size(341, 18);
      this.chkStreamlinedAccess.TabIndex = 0;
      this.chkStreamlinedAccess.Text = "Enable Streamline Access for Settlement Agents?";
      this.chkStreamlinedAccess.UseVisualStyleBackColor = false;
      this.chkStreamlinedAccess.CheckedChanged += new EventHandler(this.chkStreamlinedAccess_CheckeChanged);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Location = new Point(718, 412);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.Location = new Point(800, 412);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblAccessWhen.AutoSize = true;
      this.lblAccessWhen.Location = new Point(12, 36);
      this.lblAccessWhen.Name = "lblAccessWhen";
      this.lblAccessWhen.Size = new Size(173, 14);
      this.lblAccessWhen.TabIndex = 1;
      this.lblAccessWhen.Text = "When to Allow Streamline Access:";
      this.lblDaysSent1.AutoSize = true;
      this.lblDaysSent1.Location = new Point(636, 60);
      this.lblDaysSent1.Name = "lblDaysSent1";
      this.lblDaysSent1.Size = new Size(43, 14);
      this.lblDaysSent1.TabIndex = 8;
      this.lblDaysSent1.Text = "Expires";
      this.lblDaysAfterClosing1.AutoSize = true;
      this.lblDaysAfterClosing1.Location = new Point(636, 84);
      this.lblDaysAfterClosing1.Name = "lblDaysAfterClosing1";
      this.lblDaysAfterClosing1.Size = new Size(43, 14);
      this.lblDaysAfterClosing1.TabIndex = 11;
      this.lblDaysAfterClosing1.Text = "Expires";
      this.lblInvalidAttempts1.AutoSize = true;
      this.lblInvalidAttempts1.Location = new Point(636, 108);
      this.lblInvalidAttempts1.Name = "lblInvalidAttempts1";
      this.lblInvalidAttempts1.Size = new Size(40, 14);
      this.lblInvalidAttempts1.TabIndex = 14;
      this.lblInvalidAttempts1.Text = "Max of";
      this.lstAccessWhen.FormattingEnabled = true;
      this.lstAccessWhen.IntegralHeight = false;
      this.lstAccessWhen.Items.AddRange(new object[2]
      {
        (object) "Closing",
        (object) "Post-Closing"
      });
      this.lstAccessWhen.Location = new Point(24, 56);
      this.lstAccessWhen.Name = "lstAccessWhen";
      this.lstAccessWhen.Size = new Size(168, 92);
      this.lstAccessWhen.TabIndex = 2;
      this.lstAccessWhen.CheckOnClick = true;
      this.lstAccessWhen.ItemCheck += new ItemCheckEventHandler(this.checkedListBox_ItemCheck);
      this.lstAccessTypes.FormattingEnabled = true;
      this.lstAccessTypes.IntegralHeight = false;
      this.lstAccessTypes.Items.AddRange(new object[3]
      {
        (object) "Download Documents",
        (object) "Upload Documents",
        (object) "Launch Borrower E-Signing"
      });
      this.lstAccessTypes.Location = new Point(228, 56);
      this.lstAccessTypes.Name = "lstAccessTypes";
      this.lstAccessTypes.Size = new Size(168, 92);
      this.lstAccessTypes.TabIndex = 4;
      this.lstAccessTypes.CheckOnClick = true;
      this.lstAccessTypes.ItemCheck += new ItemCheckEventHandler(this.checkedListBox_ItemCheck);
      this.lstAccessQuestions.FormattingEnabled = true;
      this.lstAccessQuestions.IntegralHeight = false;
      this.lstAccessQuestions.Items.AddRange(new object[5]
      {
        (object) "Borrower First Name",
        (object) "Closing Date",
        (object) "Loan Amount",
        (object) "Property State",
        (object) "Property Zipcode"
      });
      this.lstAccessQuestions.Location = new Point(432, 56);
      this.lstAccessQuestions.Name = "lstAccessQuestions";
      this.lstAccessQuestions.Size = new Size(168, 92);
      this.lstAccessQuestions.TabIndex = 6;
      this.lstAccessQuestions.CheckOnClick = true;
      this.lstAccessQuestions.ItemCheck += new ItemCheckEventHandler(this.checkedListBox_ItemCheck);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(888, 443);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.gcStreamlineAccess);
      this.Controls.Add((Control) this.gcContact);
      this.Controls.Add((Control) this.gcLender);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ECloseOrgSetupDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "eClose Organization Setup";
      this.gcLender.ResumeLayout(false);
      this.gcLender.PerformLayout();
      this.gcContact.ResumeLayout(false);
      this.gcContact.PerformLayout();
      this.gcStreamlineAccess.ResumeLayout(false);
      this.gcStreamlineAccess.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
