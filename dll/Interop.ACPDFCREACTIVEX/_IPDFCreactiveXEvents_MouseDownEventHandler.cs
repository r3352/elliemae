// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX._IPDFCreactiveXEvents_MouseDownEventHandler
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [ComVisible(false)]
  [TypeLibType(16)]
  public delegate void _IPDFCreactiveXEvents_MouseDownEventHandler(
    [MarshalAs(UnmanagedType.Interface), In] acObject pObject,
    [In] int xPos,
    [In] int yPos,
    [In, Out] ref int Continue);
}
