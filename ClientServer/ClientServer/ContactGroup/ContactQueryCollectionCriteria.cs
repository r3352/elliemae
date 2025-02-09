// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactGroup.ContactQueryCollectionCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ContactGroup
{
  [Serializable]
  public class ContactQueryCollectionCriteria
  {
    public string UserId;
    public ContactType ContactType;
    public ContactQueryType[] QueryTypes;

    public ContactQueryCollectionCriteria(
      string userId,
      ContactType contactType,
      ContactQueryType[] queryTypes)
    {
      this.UserId = userId;
      this.ContactType = contactType;
      this.QueryTypes = queryTypes;
    }
  }
}
