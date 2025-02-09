// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.IRS4506TSettingsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class IRS4506TSettingsForm : SettingsUserControl
  {
    private Sessions.Session session;
    private FrmBrowserHandler browserHandler;
    private IRS4506TFields currentIRS4506TFields;
    private int currentSelectedIndex = -1;
    private bool isDirty;
    private IContainer components;
    private GroupContainer grpListContainer;
    private GridView gvIRS4506TList;
    private StandardIconButton btnNew;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private GroupContainer grpTemplateName;
    private Label label42;
    private TextBox txtTemplateName;
    private Label label41;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private ComboBox cboLayout;
    private SplitContainer splitContainerAll;
    private Panel panelInputScreen;
    private AxWebBrowser webBrowser;
    private StandardIconButton btnDuplicate;

    public IRS4506TSettingsForm(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initForm();
    }

    private void initForm()
    {
      List<IRS4506TTemplate> irS4506Ttemplates = this.session.ConfigurationManager.GetIRS4506TTemplates(true);
      for (int index = 0; index < irS4506Ttemplates.Count; ++index)
        this.addGVItem(irS4506Ttemplates[index].RequestVersion, irS4506Ttemplates[index].TemplateName, irS4506Ttemplates[index].RequestYears, irS4506Ttemplates[index].LastModifiedBy, irS4506Ttemplates[index].LastModifiedDateTime, irS4506Ttemplates[index].TemplateID, false);
      if (this.gvIRS4506TList.Items.Count > 0)
      {
        this.gvIRS4506TList.Items[0].Selected = true;
        this.Template_SelectedIndexChanged((object) null, (EventArgs) null);
      }
      else
        this.btnDelete.Enabled = this.btnDuplicate.Enabled = this.btnEdit.Enabled = this.grpTemplateName.Visible = this.panelInputScreen.Visible = false;
    }

    private void gvHMDAProfile_DoubleClick(object sender, EventArgs e)
    {
      this.btnEdit_Click(sender, e);
    }

    private void Template_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.btnSave.Enabled && this.isDirty && Utils.Dialog((IWin32Window) this, "Do you want to save the change? Otherwise, the change will be discarded!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      if (this.gvIRS4506TList.SelectedItems.Count == 1)
      {
        IRS4506TTemplate irS4506Ttemplate = this.session.ConfigurationManager.GetIRS4506TTemplate((int) this.gvIRS4506TList.SelectedItems[0].Tag);
        if (irS4506Ttemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The template '" + this.gvIRS4506TList.SelectedItems[0].SubItems[1].Text + "' is currently unavailable for use. Please check the template settings to ensure that it is still available.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.grpTemplateName.Visible = false;
          this.panelInputScreen.Visible = false;
          this.currentSelectedIndex = -1;
        }
        else
        {
          this.currentIRS4506TFields = irS4506Ttemplate.GetTemplateData();
          this.currentSelectedIndex = this.gvIRS4506TList.SelectedItems[0].Index;
          this.loadInputScreen();
        }
      }
      else
      {
        this.grpTemplateName.Visible = false;
        this.panelInputScreen.Visible = false;
        this.currentSelectedIndex = -1;
      }
      this.setButtonStatus(false);
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvIRS4506TList.SelectedItems.Count != 1)
        return;
      if (this.session.ConfigurationManager.GetIRS4506TTemplate((int) this.gvIRS4506TList.SelectedItems[0].Tag) == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The template '" + this.gvIRS4506TList.SelectedItems[0].SubItems[1].Text + "' is currently unavailable for use. Please check the template settings to ensure that it is still available.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.currentIRS4506TFields.InEditMode = true;
        if (this.currentSelectedIndex == -1)
          this.currentSelectedIndex = this.gvIRS4506TList.SelectedItems[0].Index;
        this.loadInputScreen();
        this.setButtonStatus(true);
        this.txtTemplateName.Focus();
        this.isDirty = false;
      }
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (this.currentIRS4506TFields.IsNew)
      {
        if (this.gvIRS4506TList.Items.Count > 0)
        {
          this.gvIRS4506TList.Items[0].Selected = true;
        }
        else
        {
          this.grpTemplateName.Visible = false;
          this.panelInputScreen.Visible = false;
          this.currentSelectedIndex = -1;
          this.setButtonStatus(false);
        }
      }
      else
      {
        IRS4506TTemplate irS4506Ttemplate = this.session.ConfigurationManager.GetIRS4506TTemplate((int) this.gvIRS4506TList.SelectedItems[0].Tag);
        if (irS4506Ttemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This template is not available anymore! The list will be refreshed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.gvIRS4506TList.Items.Clear();
          this.initForm();
        }
        else
        {
          this.currentIRS4506TFields = irS4506Ttemplate.GetTemplateData();
          this.currentIRS4506TFields.InEditMode = false;
          this.loadInputScreen();
          this.setButtonStatus(false);
          this.isDirty = false;
        }
      }
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (this.btnSave.Enabled && this.isDirty)
      {
        if (Utils.Dialog((IWin32Window) this, "Do you want to save the change? Otherwise, the change will be discarded!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
          this.btnSave_Click((object) null, (EventArgs) null);
        else
          this.isDirty = false;
      }
      this.gvIRS4506TList.SelectedItems.Clear();
      this.cboLayout.SelectedIndexChanged -= new EventHandler(this.cboLayout_SelectedIndexChanged);
      this.cboLayout.SelectedIndex = 0;
      this.cboLayout.SelectedIndexChanged += new EventHandler(this.cboLayout_SelectedIndexChanged);
      this.currentIRS4506TFields = new IRS4506TFields();
      this.currentIRS4506TFields.IsNew = true;
      this.currentIRS4506TFields.InEditMode = true;
      this.currentIRS4506TFields.SetField("IR0193", "4506-COct2022");
      this.loadInputScreen();
      this.setButtonStatus(true);
      this.txtTemplateName.Focus();
      this.isDirty = false;
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.btnSave.Enabled && this.isDirty && Utils.Dialog((IWin32Window) this, "Do you want to save the change? Otherwise, the change will be discarded!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      if (this.gvIRS4506TList.SelectedItems.Count != 1 || this.currentIRS4506TFields == null)
        return;
      this.currentIRS4506TFields = (IRS4506TFields) this.currentIRS4506TFields.Duplicate();
      this.currentIRS4506TFields.TemplateID = 0;
      this.currentIRS4506TFields.IsNew = true;
      this.currentIRS4506TFields.InEditMode = true;
      this.gvIRS4506TList.SelectedItems.Clear();
      this.cboLayout.SelectedIndexChanged -= new EventHandler(this.cboLayout_SelectedIndexChanged);
      this.cboLayout.SelectedIndex = 0;
      this.cboLayout.SelectedIndexChanged += new EventHandler(this.cboLayout_SelectedIndexChanged);
      this.loadInputScreen();
      this.setButtonStatus(true);
      this.txtTemplateName.Focus();
      this.isDirty = false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtTemplateName.Text.Trim()))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The template name cannot be blank!" + (sender == null ? "Your change will be discarded!" : ""), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (string.IsNullOrEmpty(this.cboLayout.Text.Trim()))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The template version name cannot be blank!" + (sender == null ? "Your change will be discarded!" : ""), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        for (int nItemIndex = 0; nItemIndex < this.gvIRS4506TList.Items.Count; ++nItemIndex)
        {
          if ((this.currentIRS4506TFields.IsNew || nItemIndex != this.currentSelectedIndex) && string.Compare(this.gvIRS4506TList.Items[nItemIndex].SubItems[1].Text, this.txtTemplateName.Text.Trim(), true) == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The current list already contains the template '" + this.txtTemplateName.Text + "'!" + (sender == null ? "Your change will be discarded!" : ""), MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (this.browserHandler != null)
          this.browserHandler.UpdateCurrentField();
        this.currentIRS4506TFields.TemplateName = this.txtTemplateName.Text.Trim();
        this.currentIRS4506TFields.LastModifiedBy = this.session.UserID;
        this.currentIRS4506TFields.LastModifiedDateTime = DateTime.Now.ToUniversalTime();
        try
        {
          if (this.currentIRS4506TFields.TemplateID == 0)
            this.currentIRS4506TFields.TemplateID = this.session.ConfigurationManager.CreateIRS4506TTemplate(this.currentIRS4506TFields.GetTemplate());
          else
            this.session.ConfigurationManager.UpdateIRS4506TTemplate(this.currentIRS4506TFields.GetTemplate());
        }
        catch (Exception ex)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The template cannot be saved due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.currentIRS4506TFields.IsNew)
        {
          this.currentIRS4506TFields.IsNew = this.currentIRS4506TFields.InEditMode = false;
          this.addGVItem(this.currentIRS4506TFields.Version, this.currentIRS4506TFields.TemplateName, this.currentIRS4506TFields.YearsRequested, this.currentIRS4506TFields.LastModifiedBy, this.currentIRS4506TFields.LastModifiedDateTime, this.currentIRS4506TFields.TemplateID, true);
        }
        else
          this.updateGVItem(this.gvIRS4506TList.Items[this.currentSelectedIndex], this.currentIRS4506TFields);
        this.setButtonStatus(false);
        if (this.browserHandler == null)
          return;
        this.browserHandler.RefreshContents();
      }
    }

    private void addGVItem(
      string requestVersion,
      string templateName,
      string requestYears,
      string lastModifyBy,
      DateTime lastModifedTime,
      int templateID,
      bool selected)
    {
      GVItem gvItem = new GVItem(requestVersion);
      gvItem.SubItems.Add((object) templateName);
      gvItem.SubItems.Add((object) requestYears);
      gvItem.SubItems.Add((object) lastModifyBy);
      gvItem.SubItems.Add((object) lastModifedTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"));
      gvItem.Tag = (object) templateID;
      gvItem.Selected = selected;
      this.gvIRS4506TList.SelectedIndexChanged -= new EventHandler(this.Template_SelectedIndexChanged);
      this.gvIRS4506TList.Items.Add(gvItem);
      this.gvIRS4506TList.SelectedIndexChanged += new EventHandler(this.Template_SelectedIndexChanged);
    }

    private void updateGVItem(GVItem item, IRS4506TFields template)
    {
      this.currentIRS4506TFields.InEditMode = false;
      item.Text = IRS4506TFields.VersionTextConversion(template.GetSimpleField("IR0193"), true);
      item.SubItems[1].Text = template.TemplateName;
      item.SubItems[2].Text = template.YearsRequested;
      item.SubItems[3].Text = template.LastModifiedBy;
      item.SubItems[4].Text = template.LastModifiedDateTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt");
      item.Tag = (object) template.TemplateID;
    }

    private void cboLayout_SelectedIndexChanged(object sender, EventArgs e)
    {
      string val = this.cboLayout.Text;
      switch (this.cboLayout.Text)
      {
        case "4506-C Oct 2022":
          val = "4506-COct2022";
          break;
        case "4506-C Sept 2020":
          val = "4506-CSept2020";
          break;
      }
      this.currentIRS4506TFields.SetField("IR0193", val);
      this.loadInputScreen();
      this.isDirty = true;
    }

    private void loadInputScreen()
    {
      if (this.browserHandler != null)
      {
        this.browserHandler.ReleaseBrowser(true);
        this.browserHandler = (FrmBrowserHandler) null;
      }
      if (this.browserHandler == null)
      {
        this.browserHandler = new FrmBrowserHandler(this.session, (IWin32Window) this, (IHtmlInput) this.currentIRS4506TFields);
        this.browserHandler.AttachToBrowser(this.webBrowser);
        this.browserHandler.OnFieldChanged += new EventHandler(this.BrowserHandler_OnFieldChanged);
      }
      this.browserHandler.Property = (object) 1;
      this.browserHandler.OpenForm(new InputFormInfo("TAX4506T", "Request for Transcript of Tax", InputFormType.Standard));
      this.txtTemplateName.Text = this.currentIRS4506TFields.TemplateName;
      this.cboLayout.Text = IRS4506TFields.VersionTextConversion(this.currentIRS4506TFields.Version, true);
    }

    private void BrowserHandler_OnFieldChanged(object sender, EventArgs e) => this.isDirty = true;

    private void setButtonStatus(bool inEditMode)
    {
      this.btnNew.Enabled = true;
      this.btnEdit.Enabled = !inEditMode;
      this.btnDelete.Enabled = !inEditMode;
      this.btnDuplicate.Enabled = !inEditMode;
      this.btnSave.Enabled = inEditMode;
      this.btnReset.Enabled = inEditMode;
      if (!inEditMode)
      {
        this.btnEdit.Enabled = this.gvIRS4506TList.SelectedItems.Count == 1;
        this.btnDuplicate.Enabled = this.gvIRS4506TList.SelectedItems.Count == 1;
        this.btnDelete.Enabled = this.gvIRS4506TList.SelectedItems.Count > 0;
        this.txtTemplateName.ReadOnly = true;
        this.cboLayout.Enabled = false;
      }
      else
      {
        this.txtTemplateName.ReadOnly = false;
        this.cboLayout.Enabled = true;
        this.btnDelete.Enabled = this.gvIRS4506TList.SelectedItems.Count > 0;
      }
      this.grpTemplateName.Visible = inEditMode || this.gvIRS4506TList.SelectedItems.Count == 1;
      this.panelInputScreen.Visible = inEditMode || this.gvIRS4506TList.SelectedItems.Count == 1;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.session.ConfigurationManager.DeleteIRS4506TTemplate(this.currentIRS4506TFields.TemplateName);
      int index = this.gvIRS4506TList.SelectedItems[0].Index;
      this.gvIRS4506TList.Items.RemoveAt(index);
      if (index < this.gvIRS4506TList.Items.Count)
      {
        this.gvIRS4506TList.Items[index].Selected = true;
      }
      else
      {
        if (this.gvIRS4506TList.Items.Count <= 0)
          return;
        this.gvIRS4506TList.Items[this.gvIRS4506TList.Items.Count - 1].Selected = true;
      }
    }

    private void txtTemplateName_TextChanged(object sender, EventArgs e)
    {
      if (!(this.txtTemplateName.Text != this.currentIRS4506TFields.TemplateName))
        return;
      this.currentIRS4506TFields.TemplateName = this.txtTemplateName.Text;
      this.isDirty = true;
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
      GVColumn gvColumn5 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IRS4506TSettingsForm));
      this.grpListContainer = new GroupContainer();
      this.btnDuplicate = new StandardIconButton();
      this.gvIRS4506TList = new GridView();
      this.btnNew = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.grpTemplateName = new GroupContainer();
      this.cboLayout = new ComboBox();
      this.label42 = new Label();
      this.txtTemplateName = new TextBox();
      this.label41 = new Label();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.splitContainerAll = new SplitContainer();
      this.panelInputScreen = new Panel();
      this.webBrowser = new AxWebBrowser();
      this.grpListContainer.SuspendLayout();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      this.grpTemplateName.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.splitContainerAll.BeginInit();
      this.splitContainerAll.Panel1.SuspendLayout();
      this.splitContainerAll.Panel2.SuspendLayout();
      this.splitContainerAll.SuspendLayout();
      this.panelInputScreen.SuspendLayout();
      this.webBrowser.BeginInit();
      this.SuspendLayout();
      this.grpListContainer.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpListContainer.Controls.Add((Control) this.btnDuplicate);
      this.grpListContainer.Controls.Add((Control) this.gvIRS4506TList);
      this.grpListContainer.Controls.Add((Control) this.btnNew);
      this.grpListContainer.Controls.Add((Control) this.btnDelete);
      this.grpListContainer.Controls.Add((Control) this.btnEdit);
      this.grpListContainer.Dock = DockStyle.Fill;
      this.grpListContainer.HeaderForeColor = SystemColors.ControlText;
      this.grpListContainer.Location = new Point(0, 0);
      this.grpListContainer.Name = "grpListContainer";
      this.grpListContainer.Size = new Size(1066, 143);
      this.grpListContainer.TabIndex = 0;
      this.grpListContainer.Text = "List of Transcript Request Templates";
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(994, 4);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 112;
      this.btnDuplicate.TabStop = false;
      this.btnDuplicate.Tag = (object) "Duplicate";
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.gvIRS4506TList.AllowMultiselect = false;
      this.gvIRS4506TList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colVersion";
      gvColumn1.Tag = (object) "Version";
      gvColumn1.Text = "Request Form and Version";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colTemplateName";
      gvColumn2.Tag = (object) "TemplateName";
      gvColumn2.Text = "Template Name";
      gvColumn2.Width = 350;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colYearsRequested";
      gvColumn3.Tag = (object) "YearsRequested";
      gvColumn3.Text = "Years Requested";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colModifiedBy";
      gvColumn4.SortMethod = GVSortMethod.DateTime;
      gvColumn4.Tag = (object) "LastModifiedBy";
      gvColumn4.Text = "Last Modified By";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colModifiedDateTime";
      gvColumn5.Tag = (object) "LastModifiedDateTIme";
      gvColumn5.Text = "Last Modified Date & Time";
      gvColumn5.Width = 150;
      this.gvIRS4506TList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvIRS4506TList.Dock = DockStyle.Fill;
      this.gvIRS4506TList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvIRS4506TList.Location = new Point(1, 26);
      this.gvIRS4506TList.Name = "gvIRS4506TList";
      this.gvIRS4506TList.Size = new Size(1064, 117);
      this.gvIRS4506TList.TabIndex = 0;
      this.gvIRS4506TList.SelectedIndexChanged += new EventHandler(this.Template_SelectedIndexChanged);
      this.gvIRS4506TList.DoubleClick += new EventHandler(this.gvHMDAProfile_DoubleClick);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(972, 4);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 67;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(1038, 4);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 111;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(1016, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 110;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.grpTemplateName.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.grpTemplateName.Controls.Add((Control) this.cboLayout);
      this.grpTemplateName.Controls.Add((Control) this.label42);
      this.grpTemplateName.Controls.Add((Control) this.txtTemplateName);
      this.grpTemplateName.Controls.Add((Control) this.label41);
      this.grpTemplateName.Controls.Add((Control) this.btnReset);
      this.grpTemplateName.Controls.Add((Control) this.btnSave);
      this.grpTemplateName.Dock = DockStyle.Top;
      this.grpTemplateName.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplateName.Location = new Point(0, 0);
      this.grpTemplateName.Name = "grpTemplateName";
      this.grpTemplateName.Size = new Size(1066, 84);
      this.grpTemplateName.TabIndex = 1;
      this.grpTemplateName.Text = "Transcript Request Template Details";
      this.cboLayout.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLayout.FormattingEnabled = true;
      this.cboLayout.Items.AddRange(new object[4]
      {
        (object) "4506-C Oct 2022",
        (object) "4506-C Sept 2020",
        (object) "4506-T",
        (object) "8821"
      });
      this.cboLayout.Location = new Point(115, 56);
      this.cboLayout.Name = "cboLayout";
      this.cboLayout.Size = new Size(193, 22);
      this.cboLayout.TabIndex = 112;
      this.cboLayout.SelectedIndexChanged += new EventHandler(this.cboLayout_SelectedIndexChanged);
      this.label42.AutoSize = true;
      this.label42.ForeColor = Color.Red;
      this.label42.Location = new Point(97, 37);
      this.label42.Name = "label42";
      this.label42.Size = new Size(11, 14);
      this.label42.TabIndex = 2;
      this.label42.Text = "*";
      this.txtTemplateName.Location = new Point(115, 33);
      this.txtTemplateName.Name = "txtTemplateName";
      this.txtTemplateName.Size = new Size(365, 20);
      this.txtTemplateName.TabIndex = 1;
      this.txtTemplateName.TextChanged += new EventHandler(this.txtTemplateName_TextChanged);
      this.label41.AutoSize = true;
      this.label41.Location = new Point(15, 36);
      this.label41.Name = "label41";
      this.label41.Size = new Size(82, 14);
      this.label41.TabIndex = 1;
      this.label41.Text = "Template Name:";
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(1039, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 111;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(1017, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 110;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.splitContainerAll.BorderStyle = BorderStyle.FixedSingle;
      this.splitContainerAll.Dock = DockStyle.Fill;
      this.splitContainerAll.Location = new Point(0, 0);
      this.splitContainerAll.Name = "splitContainerAll";
      this.splitContainerAll.Orientation = Orientation.Horizontal;
      this.splitContainerAll.Panel1.Controls.Add((Control) this.grpListContainer);
      this.splitContainerAll.Panel2.Controls.Add((Control) this.panelInputScreen);
      this.splitContainerAll.Panel2.Controls.Add((Control) this.grpTemplateName);
      this.splitContainerAll.Size = new Size(1068, 485);
      this.splitContainerAll.SplitterDistance = 145;
      this.splitContainerAll.TabIndex = 2;
      this.panelInputScreen.Controls.Add((Control) this.webBrowser);
      this.panelInputScreen.Dock = DockStyle.Fill;
      this.panelInputScreen.Location = new Point(0, 84);
      this.panelInputScreen.Name = "panelInputScreen";
      this.panelInputScreen.Size = new Size(1066, 250);
      this.panelInputScreen.TabIndex = 2;
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Enabled = true;
      this.webBrowser.Location = new Point(0, 0);
      this.webBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("webBrowser.OcxState");
      this.webBrowser.Size = new Size(1066, 250);
      this.webBrowser.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainerAll);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (IRS4506TSettingsForm);
      this.Size = new Size(1068, 485);
      this.grpListContainer.ResumeLayout(false);
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      this.grpTemplateName.ResumeLayout(false);
      this.grpTemplateName.PerformLayout();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.splitContainerAll.Panel1.ResumeLayout(false);
      this.splitContainerAll.Panel2.ResumeLayout(false);
      this.splitContainerAll.EndInit();
      this.splitContainerAll.ResumeLayout(false);
      this.panelInputScreen.ResumeLayout(false);
      this.webBrowser.EndInit();
      this.ResumeLayout(false);
    }
  }
}
