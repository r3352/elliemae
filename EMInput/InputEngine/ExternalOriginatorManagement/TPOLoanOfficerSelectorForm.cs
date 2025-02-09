// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ExternalOriginatorManagement.TPOLoanOfficerSelectorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.ExternalOriginatorManagement
{
  public class TPOLoanOfficerSelectorForm : Form
  {
    private List<ExternalOriginatorManagementData> orgs;
    protected Sessions.Session session;
    internal List<ExternalOriginatorManagementData> tpoBranches;
    private ExternalUserInfo[] tpoUsers;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private Button button1;
    private Button button2;
    private TextBox txtCompany;
    private TextBox txtBranch;
    private StandardIconButton btnSelectCompany;
    private StandardIconButton btnSelectBranch;
    private ComboBox cbLO;

    public TPOLoanOfficerSelectorForm(
      List<ExternalOriginatorManagementData> orgs,
      Sessions.Session session)
    {
      this.orgs = orgs;
      this.session = session;
      this.InitializeComponent();
    }

    private void btnSelectCompany_Click(object sender, EventArgs e)
    {
      using (TPOCompanySelectorForm companySelectorForm = new TPOCompanySelectorForm(this.orgs, this.session, false))
      {
        if (companySelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.txtCompany.Text = companySelectorForm.SelectedOrganization.OrganizationName;
        this.txtCompany.Tag = (object) companySelectorForm.SelectedOrganization;
        this.button1.Enabled = true;
        this.clear();
        this.fetchTPOSettings();
        this.addLOusers();
      }
    }

    private void btnSelectBranch_Click(object sender, EventArgs e)
    {
      using (TPOCompanySelectorForm companySelectorForm = new TPOCompanySelectorForm(this.tpoBranches, this.session, true))
      {
        if (companySelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.txtBranch.Text = companySelectorForm.SelectedOrganization.OrganizationName;
        this.txtBranch.Tag = (object) companySelectorForm.SelectedOrganization;
        this.fetchTPOSettings();
        this.addLOusers();
      }
    }

    private void fetchTPOSettings()
    {
      string text1 = this.txtCompany.Text;
      string companyExteralID = "";
      string branchExteralID = "";
      string text2 = this.txtBranch.Text;
      string siteID = "";
      if (this.txtCompany.Tag != null)
        companyExteralID = ((ExternalOriginatorManagementData) this.txtCompany.Tag).ExternalID;
      if (this.txtBranch.Tag != null)
        branchExteralID = ((ExternalOriginatorManagementData) this.txtBranch.Tag).ExternalID;
      List<object> objectList = (List<object>) null;
      if (this.tpoBranches != null)
      {
        this.tpoBranches.Clear();
        this.tpoBranches = (List<ExternalOriginatorManagementData>) null;
      }
      if (this.tpoUsers != null)
        this.tpoUsers = (ExternalUserInfo[]) null;
      try
      {
        if (!string.IsNullOrEmpty(companyExteralID) || !string.IsNullOrEmpty(text1))
          objectList = this.session.ConfigurationManager.GetTPOInformationToolSettings(companyExteralID, text1, branchExteralID, text2, siteID, this.session.UserID);
        if (objectList != null)
        {
          for (int index = 0; index < objectList.Count; ++index)
          {
            if (objectList[index] is List<ExternalOriginatorManagementData>)
              this.tpoBranches = (List<ExternalOriginatorManagementData>) objectList[index];
            else if (objectList[index] is ExternalUserInfo[])
              this.tpoUsers = (ExternalUserInfo[]) objectList[index];
          }
        }
        if (this.tpoBranches == null || this.tpoBranches.Count < 1)
          return;
        this.btnSelectBranch.Enabled = true;
      }
      catch (Exception ex)
      {
        this.tpoBranches = (List<ExternalOriginatorManagementData>) null;
        this.tpoUsers = (ExternalUserInfo[]) null;
      }
    }

    private void clear()
    {
      this.btnSelectBranch.Enabled = false;
      this.cbLO.Enabled = false;
      this.txtBranch.Text = "";
      this.txtBranch.Tag = (object) null;
    }

    private void addLOusers()
    {
      string str1 = "";
      string str2 = "";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (this.txtCompany.Tag != null)
        str1 = ((ExternalOriginatorManagementData) this.txtCompany.Tag).ExternalID;
      if (this.txtBranch.Tag != null)
        str2 = ((ExternalOriginatorManagementData) this.txtBranch.Tag).ExternalID;
      if (this.tpoUsers != null && this.tpoUsers.Length != 0)
        dictionary = this.session.ConfigurationManager.GetTpoLoanOfficer(string.IsNullOrEmpty(str2) ? str1 : str2, 8);
      if (dictionary.Any<KeyValuePair<string, string>>())
      {
        this.cbLO.Enabled = true;
        this.cbLO.DataSource = (object) new BindingSource((object) dictionary, (string) null);
        this.cbLO.DisplayMember = "Value";
        this.cbLO.ValueMember = "Key";
      }
      else
      {
        this.cbLO.Enabled = false;
        this.cbLO.DataSource = (object) null;
      }
    }

    public ExternalOriginatorManagementData SelectedOrganization
    {
      get => (ExternalOriginatorManagementData) this.txtCompany.Tag;
    }

    public ExternalOriginatorManagementData SelectedBranch
    {
      get => (ExternalOriginatorManagementData) this.txtBranch.Tag;
    }

    public ExternalUserInfo getSelectedLO()
    {
      string selectedValue = (string) this.cbLO.SelectedValue;
      if (this.tpoUsers != null)
      {
        foreach (ExternalUserInfo tpoUser in this.tpoUsers)
        {
          if (tpoUser.ContactID.Equals(selectedValue))
            return tpoUser;
        }
      }
      return (ExternalUserInfo) null;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.SelectedOrganization == null && this.SelectedBranch == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a TPO company.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.txtCompany = new TextBox();
      this.txtBranch = new TextBox();
      this.btnSelectCompany = new StandardIconButton();
      this.btnSelectBranch = new StandardIconButton();
      this.cbLO = new ComboBox();
      ((ISupportInitialize) this.btnSelectCompany).BeginInit();
      ((ISupportInitialize) this.btnSelectBranch).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(25, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(51, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Company";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(25, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(41, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Branch";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(25, 84);
      this.label3.Name = "label3";
      this.label3.Size = new Size(65, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Loan Officer";
      this.button1.Enabled = false;
      this.button1.Location = new Point(146, 138);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(247, 138);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 4;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.txtCompany.Location = new Point(121, 19);
      this.txtCompany.Name = "txtCompany";
      this.txtCompany.ReadOnly = true;
      this.txtCompany.Size = new Size(156, 20);
      this.txtCompany.TabIndex = 5;
      this.txtBranch.Location = new Point(121, 48);
      this.txtBranch.Name = "txtBranch";
      this.txtBranch.ReadOnly = true;
      this.txtBranch.Size = new Size(156, 20);
      this.txtBranch.TabIndex = 6;
      this.btnSelectCompany.BackColor = Color.Transparent;
      this.btnSelectCompany.Location = new Point(294, 23);
      this.btnSelectCompany.MouseDownImage = (Image) null;
      this.btnSelectCompany.Name = "btnSelectCompany";
      this.btnSelectCompany.Size = new Size(16, 16);
      this.btnSelectCompany.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectCompany.TabIndex = 7;
      this.btnSelectCompany.TabStop = false;
      this.btnSelectCompany.Click += new EventHandler(this.btnSelectCompany_Click);
      this.btnSelectBranch.BackColor = Color.Transparent;
      this.btnSelectBranch.Enabled = false;
      this.btnSelectBranch.Location = new Point(294, 52);
      this.btnSelectBranch.MouseDownImage = (Image) null;
      this.btnSelectBranch.Name = "btnSelectBranch";
      this.btnSelectBranch.Size = new Size(16, 16);
      this.btnSelectBranch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectBranch.TabIndex = 8;
      this.btnSelectBranch.TabStop = false;
      this.btnSelectBranch.Click += new EventHandler(this.btnSelectBranch_Click);
      this.cbLO.Enabled = false;
      this.cbLO.FormattingEnabled = true;
      this.cbLO.Location = new Point(121, 76);
      this.cbLO.Name = "cbLO";
      this.cbLO.Size = new Size(156, 21);
      this.cbLO.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(385, 176);
      this.Controls.Add((Control) this.cbLO);
      this.Controls.Add((Control) this.btnSelectBranch);
      this.Controls.Add((Control) this.btnSelectCompany);
      this.Controls.Add((Control) this.txtBranch);
      this.Controls.Add((Control) this.txtCompany);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TPOLoanOfficerSelectorForm);
      this.ShowIcon = false;
      this.Text = "Search for Loan Officer";
      ((ISupportInitialize) this.btnSelectCompany).EndInit();
      ((ISupportInitialize) this.btnSelectBranch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
