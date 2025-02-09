// Decompiled with JetBrains decompiler
// Type: Elli.Interface.Provider.ILoanSourceProvider`1
// Assembly: Elli.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E2665EB5-354E-4837-8094-9A1310A6D998
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Interface.dll

using System;

#nullable disable
namespace Elli.Interface.Provider
{
  public interface ILoanSourceProvider<LoanObjectType>
  {
    LoanObjectType GetLoan(Guid loanId);
  }
}
