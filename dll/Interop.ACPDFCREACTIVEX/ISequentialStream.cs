// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.ISequentialStream
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
  [InterfaceType(1)]
  [ComImport]
  public interface ISequentialStream
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoteRead(out byte pv, [In] uint cb, out uint pcbRead);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoteWrite([In] ref byte pv, [In] uint cb, out uint pcbWritten);
  }
}
