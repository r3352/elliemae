// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.DelegateInvoker
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class DelegateInvoker
  {
    protected readonly Delegate Delegate;
    protected readonly object[] Parameters;

    public DelegateInvoker(Delegate dlgt, params object[] args)
    {
      this.Delegate = dlgt;
      this.Parameters = args;
    }

    public virtual object Invoke() => this.Delegate.DynamicInvoke(this.Parameters);
  }
}
