// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.PointFolderSelectionPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.Import;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class PointFolderSelectionPanel : ContactImportWizardItem
  {
    private PointImportOptions importOptions;
    private Panel panel2;
    private Label label2;
    private Label lblDatabase;
    private Button btnBrowse;
    private FolderBrowserDialog fbdBrowse;
    private Label lblInstructions;
    private IContainer components;

    public PointFolderSelectionPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.importOptions = (PointImportOptions) this.ImportParameters.ImportOptions;
      this.lblDatabase.Text = this.importOptions.DatabasePath;
      if (!this.importOptions.AutoDetectDatabase)
        return;
      this.lblInstructions.Text = "Encompass has detected the location of your Cardex database to be the folder below. If you wish to change the location from which you will import the Cardex information, use the Browse button.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel2 = new Panel();
      this.lblInstructions = new Label();
      this.label2 = new Label();
      this.lblDatabase = new Label();
      this.btnBrowse = new Button();
      this.fbdBrowse = new FolderBrowserDialog();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.btnBrowse);
      this.panel2.Controls.Add((Control) this.lblDatabase);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.lblInstructions);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.lblInstructions.Location = new Point(38, 25);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(418, 47);
      this.lblInstructions.TabIndex = 0;
      this.lblInstructions.Text = "Encompass was unable to detect the location of your Point Cardex database. Use the Browse button to select the folder containing your Cardex information .";
      this.label2.Location = new Point(38, 78);
      this.label2.Name = "label2";
      this.label2.Size = new Size(348, 17);
      this.label2.TabIndex = 1;
      this.label2.Text = "CARDEX Database Folder:";
      this.lblDatabase.BackColor = SystemColors.Control;
      this.lblDatabase.BorderStyle = BorderStyle.Fixed3D;
      this.lblDatabase.Location = new Point(38, 96);
      this.lblDatabase.Name = "lblDatabase";
      this.lblDatabase.Size = new Size(348, 20);
      this.lblDatabase.TabIndex = 2;
      this.lblDatabase.TextAlign = ContentAlignment.MiddleLeft;
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Location = new Point(387, 95);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(71, 23);
      this.btnBrowse.TabIndex = 3;
      this.btnBrowse.Text = "Browse";
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.fbdBrowse.Description = "Select CARDEX Database folder";
      this.fbdBrowse.ShowNewFolderButton = false;
      this.Controls.Add((Control) this.panel2);
      this.Header = "Cardex Import";
      this.Name = nameof (PointFolderSelectionPanel);
      this.Subheader = "Provide the location of your Cardex database";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      if (this.fbdBrowse.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return;
      this.lblDatabase.Text = this.fbdBrowse.SelectedPath;
      this.OnControlsChange();
    }

    public override bool NextEnabled => this.lblDatabase.Text != "";

    public override string NextLabel => "&Import >";

    public override WizardItem Back()
    {
      this.importOptions.DatabasePath = this.lblDatabase.Text;
      return base.Back();
    }

    public override WizardItem Next()
    {
      this.importOptions.DatabasePath = this.lblDatabase.Text;
      PointCardexDatabaseImport cardexDatabaseImport = new PointCardexDatabaseImport(this.importOptions.DatabasePath, this.ImportParameters.AccessLevel, this.ImportParameters.GroupList);
      cardexDatabaseImport.ContactCreated += new ContactEventHandler(this.onContactCreated);
      cardexDatabaseImport.ContactOverwritten += new ContactEventHandler(this.onContactCreated);
      return new ProgressDialog("Importing Contacts", new AsynchronousProcess(cardexDatabaseImport.ImportContacts), (object) null, true).ShowDialog() == DialogResult.Abort ? (WizardItem) null : WizardItem.Finished;
    }

    private void onContactCreated(object sender, BizPartnerInfo contactInfo)
    {
      this.ImportParameters.WizardForm.OnContactImported((object) contactInfo);
    }
  }
}
