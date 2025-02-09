// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanySettingForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class CompanySettingForm : Form, IOnlineHelpTarget
  {
    private const string className = "CompanySettingForm";
    private static readonly string sw = Tracing.SwCommon;
    private Sessions.Session session;
    private List<ExternalSettingValue> settingValues;
    private int selectedSettingTypeId = -1;
    private string settingTypeKey = string.Empty;
    private IContainer components;
    private ToolTip toolTip1;
    private GroupContainer grpSetting;
    private ComboBox cboStatusType;
    private GridView gvSettings;
    private StandardIconButton btnDeleteSetting;
    private StandardIconButton btnEditSetting;
    private StandardIconButton btnAddSetting;
    private StandardIconButton btnMoveSettingUp;
    private StandardIconButton btnMoveSettingDown;

    public CompanySettingForm()
    {
      this.InitializeComponent();
      this.btnDeleteSetting.Enabled = false;
      this.btnEditSetting.Enabled = false;
    }

    public CompanySettingForm(Sessions.Session session)
      : this(session, (string) null)
    {
    }

    public CompanySettingForm(Sessions.Session session, string userid)
    {
      this.session = session;
      this.InitializeComponent();
      this.btnDeleteSetting.Enabled = false;
      this.btnEditSetting.Enabled = false;
      this.init();
    }

    private void init()
    {
      Cursor.Current = Cursors.WaitCursor;
      List<ExternalSettingType> externalOrgSettingTypes = this.session.ConfigurationManager.GetExternalOrgSettingTypes();
      if (externalOrgSettingTypes != null && externalOrgSettingTypes.Count > 0)
      {
        this.cboStatusType.DataSource = (object) externalOrgSettingTypes;
        this.cboStatusType.DisplayMember = "settingTypeText";
        this.cboStatusType.ValueMember = "settingTypeId";
        this.cboStatusType.SelectedIndex = 0;
        this.settingTypeKey = externalOrgSettingTypes[0].settingTypeKey;
      }
      this.cboStatusType.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private void LoadSettingsByType(int settingTypeId)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.settingValues = this.session.ConfigurationManager.GetExternalOrgSettingsByType(settingTypeId);
      this.populateSettingValues();
      Cursor.Current = Cursors.Default;
    }

    private void populateSettingValues()
    {
      this.gvSettings.BeginUpdate();
      this.gvSettings.Items.Clear();
      if (this.settingValues != null && this.settingValues.Count > 0)
      {
        foreach (ExternalSettingValue settingValue in this.settingValues)
        {
          GVItem gvItem;
          if (this.settingTypeKey == "Group")
            gvItem = new GVItem(new string[2]
            {
              settingValue.settingCode,
              settingValue.settingValue
            });
          else
            gvItem = new GVItem(new string[2]
            {
              settingValue.settingValue,
              settingValue.settingCode
            });
          gvItem.Tag = (object) settingValue;
          this.gvSettings.Items.Add(gvItem);
        }
      }
      this.gvSettings.EndUpdate();
    }

    private void btnAddSetting_Click(object sender, EventArgs e)
    {
      using (AddNewSetting addNewSetting = new AddNewSetting(this.session, this.selectedSettingTypeId, this.settingTypeKey, this.settingValues))
      {
        if (addNewSetting.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.settingValues = addNewSetting.modifiedSettingValues;
        this.populateSettingValues();
      }
    }

    private void cboStatusType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectedSettingTypeId = ((ExternalSettingType) this.cboStatusType.SelectedItem).settingTypeId;
      this.settingTypeKey = ((ExternalSettingType) this.cboStatusType.SelectedItem).settingTypeKey;
      if (this.settingTypeKey == "Group")
      {
        this.gvSettings.Columns.Clear();
        this.gvSettings.Columns.Add(new GVColumn());
        this.gvSettings.Columns.Add(new GVColumn());
        this.gvSettings.Columns[0].Text = "Group Code";
        this.gvSettings.Columns[1].Text = "Group Name";
        this.gvSettings.Columns[0].Width = 200;
        this.gvSettings.Columns[1].Width = 200;
      }
      else
      {
        this.gvSettings.Columns.Clear();
        this.gvSettings.Columns.Add(new GVColumn());
        this.gvSettings.Columns[0].Text = this.settingTypeKey;
        this.gvSettings.Columns[0].Width = 300;
      }
      this.toolTip1.SetToolTip((Control) this.btnAddSetting, "Add " + this.settingTypeKey);
      this.toolTip1.SetToolTip((Control) this.btnEditSetting, "Edit " + this.settingTypeKey);
      this.toolTip1.SetToolTip((Control) this.btnDeleteSetting, "Delete " + this.settingTypeKey);
      this.LoadSettingsByType(this.selectedSettingTypeId);
    }

    private void btnEditSetting_Click(object sender, EventArgs e)
    {
      if (this.gvSettings.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select [SettingName] which you would like to edit.".Replace("[SettingName]", this.settingTypeKey));
      }
      else
      {
        using (AddNewSetting addNewSetting = new AddNewSetting(this.session, this.selectedSettingTypeId, this.settingTypeKey, this.settingValues, (ExternalSettingValue) this.gvSettings.SelectedItems[0].Tag))
        {
          if (addNewSetting.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.settingValues = addNewSetting.modifiedSettingValues;
          this.populateSettingValues();
        }
      }
    }

    private void btnDeleteSetting_Click(object sender, EventArgs e)
    {
      List<ExternalOriginatorManagementData> externalOrganizations = (List<ExternalOriginatorManagementData>) null;
      List<ExternalUserInfo> externalContacts = (List<ExternalUserInfo>) null;
      bool flag = false;
      if (this.gvSettings.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select [SettingName] which you would like to delete.");
      }
      else
      {
        int settingId = ((ExternalSettingValue) this.gvSettings.SelectedItems[0].Tag).settingId;
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this [SettingName]?".Replace("[SettingName]", this.settingTypeKey), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        if (this.selectedSettingTypeId == 1 || this.selectedSettingTypeId == 3 || this.selectedSettingTypeId == 5)
          externalOrganizations = this.session.ConfigurationManager.GetExternalOrganizationsBySettingId(settingId);
        else if (this.selectedSettingTypeId == 2)
          externalContacts = this.session.ConfigurationManager.GetExternalContactsBySettingId(settingId);
        else if (this.selectedSettingTypeId == 4)
          flag = this.session.ConfigurationManager.CheckIfAttachmentWithCategoryExists(settingId);
        if (flag)
        {
          if (Utils.Dialog((IWin32Window) this, "Removing a category will result in Attachments that have that category not being assigned to a category anymore. Do you still want to delete the category?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          this.session.ConfigurationManager.AssignNewSettingValueToAttachments(settingId);
          this.deleteSetting(settingId);
        }
        else if (this.selectedSettingTypeId == 6)
        {
          if (Utils.Dialog((IWin32Window) this, "If you delete this Document Category, documents assigned to it will be grouped under Other section in TPO WebCenter website, Documents page. Do you want to delete this Document Category?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          this.deleteSetting(settingId);
          this.session.ConfigurationManager.UpdateDocumentCategory(settingId, -1);
        }
        else if (externalOrganizations != null && externalOrganizations.Count > 0 || externalOrganizations != null && externalOrganizations.Count > 0)
        {
          using (SettingExceptionResolver exceptionResolver = new SettingExceptionResolver(this.session, this.selectedSettingTypeId, this.settingTypeKey, this.settingValues, (ExternalSettingValue) this.gvSettings.SelectedItems[0].Tag, externalOrganizations, externalContacts))
          {
            if (exceptionResolver.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.deleteSetting(settingId);
          }
        }
        else
          this.deleteSetting(settingId);
      }
    }

    private void deleteSetting(int settingIdToBeDeleted)
    {
      try
      {
        if (!this.session.ConfigurationManager.DeleteExternalOrgSettingValues(settingIdToBeDeleted.ToString()))
          return;
        this.LoadSettingsByType(this.selectedSettingTypeId);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message);
      }
      finally
      {
        this.gvSettings.EndUpdate();
      }
    }

    private void btnMoveSettingUp_Click(object sender, EventArgs e)
    {
      int num = 0;
      if (this.gvSettings.SelectedItems.Count > 0)
      {
        num = this.gvSettings.SelectedItems[0].Index - 1;
        this.moveSetting(this.gvSettings.SelectedItems[0].Index, this.gvSettings.SelectedItems[0].Index - 1);
        this.gvSettings.Items[num].Selected = true;
      }
      this.gvSettings.EnsureVisible(num);
    }

    private void btnMoveSettingDown_Click(object sender, EventArgs e)
    {
      int num = 0;
      if (this.gvSettings.SelectedItems.Count > 0)
      {
        num = this.gvSettings.SelectedItems[0].Index + 1;
        this.moveSetting(this.gvSettings.SelectedItems[0].Index, this.gvSettings.SelectedItems[0].Index + 1);
        this.gvSettings.Items[num].Selected = true;
      }
      this.gvSettings.EnsureVisible(num);
    }

    private void moveSetting(int fromIndex, int toIndex)
    {
      try
      {
        ExternalSettingValue tag1 = (ExternalSettingValue) this.gvSettings.Items[fromIndex].Tag;
        ExternalSettingValue tag2 = (ExternalSettingValue) this.gvSettings.Items[toIndex].Tag;
        if (!tag1.Equals((object) tag2))
          this.session.ConfigurationManager.ChangeSettingSortId(tag1, tag2);
        this.LoadSettingsByType(this.selectedSettingTypeId);
      }
      catch (Exception ex)
      {
        Tracing.Log(CompanySettingForm.sw, nameof (CompanySettingForm), TraceLevel.Error, "Error saving setting order: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while updating the setting sequence: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void gvSettings_SelectedIndexChanged(object sender, EventArgs e)
    {
      ExternalSettingValue externalSettingValue = (ExternalSettingValue) null;
      if (this.gvSettings.SelectedItems.Count > 0)
      {
        externalSettingValue = (ExternalSettingValue) this.gvSettings.SelectedItems[0].Tag;
        this.btnMoveSettingUp.Enabled = this.gvSettings.SelectedItems.Count == 1 && this.gvSettings.SelectedItems[0].Index > 0;
        this.btnMoveSettingDown.Enabled = this.gvSettings.SelectedItems.Count == 1 && this.gvSettings.SelectedItems[0].Index < this.gvSettings.Items.Count - 1;
        this.btnEditSetting.Enabled = this.gvSettings.SelectedItems.Count == 1;
        this.btnDeleteSetting.Enabled = true;
        this.btnEditSetting.Enabled = true;
      }
      else
      {
        this.btnMoveSettingUp.Enabled = this.btnMoveSettingDown.Enabled = this.btnEditSetting.Enabled = false;
        this.btnDeleteSetting.Enabled = false;
        this.btnEditSetting.Enabled = false;
      }
    }

    private void gvSettings_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }

    private void gvSettings_DragDrop(object sender, DragEventArgs e)
    {
      Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
      int index = ((GVItem) e.Data.GetData(typeof (GVItem))).Index;
      GVItem gvItem = this.gvSettings.Items[index];
      int rowIndex = this.gvSettings.HitTest(client).RowIndex;
      if (e.Effect != DragDropEffects.Move)
        return;
      if (rowIndex == index)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot move a setting to its own position");
      }
      else
      {
        if (rowIndex <= 0 || rowIndex >= this.gvSettings.Items.Count || index <= 0 || index >= this.gvSettings.Items.Count)
          return;
        this.moveSetting(index, rowIndex);
      }
    }

    private void gvSettings_ItemClick(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Settings";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn = new GVColumn();
      this.grpSetting = new GroupContainer();
      this.btnMoveSettingUp = new StandardIconButton();
      this.btnMoveSettingDown = new StandardIconButton();
      this.cboStatusType = new ComboBox();
      this.gvSettings = new GridView();
      this.btnDeleteSetting = new StandardIconButton();
      this.btnEditSetting = new StandardIconButton();
      this.btnAddSetting = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.grpSetting.SuspendLayout();
      ((ISupportInitialize) this.btnMoveSettingUp).BeginInit();
      ((ISupportInitialize) this.btnMoveSettingDown).BeginInit();
      ((ISupportInitialize) this.btnDeleteSetting).BeginInit();
      ((ISupportInitialize) this.btnEditSetting).BeginInit();
      ((ISupportInitialize) this.btnAddSetting).BeginInit();
      this.SuspendLayout();
      this.grpSetting.Controls.Add((Control) this.btnMoveSettingUp);
      this.grpSetting.Controls.Add((Control) this.btnMoveSettingDown);
      this.grpSetting.Controls.Add((Control) this.cboStatusType);
      this.grpSetting.Controls.Add((Control) this.gvSettings);
      this.grpSetting.Controls.Add((Control) this.btnDeleteSetting);
      this.grpSetting.Controls.Add((Control) this.btnEditSetting);
      this.grpSetting.Controls.Add((Control) this.btnAddSetting);
      this.grpSetting.Dock = DockStyle.Fill;
      this.grpSetting.HeaderForeColor = SystemColors.ControlText;
      this.grpSetting.Location = new Point(0, 0);
      this.grpSetting.Name = "grpSetting";
      this.grpSetting.Size = new Size(730, 564);
      this.grpSetting.TabIndex = 12;
      this.btnMoveSettingUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveSettingUp.BackColor = Color.Transparent;
      this.btnMoveSettingUp.Enabled = false;
      this.btnMoveSettingUp.Location = new Point(669, 3);
      this.btnMoveSettingUp.MouseDownImage = (Image) null;
      this.btnMoveSettingUp.Name = "btnMoveSettingUp";
      this.btnMoveSettingUp.Size = new Size(16, 17);
      this.btnMoveSettingUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveSettingUp.TabIndex = 37;
      this.btnMoveSettingUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveSettingUp, "Move Setting Up");
      this.btnMoveSettingUp.Click += new EventHandler(this.btnMoveSettingUp_Click);
      this.btnMoveSettingDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveSettingDown.BackColor = Color.Transparent;
      this.btnMoveSettingDown.Enabled = false;
      this.btnMoveSettingDown.Location = new Point(690, 3);
      this.btnMoveSettingDown.MouseDownImage = (Image) null;
      this.btnMoveSettingDown.Name = "btnMoveSettingDown";
      this.btnMoveSettingDown.Size = new Size(16, 17);
      this.btnMoveSettingDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveSettingDown.TabIndex = 36;
      this.btnMoveSettingDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveSettingDown, "Move Setting Down");
      this.btnMoveSettingDown.Click += new EventHandler(this.btnMoveSettingDown_Click);
      this.cboStatusType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStatusType.DropDownWidth = 175;
      this.cboStatusType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboStatusType.FormattingEnabled = true;
      this.cboStatusType.Location = new Point(4, 1);
      this.cboStatusType.Name = "cboStatusType";
      this.cboStatusType.Size = new Size(176, 22);
      this.cboStatusType.TabIndex = 13;
      this.cboStatusType.SelectedIndexChanged += new EventHandler(this.cboStatusType_SelectedIndexChanged);
      this.gvSettings.AllowMultiselect = false;
      this.gvSettings.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "ColumnSettingValue";
      gvColumn.SortMethod = GVSortMethod.None;
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Setting";
      gvColumn.Width = 728;
      this.gvSettings.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvSettings.Dock = DockStyle.Fill;
      this.gvSettings.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvSettings.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSettings.Location = new Point(1, 26);
      this.gvSettings.Margin = new Padding(0);
      this.gvSettings.MinColumnWidth = 300;
      this.gvSettings.Name = "gvSettings";
      this.gvSettings.Size = new Size(728, 537);
      this.gvSettings.SortOption = GVSortOption.None;
      this.gvSettings.TabIndex = 10;
      this.gvSettings.SelectedIndexChanged += new EventHandler(this.gvSettings_SelectedIndexChanged);
      this.gvSettings.ItemClick += new GVItemEventHandler(this.gvSettings_ItemClick);
      this.gvSettings.DragDrop += new DragEventHandler(this.gvSettings_DragDrop);
      this.gvSettings.DragEnter += new DragEventHandler(this.gvSettings_DragEnter);
      this.btnDeleteSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSetting.BackColor = Color.Transparent;
      this.btnDeleteSetting.Location = new Point(711, 3);
      this.btnDeleteSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnDeleteSetting.MouseDownImage = (Image) null;
      this.btnDeleteSetting.Name = "btnDeleteSetting";
      this.btnDeleteSetting.Size = new Size(16, 17);
      this.btnDeleteSetting.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSetting.TabIndex = 3;
      this.btnDeleteSetting.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteSetting, "Delete Setting");
      this.btnDeleteSetting.Click += new EventHandler(this.btnDeleteSetting_Click);
      this.btnEditSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditSetting.BackColor = Color.Transparent;
      this.btnEditSetting.Location = new Point(648, 2);
      this.btnEditSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnEditSetting.MouseDownImage = (Image) null;
      this.btnEditSetting.Name = "btnEditSetting";
      this.btnEditSetting.Size = new Size(16, 18);
      this.btnEditSetting.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditSetting.TabIndex = 2;
      this.btnEditSetting.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditSetting, "Edit Setting");
      this.btnEditSetting.Click += new EventHandler(this.btnEditSetting_Click);
      this.btnAddSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddSetting.BackColor = Color.Transparent;
      this.btnAddSetting.Location = new Point(628, 3);
      this.btnAddSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnAddSetting.MouseDownImage = (Image) null;
      this.btnAddSetting.Name = "btnAddSetting";
      this.btnAddSetting.Size = new Size(16, 17);
      this.btnAddSetting.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSetting.TabIndex = 1;
      this.btnAddSetting.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddSetting, "Add Setting");
      this.btnAddSetting.Click += new EventHandler(this.btnAddSetting_Click);
      this.AutoScaleDimensions = new SizeF(5f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(730, 564);
      this.ControlBox = false;
      this.Controls.Add((Control) this.grpSetting);
      this.Font = new Font("Arial Narrow", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(2, 3, 2, 3);
      this.Name = nameof (CompanySettingForm);
      this.Text = nameof (CompanySettingForm);
      this.grpSetting.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveSettingUp).EndInit();
      ((ISupportInitialize) this.btnMoveSettingDown).EndInit();
      ((ISupportInitialize) this.btnDeleteSetting).EndInit();
      ((ISupportInitialize) this.btnEditSetting).EndInit();
      ((ISupportInitialize) this.btnAddSetting).EndInit();
      this.ResumeLayout(false);
    }
  }
}
