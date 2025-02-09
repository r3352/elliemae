// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FastRandom
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class FastRandom
  {
    private const double REAL_UNIT_INT = 4.6566128730773926E-10;
    private const double REAL_UNIT_UINT = 2.3283064365386963E-10;
    private const uint Y = 842502087;
    private const uint Z = 3579807591;
    private const uint W = 273326509;
    private uint x;
    private uint y;
    private uint z;
    private uint w;
    private uint bitBuffer;
    private uint bitMask = 1;

    public FastRandom() => this.Reinitialise(Environment.TickCount);

    public FastRandom(int seed) => this.Reinitialise(seed);

    public void Reinitialise(int seed)
    {
      this.x = (uint) seed;
      this.y = 842502087U;
      this.z = 3579807591U;
      this.w = 273326509U;
    }

    public int Next()
    {
      uint num1 = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num1 ^ (int) (num1 >> 8));
      uint num2 = this.w & (uint) int.MaxValue;
      return num2 == (uint) int.MaxValue ? this.Next() : (int) num2;
    }

    public int Next(int upperBound)
    {
      if (upperBound < 0)
        throw new ArgumentOutOfRangeException(nameof (upperBound), (object) upperBound, "upperBound must be >=0");
      uint num = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      return (int) (4.6566128730773926E-10 * (double) (int.MaxValue & (int) (this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num ^ (int) (num >> 8)))) * (double) upperBound);
    }

    public int Next(int lowerBound, int upperBound)
    {
      if (lowerBound > upperBound)
        throw new ArgumentOutOfRangeException(nameof (upperBound), (object) upperBound, "upperBound must be >=lowerBound");
      uint num1 = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      int num2 = upperBound - lowerBound;
      return num2 < 0 ? lowerBound + (int) (2.3283064365386963E-10 * (double) (this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num1 ^ (int) (num1 >> 8))) * (double) ((long) upperBound - (long) lowerBound)) : lowerBound + (int) (4.6566128730773926E-10 * (double) (int.MaxValue & (int) (this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num1 ^ (int) (num1 >> 8)))) * (double) num2);
    }

    public double NextDouble()
    {
      uint num = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      return 4.6566128730773926E-10 * (double) (int.MaxValue & (int) (this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num ^ (int) (num >> 8))));
    }

    public void NextBytes(byte[] buffer)
    {
      uint num1 = this.x;
      uint num2 = this.y;
      uint num3 = this.z;
      uint num4 = this.w;
      int num5 = 0;
      int num6 = buffer.Length - 3;
      while (num5 < num6)
      {
        uint num7 = num1 ^ num1 << 11;
        num1 = num2;
        num2 = num3;
        num3 = num4;
        num4 = (uint) ((int) num4 ^ (int) (num4 >> 19) ^ (int) num7 ^ (int) (num7 >> 8));
        byte[] numArray1 = buffer;
        int index1 = num5;
        int num8 = index1 + 1;
        int num9 = (int) (byte) num4;
        numArray1[index1] = (byte) num9;
        byte[] numArray2 = buffer;
        int index2 = num8;
        int num10 = index2 + 1;
        int num11 = (int) (byte) (num4 >> 8);
        numArray2[index2] = (byte) num11;
        byte[] numArray3 = buffer;
        int index3 = num10;
        int num12 = index3 + 1;
        int num13 = (int) (byte) (num4 >> 16);
        numArray3[index3] = (byte) num13;
        byte[] numArray4 = buffer;
        int index4 = num12;
        num5 = index4 + 1;
        int num14 = (int) (byte) (num4 >> 24);
        numArray4[index4] = (byte) num14;
      }
      if (num5 < buffer.Length)
      {
        uint num15 = num1 ^ num1 << 11;
        num1 = num2;
        num2 = num3;
        num3 = num4;
        num4 = (uint) ((int) num4 ^ (int) (num4 >> 19) ^ (int) num15 ^ (int) (num15 >> 8));
        byte[] numArray5 = buffer;
        int index5 = num5;
        int num16 = index5 + 1;
        int num17 = (int) (byte) num4;
        numArray5[index5] = (byte) num17;
        if (num16 < buffer.Length)
        {
          byte[] numArray6 = buffer;
          int index6 = num16;
          int num18 = index6 + 1;
          int num19 = (int) (byte) (num4 >> 8);
          numArray6[index6] = (byte) num19;
          if (num18 < buffer.Length)
          {
            byte[] numArray7 = buffer;
            int index7 = num18;
            int index8 = index7 + 1;
            int num20 = (int) (byte) (num4 >> 16);
            numArray7[index7] = (byte) num20;
            if (index8 < buffer.Length)
              buffer[index8] = (byte) (num4 >> 24);
          }
        }
      }
      this.x = num1;
      this.y = num2;
      this.z = num3;
      this.w = num4;
    }

    public unsafe void NextBytesUnsafe(byte[] buffer)
    {
      if (buffer.Length % 8 != 0)
        throw new ArgumentException("Buffer length must be divisible by 8", nameof (buffer));
      uint num1 = this.x;
      uint num2 = this.y;
      uint num3 = this.z;
      uint num4 = this.w;
      fixed (byte* numPtr = buffer)
      {
        int index1 = 0;
        for (int index2 = buffer.Length >> 2; index1 < index2; index1 += 2)
        {
          uint num5 = num1 ^ num1 << 11;
          uint num6 = num2;
          uint num7 = num3;
          uint num8 = num4;
          uint num9;
          ((uint*) numPtr)[index1] = num9 = (uint) ((int) num4 ^ (int) (num4 >> 19) ^ (int) num5 ^ (int) (num5 >> 8));
          uint num10 = num6 ^ num6 << 11;
          num1 = num7;
          num2 = num8;
          num3 = num9;
          ((uint*) numPtr)[index1 + 1] = num4 = (uint) ((int) num9 ^ (int) (num9 >> 19) ^ (int) num10 ^ (int) (num10 >> 8));
        }
      }
      this.x = num1;
      this.y = num2;
      this.z = num3;
      this.w = num4;
    }

    public uint NextUInt()
    {
      uint num = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      return this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num ^ (int) (num >> 8));
    }

    public int NextInt()
    {
      uint num = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      return int.MaxValue & (int) (this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num ^ (int) (num >> 8)));
    }

    public bool NextBool()
    {
      if (this.bitMask != 1U)
        return ((int) this.bitBuffer & (int) (this.bitMask >>= 1)) == 0;
      uint num = this.x ^ this.x << 11;
      this.x = this.y;
      this.y = this.z;
      this.z = this.w;
      this.bitBuffer = this.w = (uint) ((int) this.w ^ (int) (this.w >> 19) ^ (int) num ^ (int) (num >> 8));
      this.bitMask = 2147483648U;
      return ((int) this.bitBuffer & (int) this.bitMask) == 0;
    }
  }
}
