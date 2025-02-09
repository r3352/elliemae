// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LogEntryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LogEntryList : ListBase, ILogEntryList
  {
    private Hashtable lookupTable;

    public LogEntryList()
      : base(typeof (LogEntry))
    {
    }

    public LogEntryList(IList source)
      : base(typeof (LogEntry), source)
    {
    }

    public LogEntry this[int index]
    {
      get => (LogEntry) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LogEntry value) => this.List.Add((object) value);

    public bool Contains(LogEntry value) => this.List.Contains((object) value);

    public int IndexOf(LogEntry value) => this.List.IndexOf((object) value);

    public void Insert(int index, LogEntry value) => this.List.Insert(index, (object) value);

    public void Remove(LogEntry value) => this.List.Remove((object) value);

    public LogEntry[] ToArray()
    {
      LogEntry[] array = new LogEntry[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }

    protected override void OnInsertComplete(int index, object value)
    {
      this.lookupTable = (Hashtable) null;
    }

    protected override void OnRemoveComplete(int index, object value)
    {
      this.lookupTable = (Hashtable) null;
    }

    protected override void OnSetComplete(int index, object oldValue, object newValue)
    {
      this.lookupTable = (Hashtable) null;
    }

    internal LogEntry Find(LogRecordBase rec)
    {
      if (this.lookupTable == null)
        this.populateLookupTable();
      return (LogEntry) this.lookupTable[(object) rec];
    }

    private void populateLookupTable()
    {
      this.lookupTable = new Hashtable();
      for (int index = 0; index < this.Count; ++index)
        this.lookupTable.Add((object) this[index].Unwrap(), (object) this[index]);
    }
  }
}
