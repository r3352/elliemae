// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ListViewItemComparer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ListViewItemComparer : IComparer
  {
    private ListViewItemComparerType type;

    public ListViewItemComparer(ListViewItemComparerType source) => this.type = source;

    public int Compare(object x, object y)
    {
      int num1 = 0;
      int num2 = 0;
      ListViewItem listViewItem1 = (ListViewItem) x;
      ListViewItem listViewItem2 = (ListViewItem) y;
      if (this.type == ListViewItemComparerType.Persona)
      {
        num1 = ((Persona) listViewItem1.Tag).DisplayOrder;
        num2 = ((Persona) listViewItem2.Tag).DisplayOrder;
      }
      else if (this.type == ListViewItemComparerType.AclGroup)
      {
        num1 = ((AclGroup) listViewItem1.Tag).DisplayOrder;
        num2 = ((AclGroup) listViewItem2.Tag).DisplayOrder;
      }
      return num1.CompareTo(num2);
    }
  }
}
