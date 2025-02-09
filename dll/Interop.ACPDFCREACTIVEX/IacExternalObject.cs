// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IacExternalObject
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(4160)]
  [Guid("C890967F-5EA0-41E4-A31F-0746748916D7")]
  [ComImport]
  public interface IacExternalObject
  {
    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Move([In] int dx, [In] int dy);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Stretch([In] int vertex, [In, Out] ref int dx, [In, Out] ref int dy);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Draw([MarshalAs(UnmanagedType.Interface), In] IacDC dc);

    [DispId(6)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool DrawBorder([MarshalAs(UnmanagedType.Interface), In] IacDC dc);

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool DrawSizing([MarshalAs(UnmanagedType.Interface), In] IacDC dc);

    [DispId(8)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool DrawSizingGrips([MarshalAs(UnmanagedType.Interface), In] IacDC dc);

    [DispId(9)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool DrawText([MarshalAs(UnmanagedType.Interface), In] IacDC dc, [MarshalAs(UnmanagedType.BStr)] string str);

    [DispId(12)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool Refresh();

    [DispId(13)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool CheckPoint([In] int dx, [In] int dy, out int result);

    [DispId(20)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool GetBoundingBox(out int left, out int top, out int right, out int bottom);

    [DispId(21)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool GetNameWithForce([MarshalAs(UnmanagedType.BStr)] out string Name, [In] bool forceName);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool GetName([MarshalAs(UnmanagedType.BStr)] out string Name);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetName([MarshalAs(UnmanagedType.BStr), In] string Name);

    [DispId(24)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool GetObjectSubType(out int Type);

    [DispId(25)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IacExternalObject Clone();

    [DispId(26)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetBoundingBox([In] int left, [In] int top, [In] int right, [In] int bottom);

    [DispId(27)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acAttribute GetFirstAttribute();

    [DispId(28)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acAttribute GetNextAttribute();

    [DispId(29)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object get_Attribute([MarshalAs(UnmanagedType.BStr), In] string Attribute);

    [DispId(29)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void set_Attribute([MarshalAs(UnmanagedType.BStr), In] string Attribute, [MarshalAs(UnmanagedType.Struct), In] object pVal);
  }
}
