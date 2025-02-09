// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CSDBAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class CSDBAccessor
  {
    private static string className = nameof (CSDBAccessor);

    private CSDBAccessor()
    {
    }

    public static void AddContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel)
    {
      try
      {
        if (!(sharerID != userID))
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("INSERT INTO CalendarSharingList (OwnerID, UserID, AccessLevel) Values(" + SQL.Encode((object) sharerID) + ", " + SQL.Encode((object) userID) + ", " + (object) (int) accessLevel + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
    }

    public static void DeleteContact(string sharerID, string userID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from CalendarSharingList where OwnerID = " + SQL.Encode((object) sharerID) + " and UserID = " + SQL.Encode((object) userID));
        dbQueryBuilder.AppendLine("delete from CSMessages where fromUser = " + SQL.Encode((object) userID) + " and toUser = " + SQL.Encode((object) sharerID));
        dbQueryBuilder.AppendLine("delete from CSMessages where fromUser = " + SQL.Encode((object) sharerID) + " and toUser = " + SQL.Encode((object) userID) + " and msgType = " + (object) 4);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
    }

    public static void UpdateContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("UPDATE CalendarSharingList Set AccessLevel = " + (object) (int) accessLevel + " where OwnerID = " + SQL.Encode((object) sharerID) + " and UserId = " + SQL.Encode((object) userID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
    }

    public static CSListInfo GetCalenderSharingContactList(string UserID, bool fromOwner)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (fromOwner)
        {
          dbQueryBuilder.Append("select C.*, '' as OwnerFirstName, '' as OwnerLastName, U.last_name as UserFirstName, U.first_name as UserLastName from CalendarSharingList C inner join Users U on C.UserID = U.UserID");
          dbQueryBuilder.Append(" where C.OwnerID = " + SQL.Encode((object) UserID));
        }
        else
        {
          dbQueryBuilder.Append("select C.*, U.last_name as OwnerFirstName, U.first_name as OwnerLastName, '' as UserFirstName, '' as UserLastName from CalendarSharingList C inner join Users U on C.OwnerID = U.UserID");
          dbQueryBuilder.Append(" where C.UserID = " + SQL.Encode((object) UserID));
        }
        return new CSListInfo(dbQueryBuilder.Execute());
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
        return (CSListInfo) null;
      }
    }

    public static bool ContainsContact(string sharerID, string userID)
    {
      bool flag = false;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from CalendarSharingList");
        dbQueryBuilder.Append(" where OwnerID = " + SQL.Encode((object) sharerID) + " and UserID = " + SQL.Encode((object) userID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
            flag = true;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
      return flag;
    }

    public static CSControlMessage[] GetStoredCSMessages(string userID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from CSMessages where toUser = " + SQL.Encode((object) userID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return (CSControlMessage[]) null;
        List<CSControlMessage> csControlMessageList = new List<CSControlMessage>();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string fromUser = (string) dataRowCollection[index]["fromUser"];
          string toUser = (string) dataRowCollection[index]["toUser"];
          CSMessage.MessageType msgType = (CSMessage.MessageType) dataRowCollection[index]["msgType"];
          string msgText = (string) dataRowCollection[index]["msgText"];
          csControlMessageList.Add(new CSControlMessage(fromUser, toUser, msgType, msgText));
        }
        return csControlMessageList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
        return (CSControlMessage[]) null;
      }
    }

    public static void SaveCSControlMessage(
      string fromUser,
      string toUser,
      CSMessage.MessageType msgType,
      string msg)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("INSERT INTO CSMessages (fromUser, toUser, msgType, msgText)");
        dbQueryBuilder.AppendLine("Values (" + SQL.Encode((object) fromUser) + ", " + SQL.Encode((object) toUser) + ", " + (object) (int) msgType + ", " + SQL.Encode((object) msg) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
    }

    public static void DeleteCSMessage(
      string fromUser,
      string toUser,
      CSMessage.MessageType[] msgTypeList)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string[] data = new string[msgTypeList.Length];
        for (int index = 0; index < data.Length; ++index)
          data[index] = ((int) msgTypeList[index]).ToString();
        dbQueryBuilder.Append("delete from CSMessages where fromUser = " + SQL.Encode((object) fromUser) + " and toUser = " + SQL.Encode((object) toUser) + " and msgType IN (" + SQL.EncodeArray((Array) data) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(CSDBAccessor.className, ex);
      }
    }
  }
}
