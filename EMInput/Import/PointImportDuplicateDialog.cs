// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointImportDuplicateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Contacts;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointImportDuplicateDialog : Form
  {
    private DuplicateOption selectedOption;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label lblName;
    private Label lblCompany;
    private Label lblAddr;
    private Label lblCityStateZip;
    private Label lblPhone;
    private Label label6;
    private Button btnOverwrite;
    private Button btnNew;
    private Button btnCancel;
    private Button btnSkip;
    private CheckBox chkApplyToAll;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;

    public PointImportDuplicateDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (PointImportDuplicateDialog));
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.lblName = new Label();
      this.lblCompany = new Label();
      this.lblAddr = new Label();
      this.lblCityStateZip = new Label();
      this.lblPhone = new Label();
      this.label6 = new Label();
      this.btnOverwrite = new Button();
      this.btnNew = new Button();
      this.btnCancel = new Button();
      this.btnSkip = new Button();
      this.chkApplyToAll = new CheckBox();
      this.pictureBox1 = new PictureBox();
      this.SuspendLayout();
      this.label1.Location = new Point(82, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(360, 23);
      this.label1.TabIndex = 0;
      this.label1.Text = "A possible duplicate contact has been detected.";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(102, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 18);
      this.label2.TabIndex = 1;
      this.label2.Text = "Name:";
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(102, 64);
      this.label3.Name = "label3";
      this.label3.Size = new Size(62, 18);
      this.label3.TabIndex = 2;
      this.label3.Text = "Company:";
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(102, 84);
      this.label4.Name = "label4";
      this.label4.Size = new Size(62, 18);
      this.label4.TabIndex = 3;
      this.label4.Text = "Address:";
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(102, 124);
      this.label5.Name = "label5";
      this.label5.Size = new Size(63, 18);
      this.label5.TabIndex = 4;
      this.label5.Text = "Phone:";
      this.lblName.Location = new Point(168, 44);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(275, 18);
      this.lblName.TabIndex = 5;
      this.lblCompany.Location = new Point(168, 64);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(275, 18);
      this.lblCompany.TabIndex = 6;
      this.lblAddr.Location = new Point(168, 84);
      this.lblAddr.Name = "lblAddr";
      this.lblAddr.Size = new Size(275, 18);
      this.lblAddr.TabIndex = 7;
      this.lblCityStateZip.Location = new Point(168, 104);
      this.lblCityStateZip.Name = "lblCityStateZip";
      this.lblCityStateZip.Size = new Size(275, 18);
      this.lblCityStateZip.TabIndex = 8;
      this.lblPhone.Location = new Point(168, 124);
      this.lblPhone.Name = "lblPhone";
      this.lblPhone.Size = new Size(275, 18);
      this.lblPhone.TabIndex = 9;
      this.label6.Location = new Point(82, 152);
      this.label6.Name = "label6";
      this.label6.Size = new Size(360, 34);
      this.label6.TabIndex = 10;
      this.label6.Text = "You may choose to overwrite the existing contact, create a new contact or skip this contact. Press Cancel to abort the import process.";
      this.btnOverwrite.Location = new Point(106, 218);
      this.btnOverwrite.Name = "btnOverwrite";
      this.btnOverwrite.TabIndex = 11;
      this.btnOverwrite.Text = "&Overwrite";
      this.btnOverwrite.Click += new EventHandler(this.btnOverwrite_Click);
      this.btnNew.Location = new Point(188, 218);
      this.btnNew.Name = "btnNew";
      this.btnNew.TabIndex = 12;
      this.btnNew.Text = "&New";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(352, 218);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "&Cancel";
      this.btnSkip.Location = new Point(270, 218);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.TabIndex = 14;
      this.btnSkip.Text = "&Skip";
      this.btnSkip.Click += new EventHandler(this.btnSkip_Click);
      this.chkApplyToAll.Location = new Point(168, 188);
      this.chkApplyToAll.Name = "chkApplyToAll";
      this.chkApplyToAll.Size = new Size(184, 18);
      this.chkApplyToAll.TabIndex = 15;
      this.chkApplyToAll.Text = "Apply to All Potential Duplicates";
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(22, 14);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 16;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnOverwrite;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(460, 256);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.chkApplyToAll);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.btnOverwrite);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.lblPhone);
      this.Controls.Add((Control) this.lblCityStateZip);
      this.Controls.Add((Control) this.lblAddr);
      this.Controls.Add((Control) this.lblCompany);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PointImportDuplicateDialog);
      this.Text = "Contact Import";
      this.ResumeLayout(false);
    }

    public DuplicateOption ShowOptions(BizPartnerInfo info)
    {
      if (this.chkApplyToAll.Checked)
        return this.selectedOption;
      this.lblName.Text = info.FirstName + " " + info.LastName;
      this.lblCompany.Text = info.CompanyName;
      this.lblAddr.Text = info.BizAddress.Street1;
      this.lblCityStateZip.Text = info.BizAddress.City + ", " + info.BizAddress.State + " " + info.BizAddress.Zip;
      this.lblPhone.Text = info.WorkPhone;
      if (this.lblCompany.Text == "")
        this.lblCompany.Text = "(Not specified)";
      if (this.lblAddr.Text == "")
        this.lblAddr.Text = "(Not specified)";
      if (this.lblPhone.Text == "")
        this.lblPhone.Text = "(Not specified)";
      return this.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel ? DuplicateOption.Cancel : this.selectedOption;
    }

    private void btnOverwrite_Click(object sender, EventArgs e)
    {
      this.selectedOption = DuplicateOption.Overwrite;
      this.DialogResult = DialogResult.OK;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.selectedOption = DuplicateOption.New;
      this.DialogResult = DialogResult.OK;
    }

    private void btnSkip_Click(object sender, EventArgs e)
    {
      this.selectedOption = DuplicateOption.Skip;
      this.DialogResult = DialogResult.OK;
    }
  }
}
