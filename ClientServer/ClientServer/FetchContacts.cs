// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FetchContacts
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FetchContacts
  {
    public virtual int Parent { get; set; }

    public virtual string ExternalID { get; set; }

    public virtual string OrganizationName { get; set; }

    public virtual string CompanyDBAName { get; set; }

    public virtual string CompanyLegalName { get; set; }

    public virtual string Address { get; set; }

    public virtual string City { get; set; }

    public virtual string State { get; set; }

    public virtual string Zip { get; set; }

    public virtual int Depth { get; set; }

    public virtual string HierarchyPath { get; set; }

    public virtual ExternalOriginatorEntityType EntityType { get; set; }

    public FetchContacts(
      string id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      ExternalOriginatorEntityType entityType,
      string address,
      string city,
      string state,
      string zip)
      : this(0, id, organizationName, companyDBAName, companyLegalName, address, city, state, zip, 0, "")
    {
      this.EntityType = entityType;
    }

    public FetchContacts(
      int parent,
      string id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      int depth,
      string hierarchyPath)
    {
      this.Parent = parent;
      this.ExternalID = id;
      this.OrganizationName = organizationName;
      this.CompanyDBAName = companyDBAName;
      this.CompanyLegalName = companyLegalName;
      this.Address = address;
      this.City = city;
      this.State = state;
      this.Zip = zip;
      this.Depth = depth;
      this.HierarchyPath = hierarchyPath;
      this.EntityType = ExternalOriginatorEntityType.None;
    }

    public FetchContacts()
    {
    }
  }
}
