// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Win32API
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class Win32API
  {
    [DllImport("User32.dll")]
    private static extern bool GetLastInputInfo(ref LastInputInfo lastInputInfo);

    [DllImport("Kernel32.dll")]
    private static extern uint GetLastError();

    [DllImport("User32.dll")]
    public static extern bool LockWorkStation();

    public static int GetIdleTime()
    {
      LastInputInfo lastInputInfo = new LastInputInfo();
      lastInputInfo.Size = (uint) Marshal.SizeOf<LastInputInfo>(lastInputInfo);
      if (!Win32API.GetLastInputInfo(ref lastInputInfo))
        throw new Exception(Win32API.GetLastError().ToString());
      return Environment.TickCount - (int) lastInputInfo.Time;
    }
  }
}
