// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrderType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumerated the type of document orders supported by the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" /> object.
  /// </summary>
  [Flags]
  public enum DocumentOrderType
  {
    /// <summary>No DocumentOrderType selected</summary>
    None = 0,
    /// <summary>DocumentOrderType - Opening</summary>
    Opening = 1,
    /// <summary>DocumentOrderType - Closing</summary>
    Closing = 2,
    /// <summary>Both the DocumentOrderTypes selected</summary>
    Both = Closing | Opening, // 0x00000003
  }
}
