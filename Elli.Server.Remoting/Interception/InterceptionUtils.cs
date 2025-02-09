// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Interception.InterceptionUtils
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;
using Unity.Interception;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.Interceptors.TypeInterceptors;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

#nullable disable
namespace Elli.Server.Remoting.Interception
{
  public static class InterceptionUtils
  {
    public static T NewInstance<T>() where T : RemotedObject, new()
    {
      return Intercept.NewInstance<T>((ITypeInterceptor) new VirtualMethodInterceptor(), (IEnumerable<IInterceptionBehavior>) new IInterceptionBehavior[1]
      {
        (IInterceptionBehavior) new RemoteCallBehavior()
      });
    }
  }
}
