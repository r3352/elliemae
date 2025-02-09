// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TableLayout
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.Common.Extensions;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TableLayout : IXmlSerializable, IEnumerable<TableLayout.Column>, IEnumerable
  {
    private XmlList<TableLayout.Column> columns;
    private Dictionary<string, TableLayout.Column> columnIDLookup = new Dictionary<string, TableLayout.Column>();
    private bool sorted;

    public TableLayout() => this.columns = new XmlList<TableLayout.Column>();

    public TableLayout(TableLayout.Column[] columns)
      : this()
    {
      this.AddColumns(columns);
    }

    public TableLayout(XmlSerializationInfo info)
    {
      this.columns = (XmlList<TableLayout.Column>) info.GetValue(nameof (columns), typeof (XmlList<TableLayout.Column>));
      this.sorted = true;
      foreach (TableLayout.Column column in (List<TableLayout.Column>) this.columns)
        this.columnIDLookup[column.ColumnID] = column;
    }

    public void AddColumn(TableLayout.Column column)
    {
      if (column.DisplayOrder < 0)
        column.DisplayOrder = this.columns.Count;
      this.Remove(column.ColumnID);
      this.columns.Add(column);
      this.columnIDLookup[column.ColumnID] = column;
      this.sorted = false;
    }

    public void AddColumns(TableLayout.Column[] columnList)
    {
      foreach (TableLayout.Column column in columnList)
        this.AddColumn(column);
    }

    public void InsertColumn(int index, TableLayout.Column column)
    {
      this.ensureSorted();
      this.Remove(column.ColumnID);
      this.columns.Insert(index, column);
      this.columnIDLookup[column.ColumnID] = column;
      this.reassignDisplayOrder();
    }

    public void InsertColumns(int index, TableLayout.Column[] columns)
    {
      this.ensureSorted();
      for (int index1 = 0; index1 < columns.Length; ++index1)
      {
        this.Remove(columns[index1].ColumnID);
        this.columns.Insert(index + index1, columns[index1]);
        this.columnIDLookup[columns[index1].ColumnID] = columns[index1];
      }
      this.reassignDisplayOrder();
    }

    public bool Contains(TableLayout.Column column)
    {
      return this.columnIDLookup.ContainsKey(column.ColumnID);
    }

    public bool Contains(string columnId) => this.columnIDLookup.ContainsKey(columnId);

    public int ColumnCount => this.columns.Count;

    public string[] ColumnTags
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (TableLayout.Column column in (List<TableLayout.Column>) this.columns)
          stringList.Add(column.Tag);
        return stringList.ToArray();
      }
    }

    public void Clear()
    {
      this.columns.Clear();
      this.columnIDLookup.Clear();
    }

    public void Remove(TableLayout.Column column)
    {
      if (!this.Contains(column))
        return;
      this.columns.Remove(column);
      this.columnIDLookup.Remove(column.ColumnID);
    }

    public void Remove(string columnId)
    {
      if (!this.Contains(columnId))
        return;
      this.columns.Remove(this.columnIDLookup[columnId]);
      this.columnIDLookup.Remove(columnId);
    }

    public void SortByDescription()
    {
      this.columns.Sort(new Comparison<TableLayout.Column>(TableLayout.Column.CompareDescriptions));
      this.sorted = true;
      this.reassignDisplayOrder();
    }

    public TableLayout.Column[] GetSortColumnsByPriority()
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in this)
      {
        if (column.SortOrder != SortOrder.None)
          columnList.Add(column);
      }
      columnList.Sort(new Comparison<TableLayout.Column>(this.compareColumnsBySortPriority));
      return columnList.ToArray();
    }

    public IEnumerator<TableLayout.Column> GetEnumerator()
    {
      this.ensureSorted();
      return (IEnumerator<TableLayout.Column>) this.columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public TableLayout.Column GetColumnByID(string columnId)
    {
      return this.Contains(columnId) ? this.columnIDLookup[columnId] : (TableLayout.Column) null;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.ensureSorted();
      info.AddValue("columns", (object) this.columns);
    }

    private void reassignDisplayOrder()
    {
      for (int index = 0; index < this.columns.Count; ++index)
        this.columns[index].DisplayOrder = index;
    }

    private int compareColumnsBySortPriority(TableLayout.Column a, TableLayout.Column b)
    {
      return a.SortPriority == b.SortPriority ? a.DisplayOrder - b.DisplayOrder : a.SortPriority - b.SortPriority;
    }

    private void ensureSorted()
    {
      if (this.sorted)
        return;
      this.columns.Sort();
      this.sorted = true;
    }

    public static TableLayout Parse(string xml)
    {
      return xml.IsNullOrWhiteSpace() ? (TableLayout) null : (TableLayout) new XmlSerializer().Deserialize(xml, typeof (TableLayout));
    }

    [CLSCompliant(false)]
    public string ToXML() => new XmlSerializer().Serialize((object) this);

    [Serializable]
    public class Column : IXmlSerializable, IComparable, ICloneable
    {
      private string columnID;
      public string Title;
      public string Description;
      public int Width;
      public HorizontalAlignment Alignment;
      public int DisplayOrder;
      public bool Required;
      public SortOrder SortOrder;
      public int SortPriority = -1;
      public string Tag;

      public Column(string columnId, string title, HorizontalAlignment alignment, int width)
        : this(columnId, title, title, alignment, width)
      {
      }

      public Column(
        string columnId,
        string title,
        string description,
        HorizontalAlignment alignment,
        int width)
        : this(columnId, title, description, columnId, alignment, width, false)
      {
      }

      public Column(
        string columnId,
        string title,
        string description,
        string tag,
        HorizontalAlignment alignment,
        int width,
        bool required)
      {
        this.Title = title;
        this.Description = description;
        this.Tag = tag;
        this.columnID = columnId;
        this.DisplayOrder = -1;
        this.Width = width;
        this.Alignment = alignment;
        this.Required = required;
      }

      public Column(XmlSerializationInfo info)
      {
        this.columnID = info.GetString("id");
        this.Title = info.GetString("title");
        this.Description = info.GetString("desc", this.Title);
        this.Tag = info.GetString("tag");
        this.Width = info.GetInteger("width");
        this.DisplayOrder = info.GetInteger("displayOrder");
        this.SortOrder = (SortOrder) info.GetValue("sortOrder", typeof (SortOrder), (object) SortOrder.None);
        this.SortPriority = info.GetInteger("sortPriority", -1);
        this.Alignment = (HorizontalAlignment) info.GetValue("alignment", typeof (HorizontalAlignment));
        this.Required = info.GetBoolean("required");
      }

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("id", (object) this.ColumnID);
        info.AddValue("title", (object) this.Title);
        info.AddValue("desc", (object) this.Description);
        info.AddValue("tag", (object) this.Tag);
        info.AddValue("width", (object) this.Width);
        info.AddValue("displayOrder", (object) this.DisplayOrder);
        info.AddValue("sortOrder", (object) this.SortOrder);
        info.AddValue("sortPriority", (object) this.SortPriority);
        info.AddValue("alignment", (object) this.Alignment);
        info.AddValue("required", (object) this.Required);
      }

      public string ColumnID => this.columnID;

      public override string ToString() => this.Description;

      public int CompareTo(object o)
      {
        if (!(o is TableLayout.Column column))
          return 1;
        return this.DisplayOrder != column.DisplayOrder ? this.DisplayOrder - column.DisplayOrder : string.Compare(this.Title, column.Title, true);
      }

      public override bool Equals(object obj)
      {
        return obj is TableLayout.Column column && column.ColumnID == this.ColumnID;
      }

      public override int GetHashCode() => this.ColumnID.GetHashCode();

      public object Clone() => this.MemberwiseClone();

      public TableLayout.Column Copy(string columnId)
      {
        TableLayout.Column column = (TableLayout.Column) this.Clone();
        column.columnID = columnId;
        return column;
      }

      public static int CompareDescriptions(TableLayout.Column a, TableLayout.Column b)
      {
        return string.Compare(a.Description, b.Description, true);
      }
    }
  }
}
