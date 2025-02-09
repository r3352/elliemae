// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.LogTargetFactoryBase
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public abstract class LogTargetFactoryBase : ILogTargetFactory
  {
    protected readonly ILogManager _logManager;
    private readonly int _id;
    private readonly IAsyncTargetWrapperFactory _asyncTargetWrapperFactory;

    public LogTargetFactoryBase(ILogManager logManager)
    {
      this._logManager = ArgumentChecks.IsNotNull<ILogManager>(logManager, nameof (logManager));
      this._id = Guid.NewGuid().GetHashCode();
      this._asyncTargetWrapperFactory = (IAsyncTargetWrapperFactory) null;
    }

    public LogTargetFactoryBase(
      ILogManager logManager,
      IAsyncTargetWrapperFactory asyncTargetWrapperFactory)
      : this(logManager)
    {
      this._asyncTargetWrapperFactory = asyncTargetWrapperFactory;
    }

    protected abstract ILogTarget GetTargetInternal();

    public virtual ILogTarget GetTarget()
    {
      ILogTarget targetInternal = this.GetTargetInternal();
      return this._asyncTargetWrapperFactory != null ? this._asyncTargetWrapperFactory.WrapTarget(targetInternal) : targetInternal;
    }

    public override int GetHashCode() => this._id;

    public override bool Equals(object obj)
    {
      return obj != null && obj is LogTargetFactoryBase && obj.GetHashCode() == this._id;
    }
  }
}
