// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.ValueType
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public enum ValueType
  {
    None = 0,
    Object = 1,
    Integer = 2,
    Decimal = 4,
    String = 8,
    Date = 22, // 0x00000016
    DateTime = 50, // 0x00000032
    Boolean = 100, // 0x00000064
    NullableBoolean = 296, // 0x00000128
    NullableDecimal = 598, // 0x00000256
    NullableInteger = 1298, // 0x00000512
    NullableDate = 4132, // 0x00001024
    NullableDateTime = 8264, // 0x00002048
    Short = 16534, // 0x00004096
    Byte = 33170, // 0x00008192
    NullableShort = 91012, // 0x00016384
  }
}
