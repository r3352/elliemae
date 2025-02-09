// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IacBookmark
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("596703D6-7CDB-4C7B-866F-17F4824375AE")]
  [TypeLibType(4160)]
  [ComImport]
  public interface IacBookmark
  {
    [DispId(1)]
    string Title { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(2)]
    string Object { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(3)]
    int Count { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(4)]
    acBookmark Parent { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acBookmark get_Child(int index);

    [DispId(6)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoveChild(int index);

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InsertChild(int index, [MarshalAs(UnmanagedType.BStr)] string Title, [MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.Interface)] out acBookmark Bookmark);

    [DispId(8)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ReachBookmark();
  }
}
