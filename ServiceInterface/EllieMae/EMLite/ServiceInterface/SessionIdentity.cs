// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.SessionIdentity
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public sealed class SessionIdentity
  {
    private const int MASKLENGTH = 18;

    public SessionIdentity(string instanceName, string sessionId)
    {
      this.InstanceName = instanceName;
      this.SessionID = sessionId;
    }

    public string InstanceName { get; private set; }

    public string SessionID { get; private set; }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.InstanceName) ? this.SessionID : this.InstanceName + "_" + this.SessionID;
    }

    public override bool Equals(object obj)
    {
      return obj is SessionIdentity sessionIdentity && string.Compare(this.InstanceName, sessionIdentity.InstanceName, true) == 0 && string.Compare(this.SessionID, sessionIdentity.SessionID, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.ToString());
    }

    public static SessionIdentity Parse(string token)
    {
      string[] strArray = token.Split('_');
      if (strArray.Length == 0)
        throw new FormatException("Invalid SessionIdentity token");
      return strArray.Length == 1 ? new SessionIdentity("", strArray[0]) : new SessionIdentity(strArray[0], strArray[1]);
    }

    public static string GetMaskedSessionId(string sessionToken)
    {
      SessionIdentity sessionIdentity = SessionIdentity.Parse(sessionToken);
      return sessionIdentity.SessionID.Length <= 18 ? sessionIdentity.SessionID : new string('*', 18) + sessionIdentity.SessionID.Substring(18);
    }
  }
}
