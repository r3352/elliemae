// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.DupContactDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class DupContactDialog : Form
  {
    private Label label1;
    private Label label2;
    private Button btnYes;
    private Button btnYesAll;
    private Button btnNo;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private Button btnSkip;
    private Label label3;
    private Label label4;
    private Label lblBdayOrCompanyOld;
    private Label lblSsnOrTitleOld;
    private Label lblSsnOrTitleNew;
    private Label lblBdayOrCompanyNew;
    private Label label15;
    private Label label16;
    private Label lblNameOld;
    private Label lblAddressOld;
    private Label lblBdayOld;
    private Label lblSsnOld;
    private Label lblBdayNew;
    private Label lblAddressNew;
    private Label lblNameNew;
    private Label lblSsnNew;
    private Panel panel1;
    private Panel panel2;
    private Label label5;
    private PictureBox pictureBoxBor1;
    private PictureBox pictureBoxBor2;
    private PictureBox pictureBoxBiz1;
    private PictureBox pictureBoxBiz2;
    private ContactImportDupOption dupOption = ContactImportDupOption.Replace;
    private bool globalDuplicate;
    private int LEN = 40;

    public ContactImportDupOption DupOption => this.dupOption;

    public DupContactDialog(BorrowerInfo oldContact, BorrowerInfo newContact)
    {
      this.InitializeComponent();
      this.pictureBoxBor1.Visible = true;
      this.pictureBoxBor2.Visible = true;
      this.pictureBoxBiz1.Visible = false;
      this.pictureBoxBiz2.Visible = false;
      this.lblNameOld.Text = this.chopString(oldContact.FirstName + " " + oldContact.LastName, this.LEN);
      this.lblAddressOld.Text = this.getAddress(oldContact.HomeAddress);
      this.lblBdayOld.Text = oldContact.Birthdate == DateTime.MinValue ? string.Empty : oldContact.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "\n";
      this.lblSsnOld.Text = oldContact.SSN;
      this.lblNameNew.Text = this.chopString(newContact.FirstName + " " + newContact.LastName, this.LEN);
      this.lblAddressNew.Text = this.getAddress(newContact.HomeAddress);
      this.lblBdayNew.Text = newContact.Birthdate == DateTime.MinValue ? string.Empty : newContact.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "\n";
      this.lblSsnNew.Text = newContact.SSN;
    }

    public DupContactDialog(BizPartnerInfo oldContact, BizPartnerInfo newContact)
    {
      this.InitializeComponent();
      this.lblBdayOrCompanyOld.Text = "Company:";
      this.lblBdayOrCompanyNew.Text = "Company:";
      this.lblSsnOrTitleNew.Text = "Title: ";
      this.lblSsnOrTitleOld.Text = "Title: ";
      this.pictureBoxBor1.Visible = false;
      this.pictureBoxBor2.Visible = false;
      this.pictureBoxBiz1.Visible = true;
      this.pictureBoxBiz2.Visible = true;
      this.lblNameOld.Text = this.chopString(oldContact.FirstName + " " + oldContact.LastName, this.LEN);
      this.lblAddressOld.Text = this.getAddress(oldContact.BizAddress);
      this.lblBdayOld.Text = this.chopString(oldContact.CompanyName, this.LEN);
      this.lblSsnOld.Text = this.chopString(oldContact.JobTitle, this.LEN);
      this.lblNameNew.Text = this.chopString(newContact.FirstName + " " + newContact.LastName, this.LEN);
      this.lblAddressNew.Text = this.getAddress(newContact.BizAddress);
      this.lblBdayNew.Text = this.chopString(newContact.CompanyName, this.LEN);
      this.lblSsnNew.Text = this.chopString(newContact.JobTitle, this.LEN);
    }

    public DupContactDialog(BorrowerInfo oldContact, BorrowerInfo newContact, bool GlobalDuplicate)
    {
      this.InitializeComponent();
      this.pictureBoxBor1.Visible = true;
      this.pictureBoxBor2.Visible = true;
      this.pictureBoxBiz1.Visible = false;
      this.pictureBoxBiz2.Visible = false;
      this.label2.Visible = false;
      this.btnNo.Enabled = false;
      this.Text = "Confirm Contact Creation";
      this.label5.Text = "This contact already exists in Encompass, it is owned by another user.";
      this.label1.Text = "Do you still want to create this contact?  You will be assigned as the owner.";
      this.globalDuplicate = true;
      this.lblNameOld.Text = this.chopString(oldContact.FirstName + " " + oldContact.LastName, this.LEN);
      this.lblAddressOld.Text = this.getAddress(oldContact.HomeAddress);
      this.lblBdayOld.Text = oldContact.Birthdate == DateTime.MinValue ? string.Empty : oldContact.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "\n";
      this.lblSsnOld.Text = oldContact.SSN;
      this.lblNameNew.Text = this.chopString(newContact.FirstName + " " + newContact.LastName, this.LEN);
      this.lblAddressNew.Text = this.getAddress(newContact.HomeAddress);
      this.lblBdayNew.Text = newContact.Birthdate == DateTime.MinValue ? string.Empty : newContact.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "\n";
      this.lblSsnNew.Text = newContact.SSN;
    }

    public DupContactDialog(
      ExternalOriginatorManagementData oldTPOCompany,
      ExternalOriginatorManagementData newTPOCompany)
    {
      this.InitializeComponent();
      this.pictureBoxBor1.Visible = true;
      this.pictureBoxBor2.Visible = true;
      this.pictureBoxBiz1.Visible = false;
      this.pictureBoxBiz2.Visible = false;
      this.lblBdayOrCompanyOld.Visible = this.lblBdayOld.Visible = this.lblBdayOrCompanyNew.Visible = this.lblBdayNew.Visible = false;
      this.lblSsnOrTitleOld.Top = this.lblSsnOld.Top = this.lblBdayOrCompanyOld.Top;
      this.lblSsnOrTitleNew.Top = this.lblSsnNew.Top = this.lblBdayOrCompanyNew.Top;
      this.label2.Visible = false;
      this.btnNo.Enabled = false;
      this.Text = "Confirm TPO Organization Creation";
      this.label5.Text = "This TPO Organization already exists in Encompass.";
      this.label1.Text = "Do you still want to create this organization in TPO Management?";
      this.globalDuplicate = true;
      this.lblNameOld.Text = oldTPOCompany.CompanyLegalName;
      this.lblAddressOld.Text = this.getAddress(new Address(oldTPOCompany.Address, string.Empty, oldTPOCompany.City, oldTPOCompany.State, oldTPOCompany.Zip));
      this.lblNameNew.Text = newTPOCompany.CompanyLegalName;
      this.lblAddressNew.Text = this.getAddress(new Address(newTPOCompany.Address, string.Empty, newTPOCompany.City, newTPOCompany.State, newTPOCompany.Zip));
      this.dupOption = ContactImportDupOption.Skip;
    }

    public DupContactDialog(ExternalUserInfo oldTPOUser, ExternalUserInfo newTPOUser)
    {
      this.InitializeComponent();
      this.pictureBoxBor1.Visible = true;
      this.pictureBoxBor2.Visible = true;
      this.pictureBoxBiz1.Visible = false;
      this.pictureBoxBiz2.Visible = false;
      this.lblBdayOrCompanyOld.Visible = this.lblBdayOld.Visible = this.lblBdayOrCompanyNew.Visible = this.lblBdayNew.Visible = false;
      this.lblSsnOrTitleOld.Top = this.lblSsnOld.Top = this.lblBdayOrCompanyOld.Top;
      this.lblSsnOrTitleNew.Top = this.lblSsnNew.Top = this.lblBdayOrCompanyNew.Top;
      this.label2.Visible = false;
      this.btnNo.Enabled = false;
      this.Text = "Confirm TPO Contact Creation";
      this.label5.Text = "This TPO Contact already exists in Encompass.";
      this.label1.Text = "Do you still want to create this contact in TPO Management?.";
      this.globalDuplicate = true;
      this.lblNameOld.Text = this.chopString(oldTPOUser.FirstName + " " + oldTPOUser.LastName, this.LEN);
      this.lblAddressOld.Text = this.getAddress(new Address(oldTPOUser.Address, string.Empty, oldTPOUser.City, oldTPOUser.State, oldTPOUser.Zipcode));
      this.lblSsnOld.Text = oldTPOUser.SSN;
      this.lblNameNew.Text = this.chopString(newTPOUser.FirstName + " " + newTPOUser.LastName, this.LEN);
      this.lblAddressNew.Text = this.getAddress(new Address(newTPOUser.Address, string.Empty, newTPOUser.City, newTPOUser.State, newTPOUser.Zipcode));
      this.lblSsnNew.Text = newTPOUser.SSN;
      this.dupOption = ContactImportDupOption.Skip;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DupContactDialog));
      this.btnYes = new Button();
      this.btnYesAll = new Button();
      this.btnNo = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.pictureBoxBor1 = new PictureBox();
      this.pictureBoxBor2 = new PictureBox();
      this.btnSkip = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.lblBdayOrCompanyOld = new Label();
      this.lblSsnOrTitleOld = new Label();
      this.lblNameOld = new Label();
      this.lblAddressOld = new Label();
      this.lblBdayOld = new Label();
      this.lblSsnOld = new Label();
      this.lblSsnNew = new Label();
      this.lblBdayNew = new Label();
      this.lblAddressNew = new Label();
      this.lblNameNew = new Label();
      this.lblSsnOrTitleNew = new Label();
      this.lblBdayOrCompanyNew = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.panel1 = new Panel();
      this.pictureBoxBiz1 = new PictureBox();
      this.panel2 = new Panel();
      this.pictureBoxBiz2 = new PictureBox();
      this.label5 = new Label();
      ((ISupportInitialize) this.pictureBoxBor1).BeginInit();
      ((ISupportInitialize) this.pictureBoxBor2).BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxBiz1).BeginInit();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxBiz2).BeginInit();
      this.SuspendLayout();
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(64, 329);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(72, 24);
      this.btnYes.TabIndex = 0;
      this.btnYes.Text = "Yes";
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.btnYesAll.DialogResult = DialogResult.OK;
      this.btnYesAll.Location = new Point(140, 329);
      this.btnYesAll.Name = "btnYesAll";
      this.btnYesAll.Size = new Size(72, 24);
      this.btnYesAll.TabIndex = 1;
      this.btnYesAll.Text = "Yes to All";
      this.btnYesAll.Click += new EventHandler(this.btnYesAll_Click);
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(216, 329);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(72, 24);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "No, Create";
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(368, 329);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 24);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Stop Import";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label1.Location = new Point(16, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(384, 15);
      this.label1.TabIndex = 4;
      this.label1.Text = "Would you like to replace the existing contact";
      this.label2.Location = new Point(13, 182);
      this.label2.Name = "label2";
      this.label2.Size = new Size(384, 20);
      this.label2.TabIndex = 5;
      this.label2.Text = "with this one?";
      this.pictureBoxBor1.Image = (Image) componentResourceManager.GetObject("pictureBoxBor1.Image");
      this.pictureBoxBor1.Location = new Point(8, 8);
      this.pictureBoxBor1.Name = "pictureBoxBor1";
      this.pictureBoxBor1.Size = new Size(28, 28);
      this.pictureBoxBor1.TabIndex = 6;
      this.pictureBoxBor1.TabStop = false;
      this.pictureBoxBor2.Image = (Image) componentResourceManager.GetObject("pictureBoxBor2.Image");
      this.pictureBoxBor2.Location = new Point(8, 8);
      this.pictureBoxBor2.Name = "pictureBoxBor2";
      this.pictureBoxBor2.Size = new Size(28, 28);
      this.pictureBoxBor2.TabIndex = 8;
      this.pictureBoxBor2.TabStop = false;
      this.btnSkip.DialogResult = DialogResult.Ignore;
      this.btnSkip.Location = new Point(292, 329);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new Size(72, 24);
      this.btnSkip.TabIndex = 11;
      this.btnSkip.Text = "No, Skip";
      this.btnSkip.Click += new EventHandler(this.btnSkip_Click);
      this.label3.Location = new Point(48, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 16);
      this.label3.TabIndex = 12;
      this.label3.Text = "Name:";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.Location = new Point(48, 28);
      this.label4.Name = "label4";
      this.label4.Size = new Size(56, 16);
      this.label4.TabIndex = 13;
      this.label4.Text = "Address:";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBdayOrCompanyOld.Location = new Point(48, 72);
      this.lblBdayOrCompanyOld.Name = "lblBdayOrCompanyOld";
      this.lblBdayOrCompanyOld.Size = new Size(56, 16);
      this.lblBdayOrCompanyOld.TabIndex = 14;
      this.lblBdayOrCompanyOld.Text = "Birthday:";
      this.lblBdayOrCompanyOld.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSsnOrTitleOld.Location = new Point(48, 92);
      this.lblSsnOrTitleOld.Name = "lblSsnOrTitleOld";
      this.lblSsnOrTitleOld.Size = new Size(56, 16);
      this.lblSsnOrTitleOld.TabIndex = 15;
      this.lblSsnOrTitleOld.Text = "SSN:";
      this.lblSsnOrTitleOld.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNameOld.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNameOld.Location = new Point(112, 8);
      this.lblNameOld.Name = "lblNameOld";
      this.lblNameOld.Size = new Size(304, 16);
      this.lblNameOld.TabIndex = 16;
      this.lblAddressOld.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAddressOld.Location = new Point(112, 28);
      this.lblAddressOld.Name = "lblAddressOld";
      this.lblAddressOld.Size = new Size(304, 40);
      this.lblAddressOld.TabIndex = 17;
      this.lblBdayOld.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblBdayOld.Location = new Point(112, 72);
      this.lblBdayOld.Name = "lblBdayOld";
      this.lblBdayOld.Size = new Size(304, 16);
      this.lblBdayOld.TabIndex = 18;
      this.lblSsnOld.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSsnOld.Location = new Point(112, 92);
      this.lblSsnOld.Name = "lblSsnOld";
      this.lblSsnOld.Size = new Size(304, 16);
      this.lblSsnOld.TabIndex = 19;
      this.lblSsnNew.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSsnNew.Location = new Point(112, 92);
      this.lblSsnNew.Name = "lblSsnNew";
      this.lblSsnNew.Size = new Size(304, 16);
      this.lblSsnNew.TabIndex = 27;
      this.lblBdayNew.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblBdayNew.Location = new Point(112, 72);
      this.lblBdayNew.Name = "lblBdayNew";
      this.lblBdayNew.Size = new Size(304, 16);
      this.lblBdayNew.TabIndex = 26;
      this.lblAddressNew.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAddressNew.Location = new Point(112, 28);
      this.lblAddressNew.Name = "lblAddressNew";
      this.lblAddressNew.Size = new Size(304, 40);
      this.lblAddressNew.TabIndex = 25;
      this.lblNameNew.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNameNew.Location = new Point(112, 8);
      this.lblNameNew.Name = "lblNameNew";
      this.lblNameNew.Size = new Size(304, 16);
      this.lblNameNew.TabIndex = 24;
      this.lblSsnOrTitleNew.Location = new Point(48, 92);
      this.lblSsnOrTitleNew.Name = "lblSsnOrTitleNew";
      this.lblSsnOrTitleNew.Size = new Size(56, 16);
      this.lblSsnOrTitleNew.TabIndex = 23;
      this.lblSsnOrTitleNew.Text = "SSN:";
      this.lblSsnOrTitleNew.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBdayOrCompanyNew.Location = new Point(48, 72);
      this.lblBdayOrCompanyNew.Name = "lblBdayOrCompanyNew";
      this.lblBdayOrCompanyNew.Size = new Size(56, 16);
      this.lblBdayOrCompanyNew.TabIndex = 22;
      this.lblBdayOrCompanyNew.Text = "Birthday:";
      this.lblBdayOrCompanyNew.TextAlign = ContentAlignment.MiddleLeft;
      this.label15.Location = new Point(48, 28);
      this.label15.Name = "label15";
      this.label15.Size = new Size(56, 16);
      this.label15.TabIndex = 21;
      this.label15.Text = "Address:";
      this.label15.TextAlign = ContentAlignment.MiddleLeft;
      this.label16.Location = new Point(48, 8);
      this.label16.Name = "label16";
      this.label16.Size = new Size(56, 16);
      this.label16.TabIndex = 20;
      this.label16.Text = "Name:";
      this.label16.TextAlign = ContentAlignment.MiddleLeft;
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.pictureBoxBiz1);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.lblBdayOrCompanyOld);
      this.panel1.Controls.Add((Control) this.lblSsnOrTitleOld);
      this.panel1.Controls.Add((Control) this.lblNameOld);
      this.panel1.Controls.Add((Control) this.lblAddressOld);
      this.panel1.Controls.Add((Control) this.lblBdayOld);
      this.panel1.Controls.Add((Control) this.lblSsnOld);
      this.panel1.Controls.Add((Control) this.pictureBoxBor1);
      this.panel1.Location = new Point(12, 52);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(428, 118);
      this.panel1.TabIndex = 28;
      this.pictureBoxBiz1.Image = (Image) componentResourceManager.GetObject("pictureBoxBiz1.Image");
      this.pictureBoxBiz1.Location = new Point(8, 8);
      this.pictureBoxBiz1.Name = "pictureBoxBiz1";
      this.pictureBoxBiz1.Size = new Size(28, 28);
      this.pictureBoxBiz1.TabIndex = 20;
      this.pictureBoxBiz1.TabStop = false;
      this.panel2.BorderStyle = BorderStyle.Fixed3D;
      this.panel2.Controls.Add((Control) this.pictureBoxBiz2);
      this.panel2.Controls.Add((Control) this.lblSsnNew);
      this.panel2.Controls.Add((Control) this.lblBdayNew);
      this.panel2.Controls.Add((Control) this.lblAddressNew);
      this.panel2.Controls.Add((Control) this.lblNameNew);
      this.panel2.Controls.Add((Control) this.lblSsnOrTitleNew);
      this.panel2.Controls.Add((Control) this.lblBdayOrCompanyNew);
      this.panel2.Controls.Add((Control) this.label15);
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.pictureBoxBor2);
      this.panel2.Location = new Point(12, 200);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(428, 116);
      this.panel2.TabIndex = 29;
      this.pictureBoxBiz2.Image = (Image) componentResourceManager.GetObject("pictureBoxBiz2.Image");
      this.pictureBoxBiz2.Location = new Point(8, 8);
      this.pictureBoxBiz2.Name = "pictureBoxBiz2";
      this.pictureBoxBiz2.Size = new Size(28, 28);
      this.pictureBoxBiz2.TabIndex = 28;
      this.pictureBoxBiz2.TabStop = false;
      this.label5.Location = new Point(13, 4);
      this.label5.Name = "label5";
      this.label5.Size = new Size(427, 31);
      this.label5.TabIndex = 30;
      this.label5.Text = "This contact already exists.";
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(450, 364);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYesAll);
      this.Controls.Add((Control) this.btnYes);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (DupContactDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Confirm Contact Replace";
      ((ISupportInitialize) this.pictureBoxBor1).EndInit();
      ((ISupportInitialize) this.pictureBoxBor2).EndInit();
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBoxBiz1).EndInit();
      this.panel2.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBoxBiz2).EndInit();
      this.ResumeLayout(false);
    }

    private string chopString(string strValue, int len)
    {
      return strValue.Length < len ? strValue : strValue.Substring(0, len - 3) + "...";
    }

    private string getAddress(Address addr)
    {
      return this.chopString(addr.Street1.Trim(), this.LEN) + "\n" + (addr.Street2.Trim() != string.Empty ? this.chopString(addr.Street2.Trim(), this.LEN) + "\n" : "") + addr.City + (addr.City.Trim() != string.Empty ? ", " : "") + addr.State + " " + addr.Zip;
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      if (!this.globalDuplicate)
        this.dupOption = ContactImportDupOption.Replace;
      else
        this.dupOption = ContactImportDupOption.CreateNew;
    }

    private void btnYesAll_Click(object sender, EventArgs e)
    {
      if (!this.globalDuplicate)
        this.dupOption = ContactImportDupOption.ReplaceAll;
      else
        this.dupOption = ContactImportDupOption.CreateNewAll;
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.dupOption = ContactImportDupOption.CreateNew;
    }

    private void btnSkip_Click(object sender, EventArgs e)
    {
      this.dupOption = ContactImportDupOption.Skip;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.dupOption = ContactImportDupOption.Abort;
    }
  }
}
