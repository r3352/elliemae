// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HtmlWriter
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class HtmlWriter : XmlTextWriter
  {
    public HtmlWriter(TextWriter writer)
      : base(writer)
    {
    }

    public HtmlWriter(StringBuilder builder)
      : base((TextWriter) new StringWriter(builder))
    {
    }

    public HtmlWriter(Stream stream, Encoding enc)
      : base(stream, enc)
    {
    }

    public override void WriteString(string text)
    {
      if (text == null || text == string.Empty)
        text = string.Empty;
      base.WriteString(text);
    }
  }
}
