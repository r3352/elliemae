// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalOrgContact
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalOrgContact
  {
    private string name = string.Empty;
    private string title = string.Empty;
    private string email = string.Empty;
    private string phone = string.Empty;
    private ExternalOriginatorContactType type;

    public int ExternalOrgContactID { get; set; }

    public string ExternalUserID { get; set; }

    public int ExternalOrgID { get; set; }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    public string Email
    {
      get => this.email;
      set => this.email = value;
    }

    public string Phone
    {
      get => this.phone;
      set => this.phone = value;
    }

    public ExternalOriginatorContactType Type
    {
      get => this.type;
      set => this.type = value;
    }
  }
}
