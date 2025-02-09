// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactGroup.ContactGroupMemberCollectionCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ContactGroup
{
  [Serializable]
  public class ContactGroupMemberCollectionCriteria
  {
    public ContactType ContactType;
    public int GroupId;
    public bool OwnedContactsOnly;

    public ContactGroupMemberCollectionCriteria(
      ContactType contactType,
      int groupId,
      bool ownedContactsOnly)
    {
      this.ContactType = contactType;
      this.GroupId = groupId;
      this.OwnedContactsOnly = ownedContactsOnly;
    }
  }
}
