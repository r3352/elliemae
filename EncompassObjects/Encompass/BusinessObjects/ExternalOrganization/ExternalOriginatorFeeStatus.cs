// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOriginatorFeeStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>ExternalOriginatorFeeStatus enum</summary>
  public enum ExternalOriginatorFeeStatus : byte
  {
    /// <summary>ExternalOriginatorFeeStatus - Pending</summary>
    Pending = 1,
    /// <summary>ExternalOriginatorFeeStatus - Active</summary>
    Active = 2,
    /// <summary>ExternalOriginatorFeeStatus - NoActive</summary>
    NotActive = 3,
    /// <summary>ExternalOriginatorFeeStatus - Expired</summary>
    Expired = 4,
  }
}
