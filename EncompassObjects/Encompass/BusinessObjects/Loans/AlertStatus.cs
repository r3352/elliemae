// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AlertStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// The possible status of a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert" /> object.
  /// </summary>
  public enum AlertStatus
  {
    /// <summary>No status is provided</summary>
    None,
    /// <summary>The specified item is due to be completed or received</summary>
    Due,
    /// <summary>The specified item is set to expire</summary>
    Expires,
    /// <summary>The alert signals eligibility for a special offer</summary>
    Eligible,
    /// <summary>The alert signals that a special offer has been received</summary>
    Received,
  }
}
