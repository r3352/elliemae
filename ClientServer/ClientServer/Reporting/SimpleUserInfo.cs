// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.SimpleUserInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class SimpleUserInfo
  {
    private string userId;
    private string firstName;
    private string lastName;

    public SimpleUserInfo(string userId, string lastName, string firstName)
    {
      this.userId = userId;
      this.firstName = firstName;
      this.lastName = lastName;
    }

    public string UserId => this.userId;

    public string FirstName => this.firstName;

    public string LastName => this.lastName;

    public override string ToString() => this.userId;
  }
}
