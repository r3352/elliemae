// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ManuallyAdd
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class ManuallyAdd : Form
  {
    private LOCompHistoryControl loCompHistoryControl;
    private LOCompCurrentControl loCompCurrentControl;
    private Sessions.Session session;
    private int parent;
    private int depth;
    private int oid;
    private bool edit;
    private string hierarchyPath;
    private int compType;
    private bool forLender;
    private ExternalOriginatorManagementData externalContact;
    private Dictionary<int, string> alreadyExists = new Dictionary<int, string>();
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private LoanCompHistoryList loanCompHistoryList;
    private string originalZipCode = string.Empty;
    private IContainer components;
    private Label lblOriginatorName;
    private CheckBox chkBroker;
    private CheckBox chkCorrespondent;
    private Label lblName;
    private Label lblAddress;
    private Label lblCity;
    private Label lblState;
    private Label lblZip;
    private TextBox txtName;
    private TextBox txtAddress;
    private TextBox txtCity;
    private TextBox txtState;
    private TextBox txtZip;
    private Panel panelLOCompHistory;
    private Button btnOK;
    private Button btnCancel;
    private TextBox textDBA;
    private Label label1;
    private Panel panelLOCompCurrent;

    public Dictionary<int, string> HierarchyNodes => this.hierarchyNodes;

    public LoanCompHistoryList LOCompHistoryList => this.loanCompHistoryList;

    public ManuallyAdd(
      Sessions.Session session,
      int oid,
      int parent,
      int depth,
      string hierarchyPath,
      bool edit)
    {
      this.session = session;
      this.parent = parent;
      this.depth = depth;
      this.oid = oid;
      this.edit = edit;
      this.hierarchyPath = hierarchyPath;
      if (this.hierarchyPath.StartsWith("Lenders"))
        this.forLender = true;
      this.InitializeComponent();
      this.loCompHistoryControl = new LOCompHistoryControl(this.session, false, true, this.forLender);
      this.loCompCurrentControl = new LOCompCurrentControl(this.session, false, true, this.forLender);
      this.panelLOCompHistory.Controls.Add((Control) this.loCompHistoryControl);
      this.panelLOCompCurrent.Controls.Add((Control) this.loCompCurrentControl);
      if (edit)
      {
        this.populateFields(oid);
        this.loanCompHistoryList = this.session.ConfigurationManager.GetCompPlansByoid(this.forLender, oid);
        this.loanCompHistoryList.UseParentInfo = this.session.ConfigurationManager.GetUseParentInfoValue(this.forLender, oid);
      }
      else
      {
        this.loanCompHistoryList = new LoanCompHistoryList(oid.ToString());
        this.btnOK.Enabled = false;
      }
      this.loCompCurrentControl.RefreshDate(this.loanCompHistoryList, parent.ToString(), oid.ToString());
      this.loCompHistoryControl.RefreshData(this.loanCompHistoryList, parent.ToString(), oid.ToString());
      this.loCompHistoryControl.HistorySelectedIndexChanged += new EventHandler(this.loCompHistoryControl_HistorySelectedIndexChanged);
      this.loCompHistoryControl.UseParentInfoClicked += new EventHandler(this.loCompHistoryControl_UseParentInfoClicked);
      this.loCompHistoryControl.AssignPlanButtonClicked += new EventHandler(this.loCompHistoryControl_AssignPlanButtonClicked);
      this.loCompCurrentControl.StartDateChanged += new EventHandler(this.loCompCurrentControl_StartDateChanged);
      this.loCompHistoryControl.AutoSetGrid();
      this.loCompHistoryControl.DisableButtons(this.chkBroker.Checked);
    }

    public void populateFields(int oid)
    {
      this.externalContact = this.session.ConfigurationManager.GetByoid(this.forLender, oid);
      this.compType = (int) this.externalContact.entityType;
      if (this.externalContact.entityType == ExternalOriginatorEntityType.Both)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.Broker)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = false;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.Correspondent)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.None)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = false;
      }
      this.txtName.Text = this.externalContact.CompanyLegalName;
      this.textDBA.Text = this.externalContact.CompanyDBAName;
      this.txtAddress.Text = this.externalContact.Address;
      this.txtCity.Text = this.externalContact.City;
      this.txtState.Text = this.externalContact.State;
      this.txtZip.Text = this.externalContact.Zip;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      ExternalOriginatorEntityType entityType = !this.chkBroker.Checked || !this.chkCorrespondent.Checked ? (this.chkBroker.Checked ? ExternalOriginatorEntityType.Broker : (this.chkCorrespondent.Checked ? ExternalOriginatorEntityType.Correspondent : ExternalOriginatorEntityType.None)) : ExternalOriginatorEntityType.Both;
      if (this.edit)
      {
        if (this.textDBA.Text.Trim() == "")
          this.textDBA.Text = this.txtName.Text;
        if (this.externalContact != null && string.Compare(this.externalContact.CompanyLegalName, this.txtName.Text.Trim(), true) != 0)
        {
          List<ExternalOriginatorManagementData> contactByName = this.session.ConfigurationManager.GetContactByName(this.forLender, this.txtName.Text, false);
          if (contactByName != null && contactByName.Count > 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You already have company named '" + this.txtName.Text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.txtName.Focus();
            return;
          }
        }
        ExternalOriginatorManagementData byoid = this.session.ConfigurationManager.GetByoid(this.forLender, this.oid);
        byoid.CompanyDBAName = this.textDBA.Text;
        byoid.CompanyLegalName = this.txtName.Text;
        byoid.Address = this.txtAddress.Text;
        byoid.City = this.txtCity.Text;
        byoid.State = this.txtState.Text;
        byoid.Zip = this.txtZip.Text;
        byoid.entityType = entityType;
        this.session.ConfigurationManager.UpdateTPOContact(this.forLender, string.Concat((object) this.oid), byoid, this.loCompHistoryControl.GetUseParentInfo(), this.parent, this.depth, this.hierarchyPath, this.loanCompHistoryList);
      }
      else
      {
        List<ExternalOriginatorManagementData> contactByName = this.session.ConfigurationManager.GetContactByName(this.forLender, this.txtName.Text, false);
        if (contactByName != null && contactByName.Count > 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You already have company named '" + this.txtName.Text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtName.Focus();
          return;
        }
        this.oid = this.session.ConfigurationManager.AddManualContact(this.forLender, entityType, this.textDBA.Text, this.txtName.Text, this.txtAddress.Text, this.txtCity.Text, this.txtState.Text, this.txtZip.Text, this.loCompHistoryControl.GetUseParentInfo(), this.parent, this.depth, this.hierarchyPath + "\\" + this.txtName.Text, this.loanCompHistoryList);
      }
      this.hierarchyNodes.Add(this.oid, this.txtName.Text);
      this.DialogResult = DialogResult.OK;
    }

    private void loCompHistoryControl_HistorySelectedIndexChanged(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompHistoryControl_UseParentInfoClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompHistoryControl_AssignPlanButtonClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompCurrentControl_StartDateChanged(object sender, EventArgs e)
    {
      this.loCompHistoryControl.RefreshHistoryList((LoanCompHistory) sender);
    }

    private void zipcodeField_KeyUp(object sender, KeyEventArgs e)
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

    private void zipField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return;
      string str = textBox.Text.Trim();
      if (str.Length < 5 || this.originalZipCode == str && this.txtState.Text.Trim() != string.Empty && this.txtCity.Text.Trim() != string.Empty)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(str.Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(str.Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      this.txtCity.Text = zipCodeInfo.City;
      this.txtState.Text = zipCodeInfo.State.ToUpper();
    }

    private void CompType_CheckedChanged(object sender, EventArgs e)
    {
      this.compType = !this.chkBroker.Checked || !this.chkCorrespondent.Checked ? (!this.chkBroker.Checked ? (!this.chkCorrespondent.Checked ? 0 : 1) : 2) : 3;
      if (!this.chkBroker.Checked)
        this.loCompHistoryControl.DisableButtons(false);
      else
        this.loCompHistoryControl.DisableButtons(true);
    }

    private void companyName_TextChanged(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == "")
        this.btnOK.Enabled = false;
      else
        this.btnOK.Enabled = true;
    }

    private void companyName_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (e.KeyChar.Equals('\\'))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void txtZip_Enter(object sender, EventArgs e)
    {
      this.originalZipCode = this.txtZip.Text.Trim();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblOriginatorName = new Label();
      this.chkBroker = new CheckBox();
      this.chkCorrespondent = new CheckBox();
      this.lblName = new Label();
      this.lblAddress = new Label();
      this.lblCity = new Label();
      this.lblState = new Label();
      this.lblZip = new Label();
      this.txtName = new TextBox();
      this.txtAddress = new TextBox();
      this.txtCity = new TextBox();
      this.txtState = new TextBox();
      this.txtZip = new TextBox();
      this.panelLOCompHistory = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.textDBA = new TextBox();
      this.label1 = new Label();
      this.panelLOCompCurrent = new Panel();
      this.SuspendLayout();
      this.lblOriginatorName.AutoSize = true;
      this.lblOriginatorName.Location = new Point(6, 16);
      this.lblOriginatorName.Name = "lblOriginatorName";
      this.lblOriginatorName.Size = new Size(73, 13);
      this.lblOriginatorName.TabIndex = 0;
      this.lblOriginatorName.Text = "Channel Type";
      this.chkBroker.AutoSize = true;
      this.chkBroker.Location = new Point(123, 14);
      this.chkBroker.Name = "chkBroker";
      this.chkBroker.Size = new Size(57, 17);
      this.chkBroker.TabIndex = 2;
      this.chkBroker.Text = "Broker";
      this.chkBroker.UseVisualStyleBackColor = true;
      this.chkBroker.CheckedChanged += new EventHandler(this.CompType_CheckedChanged);
      this.chkCorrespondent.AutoSize = true;
      this.chkCorrespondent.Location = new Point(207, 14);
      this.chkCorrespondent.Name = "chkCorrespondent";
      this.chkCorrespondent.Size = new Size(95, 17);
      this.chkCorrespondent.TabIndex = 4;
      this.chkCorrespondent.Text = "Correspondent";
      this.chkCorrespondent.UseVisualStyleBackColor = true;
      this.chkCorrespondent.CheckedChanged += new EventHandler(this.CompType_CheckedChanged);
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(6, 38);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(111, 13);
      this.lblName.TabIndex = 7;
      this.lblName.Text = "Company Legal Name";
      this.lblAddress.AutoSize = true;
      this.lblAddress.Location = new Point(386, 16);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(45, 13);
      this.lblAddress.TabIndex = 11;
      this.lblAddress.Text = "Address";
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(386, 38);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(24, 13);
      this.lblCity.TabIndex = 13;
      this.lblCity.Text = "City";
      this.lblState.AutoSize = true;
      this.lblState.Location = new Point(386, 60);
      this.lblState.Name = "lblState";
      this.lblState.Size = new Size(32, 13);
      this.lblState.TabIndex = 14;
      this.lblState.Text = "State";
      this.lblZip.AutoSize = true;
      this.lblZip.Location = new Point(502, 60);
      this.lblZip.Name = "lblZip";
      this.lblZip.Size = new Size(22, 13);
      this.lblZip.TabIndex = 16;
      this.lblZip.Text = "Zip";
      this.txtName.Location = new Point(123, 35);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size((int) byte.MaxValue, 20);
      this.txtName.TabIndex = 8;
      this.txtName.TextChanged += new EventHandler(this.companyName_TextChanged);
      this.txtName.KeyPress += new KeyPressEventHandler(this.companyName_KeyPress);
      this.txtAddress.Location = new Point(437, 13);
      this.txtAddress.MaxLength = 100;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(375, 20);
      this.txtAddress.TabIndex = 12;
      this.txtCity.Location = new Point(437, 35);
      this.txtCity.MaxLength = 100;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(375, 20);
      this.txtCity.TabIndex = 14;
      this.txtState.Location = new Point(437, 57);
      this.txtState.MaxLength = 3;
      this.txtState.Name = "txtState";
      this.txtState.Size = new Size(41, 20);
      this.txtState.TabIndex = 15;
      this.txtZip.Location = new Point(530, 57);
      this.txtZip.MaxLength = 15;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(87, 20);
      this.txtZip.TabIndex = 17;
      this.txtZip.Tag = (object) "2284";
      this.txtZip.Enter += new EventHandler(this.txtZip_Enter);
      this.txtZip.KeyUp += new KeyEventHandler(this.zipcodeField_KeyUp);
      this.txtZip.Leave += new EventHandler(this.zipField_Leave);
      this.panelLOCompHistory.Location = new Point(386, 87);
      this.panelLOCompHistory.Name = "panelLOCompHistory";
      this.panelLOCompHistory.Size = new Size(426, 190);
      this.panelLOCompHistory.TabIndex = 19;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(656, 287);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 20;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(737, 287);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.textDBA.Location = new Point(123, 57);
      this.textDBA.MaxLength = 64;
      this.textDBA.Name = "textDBA";
      this.textDBA.Size = new Size((int) byte.MaxValue, 20);
      this.textDBA.TabIndex = 10;
      this.textDBA.TextChanged += new EventHandler(this.companyName_TextChanged);
      this.textDBA.KeyPress += new KeyPressEventHandler(this.companyName_KeyPress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 60);
      this.label1.Name = "label1";
      this.label1.Size = new Size(107, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Company DBA Name";
      this.panelLOCompCurrent.Location = new Point(9, 87);
      this.panelLOCompCurrent.Name = "panelLOCompCurrent";
      this.panelLOCompCurrent.Size = new Size(369, 190);
      this.panelLOCompCurrent.TabIndex = 18;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(820, 316);
      this.Controls.Add((Control) this.panelLOCompCurrent);
      this.Controls.Add((Control) this.textDBA);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panelLOCompHistory);
      this.Controls.Add((Control) this.txtZip);
      this.Controls.Add((Control) this.txtState);
      this.Controls.Add((Control) this.txtCity);
      this.Controls.Add((Control) this.txtAddress);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblZip);
      this.Controls.Add((Control) this.lblState);
      this.Controls.Add((Control) this.lblCity);
      this.Controls.Add((Control) this.lblAddress);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.chkCorrespondent);
      this.Controls.Add((Control) this.chkBroker);
      this.Controls.Add((Control) this.lblOriginatorName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ManuallyAdd);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Company Details";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
