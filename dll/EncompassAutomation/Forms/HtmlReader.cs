// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HtmlReader
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using Sgml;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class HtmlReader : SgmlReader
  {
    public HtmlReader(TextReader reader, bool preserveCase)
    {
      this.InputStream = reader;
      this.DocType = "HTML";
      if (preserveCase)
        return;
      this.CaseFolding = (CaseFolding) 2;
    }

    public HtmlReader(string content, bool preserveCase)
    {
      this.InputStream = (TextReader) new StringReader(content);
      this.DocType = "HTML";
      if (preserveCase)
        return;
      this.CaseFolding = (CaseFolding) 2;
    }
  }
}
