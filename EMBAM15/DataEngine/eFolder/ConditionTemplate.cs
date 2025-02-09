// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.ConditionTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public abstract class ConditionTemplate : IXmlSerializable, IIdentifiable
  {
    private string guid;
    private string name = string.Empty;
    private string description = string.Empty;
    private string[] docList;
    private int daysTillDue;

    public ConditionTemplate() => this.guid = System.Guid.NewGuid().ToString();

    public ConditionTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid));
      this.name = info.GetString(nameof (Name));
      this.description = info.GetString(nameof (Description));
      this.daysTillDue = info.GetInteger(nameof (DaysTillDue), 0);
      XmlList<string> xmlList = (XmlList<string>) info.GetValue("Documents", typeof (XmlList<string>), (object) null);
      if (xmlList == null)
        return;
      this.docList = xmlList.ToArray();
    }

    protected ConditionTemplate(string guid) => this.guid = guid;

    public abstract ConditionType ConditionType { get; }

    public string Guid => this.guid;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public int DaysTillDue
    {
      get => this.daysTillDue;
      set => this.daysTillDue = value;
    }

    public bool ContainsDocument(DocumentTemplate template)
    {
      return this.docList != null && Array.IndexOf<string>(this.docList, template.Guid) >= 0;
    }

    public DocumentTemplate[] GetDocuments(DocumentTrackingSetup docSetup)
    {
      if (this.docList == null)
        return new DocumentTemplate[0];
      ArrayList arrayList = new ArrayList();
      foreach (string doc in this.docList)
      {
        DocumentTemplate byId = docSetup.GetByID(doc);
        if (byId != null)
          arrayList.Add((object) byId);
      }
      return (DocumentTemplate[]) arrayList.ToArray(typeof (DocumentTemplate));
    }

    public DocumentTemplate GetTPODefaultDoc(DocumentTrackingSetup docSetup, string TPOCondDocGuid)
    {
      ArrayList arrayList = new ArrayList();
      return TPOCondDocGuid != "" ? docSetup.GetByID(TPOCondDocGuid) ?? (DocumentTemplate) null : (DocumentTemplate) null;
    }

    public string[] GetDocumentIDs()
    {
      return this.docList == null ? new string[0] : (string[]) this.docList.Clone();
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

    public void SetDocumentIds(string[] docList)
    {
      if (docList.Length != 0)
        this.docList = (string[]) docList.Clone();
      else
        this.docList = (string[]) null;
    }

    public abstract ConditionLog CreateLogEntry(string addedBy, string pairId);

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("Name", (object) this.name);
      info.AddValue("Description", (object) this.description);
      info.AddValue("DaysTillDue", (object) this.daysTillDue);
      if (this.docList != null)
        info.AddValue("Documents", (object) new XmlList<string>((IEnumerable<string>) this.docList));
      else
        info.AddValue("Documents", (object) null);
    }

    public override string ToString() => this.name;
  }
}
