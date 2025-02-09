// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactAccessPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactAccessPanel : ContactImportWizardItem
  {
    private Panel panel2;
    private RadioButton radPrivate;
    private RadioButton radPublic;
    private Label lblInstructions;
    private IContainer components;

    public ContactAccessPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      if (this.ImportParameters.ContactType == ContactType.Borrower)
        this.radPublic.Text = "Public - accessible by you and your superiors";
      else if (Session.EncompassEdition == EncompassEdition.Banker)
      {
        this.radPublic.Text = "Public - access based on user group setting";
      }
      else
      {
        this.radPublic.Text = "Public - accessible to other users";
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
        if (contactGroupRefs == null || contactGroupRefs.Length == 0)
          this.radPublic.Enabled = false;
      }
      if (this.ImportParameters.AccessLevel == ContactAccess.Private)
        this.radPrivate.Checked = true;
      else
        this.radPublic.Checked = true;
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
      this.radPrivate = new RadioButton();
      this.radPublic = new RadioButton();
      this.lblInstructions = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.radPrivate);
      this.panel2.Controls.Add((Control) this.radPublic);
      this.panel2.Controls.Add((Control) this.lblInstructions);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 14;
      this.radPrivate.Location = new Point(72, 68);
      this.radPrivate.Name = "radPrivate";
      this.radPrivate.Size = new Size(352, 24);
      this.radPrivate.TabIndex = 17;
      this.radPrivate.Text = "Personal - accessible only by you";
      this.radPublic.Location = new Point(72, 94);
      this.radPublic.Name = "radPublic";
      this.radPublic.Size = new Size(352, 24);
      this.radPublic.TabIndex = 16;
      this.radPublic.Text = "Public - accessible by all users";
      this.lblInstructions.Location = new Point(38, 32);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(418, 30);
      this.lblInstructions.TabIndex = 14;
      this.lblInstructions.Text = "Select the access level for the imported contacts.";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Contact Access";
      this.Name = nameof (ContactAccessPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override WizardItem Next()
    {
      this.ImportParameters.AccessLevel = this.radPublic.Checked ? ContactAccess.Public : ContactAccess.Private;
      if (this.ImportParameters.AccessLevel == ContactAccess.Public && this.ImportParameters.ContactType == ContactType.BizPartner)
      {
        if (Session.EncompassEdition == EncompassEdition.Banker)
          return (WizardItem) new BizContactGroupSelectionPanel((ContactImportWizardItem) this);
        this.ImportParameters.GroupList = Session.ContactGroupManager.GetPublicBizContactGroups();
        if (this.ImportParameters.ImportMethod == ImportMethod.Outlook)
          return (WizardItem) new OutlookImportPanel((ContactImportWizardItem) this);
        return this.ImportParameters.ImportMethod == ImportMethod.CSV ? (WizardItem) new CsvFileSelectionPanel((ContactImportWizardItem) this) : (WizardItem) new PointFolderSelectionPanel((ContactImportWizardItem) this);
      }
      if (this.ImportParameters.ImportMethod == ImportMethod.Outlook)
        return (WizardItem) new OutlookImportPanel((ContactImportWizardItem) this);
      return this.ImportParameters.ImportMethod == ImportMethod.CSV ? (WizardItem) new CsvFileSelectionPanel((ContactImportWizardItem) this) : (WizardItem) new PointFolderSelectionPanel((ContactImportWizardItem) this);
    }

    public override WizardItem Back()
    {
      this.ImportParameters.AccessLevel = this.radPublic.Checked ? ContactAccess.Public : ContactAccess.Private;
      return base.Back();
    }
  }
}
