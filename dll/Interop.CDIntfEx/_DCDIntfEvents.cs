// Decompiled with JetBrains decompiler
// Type: CDIntfEx._DCDIntfEvents
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [Guid("067B6CE2-DE85-435E-8E99-F52727F57E26")]
  [TypeLibType(4096)]
  [InterfaceType(2)]
  [ComImport]
  public interface _DCDIntfEvents
  {
    [DispId(1)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartDocPre();

    [DispId(2)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartDocPost(int JobID, int hDC);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndDocPre(int JobID, int hDC);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndDocPost(int JobID, int hDC);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartPage(int JobID, int hDC);

    [DispId(6)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndPage(int JobID, int hDC);

    [DispId(7)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnabledPre();
  }
}
