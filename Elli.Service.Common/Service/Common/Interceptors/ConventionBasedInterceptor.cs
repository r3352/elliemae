// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Interceptors.ConventionBasedInterceptor
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.InversionOfControl;
using System;

#nullable disable
namespace Elli.Service.Common.Interceptors
{
  public abstract class ConventionBasedInterceptor : 
    Disposable,
    IRequestHandlerInterceptor,
    IDisposable
  {
    public abstract void BeforeHandlingRequest(RequestProcessingContext context);

    public abstract void AfterHandlingRequest(RequestProcessingContext context);

    protected IConventions Conventions { get; private set; }

    protected ConventionBasedInterceptor()
    {
      this.Conventions = IoC.Container.GetInstance<IConventions>();
    }

    public Response CreateDefaultResponseFor(Request request)
    {
      return (Response) Activator.CreateInstance(this.Conventions.GetResponseTypeFor(request));
    }
  }
}
