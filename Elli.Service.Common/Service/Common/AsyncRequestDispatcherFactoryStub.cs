// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.AsyncRequestDispatcherFactoryStub
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

#nullable disable
namespace Elli.Service.Common
{
  public class AsyncRequestDispatcherFactoryStub : IAsyncRequestDispatcherFactory
  {
    private readonly AsyncRequestDispatcherStub _asyncRequestDispatcherStub;

    public AsyncRequestDispatcherFactoryStub(
      AsyncRequestDispatcherStub asyncRequestDispatcherStub)
    {
      this._asyncRequestDispatcherStub = asyncRequestDispatcherStub;
    }

    public IAsyncRequestDispatcher CreateAsyncRequestDispatcher()
    {
      return (IAsyncRequestDispatcher) this._asyncRequestDispatcherStub;
    }
  }
}
