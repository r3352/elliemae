// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.AdjustableRateTypes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class AdjustableRateTypes : EnumBase, IAdjustableRateTypes
  {
    internal AdjustableRateTypes()
    {
      int num = 0;
      foreach (ARMType armType in (IEnumerable<ARMType>) ARMTypeList.ARMTypes)
        this.AddItem((EnumItem) new AdjustableRateType(num++, armType));
    }

    public AdjustableRateType this[int index] => (AdjustableRateType) this.GetItem(index);

    public AdjustableRateType GetItemByID(int itemId)
    {
      return (AdjustableRateType) base.GetItemByID(itemId);
    }

    public AdjustableRateType GetItemByName(string name)
    {
      return (AdjustableRateType) base.GetItemByName(name);
    }

    public AdjustableRateType GetItemByTypeCode(string typeCode)
    {
      foreach (AdjustableRateType itemByTypeCode in (EnumBase) this)
      {
        if (string.Compare(itemByTypeCode.TypeCode, typeCode, true) == 0)
          return itemByTypeCode;
      }
      return (AdjustableRateType) null;
    }
  }
}
