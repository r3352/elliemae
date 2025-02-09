// Decompiled with JetBrains decompiler
// Type: Elli.ElliEnum.InternalFieldPopulationOption
// Assembly: Elli.ElliEnum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F027F78E-EB94-4F7C-9C8B-5B69BCA83B9B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.ElliEnum.dll

using System;

#nullable disable
namespace Elli.ElliEnum
{
  [Flags]
  public enum InternalFieldPopulationOption
  {
    None = 0,
    CopyBorrowersToLockRequest = 1,
    CopyLoanInfoToLockRequestForm = 2,
    PopulateOthers = 4,
    ClearMipFields = 8,
    UpdateProposedMortgageAmount = 16, // 0x00000010
    SwitchToURLA2020 = 32, // 0x00000020
    SetLockValidationStatus = 64, // 0x00000040
    GFEApplicationDate = 128, // 0x00000080
    UpdateUpbAmount = 256, // 0x00000100
  }
}
