// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingExceptionResolver
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingExceptionResolver : Form
  {
    private int settingTypeId = -1;
    private string oids = string.Empty;
    private string contactIds = string.Empty;
    private string settingTypeKey = string.Empty;
    private List<ExternalSettingValue> settingValues;
    private ExternalSettingValue externalSettingValue;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcExceptions;
    private GridView gvExceptions;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox cboSettingValues;
    private Label lblHeading;
    private Label lblSettingDropdown;

    public SettingExceptionResolver() => this.InitializeComponent();

    public SettingExceptionResolver(
      Sessions.Session session,
      int settingTypeId,
      string settingTypeKey,
      List<ExternalSettingValue> settingValues,
      ExternalSettingValue externalSettingValue,
      List<ExternalOriginatorManagementData> externalOrganizations,
      List<ExternalUserInfo> externalContacts)
    {
      this.InitializeComponent();
      this.settingTypeId = settingTypeId;
      this.settingTypeKey = settingTypeKey;
      this.session = session;
      this.externalSettingValue = externalSettingValue;
      this.settingValues = settingValues;
      switch (settingTypeId)
      {
        case 1:
        case 3:
        case 5:
          foreach (ExternalOriginatorManagementData externalOrganization in externalOrganizations)
          {
            this.gvExceptions.Items.Add(new GVItem(externalOrganization.OrganizationName)
            {
              Tag = (object) externalOrganization
            });
            this.oids = !(this.oids == string.Empty) ? this.oids + "," + externalOrganization.oid.ToString() : externalOrganization.oid.ToString();
          }
          this.gvExceptions.Columns.Add(new GVColumn("columnCompanyName"));
          this.gvExceptions.Columns[0].Width = 495;
          this.gvExceptions.Columns[0].Text = "Company/Branch";
          this.gcExceptions.Text = "Companies/Branches using setting - " + externalSettingValue.settingValue;
          break;
        case 2:
          foreach (ExternalUserInfo externalContact in externalContacts)
          {
            this.gvExceptions.Items.Add(new GVItem(externalContact.FirstName)
            {
              SubItems = {
                (object) externalContact.LastName,
                (object) externalContact.Title,
                (object) externalContact.Phone,
                (object) externalContact.Email
              },
              Tag = (object) externalContact
            });
            this.contactIds = !(this.contactIds == string.Empty) ? this.contactIds + "," + externalContact.ContactID.ToString() : externalContact.ContactID.ToString();
          }
          this.gvExceptions.Columns.Add(new GVColumn("columnFirstName"));
          this.gvExceptions.Columns.Add(new GVColumn("columnLastName"));
          this.gvExceptions.Columns.Add(new GVColumn("columnTitle"));
          this.gvExceptions.Columns.Add(new GVColumn("columnPhone"));
          this.gvExceptions.Columns.Add(new GVColumn("columnEmail"));
          this.gvExceptions.Columns[0].Width = 100;
          this.gvExceptions.Columns[1].Width = 100;
          this.gvExceptions.Columns[2].Width = 100;
          this.gvExceptions.Columns[3].Width = 95;
          this.gvExceptions.Columns[4].Width = 100;
          this.gvExceptions.Columns[0].Text = "First Name";
          this.gvExceptions.Columns[1].Text = "Last Name";
          this.gvExceptions.Columns[2].Text = "Title";
          this.gvExceptions.Columns[3].Text = "Phone";
          this.gvExceptions.Columns[4].Text = "Email";
          this.gcExceptions.Text = "Contacts using setting - " + externalSettingValue.settingValue;
          break;
      }
      if (settingValues != null && settingValues.Count > 0)
      {
        settingValues.Add(new ExternalSettingValue(-1, -1, "", "", 0));
        settingValues.Remove(externalSettingValue);
        this.cboSettingValues.DataSource = (object) settingValues;
        this.cboSettingValues.DisplayMember = "settingValue";
        this.cboSettingValues.ValueMember = "settingId";
        this.cboSettingValues.SelectedIndex = settingValues.Count - 1;
      }
      this.cboSettingValues.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.session.ConfigurationManager.AssignNewSettingValueToExternalOrg(((ExternalSettingValue) this.cboSettingValues.SelectedItem).settingId, this.settingTypeId, this.oids);
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.gcExceptions = new GroupContainer();
      this.gvExceptions = new GridView();
      this.cboSettingValues = new ComboBox();
      this.lblHeading = new Label();
      this.lblSettingDropdown = new Label();
      this.gcExceptions.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(438, 395);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(343, 395);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.gcExceptions.Controls.Add((Control) this.gvExceptions);
      this.gcExceptions.HeaderForeColor = SystemColors.ControlText;
      this.gcExceptions.Location = new Point(14, 57);
      this.gcExceptions.Name = "gcExceptions";
      this.gcExceptions.Size = new Size(501, 270);
      this.gcExceptions.TabIndex = 1;
      this.gcExceptions.Text = "Exceptions";
      this.gvExceptions.AllowMultiselect = false;
      this.gvExceptions.BorderStyle = BorderStyle.None;
      this.gvExceptions.Dock = DockStyle.Fill;
      this.gvExceptions.Location = new Point(1, 26);
      this.gvExceptions.Name = "gvExceptions";
      this.gvExceptions.Size = new Size(499, 243);
      this.gvExceptions.SortOption = GVSortOption.None;
      this.gvExceptions.TabIndex = 1;
      this.cboSettingValues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSettingValues.FormattingEnabled = true;
      this.cboSettingValues.Location = new Point(87, 353);
      this.cboSettingValues.Name = "cboSettingValues";
      this.cboSettingValues.Size = new Size(183, 21);
      this.cboSettingValues.TabIndex = 31;
      this.lblHeading.Location = new Point(12, 9);
      this.lblHeading.Name = "lblHeading";
      this.lblHeading.Size = new Size(503, 27);
      this.lblHeading.TabIndex = 36;
      this.lblHeading.Text = "Setting selected for the deletion has been used by the following companies/branches. Please select the new setting value from the dropdown, to be assigned to these companies/branches. ";
      this.lblSettingDropdown.Location = new Point(11, 356);
      this.lblSettingDropdown.Name = "lblSettingDropdown";
      this.lblSettingDropdown.Size = new Size(70, 21);
      this.lblSettingDropdown.TabIndex = 37;
      this.lblSettingDropdown.Text = "Setting Value";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(538, 430);
      this.Controls.Add((Control) this.lblSettingDropdown);
      this.Controls.Add((Control) this.lblHeading);
      this.Controls.Add((Control) this.cboSettingValues);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gcExceptions);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SettingExceptionResolver);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "TPO Setting Exception";
      this.gcExceptions.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
