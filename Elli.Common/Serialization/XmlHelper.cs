// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.XmlHelper
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class XmlHelper
  {
    private XmlHelper()
    {
    }

    public static string GetAttribute(XElement e, string name)
    {
      return XmlHelper.GetAttribute(e, name, (string) null);
    }

    public static string GetAttribute(XElement e, string name, string defaultValue)
    {
      XAttribute xattribute = e.Attribute((XName) name);
      return xattribute == null ? defaultValue : xattribute.Value;
    }
  }
}
