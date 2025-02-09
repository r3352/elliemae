// Decompiled with JetBrains decompiler
// Type: CDIntfEx._DCDIntfEvents_SinkHelper
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [ClassInterface(ClassInterfaceType.None)]
  public sealed class _DCDIntfEvents_SinkHelper : _DCDIntfEvents
  {
    public _DCDIntfEvents_EnabledPreEventHandler m_EnabledPreDelegate;
    public _DCDIntfEvents_EndPageEventHandler m_EndPageDelegate;
    public _DCDIntfEvents_StartPageEventHandler m_StartPageDelegate;
    public _DCDIntfEvents_EndDocPostEventHandler m_EndDocPostDelegate;
    public _DCDIntfEvents_EndDocPreEventHandler m_EndDocPreDelegate;
    public _DCDIntfEvents_StartDocPostEventHandler m_StartDocPostDelegate;
    public _DCDIntfEvents_StartDocPreEventHandler m_StartDocPreDelegate;
    public int m_dwCookie;

    public virtual void EnabledPre()
    {
      if (this.m_EnabledPreDelegate == null)
        return;
      this.m_EnabledPreDelegate();
    }

    public virtual void EndPage([In] int obj0, [In] int obj1)
    {
      if (this.m_EndPageDelegate == null)
        return;
      this.m_EndPageDelegate(obj0, obj1);
    }

    public virtual void StartPage([In] int obj0, [In] int obj1)
    {
      if (this.m_StartPageDelegate == null)
        return;
      this.m_StartPageDelegate(obj0, obj1);
    }

    public virtual void EndDocPost([In] int obj0, [In] int obj1)
    {
      if (this.m_EndDocPostDelegate == null)
        return;
      this.m_EndDocPostDelegate(obj0, obj1);
    }

    public virtual void EndDocPre([In] int obj0, [In] int obj1)
    {
      if (this.m_EndDocPreDelegate == null)
        return;
      this.m_EndDocPreDelegate(obj0, obj1);
    }

    public virtual void StartDocPost([In] int obj0, [In] int obj1)
    {
      if (this.m_StartDocPostDelegate == null)
        return;
      this.m_StartDocPostDelegate(obj0, obj1);
    }

    public virtual void StartDocPre()
    {
      if (this.m_StartDocPreDelegate == null)
        return;
      this.m_StartDocPreDelegate();
    }

    internal _DCDIntfEvents_SinkHelper()
    {
      this.m_dwCookie = 0;
      this.m_EnabledPreDelegate = (_DCDIntfEvents_EnabledPreEventHandler) null;
      this.m_EndPageDelegate = (_DCDIntfEvents_EndPageEventHandler) null;
      this.m_StartPageDelegate = (_DCDIntfEvents_StartPageEventHandler) null;
      this.m_EndDocPostDelegate = (_DCDIntfEvents_EndDocPostEventHandler) null;
      this.m_EndDocPreDelegate = (_DCDIntfEvents_EndDocPreEventHandler) null;
      this.m_StartDocPostDelegate = (_DCDIntfEvents_StartDocPostEventHandler) null;
      this.m_StartDocPreDelegate = (_DCDIntfEvents_StartDocPreEventHandler) null;
    }
  }
}
