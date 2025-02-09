// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.ServiceCredential
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public class ServiceCredential
  {
    public Guid Id { get; set; }

    public Guid SvcProductId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Credentials { get; set; }

    public bool AutoWorkflow { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool IsCredentialsEncrypted { get; set; }

    public string Category { get; set; }

    public string ProductName { get; set; }

    public string PartnerID { get; set; }

    public string Version { get; set; }

    public List<AuthorizedUser> AuthorizedUsers { get; set; }
  }
}
