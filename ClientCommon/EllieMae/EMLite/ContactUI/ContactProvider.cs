// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactProvider
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public interface ContactProvider
  {
    ContactQueries GetContactQueries(string userid);

    void DeleteContactQueries(string userid, ContactQuery query);

    void AddContactQuery(string userid, ContactQuery query);

    void UpdateContactQueries(string userid, ContactQuery query);
  }
}
