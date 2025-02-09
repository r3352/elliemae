// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanDataFormatterUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.JedLib;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  internal class LoanDataFormatterUtils
  {
    private const string className = "LoanDataFormatterUtils�";
    private static LoanDataFormatterUtils.FormatterHelper helper = new LoanDataFormatterUtils.FormatterHelper();
    private static b jed = (b) null;

    static LoanDataFormatterUtils() => LoanDataFormatterUtils.jed = a.b("89rw372ttr0W3840");

    public byte[] Serialize(string rawData)
    {
      lock (LoanDataFormatterUtils.jed)
      {
        LoanDataFormatterUtils.jed.b();
        return LoanDataFormatterUtils.jed.b(rawData);
      }
    }

    private class FormatterHelper
    {
      public FormatterHelper() => a.a("3299879874903");
    }
  }
}
