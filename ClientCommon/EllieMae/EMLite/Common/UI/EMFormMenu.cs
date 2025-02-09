// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.EMFormMenu
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class EMFormMenu : ListBoxEx
  {
    private List<string> formList = new List<string>();
    private List<int> dividerIndices = new List<int>();
    private EMFormMenu.DisplayMode displayMode;
    private bool supressEvents;

    public EMFormMenu()
    {
      this.AlternatingColors = false;
      this.GridLines = false;
      this.SelectionMode = SelectionMode.One;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedFormName
    {
      get => string.Concat(this.SelectedItem);
      set
      {
        this.SelectedItem = (object) value;
        this.Refresh();
      }
    }

    public override int SelectedIndex
    {
      get => base.SelectedIndex;
      set
      {
        base.SelectedIndex = value;
        this.Refresh();
      }
    }

    [Category("Appearance")]
    [DefaultValue(EMFormMenu.DisplayMode.Default)]
    public EMFormMenu.DisplayMode ListDisplayMode
    {
      get => this.displayMode;
      set
      {
        if (this.displayMode == value)
          return;
        this.displayMode = value;
        this.RefreshFormList();
      }
    }

    public void ClearFormList()
    {
      this.formList.Clear();
      this.dividerIndices.Clear();
      this.Items.Clear();
    }

    public void RemoveForm(string formName)
    {
      int index1 = this.formList.IndexOf(formName);
      if (index1 < 0)
        return;
      this.formList.RemoveAt(index1);
      for (int index2 = 0; index2 < this.dividerIndices.Count; ++index2)
      {
        if (this.dividerIndices[index2] >= index1)
          this.dividerIndices[index2]--;
      }
      this.Items.Remove((object) formName);
    }

    public void RefreshFormList()
    {
      this.BeginUpdate();
      string selectedFormName = this.SelectedFormName;
      this.supressEvents = true;
      this.Items.Clear();
      List<string> stringList = this.formList;
      if (this.displayMode == EMFormMenu.DisplayMode.Alphabetical)
      {
        stringList = new List<string>((IEnumerable<string>) this.formList);
        stringList.Sort();
      }
      foreach (object obj in stringList)
        this.Items.Add(obj);
      if (selectedFormName != "")
      {
        int index = this.Items.IndexOf((object) selectedFormName);
        if (index >= 0)
          this.SetSelected(index, true);
      }
      this.supressEvents = false;
      if (selectedFormName != "" && this.SelectedItems.Count == 0)
        this.OnSelectedIndexChanged(EventArgs.Empty);
      this.EndUpdate();
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.supressEvents)
        return;
      base.OnSelectedIndexChanged(e);
    }

    public void LoadFormList(string[] formNames)
    {
      this.dividerIndices = new List<int>();
      this.formList = new List<string>();
      for (int index = 0; index < formNames.Length; ++index)
      {
        string formName = formNames[index];
        if (this.isDivider(formName))
          this.dividerIndices.Add(this.formList.Count);
        else
          this.formList.Add(formName);
      }
      this.RefreshFormList();
    }

    public bool CompareToFormList(string[] formNames)
    {
      for (int index = 0; index < formNames.Length; ++index)
      {
        if (!this.isDivider(formNames[index]) && !this.formList.Contains(formNames[index]))
          return false;
      }
      List<string> stringList = new List<string>((IEnumerable<string>) formNames);
      for (int index = 0; index < this.formList.Count; ++index)
      {
        if (!stringList.Contains(this.formList[index]))
          return false;
      }
      return true;
    }

    public bool AppendForms(string[] formNames, bool addDivider)
    {
      if (this.formList == null)
      {
        this.LoadFormList(formNames);
        return true;
      }
      bool flag = false;
      for (int index = 0; index < formNames.Length; ++index)
      {
        string formName = formNames[index];
        if (this.isDivider(formName))
          addDivider = true;
        else if (!this.formList.Contains(formName))
        {
          if (addDivider)
            this.dividerIndices.Add(this.formList.Count);
          this.formList.Add(formName);
          flag = true;
          addDivider = false;
        }
      }
      if (flag)
        this.RefreshFormList();
      return flag;
    }

    public bool AddForms(string[] formNames, string addToFormName, bool rightAfter)
    {
      if (this.formList == null)
      {
        this.LoadFormList(formNames);
        return true;
      }
      for (int index = 0; index < formNames.Length; ++index)
      {
        if (this.formList.Contains(formNames[index]))
          formNames[index] = string.Empty;
      }
      for (int index1 = 0; index1 < this.formList.Count; ++index1)
      {
        if (string.Compare(this.formList[index1], addToFormName, true) == 0)
        {
          int num = 0;
          for (int index2 = 0; index2 < formNames.Length; ++index2)
          {
            if (formNames[index2] != string.Empty)
            {
              this.formList.Insert(rightAfter ? index1 + 1 : index1, formNames[index2]);
              ++num;
            }
          }
          for (int index3 = 0; index3 < this.dividerIndices.Count; ++index3)
          {
            if (this.dividerIndices[index3] >= (rightAfter ? index1 + 1 : index1))
              this.dividerIndices[index3] += num;
          }
          this.RefreshFormList();
          return true;
        }
      }
      return false;
    }

    private bool isDivider(string formName) => formName.StartsWith("----") || formName == "-";

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      base.OnDrawItem(e);
      if (this.displayMode != EMFormMenu.DisplayMode.Default || !this.dividerIndices.Contains(e.Index) || e.Index == 0)
        return;
      Point pt1 = new Point(e.Bounds.X, e.Bounds.Top);
      Point pt2 = new Point(e.Bounds.Right - 1, e.Bounds.Top);
      using (Pen pen = new Pen(EncompassColors.Secondary1))
        e.Graphics.DrawLine(pen, pt1, pt2);
    }

    public enum DisplayMode
    {
      Default,
      Alphabetical,
    }
  }
}
