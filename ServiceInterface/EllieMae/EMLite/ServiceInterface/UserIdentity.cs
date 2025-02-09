// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.UserIdentity
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Security.Principal;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public class UserIdentity : IIdentity
  {
    private const string TokenPrefix = "Encompass";

    public UserIdentity(string instanceName, string userId, bool isVirtual = false)
    {
      this.InstanceName = instanceName;
      this.UserID = userId;
      this.IsVirtualUser = isVirtual;
    }

    public string InstanceName { get; private set; }

    public string UserID { get; private set; }

    public bool IsVirtualUser { get; private set; }

    public override bool Equals(object obj)
    {
      return obj is UserIdentity userIdentity && string.Compare(this.InstanceName, userIdentity.InstanceName, true) == 0 && string.Compare(this.UserID, userIdentity.UserID, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.ToString());
    }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.InstanceName) ? "Encompass\\" + this.UserID : "Encompass\\" + this.InstanceName + "\\" + this.UserID;
    }

    string IIdentity.AuthenticationType => "Encompass";

    bool IIdentity.IsAuthenticated => true;

    string IIdentity.Name => this.ToString();

    public static UserIdentity Parse(string userName)
    {
      string[] strArray = userName.Split('\\');
      bool isVirtual = false;
      if (strArray.Length < 2)
        throw new ArgumentException("Invalid user name format");
      if (strArray[0] != "Encompass")
        throw new ArgumentException("Invalid user name source");
      if (strArray.Length == 2)
      {
        if (strArray[1].StartsWith("<") && strArray[1].EndsWith(">"))
          isVirtual = true;
        return new UserIdentity("", strArray[1], isVirtual);
      }
      if (strArray.Length != 3)
        throw new ArgumentException("Invalid user name format");
      if (strArray[2].StartsWith("<") && strArray[2].EndsWith(">"))
        isVirtual = true;
      return new UserIdentity(strArray[1], strArray[2], isVirtual);
    }
  }
}
