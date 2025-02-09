// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.BrowserControls.EncWebFormBrowserControl
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.EventObjects;
using Elli.Web.Host.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.BrowserControls
{
  public abstract class EncWebFormBrowserControl : UserControl, IWebFormBrowser
  {
    public event EventHandler<BeforeNavigationEventArgs> BeforeNavigation;

    public event PageCompleteEventHandler PageComplete;

    public event EventHandler<MouseReleasedEventArgs> MouseReleased;

    public event EventHandler ProcessSelectionChange;

    public event EventHandler ProcessReadyStateChange;

    public event EventHandler<HtmlEditorKeyDownEventArgs> ProcessKeyDown;

    public event EventHandler ContentChanged;

    public event EventHandler<TitleChangeEventArgs> BrowserTitleChanged;

    public abstract bool IsDocumentExists { get; }

    protected virtual void OnBeforeNavigation(object sender, BeforeNavigationEventArgs e)
    {
      EventHandler<BeforeNavigationEventArgs> beforeNavigation = this.BeforeNavigation;
      if (beforeNavigation == null)
        return;
      beforeNavigation(sender, e);
    }

    protected virtual void OnPageComplete(object sender, FinishedLoadingEventArgs e)
    {
      PageCompleteEventHandler pageComplete = this.PageComplete;
      if (pageComplete == null)
        return;
      pageComplete(sender, e);
    }

    protected virtual void OnMouseReleased(object sender, MouseReleasedEventArgs e)
    {
      EventHandler<MouseReleasedEventArgs> mouseReleased = this.MouseReleased;
      if (mouseReleased == null)
        return;
      mouseReleased(sender, e);
    }

    protected virtual void OnProcessSelectionChange(object sender, EventArgs e)
    {
      this.ProcessSelectionChange(sender, e);
    }

    protected virtual void OnProcessReadyStateChange(object sender, EventArgs e)
    {
      this.ProcessReadyStateChange(sender, e);
    }

    protected virtual void OnProcessKeyDown(object sender, HtmlEditorKeyDownEventArgs e)
    {
      this.ProcessKeyDown(sender, e);
    }

    protected virtual void OnContentChanged(object sender, EventArgs e)
    {
      this.ContentChanged(sender, e);
    }

    protected virtual void OnTitleChanged(object sender, TitleChangeEventArgs e)
    {
      EventHandler<TitleChangeEventArgs> browserTitleChanged = this.BrowserTitleChanged;
      if (browserTitleChanged == null)
        return;
      browserTitleChanged(sender, e);
    }

    public abstract string GetBrowserHtml([Optional] object frameName);

    public abstract void Navigate(string url);

    public abstract void Navigate(
      string url,
      string postData,
      Dictionary<string, string> headerCollection);

    public abstract void LoadHtml(string html);

    public abstract void LoadHtml(string html, bool documentReadonly);

    public abstract bool ExecuteCommand(string command, object value = null);

    public abstract bool IsQueryCommandEnabled(string command);

    public abstract string GetQueryCommandValue(string command);

    public abstract void AddDomEvents(string eventName, object caller);

    public abstract string GetHtmlBodyText([Optional] object frameName);

    public abstract void InsertField(string fieldID, string fieldName);

    public abstract Color GetSelectedFontColor();

    public virtual void InvokeHtmlEvent()
    {
    }

    public abstract void ShowContextMenu();

    public abstract void SetOpaqueBackground();
  }
}
