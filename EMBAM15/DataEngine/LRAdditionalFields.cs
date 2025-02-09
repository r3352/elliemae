// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LRAdditionalFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LRAdditionalFields : IFieldSearchable
  {
    private List<string> lockRequestFields;
    private List<string> loanSnapshotFields;
    private Hashtable requestFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable snapshotFields = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public LRAdditionalFields()
    {
      this.lockRequestFields = new List<string>();
      this.loanSnapshotFields = new List<string>();
    }

    [PgReady]
    public void AddField(DataRow r, DbServerType dbServerType)
    {
      if (dbServerType == DbServerType.Postgres)
        this.AddField((string) r["fieldID"], int.Parse(r["forRequest"].ToString()) == 1);
      else
        this.AddField((string) r["fieldID"], (bool) r["forRequest"]);
    }

    public void AddField(string id, bool forRequest)
    {
      if (forRequest)
      {
        this.lockRequestFields.Add(id);
        if (this.requestFields.ContainsKey((object) id))
          return;
        this.requestFields.Add((object) id, (object) "");
      }
      else
      {
        this.loanSnapshotFields.Add(id);
        if (this.snapshotFields.ContainsKey((object) id))
          return;
        this.snapshotFields.Add((object) id, (object) "");
      }
    }

    public string[] GetFields(bool forRequest)
    {
      return forRequest ? this.lockRequestFields.ToArray() : this.loanSnapshotFields.ToArray();
    }

    public bool IsAdditionalField(string id, bool forRequest)
    {
      return forRequest ? this.requestFields.ContainsKey((object) id) : this.snapshotFields.ContainsKey((object) id);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (string lockRequestField in this.lockRequestFields)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, lockRequestField);
      foreach (string loanSnapshotField in this.loanSnapshotFields)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.SnapShotFields, loanSnapshotField);
    }
  }
}
