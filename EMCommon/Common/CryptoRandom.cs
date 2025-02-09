// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CryptoRandom
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CryptoRandom : RandomNumberGenerator
  {
    private static RandomNumberGenerator rand = (RandomNumberGenerator) new RNGCryptoServiceProvider();

    public CryptoRandom() => CryptoRandom.rand = RandomNumberGenerator.Create();

    public override void GetBytes(byte[] buffer) => CryptoRandom.rand.GetBytes(buffer);

    public override void GetNonZeroBytes(byte[] data) => CryptoRandom.rand.GetNonZeroBytes(data);

    public double NextDouble()
    {
      byte[] data = new byte[4];
      CryptoRandom.rand.GetBytes(data);
      return (double) BitConverter.ToUInt32(data, 0) / (double) uint.MaxValue;
    }

    public int Next(int minValue, int maxValue)
    {
      long num = (long) (maxValue - minValue);
      return (int) ((long) Math.Floor(this.NextDouble() * (double) num) + (long) minValue);
    }

    public int Next() => this.Next(0, int.MaxValue);

    public int Next(int maxValue) => this.Next(0, maxValue);

    public Decimal RandomDecimal(int precision, int scale)
    {
      if (precision < 1 || precision > 28)
        throw new ArgumentOutOfRangeException(nameof (precision), (object) precision, "Precision must be between 1 and 28.");
      if (scale < 0 || scale > precision)
        throw new ArgumentOutOfRangeException(nameof (scale), (object) precision, "Scale must be between 0 and precision.");
      Decimal num1 = 0M;
      for (int index = 0; index < precision; ++index)
      {
        int num2 = this.Next(0, 10);
        num1 = num1 * 10M + (Decimal) num2;
      }
      for (int index = 0; index < scale; ++index)
        num1 /= 10M;
      return num1;
    }

    public string RandomString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < 10; ++index)
      {
        char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * this.NextDouble() + 65.0)));
        stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    public DateTime RandomDate()
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      int days = (DateTime.Today - dateTime).Days;
      return dateTime.AddDays((double) this.Next(days));
    }

    public string RandomYesNo()
    {
      return new string(Enumerable.Repeat<string>("YN", 1).Select<string, char>((Func<string, char>) (s => s[this.Next(s.Length)])).ToArray<char>());
    }
  }
}
