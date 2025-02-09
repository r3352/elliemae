// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.ServicingType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public enum ServicingType
  {
    [Description("None")] None = -999, // 0xFFFFFC19
    [Description("Servicing Released")] ServicingReleased = 0,
    [Description("Servicing Retained")] ServicingRetained = 1,
  }
}
