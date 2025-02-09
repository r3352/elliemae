// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactImportWizardItem
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common.UI.Wizard;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactImportWizardItem : WizardItemWithHeader
  {
    protected ContactImportParameters ImportParameters;

    public ContactImportWizardItem()
    {
    }

    private void InitializeComponent()
    {
      this.BackColor = SystemColors.Control;
      this.Name = nameof (ContactImportWizardItem);
    }

    public ContactImportWizardItem(ContactImportParameters importParams)
    {
      this.ImportParameters = importParams;
    }

    public ContactImportWizardItem(ContactImportWizardItem prevItem)
      : this((WizardItem) prevItem, prevItem.ImportParameters)
    {
    }

    public ContactImportWizardItem(WizardItem prevItem, ContactImportParameters importParams)
      : base(prevItem)
    {
      this.ImportParameters = importParams;
    }
  }
}
