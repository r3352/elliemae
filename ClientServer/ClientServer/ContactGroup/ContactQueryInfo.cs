// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactGroup.ContactQueryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ContactGroup
{
  [Serializable]
  public class ContactQueryInfo : BusinessInfoBase
  {
    public int QueryId;
    public string UserId;
    public ContactType ContactType;
    public ContactQueryType QueryType;
    public string QueryName;
    public string QueryDesc;
    public string XmlQueryString;
    public bool PrimaryOnly;
    public bool OwnerOnly;
    public bool NotOwner;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public ContactQueryInfo()
    {
    }

    public ContactQueryInfo(
      int queryId,
      string userId,
      ContactType contactType,
      ContactQueryType queryType,
      string queryName,
      string queryDesc,
      string xmlQueryString,
      bool primaryOnly)
    {
      this.QueryId = queryId;
      this.UserId = userId;
      this.ContactType = contactType;
      this.QueryType = queryType;
      this.QueryName = queryName;
      this.QueryDesc = queryDesc;
      this.XmlQueryString = xmlQueryString;
      this.PrimaryOnly = primaryOnly;
    }
  }
}
