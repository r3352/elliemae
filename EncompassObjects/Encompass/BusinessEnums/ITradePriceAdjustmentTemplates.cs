// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ITradePriceAdjustmentTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>Interface for PriceAdjustmentTemplates class</summary>
  [Guid("65a9c369-a4b8-4cf6-80a0-ad5e536b0d99")]
  public interface ITradePriceAdjustmentTemplates
  {
    TradePriceAdjustmentTemplate this[int index] { get; }

    TradePriceAdjustmentTemplate this[string name] { get; }

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}
