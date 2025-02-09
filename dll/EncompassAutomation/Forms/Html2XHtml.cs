// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Html2XHtml
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class Html2XHtml
  {
    private Html2XHtml()
    {
    }

    public static string Convert(string htmlString) => Html2XHtml.Convert(htmlString, true, false);

    public static string Convert(string htmlString, bool preserveCase, bool useRoundTripEncoding)
    {
      htmlString = htmlString.Replace("&nbsp;", "<nbsp />");
      htmlString = htmlString.Replace("&#160;", "<nbsp />");
      htmlString = Regex.Replace(htmlString, "(?:\\<!\\[CDATA\\[(?:[\\s\\S]*?)\\]\\]\\>)", new MatchEvaluator(Html2XHtml.CapText));
      StringReader reader1 = new StringReader(htmlString);
      StringWriter writer = new StringWriter();
      HtmlReader reader2 = new HtmlReader((TextReader) reader1, preserveCase);
      HtmlWriter htmlWriter = new HtmlWriter((TextWriter) writer);
      ((XmlReader) reader2).Read();
      while (!((XmlReader) reader2).EOF)
        htmlWriter.WriteNode((XmlReader) reader2, false);
      htmlWriter.Flush();
      string xmlString = Html2XHtml.XmlFormatting(writer.ToString());
      if (!useRoundTripEncoding)
        xmlString = Html2XHtml.Revert(xmlString);
      return xmlString;
    }

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

    private static string CapText(Match m)
    {
      string str = m.ToString();
      return str.Substring(9, str.Length - 12);
    }
  }
}
