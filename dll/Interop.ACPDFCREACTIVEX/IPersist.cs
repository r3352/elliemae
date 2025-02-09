// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IPersist
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("0000010C-0000-0000-C000-000000000046")]
  [InterfaceType(1)]
  [ComImport]
  public interface IPersist
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetClassID(out Guid pClassID);
  }
}
