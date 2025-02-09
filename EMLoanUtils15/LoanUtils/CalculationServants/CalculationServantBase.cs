// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.CalculationServantBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public abstract class CalculationServantBase
  {
    protected static readonly string TraceSwitch = Tracing.SwDataEngine;
    protected static readonly string Nil = string.Empty;
    protected readonly ILoanModelProvider ModelProvider;
    protected readonly LoanDisclosedChecker LoanDisclosedChecker;

    protected CalculationServantBase(ILoanModelProvider modelProvider)
    {
      this.ModelProvider = modelProvider;
      this.LoanDisclosedChecker = new LoanDisclosedChecker(modelProvider);
    }

    protected ICalculationObjects CalculationObjects => this.ModelProvider.CalculationObjects;

    protected virtual bool IsPrimaryPair() => this.ModelProvider.IsPrimaryPair();

    protected bool UseNewGfeHud => this.ModelProvider.UseNewGFEHUD;

    protected bool UseNew2015GFEHUD => this.ModelProvider.UseNew2015GFEHUD;

    protected virtual string Val(string id) => this.ModelProvider.Val(id);

    protected virtual string Val(string id, int borrowerPairIndex)
    {
      return this.ModelProvider.Val(id, borrowerPairIndex);
    }

    protected virtual void SetVal(string id, string value) => this.ModelProvider.SetVal(id, value);

    protected virtual double FltVal(string id) => this.ModelProvider.FltVal(id);

    protected virtual void SetCurrentNum(string id, double num)
    {
      this.ModelProvider.SetCurrentNum(id, num, false);
    }

    protected virtual void SetVal(string id, string value, int borrowerPairIndex)
    {
      this.ModelProvider.SetVal(id, value, borrowerPairIndex);
    }

    protected double ParseDouble(string value) => this.ModelProvider.Flt(value);

    protected virtual void SplitName(string id1, string id2, string name)
    {
      name = name.Trim();
      if (name.IndexOf(" ", StringComparison.Ordinal) == -1)
      {
        this.SetVal(id1, name);
        this.SetVal(id2, "");
      }
      else
      {
        int length = name.IndexOf(" ", StringComparison.Ordinal);
        this.SetVal(id1, name.Substring(0, length).Trim());
        this.SetVal(id2, name.Substring(length + 1).Trim());
      }
    }

    public bool IsLoanDisclosed(bool manualSyncItemization)
    {
      return this.LoanDisclosedChecker.IsLoanDisclosed(manualSyncItemization);
    }
  }
}
