// Decompiled with JetBrains decompiler
// Type: Encompass.Security.StreamExtensions
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

using System;
using System.IO;

#nullable disable
namespace Encompass.Security
{
  public static class StreamExtensions
  {
    public static void CopyTo(this Stream source, Stream dest, long bytes)
    {
      int length1 = source.CanSeek ? Math.Min((int) (source.Length - source.Position), 81920) : 81920;
      byte[] buffer = new byte[length1];
      long num = 0;
      int length2 = buffer.Length;
      int count;
      do
      {
        if (num + (long) length1 > bytes)
          length2 -= (int) (bytes % (long) length2);
        count = source.Read(buffer, 0, buffer.Length);
        num += (long) count;
        dest.Write(buffer, 0, count);
      }
      while (count >= buffer.Length && count != 0);
    }

    public static void CopyTo(
      this Stream source,
      Stream dest,
      long bytes,
      ProgressState initialState,
      IProgressNotify notify)
    {
      int length = source.CanSeek ? Math.Min((int) (source.Length - source.Position), 81920) : 81920;
      byte[] buffer = new byte[length];
      long num = 0;
      int count1 = buffer.Length;
      while (!notify.PendingCancel)
      {
        if (num + (long) length > bytes)
          count1 = (int) (bytes - num);
        int count2 = source.Read(buffer, 0, count1);
        num += (long) count2;
        dest.Write(buffer, 0, count2);
        initialState.BytesTotal = num;
        initialState.Value = Math.Min((int) ((double) num / (double) bytes * 100.0), 100);
        notify.UpdateProgress(initialState);
        if (count2 < buffer.Length || count2 == 0)
          break;
      }
    }
  }
}
