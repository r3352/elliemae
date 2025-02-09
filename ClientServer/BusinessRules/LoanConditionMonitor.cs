// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanConditionMonitor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanConditionMonitor : IDisposable
  {
    private static int nextId;
    private int id = Interlocked.Increment(ref LoanConditionMonitor.nextId);
    private ConditionEvaluator evaluator;
    private EllieMae.EMLite.Customization.ExecutionContext context;
    private bool active;

    public event EventHandler ActiveStateChanged;

    public LoanConditionMonitor(ConditionEvaluator evaluator, EllieMae.EMLite.Customization.ExecutionContext context)
    {
      this.evaluator = evaluator;
      this.context = context;
      this.active = evaluator.AppliesTo(context);
    }

    public ConditionEvaluator ConditionEvaluator => this.evaluator;

    public EllieMae.EMLite.Customization.ExecutionContext Context => this.context;

    public bool Active => this.active;

    public bool Reevaluate()
    {
      if (this.evaluator == null)
        return false;
      bool flag = this.evaluator.AppliesTo(this.context);
      if (this.active == flag)
        return false;
      this.active = flag;
      if (this.ActiveStateChanged != null)
        this.ActiveStateChanged((object) this, EventArgs.Empty);
      return true;
    }

    public virtual void Dispose()
    {
      this.context = (EllieMae.EMLite.Customization.ExecutionContext) null;
      this.evaluator = (ConditionEvaluator) null;
    }
  }
}
