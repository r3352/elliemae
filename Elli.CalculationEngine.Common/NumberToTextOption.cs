// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.NumberToTextOption
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  [Flags]
  public enum NumberToTextOption
  {
    None = 0,
    TwoDecimalPlaces = 1,
    DollarsAndCents = 2,
    BlankIfZero = 4,
  }
}
