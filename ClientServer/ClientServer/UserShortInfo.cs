// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserShortInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserShortInfo : IComparable
  {
    public readonly string SessionID;
    public readonly string Userid;
    public readonly string FirstName;
    public readonly string LastName;
    public readonly LockInfo.ExclusiveLock Exclusive;
    public readonly string[] LoanRoles;

    public string[] UniqueLoanroles => this.removeDuplicates(this.LoanRoles);

    public UserShortInfo(
      string sessionID,
      string userid,
      string firstName,
      string lastName,
      LockInfo.ExclusiveLock exclusive,
      string[] loanRoles)
    {
      this.SessionID = sessionID;
      this.Userid = userid;
      this.FirstName = firstName;
      this.LastName = lastName;
      this.Exclusive = exclusive;
      this.LoanRoles = loanRoles;
    }

    public override string ToString()
    {
      string[] strArray = this.removeDuplicates(this.LoanRoles);
      string str = this.FirstName + " " + this.LastName;
      return strArray == null || strArray.Length == 0 ? "Other: " + str : string.Join(",", strArray) + ": " + str;
    }

    public int CompareTo(object obj)
    {
      UserShortInfo userShortInfo = obj as UserShortInfo;
      if (obj == null)
        throw new Exception("Invalid value for comparison");
      return this.Userid == userShortInfo.Userid ? 0 : string.Compare(this.ToString(), userShortInfo.ToString(), true);
    }

    private string[] removeDuplicates(string[] stringArray)
    {
      if (stringArray == null || stringArray.Length == 0)
        return stringArray;
      List<string> stringList = new List<string>();
      foreach (string str in stringArray)
      {
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
      return stringList.ToArray();
    }
  }
}
