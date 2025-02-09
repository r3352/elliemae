// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HtmlReader
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using Sgml;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// This class skips all nodes which has some kind of prefix. This trick does the job
  /// to clean up MS Word/Outlook HTML markups.
  /// </summary>
  internal class HtmlReader : SgmlReader
  {
    /// <summary>
    /// Overloads <see cref="T:Sgml.SgmlReader">Sgml.SgmlReader</see>'s constructor <see cref="T:Sgml.SgmlReader">SgmlReader( TextReader reader )</see>
    /// </summary>
    /// <param name="preserveCase"></param>
    /// <param name="reader">The TextReader reads from. </param>
    public HtmlReader(TextReader reader, bool preserveCase)
    {
      this.InputStream = reader;
      this.DocType = "HTML";
      if (preserveCase)
        return;
      this.CaseFolding = CaseFolding.ToLower;
    }

    /// <summary>
    /// Overloads <see cref="T:Sgml.SgmlReader">Sgml.SgmlReader</see>'s constructor <see cref="T:Sgml.SgmlReader">SgmlReader( )</see>
    /// </summary>
    /// <param name="preserveCase"></param>
    /// <param name="content">The content string reads from.</param>
    public HtmlReader(string content, bool preserveCase)
    {
      this.InputStream = (TextReader) new StringReader(content);
      this.DocType = "HTML";
      if (preserveCase)
        return;
      this.CaseFolding = CaseFolding.ToLower;
    }
  }
}
