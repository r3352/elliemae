// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.ViewSelectionDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientSession.Dashboard;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class ViewSelectionDialog : Form
  {
    private DashboardViewList viewList;
    private int selectedViewId;
    private IContainer components;
    private ListView lvwViewSelection;
    private Button btnOK;
    private Button btnCancel;
    private ColumnHeader columnHeader;

    public int SelectedViewId => this.selectedViewId;

    public ViewSelectionDialog(DashboardViewList viewList)
    {
      this.viewList = viewList;
      this.InitializeComponent();
      this.initializeDialog();
    }

    private void initializeDialog()
    {
      this.lvwViewSelection.BeginUpdate();
      foreach (DashboardView view in (Collection<DashboardView>) this.viewList)
        this.lvwViewSelection.Items.Add(new ListViewItem(view.ViewName)
        {
          Tag = (object) view
        });
      this.lvwViewSelection.EndUpdate();
    }

    private void lvwViewSelection_ItemSelectionChanged(
      object sender,
      ListViewItemSelectionChangedEventArgs e)
    {
      this.btnOK.Enabled = 1 == this.lvwViewSelection.SelectedItems.Count;
    }

    private void lvwViewSelection_DoubleClick(object sender, EventArgs e)
    {
      if (1 != this.lvwViewSelection.SelectedItems.Count)
        return;
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.selectedViewId = 0;
      if (1 == this.lvwViewSelection.SelectedItems.Count)
      {
        if (this.lvwViewSelection.SelectedItems[0].Tag is DashboardView tag)
          this.selectedViewId = tag.ViewId;
        this.DialogResult = DialogResult.OK;
      }
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lvwViewSelection = new ListView();
      this.columnHeader = new ColumnHeader();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lvwViewSelection.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwViewSelection.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader
      });
      this.lvwViewSelection.FullRowSelect = true;
      this.lvwViewSelection.Location = new Point(12, 12);
      this.lvwViewSelection.MultiSelect = false;
      this.lvwViewSelection.Name = "lvwViewSelection";
      this.lvwViewSelection.Size = new Size(275, 355);
      this.lvwViewSelection.TabIndex = 0;
      this.lvwViewSelection.UseCompatibleStateImageBehavior = false;
      this.lvwViewSelection.View = View.Details;
      this.lvwViewSelection.DoubleClick += new EventHandler(this.lvwViewSelection_DoubleClick);
      this.lvwViewSelection.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.lvwViewSelection_ItemSelectionChanged);
      this.columnHeader.Text = "View Name";
      this.columnHeader.Width = 250;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(131, 373);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(212, 373);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(299, 408);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lvwViewSelection);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewSelectionDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Dashboard View";
      this.ResumeLayout(false);
    }
  }
}
