// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ChangeOfCircumstanceSelectionPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ChangeOfCircumstanceSelectionPage : Form
  {
    private Dictionary<int, AclFileResource> allFiles = new Dictionary<int, AclFileResource>();
    private Dictionary<int, ChangeCircumstanceSettings> currentSelectedFile = new Dictionary<int, ChangeCircumstanceSettings>();
    private int currentGroupID;
    private bool dirty;
    private Sessions.Session session;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label2;
    private Label label1;
    private Button btnRemove;
    private Button btnAdd;
    private GridView gcAll;
    private GridView gcSelected;

    public event EventHandler DirtyFlagChanged;

    public ChangeOfCircumstanceSelectionPage(
      Sessions.Session session,
      int groupId,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.currentGroupID = groupId;
      this.initialPageValue();
    }

    private void initialPageValue()
    {
      string[] circumstanceOptions = this.session.AclGroupManager.GetAclGroupChangeCircumstanceOptions(this.currentGroupID);
      foreach (ChangeCircumstanceSettings circumstanceSetting in this.session.ConfigurationManager.GetAllChangeCircumstanceSettings())
      {
        GVItem gvItem = new GVItem(circumstanceSetting.Description);
        gvItem.SubItems.Add((object) circumstanceSetting.CocType);
        gvItem.Tag = (object) circumstanceSetting;
        if (((IEnumerable<string>) circumstanceOptions).Contains<string>(circumstanceSetting.optionId.ToString()))
        {
          this.currentSelectedFile.Add(circumstanceSetting.optionId, circumstanceSetting);
          this.gcSelected.Items.Add(gvItem);
        }
        else
          this.gcAll.Items.Add(gvItem);
      }
    }

    public bool HasBeenModified() => this.dirty;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.DirtyFlagChanged != null)
        this.DirtyFlagChanged((object) this, (EventArgs) null);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.gcAll.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the options you want to make visible in the left list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.dirty = true;
        foreach (GVItem selectedItem in this.gcAll.SelectedItems)
        {
          this.gcAll.Items.Remove(selectedItem);
          selectedItem.Selected = false;
          this.gcSelected.Items.Add(selectedItem);
        }
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gcSelected.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the options you want to make invisible in the right list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.dirty = true;
        foreach (GVItem selectedItem in this.gcSelected.SelectedItems)
        {
          this.gcSelected.Items.Remove(selectedItem);
          selectedItem.Selected = false;
          this.gcAll.Items.Add(selectedItem);
        }
      }
    }

    public void SaveData()
    {
      string[] optionIDs = new string[this.gcSelected.Items.Count];
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gcSelected.Items)
      {
        for (int nItemIndex = 0; nItemIndex < this.gcSelected.Items.Count; ++nItemIndex)
          optionIDs[nItemIndex] = ((ChangeCircumstanceSettings) this.gcSelected.Items[nItemIndex].Tag).optionId.ToString();
      }
      this.session.AclGroupManager.ResetAclGroupChangeCircumstanceOptions(this.currentGroupID, optionIDs);
      this.dirty = false;
    }

    public List<ChangeCircumstanceSettings> getCurrentSelectedFiles()
    {
      List<ChangeCircumstanceSettings> currentSelectedFiles = new List<ChangeCircumstanceSettings>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gcSelected.Items)
        currentSelectedFiles.Add((ChangeCircumstanceSettings) gvItem.Tag);
      return currentSelectedFiles;
    }

    public void SetTempState(List<ChangeCircumstanceSettings> settings)
    {
      this.gcSelected.Items.Clear();
      this.gcAll.Items.Clear();
      this.currentSelectedFile.Clear();
      this.session.AclGroupManager.GetAclGroupChangeCircumstanceOptions(this.currentGroupID);
      foreach (ChangeCircumstanceSettings circumstanceSetting in this.session.ConfigurationManager.GetAllChangeCircumstanceSettings())
      {
        ChangeCircumstanceSettings cc = circumstanceSetting;
        GVItem gvItem = new GVItem(cc.Description);
        gvItem.SubItems.Add((object) cc.CocType);
        gvItem.Tag = (object) cc;
        if (settings.Any<ChangeCircumstanceSettings>((Func<ChangeCircumstanceSettings, bool>) (s => s.Code == cc.Code)))
        {
          this.currentSelectedFile.Add(cc.optionId, cc);
          this.gcSelected.Items.Add(gvItem);
        }
        else
          this.gcAll.Items.Add(gvItem);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label2 = new Label();
      this.label1 = new Label();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.gcAll = new GridView();
      this.gcSelected = new GridView();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(531, 345);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 28);
      this.btnCancel.TabIndex = 29;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(451, 345);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 28);
      this.btnOK.TabIndex = 28;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(338, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(164, 20);
      this.label2.TabIndex = 25;
      this.label2.Text = "Selected CoC Options";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(198, 20);
      this.label1.TabIndex = 24;
      this.label1.Text = "All CoC Options";
      this.btnRemove.Location = new Point(285, 192);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(48, 28);
      this.btnRemove.TabIndex = 23;
      this.btnRemove.Text = "<<";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Location = new Point(285, 136);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(48, 28);
      this.btnAdd.TabIndex = 22;
      this.btnAdd.Text = ">>";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Description";
      gvColumn1.Width = 205;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "LE or CD";
      gvColumn2.Width = 55;
      this.gcAll.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gcAll.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gcAll.Location = new Point(15, 32);
      this.gcAll.Name = "gcAll";
      this.gcAll.Size = new Size(264, 307);
      this.gcAll.TabIndex = 31;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 205;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "LE or CD";
      gvColumn4.Width = 55;
      this.gcSelected.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gcSelected.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gcSelected.Location = new Point(341, 32);
      this.gcSelected.Name = "gcSelected";
      this.gcSelected.Size = new Size(264, 307);
      this.gcSelected.TabIndex = 32;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(615, 385);
      this.Controls.Add((Control) this.gcSelected);
      this.Controls.Add((Control) this.gcAll);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangeOfCircumstanceSelectionPage);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Change of Circumstance Options";
      this.ResumeLayout(false);
    }
  }
}
