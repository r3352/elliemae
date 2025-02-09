// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.DelayedTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class DelayedTrigger
  {
    private ExecutionContext context;

    public DelayedTrigger(ExecutionContext context) => this.context = context;

    public ExecutionContext Context => this.context;

    public abstract bool SupportsDirectExecution { get; }

    public abstract bool IsActivated();

    public abstract void Reset();

    public abstract void ResubscribeToFieldEvents();

    public abstract bool Execute(LoanDataMgr dataMgr);

    public abstract void ResetFieldValue(string fieldId, string val);
  }
}
