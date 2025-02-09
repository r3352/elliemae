// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ContactGroupDropdownBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ContactGroupDropdownBox : ComboBox
  {
    private ContactType contactType;
    private bool suspendEvent;

    public ContactGroupDropdownBox(ContactType contactType)
    {
      this.DropDownStyle = ComboBoxStyle.DropDownList;
      this.contactType = contactType;
      this.loadItems();
    }

    private void loadItems()
    {
      this.Items.Clear();
      ContactGroupInfo[] contactGroupInfoArray;
      if (this.contactType == ContactType.Borrower)
        contactGroupInfoArray = Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1]));
      else if (this.contactType == ContactType.BizPartner)
      {
        contactGroupInfoArray = Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1]));
      }
      else
      {
        contactGroupInfoArray = Session.ContactGroupManager.GetPublicBizContactGroups();
        if (!Session.UserInfo.IsSuperAdministrator())
        {
          BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
          ArrayList arrayList = new ArrayList();
          foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
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
          contactGroupInfoArray = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
        }
      }
      this.Items.Add((object) "");
      foreach (ContactGroupInfo groupInfo in contactGroupInfoArray)
        this.Items.Add((object) new ContactGroupItem(groupInfo));
    }

    public ContactType ContactType => this.contactType;

    public string[] ValueList
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (object obj in this.Items)
        {
          if (obj.ToString() != "")
            stringList.Add(obj.ToString());
        }
        return stringList.ToArray();
      }
    }

    public void RefreshFilter()
    {
      int num = -1;
      if (this.SelectedItem != null && this.SelectedItem.ToString() != "")
        num = ((ContactGroupItem) this.SelectedItem).GroupID;
      this.suspendEvent = true;
      this.loadItems();
      if (num > 0)
      {
        bool flag = false;
        for (int index = 0; index < this.Items.Count; ++index)
        {
          if (!(this.Items[index].ToString() == "") && ((ContactGroupItem) this.Items[index]).GroupID == num)
          {
            flag = true;
            this.SelectedIndex = index;
            break;
          }
        }
        if (!flag)
        {
          this.suspendEvent = false;
          this.SelectedIndex = 0;
        }
      }
      else
        this.SelectedIndex = 0;
      this.suspendEvent = false;
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.suspendEvent)
        return;
      base.OnSelectedIndexChanged(e);
    }
  }
}
