// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalDocumentsSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public interface IExternalDocumentsSettings
  {
    Guid Guid { get; }

    int ExternalOrgId { get; set; }

    string AddedBy { get; set; }

    bool Active { get; set; }

    bool DefaultActive { get; set; }

    string FileName { get; set; }

    string DisplayName { get; set; }

    int Category { get; set; }

    ExternalOrganizationEntityType Channel { get; set; }

    DateTime DateAdded { get; set; }

    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    bool AvailbleAllTPO { get; set; }

    bool IsArchive { get; set; }

    bool IsDefault { get; set; }

    int SortId { get; set; }

    ExternalOrgOriginatorStatus Status { get; set; }

    ExternalOrgOriginatorStatus DefaultStatus { get; set; }

    int AssignCount { get; set; }
  }
}
