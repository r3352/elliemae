// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IMDBAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class IMDBAccessor
  {
    private const string className = "MessengerList�";

    private IMDBAccessor()
    {
    }

    public static MessengerListInfo GetMessengerList(string userid)
    {
      if (userid == null)
        throw new Exception("IMDBAccessor.GetMessengerList(): userid cannot be null");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select ml.groupName, mg.contactUserid, mg.firstName, mg.lastName from MessengerList AS ml LEFT JOIN MessengerGroup as mg ON ml.messengerListID = mg.messengerListID");
      dbQueryBuilder.Append(" where ml.userid = " + SQL.Encode((object) userid) + " order by ml.messengerListID asc");
      return new MessengerListInfo(dbQueryBuilder.Execute());
    }

    public static void AddGroup(string userid, string groupName)
    {
      if (userid == null || groupName == null)
        throw new Exception("IMDBAccessor.AddGroup(): userid/groupName cannot be null");
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("insert into MessengerList (userid, groupName) VALUES (" + SQL.Encode((object) userid) + "," + SQL.Encode((object) groupName) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddContact(
      string userid,
      string groupName,
      string contactUserid,
      string firstName,
      string lastName,
      string reqMsg)
    {
      if (userid == null || groupName == null || contactUserid == null)
        throw new Exception("IMDBAccessor.AddContact(): arguments cannot be null");
      if (firstName == null)
        firstName = "";
      if (lastName == null)
        lastName = "";
      if (reqMsg == null)
        reqMsg = "";
      else if (reqMsg.Length > 512)
        reqMsg = reqMsg.Substring(0, 512);
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("insert into MessengerGroup (messengerListID, contactUserid, firstName, lastName) ");
      dbQueryBuilder.AppendLine("select messengerListID, " + SQL.Encode((object) contactUserid) + ", " + SQL.Encode((object) firstName) + ", " + SQL.Encode((object) lastName));
      dbQueryBuilder.AppendLine(" from MessengerList where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) groupName));
      if (userid != contactUserid)
      {
        dbQueryBuilder.AppendLine("insert into IMMessages (fromUser, toUser, msgType, msgText)");
        dbQueryBuilder.AppendLine("values (" + SQL.Encode((object) userid) + ", " + SQL.Encode((object) contactUserid) + ", " + (object) 1 + ", " + SQL.Encode((object) reqMsg) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteContact(string userid, string groupName, string contactUserid)
    {
      if (userid == null || groupName == null || contactUserid == null)
        throw new Exception("IMDBAccessor.DeleteContact(): arguments cannot be null");
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from MessengerGroup where messengerListID = ");
      dbQueryBuilder.AppendLine("(select messengerListID from MessengerList where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) groupName) + ")");
      dbQueryBuilder.AppendLine("and contactUserid = " + SQL.Encode((object) contactUserid));
      dbQueryBuilder.AppendLine("delete from IMMessages where fromUser = " + SQL.Encode((object) userid) + " and toUser = " + SQL.Encode((object) contactUserid) + " and msgType = " + (object) 1);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteGroup(string userid, string groupName)
    {
      if (userid == null || groupName == null)
        throw new Exception("IMDBAccessor.DeleteGroup(): arguments cannot be null");
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "select messengerListID where userid=" + SQL.Encode((object) userid) + " and groupName=" + SQL.Encode((object) groupName);
      dbQueryBuilder.Append("delete from MessengerList where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) groupName));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void RenameGroup(string userid, string groupName, string newGroupName)
    {
      if (userid == null || groupName == null || newGroupName == null)
        throw new Exception("IMDBAccessor.RenameGroup(): arguments cannot be null");
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      if (newGroupName.Length > 64)
        newGroupName = newGroupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("update MessengerList set groupName = " + SQL.Encode((object) newGroupName) + " where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) groupName));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void MoveContact(
      string userid,
      string contactUserid,
      string fromGroup,
      string toGroup)
    {
      if (userid == null || contactUserid == null || fromGroup == null || toGroup == null)
        throw new Exception("IMDBAccessor.MoveContact(): arguments cannot be null");
      fromGroup = fromGroup.Trim();
      if (fromGroup.Length > 64)
        fromGroup = fromGroup.Substring(0, 64);
      toGroup = toGroup.Trim();
      if (toGroup.Length > 64)
        toGroup = toGroup.Substring(0, 64);
      if (fromGroup.ToLower() == toGroup.ToLower())
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update MessengerGroup set messengerListID=");
      dbQueryBuilder.AppendLine("(select messengerListID from MessengerList where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) toGroup) + ")");
      dbQueryBuilder.AppendLine("where contactUserid = " + SQL.Encode((object) contactUserid));
      dbQueryBuilder.AppendLine("and messengerListID=(select messengerListID from MessengerList where userid=" + SQL.Encode((object) userid) + " and groupName=" + SQL.Encode((object) fromGroup) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool ContainsGroup(string userid, string groupName)
    {
      if (userid == null || groupName == null)
        throw new Exception("IMDBAccessor.ContainsGroup(): arguments cannot be null");
      if (groupName.Length > 64)
        groupName = groupName.Substring(0, 64);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select messengerListID from MessengerList where userid = " + SQL.Encode((object) userid) + " and groupName = " + SQL.Encode((object) groupName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count != 0;
    }

    public static bool ContainsContact(string userid, string contactUserid)
    {
      if (userid == null || contactUserid == null)
        throw new Exception("IMDBAccessor.ContainsContact(): arguments cannot be null");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select ml.messengerListID from MessengerList as ml INNER JOIN MessengerGroup as mg ON ml.messengerListID = mg.messengerListID where ml.userid = " + SQL.Encode((object) userid) + " and mg.contactUserid = " + SQL.Encode((object) contactUserid));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count != 0;
    }

    public static void InsertToIMMessages(IMControlMessage msg)
    {
      string fromUser = msg.FromUser;
      string toUser = msg.ToUser;
      int msgType = (int) msg.MsgType;
      string text = msg.Text == null ? "" : msg.Text;
      if (msg.MsgType == IMMessage.MessageType.RequestAddToList || msg.MsgType == IMMessage.MessageType.DenyAddToList)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select id from IMMessages where fromUser = " + SQL.Encode((object) fromUser) + " and toUser = " + SQL.Encode((object) toUser) + " and msgType = " + (object) msgType);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          return;
      }
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("insert into IMMessages (fromUser, toUser, msgType, msgText) VALUES (" + SQL.Encode((object) fromUser) + ", " + SQL.Encode((object) toUser) + ", " + (object) msgType + ", " + SQL.Encode((object) text) + ")");
      dbQueryBuilder1.ExecuteNonQuery();
    }

    public static void DeleteAddToListRequest(string fromUser, string toUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("delete from IMMessages where fromUser = " + SQL.Encode((object) fromUser) + " and toUser = " + SQL.Encode((object) toUser) + " and msgType = " + (object) 1);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteDenyAddToList(string fromUser, string toUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("delete from IMMessages where fromUser = " + SQL.Encode((object) fromUser) + " and toUser = " + SQL.Encode((object) toUser) + " and msgType = " + (object) 3);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ProcessDenyAddToListRequest(IMControlMessage msg)
    {
      if (msg.MsgType != IMMessage.MessageType.DenyAddToList)
        return;
      string fromUser = msg.FromUser;
      string toUser = msg.ToUser;
      int msgType = (int) msg.MsgType;
      string text = msg.Text;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from MessengerGroup where messengerListID = ");
      dbQueryBuilder.AppendLine("(select mg.messengerListID from MessengerGroup as mg INNER JOIN MessengerList as ml ON ml.messengerListID = mg.messengerListID");
      dbQueryBuilder.AppendLine("where ml.userid = " + SQL.Encode((object) toUser) + " and mg.contactUserid = " + SQL.Encode((object) fromUser) + ")");
      dbQueryBuilder.AppendLine("and contactUserid = " + SQL.Encode((object) fromUser));
      dbQueryBuilder.AppendLine("insert into IMMessages (fromUser, toUser, msgType, msgText)");
      dbQueryBuilder.AppendLine("VALUES (" + SQL.Encode((object) fromUser) + ", " + SQL.Encode((object) toUser) + ", " + (object) msgType + ", " + SQL.Encode((object) text) + ")");
      dbQueryBuilder.AppendLine("delete from IMMessages where fromUser = " + SQL.Encode((object) toUser) + " and toUser = " + SQL.Encode((object) fromUser) + " and msgType = " + (object) 1);
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    public static IMControlMessage[] GetStoredIMMessages(string userid)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from IMMessages where toUser = " + SQL.Encode((object) userid));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return (IMControlMessage[]) null;
        IMControlMessage[] storedImMessages = new IMControlMessage[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string fromUser = (string) dataRowCollection[index]["fromUser"];
          string toUser = (string) dataRowCollection[index]["toUser"];
          IMMessage.MessageType msgType = (IMMessage.MessageType) dataRowCollection[index]["msgType"];
          string msgText = (string) dataRowCollection[index]["msgText"];
          storedImMessages[index] = new IMControlMessage(fromUser, toUser, msgType, msgText);
        }
        return storedImMessages;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from IMMessages where toUser = " + SQL.Encode((object) userid));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 == null)
        return (IMControlMessage[]) null;
      IMControlMessage[] storedImMessages1 = new IMControlMessage[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
      {
        string fromUser = (string) dataRowCollection1[index]["fromUser"];
        string toUser = (string) dataRowCollection1[index]["toUser"];
        IMMessage.MessageType msgType = (IMMessage.MessageType) dataRowCollection1[index]["msgType"];
        string msgText = (string) dataRowCollection1[index]["msgText"];
        storedImMessages1[index] = new IMControlMessage(fromUser, toUser, msgType, msgText);
      }
      return storedImMessages1;
    }
  }
}
