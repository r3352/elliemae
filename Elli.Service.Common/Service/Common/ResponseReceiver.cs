// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.ResponseReceiver
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common
{
  public class ResponseReceiver
  {
    private readonly Action<ReceivedResponses> _responseReceivedCallback;
    private readonly Action<ExceptionInfo, ExceptionType> _exceptionAndTypeOccuredCallback;
    private readonly Action<ExceptionInfo> _exceptionOccurredCallback;
    private readonly Dictionary<string, int> _keyToResultPositions;
    private readonly ICacheManager _cacheManager;

    public ResponseReceiver(
      Action<ReceivedResponses> responseReceivedCallback,
      Action<ExceptionInfo> exceptionOccurredCallback,
      Dictionary<string, int> keyToResultPositions,
      ICacheManager cacheManager)
    {
      if (responseReceivedCallback == null)
        throw new ArgumentNullException(nameof (responseReceivedCallback));
      if (exceptionOccurredCallback == null)
        throw new ArgumentNullException(nameof (exceptionOccurredCallback));
      this._responseReceivedCallback = responseReceivedCallback;
      this._exceptionOccurredCallback = exceptionOccurredCallback;
      this._keyToResultPositions = keyToResultPositions;
      this._cacheManager = cacheManager;
    }

    public ResponseReceiver(
      Action<ReceivedResponses> responseReceivedCallback,
      Action<ExceptionInfo, ExceptionType> exceptionAndTypeOccuredCallback,
      Dictionary<string, int> keyToResultPositions,
      ICacheManager cacheManager)
    {
      if (responseReceivedCallback == null)
        throw new ArgumentNullException(nameof (responseReceivedCallback));
      if (exceptionAndTypeOccuredCallback == null)
        throw new ArgumentNullException(nameof (exceptionAndTypeOccuredCallback));
      this._responseReceivedCallback = responseReceivedCallback;
      this._exceptionAndTypeOccuredCallback = exceptionAndTypeOccuredCallback;
      this._keyToResultPositions = keyToResultPositions;
      this._cacheManager = cacheManager;
    }

    public void ReceiveResponses(
      ProcessRequestsAsyncCompletedArgs args,
      Response[] tempResponseArray,
      Request[] requestsToSendAsArray)
    {
      if (args == null)
        this._responseReceivedCallback(new ReceivedResponses(tempResponseArray, this._keyToResultPositions));
      else if (ResponseReceiver.HasException(args))
      {
        this.HandleException(args);
      }
      else
      {
        if (this._responseReceivedCallback.Target is Disposable target && target.IsDisposed)
          return;
        Response[] result = args.Result;
        this.AddCacheableResponsesToCache(result, requestsToSendAsArray);
        this.PutReceivedResponsesInTempResponseArray(tempResponseArray, result);
        this._responseReceivedCallback(new ReceivedResponses(tempResponseArray, this._keyToResultPositions));
      }
    }

    private void AddCacheableResponsesToCache(
      Response[] receivedResponses,
      Request[] requestsToSend)
    {
      if (this._cacheManager == null)
        return;
      for (int index = 0; index < receivedResponses.Length; ++index)
      {
        if (receivedResponses[index].ExceptionType == ExceptionType.None && this._cacheManager.IsCachingEnabledFor(requestsToSend[index].GetType()))
          this._cacheManager.StoreInCache(requestsToSend[index], receivedResponses[index]);
      }
    }

    private void PutReceivedResponsesInTempResponseArray(
      Response[] tempResponseArray,
      Response[] receivedResponses)
    {
      int num = 0;
      for (int index = 0; index < tempResponseArray.Length; ++index)
      {
        if (tempResponseArray[index] == null)
          tempResponseArray[index] = receivedResponses[num++];
      }
    }

    private void HandleException(ProcessRequestsAsyncCompletedArgs args)
    {
      if (this._responseReceivedCallback.Target is Disposable target && target.IsDisposed)
        return;
      ExceptionInfo exception = ResponseReceiver.GetException(args);
      if (this._exceptionOccurredCallback != null)
        this._exceptionOccurredCallback(exception);
      else if (this._exceptionAndTypeOccuredCallback != null)
      {
        ExceptionType exceptionType = ResponseReceiver.GetExceptionType(args);
        this._exceptionAndTypeOccuredCallback(exception, exceptionType);
      }
      else
        this._responseReceivedCallback(new ReceivedResponses(args.Result, this._keyToResultPositions));
    }

    private static bool HasException(ProcessRequestsAsyncCompletedArgs args)
    {
      return args.Error != null || ((IEnumerable<Response>) args.Result).Any<Response>((Func<Response, bool>) (r => r.Exception != null));
    }

    private static ExceptionInfo GetException(ProcessRequestsAsyncCompletedArgs args)
    {
      if (args.Error != null)
        return new ExceptionInfo(args.Error);
      return ResponseReceiver.GetFirstException((IEnumerable<Response>) args.Result)?.Exception;
    }

    private static ExceptionType GetExceptionType(ProcessRequestsAsyncCompletedArgs args)
    {
      if (args.Error != null)
        return ExceptionType.Unknown;
      Response firstException = ResponseReceiver.GetFirstException((IEnumerable<Response>) args.Result);
      return firstException != null ? firstException.ExceptionType : ExceptionType.Unknown;
    }

    private static Response GetFirstException(IEnumerable<Response> responsesToCheck)
    {
      return responsesToCheck.FirstOrDefault<Response>((Func<Response, bool>) (r => r.Exception != null));
    }
  }
}
