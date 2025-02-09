// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Enum for grace period later</summary>
  public enum GracePeriodLaterOf : byte
  {
    /// <summary>None</summary>
    None = 0,
    /// <summary>InitialSuspenseDate</summary>
    InitialSuspenseDate = 1,
    /// <summary>ConditionsReceivedDate</summary>
    ConditionsReceivedDate = 2,
    /// <summary>DeliveryExpirationDate</summary>
    DeliveryExpirationDate = 4,
    /// <summary>LatestConditionsIssuedDate</summary>
    LatestConditionsIssuedDate = 8,
    /// <summary>OtherDate</summary>
    OtherDate = 16, // 0x10
  }
}
