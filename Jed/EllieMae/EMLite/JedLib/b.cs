// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.JedLib.b
// Assembly: Jed, Version=1.0.1234.56789, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D965D698-A97D-45D6-911B-975853D5C21D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Jed.dll

using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.JedLib
{
  public class b
  {
    private const int a = 1;
    private const int b = 255;
    private const int c = 255;
    private byte[] d = (byte[]) null;
    private int e = -1;
    private int f = -1;
    private static EllieMae.EMLite.JedLib.b g = (EllieMae.EMLite.JedLib.b) null;

    [SpecialName]
    public static EllieMae.EMLite.JedLib.b a() => EllieMae.EMLite.JedLib.b.g;

    public b(string A_0)
    {
      this.d = Encoding.ASCII.GetBytes(A_0);
      this.b();
    }

    public b(byte[] A_0)
    {
      this.d = A_0;
      this.b();
    }

    public static void a(string A_0)
    {
      if (EllieMae.EMLite.JedLib.b.g != null)
        return;
      EllieMae.EMLite.JedLib.b.g = new EllieMae.EMLite.JedLib.b(A_0);
    }

    public static void a(byte[] A_0)
    {
      if (EllieMae.EMLite.JedLib.b.g != null)
        return;
      EllieMae.EMLite.JedLib.b.g = new EllieMae.EMLite.JedLib.b(A_0);
    }

    public void b()
    {
      this.e = 0;
      this.f = (int) this.d[0];
    }

    private byte[] a(bool A_0, byte[] A_1, int A_2, int A_3)
    {
      if (this.d == null || A_1 == null)
        return (byte[]) null;
      byte[] numArray = new byte[A_3];
      int length = this.d.Length;
      for (int index = 0; index < A_3; ++index)
      {
        int num1 = (int) A_1[index + A_2];
        int num2;
        if (num1 >= 1 && num1 <= (int) byte.MaxValue)
        {
          int num3 = (int) this.d[this.e++ % length] - 1;
          int num4 = num1 - 1;
          num2 = (510 + num3 + this.f - num4) % (int) byte.MaxValue + 1;
          this.f = !A_0 ? num4 + 1 : num2;
        }
        else
          goto label_4;
label_3:
        numArray[index] = (byte) num2;
        continue;
label_4:
        num2 = num1;
        goto label_3;
      }
      return numArray;
    }

    public byte[] b(byte[] A_0, int A_1, int A_2) => this.a(true, A_0, A_1, A_2);

    public byte[] b(string A_0)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(A_0);
      return this.b(bytes, 0, bytes.Length);
    }

    public string a(byte[] A_0, int A_1, int A_2)
    {
      byte[] bytes = this.a(false, A_0, A_1, A_2);
      return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
    }

    public string a(Stream A_0)
    {
      int length = (int) A_0.Length;
      byte[] numArray = new byte[length];
      A_0.Read(numArray, 0, length);
      return this.a(numArray, 0, length);
    }

    public byte[] c(byte[] A_0, int A_1, int A_2) => this.a(false, A_0, A_1, A_2);

    public byte[] b(Stream A_0)
    {
      int length = (int) A_0.Length;
      byte[] numArray = new byte[length];
      A_0.Read(numArray, 0, length);
      return this.c(numArray, 0, length);
    }
  }
}
