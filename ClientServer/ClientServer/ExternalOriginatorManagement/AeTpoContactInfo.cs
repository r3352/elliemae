// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.AeTpoContactInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class AeTpoContactInfo
  {
    public string ExternalContactId { get; set; }

    public string ContactId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string BusinessPhone { get; set; }

    public string CompanyName { get; set; }

    public string BranchName { get; set; }

    public int CompanyId { get; set; }

    public string CompanyExternalId { get; set; }

    public int BranchOrgId { get; set; }

    public string BranchId { get; set; }

    public Persona[] Personas { get; set; }
  }
}
