// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.EfolderDocTrackViewManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class EfolderDocTrackViewManager : SessionBoundObject, IEfolderDocTrackViewManager
  {
    private const string className = "EfolderDocTrackViewManager";

    public EfolderDocTrackViewManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (EfolderDocTrackViewManager).ToLower());
      return this;
    }

    public virtual List<ViewSummary> GetViewsSummary()
    {
      this.onApiCalled(nameof (EfolderDocTrackViewManager), nameof (GetViewsSummary), Array.Empty<object>());
      try
      {
        return new EfolderDocTrackViewAccessor().GetViewsSummary(this.Session?.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EfolderDocTrackViewManager), ex, this.Session.SessionInfo);
        return (List<ViewSummary>) null;
      }
    }

    public virtual DocumentTrackingView GetView(string viewId)
    {
      this.onApiCalled(nameof (EfolderDocTrackViewManager), nameof (GetView), Array.Empty<object>());
      try
      {
        return string.IsNullOrEmpty(viewId) ? (DocumentTrackingView) null : new EfolderDocTrackViewAccessor().GetView(this.Session?.UserID, viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EfolderDocTrackViewManager), ex, this.Session.SessionInfo);
        return (DocumentTrackingView) null;
      }
    }

    public virtual DocumentTrackingView CreateView(DocumentTrackingView view)
    {
      this.onApiCalled(nameof (EfolderDocTrackViewManager), nameof (CreateView), Array.Empty<object>());
      try
      {
        return new EfolderDocTrackViewAccessor().CreateView(this.Session?.UserID, view);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EfolderDocTrackViewManager), ex, this.Session.SessionInfo);
        return (DocumentTrackingView) null;
      }
    }

    public virtual DocumentTrackingView UpdateView(DocumentTrackingView view)
    {
      this.onApiCalled(nameof (EfolderDocTrackViewManager), nameof (UpdateView), Array.Empty<object>());
      try
      {
        return new EfolderDocTrackViewAccessor().UpdateView(this.Session?.UserID, view);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EfolderDocTrackViewManager), ex, this.Session.SessionInfo);
        return (DocumentTrackingView) null;
      }
    }

    public virtual bool DeleteView(string viewId)
    {
      this.onApiCalled(nameof (EfolderDocTrackViewManager), nameof (DeleteView), Array.Empty<object>());
      bool flag = false;
      try
      {
        flag = new EfolderDocTrackViewAccessor().DeleteView(this.Session?.UserID, viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EfolderDocTrackViewManager), ex, this.Session.SessionInfo);
      }
      return flag;
    }
  }
}
