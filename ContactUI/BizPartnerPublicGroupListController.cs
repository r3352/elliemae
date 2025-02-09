// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerPublicGroupListController
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerPublicGroupListController(ComboBox cmbBox) : ContactGroupListController(cmbBox)
  {
    public override void loadContactGroups()
    {
      this.cmbBoxContactGroups.Items.Clear();
      this.cmbBoxContactGroups.Items.Add((object) "All Contacts");
      ContactGroupInfo[] items = Session.ContactGroupManager.GetPublicBizContactGroups();
      if (!Session.UserInfo.IsSuperAdministrator() && this.editOnly)
      {
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
        ArrayList arrayList = new ArrayList();
        foreach (ContactGroupInfo contactGroupInfo in items)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId && !arrayList.Contains((object) contactGroupInfo))
            {
              arrayList.Add((object) contactGroupInfo);
              break;
            }
          }
        }
        items = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
      }
      if (items == null)
        return;
      this.cmbBoxContactGroups.Items.AddRange((object[]) items);
    }
  }
}
