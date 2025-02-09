// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMessengerListManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IMessengerListManager
  {
    void AddGroup(string groupName);

    void AddContact(
      string groupName,
      string contactUserid,
      string firstName,
      string lastName,
      string reqMsg);

    void DeleteGroup(string groupName);

    void DeleteContact(string groupName, string contactUserid);

    void RenameGroup(string groupName, string newGroupName);

    void MoveContact(string contactUserid, string fromGroup, string toGroup);

    MessengerListInfo GetMessengerList();

    bool ContainsGroup(string groupName);

    bool ContainsContact(string contactUserid);

    IMControlMessage[] GetStoredIMMessages();
  }
}
