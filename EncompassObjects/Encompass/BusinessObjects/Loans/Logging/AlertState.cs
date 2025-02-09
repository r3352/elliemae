// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.AlertState
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumerates the possible states for a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert" />.
  /// </summary>
  [Guid("14B61907-6C47-4aa5-BB10-366405C9C887")]
  public enum AlertState
  {
    /// <summary>Indicates the alert's due date has not yet been reached</summary>
    Pending = 1,
    /// <summary>Indicates the alert's due date has passed</summary>
    Overdue = 2,
    /// <summary>Indicates the alert has been followed up on and is considered closed</summary>
    Complete = 3,
  }
}
