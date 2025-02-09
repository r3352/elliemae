// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentGroup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentGroup : IXmlSerializable, IIdentifiable
  {
    private string guid;
    private string name;
    private string[] docList;

    public DocumentGroup(string name)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.name = name;
    }

    public DocumentGroup(string name, string guid, string[] docList)
    {
      this.name = name;
      this.guid = guid;
      this.docList = docList;
    }

    public DocumentGroup(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid));
      this.name = info.GetString(nameof (Name));
      XmlList<string> xmlList = (XmlList<string>) info.GetValue("Documents", typeof (XmlList<string>), (object) null);
      if (xmlList == null)
        return;
      this.docList = xmlList.ToArray();
    }

    public string Guid => this.guid;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string[] DocList => this.docList;

    public bool Contains(DocumentTemplate template)
    {
      return this.docList != null && Array.IndexOf<string>(this.docList, template.Guid) >= 0;
    }

    public DocumentTemplate[] GetDocuments(DocumentTrackingSetup docSetup)
    {
      if (this.docList == null)
        return new DocumentTemplate[0];
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      foreach (string doc in this.docList)
      {
        DocumentTemplate byId = docSetup.GetByID(doc);
        if (byId != null)
          documentTemplateList.Add(byId);
      }
      return documentTemplateList.ToArray();
    }

    public void SetDocuments(DocumentTemplate[] docList)
    {
      if (docList.Length != 0)
      {
        this.docList = new string[docList.Length];
        for (int index = 0; index < docList.Length; ++index)
          this.docList[index] = docList[index].Guid;
      }
      else
        this.docList = (string[]) null;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("Name", (object) this.name);
      if (this.docList != null)
        info.AddValue("Documents", (object) new XmlList<string>((IEnumerable<string>) this.docList));
      else
        info.AddValue("Documents", (object) null);
    }

    public override string ToString() => this.name;
  }
}
