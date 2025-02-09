// Decompiled with JetBrains decompiler
// Type: CDIntfEx._DCDIntfEvents_EventProvider
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace CDIntfEx
{
  internal sealed class _DCDIntfEvents_EventProvider : _DCDIntfEvents_Event, IDisposable
  {
    private UCOMIConnectionPointContainer m_ConnectionPointContainer;
    private ArrayList m_aEventSinkHelpers;
    private UCOMIConnectionPoint m_ConnectionPoint;

    private void Init()
    {
      UCOMIConnectionPoint ppCP = (UCOMIConnectionPoint) null;
      Guid riid = new Guid(new byte[16]
      {
        (byte) 226,
        (byte) 108,
        (byte) 123,
        (byte) 6,
        (byte) 133,
        (byte) 222,
        (byte) 94,
        (byte) 67,
        (byte) 142,
        (byte) 153,
        (byte) 245,
        (byte) 39,
        (byte) 39,
        (byte) 245,
        (byte) 126,
        (byte) 38
      });
      this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
      this.m_ConnectionPoint = ppCP;
      this.m_aEventSinkHelpers = new ArrayList();
    }

    public virtual void add_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_EnabledPreDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_EnabledPreDelegate != null && ((aEventSinkHelper.m_EnabledPreDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_EndPageDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_EndPageDelegate != null && ((aEventSinkHelper.m_EndPageDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_StartPageDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_StartPageDelegate != null && ((aEventSinkHelper.m_StartPageDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_EndDocPostDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_EndDocPostDelegate != null && ((aEventSinkHelper.m_EndDocPostDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_EndDocPreDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_EndDocPreDelegate != null && ((aEventSinkHelper.m_EndDocPreDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_StartDocPostDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_StartDocPostDelegate != null && ((aEventSinkHelper.m_StartDocPostDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void add_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        if (this.m_ConnectionPoint == null)
          this.Init();
        _DCDIntfEvents_SinkHelper pUnkSink = new _DCDIntfEvents_SinkHelper();
        int pdwCookie = 0;
        this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
        pUnkSink.m_dwCookie = pdwCookie;
        pUnkSink.m_StartDocPreDelegate = obj0;
        this.m_aEventSinkHelpers.Add((object) pUnkSink);
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public virtual void remove_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0)
    {
      Monitor.Enter((object) this);
      try
      {
        int count = this.m_aEventSinkHelpers.Count;
        int index = 0;
        if (0 >= count)
          return;
        do
        {
          _DCDIntfEvents_SinkHelper aEventSinkHelper = (_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index];
          if (aEventSinkHelper.m_StartDocPreDelegate != null && ((aEventSinkHelper.m_StartDocPreDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
          {
            this.m_aEventSinkHelpers.RemoveAt(index);
            this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
            if (count <= 1)
            {
              Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
              this.m_ConnectionPoint = (UCOMIConnectionPoint) null;
              this.m_aEventSinkHelpers = (ArrayList) null;
              return;
            }
            goto label_8;
          }
          else
            ++index;
        }
        while (index < count);
        goto label_9;
label_8:
        return;
label_9:;
      }
      finally
      {
        Monitor.Exit((object) this);
      }
    }

    public _DCDIntfEvents_EventProvider([In] object obj0)
    {
      this.m_ConnectionPointContainer = (UCOMIConnectionPointContainer) obj0;
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
            this.m_ConnectionPoint.Unadvise(((_DCDIntfEvents_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
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
