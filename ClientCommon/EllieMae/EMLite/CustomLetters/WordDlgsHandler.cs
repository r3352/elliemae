// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.WordDlgsHandler
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class WordDlgsHandler
  {
    private static string className = nameof (WordDlgsHandler);
    private static string sw = Tracing.SwCustomLetters;
    public static int HWND_TOP = 0;
    public static int HWND_TOPMOST = -1;
    public static uint SWP_NOSIZE = 1;
    public static uint SWP_NOMOVE = 2;
    public static uint SWP_SHOWWINDOW = 64;
    public static uint SWP_NOACTIVATE = 16;
    private static bool _StopThread = false;
    public static int WM_SYSCOMMAND = 274;
    public static int SC_CLOSE = 61536;
    public static int SW_HIDE = 0;
    public static int GW_OWNER = 4;

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr handle, int cmd);

    [DllImport("user32")]
    public static extern int BringWindowToTop(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      int hWndInsertAfter,
      int X,
      int Y,
      int cx,
      int cy,
      uint uFlags);

    [DllImport("user32")]
    public static extern int EnumThreadWindows(int dwThreadId, int lpfn, int lParam);

    private WordDlgsHandler()
    {
    }

    public static void StopWordDlgThreadProc() => WordDlgsHandler._StopThread = true;

    private static void ShowThreadsInWordProcess()
    {
      Process[] processesByName = Process.GetProcessesByName("WINWORD");
      if (processesByName.Length < 1)
        return;
      foreach (ProcessThread thread in (ReadOnlyCollectionBase) processesByName[0].Threads)
        NativeWIN32.EnumThreadWindows(thread.Id, new NativeWIN32.EnumThreadProc(WordDlgsHandler.MyEnumThreadWindowsProc), IntPtr.Zero);
    }

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32")]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    public static bool MyEnumThreadWindowsProc(IntPtr hwnd, IntPtr lParam)
    {
      if (WordDlgsHandler.IsWindowVisible(hwnd))
      {
        NativeWIN32.STRINGBUFFER ClassName;
        NativeWIN32.GetWindowText(hwnd, out ClassName, 256);
        if (ClassName.szText.Trim() == "Microsoft Office Word")
          WordDlgsHandler.SetWindowPos(hwnd, WordDlgsHandler.HWND_TOP, 0, 0, 0, 0, WordDlgsHandler.SWP_NOMOVE | WordDlgsHandler.SWP_NOSIZE);
        else
          WordDlgsHandler.SetWindowPos(hwnd, WordDlgsHandler.HWND_TOPMOST, 0, 0, 0, 0, WordDlgsHandler.SWP_NOMOVE | WordDlgsHandler.SWP_NOSIZE);
      }
      return true;
    }

    public static void ShowWordDlgThreadProc()
    {
      WordDlgsHandler._StopThread = false;
      if (File.Exists(SystemSettings.ConfigDir + "DisableWordDlgHandler.config"))
      {
        Tracing.Log(WordDlgsHandler.sw, TraceLevel.Info, WordDlgsHandler.className, "No detection of Word dialog windows.");
      }
      else
      {
        try
        {
          while (!WordDlgsHandler._StopThread)
          {
            WordDlgsHandler.ShowThreadsInWordProcess();
            Thread.Sleep(2000);
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(WordDlgsHandler.sw, TraceLevel.Error, WordDlgsHandler.className, "Error detecting Word dialog windows: " + ex.Message);
        }
      }
    }
  }
}
