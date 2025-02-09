// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.acContainerClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(2)]
  [ClassInterface(0)]
  [Guid("0874C7BD-284C-42B9-8592-34D4FFD356A2")]
  [ComImport]
  public class acContainerClass : IacContainer, acContainer
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern acContainerClass();

    [DispId(1610678272)]
    public virtual extern acContainer ParentContainer { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AppendObject([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.IDispatch)] object pDispatch);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AppendObjectByProgId([MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ProgId);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetObject([MarshalAs(UnmanagedType.BStr)] string Name, int Recursive, [MarshalAs(UnmanagedType.IDispatch)] ref object pDispatch);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DeleteObject([MarshalAs(UnmanagedType.BStr)] string Name);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Clear();
  }
}
