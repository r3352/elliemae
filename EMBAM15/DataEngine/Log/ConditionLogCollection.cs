// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ConditionLogCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class ConditionLogCollection : IEnumerable
  {
    private DocumentLog doc;
    private ObjectIDCollection recordIds;

    public ConditionLogCollection(DocumentLog doc, ObjectIDCollection recordIds)
    {
      this.doc = doc;
      this.recordIds = recordIds;
    }

    public void Add(string conditionId)
    {
      if (this.recordIds.Contains(conditionId))
        return;
      this.recordIds.Add(conditionId);
      this.doc.MarkLastUpdated();
    }

    public void Add(ConditionLog cond)
    {
      if (this.recordIds.Contains(cond.Guid))
        return;
      this.recordIds.Add(cond.Guid);
      this.doc.MarkLastUpdated();
      cond.TrackChange("Doc assigned", (LogRecordBase) this.doc);
    }

    public void Remove(ConditionLog cond)
    {
      if (!this.recordIds.Contains(cond.Guid))
        return;
      this.recordIds.Remove(cond.Guid);
      this.doc.MarkLastUpdated();
      cond.TrackChange("Doc removed", (LogRecordBase) this.doc);
    }

    public void Replace(ConditionLog[] condList)
    {
      foreach (ConditionLog cond in condList)
        this.Add(cond);
      foreach (ConditionLog condition in this.getConditionList(false))
      {
        if (Array.IndexOf<ConditionLog>(condList, condition) < 0)
          this.Remove(condition);
      }
    }

    public string[] GetConditionIds() => this.recordIds.ToArray();

    public void Clear() => this.Clear(true);

    public void Clear(bool checkAccess)
    {
      foreach (ConditionLog condition in this.getConditionList(checkAccess))
        this.Remove(condition);
    }

    public bool Contains(ConditionLog cond) => this.recordIds.Contains(cond.Guid);

    public bool Contains(string conditionId) => this.recordIds.Contains(conditionId);

    public ConditionLog this[int index] => this.getConditionList(true)[index];

    public int Count => this.getConditionList(true).Length;

    public IEnumerator GetEnumerator() => this.GetEnumerator(true);

    public IEnumerator GetEnumerator(bool checkAccess)
    {
      return this.getConditionList(checkAccess).GetEnumerator();
    }

    private ConditionLog[] getConditionList(bool checkAccess)
    {
      if (!this.doc.IsAttachedToLog)
        throw new Exception("Conditions collection cannot be accessed until document is added to log.");
      List<ConditionLog> conditionLogList = new List<ConditionLog>();
      foreach (string recordId in this.recordIds)
      {
        LogRecordBase recordById = this.doc.Log.GetRecordByID(recordId, checkAccess);
        if (recordById is ConditionLog)
          conditionLogList.Add(recordById as ConditionLog);
      }
      return conditionLogList.ToArray();
    }
  }
}
