// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BufferPools
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public static class BufferPools
  {
    public static readonly int SmallPoolBufferSize = 40000;
    public static readonly int SmallPoolMaxBufferCount = 1000;
    public static readonly int LargePoolBufferSize = 400000;
    public static readonly int LargePoolMaxBufferCount = 500;
    public static readonly BufferPool SmallBufferPool = (BufferPool) null;
    public static readonly BufferPool LargeBufferPool = (BufferPool) null;

    static BufferPools()
    {
      int result1;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolSmallBufferSize"]), out result1))
        BufferPools.SmallPoolBufferSize = result1;
      int result2;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolSmallBufferCount"]), out result2))
        BufferPools.SmallPoolMaxBufferCount = result2;
      int result3;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolLargeBufferSize"]), out result3))
        BufferPools.LargePoolBufferSize = result3;
      int result4;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolLargeBufferCount"]), out result4))
        BufferPools.LargePoolMaxBufferCount = result4;
      BufferPools.SmallBufferPool = new BufferPool(nameof (SmallBufferPool), BufferPools.SmallPoolBufferSize, 100, BufferPools.SmallPoolMaxBufferCount);
      BufferPools.LargeBufferPool = new BufferPool(nameof (LargeBufferPool), BufferPools.LargePoolBufferSize, 10, BufferPools.LargePoolMaxBufferCount);
    }
  }
}
