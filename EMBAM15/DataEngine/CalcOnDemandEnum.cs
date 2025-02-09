// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CalcOnDemandEnum
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Flags]
  public enum CalcOnDemandEnum
  {
    None = 0,
    All = 1,
    PaymentSchedule = 2,
    Update2010ItemizationFrom2015Itemization = 4,
    FundingFees = 8,
    CreateConstructionLinkedSubsetFields = 16, // 0x00000010
    MaxPaymentSample = 32, // 0x00000020
  }
}
