// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerGroupListController
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerGroupListController(ComboBox cmbBox) : ContactGroupListController(cmbBox)
  {
    public override void loadContactGroups()
    {
      this.cmbBoxContactGroups.Items.Clear();
      this.cmbBoxContactGroups.Items.Add((object) "All Contacts");
      ContactGroupInfo[] contactGroupsForUser = Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, ContactType.BizPartner, new ContactGroupType[1]));
      if (contactGroupsForUser == null)
        return;
      this.cmbBoxContactGroups.Items.AddRange((object[]) contactGroupsForUser);
    }
  }
}
