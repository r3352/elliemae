// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Html2XHtml
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Provides methods for converting HTML to XHTML.</summary>
  /// <exclude />
  public class Html2XHtml
  {
    private Html2XHtml()
    {
    }

    /// <summary>Converts an HTML string to an XHTML string.</summary>
    /// <param name="htmlString">The HTML string to be converted</param>
    /// <returns>A string containing XHTML.</returns>
    public static string Convert(string htmlString) => Html2XHtml.Convert(htmlString, true, false);

    /// <summary>Converts an HTML string to an XHTML string.</summary>
    /// <param name="htmlString">The string to be converted.</param>
    /// <param name="preserveCase">Flag indicating if case of tags and attributes should be
    /// preserved during the conversion.</param>
    /// <param name="useRoundTripEncoding">Flag indicating if round-trip encoding is to be used
    /// to ensure non-breaking spaces are properly encoded.</param>
    /// <returns>An XHTML-formatted string.</returns>
    public static string Convert(string htmlString, bool preserveCase, bool useRoundTripEncoding)
    {
      htmlString = htmlString.Replace("&nbsp;", "<nbsp />");
      htmlString = htmlString.Replace("&#160;", "<nbsp />");
      htmlString = Regex.Replace(htmlString, "(?:\\<!\\[CDATA\\[(?:[\\s\\S]*?)\\]\\]\\>)", new MatchEvaluator(Html2XHtml.CapText));
      StringReader reader1 = new StringReader(htmlString);
      StringWriter writer = new StringWriter();
      HtmlReader reader2 = new HtmlReader((TextReader) reader1, preserveCase);
      HtmlWriter htmlWriter = new HtmlWriter((TextWriter) writer);
      reader2.Read();
      while (!reader2.EOF)
        htmlWriter.WriteNode((XmlReader) reader2, false);
      htmlWriter.Flush();
      string xmlString = Html2XHtml.XmlFormatting(writer.ToString());
      if (!useRoundTripEncoding)
        xmlString = Html2XHtml.Revert(xmlString);
      return xmlString;
    }

    /// <summary>
    /// Reverts an XHTML string that was round-trip encoded using the Convert method.
    /// </summary>
    /// <param name="xmlString">The string to be reverted.</param>
    /// <returns>Returns a string in which the non-breaking spaces have been reverted to
    /// their native values.</returns>
    public static string Revert(string xmlString)
    {
      xmlString = xmlString.Replace("<nbsp />" + Environment.NewLine, "<nbsp />");
      while (xmlString.IndexOf(" <nbsp />") >= 0)
        xmlString = xmlString.Replace(" <nbsp />", "<nbsp />");
      xmlString = xmlString.Replace("<nbsp />", "&#160;");
      return xmlString;
    }

    private static string XmlFormatting(string primaryString)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        StringWriter w1 = new StringWriter();
        XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
        xmlDocument.LoadXml(primaryString);
        w2.Formatting = Formatting.Indented;
        w2.Indentation = 2;
        xmlDocument.DocumentElement.WriteTo((XmlWriter) w2);
        w2.Flush();
        return w1.ToString();
      }
      catch
      {
        return primaryString;
      }
    }

    /// <summary>remove the cdata mark from html string</summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private static string CapText(Match m)
    {
      string str = m.ToString();
      return str.Substring(9, str.Length - 12);
    }
  }
}
