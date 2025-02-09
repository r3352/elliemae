// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineFieldDescriptor
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineFieldDescriptor : DocEngineEntity
  {
    private string attributeName;
    private string docEngineFieldID;
    private string encompassFieldID;
    private string fieldName;
    private string description;
    private FieldFormat format;

    public DocEngineFieldDescriptor(XmlElement xml)
      : base(xml)
    {
      this.attributeName = this.GetAttribute("ID");
      this.docEngineFieldID = this.GetAttribute("DocEngineFieldId");
      this.encompassFieldID = this.GetAttribute("EncompassFieldId");
      this.fieldName = this.GetAttribute("Name");
      this.description = this.GetAttribute(nameof (Description));
      this.format = (FieldFormat) Enum.Parse(typeof (FieldFormat), this.GetAttribute("Format"));
    }

    public string AttributeName => this.attributeName;

    public string DocEngineFieldID => this.docEngineFieldID;

    public string EncompassFieldID => this.encompassFieldID;

    public string Description => this.description;

    public string FieldName => this.fieldName;
  }
}
