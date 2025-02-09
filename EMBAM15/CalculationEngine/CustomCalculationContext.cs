// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculationContext
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CustomCalculationContext(
    UserInfo currentUser,
    LoanData loan,
    IServerDataProvider serverDataProvider) : 
    ExecutionContext(currentUser, loan, serverDataProvider, true),
    ICalculationContext,
    IExecutionContext,
    IDisposable,
    ICloneable
  {
    public override object Clone()
    {
      return (object) new CustomCalculationContext(this.User, this.Loan, this.ServerDataProvider);
    }

    public override void Dispose() => base.Dispose();
  }
}
