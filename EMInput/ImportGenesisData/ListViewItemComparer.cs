// Decompiled with JetBrains decompiler
// Type: ImportGenesisData.ListViewItemComparer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace ImportGenesisData
{
  internal class ListViewItemComparer : IComparer
  {
    private int col;

    public ListViewItemComparer() => this.col = 0;

    public ListViewItemComparer(int column) => this.col = column;

    public int Compare(object x, object y)
    {
      return string.Compare(((ListViewItem) x).SubItems[this.col].Text, ((ListViewItem) y).SubItems[this.col].Text);
    }
  }
}
