// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.ElementWriter
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class ElementWriter
  {
    private XElement baseElement;

    public ElementWriter(XElement baseElement) => this.baseElement = baseElement;

    public XElement BaseElement => this.baseElement;

    public XElement Append(string name, string innerText)
    {
      XElement content = new XElement((XName) name);
      content.SetValue((object) innerText);
      this.baseElement.Add((object) content);
      return content;
    }

    public XElement Append(string name)
    {
      XElement content = new XElement((XName) name);
      this.baseElement.Add((object) content);
      return content;
    }
  }
}
