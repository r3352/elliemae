// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ePass.ePassPasswordMgr
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup.ePass
{
  public class ePassPasswordMgr : UserControl
  {
    private const int SERVICEC_CATEGORY_COLUMN_INDEX = 0;
    private const int PROVIDER_NAME_COLUMN_INDEX = 1;
    private const int DESCRIPTION_COLUMN_INDEX = 2;
    private const int USERS_COLUMN_INDEX = 3;
    private GridViewFilterManager filterMnger;
    private IContainer components;
    private GridView gvSettingsList;
    private GroupContainer groupContainer1;
    private StandardIconButton btnEdit;
    private StandardIconButton btnAdd;
    private StandardIconButton btnDelete;
    private ToolTip toolTip1;

    public ePassPasswordMgr()
    {
      this.InitializeComponent();
      this.filterMnger = new GridViewFilterManager(Session.DefaultInstance, this.gvSettingsList, false);
      ComboBox columnFilter = (ComboBox) this.filterMnger.CreateColumnFilter(0, GridViewFilterControlType.DropdownList);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassai/getallservices.asp");
      WebResponse webResponse = (WebResponse) null;
      columnFilter.Items.Add((object) "");
      try
      {
        webResponse = httpWebRequest.GetResponse();
        Stream responseStream = webResponse.GetResponseStream();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(responseStream);
        foreach (XmlNode selectNode in xmlDocument.SelectNodes("SERVICES/S/@Title"))
          columnFilter.Items.Add((object) selectNode.InnerText);
      }
      catch
      {
        webResponse.Close();
      }
      this.filterMnger.CreateColumnFilter(1, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(2, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(3, GridViewFilterControlType.Integer);
      this.filterMnger.FilterChanged += new EventHandler(this.filterMnger_FilterChanged);
      this.initialPageValue();
    }

    private void filterMnger_FilterChanged(object sender, EventArgs e) => this.reloadGridView();

    private void initialPageValue()
    {
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Password Setting.");
      this.toolTip1.SetToolTip((Control) this.btnAdd, "New Password Setting.");
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Password Setting.");
      this.reloadGridView();
      this.gvSettingsList.Sort(0, SortOrder.Descending);
    }

    private void reloadGridView()
    {
      this.gvSettingsList.Items.Clear();
      foreach (ePassCredentialSetting credentialSetting in Session.ConfigurationManager.GetAllePassCredentialSettings())
      {
        GVItem gvItem = new GVItem(credentialSetting.Category);
        gvItem.SubItems[1].Text = credentialSetting.Title;
        gvItem.SubItems[2].Text = credentialSetting.Description;
        gvItem.SubItems[3].Text = string.Concat((object) Session.ConfigurationManager.GetUserIDListByePassCredentialID(credentialSetting.CredentialID).Count);
        gvItem.Tag = (object) credentialSetting;
        string valueDescription1 = this.filterMnger.GetFilterForColumn(0) == null ? "" : this.filterMnger.GetFilterForColumn(0).ValueDescription;
        if (!(valueDescription1 != "") || !(credentialSetting.Category != valueDescription1))
        {
          string valueDescription2 = this.filterMnger.GetFilterForColumn(1) == null ? "" : this.filterMnger.GetFilterForColumn(1).ValueDescription;
          if (!(valueDescription2 != "") || credentialSetting.Title.ToLower().Contains(valueDescription2.ToLower()))
          {
            string valueDescription3 = this.filterMnger.GetFilterForColumn(2) == null ? "" : this.filterMnger.GetFilterForColumn(2).ValueDescription;
            if (!(valueDescription3 != "") || credentialSetting.Description.ToLower().Contains(valueDescription3.ToLower()))
            {
              string valueDescription4 = this.filterMnger.GetFilterForColumn(3) == null ? "" : this.filterMnger.GetFilterForColumn(3).ValueDescription;
              if (valueDescription4 != "")
              {
                switch (this.filterMnger.GetFilterForColumn(3).OperatorType)
                {
                  case OperatorTypes.Equals:
                    if (!(valueDescription4 != gvItem.SubItems[3].Text))
                      break;
                    continue;
                  case OperatorTypes.NotEqual:
                    if (!(valueDescription4 == gvItem.SubItems[3].Text))
                      break;
                    continue;
                  case OperatorTypes.GreaterThan:
                    int num1 = int.Parse(valueDescription4);
                    if (Utils.ParseInt((object) gvItem.SubItems[3].Text, 0) > num1)
                      break;
                    continue;
                  case OperatorTypes.NotGreaterThan:
                    int num2 = int.Parse(valueDescription4);
                    if (Utils.ParseInt((object) gvItem.SubItems[3].Text, 0) > num2)
                      continue;
                    break;
                  case OperatorTypes.LessThan:
                    int num3 = int.Parse(valueDescription4);
                    if (Utils.ParseInt((object) gvItem.SubItems[3].Text, 0) < num3)
                      break;
                    continue;
                  case OperatorTypes.NotLessThan:
                    int num4 = int.Parse(valueDescription4);
                    if (Utils.ParseInt((object) gvItem.SubItems[3].Text, 0) >= num4)
                      break;
                    continue;
                }
              }
              this.gvSettingsList.Items.Add(gvItem);
            }
          }
        }
      }
      this.groupContainer1.Text = "Accounts (" + (object) this.gvSettingsList.Items.Count + ")";
      this.gvSettingsList.ReSort();
    }

    private void gvSettingsList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      ePassCredentialSetting tag = (ePassCredentialSetting) e.Item.Tag;
      ePassCredentialDetail credentialDetail = new ePassCredentialDetail(tag);
      if (DialogResult.OK != credentialDetail.ShowDialog((IWin32Window) this))
        return;
      Session.ConfigurationManager.UpdateePassCredentialSetting(credentialDetail.UpdatedSetting);
      Session.ConfigurationManager.UpdateePassCredentialUserList(tag.CredentialID, tag.Title, credentialDetail.SelectedUserIDs);
      this.reloadGridView();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      ePassCredentialSetting tag = (ePassCredentialSetting) this.gvSettingsList.SelectedItems[0].Tag;
      ePassCredentialDetail credentialDetail = new ePassCredentialDetail(tag);
      if (DialogResult.OK != credentialDetail.ShowDialog((IWin32Window) this))
        return;
      Session.ConfigurationManager.UpdateePassCredentialSetting(credentialDetail.UpdatedSetting);
      Session.ConfigurationManager.UpdateePassCredentialUserList(tag.CredentialID, tag.Title, credentialDetail.SelectedUserIDs);
      this.reloadGridView();
    }

    private void gvSettingsList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvSettingsList.SelectedItems.Count > 0)
      {
        this.btnDelete.Enabled = true;
        if (this.gvSettingsList.SelectedItems.Count > 1)
          this.btnEdit.Enabled = false;
        else
          this.btnEdit.Enabled = true;
      }
      else
        this.btnEdit.Enabled = this.btnDelete.Enabled = false;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      ePassCredentialDetail credentialDetail = new ePassCredentialDetail();
      if (DialogResult.OK != credentialDetail.ShowDialog((IWin32Window) this))
        return;
      ePassCredentialSetting credentialSetting = Session.ConfigurationManager.UpdateePassCredentialSetting(credentialDetail.UpdatedSetting);
      Session.ConfigurationManager.UpdateePassCredentialUserList(credentialSetting.CredentialID, credentialSetting.Title, credentialDetail.SelectedUserIDs);
      this.reloadGridView();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvSettingsList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a password setting to delete.");
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this password setting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        foreach (GVItem selectedItem in this.gvSettingsList.SelectedItems)
          Session.ConfigurationManager.DeleteePassCredentialSetting(((ePassCredentialSetting) selectedItem.Tag).CredentialID);
        this.reloadGridView();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvSettingsList = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.btnDelete);
      this.groupContainer1.Controls.Add((Control) this.btnEdit);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.gvSettingsList);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(720, 499);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Accounts";
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(700, 4);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 103;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(678, 4);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 102;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(656, 4);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 101;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvSettingsList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "Service Category";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Provider Name";
      gvColumn2.Width = 180;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column4";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Users";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Width = 50;
      this.gvSettingsList.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvSettingsList.Dock = DockStyle.Fill;
      this.gvSettingsList.FilterVisible = true;
      this.gvSettingsList.Location = new Point(1, 26);
      this.gvSettingsList.Name = "gvSettingsList";
      this.gvSettingsList.Size = new Size(718, 472);
      this.gvSettingsList.TabIndex = 100;
      this.gvSettingsList.SelectedIndexChanged += new EventHandler(this.gvSettingsList_SelectedIndexChanged);
      this.gvSettingsList.ItemDoubleClick += new GVItemEventHandler(this.gvSettingsList_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (ePassPasswordMgr);
      this.Size = new Size(720, 499);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
