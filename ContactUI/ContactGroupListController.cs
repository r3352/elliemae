// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactGroupListController
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public abstract class ContactGroupListController
  {
    public ComboBox cmbBoxContactGroups;
    public bool editOnly;

    public ContactGroupListController(ComboBox cmbBox) => this.cmbBoxContactGroups = cmbBox;

    public abstract void loadContactGroups();
  }
}
