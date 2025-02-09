// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.UserCredentialRequest
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Dynamic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class UserCredentialRequest
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public List<AuthorizedUser> AuthorizedUsers { get; set; }

    public ExpandoObject Credential { get; set; }

    public string ProviderId { get; set; }

    public string PartnerId { get; set; }

    public string ProductName { get; set; }
  }
}
