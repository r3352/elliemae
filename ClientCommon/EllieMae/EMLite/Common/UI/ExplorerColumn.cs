// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ExplorerColumn
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ExplorerColumn
  {
    private GVColumn column;
    private string dataProperty;
    private string dataFormat;

    internal ExplorerColumn(GVColumn col) => this.column = col;

    public string Title
    {
      get => this.column.Text;
      set => this.column.Text = value;
    }

    public GVSortMethod SortMethod
    {
      get => this.column.SortMethod;
      set => this.column.SortMethod = value;
    }

    public int Width
    {
      get => this.column.Width;
      set => this.column.Width = value;
    }

    public string DataProperty
    {
      get => this.dataProperty == null ? this.Title : this.dataProperty;
      set => this.dataProperty = value;
    }

    public string DataFormat
    {
      get => this.dataFormat;
      set => this.dataFormat = value;
    }
  }
}
