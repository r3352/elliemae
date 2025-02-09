// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HtmlWriter
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Extends XmlTextWriter to provide Html writing feature which is not as strict as Xml
  /// writing. For example, Attributes without value will give it a value "true".
  /// </summary>
  internal class HtmlWriter : XmlTextWriter
  {
    /// <summary>
    /// Overloads <see cref="T:System.Xml.XmlTextWriter">XmlTextWriter</see>'s constructor <see cref="T:System.Xml.XmlTextWriter">HtmlWriter( TextWriter writer )</see>
    /// </summary>
    /// <param name="writer">The TextWriter to write to. It is assumed that the TextWriter is already set to the correct encoding.</param>
    public HtmlWriter(TextWriter writer)
      : base(writer)
    {
    }

    /// <summary>
    /// Overloads <see cref="T:System.Xml.XmlTextWriter">XmlTextWriter</see>'s constructor <see cref="T:System.Xml.XmlTextWriter">HtmlWriter( StringWriter builder )</see>
    /// </summary>
    /// <param name="builder">The stream to which you want to write. </param>
    public HtmlWriter(StringBuilder builder)
      : base((TextWriter) new StringWriter(builder))
    {
    }

    /// <summary>
    /// Overloads <see cref="T:System.Xml.XmlTextWriter">XmlTextWriter</see>'s constructor <see cref="T:System.Xml.XmlTextWriter">HtmlWriter( Stream stream, Encoding enc )</see>
    /// </summary>
    /// <param name="stream">The stream to which you want to write. </param>
    /// <param name="enc">The encoding to generate. If encoding is a null reference (Nothing in Visual Basic) it writes out the stream as UTF-8 and omits the encoding attribute from the ProcessingInstruction. </param>
    public HtmlWriter(Stream stream, Encoding enc)
      : base(stream, enc)
    {
    }

    /// <summary>Adds a value of "true" to attributes without value</summary>
    /// <param name="text">Attributes value</param>
    public override void WriteString(string text)
    {
      if (text == null || text == string.Empty)
        text = string.Empty;
      base.WriteString(text);
    }
  }
}
