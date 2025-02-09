// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Interception.RemoteCallBehavior
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Interfaces;
using System;
using System.Collections.Generic;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

#nullable disable
namespace Elli.Server.Remoting.Interception
{
  public class RemoteCallBehavior : IInterceptionBehavior
  {
    public static readonly InjectionMember InjectionMember = (InjectionMember) new InterceptionBehavior<RemoteCallBehavior>();

    public bool WillExecute => true;

    public IEnumerable<Type> GetRequiredInterfaces() => (IEnumerable<Type>) Type.EmptyTypes;

    public IMethodReturn Invoke(
      IMethodInvocation input,
      GetNextInterceptionBehaviorDelegate getNext)
    {
      using (IRequestContext request = input.Target is ISessionBoundObject target ? target.Session?.Context?.MakeCurrent() : (IRequestContext) null)
      {
        using (RemoteCallTracker remoteCallTracker = RemoteCallTracker.New(target?.Session, request, input))
        {
          IMethodReturn methodReturn = getNext()(input, getNext);
          remoteCallTracker?.SetMethodReturn(methodReturn);
          return methodReturn;
        }
      }
    }
  }
}
