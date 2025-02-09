// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.SyncTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class SyncTemplate
  {
    private int templateID = -1;
    private string templateName;
    private string templateDescription;
    private List<string> syncFields;
    private bool urla2020Excluded;
    private List<string> sync2020Fields;

    public int TemplateID
    {
      set => this.templateID = value;
      get => this.templateID;
    }

    public string TemplateName
    {
      set => this.templateName = value;
      get => this.templateName;
    }

    public string TemplateDescription
    {
      set => this.templateDescription = value;
      get => this.templateDescription;
    }

    public List<string> SyncFields => this.syncFields;

    public List<string> GetSyncFields(bool excludeURLA2020)
    {
      if (this.syncFields == null || this.syncFields.Count == 0)
        return (List<string>) null;
      if (!excludeURLA2020 || this.urla2020Excluded || this.sync2020Fields == null || this.sync2020Fields.Count == 0)
        return this.syncFields;
      for (int index = this.syncFields.Count - 1; index >= 0; --index)
      {
        if (this.sync2020Fields.Contains(this.syncFields[index]))
          this.syncFields.RemoveAt(index);
      }
      this.urla2020Excluded = true;
      return this.syncFields;
    }

    public List<string> Sync2020Fields => this.sync2020Fields;

    public SyncTemplate(string templateName, string templateDescription)
      : this(-1, templateName, templateDescription)
    {
    }

    public SyncTemplate(int templateID, string templateName, string templateDescription)
    {
      this.templateID = templateID;
      this.templateName = templateName;
      this.templateDescription = templateDescription;
      this.syncFields = new List<string>();
      this.sync2020Fields = new List<string>();
    }

    public void AddField(string fieldID)
    {
      if (this.syncFields.Contains(fieldID))
        return;
      this.syncFields.Add(fieldID);
    }

    public void AddFields(List<string> fieldIDs)
    {
      if (fieldIDs == null || fieldIDs.Count == 0)
        return;
      for (int index = 0; index < fieldIDs.Count; ++index)
        this.AddField(fieldIDs[index]);
    }

    public void AddURLA2020Fields(List<string> fieldIDs)
    {
      if (fieldIDs == null || fieldIDs.Count == 0)
        return;
      for (int index = 0; index < fieldIDs.Count; ++index)
      {
        if (!this.sync2020Fields.Contains(fieldIDs[index]))
          this.sync2020Fields.Add(fieldIDs[index]);
      }
    }

    public void RemoveField(string fieldID)
    {
      if (!this.syncFields.Contains(fieldID))
        return;
      this.syncFields.Remove(fieldID);
    }

    public void RemoveFields(List<string> fieldIDs)
    {
      for (int index = 0; index < fieldIDs.Count; ++index)
        this.RemoveField(fieldIDs[index]);
    }

    public void ClearFields()
    {
      if (this.syncFields == null)
        return;
      this.syncFields.Clear();
    }

    public SyncTemplate Clone()
    {
      SyncTemplate syncTemplate = new SyncTemplate(this.templateName, this.templateDescription);
      syncTemplate.AddFields(this.syncFields);
      return syncTemplate;
    }
  }
}
