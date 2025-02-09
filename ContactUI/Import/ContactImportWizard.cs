// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactImportWizard
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactImportWizard : WizardBase
  {
    private IContainer components;

    public event ContactImportedEventHandler ContactImported;

    public ContactImportWizard(ContactType contactType)
      : this(contactType, -1, (List<long>) null)
    {
    }

    public ContactImportWizard(
      ContactType contactType,
      int tpoExternalOrgID,
      List<long> allContactIds)
    {
      bool isTPOMVP = Session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.InitializeComponent();
      switch (contactType)
      {
        case ContactType.BizPartner:
          this.Text = "Business Contact Import Wizard";
          break;
        case ContactType.TPO:
          this.Text = "TPO Contact Import Wizard";
          break;
        case ContactType.TPOCompany:
          this.Text = "TPO Organization Import Wizard";
          break;
        default:
          this.Text = "Borrower Contact Import Wizard";
          break;
      }
      ContactImportParameters importParams = new ContactImportParameters(this, contactType);
      if (contactType == ContactType.TPO)
      {
        importParams.TPOExternalOrgID = tpoExternalOrgID;
        importParams.AllContactIds = allContactIds;
        importParams.ImportMethod = ImportMethod.CSV;
        importParams.ImportOptions = (object) new CsvImportParameters(ContactType.TPO, isTPOMVP);
        this.Current = (WizardItem) new CsvFileSelectionPanel(importParams);
      }
      else if (contactType == ContactType.TPOCompany)
      {
        importParams.ImportMethod = ImportMethod.CSV;
        importParams.ImportOptions = (object) new CsvImportParameters(ContactType.TPOCompany);
        this.Current = (WizardItem) new CsvFileSelectionPanel(importParams);
      }
      else
        this.Current = (WizardItem) new ImportSourcePanel(importParams);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlFooter.SuspendLayout();
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackButtonVisible = true;
      this.ClientSize = new Size(496, 358);
      this.MinimizeBox = false;
      this.Name = nameof (ContactImportWizard);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Contact Import Wizard";
      this.pnlFooter.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void OnContactImported(object contact)
    {
      if (this.ContactImported == null)
        return;
      this.ContactImported(contact);
    }
  }
}
