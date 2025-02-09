// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class MilestoneSetup
  {
    private List<CustomMilestone> coreMilestones = new List<CustomMilestone>();
    private Dictionary<string, CustomMilestone> coreMilestoneNameMap = new Dictionary<string, CustomMilestone>();
    private Dictionary<CustomMilestone, CustomMilestoneList> customMilestones = new Dictionary<CustomMilestone, CustomMilestoneList>();

    public MilestoneSetup()
    {
      foreach (string stage in Milestone.Stages)
      {
        CustomMilestone key = CustomMilestone.FromCoreMilestone(stage);
        this.coreMilestones.Add(key);
        this.coreMilestoneNameMap[key.Name] = key;
        this.customMilestones[key] = new CustomMilestoneList();
      }
    }

    public void AddCoreMilestoneDays(string coreName, int days)
    {
      if (!this.coreMilestoneNameMap.ContainsKey(coreName))
        throw new Exception("Invalid core milestone name '" + coreName + "'");
      this.coreMilestoneNameMap[coreName].Days = days;
    }

    public void InsertCustomMilestone(string coreName, string guid, string name, int days)
    {
      if (!this.coreMilestoneNameMap.ContainsKey(coreName))
        throw new Exception("Invalid core milestone name '" + coreName + "'");
      this.customMilestones[this.coreMilestoneNameMap[coreName]].Add(new CustomMilestone(guid, name, days));
    }

    public string[] GetCustomMilestoneNamesList(string stage)
    {
      CustomMilestoneList customMilestoneList = this.GetCustomMilestoneList(stage);
      ArrayList arrayList = new ArrayList();
      if (customMilestoneList != null)
      {
        foreach (CustomMilestone customMilestone in (CollectionBase) customMilestoneList)
          arrayList.Add((object) customMilestone.Name);
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public bool IsMilestoneNameExists(string name)
    {
      name = name.ToUpper();
      foreach (CustomMilestone completeMilestone in (CollectionBase) this.GetCompleteMilestoneList())
      {
        if (name == completeMilestone.Name.ToUpper())
          return true;
        MilestoneSettings settings = MilestoneUIConfig.GetSettings(completeMilestone.Name);
        if (settings != null && (name == settings.DoneText.ToUpper() || name == settings.ExpText.ToUpper() || name == settings.StageText.ToUpper()))
          return true;
      }
      return false;
    }

    public CustomMilestoneList GetCustomMilestoneList(string stage)
    {
      if (!this.coreMilestoneNameMap.ContainsKey(stage))
        throw new Exception("Invalid core milestone name '" + stage + "'");
      return this.customMilestones[this.coreMilestoneNameMap[stage]];
    }

    public void MarkArchivedMilestones(Hashtable archivedGUID)
    {
      if (archivedGUID == null || archivedGUID.Count == 0)
        return;
      foreach (CustomMilestone completeMilestone in (CollectionBase) this.GetCompleteMilestoneList())
      {
        if (archivedGUID.ContainsKey((object) completeMilestone.MilestoneID))
          completeMilestone.Archived = true;
      }
    }

    public void RemoveArchivedMilestones()
    {
      foreach (CustomMilestoneList customMilestoneList in this.customMilestones.Values)
      {
        for (int index = customMilestoneList.Count - 1; index >= 0; --index)
        {
          if (customMilestoneList[index].Archived)
            customMilestoneList.RemoveAt(index);
        }
      }
    }

    public Hashtable GetCompleteMilestoneGUID()
    {
      CustomMilestoneList completeMilestoneList = this.GetCompleteMilestoneList();
      Hashtable completeMilestoneGuid = new Hashtable();
      for (int index = 0; index < completeMilestoneList.Count; ++index)
      {
        string milestoneId = completeMilestoneList[index].MilestoneID;
        string key = !Milestone.IsCoreMilestone(completeMilestoneList[index].Name) ? completeMilestoneList[index].Name : MilestoneUIConfig.GetSettings(completeMilestoneList[index].Name).StageText;
        if (!completeMilestoneGuid.ContainsKey((object) key))
          completeMilestoneGuid.Add((object) key, (object) milestoneId);
      }
      return completeMilestoneGuid;
    }

    public CustomMilestoneList GetCompleteMilestoneList() => this.GetCompleteMilestoneList(true);

    public CustomMilestoneList GetCompleteMilestoneList(bool includeArchivedMS)
    {
      CustomMilestoneList completeMilestoneList = new CustomMilestoneList();
      foreach (CustomMilestone coreMilestone in this.coreMilestones)
      {
        completeMilestoneList.Add(coreMilestone);
        if (includeArchivedMS)
        {
          completeMilestoneList.AddRange((ICollection) this.customMilestones[coreMilestone]);
        }
        else
        {
          foreach (CustomMilestone ms in (CollectionBase) this.customMilestones[coreMilestone])
          {
            if (!ms.Archived)
              completeMilestoneList.Add(ms);
          }
        }
      }
      return completeMilestoneList;
    }

    public int GetDays(string stage)
    {
      CustomMilestone customMilestone = (CustomMilestone) null;
      return customMilestone != null ? customMilestone.Days : 0;
    }
  }
}
