// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalOrgURL
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalOrgURL
  {
    public int Id { get; set; }

    public int URLID { get; set; }

    public int ExternalOrgID { get; set; }

    public DateTime DateAdded { get; set; }

    public string URL { get; set; }

    public bool isDeleted { get; set; }

    public int EntityType { get; set; }

    public string siteId { get; set; }

    public bool TPOAdminLinkAccess { get; set; }
  }
}
