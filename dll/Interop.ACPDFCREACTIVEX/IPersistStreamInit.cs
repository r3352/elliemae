// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IPersistStreamInit
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [InterfaceType(1)]
  [Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
  [ComImport]
  public interface IPersistStreamInit : IPersist
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetClassID(out Guid pClassID);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsDirty();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Load([MarshalAs(UnmanagedType.Interface), In] IStream pstm);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Save([MarshalAs(UnmanagedType.Interface), In] IStream pstm, [In] int fClearDirty);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSizeMax(out _ULARGE_INTEGER pCbSize);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InitNew();
  }
}
