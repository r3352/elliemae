// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactImportParameters
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactImportParameters
  {
    public ContactType ContactType;
    public ContactAccess AccessLevel;
    public ImportMethod ImportMethod;
    public ContactGroupInfo[] GroupList;
    public object ImportOptions;
    public ContactImportWizard WizardForm;
    public int TPOExternalOrgID = -1;
    public List<long> AllContactIds;

    public ContactImportParameters(ContactImportWizard wizardForm, ContactType type)
    {
      this.WizardForm = wizardForm;
      this.ContactType = type;
    }
  }
}
