// Decompiled with JetBrains decompiler
// Type: Elli.ElliEnum.LoanSecurityOptions
// Assembly: Elli.ElliEnum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F027F78E-EB94-4F7C-9C8B-5B69BCA83B9B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.ElliEnum.dll

using System;

#nullable disable
namespace Elli.ElliEnum
{
  [Flags]
  public enum LoanSecurityOptions
  {
    None = 0,
    ApplyFieldPolicyRules = 1,
    ApplyLoanActivityRestrictions = 2,
    All = ApplyLoanActivityRestrictions | ApplyFieldPolicyRules, // 0x00000003
  }
}
