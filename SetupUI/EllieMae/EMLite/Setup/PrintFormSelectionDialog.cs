// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintFormSelectionDialog : Form
  {
    private Sessions.Session session;
    private int groupId = -1;
    private Button btnAdd;
    private Button btnRemove;
    private Label label1;
    private Label label2;
    private IContainer components;
    private ListView listViewLeft;
    private ColumnHeader columnHeader1;
    private ListView listViewRight;
    private ColumnHeader columnHeader2;
    private Button btnOK;
    private Button btnCancel;
    private Label label3;
    private ListViewSortManager sortMgrLeft;
    private ListViewSortManager sortMgrRight;
    private bool dirty;
    private bool useURLA2020;
    private bool useNewURLAForms;

    public event EventHandler DirtyFlagChanged;

    public PrintFormSelectionDialog(
      Sessions.Session session,
      int groupId,
      EventHandler dirtyFlagChanged)
    {
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.session = session;
      this.useURLA2020 = this.session.StartupInfo.AllowURLA2020;
      this.useNewURLAForms = DateTime.Now.Date >= Utils.ParseDate((object) this.session.ConfigurationManager.GetCompanySetting("Policies", "URLA2020")).Date;
      this.sortMgrLeft = new ListViewSortManager(this.listViewLeft, new System.Type[1]
      {
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgrLeft.Sort(0);
      this.sortMgrRight = new ListViewSortManager(this.listViewRight, new System.Type[1]
      {
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgrRight.Sort(0);
      this.groupId = groupId;
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.listViewLeft = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.listViewRight = new ListView();
      this.columnHeader2 = new ColumnHeader();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label3 = new Label();
      this.SuspendLayout();
      this.btnAdd.Location = new Point(306, 68);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(48, 28);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = ">>";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnRemove.Location = new Point(306, 124);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(48, 28);
      this.btnRemove.TabIndex = 5;
      this.btnRemove.Text = "<<";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(184, 20);
      this.label1.TabIndex = 7;
      this.label1.Text = "All Standard Print Forms";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(365, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(164, 20);
      this.label2.TabIndex = 8;
      this.label2.Text = "Selected Forms";
      this.listViewLeft.Activation = ItemActivation.OneClick;
      this.listViewLeft.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.listViewLeft.FullRowSelect = true;
      this.listViewLeft.HeaderStyle = ColumnHeaderStyle.None;
      this.listViewLeft.Location = new Point(8, 24);
      this.listViewLeft.Name = "listViewLeft";
      this.listViewLeft.Size = new Size(291, 370);
      this.listViewLeft.TabIndex = 9;
      this.listViewLeft.UseCompatibleStateImageBehavior = false;
      this.listViewLeft.View = View.Details;
      this.columnHeader1.Width = 285;
      this.listViewRight.Activation = ItemActivation.OneClick;
      this.listViewRight.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader2
      });
      this.listViewRight.FullRowSelect = true;
      this.listViewRight.HeaderStyle = ColumnHeaderStyle.None;
      this.listViewRight.Location = new Point(361, 24);
      this.listViewRight.Name = "listViewRight";
      this.listViewRight.Size = new Size(291, 370);
      this.listViewRight.TabIndex = 10;
      this.listViewRight.UseCompatibleStateImageBehavior = false;
      this.listViewRight.View = View.Details;
      this.columnHeader2.Width = 285;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(501, 416);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 28);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(581, 416);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 28);
      this.btnCancel.TabIndex = 12;
      this.btnCancel.Text = "Cancel";
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.BorderStyle = BorderStyle.Fixed3D;
      this.label3.Location = new Point(8, 408);
      this.label3.Name = "label3";
      this.label3.Size = new Size(646, 2);
      this.label3.TabIndex = 13;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(661, 456);
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
      this.Name = nameof (PrintFormSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Standard Print Forms";
      this.ResumeLayout(false);
    }

    private void init() => this.loadFormLists();

    private void loadFormLists()
    {
      this.listViewLeft.Items.Clear();
      this.listViewRight.Items.Clear();
      ArrayList arrayList = new ArrayList((ICollection) this.session.AclGroupManager.GetAclGroupStdPrintForms(this.groupId));
      PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
      for (int index = 0; index < printFormList.Count; ++index)
      {
        PrintForm printForm = printFormList[index];
        ListViewItem listViewItem = new ListViewItem(new string[1]
        {
          printForm.UIName
        });
        listViewItem.Tag = (object) printForm.FormID;
        if (!(printForm.URLAFormVersion == "URLA2020") || this.useURLA2020)
        {
          if (arrayList.Contains((object) printForm.FormID))
            this.listViewRight.Items.Add(listViewItem);
          else
            this.listViewLeft.Items.Add(listViewItem);
        }
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

    private void btnOK_Click(object sender, EventArgs e)
    {
      string[] strArray = new string[this.listViewRight.Items.Count];
      for (int index = 0; index < this.listViewRight.Items.Count; ++index)
        strArray[index] = (string) this.listViewRight.Items[index].Tag;
      if (this.DirtyFlagChanged != null)
        this.DirtyFlagChanged((object) this, (EventArgs) null);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    public string[] getCurrentSelectedFiles()
    {
      string[] currentSelectedFiles = new string[this.listViewRight.Items.Count];
      for (int index = 0; index < this.listViewRight.Items.Count; ++index)
        currentSelectedFiles[index] = (string) this.listViewRight.Items[index].Tag;
      return currentSelectedFiles;
    }

    public void SaveData()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.listViewRight.Items.Count; ++index)
        arrayList.Add((object) (string) this.listViewRight.Items[index].Tag);
      if (!this.useURLA2020 || !this.useNewURLAForms)
      {
        if (arrayList.Contains((object) "1003 - All Required Pages") && !arrayList.Contains((object) "2020 URLA (1003/65) - All Pages (Legal)"))
          arrayList.Add((object) "2020 URLA (1003/65) - All Pages (Legal)");
        if (arrayList.Contains((object) "1003 - All Required Pages (Letter)") && !arrayList.Contains((object) "2020 URLA (1003/65) - All Pages"))
          arrayList.Add((object) "2020 URLA (1003/65) - All Pages");
      }
      this.session.AclGroupManager.ResetAclGroupStdPrintForms(this.groupId, Array.ConvertAll<object, string>(arrayList.ToArray(), (Converter<object, string>) (x => x.ToString())));
      this.dirty = false;
    }

    public bool HasBeenModified() => this.dirty;

    public void SetTempState(string[] fileIds)
    {
      this.listViewLeft.Items.Clear();
      this.listViewRight.Items.Clear();
      ArrayList arrayList = new ArrayList((ICollection) fileIds);
      PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
      for (int index = 0; index < printFormList.Count; ++index)
      {
        PrintForm printForm = printFormList[index];
        ListViewItem listViewItem = new ListViewItem(new string[1]
        {
          printForm.UIName
        });
        listViewItem.Tag = (object) printForm.FormID;
        if (!(printForm.URLAFormVersion == "URLA2020") || this.useURLA2020)
        {
          if (arrayList.Contains((object) printForm.FormID))
            this.listViewRight.Items.Add(listViewItem);
          else
            this.listViewLeft.Items.Add(listViewItem);
        }
      }
    }
  }
}
