// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDBAName
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External DBA Name.</summary>
  public class ExternalDBAName : IExternalDBAName
  {
    private ExternalOrgDBAName externalOrgDBA;

    internal ExternalDBAName(ExternalOrgDBAName externalOrgDBA)
    {
      this.externalOrgDBA = externalOrgDBA;
    }

    /// <summary>Gets ExternalOrgID of the DBA record</summary>
    public int ExternalOrgID => this.externalOrgDBA.ExternalOrgID;

    /// <summary>Gets DBAID of the DBA record</summary>
    public int DBAID => this.externalOrgDBA.DBAID;

    /// <summary>Gets or sets Name of the DBA record</summary>
    public string Name
    {
      get => this.externalOrgDBA.Name;
      set => this.externalOrgDBA.Name = value;
    }

    /// <summary>Gets or sets whether the DBA is default</summary>
    public bool SetAsDefault
    {
      get => this.externalOrgDBA.SetAsDefault;
      set => this.externalOrgDBA.SetAsDefault = value;
    }

    /// <summary>Gets or sets SortIndex of the DBA record</summary>
    public int SortIndex
    {
      get => this.externalOrgDBA.SortIndex;
      set => this.externalOrgDBA.SortIndex = value;
    }

    internal static ExternalDBAList ToList(List<ExternalOrgDBAName> fees)
    {
      ExternalDBAList list = new ExternalDBAList();
      for (int index = 0; index < fees.Count; ++index)
        list.Add(new ExternalDBAName(fees[index]));
      return list;
    }

    internal ExternalOrgDBAName Original => this.externalOrgDBA;
  }
}
