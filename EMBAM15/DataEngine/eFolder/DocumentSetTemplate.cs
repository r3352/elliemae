// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentSetTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentSetTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private Hashtable docList = new Hashtable();
    private string templateName = string.Empty;
    private string description = string.Empty;
    private bool isDefault;

    public Hashtable DocList => this.docList;

    public string TemplateName
    {
      get
      {
        if (this.templateName == null)
          this.templateName = string.Empty;
        return this.templateName;
      }
      set => this.templateName = value;
    }

    public string Description
    {
      get
      {
        if (this.description == null)
          this.description = string.Empty;
        return this.description;
      }
      set => this.description = value;
    }

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public DocumentSetTemplate()
    {
    }

    public DocumentSetTemplate(XmlSerializationInfo info)
    {
      if (info == null)
        return;
      XmlStringTable xmlStringTable1 = (XmlStringTable) info.GetValue("0", typeof (XmlStringTable));
      this.templateName = string.Concat(xmlStringTable1["DTNAME"]);
      this.description = string.Concat(xmlStringTable1["DTDESC"]);
      this.isDefault = (string) xmlStringTable1["DEFAULT"] == "YES";
      foreach (string name in info)
      {
        if (!(name == "0"))
        {
          XmlStringTable xmlStringTable2 = (XmlStringTable) info.GetValue(name, typeof (XmlStringTable));
          string key1 = (string) xmlStringTable2["milestonename"];
          xmlStringTable2.Remove("milestonename");
          ArrayList arrayList = new ArrayList();
          for (int index = 0; index < xmlStringTable2.Count; ++index)
          {
            string key2 = index.ToString();
            arrayList.Add(xmlStringTable2[key2]);
          }
          this.docList[(object) key1] = (object) arrayList;
        }
      }
    }

    public ArrayList GetDocumentsByMilestone(string milestonename)
    {
      ArrayList documentsByMilestone = (ArrayList) this.docList[(object) milestonename];
      if (documentsByMilestone == null)
      {
        documentsByMilestone = new ArrayList();
        this.docList[(object) milestonename] = (object) documentsByMilestone;
      }
      return documentsByMilestone;
    }

    public void RemoveDocumentsByMilestone(string milestonename)
    {
      this.docList.Remove((object) milestonename);
    }

    public void RenameMilestoneInDocuments(string oldName, string newName)
    {
      object doc1 = this.docList[(object) oldName];
      if (doc1 == null)
        return;
      this.docList.Remove((object) oldName);
      object doc2 = this.docList[(object) newName];
      if (doc2 != null)
      {
        ArrayList c = (ArrayList) doc1;
        ArrayList arrayList1 = (ArrayList) doc2;
        ArrayList arrayList2 = new ArrayList();
        foreach (string str in c)
        {
          if (arrayList1.Contains((object) str))
            arrayList2.Add((object) str);
        }
        if (arrayList2.Count != 0)
        {
          foreach (string str in arrayList2)
            c.Remove((object) str);
        }
        arrayList1.AddRange((ICollection) c);
      }
      else
        this.docList[(object) newName] = doc1;
    }

    public void CleanAllFields()
    {
      if (this.docList == null)
        return;
      this.docList.Clear();
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      XmlStringTable xmlStringTable1 = new XmlStringTable();
      xmlStringTable1["DTNAME"] = (object) this.templateName;
      xmlStringTable1["DTDESC"] = (object) this.description;
      if (this.isDefault)
        xmlStringTable1["DEFAULT"] = (object) "YES";
      else
        xmlStringTable1["DEFAULT"] = (object) "";
      info.AddValue("0", (object) xmlStringTable1);
      int num = 1;
      foreach (object key in (IEnumerable) this.docList.Keys)
      {
        ArrayList doc = (ArrayList) this.docList[key];
        XmlStringTable xmlStringTable2 = new XmlStringTable();
        xmlStringTable2["milestonename"] = (object) (string) key;
        for (int index = 0; index < doc.Count; ++index)
          xmlStringTable2[index.ToString()] = doc[index];
        info.AddValue(num.ToString(), (object) xmlStringTable2);
        ++num;
      }
    }

    public static explicit operator DocumentSetTemplate(BinaryObject obj)
    {
      return (DocumentSetTemplate) BinaryConvertibleObject.Parse(obj, typeof (DocumentSetTemplate));
    }
  }
}
