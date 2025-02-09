// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Bitmask
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class Bitmask
  {
    private uint bitmask;

    public Bitmask(object value) => this.bitmask = Convert.ToUInt32(value);

    public bool Contains(object value)
    {
      uint uint32 = Convert.ToUInt32(value);
      return ((int) this.bitmask & (int) uint32) == (int) uint32;
    }

    public void Add(object value) => this.bitmask |= Convert.ToUInt32(value);

    public void Remove(object value) => this.bitmask &= ~Convert.ToUInt32(value);

    public int GetBitCount()
    {
      int bitCount = 0;
      for (int index = 0; index < 32; ++index)
        bitCount += (int) (this.bitmask >> index) & 1;
      return bitCount;
    }
  }
}
