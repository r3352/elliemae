// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.EMWebBrowser
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls
{
  public class EMWebBrowser : WebBrowser
  {
    private AxHost.ConnectionPointCookie cookie;
    private EMWebBrowser.EMWebBrowserEventHelper helper;
    private IContainer components;

    public EMWebBrowser() => this.InitializeComponent();

    public EMWebBrowser(IContainer container)
    {
      container.Add((IComponent) this);
      this.InitializeComponent();
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void CreateSink()
    {
      base.CreateSink();
      this.helper = new EMWebBrowser.EMWebBrowserEventHelper(this);
      this.cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, (object) this.helper, typeof (EMWebBrowser.DWebBrowserEvents2));
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void DetachSink()
    {
      if (this.cookie != null)
      {
        this.cookie.Disconnect();
        this.cookie = (AxHost.ConnectionPointCookie) null;
      }
      base.DetachSink();
    }

    [Category("Action")]
    public event EMWebBrowser.WebBrowserNavigateErrorEventHandler NavigateError;

    protected virtual void OnNavigateError(EMWebBrowser.WebBrowserNavigateErrorEventArgs e)
    {
      if (this.NavigateError == null)
        return;
      this.NavigateError((object) this, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();

    private class EMWebBrowserEventHelper : StandardOleMarshalObject, EMWebBrowser.DWebBrowserEvents2
    {
      private EMWebBrowser parent;

      public EMWebBrowserEventHelper(EMWebBrowser parent) => this.parent = parent;

      public void NavigateError(
        object pDisp,
        ref object url,
        ref object frame,
        ref object statusCode,
        ref bool cancel)
      {
        this.parent.OnNavigateError(new EMWebBrowser.WebBrowserNavigateErrorEventArgs((string) url, (string) frame, (int) statusCode, cancel));
      }
    }

    public delegate void WebBrowserNavigateErrorEventHandler(
      object sender,
      EMWebBrowser.WebBrowserNavigateErrorEventArgs e);

    public class WebBrowserNavigateErrorEventArgs : EventArgs
    {
      private string urlValue;
      private string frameValue;
      private int statusCodeValue;
      private bool cancelValue;

      public WebBrowserNavigateErrorEventArgs(
        string url,
        string frame,
        int statusCode,
        bool cancel)
      {
        this.urlValue = url;
        this.frameValue = frame;
        this.statusCodeValue = statusCode;
        this.cancelValue = cancel;
      }

      public string Url
      {
        get => this.urlValue;
        set => this.urlValue = value;
      }

      public string Frame
      {
        get => this.frameValue;
        set => this.frameValue = value;
      }

      public int StatusCode
      {
        get => this.statusCodeValue;
        set => this.statusCodeValue = value;
      }

      public bool Cancel
      {
        get => this.cancelValue;
        set => this.cancelValue = value;
      }
    }

    [Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [TypeLibType(TypeLibTypeFlags.FHidden)]
    [ComImport]
    public interface DWebBrowserEvents2
    {
      [DispId(271)]
      void NavigateError(
        [MarshalAs(UnmanagedType.IDispatch), In] object pDisp,
        [In] ref object URL,
        [In] ref object frame,
        [In] ref object statusCode,
        [In, Out] ref bool cancel);
    }
  }
}
