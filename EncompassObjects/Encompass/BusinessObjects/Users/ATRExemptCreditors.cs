// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ATRExemptCreditors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Defines the possible options for ATR Exempt Creditor setting
  /// </summary>
  [Guid("5C7D7D2F-1C82-42C2-9E92-AADAC7A7F9C2")]
  public enum ATRExemptCreditors
  {
    /// <summary>None</summary>
    None,
    /// <summary>Indicates Community Development Final Instituion option for the ATR/QM Exempt Creditor setting</summary>
    CommunityDevelopmentFinancialInstitution,
    /// <summary>Indicates Community Housing Development Organization option for the ATR/QM Exempt Creditor setting</summary>
    CommunityHousingDevelopmentOrganization,
    /// <summary>Indicates Downpayment Assisance Provider option for the ATR/QM Exempt Creditor setting</summary>
    DownpaymentAssistanceProvider,
    /// <summary>Indicates Non Profit Organization option for the ATR/QM Exempt Creditor setting</summary>
    NonprofitOrganization,
  }
}
