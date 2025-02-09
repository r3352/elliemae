// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.StackingElement
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  [Serializable]
  public class StackingElement : IXmlSerializable
  {
    private StackingElementType elementType;
    private string elementName;

    public StackingElement(StackingElementType elementType, string elementName)
    {
      this.elementType = elementType;
      this.elementName = elementName;
    }

    public StackingElement(XmlSerializationInfo info)
    {
      this.elementName = info.GetString(nameof (Name));
      this.elementType = info.GetEnum<StackingElementType>(nameof (Type));
    }

    public string Name => this.elementName;

    public StackingElementType Type => this.elementType;

    public override string ToString()
    {
      return this.Type == StackingElementType.Document ? "Document::" + this.Name : "Type::" + this.Name;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Name", (object) this.elementName);
      info.AddValue("Type", (object) this.elementType);
    }

    public override bool Equals(object obj)
    {
      return obj is StackingElement stackingElement && string.Compare(stackingElement.Name, this.Name, true) == 0 && stackingElement.Type == this.Type;
    }

    public override int GetHashCode()
    {
      return this.Name.ToLower().GetHashCode() ^ this.Type.GetHashCode();
    }

    public static StackingElement Parse(string text)
    {
      if (text.StartsWith("Type::", StringComparison.CurrentCultureIgnoreCase))
        return new StackingElement(StackingElementType.DocumentType, text.Substring(6));
      return text.StartsWith("Document::", StringComparison.CurrentCultureIgnoreCase) ? new StackingElement(StackingElementType.Document, text.Substring(10)) : new StackingElement(StackingElementType.Document, text);
    }
  }
}
