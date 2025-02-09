// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.ServiceProfile
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public class ServiceProfile
  {
    public Guid SvcProfileID { get; set; }

    public Guid SvcProductID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Default { get; set; }

    public string Condition { get; set; }

    public string LastModifiedByUserId { get; set; }

    public DateTime LastModified { get; set; }

    public List<ServiceProfileUser> AuthorizedUsers { get; set; }
  }
}
