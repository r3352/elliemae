// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaPipelineViewColumn
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PersonaPipelineViewColumn : PipelineViewColumnBase
  {
    public PersonaPipelineViewColumn(
      string columnDBName,
      int orderIndex,
      SortOrder sortOrder,
      int width)
    {
      this.columnDBName = columnDBName;
      this.orderIndex = orderIndex;
      this.sortOrder = sortOrder;
      this.width = width;
    }

    internal PersonaPipelineViewColumn(string columnDBName) => this.columnDBName = columnDBName;

    public string ColumnDBName
    {
      get => this.columnDBName;
      set => this.columnDBName = value;
    }

    public int OrderIndex => this.orderIndex;

    public SortOrder SortOrder => this.sortOrder;

    public int Width
    {
      get => this.width;
      set => this.width = value;
    }

    internal void SetOrderIndex(int index) => this.orderIndex = index;

    internal void SetSortOrder(SortOrder sortOrder) => this.sortOrder = sortOrder;

    public PersonaPipelineViewColumn Clone()
    {
      return new PersonaPipelineViewColumn(this.columnDBName, this.orderIndex, this.sortOrder, this.width);
    }
  }
}
