// Decompiled with JetBrains decompiler
// Type: CDIntfEx._DCDIntfEvents_Event
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [TypeLibType(16)]
  [ComVisible(false)]
  [ComEventInterface(typeof (_DCDIntfEvents), typeof (_DCDIntfEvents_EventProvider))]
  public interface _DCDIntfEvents_Event
  {
    event _DCDIntfEvents_StartDocPreEventHandler StartDocPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0);

    event _DCDIntfEvents_StartDocPostEventHandler StartDocPost;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0);

    event _DCDIntfEvents_EndDocPreEventHandler EndDocPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0);

    event _DCDIntfEvents_EndDocPostEventHandler EndDocPost;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0);

    event _DCDIntfEvents_StartPageEventHandler StartPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0);

    event _DCDIntfEvents_EndPageEventHandler EndPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0);

    event _DCDIntfEvents_EnabledPreEventHandler EnabledPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void add_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void remove_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0);
  }
}
