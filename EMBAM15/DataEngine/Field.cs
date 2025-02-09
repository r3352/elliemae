// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Field
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class Field
  {
    public static readonly Field Empty = new Field(FieldDefinition.Empty);
    private FieldDefinition fieldDef;
    private FieldEvents fieldEvents;
    private string xpath;
    private bool xpathParsed;
    private string xpathElement;
    private string xpathAttribute;
    private string cachedValue;
    private XmlElement cachedElement;

    internal Field(FieldDefinition def)
    {
      this.fieldDef = def;
      if (!(def is PersistentField) || def.Category != FieldCategory.Common)
        return;
      this.xpath = ((PersistentField) def).XPath;
    }

    internal Field(FieldDefinition def, FieldEvents fieldEvents)
      : this(def)
    {
      this.fieldEvents = fieldEvents;
    }

    internal Field(Field source, string predicate)
      : this(source.fieldDef, source.fieldEvents)
    {
      if (!(this.fieldDef is PersistentField))
        return;
      this.xpath = ((PersistentField) this.fieldDef).XPath.Replace("#", predicate);
    }

    public FieldDefinition Definition => this.fieldDef;

    public string Xpath => this.xpath ?? "";

    public FieldFormat DataFormat => this.fieldDef.Format;

    public string Id => this.fieldDef.FieldID;

    public FieldEvents Events => this.fieldEvents;

    public bool IsXPathFixed
    {
      get => this.Definition.Category == FieldCategory.Common && !this.Definition.MultiInstance;
    }

    public string CachedValue
    {
      get => this.cachedValue;
      set
      {
        if (!this.IsXPathFixed)
          return;
        this.cachedValue = value;
      }
    }

    public XmlElement CachedElement
    {
      get => this.cachedElement;
      set
      {
        if (!this.IsXPathFixed)
          return;
        this.cachedElement = value;
      }
    }

    public string XPathElement
    {
      get
      {
        this.ensureXPathParsed();
        return this.xpathElement;
      }
    }

    public string XPathAttribute
    {
      get
      {
        this.ensureXPathParsed();
        return this.xpathAttribute;
      }
    }

    private void ensureXPathParsed()
    {
      if (this.xpath == null || this.xpathParsed)
        return;
      Field.parseXPath(this.xpath, out this.xpathElement, out this.xpathAttribute);
      this.xpathParsed = true;
    }

    private static bool parseXPath(string xpath, out string elementXPath, out string attrName)
    {
      elementXPath = attrName = (string) null;
      int length = xpath.LastIndexOf("/@");
      if (length <= 0 || length >= xpath.Length - 2)
        return false;
      elementXPath = xpath.Substring(0, length);
      attrName = xpath.Substring(length + 2);
      return true;
    }
  }
}
