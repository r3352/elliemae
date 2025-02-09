// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrgOriginatorStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Defines the possible status for External Originator</summary>
  public enum ExternalOrgOriginatorStatus : byte
  {
    /// <summary>Indicates Pending</summary>
    Pending = 1,
    /// <summary>Indicates Active</summary>
    Active = 2,
    /// <summary>Indicates Not Active</summary>
    NotActive = 3,
    /// <summary>Indicates Expired</summary>
    Expired = 4,
  }
}
