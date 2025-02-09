// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.JavascriptFunctionCallback`1
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Js;
using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Elli.Web.Host
{
  public class JavascriptFunctionCallback<T>
  {
    private const string className = "JavascriptFunctionCallback";
    private static readonly string sw = Tracing.SwThinThick;
    private ManualResetEvent _mre;
    private T _data;

    public JavascriptFunctionCallback()
    {
      this._mre = new ManualResetEvent(false);
      this._data = default (T);
    }

    public T WaitForCallback(int millisecondsTimeout)
    {
      Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Calling ManualResetEvent.WaitOne: " + (object) millisecondsTimeout);
      if (this._mre.WaitOne(millisecondsTimeout))
        return this._data;
      throw new TimeoutException("Callback timed out");
    }

    public object Callback(IJsObject jsObject)
    {
      Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Checking JsObject.Properties");
      if (jsObject != null && jsObject.Properties.Contains("length") && !typeof (T).IsArray)
      {
        Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Getting JsObject.Properties.length");
        int int32 = Convert.ToInt32(jsObject.Properties["length"]);
        Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Checking JsObject.Properties.length: " + (object) int32);
        if (int32 > 0)
        {
          Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Getting JsObject.Properties[0]");
          jsObject = jsObject.Properties[0U] as IJsObject;
        }
        else
        {
          Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "JsObject is an empty array");
          jsObject = (IJsObject) null;
        }
      }
      Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Checking JsObject");
      if (jsObject != null)
      {
        Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Calling JsObject.ToJsonString");
        string jsonString = jsObject.ToJsonString();
        Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Deserializing Object: " + jsonString);
        this._data = JsonConvert.DeserializeObject<T>(jsonString);
      }
      Tracing.Log(JavascriptFunctionCallback<T>.sw, TraceLevel.Verbose, nameof (JavascriptFunctionCallback<T>), "Calling ManualResetEvent.Set");
      this._mre.Set();
      return (object) null;
    }
  }
}
