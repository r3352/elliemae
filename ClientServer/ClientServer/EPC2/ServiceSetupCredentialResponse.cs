// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.ServiceSetupCredentialResponse
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class ServiceSetupCredentialResponse
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string InstanceId { get; set; }

    public string OrderType { get; set; }

    public bool IsActive { get; set; }

    public string ProviderId { get; set; }

    public string Category { get; set; }

    public string Scope { get; set; }

    public ServiceCredentialResponse ServiceCredential { get; set; }

    public List<AuthorizedUser> AuthorizedUsers { get; set; }
  }
}
