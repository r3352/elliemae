// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.XElementExtensions
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.Xml;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Extensions
{
  public static class XElementExtensions
  {
    public static string ToString(this XElement element, bool validateCharacters)
    {
      using (MemoryStream stream = StreamHelper.NewMemoryStream())
      {
        using (XmlWriter writer = XmlHelper.CreateWriter((Stream) stream, true))
          element.WriteTo(writer);
        return stream.ToString(Encoding.UTF8, true);
      }
    }
  }
}
