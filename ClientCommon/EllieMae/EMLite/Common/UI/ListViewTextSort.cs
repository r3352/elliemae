// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewTextSort
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewTextSort(int sortColumn, bool ascending, ListViewSortManager sortMngr) : 
    ListViewCascadingSort(sortColumn, ascending, sortMngr)
  {
    protected override int CompareItems(ListViewItem lhs, ListViewItem rhs)
    {
      ListViewItem.ListViewSubItemCollection subItems1 = lhs.SubItems;
      ListViewItem.ListViewSubItemCollection subItems2 = rhs.SubItems;
      int num = this.CompareText(subItems1.Count > this.Column ? subItems1[this.Column].Text : string.Empty, subItems2.Count > this.Column ? subItems2[this.Column].Text : string.Empty);
      if (!this.Ascending)
        num = -num;
      return num;
    }

    protected virtual int CompareText(string lhs, string rhs) => string.Compare(lhs, rhs, false);

    protected string ConvertToNumericString(string text)
    {
      string numericString = "";
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsDigit(text[index]) || text[index] == '.')
          numericString += text[index].ToString();
      }
      return numericString;
    }
  }
}
