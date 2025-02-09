// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ePassServices
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class ePassServices : Form
  {
    private ServicesAclManager serviceMgr;
    private int personaID;
    private AclFeature feature;
    private bool initial = true;
    private bool dirty;
    private ArrayList previousView;
    private int selectOption = 2;
    private string userID = "";
    private Persona[] personaList;
    private bool isPersonal;
    private bool readOnly;
    private Sessions.Session session;
    private ServiceAclInfo.ServicesDefaultSetting previousSettings = ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
    private IContainer components;
    private ListView lsvCategory;
    private ColumnHeader columnHeader1;
    private Button btnOk;
    private Button btnCancel;
    private ImageList imgListTv;
    private ContextMenu contMenu;
    private MenuItem miLinkTo;
    private MenuItem miDisconTo;
    private Label lblDisconnected;
    private Label lblLinked;
    private RadioButton rdbAll;
    private RadioButton rdbCustom;
    private RadioButton rdbNone;
    private Label lblDefaultSetting;
    private ContextMenu contMenuDefault;
    private MenuItem miDefaultLinkTo;
    private MenuItem miDefaultDisconFrom;

    public ePassServices(
      Sessions.Session session,
      AclFeature feature,
      int personaId,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.serviceMgr = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      this.previousView = new ArrayList();
      this.feature = feature;
      this.personaID = personaId;
      this.selectOption = option;
      this.loadCategoryForPersona();
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly, false);
      if (!this.readOnly)
      {
        if (this.selectOption == 1)
        {
          this.selectAll(false);
          this.rdbCustom.Checked = true;
        }
        else
        {
          if (this.selectOption != 0)
            return;
          this.deselectAll(false);
          this.rdbCustom.Checked = true;
        }
      }
      else
        this.setView();
    }

    public ePassServices(
      Sessions.Session session,
      AclFeature feature,
      string userID,
      Persona[] personaList,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.userID = userID;
      this.personaList = personaList;
      this.isPersonal = true;
      this.lblDisconnected.Visible = true;
      this.lblLinked.Visible = true;
      this.readOnly = readOnly;
      this.feature = feature;
      this.previousView = new ArrayList();
      this.selectOption = option;
      this.lsvCategory.SmallImageList = this.imgListTv;
      this.lsvCategory.ContextMenu = this.contMenu;
      this.MakeReadOnly(this.readOnly, false);
      this.serviceMgr = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      this.loadCategoryForUser();
      if (!this.readOnly)
      {
        if (this.selectOption == 1)
        {
          this.selectAll(true);
          this.rdbCustom.Checked = true;
        }
        else if (this.selectOption == 0)
        {
          this.deselectAll(true);
          this.rdbCustom.Checked = true;
        }
      }
      if (this.selectOption != 2)
        return;
      this.setView();
    }

    public bool IsReadOnly
    {
      set => this.MakeReadOnly(value, false);
    }

    private void MakeReadOnly(bool makeReadOnly, bool makeCategoryReadOnly)
    {
      if (makeReadOnly)
      {
        this.btnOk.Enabled = false;
        this.rdbAll.Enabled = false;
        this.rdbCustom.Enabled = false;
        this.rdbNone.Enabled = false;
        this.lblDefaultSetting.ContextMenu = (ContextMenu) null;
      }
      else
      {
        this.btnOk.Enabled = true;
        this.lsvCategory.ContextMenu = this.contMenu;
        this.lblDefaultSetting.ContextMenu = this.contMenuDefault;
        this.rdbAll.Enabled = true;
        this.rdbCustom.Enabled = true;
        this.rdbNone.Enabled = true;
      }
      if (!makeCategoryReadOnly)
        return;
      this.lsvCategory.ContextMenu = (ContextMenu) null;
    }

    private void selectAll(bool custom)
    {
      this.initial = false;
      foreach (ListViewItem listViewItem in this.lsvCategory.Items)
      {
        listViewItem.Checked = true;
        if (custom)
        {
          ((ServiceAclInfo) listViewItem.Tag).CustomAccess = AclResourceAccess.ReadWrite;
          listViewItem.ImageIndex = 0;
        }
        else
          ((ServiceAclInfo) listViewItem.Tag).PersonaAccess = AclResourceAccess.ReadWrite;
      }
      this.dirty = true;
    }

    private void deselectAll(bool custom)
    {
      this.initial = false;
      foreach (ListViewItem listViewItem in this.lsvCategory.Items)
      {
        listViewItem.Checked = false;
        if (custom)
        {
          ((ServiceAclInfo) listViewItem.Tag).CustomAccess = AclResourceAccess.ReadOnly;
          listViewItem.ImageIndex = 0;
        }
        else
          ((ServiceAclInfo) listViewItem.Tag).PersonaAccess = AclResourceAccess.ReadOnly;
      }
      this.dirty = true;
    }

    private void setView()
    {
      this.previousView = new ArrayList();
      foreach (ListViewItem listViewItem in this.lsvCategory.Items)
        this.previousView.Add(listViewItem.Clone());
    }

    private void loadCategoryForPersona()
    {
      this.initial = true;
      this.dirty = false;
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = this.serviceMgr.GetServicesDefaultSetting(this.feature, this.personaID);
      this.previousSettings = servicesDefaultSetting;
      ServiceAclInfo[] permissions = this.serviceMgr.GetPermissions(this.feature, this.personaID);
      this.lsvCategory.Items.Clear();
      foreach (ServiceAclInfo serviceAclInfo in permissions)
        this.lsvCategory.Items.Add(new ListViewItem(serviceAclInfo.ServiceTitle)
        {
          Tag = (object) serviceAclInfo,
          Checked = serviceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite
        });
      switch (servicesDefaultSetting)
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          foreach (ListViewItem listViewItem in this.lsvCategory.Items)
          {
            listViewItem.Checked = false;
            listViewItem.ForeColor = Color.Gray;
          }
          this.rdbNone.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.Custom:
          foreach (ListViewItem listViewItem in this.lsvCategory.Items)
            listViewItem.ForeColor = Color.Black;
          this.rdbCustom.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          foreach (ListViewItem listViewItem in this.lsvCategory.Items)
          {
            listViewItem.Checked = true;
            listViewItem.ForeColor = Color.Gray;
          }
          this.rdbAll.Checked = true;
          break;
      }
      this.setView();
    }

    private void loadCategoryForUser()
    {
      this.initial = true;
      this.dirty = false;
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = this.serviceMgr.GetUserSpecificServicesDefaultSetting(this.feature, this.userID, this.personaList);
      ServiceAclInfo[] permissions = this.serviceMgr.GetPermissions(this.feature, this.userID, this.personaList);
      this.lsvCategory.Items.Clear();
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        ListViewItem listViewItem = new ListViewItem(serviceAclInfo.ServiceTitle);
        listViewItem.Tag = (object) serviceAclInfo;
        if (serviceAclInfo.CustomAccess != AclResourceAccess.None)
        {
          listViewItem.Checked = serviceAclInfo.CustomAccess == AclResourceAccess.ReadWrite;
          listViewItem.ImageIndex = 0;
        }
        else
        {
          listViewItem.Checked = serviceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite;
          listViewItem.ImageIndex = 1;
        }
        this.lsvCategory.Items.Add(listViewItem);
      }
      if (servicesDefaultSetting == ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
      {
        this.lblDefaultSetting.ImageIndex = 1;
        servicesDefaultSetting = this.serviceMgr.GetServicesDefaultSetting(this.feature, this.userID, this.personaList);
      }
      else
        this.lblDefaultSetting.ImageIndex = 0;
      switch (servicesDefaultSetting)
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          this.lsvCategory.ContextMenu = (ContextMenu) null;
          this.rdbNone.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.Custom:
          this.rdbCustom.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          this.lsvCategory.ContextMenu = (ContextMenu) null;
          this.rdbAll.Checked = true;
          break;
      }
      this.setView();
    }

    private void lsvCategory_Click(object sender, EventArgs e) => this.initial = false;

    public int GetImageIndex()
    {
      int imageIndex = 0;
      if (this.lblDefaultSetting.ImageIndex == 0)
      {
        imageIndex = 1;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lsvCategory.Items)
        {
          if (listViewItem.ImageIndex == 0)
          {
            imageIndex = 1;
            break;
          }
        }
      }
      return imageIndex;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.rdbAll.Checked)
        this.previousSettings = ServiceAclInfo.ServicesDefaultSetting.All;
      else if (this.rdbCustom.Checked)
        this.previousSettings = ServiceAclInfo.ServicesDefaultSetting.Custom;
      else if (this.rdbNone.Checked)
        this.previousSettings = ServiceAclInfo.ServicesDefaultSetting.None;
      this.DialogResult = DialogResult.OK;
      this.setView();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.initial = true;
      this.lsvCategory.Items.Clear();
      if (this.previousSettings != ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
      {
        switch (this.previousSettings)
        {
          case ServiceAclInfo.ServicesDefaultSetting.None:
            this.rdbNone.Checked = true;
            break;
          case ServiceAclInfo.ServicesDefaultSetting.Custom:
            this.rdbCustom.Checked = true;
            break;
          case ServiceAclInfo.ServicesDefaultSetting.All:
            this.rdbAll.Checked = true;
            break;
        }
      }
      foreach (ListViewItem listViewItem in this.previousView)
        this.lsvCategory.Items.Add((ListViewItem) listViewItem.Clone());
      this.DialogResult = DialogResult.Cancel;
    }

    public bool HasBeenModified => this.dirty;

    public void SaveData()
    {
      List<ServiceAclInfo> serviceAclInfoList = new List<ServiceAclInfo>();
      foreach (object obj in this.lsvCategory.Items)
        serviceAclInfoList.Add((ServiceAclInfo) ((ListViewItem) obj).Tag);
      ServiceAclInfo.ServicesDefaultSetting defaultValue = ServiceAclInfo.ServicesDefaultSetting.All;
      if (this.rdbCustom.Checked)
        defaultValue = ServiceAclInfo.ServicesDefaultSetting.Custom;
      else if (this.rdbNone.Checked)
        defaultValue = ServiceAclInfo.ServicesDefaultSetting.None;
      if (!this.isPersonal)
      {
        this.serviceMgr.SetPermissions(this.feature, serviceAclInfoList.ToArray(), this.personaID);
        this.serviceMgr.SetDefaultValue(this.feature, this.personaID, defaultValue);
      }
      else
      {
        if (this.lblDefaultSetting.ImageIndex == 1)
          defaultValue = ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
        this.serviceMgr.SetPermissions(this.feature, serviceAclInfoList.ToArray(), this.userID);
        this.serviceMgr.SetDefaultValue(this.feature, this.userID, defaultValue);
      }
      this.dirty = false;
    }

    public bool HasSomethingChecked()
    {
      bool flag = false;
      foreach (ListViewItem listViewItem in this.lsvCategory.Items)
      {
        if (listViewItem.Checked)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void lsvCategory_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (this.initial || this.readOnly)
        return;
      this.dirty = true;
      ServiceAclInfo tag = (ServiceAclInfo) e.Item.Tag;
      if (!this.isPersonal)
      {
        tag.PersonaAccess = !e.Item.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
      }
      else
      {
        tag.CustomAccess = !e.Item.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
        e.Item.ImageIndex = 0;
      }
      e.Item.Tag = (object) tag;
    }

    private void miLinkTo_Click(object sender, EventArgs e)
    {
      ListViewItem selectedItem = this.lsvCategory.SelectedItems[0];
      this.initial = true;
      if (selectedItem != null && selectedItem.ImageIndex < 1)
      {
        selectedItem.ImageIndex = 1;
        ServiceAclInfo tag = (ServiceAclInfo) selectedItem.Tag;
        if (tag != null)
          selectedItem.Checked = tag.PersonaAccess == AclResourceAccess.ReadWrite;
        tag.CustomAccess = AclResourceAccess.None;
        selectedItem.Tag = (object) tag;
        this.dirty = true;
      }
      this.initial = false;
    }

    private void miDisconTo_Click(object sender, EventArgs e)
    {
      ListViewItem selectedItem = this.lsvCategory.SelectedItems[0];
      this.initial = true;
      if (selectedItem != null && selectedItem.ImageIndex == 1)
      {
        selectedItem.ImageIndex = 0;
        ServiceAclInfo tag = (ServiceAclInfo) selectedItem.Tag;
        if (tag != null)
          tag.CustomAccess = !selectedItem.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
        selectedItem.Tag = (object) tag;
        this.dirty = true;
      }
      this.initial = false;
    }

    private void lsvCategory_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.initial)
        return;
      if (this.readOnly)
      {
        this.dirty = false;
        e.NewValue = e.CurrentValue;
      }
      else if (this.rdbAll.Checked)
      {
        e.NewValue = CheckState.Checked;
      }
      else
      {
        if (!this.rdbNone.Checked)
          return;
        e.NewValue = CheckState.Unchecked;
      }
    }

    private void rdb_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initial)
        return;
      this.dirty = true;
      if (this.rdbAll.Checked)
      {
        foreach (ListViewItem listViewItem in this.lsvCategory.Items)
        {
          listViewItem.Checked = true;
          listViewItem.ForeColor = Color.Gray;
        }
        this.lsvCategory.ContextMenu = (ContextMenu) null;
      }
      else if (this.rdbCustom.Checked)
      {
        foreach (ListViewItem listViewItem in this.lsvCategory.Items)
          listViewItem.ForeColor = Color.Black;
        if (this.isPersonal)
          this.lsvCategory.ContextMenu = this.contMenu;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lsvCategory.Items)
        {
          listViewItem.Checked = false;
          listViewItem.ForeColor = Color.Gray;
        }
        this.lsvCategory.ContextMenu = (ContextMenu) null;
      }
      if (!this.isPersonal)
        return;
      this.lblDefaultSetting.ImageIndex = 0;
    }

    private void rdb_MouseEnter(object sender, EventArgs e) => this.initial = false;

    private void miDefaultDisconFrom_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      this.lblDefaultSetting.ImageIndex = 0;
    }

    private void miDefaultLinkTo_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      this.initial = true;
      switch (this.serviceMgr.GetPersonasServicesDefaultSetting(this.feature, this.userID, this.personaList))
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          this.rdbNone.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.Custom:
          this.rdbCustom.Checked = true;
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          this.rdbAll.Checked = true;
          break;
      }
      foreach (ListViewItem listViewItem in this.lsvCategory.Items)
      {
        listViewItem.ImageIndex = 1;
        ServiceAclInfo tag = (ServiceAclInfo) listViewItem.Tag;
        if (tag != null)
          listViewItem.Checked = tag.PersonaAccess == AclResourceAccess.ReadWrite;
        tag.CustomAccess = AclResourceAccess.None;
        listViewItem.Tag = (object) tag;
      }
      this.lblDefaultSetting.ImageIndex = 1;
      this.initial = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ePassServices));
      this.lsvCategory = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.imgListTv = new ImageList(this.components);
      this.contMenu = new ContextMenu();
      this.miLinkTo = new MenuItem();
      this.miDisconTo = new MenuItem();
      this.lblDisconnected = new Label();
      this.lblLinked = new Label();
      this.rdbAll = new RadioButton();
      this.rdbCustom = new RadioButton();
      this.rdbNone = new RadioButton();
      this.lblDefaultSetting = new Label();
      this.contMenuDefault = new ContextMenu();
      this.miDefaultLinkTo = new MenuItem();
      this.miDefaultDisconFrom = new MenuItem();
      this.SuspendLayout();
      this.lsvCategory.CheckBoxes = true;
      this.lsvCategory.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lsvCategory.FullRowSelect = true;
      this.lsvCategory.GridLines = true;
      this.lsvCategory.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lsvCategory.Location = new Point(12, 48);
      this.lsvCategory.MultiSelect = false;
      this.lsvCategory.Name = "lsvCategory";
      this.lsvCategory.Size = new Size(271, 227);
      this.lsvCategory.Sorting = SortOrder.Ascending;
      this.lsvCategory.TabIndex = 0;
      this.lsvCategory.UseCompatibleStateImageBehavior = false;
      this.lsvCategory.View = View.Details;
      this.lsvCategory.ItemChecked += new ItemCheckedEventHandler(this.lsvCategory_ItemChecked);
      this.lsvCategory.ItemCheck += new ItemCheckEventHandler(this.lsvCategory_ItemCheck);
      this.lsvCategory.Click += new EventHandler(this.lsvCategory_Click);
      this.columnHeader1.Text = "Categories";
      this.columnHeader1.Width = 224;
      this.btnOk.Location = new Point((int) sbyte.MaxValue, 310);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(208, 310);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.contMenu.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinkTo,
        this.miDisconTo
      });
      this.miLinkTo.Index = 0;
      this.miLinkTo.Text = "Link with Persona Rights";
      this.miLinkTo.Click += new EventHandler(this.miLinkTo_Click);
      this.miDisconTo.Index = 1;
      this.miDisconTo.Text = "Disconnect from Persona Rights";
      this.miDisconTo.Click += new EventHandler(this.miDisconTo_Click);
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 0;
      this.lblDisconnected.ImageList = this.imgListTv;
      this.lblDisconnected.Location = new Point(9, 291);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(197, 16);
      this.lblDisconnected.TabIndex = 8;
      this.lblDisconnected.Text = "      Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 1;
      this.lblLinked.ImageList = this.imgListTv;
      this.lblLinked.Location = new Point(9, 275);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(197, 16);
      this.lblLinked.TabIndex = 7;
      this.lblLinked.Text = "      Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.rdbAll.AutoSize = true;
      this.rdbAll.Location = new Point(11, 25);
      this.rdbAll.Name = "rdbAll";
      this.rdbAll.Size = new Size(36, 17);
      this.rdbAll.TabIndex = 9;
      this.rdbAll.TabStop = true;
      this.rdbAll.Text = "All";
      this.rdbAll.UseVisualStyleBackColor = true;
      this.rdbAll.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.rdbCustom.AutoSize = true;
      this.rdbCustom.Location = new Point(54, 25);
      this.rdbCustom.Name = "rdbCustom";
      this.rdbCustom.Size = new Size(60, 17);
      this.rdbCustom.TabIndex = 10;
      this.rdbCustom.TabStop = true;
      this.rdbCustom.Text = "Custom";
      this.rdbCustom.UseVisualStyleBackColor = true;
      this.rdbCustom.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.rdbCustom.CheckedChanged += new EventHandler(this.rdb_CheckedChanged);
      this.rdbNone.AutoSize = true;
      this.rdbNone.Location = new Point(120, 25);
      this.rdbNone.Name = "rdbNone";
      this.rdbNone.Size = new Size(51, 17);
      this.rdbNone.TabIndex = 11;
      this.rdbNone.TabStop = true;
      this.rdbNone.Text = "None";
      this.rdbNone.UseVisualStyleBackColor = true;
      this.rdbNone.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.rdbNone.CheckedChanged += new EventHandler(this.rdb_CheckedChanged);
      this.lblDefaultSetting.AutoSize = true;
      this.lblDefaultSetting.ImageAlign = ContentAlignment.MiddleRight;
      this.lblDefaultSetting.ImageList = this.imgListTv;
      this.lblDefaultSetting.Location = new Point(9, 9);
      this.lblDefaultSetting.Name = "lblDefaultSetting";
      this.lblDefaultSetting.Size = new Size(101, 13);
      this.lblDefaultSetting.TabIndex = 12;
      this.lblDefaultSetting.Text = "Default Setting        ";
      this.contMenuDefault.MenuItems.AddRange(new MenuItem[2]
      {
        this.miDefaultLinkTo,
        this.miDefaultDisconFrom
      });
      this.miDefaultLinkTo.Index = 0;
      this.miDefaultLinkTo.Text = "Link with Persona Rights";
      this.miDefaultLinkTo.Click += new EventHandler(this.miDefaultLinkTo_Click);
      this.miDefaultDisconFrom.Index = 1;
      this.miDefaultDisconFrom.Text = "Disconnect from Persona Rights";
      this.miDefaultDisconFrom.Click += new EventHandler(this.miDefaultDisconFrom_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(296, 340);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDefaultSetting);
      this.Controls.Add((Control) this.rdbNone);
      this.Controls.Add((Control) this.rdbCustom);
      this.Controls.Add((Control) this.rdbAll);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.lsvCategory);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ePassServices);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ICE Mortgage Technology Network Service Categories";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
