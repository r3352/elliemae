// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FastGuid
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class FastGuid
  {
    [ThreadStatic]
    private static FastRandom Instance;

    private static FastRandom GetInstance()
    {
      return FastGuid.Instance ?? (FastGuid.Instance = new FastRandom(Environment.TickCount * Thread.CurrentThread.ManagedThreadId));
    }

    public static Guid NewGuid()
    {
      byte[] numArray = new byte[16];
      FastGuid.GetInstance().NextBytesUnsafe(numArray);
      int index = BitConverter.IsLittleEndian ? 7 : 6;
      numArray[index] = (byte) ((int) numArray[index] & 15 | 64);
      numArray[8] = (byte) ((int) numArray[8] & 63 | 128);
      return new Guid(numArray);
    }
  }
}
