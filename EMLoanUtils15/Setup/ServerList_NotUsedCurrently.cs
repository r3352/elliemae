// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ServerList_NotUsedCurrently
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class ServerList_NotUsedCurrently
  {
    private SortedList tbl = new SortedList();
    private int mostRecentlyVisited;

    public int MostRecentlyVisited => this.mostRecentlyVisited;

    public int Count => this.tbl.Count;

    public string GetByIndex(int i) => (string) this.tbl.GetKey(i);

    public void Insert(string server)
    {
      if (this.tbl.ContainsKey((object) server))
      {
        this.mostRecentlyVisited = this.tbl.IndexOfKey((object) server);
      }
      else
      {
        this.tbl.Add((object) server, (object) null);
        this.mostRecentlyVisited = this.tbl.IndexOfKey((object) server);
      }
    }
  }
}
