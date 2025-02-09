// Decompiled with JetBrains decompiler
// Type: <Module>
// Assembly: Jed, Version=1.0.1234.56789, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D965D698-A97D-45D6-911B-975853D5C21D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Jed.dll

#nullable disable
internal class \u003CModule\u003E
{
  static string a(string A_0)
  {
    char[] chArray = new char[(int) checked ((uint) A_0.Length)];
    int num1 = 1171741865;
    for (int index = 0; index < chArray.Length; ++index)
    {
      char ch = A_0[index];
      int num2 = (int) ch & (int) byte.MaxValue;
      int num3 = num1;
      int num4 = num3 + 1;
      byte num5 = (byte) (num2 ^ num3);
      int num6 = (int) ch >> 8;
      int num7 = num4;
      num1 = num7 + 1;
      byte num8 = (byte) (num6 ^ num7);
      chArray[index] = (char) ((uint) num8 << 8 | (uint) num5);
    }
    return string.Intern(new string(chArray));
  }
}
