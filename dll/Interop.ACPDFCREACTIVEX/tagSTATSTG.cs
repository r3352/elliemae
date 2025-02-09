// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.tagSTATSTG
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct tagSTATSTG
  {
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pwcsName;
    public uint Type;
    public _ULARGE_INTEGER cbSize;
    public _FILETIME mtime;
    public _FILETIME ctime;
    public _FILETIME atime;
    public uint grfMode;
    public uint grfLocksSupported;
    public Guid clsid;
    public uint grfStateBits;
    public uint reserved;
  }
}
