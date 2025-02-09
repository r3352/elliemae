// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IacContainer
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("97C5FFFB-7297-42B3-ADEB-9C356C2DE057")]
  [InterfaceType(1)]
  [ComImport]
  public interface IacContainer
  {
    [DispId(1610678272)]
    acContainer ParentContainer { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AppendObject([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.IDispatch)] object pDispatch);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AppendObjectByProgId([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ProgId);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetObject([MarshalAs(UnmanagedType.BStr)] string Name, int Recursive, [MarshalAs(UnmanagedType.IDispatch)] ref object pDispatch);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DeleteObject([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Clear();
  }
}
