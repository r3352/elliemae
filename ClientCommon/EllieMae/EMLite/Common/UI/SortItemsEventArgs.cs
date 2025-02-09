// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SortItemsEventArgs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class SortItemsEventArgs : EventArgs
  {
    private int columnIndex;
    private SortOrder sortOrder;
    private bool cancel;

    public SortItemsEventArgs(int columnIndex, SortOrder sortOrder)
    {
      this.columnIndex = columnIndex;
      this.sortOrder = sortOrder;
    }

    public int ColumnIndex => this.columnIndex;

    public SortOrder SortOrder => this.sortOrder;

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
