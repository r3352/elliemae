// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionalLetterSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionalLetterSelectionDialog : Form
  {
    private Dictionary<int, AclFileResource> allFiles = new Dictionary<int, AclFileResource>();
    private Dictionary<int, FileInGroup> currentSelectedFile = new Dictionary<int, FileInGroup>();
    private int currentGroupID;
    private EventHandler dirtyFlagChanged;
    private bool dirty;
    private Sessions.Session session;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private ListView listViewRight;
    private ColumnHeader columnHeader2;
    private ListView listViewLeft;
    private ColumnHeader columnHeader1;
    private Label label2;
    private Label label1;
    private Button btnRemove;
    private Button btnAdd;
    private Label label3;

    public ConditionalLetterSelectionDialog(
      Sessions.Session session,
      int groupId,
      List<AclFileResource> fileList,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.InitializeComponent();
      this.dirtyFlagChanged += dirtyFlagChanged;
      this.currentGroupID = groupId;
      foreach (AclFileResource file in fileList)
        this.allFiles.Add(file.FileID, file);
      this.initialPageValue();
    }

    private void initialPageValue()
    {
      foreach (FileInGroup aclGroupFileRef in this.session.AclGroupManager.GetAclGroupFileRefs(this.currentGroupID, AclFileType.ConditionalApprovalLetter))
        this.currentSelectedFile.Add(aclGroupFileRef.FileID, aclGroupFileRef);
      foreach (int key in this.allFiles.Keys)
      {
        AclFileResource allFile = this.allFiles[key];
        ListViewItem listViewItem = new ListViewItem(allFile.FileName);
        listViewItem.Tag = (object) allFile;
        if (this.currentSelectedFile.ContainsKey(allFile.FileID))
          this.listViewRight.Items.Add(listViewItem);
        else
          this.listViewLeft.Items.Add(listViewItem);
      }
    }

    public bool HasBeenModified() => this.dirty;

    public void SaveData()
    {
      List<FileInGroup> fileInGroupList = new List<FileInGroup>();
      foreach (int key in this.currentSelectedFile.Keys)
        fileInGroupList.Add(new FileInGroup(this.currentSelectedFile[key].FileID, false, AclResourceAccess.ReadWrite));
      this.session.AclGroupManager.ResetAclGroupFileRefs(this.currentGroupID, fileInGroupList.ToArray(), AclFileType.ConditionalApprovalLetter);
      this.dirty = false;
    }

    public AclFileResource[] GetSelectedFileID()
    {
      List<AclFileResource> aclFileResourceList = new List<AclFileResource>();
      foreach (int key in this.currentSelectedFile.Keys)
        aclFileResourceList.Add(this.allFiles[key]);
      return aclFileResourceList.ToArray();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.listViewLeft.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the forms you want to make visible in the left list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.dirty = true;
        ListViewItem[] selectedListViewItems = this.getSelectedListViewItems(this.listViewLeft);
        for (int index = 0; index < selectedListViewItems.Length; ++index)
        {
          selectedListViewItems[index].Remove();
          selectedListViewItems[index].Selected = false;
        }
        this.listViewRight.Items.AddRange(selectedListViewItems);
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.listViewRight.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the forms you want to make invisible in the right list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.dirty = true;
        ListViewItem[] selectedListViewItems = this.getSelectedListViewItems(this.listViewRight);
        for (int index = 0; index < selectedListViewItems.Length; ++index)
        {
          selectedListViewItems[index].Remove();
          selectedListViewItems[index].Selected = false;
        }
        this.listViewLeft.Items.AddRange(selectedListViewItems);
      }
    }

    private ListViewItem[] getSelectedListViewItems(ListView listView)
    {
      int count = listView.SelectedItems.Count;
      if (count == 0)
        return new ListViewItem[0];
      ListViewItem[] selectedListViewItems = new ListViewItem[count];
      int index1 = 0;
      for (int index2 = 0; index2 < listView.Items.Count; ++index2)
      {
        ListViewItem listViewItem = listView.Items[index2];
        if (listView.SelectedItems.Contains(listViewItem))
        {
          selectedListViewItems[index1] = listViewItem;
          ++index1;
        }
      }
      return selectedListViewItems;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.currentSelectedFile.Clear();
      foreach (ListViewItem listViewItem in this.listViewRight.Items)
      {
        AclFileResource tag = (AclFileResource) listViewItem.Tag;
        this.currentSelectedFile.Add(tag.FileID, new FileInGroup(tag.FileID, false, AclResourceAccess.ReadWrite));
      }
      if (this.dirtyFlagChanged != null)
        this.dirtyFlagChanged((object) this, (EventArgs) null);
      this.DialogResult = DialogResult.OK;
    }

    public void SetTempState(List<AclFileResource> fileList)
    {
      this.currentSelectedFile.Clear();
      foreach (AclFileResource file in fileList)
        this.currentSelectedFile.Add(file.FileID, new FileInGroup(file.FileID, false, AclResourceAccess.ReadWrite));
      List<ListViewItem> listViewItemList1 = new List<ListViewItem>();
      List<ListViewItem> listViewItemList2 = new List<ListViewItem>();
      foreach (ListViewItem listViewItem in this.listViewRight.Items)
      {
        if (this.currentSelectedFile.ContainsKey(((AclFileResource) listViewItem.Tag).FileID))
          listViewItemList2.Add(listViewItem);
        else
          listViewItemList1.Add(listViewItem);
        listViewItem.Remove();
      }
      foreach (ListViewItem listViewItem in this.listViewLeft.Items)
      {
        if (this.currentSelectedFile.ContainsKey(((AclFileResource) listViewItem.Tag).FileID))
          listViewItemList2.Add(listViewItem);
        else
          listViewItemList1.Add(listViewItem);
        listViewItem.Remove();
      }
      foreach (ListViewItem listViewItem in listViewItemList1)
        this.listViewLeft.Items.Add(listViewItem);
      foreach (ListViewItem listViewItem in listViewItemList2)
        this.listViewRight.Items.Add(listViewItem);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.listViewRight = new ListView();
      this.columnHeader2 = new ColumnHeader();
      this.listViewLeft = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.label2 = new Label();
      this.label1 = new Label();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.label3 = new Label();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(410, 331);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 28);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Location = new Point(330, 331);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 28);
      this.btnOK.TabIndex = 19;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.listViewRight.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader2
      });
      this.listViewRight.FullRowSelect = true;
      this.listViewRight.HeaderStyle = ColumnHeaderStyle.None;
      this.listViewRight.Location = new Point(288, 29);
      this.listViewRight.Name = "listViewRight";
      this.listViewRight.Size = new Size(212, 284);
      this.listViewRight.TabIndex = 18;
      this.listViewRight.UseCompatibleStateImageBehavior = false;
      this.listViewRight.View = View.Details;
      this.columnHeader2.Width = 178;
      this.listViewLeft.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.listViewLeft.FullRowSelect = true;
      this.listViewLeft.HeaderStyle = ColumnHeaderStyle.None;
      this.listViewLeft.Location = new Point(12, 29);
      this.listViewLeft.Name = "listViewLeft";
      this.listViewLeft.Size = new Size(212, 284);
      this.listViewLeft.TabIndex = 17;
      this.listViewLeft.UseCompatibleStateImageBehavior = false;
      this.listViewLeft.View = View.Details;
      this.columnHeader1.Width = 178;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(285, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(164, 20);
      this.label2.TabIndex = 16;
      this.label2.Text = "Selected Forms";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(9, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(198, 20);
      this.label1.TabIndex = 15;
      this.label1.Text = "All Condition Forms";
      this.btnRemove.Location = new Point(232, 129);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(48, 28);
      this.btnRemove.TabIndex = 14;
      this.btnRemove.Text = "<<";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Location = new Point(232, 73);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(48, 28);
      this.btnAdd.TabIndex = 13;
      this.btnAdd.Text = ">>";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.label3.BorderStyle = BorderStyle.Fixed3D;
      this.label3.Location = new Point(12, 324);
      this.label3.Name = "label3";
      this.label3.Size = new Size(488, 2);
      this.label3.TabIndex = 21;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(514, 366);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.listViewRight);
      this.Controls.Add((Control) this.listViewLeft);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionalLetterSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Condition Forms";
      this.ResumeLayout(false);
    }
  }
}
