// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PiggybackSetupPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PiggybackSetupPanel : SettingsUserControl
  {
    private Sessions.Session session;
    private PiggybackFields piggyFields;
    private Hashtable selectedFields;
    private bool isSettingSync;
    private FieldSettings fieldSettings;
    private bool forLinkSync;
    private SyncTemplate syncTemplate;
    private IContainer components;
    private GridView listView;
    private CheckedListBoxEx checkedListSync;
    private Button defaultBtn;
    private GroupContainer gcVerification;
    private GroupContainer gcFields;
    private StandardIconButton stdIconBtnNew;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnFind;
    private StandardIconButton stdIconBtnDelete;
    private VerticalSeparator verticalSeparator1;

    public event EventHandler OnStatusChanged;

    public PiggybackSetupPanel(
      Sessions.Session session,
      SyncTemplate syncTemplate,
      FieldSettings fieldSettings)
      : base((SetUpContainer) null)
    {
      this.session = session;
      this.forLinkSync = true;
      this.fieldSettings = fieldSettings;
      this.syncTemplate = syncTemplate;
      this.isSettingSync = false;
      this.InitializeComponent();
      this.listView.Columns[0].Width = 400;
      this.listView.Columns[1].SpringToFit = true;
      this.Dock = DockStyle.Fill;
      this.Reset();
      this.listView_SelectedIndexChanged((object) this, (EventArgs) null);
      this.setDirtyFlag(false);
    }

    public PiggybackSetupPanel(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.InitializeComponent();
      this.Reset();
      this.listView_SelectedIndexChanged((object) this, (EventArgs) null);
      this.setDirtyFlag(false);
      if (setupContainer != null)
        return;
      this.isSettingSync = true;
      this.defaultBtn.Enabled = false;
      this.stdIconBtnNew.Enabled = false;
      this.stdIconBtnFind.Enabled = false;
      this.stdIconBtnDelete.Enabled = false;
      this.checkedListSync.Enabled = false;
    }

    public string[] SelectedFieldIDs
    {
      get
      {
        return this.listView.SelectedItems.Count == 0 ? (string[]) null : this.listView.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToArray<string>();
      }
      set
      {
        if (value == null || ((IEnumerable<string>) value).Count<string>() == 0)
          return;
        foreach (GVItem gvItem in this.listView.Items.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(item.SubItems[0].Text))))
          gvItem.Selected = true;
      }
    }

    public override void Reset()
    {
      if (this.selectedFields != null)
        this.selectedFields.Clear();
      this.selectedFields = new Hashtable();
      this.listView.Items.Clear();
      for (int index = 0; index <= 3; ++index)
        this.checkedListSync.SetItemChecked(index, false);
      if (!this.session.StartupInfo.AllowURLA2020 && this.checkedListSync.Items.Count > 4)
      {
        for (int index = this.checkedListSync.Items.Count - 1; index > 3; --index)
          this.checkedListSync.Items.RemoveAt(index);
      }
      else if (this.session.StartupInfo.AllowURLA2020)
      {
        for (int index = 4; index <= 7; ++index)
          this.checkedListSync.SetItemChecked(index, false);
      }
      List<string> stringList = (List<string>) null;
      if (!this.session.StartupInfo.AllowURLA2020)
      {
        try
        {
          stringList = Utils.LoadPiggybackDefaultSyncFields((IWin32Window) null, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList20.xml", SystemSettings.LocalAppDir));
        }
        catch (Exception ex)
        {
          stringList = (List<string>) null;
        }
      }
      string[] fields;
      try
      {
        if (this.forLinkSync)
        {
          if (this.syncTemplate != null)
            this.syncTemplate.AddURLA2020Fields(stringList);
          fields = this.syncTemplate != null ? this.syncTemplate.GetSyncFields(!this.session.StartupInfo.AllowURLA2020).ToArray() : (string[]) null;
        }
        else
        {
          this.piggyFields = (PiggybackFields) this.session.GetSystemSettings(typeof (PiggybackFields));
          fields = this.piggyFields.GetSyncFields(stringList);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Piggyback Synchronization Field List cannot be accessed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      if (fields == null || fields.Length == 0)
        return;
      this.populateFields(fields);
      this.setDirtyFlag(false);
    }

    private void populateFields(string[] fields)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      this.listView.BeginUpdate();
      foreach (string field in fields)
      {
        if (!this.forLinkSync || !(field == "19") && !(field == "4084"))
        {
          switch (field)
          {
            case "":
              continue;
            case "URLARGG":
              flag5 = true;
              continue;
            case "URLAROA":
              flag6 = true;
              continue;
            case "URLAROIS":
              flag7 = true;
              continue;
            case "URLAROL":
              flag8 = true;
              continue;
            case "VOD":
              flag3 = true;
              continue;
            case "VOE":
              flag4 = true;
              continue;
            case "VOLVOM":
              flag1 = true;
              continue;
            case "VOR":
              flag2 = true;
              continue;
            default:
              this.listView.Items.Add(this.createListItem(field));
              this.selectedFields.Add((object) field, (object) "");
              continue;
          }
        }
      }
      this.listView.EndUpdate();
      this.listView.Sort(0, SortOrder.Ascending);
      this.updatesSyncronizationCount();
      this.checkedListSync.SetItemChecked(0, flag3);
      this.checkedListSync.SetItemChecked(1, flag2);
      this.checkedListSync.SetItemChecked(2, flag1);
      this.checkedListSync.SetItemChecked(3, flag4);
      if (!this.session.StartupInfo.AllowURLA2020)
        return;
      this.checkedListSync.SetItemChecked(4, flag5);
      this.checkedListSync.SetItemChecked(5, flag7);
      this.checkedListSync.SetItemChecked(6, flag8);
      this.checkedListSync.SetItemChecked(7, flag6);
    }

    private void updatesSyncronizationCount()
    {
      this.gcFields.Text = "Synchronization Fields (" + (object) this.listView.Items.Count + ")";
      if (this.OnStatusChanged == null)
        return;
      this.OnStatusChanged((object) null, new EventArgs());
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.listView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected fields?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
          return;
        int nItemIndex = 0;
        int num2 = 0;
        GVItem[] gvItemArray = new GVItem[this.listView.SelectedItems.Count];
        foreach (GVItem selectedItem in this.listView.SelectedItems)
        {
          gvItemArray[num2++] = selectedItem;
          nItemIndex = selectedItem.Index;
        }
        foreach (GVItem gvItem in gvItemArray)
        {
          if (this.selectedFields.ContainsKey((object) gvItem.Text))
            this.selectedFields.Remove((object) gvItem.Text);
          this.listView.Items.Remove(gvItem);
        }
        if (this.listView.Items.Count > 0)
        {
          if (nItemIndex >= this.listView.Items.Count)
            this.listView.Items[this.listView.Items.Count - 1].Selected = true;
          else
            this.listView.Items[nItemIndex].Selected = true;
        }
        this.updatesSyncronizationCount();
        this.setDirtyFlag(true);
      }
    }

    public void SetSyncTemplate(SyncTemplate syncTemplate)
    {
      syncTemplate.ClearFields();
      for (int nItemIndex = 0; nItemIndex < this.listView.Items.Count; ++nItemIndex)
        syncTemplate.AddField(this.listView.Items[nItemIndex].Text);
      for (int index = 0; index < this.checkedListSync.CheckedItems.Count; ++index)
      {
        switch (this.checkedListSync.CheckedItems[index].ToString())
        {
          case "VOD":
            syncTemplate.AddField("VOD");
            break;
          case "VOE":
            syncTemplate.AddField("VOE");
            break;
          case "VOL and VOM":
            syncTemplate.AddField("VOLVOM");
            break;
          case "VOR":
            syncTemplate.AddField("VOR");
            break;
          case "Verification of Gifts and Grants":
            syncTemplate.AddField("URLARGG");
            break;
          case "Verification of Other Assets":
            syncTemplate.AddField("URLAROA");
            break;
          case "Verification of Other Income":
            syncTemplate.AddField("URLAROIS");
            break;
          case "Verification of Other Liability":
            syncTemplate.AddField("URLAROL");
            break;
        }
      }
    }

    public bool SyncTemplateValidation()
    {
      return this.listView.Items.Count > 0 || this.checkedListSync.CheckedItems.Count > 0;
    }

    public override void Save() => this.saveList();

    private void saveList()
    {
      if (!this.IsDirty)
        return;
      this.piggyFields.ClearFields();
      for (int nItemIndex = 0; nItemIndex < this.listView.Items.Count; ++nItemIndex)
        this.piggyFields.AddField(this.listView.Items[nItemIndex].Text);
      for (int index = 0; index < this.checkedListSync.CheckedItems.Count; ++index)
      {
        switch (this.checkedListSync.CheckedItems[index].ToString())
        {
          case "VOD":
            this.piggyFields.AddField("VOD");
            break;
          case "VOR":
            this.piggyFields.AddField("VOR");
            break;
          case "VOL and VOM":
            this.piggyFields.AddField("VOLVOM");
            break;
          case "VOE":
            this.piggyFields.AddField("VOE");
            break;
        }
      }
      if (this.session.StartupInfo.AllowURLA2020)
      {
        for (int index = 0; index < this.checkedListSync.CheckedItems.Count; ++index)
        {
          switch (this.checkedListSync.CheckedItems[index].ToString())
          {
            case "Verification of Gifts and Grants":
              this.piggyFields.AddField("URLARGG");
              break;
            case "Verification of Other Income":
              this.piggyFields.AddField("URLAROIS");
              break;
            case "Verification of Other Liability":
              this.piggyFields.AddField("URLAROL");
              break;
            case "Verification of Other Assets":
              this.piggyFields.AddField("URLAROA");
              break;
          }
        }
      }
      this.session.SaveSystemSettings((object) this.piggyFields);
      this.updatesSyncronizationCount();
      this.setDirtyFlag(false);
    }

    private void findFieldbtn_Click(object sender, EventArgs e)
    {
      int num = 0;
      string[] existingFields = new string[this.selectedFields.Count];
      foreach (DictionaryEntry selectedField in this.selectedFields)
        existingFields[num++] = selectedField.Key.ToString();
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, existingFields, true, string.Empty, false, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
          return;
        this.addFields(ruleFindFieldDialog.SelectedRequiredFields);
        this.updatesSyncronizationCount();
      }
    }

    private void checkedListSync_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      this.setDirtyFlag(true);
      if (this.OnStatusChanged == null)
        return;
      this.OnStatusChanged((object) null, new EventArgs());
    }

    public void addFieldbtn_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.addFields(addFields.SelectedFieldIDs);
      }
      this.updatesSyncronizationCount();
    }

    private void addFields(string[] ids)
    {
      this.listView.BeginUpdate();
      string str = "";
      for (int index = 0; index < ids.Length; ++index)
      {
        if (!this.selectedFields.ContainsKey((object) ids[index]))
        {
          if (this.forLinkSync && (ids[index] == "19" || ids[index] == "4084"))
          {
            str = str + (str != "" ? ", " : "") + ids[index];
          }
          else
          {
            GVItem listItem = this.createListItem(ids[index]);
            listItem.Selected = true;
            this.listView.Items.Add(listItem);
            this.selectedFields.Add((object) ids[index], (object) "");
            this.setDirtyFlag(true);
          }
        }
      }
      this.listView.EndUpdate();
      if (!this.forLinkSync || !(str != ""))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The following fields can not be included sync list:\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
    }

    private GVItem createListItem(string id)
    {
      return new GVItem(id)
      {
        SubItems = {
          (object) EncompassFields.GetDescription(id, this.fieldSettings)
        }
      };
    }

    private void defaultBtn_Click(object sender, EventArgs e)
    {
      if ((this.listView.Items.Count > 0 || this.checkedListSync.CheckedItems.Count > 0) && Utils.Dialog((IWin32Window) this, "Are you sure you want to recover the default list?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
        return;
      List<string> stringList = Utils.LoadPiggybackDefaultSyncFields((IWin32Window) Session.MainScreen, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList.xml", SystemSettings.LocalAppDir), this.forLinkSync);
      if (stringList == null || stringList.Count == 0)
        return;
      if (this.session.StartupInfo.AllowURLA2020)
        stringList.AddRange((IEnumerable<string>) Utils.LoadPiggybackDefaultSyncFields((IWin32Window) this.session.MainForm, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList20.xml", SystemSettings.LocalAppDir)));
      if (this.selectedFields != null)
        this.selectedFields.Clear();
      this.selectedFields = new Hashtable();
      this.listView.Items.Clear();
      for (int index = 0; index <= 3; ++index)
        this.checkedListSync.SetItemChecked(index, false);
      if (this.session.StartupInfo.AllowURLA2020)
      {
        for (int index = 4; index <= 7; ++index)
          this.checkedListSync.SetItemChecked(index, false);
      }
      this.populateFields(stringList.ToArray());
      this.setDirtyFlag(true);
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count > 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.defaultBtn = new Button();
      this.checkedListSync = new CheckedListBoxEx();
      this.listView = new GridView();
      this.gcVerification = new GroupContainer();
      this.gcFields = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnFind = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcVerification.SuspendLayout();
      this.gcFields.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnFind).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.defaultBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.defaultBtn.BackColor = SystemColors.Control;
      this.defaultBtn.Location = new Point(473, 2);
      this.defaultBtn.Name = "defaultBtn";
      this.defaultBtn.Size = new Size(75, 22);
      this.defaultBtn.TabIndex = 20;
      this.defaultBtn.Text = "Default &List";
      this.defaultBtn.UseVisualStyleBackColor = true;
      this.defaultBtn.Click += new EventHandler(this.defaultBtn_Click);
      this.checkedListSync.BorderStyle = BorderStyle.None;
      this.checkedListSync.CheckOnClick = true;
      this.checkedListSync.Dock = DockStyle.Fill;
      this.checkedListSync.FormattingEnabled = true;
      this.checkedListSync.Items.AddRange(new object[8]
      {
        (object) "VOD",
        (object) "VOR",
        (object) "VOL and VOM",
        (object) "VOE",
        (object) "Verification of Gifts and Grants",
        (object) "Verification of Other Income",
        (object) "Verification of Other Liability",
        (object) "Verification of Other Assets"
      });
      this.checkedListSync.Location = new Point(1, 25);
      this.checkedListSync.Name = "checkedListSync";
      this.checkedListSync.Size = new Size(550, 130);
      this.checkedListSync.TabIndex = 12;
      this.checkedListSync.ItemCheck += new ItemCheckEventHandler(this.checkedListSync_ItemCheck);
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 94;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 215;
      this.listView.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.ForeColor = SystemColors.WindowText;
      this.listView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(550, 318);
      this.listView.TabIndex = 4;
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
      this.gcVerification.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcVerification.Controls.Add((Control) this.checkedListSync);
      this.gcVerification.Dock = DockStyle.Bottom;
      this.gcVerification.HeaderForeColor = SystemColors.ControlText;
      this.gcVerification.Location = new Point(0, 345);
      this.gcVerification.Name = "gcVerification";
      this.gcVerification.Size = new Size(552, 156);
      this.gcVerification.TabIndex = 19;
      this.gcVerification.Text = "Verifications";
      this.gcFields.Controls.Add((Control) this.verticalSeparator1);
      this.gcFields.Controls.Add((Control) this.stdIconBtnNew);
      this.gcFields.Controls.Add((Control) this.stdIconBtnFind);
      this.gcFields.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcFields.Controls.Add((Control) this.defaultBtn);
      this.gcFields.Controls.Add((Control) this.listView);
      this.gcFields.Dock = DockStyle.Fill;
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(0, 0);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(552, 345);
      this.gcFields.TabIndex = 20;
      this.gcFields.Text = "Synchronization Fields (0)";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(467, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 24;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(403, 4);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 17);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 23;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.addFieldbtn_Click);
      this.stdIconBtnFind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnFind.BackColor = Color.Transparent;
      this.stdIconBtnFind.Location = new Point(425, 4);
      this.stdIconBtnFind.MouseDownImage = (Image) null;
      this.stdIconBtnFind.Name = "stdIconBtnFind";
      this.stdIconBtnFind.Size = new Size(16, 17);
      this.stdIconBtnFind.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdIconBtnFind.TabIndex = 22;
      this.stdIconBtnFind.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnFind, "Find");
      this.stdIconBtnFind.Click += new EventHandler(this.findFieldbtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(446, 4);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 17);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 21;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.gcFields);
      this.Controls.Add((Control) this.gcVerification);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PiggybackSetupPanel);
      this.Size = new Size(552, 501);
      this.gcVerification.ResumeLayout(false);
      this.gcFields.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnFind).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
