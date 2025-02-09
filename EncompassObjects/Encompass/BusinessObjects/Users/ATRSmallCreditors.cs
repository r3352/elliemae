// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ATRSmallCreditors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Defines the possbile options for ATR Small Creditor setting
  /// </summary>
  [Guid("C51EE638-DFB9-4D3C-A7C5-993846ADA551")]
  public enum ATRSmallCreditors
  {
    /// <summary>None</summary>
    None,
    /// <summary>Indicates Small Creditor for the ATR/QM Small Creditor setting</summary>
    SmallCreditor,
    /// <summary>Indicates Rural Small Creditor for the ATR/QM Small Creditor setting</summary>
    RuralSmallCreditor,
  }
}
