// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ConditionType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumeration of the different condition types available in Encompass.
  /// </summary>
  [Guid("757985C5-DF16-409e-8037-25F57280CD10")]
  public enum ConditionType
  {
    /// <summary>Represents an underwriting condition.</summary>
    Underwriting = 1,
    /// <summary>Represents a post-closing or shipping condition.</summary>
    PostClosing = 2,
    /// <summary>Represents a preliminary condition.</summary>
    Preliminary = 3,
  }
}
