// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.AdjustableRateTypes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>Represents the collection of adjustable rate types.</summary>
  public class AdjustableRateTypes : EnumBase, IAdjustableRateTypes
  {
    internal AdjustableRateTypes()
    {
      int num = 0;
      foreach (ARMType armType in (IEnumerable<ARMType>) ARMTypeList.ARMTypes)
        this.AddItem((EnumItem) new AdjustableRateType(num++, armType));
    }

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.AdjustableRateType">AdjustableRateType</see> with the specified index.</summary>
    public AdjustableRateType this[int index] => (AdjustableRateType) this.GetItem(index);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.AdjustableRateType">AdjustableRateType</see> with the specified ID value.</summary>
    public AdjustableRateType GetItemByID(int itemId)
    {
      return (AdjustableRateType) base.GetItemByID(itemId);
    }

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.AdjustableRateType">AdjustableRateType</see> with the specified name.</summary>
    /// <param name="name">The name of the item being retrieved (case insensitive).</param>
    public AdjustableRateType GetItemByName(string name)
    {
      return (AdjustableRateType) base.GetItemByName(name);
    }

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.AdjustableRateType">AdjustableRateType</see> with the specified TypeCode.</summary>
    /// <param name="typeCode">The TypeCode of the item being retrieved (case insensitive).</param>
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
