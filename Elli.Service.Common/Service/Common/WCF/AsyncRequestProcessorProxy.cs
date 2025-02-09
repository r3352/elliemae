// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.WCF.AsyncRequestProcessorProxy
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

#nullable disable
namespace Elli.Service.Common.WCF
{
  public class AsyncRequestProcessorProxy : 
    ClientBase<IAsyncWcfRequestProcessor>,
    IAsyncRequestProcessor,
    IDisposable
  {
    public event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

    public AsyncRequestProcessorProxy()
    {
    }

    public AsyncRequestProcessorProxy(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public AsyncRequestProcessorProxy(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public AsyncRequestProcessorProxy(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public AsyncRequestProcessorProxy(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    IAsyncResult IAsyncRequestProcessor.BeginProcessRequests(
      Request[] requests,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Channel.BeginProcessRequests(requests, callback, asyncState);
    }

    IAsyncResult IAsyncRequestProcessor.BeginProcessOneWayRequests(
      OneWayRequest[] oneWayRequests,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Channel.BeginProcessOneWayRequests(oneWayRequests, callback, asyncState);
    }

    Response[] IAsyncRequestProcessor.EndProcessRequests(IAsyncResult result)
    {
      return this.Channel.EndProcessRequests(result);
    }

    void IAsyncRequestProcessor.EndProcessOneWayRequests(IAsyncResult result)
    {
      this.Channel.EndProcessOneWayRequests(result);
    }

    private IAsyncResult OnBeginProcessRequests(
      object[] inValues,
      AsyncCallback callback,
      object asyncState)
    {
      return ((IAsyncRequestProcessor) this).BeginProcessRequests((Request[]) inValues[0], callback, asyncState);
    }

    private IAsyncResult OnBeginProcessOneWayRequests(
      object[] inValues,
      AsyncCallback callback,
      object asyncState)
    {
      return ((IAsyncRequestProcessor) this).BeginProcessOneWayRequests((OneWayRequest[]) inValues[0], callback, asyncState);
    }

    private object[] OnEndProcessRequests(IAsyncResult result)
    {
      return new object[1]
      {
        (object) ((IAsyncRequestProcessor) this).EndProcessRequests(result)
      };
    }

    private object[] OnEndProcessOneWayRequests(IAsyncResult result)
    {
      ((IAsyncRequestProcessor) this).EndProcessOneWayRequests(result);
      return new object[0];
    }

    private void OnProcessRequestsCompleted(object state)
    {
      ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs) state;
      ((Action<ProcessRequestsAsyncCompletedArgs>) completedEventArgs.UserState)(new ProcessRequestsAsyncCompletedArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    private void OnProcessOneWayRequestsCompleted(object state)
    {
      ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs) state;
      ((Action<AsyncCompletedEventArgs>) completedEventArgs.UserState)((AsyncCompletedEventArgs) new ProcessRequestsAsyncCompletedArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public void ProcessRequestsAsync(
      Request[] requests,
      Action<ProcessRequestsAsyncCompletedArgs> processCompleted)
    {
      this.InvokeAsync(new ClientBase<IAsyncWcfRequestProcessor>.BeginOperationDelegate(this.OnBeginProcessRequests), new object[1]
      {
        (object) requests
      }, new ClientBase<IAsyncWcfRequestProcessor>.EndOperationDelegate(this.OnEndProcessRequests), new SendOrPostCallback(this.OnProcessRequestsCompleted), (object) processCompleted);
    }

    public void ProcessOneWayRequestsAsync(
      OneWayRequest[] oneWayRequests,
      Action<AsyncCompletedEventArgs> processCompleted)
    {
      this.InvokeAsync(new ClientBase<IAsyncWcfRequestProcessor>.BeginOperationDelegate(this.OnBeginProcessOneWayRequests), new object[1]
      {
        (object) oneWayRequests
      }, new ClientBase<IAsyncWcfRequestProcessor>.EndOperationDelegate(this.OnEndProcessOneWayRequests), new SendOrPostCallback(this.OnProcessOneWayRequestsCompleted), (object) processCompleted);
    }

    private IAsyncResult OnBeginOpen(object[] inValues, AsyncCallback callback, object asyncState)
    {
      return ((ICommunicationObject) this).BeginOpen(callback, asyncState);
    }

    private object[] OnEndOpen(IAsyncResult result)
    {
      ((ICommunicationObject) this).EndOpen(result);
      return (object[]) null;
    }

    private void OnOpenCompleted(object state)
    {
      if (this.OpenCompleted == null)
        return;
      ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs) state;
      this.OpenCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public void OpenAsync() => this.OpenAsync((object) null);

    public void OpenAsync(object userState)
    {
      this.InvokeAsync(new ClientBase<IAsyncWcfRequestProcessor>.BeginOperationDelegate(this.OnBeginOpen), (object[]) null, new ClientBase<IAsyncWcfRequestProcessor>.EndOperationDelegate(this.OnEndOpen), new SendOrPostCallback(this.OnOpenCompleted), userState);
    }

    private IAsyncResult OnBeginClose(object[] inValues, AsyncCallback callback, object asyncState)
    {
      return ((ICommunicationObject) this).BeginClose(callback, asyncState);
    }

    private object[] OnEndClose(IAsyncResult result)
    {
      ((ICommunicationObject) this).EndClose(result);
      return (object[]) null;
    }

    private void OnCloseCompleted(object state)
    {
      ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<IAsyncWcfRequestProcessor>.InvokeAsyncCompletedEventArgs) state;
      this.CloseCompleted(new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public void CloseCompleted(AsyncCompletedEventArgs args)
    {
      if (args.Error == null)
        return;
      this.Abort();
    }

    public void CloseAsync() => this.CloseAsync((object) null);

    public void CloseAsync(object userState)
    {
      this.InvokeAsync(new ClientBase<IAsyncWcfRequestProcessor>.BeginOperationDelegate(this.OnBeginClose), (object[]) null, new ClientBase<IAsyncWcfRequestProcessor>.EndOperationDelegate(this.OnEndClose), new SendOrPostCallback(this.OnCloseCompleted), userState);
    }

    public void Dispose() => this.CloseAsync();
  }
}
