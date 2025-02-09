// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public enum GracePeriodLaterOf : byte
  {
    None = 0,
    InitialSuspenseDate = 1,
    ConditionsReceivedDate = 2,
    DeliveryExpirationDate = 4,
    LatestConditionsIssuedDate = 8,
    OtherDate = 16, // 0x10
  }
}
