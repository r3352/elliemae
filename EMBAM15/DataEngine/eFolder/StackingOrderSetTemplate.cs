// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.StackingOrderSetTemplate
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
  public class StackingOrderSetTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private ArrayList requiredDocs = new ArrayList();
    private ArrayList docNames = new ArrayList();
    private ArrayList ndeDocGroups = new ArrayList();
    private string templateName = string.Empty;
    private string description = string.Empty;
    private bool isDefault;
    private bool autoSelectDocuments = true;
    private bool filterDocuments;

    public int DocumentStackingTemplateID { get; set; }

    public ArrayList RequiredDocs => this.requiredDocs;

    public ArrayList DocNames => this.docNames;

    public ArrayList NDEDocGroups => this.ndeDocGroups;

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

    public bool AutoSelectDocuments
    {
      get => this.autoSelectDocuments;
      set => this.autoSelectDocuments = value;
    }

    public bool FilterDocuments
    {
      get => this.filterDocuments;
      set => this.filterDocuments = value;
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public StackingOrderSetTemplate()
    {
    }

    public StackingOrderSetTemplate(XmlSerializationInfo info)
    {
      if (info == null)
        return;
      XmlStringTable xmlStringTable1 = (XmlStringTable) info.GetValue("0", typeof (XmlStringTable));
      this.DocumentStackingTemplateID = xmlStringTable1[nameof (DocumentStackingTemplateID)] != null ? Convert.ToInt32(xmlStringTable1[nameof (DocumentStackingTemplateID)].ToString()) : 0;
      this.templateName = string.Concat(xmlStringTable1["DTNAME"]);
      this.description = string.Concat(xmlStringTable1["DTDESC"]);
      this.isDefault = (string) xmlStringTable1["DEFAULT"] == "YES";
      this.autoSelectDocuments = !((string) xmlStringTable1["AUTOSELECTDOCUMENTS"] == "NO");
      this.filterDocuments = (string) xmlStringTable1["FILTERDOCUMENTS"] == "YES";
      ArrayList arrayList1 = new ArrayList();
      try
      {
        XmlStringTable xmlStringTable2 = (XmlStringTable) info.GetValue("2", typeof (XmlStringTable));
        for (int index = 0; index < xmlStringTable2.Count; ++index)
          arrayList1.Add(xmlStringTable2[index.ToString()]);
      }
      catch (Exception ex)
      {
      }
      this.requiredDocs = arrayList1;
      ArrayList arrayList2 = new ArrayList();
      try
      {
        XmlStringTable xmlStringTable3 = (XmlStringTable) info.GetValue("3", typeof (XmlStringTable));
        for (int index = 0; index < xmlStringTable3.Count; ++index)
          arrayList2.Add(xmlStringTable3[index.ToString()]);
      }
      catch (Exception ex)
      {
      }
      if (arrayList2.Count == 0 && info.TryGetValue("1", typeof (XmlStringTable), out object _))
      {
        XmlStringTable xmlStringTable4 = (XmlStringTable) info.GetValue("1", typeof (XmlStringTable));
        for (int index = 0; index < xmlStringTable4.Count; ++index)
          arrayList2.Add(xmlStringTable4[index.ToString()]);
      }
      this.docNames = arrayList2;
      ArrayList arrayList3 = new ArrayList();
      try
      {
        XmlStringTable xmlStringTable5 = (XmlStringTable) info.GetValue("4", typeof (XmlStringTable));
        for (int index = 0; index < xmlStringTable5.Count; ++index)
          arrayList3.Add(xmlStringTable5[index.ToString()]);
      }
      catch (Exception ex)
      {
      }
      this.ndeDocGroups = arrayList3;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      XmlStringTable xmlStringTable1 = new XmlStringTable();
      xmlStringTable1["DocumentStackingTemplateID"] = (object) this.DocumentStackingTemplateID.ToString();
      xmlStringTable1["DTNAME"] = (object) this.templateName;
      xmlStringTable1["DTDESC"] = (object) this.description;
      if (this.isDefault)
        xmlStringTable1["DEFAULT"] = (object) "YES";
      else
        xmlStringTable1["DEFAULT"] = (object) "";
      if (this.autoSelectDocuments)
        xmlStringTable1["AUTOSELECTDOCUMENTS"] = (object) "YES";
      else
        xmlStringTable1["AUTOSELECTDOCUMENTS"] = (object) "NO";
      if (this.filterDocuments)
        xmlStringTable1["FILTERDOCUMENTS"] = (object) "YES";
      else
        xmlStringTable1["FILTERDOCUMENTS"] = (object) "";
      info.AddValue("0", (object) xmlStringTable1);
      XmlStringTable xmlStringTable2 = new XmlStringTable();
      for (int index = 0; index < this.requiredDocs.Count; ++index)
        xmlStringTable2[index.ToString()] = this.requiredDocs[index];
      info.AddValue("2", (object) xmlStringTable2);
      XmlStringTable xmlStringTable3 = new XmlStringTable();
      for (int index = 0; index < this.docNames.Count; ++index)
        xmlStringTable3[index.ToString()] = this.docNames[index];
      info.AddValue("3", (object) xmlStringTable3);
      XmlStringTable xmlStringTable4 = new XmlStringTable();
      for (int index = 0; index < this.ndeDocGroups.Count; ++index)
        xmlStringTable4[index.ToString()] = this.ndeDocGroups[index];
      info.AddValue("4", (object) xmlStringTable4);
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public static explicit operator StackingOrderSetTemplate(BinaryObject obj)
    {
      return (StackingOrderSetTemplate) BinaryConvertibleObject.Parse(obj, typeof (StackingOrderSetTemplate));
    }
  }
}
