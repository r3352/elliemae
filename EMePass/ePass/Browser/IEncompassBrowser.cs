// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.IEncompassBrowser
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public interface IEncompassBrowser
  {
    event EncFinishLoadingFrameEvent FinishLoadingFrameEvent;

    event EncStartLoadingFrameEvent StartLoadingFrameEvent;

    event EncPrintJobEvent PrintJobStatusChangeEvent;

    event EncConsoleMessageEvent ConsoleMessageEvent;

    event EncBeforeNavigationEvent BeforeNavigationEvent;

    void Navigate(string url, string postData, Dictionary<string, string> headerCollection);

    void Navigate(string url);

    void Close();

    bool ShowDebugPopUp();

    void Reload(bool ignoreCache = false);

    void ExecuteJavascript(string javascriptCode, [Optional] long? frameId);

    void LoadModule(
      string hostUrl,
      string guestUrl,
      EncModuleParameters parameters,
      bool allowDragDrop = false);

    void LoadModule(string guestUrl, EncModuleParameters parameters);

    void LoadHtml(string html);

    T ExecuteAndReturnValue<T>(string functionName, object[] eventParams = null);

    T RaiseEvent<T>(string eventType, object eventParams, int millisecondsTimeout);

    bool SetJsObjectProperty(string sourceName, string propertyName, object propertyValue);

    void BeginPrintWebPage(EncPrintSettings printSettings, [Optional] long? frameId, bool forcePrint = false);

    void InitCustomPopupHandler(Form parentWindowForm);

    string GetBrowserHtmlContent([Optional] long? frameId);

    bool DOMElementExists([Optional] object frameName);
  }
}
