// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ClientCommonUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI.Controls;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ClientCommonUtils
  {
    public static void PopulateDropdown(ComboBox box, object value, bool addValue)
    {
      if (box.Items.Contains(value))
        box.SelectedItem = value;
      else if (string.Concat(value) == "")
        box.SelectedIndex = -1;
      else if (addValue)
      {
        box.Items.Add(value);
        box.SelectedItem = value;
      }
      else
        box.SelectedIndex = -1;
    }

    public static bool ItemExistInDropDown(ComboBox box, object value) => box.Items.Contains(value);

    public static void PopulateLoanFolderDropdown(
      ComboBox box,
      LoanFolderInfo loanFolder,
      bool addValue)
    {
      if (loanFolder == null)
      {
        box.SelectedIndex = -1;
      }
      else
      {
        int num = -1;
        for (int index = 0; index < box.Items.Count; ++index)
        {
          if (((LoanFolderInfo) box.Items[index]).Name.ToUpper() == loanFolder.Name.ToUpper())
          {
            num = index;
            break;
          }
        }
        if (num >= 0)
          box.SelectedIndex = num;
        else if ((loanFolder.Name ?? "") == "")
          box.SelectedIndex = -1;
        else if (addValue)
        {
          box.Items.Add((object) loanFolder);
          box.SelectedItem = (object) loanFolder;
        }
        else
          box.SelectedIndex = -1;
      }
    }

    public static void ApplyControlStateToMenu(ToolStripItem menuItem, Control stateControl)
    {
      menuItem.Visible = stateControl.Visible;
      menuItem.Enabled = stateControl.Enabled;
    }

    public static void CheckLoanFolders(CheckedComboBox box, List<ComboBoxItem> items)
    {
      foreach (ComboBoxItem comboBoxItem in items)
        ClientCommonUtils.CheckLoanFolder(box, comboBoxItem.Name, CheckState.Checked);
    }

    public static void UncheckLoanFolders(CheckedComboBox box, List<ComboBoxItem> items)
    {
      foreach (ComboBoxItem comboBoxItem in items)
        ClientCommonUtils.CheckLoanFolder(box, comboBoxItem.Name, CheckState.Unchecked);
    }

    public static void CheckLoanFolder(
      CheckedComboBox box,
      string folderName,
      CheckState checkState)
    {
      ComboBoxItem comboBoxItem = (ComboBoxItem) null;
      foreach (object obj in (ListBox.ObjectCollection) box.Items)
      {
        if (string.Compare(((ComboBoxItem) obj).Name, folderName, true) == 0)
        {
          comboBoxItem = (ComboBoxItem) obj;
          break;
        }
      }
      if (comboBoxItem == null)
        return;
      box.SetItemChecked(comboBoxItem, checkState);
    }

    public static string[] GetSelectedFolders(CheckedComboBox box)
    {
      List<string> stringList = new List<string>();
      bool flag = false;
      foreach (ComboBoxItem checkedItem in box.CheckedItems)
      {
        if (string.Compare(checkedItem.Name, "<all folders>", true) == 0)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        foreach (ComboBoxItem comboBoxItem in (ListBox.ObjectCollection) box.Items)
        {
          string name = comboBoxItem.Name;
          if (string.Compare(name, "<all folders>", true) != 0)
            stringList.Add(name);
        }
      }
      else
      {
        foreach (ComboBoxItem checkedItem in box.CheckedItems)
        {
          string name = checkedItem.Name;
          if (string.Compare(name, "<all folders>", true) != 0)
            stringList.Add(name);
        }
      }
      return stringList.ToArray();
    }
  }
}
