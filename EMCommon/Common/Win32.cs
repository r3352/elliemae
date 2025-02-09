// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Win32
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class Win32
  {
    public static int IDOK = 1;
    public static int WM_DESTROY = 2;
    public static int SW_SHOW = 5;
    public static int SW_NORMAL = 1;

    private Win32()
    {
    }

    [DllImport("user32")]
    public static extern IntPtr SendMessage(IntPtr Handle, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll")]
    public static extern int FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32")]
    public static extern int SetDlgItemText(int hDlg, int nIDDlgItem, string lpString);

    [DllImport("user32.dll")]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
  }
}
