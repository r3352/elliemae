// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.EncCustomPopUpHandler
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using DotNetBrowser.WinForms;
using System;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  internal class EncCustomPopUpHandler : IHandler<OpenPopupParameters>
  {
    private Form parentForm;
    private Form form;

    internal EncCustomPopUpHandler(Form form) => this.parentForm = form;

    public void Handle(OpenPopupParameters parameters)
    {
      this.parentForm.BeginInvoke((Delegate) (() => this.ShowPopup(parameters.PopupBrowser, parameters.Rectangle)));
    }

    private void ShowPopup(IBrowser popupBrowser, DotNetBrowser.Geometry.Rectangle rectangle)
    {
      BrowserView browserView1 = new BrowserView();
      browserView1.Dock = DockStyle.Fill;
      BrowserView browserView2 = browserView1;
      browserView2.InitializeFrom(popupBrowser);
      popupBrowser.OpenPopupHandler = (IHandler<OpenPopupParameters>) new EncCustomPopUpHandler(browserView2.FindForm());
      this.form = new Form();
      this.form.Icon = this.parentForm.FindForm()?.Icon;
      this.form.Tag = (object) "HostAdapater Popup";
      if (!rectangle.IsEmpty)
      {
        this.form.StartPosition = FormStartPosition.Manual;
        this.form.Location = new System.Drawing.Point(rectangle.Origin.X, rectangle.Origin.Y);
        this.form.ClientSize = new System.Drawing.Size((int) rectangle.Size.Width, (int) rectangle.Size.Height);
        browserView2.Width = (int) rectangle.Size.Width;
        browserView2.Height = (int) rectangle.Size.Height;
      }
      else
      {
        this.form.Width = 800;
        this.form.Height = 600;
      }
      this.form.Closed += (EventHandler) ((_param1, _param2) =>
      {
        this.form.Controls.Clear();
        if (popupBrowser.IsDisposed)
          return;
        popupBrowser.Dispose();
      });
      popupBrowser.TitleChanged += (EventHandler<TitleChangedEventArgs>) ((sender, e) => this.form.BeginInvoke((Delegate) (() => this.form.Text = e.Title)));
      popupBrowser.Disposed += (EventHandler) ((_param1, _param2) => this.form.BeginInvoke((Delegate) (() =>
      {
        this.form.Controls.Clear();
        this.form.Hide();
        this.form.Close();
        this.form.Dispose();
      })));
      this.form.Controls.Add((Control) browserView2);
      this.form.Show();
    }
  }
}
