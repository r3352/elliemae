// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineMetadata
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineMetadata : 
    DocEngineEntity,
    IEnumerable<DocEngineFieldDescriptor>,
    IEnumerable
  {
    private List<DocEngineFieldDescriptor> fields = new List<DocEngineFieldDescriptor>();
    private Dictionary<string, DocEngineFieldDescriptor> encompassFieldLookup = new Dictionary<string, DocEngineFieldDescriptor>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, DocEngineFieldDescriptor> docEngineFieldLookup = new Dictionary<string, DocEngineFieldDescriptor>();

    public DocEngineMetadata(XmlElement metaXml)
      : base(metaXml)
    {
      foreach (XmlElement selectNode in this.XML.SelectNodes("Attribute"))
      {
        DocEngineFieldDescriptor engineFieldDescriptor = new DocEngineFieldDescriptor(selectNode);
        if (engineFieldDescriptor.EncompassFieldID != "")
        {
          this.fields.Add(engineFieldDescriptor);
          this.encompassFieldLookup[engineFieldDescriptor.EncompassFieldID] = engineFieldDescriptor;
          this.docEngineFieldLookup[engineFieldDescriptor.DocEngineFieldID] = engineFieldDescriptor;
        }
      }
    }

    public DocEngineFieldDescriptor GetEncompassField(string fieldId)
    {
      return this.encompassFieldLookup.ContainsKey(fieldId) ? this.encompassFieldLookup[fieldId] : (DocEngineFieldDescriptor) null;
    }

    public DocEngineFieldDescriptor GetDocEngineField(string fieldId)
    {
      return this.docEngineFieldLookup.ContainsKey(fieldId) ? this.docEngineFieldLookup[fieldId] : (DocEngineFieldDescriptor) null;
    }

    public IEnumerator<DocEngineFieldDescriptor> GetEnumerator()
    {
      return (IEnumerator<DocEngineFieldDescriptor>) this.fields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    internal static DocEngineMetadata Extract(DocEngineResponse response)
    {
      XmlElement metaXml = (XmlElement) response.ResponseXml.SelectSingleNode("//Prototype");
      return metaXml == null ? (DocEngineMetadata) null : new DocEngineMetadata(metaXml);
    }
  }
}
