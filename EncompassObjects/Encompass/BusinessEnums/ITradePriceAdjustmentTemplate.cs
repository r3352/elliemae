// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ITradePriceAdjustmentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>Interface for PriceAdjustmentTemplate class</summary>
  [Guid("53742b23-71a9-4dcf-82f2-b5e87e714082")]
  public interface ITradePriceAdjustmentTemplate
  {
    string Name { get; }

    string Description { get; }

    string GUID { get; }
  }
}
