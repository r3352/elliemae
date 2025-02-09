// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX._IPDFCreactiveXEvents_EventProvider
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

#nullable disable
namespace ACPDFCREACTIVEX
{
  internal sealed class _IPDFCreactiveXEvents_EventProvider : 
    _IPDFCreactiveXEvents_Event,
    IDisposable
  {
    private IConnectionPointContainer m_ConnectionPointContainer;
    private ArrayList m_aEventSinkHelpers;
    private IConnectionPoint m_ConnectionPoint;

    private void Init()
    {
      IConnectionPoint ppCP = (IConnectionPoint) null;
      Guid riid = new Guid(new byte[16]
      {
        (byte) 118,
        (byte) 77,
        (byte) 1,
        (byte) 94,
        (byte) 253,
        (byte) 96,
        (byte) 54,
        (byte) 68,
        (byte) 139,
        (byte) 47,
        (byte) 209,
        (byte) 213,
        (byte) 160,
        (byte) 120,
        (byte) 69,
        (byte) 82
      });
      this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
      this.m_ConnectionPoint = ppCP;
      this.m_aEventSinkHelpers = new ArrayList();
    }

    public virtual void add_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_BeforeDeleteDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_BeforeDeleteDelegate != null && ((aEventSinkHelper.m_BeforeDeleteDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_PrintPageDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_PrintPageDelegate != null && ((aEventSinkHelper.m_PrintPageDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_SavePageDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_SavePageDelegate != null && ((aEventSinkHelper.m_SavePageDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_ClickHyperlinkDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_ClickHyperlinkDelegate != null && ((aEventSinkHelper.m_ClickHyperlinkDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_RefreshDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_RefreshDelegate != null && ((aEventSinkHelper.m_RefreshDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_SelectedObjectChangeDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_SelectedObjectChangeDelegate != null && ((aEventSinkHelper.m_SelectedObjectChangeDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_ObjectTextChangeDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_ObjectTextChangeDelegate != null && ((aEventSinkHelper.m_ObjectTextChangeDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_ContextSensitiveMenuDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_ContextSensitiveMenuDelegate != null && ((aEventSinkHelper.m_ContextSensitiveMenuDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_MouseDownDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_MouseDownDelegate != null && ((aEventSinkHelper.m_MouseDownDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_MouseMoveDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_MouseMoveDelegate != null && ((aEventSinkHelper.m_MouseMoveDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_MouseUpDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_MouseUpDelegate != null && ((aEventSinkHelper.m_MouseUpDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_NewObjectDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_NewObjectDelegate != null && ((aEventSinkHelper.m_NewObjectDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_ActivateObjectDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_ActivateObjectDelegate != null && ((aEventSinkHelper.m_ActivateObjectDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_LoadPageDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_LoadPageDelegate != null && ((aEventSinkHelper.m_LoadPageDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_EvaluateExpressionDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_EvaluateExpressionDelegate != null && ((aEventSinkHelper.m_EvaluateExpressionDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _IPDFCreactiveXEvents_SinkHelper pUnkSink = new _IPDFCreactiveXEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_ProcessingProgressDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_aEventSinkHelpers == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _IPDFCreactiveXEvents_SinkHelper aEventSinkHelper = (_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_ProcessingProgressDelegate != null && ((aEventSinkHelper.m_ProcessingProgressDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (IConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_10;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_11;
label_10:
        return;
label_11:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public _IPDFCreactiveXEvents_EventProvider([In] object obj0)
    {
      this.m_ConnectionPointContainer = (IConnectionPointContainer) obj0;
    }

    public override void Finalize()
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          return;
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 < count)
        {
          do
          {
            this.m_ConnectionPoint.Unadvise(((_IPDFCreactiveXEvents_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
            ++index;
          }
          while (index < count);
        }
        Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
      }
      catch (Exception ex)
      {
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void Dispose()
    {
      this.Finalize();
      GC.SuppressFinalize((object) this);
    }
  }
}
