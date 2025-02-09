// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.TaskSetTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class TaskSetTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private Hashtable taskList = new Hashtable();
    private string templateName = string.Empty;
    private string description = string.Empty;
    private bool isDefault;

    public Hashtable DocList => this.taskList;

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

    public virtual ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public TaskSetTemplate()
    {
    }

    public TaskSetTemplate(XmlSerializationInfo info)
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
          this.taskList[(object) key1] = (object) arrayList;
        }
      }
    }

    public ArrayList GetTasksByMilestone(string milestonename)
    {
      if (milestonename == "submittal")
        milestonename = "Submittal";
      ArrayList tasksByMilestone = (ArrayList) this.taskList[(object) milestonename];
      if (tasksByMilestone == null)
      {
        tasksByMilestone = new ArrayList();
        this.taskList[(object) milestonename] = (object) tasksByMilestone;
      }
      return tasksByMilestone;
    }

    public void RemoveTasksByMilestone(string milestonename)
    {
      this.taskList.Remove((object) milestonename);
    }

    public void RenameMilestoneInTasks(string oldName, string newName)
    {
      object task1 = this.taskList[(object) oldName];
      if (task1 == null)
        return;
      this.taskList.Remove((object) oldName);
      object task2 = this.taskList[(object) newName];
      if (task2 != null)
      {
        ArrayList c = (ArrayList) task1;
        ArrayList arrayList1 = (ArrayList) task2;
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
        this.taskList[(object) newName] = task1;
    }

    public void CleanAllFields()
    {
      if (this.taskList == null)
        return;
      this.taskList.Clear();
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
      foreach (object key in (IEnumerable) this.taskList.Keys)
      {
        ArrayList task = (ArrayList) this.taskList[key];
        XmlStringTable xmlStringTable2 = new XmlStringTable();
        xmlStringTable2["milestonename"] = (object) (string) key;
        for (int index = 0; index < task.Count; ++index)
          xmlStringTable2[index.ToString()] = task[index];
        info.AddValue(num.ToString(), (object) xmlStringTable2);
        ++num;
      }
    }

    public static explicit operator TaskSetTemplate(BinaryObject obj)
    {
      return (TaskSetTemplate) BinaryConvertibleObject.Parse(obj, typeof (TaskSetTemplate));
    }
  }
}
