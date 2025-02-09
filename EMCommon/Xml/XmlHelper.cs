// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.XmlHelper
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public static class XmlHelper
  {
    private static readonly bool UseFastXmlDocumentForLoans = Utils.ParseBoolean((object) ConfigurationManager.AppSettings[nameof (UseFastXmlDocumentForLoans)]);

    public static string GetAttribute(XmlElement e, string name)
    {
      return XmlHelper.GetAttribute(e, name, (string) null);
    }

    public static string GetAttribute(XmlElement e, string name, string defaultValue)
    {
      return e.GetAttribute(name) ?? defaultValue;
    }

    public static TextReader CreateReader(Stream stream)
    {
      return XmlHelper.CreateReader(stream, Encoding.Default);
    }

    public static TextReader CreateReader(Stream stream, Encoding defaultEncoding)
    {
      if (defaultEncoding == null)
        throw new ArgumentNullException(nameof (defaultEncoding));
      stream.Seek(0L, SeekOrigin.Begin);
      return (TextReader) new StreamReader(stream, defaultEncoding, true, 1024, true);
    }

    public static XmlWriter CreateWriter(Stream stream, bool omitXmlDeclaration = false)
    {
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        Encoding = Encoding.UTF8,
        OmitXmlDeclaration = omitXmlDeclaration,
        CheckCharacters = false
      };
      return XmlWriter.Create(stream, settings);
    }

    public static XmlDocument NewXmlDocument()
    {
      return !XmlHelper.UseFastXmlDocumentForLoans ? new XmlDocument() : (XmlDocument) new FastXmlDocument();
    }
  }
}
