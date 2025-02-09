// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.AdjustableRateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class AdjustableRateType : EnumItem, IAdjustableRateType
  {
    private ARMType armType;

    internal AdjustableRateType(int index, ARMType armType)
      : base(index, armType.Description)
    {
      this.armType = armType;
    }

    public string TypeCode => this.armType.TypeID;
  }
}
