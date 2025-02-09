// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.tagMSG
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ComConversionLoss]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct tagMSG
  {
    [ComConversionLoss]
    [ComAliasName("ACPDFCREACTIVEX.wireHWND")]
    public IntPtr hwnd;
    public uint message;
    public uint wParam;
    public int lParam;
    public uint time;
    public tagPOINT pt;
  }
}
