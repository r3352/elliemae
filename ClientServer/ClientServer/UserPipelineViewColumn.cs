// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserPipelineViewColumn
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserPipelineViewColumn : PipelineViewColumnBase, IXmlSerializable
  {
    private int sortPriority = -1;
    private string alignment;
    private bool isRequired;

    public UserPipelineViewColumn()
    {
    }

    public UserPipelineViewColumn(
      string columnDBName,
      int orderIndex,
      SortOrder sortOrder,
      int width,
      int sortPriority,
      string alignment,
      bool isRequired)
    {
      this.columnDBName = columnDBName;
      this.orderIndex = orderIndex;
      this.sortOrder = sortOrder;
      this.width = width;
      this.sortPriority = sortPriority;
      this.alignment = alignment;
      this.isRequired = isRequired;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Name", (object) this.columnDBName);
    }

    internal UserPipelineViewColumn(string columnDBName) => this.columnDBName = columnDBName;

    public string ColumnDBName
    {
      get => this.columnDBName;
      set => this.columnDBName = value;
    }

    public string Alignment
    {
      get => this.alignment;
      set => this.alignment = value;
    }

    public int OrderIndex
    {
      get => this.orderIndex;
      set => this.orderIndex = value;
    }

    public int SortPriority
    {
      get => this.sortPriority;
      set => this.sortPriority = value;
    }

    public SortOrder SortOrder
    {
      get => this.sortOrder;
      set => this.sortOrder = value;
    }

    public int Width
    {
      get => this.width;
      set => this.width = value;
    }

    public bool IsRequired
    {
      get => this.isRequired;
      set => this.isRequired = value;
    }

    internal void SetOrderIndex(int index) => this.orderIndex = index;

    internal void SetSortOrder(SortOrder sortOrder) => this.sortOrder = sortOrder;

    public UserPipelineViewColumn Clone()
    {
      return new UserPipelineViewColumn(this.columnDBName, this.orderIndex, this.sortOrder, this.width, this.sortPriority, this.alignment, this.isRequired);
    }
  }
}
