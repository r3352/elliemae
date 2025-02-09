// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.NativeWIN32
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class NativeWIN32
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool EnumThreadWindows(
      int threadId,
      NativeWIN32.EnumThreadProc pfnEnum,
      IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowText(
      IntPtr hWnd,
      out NativeWIN32.STRINGBUFFER ClassName,
      int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STRINGBUFFER
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string szText;
    }
  }
}
