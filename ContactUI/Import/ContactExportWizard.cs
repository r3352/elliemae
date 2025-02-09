// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactExportWizard
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common.UI.Wizard;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactExportWizard : WizardBase
  {
    private IContainer components;

    public event ContactExportedEventHandler ContactExported;

    public ContactExportWizard(ContactType contactType, int[] contactIds)
    {
      this.InitializeComponent();
      this.BackButtonVisible = false;
      this.Text = contactType == ContactType.BizPartner ? "Business Contact Export Wizard" : "Borrower Contact Export Wizard";
      this.Current = (WizardItem) new CsvExportFileSelectionPanel(contactType, contactIds);
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
      this.Name = nameof (ContactExportWizard);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Contact Export Wizard";
      this.pnlFooter.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void OnContactExported(object contact)
    {
      if (this.ContactExported == null)
        return;
      this.ContactExported(contact);
    }
  }
}
