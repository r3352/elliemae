// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneListControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneListControl : UserControl
  {
    private ComboBox comboBoxMS;
    private System.ComponentModel.Container components;
    private ListViewItem viewItem;
    private const string ARCHIVED = " (Archived)";
    private bool forField;
    private bool setListViewItemTag = true;

    public MilestoneListControl(
      string[] msList,
      string selected,
      ListViewItem viewItem,
      bool forField)
      : this(msList, selected, viewItem, forField, true)
    {
    }

    public MilestoneListControl(
      string[] msList,
      string selected,
      ListViewItem viewItem,
      bool forField,
      bool setListViewItemTag)
    {
      this.viewItem = viewItem;
      this.forField = forField;
      this.setListViewItemTag = setListViewItemTag;
      this.InitializeComponent();
      this.comboBoxMS.Dock = DockStyle.Fill;
      this.comboBoxMS.Items.AddRange((object[]) msList);
      this.comboBoxMS.Text = selected;
      this.comboBoxMS_SelectedIndexChanged((object) null, (EventArgs) null);
      this.comboBoxMS.SelectedIndexChanged += new EventHandler(this.comboBoxMS_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string SelectedText => this.comboBoxMS.Text;

    private void InitializeComponent()
    {
      this.comboBoxMS = new ComboBox();
      this.SuspendLayout();
      this.comboBoxMS.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxMS.Location = new Point(0, 0);
      this.comboBoxMS.Name = "comboBoxMS";
      this.comboBoxMS.Size = new Size(121, 21);
      this.comboBoxMS.TabIndex = 0;
      this.Controls.Add((Control) this.comboBoxMS);
      this.Name = nameof (MilestoneListControl);
      this.Size = new Size(200, 20);
      this.ResumeLayout(false);
    }

    private void comboBoxMS_SelectedIndexChanged(object sender, EventArgs e)
    {
      string str = this.comboBoxMS.Text.Replace(" (Archived)", "");
      if (this.forField)
        this.viewItem.SubItems[2].Text = str;
      else
        this.viewItem.SubItems[1].Text = str;
      if (!this.setListViewItemTag)
        return;
      this.viewItem.Tag = (object) str;
    }
  }
}
