// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.HierarchySummary
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class HierarchySummary
  {
    public virtual int oid { get; set; }

    public virtual int Parent { get; set; }

    public virtual string CompanyDBAName { get; set; }

    public virtual string CompanyLegalName { get; set; }

    public virtual string OrganizationName { get; set; }

    public virtual int Depth { get; set; }

    public virtual string HierarchyPath { get; set; }

    public virtual string ExternalID { get; set; }

    public HierarchySummary(
      int oid,
      int parent,
      string externalID,
      string OrganizationName,
      string CompanyLegalName,
      string CompanyDBAName,
      int depth,
      string hierarchyPath)
    {
      this.oid = oid;
      this.Parent = parent;
      this.ExternalID = externalID;
      this.CompanyLegalName = CompanyLegalName;
      this.CompanyDBAName = CompanyDBAName;
      this.OrganizationName = OrganizationName;
      this.Depth = depth;
      this.HierarchyPath = hierarchyPath;
    }

    public HierarchySummary()
    {
    }
  }
}
