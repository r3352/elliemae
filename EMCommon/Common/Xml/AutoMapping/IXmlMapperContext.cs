// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Xml.AutoMapping.IXmlMapperContext
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.Xml.AutoMapping
{
  [CLSCompliant(true)]
  public interface IXmlMapperContext
  {
    bool HasAttribute(string attributeName);

    string GetAttribute(string attributeName);

    TValue GetAttributeValue<TValue>(string attributeName);

    void SetAttribute(string attributeName, object value);

    bool HasElement(string elementName);

    IXmlMapperContext GetElement(string elementName);

    IEnumerable<IXmlMapperContext> GetElements(string elementName);

    IXmlMapperContext NewElement(string elementName);
  }
}
