// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.ProcessRequestsAsyncCompletedArgs
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Elli.Service.Common
{
  public class ProcessRequestsAsyncCompletedArgs : AsyncCompletedEventArgs
  {
    private readonly object[] _results;

    public ProcessRequestsAsyncCompletedArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this._results = results;
    }

    public Response[] Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this._results[0] as Response[];
      }
    }
  }
}
