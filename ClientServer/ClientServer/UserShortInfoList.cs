// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserShortInfoList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserShortInfoList
  {
    public readonly UserShortInfo[] UserShortInfos;
    public readonly string[] SessionIDs;
    public readonly string[] Names;
    public readonly int Count;

    public UserShortInfoList(UserShortInfo[] userShortInfos)
    {
      this.UserShortInfos = userShortInfos;
      if (this.UserShortInfos == null)
        return;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (UserShortInfo userShortInfo in this.UserShortInfos)
      {
        stringList1.Add(userShortInfo.SessionID);
        stringList2.Add(userShortInfo.FirstName + " " + userShortInfo.LastName);
      }
      this.SessionIDs = stringList1.ToArray();
      this.Names = stringList2.ToArray();
      this.Count = this.UserShortInfos.Length;
    }

    public UserShortInfo GetUserInfo(string sessionID)
    {
      foreach (UserShortInfo userShortInfo in this.UserShortInfos)
      {
        if (userShortInfo.SessionID == sessionID)
          return userShortInfo;
      }
      return (UserShortInfo) null;
    }
  }
}
