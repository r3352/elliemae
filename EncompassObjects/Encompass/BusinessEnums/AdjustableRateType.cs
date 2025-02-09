// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.AdjustableRateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Represents a single adjustable rate type that can be selected for a loan.
  /// </summary>
  public class AdjustableRateType : EnumItem, IAdjustableRateType
  {
    private ARMType armType;

    internal AdjustableRateType(int index, ARMType armType)
      : base(index, armType.Description)
    {
      this.armType = armType;
    }

    /// <summary>Gets the type identifier for the ARM type.</summary>
    public string TypeCode => this.armType.TypeID;
  }
}
