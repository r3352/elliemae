// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EDMEmailControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EDMEmailControl : SettingsUserControl
  {
    private Sessions.Session session;
    private GridViewDataManager gvTemplatesMgr;
    private IContainer components;
    private GroupContainer gcTemplates;
    private GridView gvTemplates;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnDelete;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnAdd;
    private ToolTip toolTip;

    public EDMEmailControl(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      NotificationTemplateRestClient.MigrateEConsentTemplates(this.session.SessionObjects);
      this.gvTemplates.AllowMultiselect = allowMultiSelect;
      this.initTemplateList();
      this.loadTemplateList();
    }

    private void initTemplateList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvTemplates, (LoanDataMgr) null);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.EmailTemplateTypeColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.WebCenterColumn,
        GridViewDataManager.ConsumerConnectColumn
      });
      this.gvTemplates.Columns[1].SpringToFit = true;
      this.gvTemplates.Sort(new GVColumnSort[2]
      {
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(1, SortOrder.Ascending)
      });
    }

    private void loadTemplateList()
    {
      foreach (HtmlEmailTemplate htmlEmailTemplate in this.session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.RequestDocuments | HtmlEmailTemplateType.SendDocuments | HtmlEmailTemplateType.InitialDisclosures | HtmlEmailTemplateType.LoanLevelConsent | HtmlEmailTemplateType.PreClosing | HtmlEmailTemplateType.ConsumerConnectPreClosing | HtmlEmailTemplateType.ConsumerConnectRequestDocuments | HtmlEmailTemplateType.ConsumerConnectSendDocuments | HtmlEmailTemplateType.ConsumerConnectInitialDisclosures | HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent))
        this.gvTemplatesMgr.AddItem(htmlEmailTemplate);
      this.gvTemplates.ReSort();
    }

    public event HtmlEmailTemplateChangedEventHandler HtmlEmailTemplateChanged;

    protected virtual void OnHtmlEmailTemplateChanged(HtmlEmailTemplate template)
    {
      if (this.HtmlEmailTemplateChanged == null)
        return;
      this.HtmlEmailTemplateChanged((object) this, template);
    }

    private void gvTemplates_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvTemplates.SelectedItems.Count;
      this.btnDuplicate.Enabled = count == 1;
      this.btnEdit.Enabled = count == 1;
      this.btnDelete.Enabled = count == 1;
    }

    public void btnAdd_Click(object sender, EventArgs e)
    {
      HtmlEmailTemplate template = new HtmlEmailTemplate((string) null);
      using (EmailTemplateDialog emailTemplateDialog = new EmailTemplateDialog(this.session, template, false))
      {
        if (emailTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gvTemplatesMgr.AddItem(template).Selected = true;
        this.gvTemplates.ReSort();
        this.OnHtmlEmailTemplateChanged(template);
      }
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      HtmlEmailTemplate template = ((HtmlEmailTemplate) this.gvTemplates.SelectedItems[0].Tag).Clone((string) null);
      template.Subject = string.Empty;
      using (EmailTemplateDialog emailTemplateDialog = new EmailTemplateDialog(this.session, template, false))
      {
        if (emailTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gvTemplatesMgr.AddItem(template).Selected = true;
        this.gvTemplates.ReSort();
        this.OnHtmlEmailTemplateChanged(template);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count != 1)
        return;
      GVItem selectedItem = this.gvTemplates.SelectedItems[0];
      HtmlEmailTemplate tag = (HtmlEmailTemplate) selectedItem.Tag;
      using (EmailTemplateDialog emailTemplateDialog = new EmailTemplateDialog(this.session, tag, false))
      {
        if (emailTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gvTemplatesMgr.RefreshItem(selectedItem, tag);
        this.gvTemplates.ReSort();
        this.OnHtmlEmailTemplateChanged(tag);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.gvTemplates.SelectedItems[0];
      HtmlEmailTemplate tag = (HtmlEmailTemplate) selectedItem.Tag;
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete this email template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      this.session.ConfigurationManager.DeleteHtmlEmailTemplate(tag);
      this.gvTemplates.Items.Remove(selectedItem);
      this.OnHtmlEmailTemplateChanged(tag);
    }

    public string[] SelectedHtmlTemplates
    {
      get
      {
        return this.gvTemplates.SelectedItems.Count == 0 ? (string[]) null : this.gvTemplates.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => ((HtmlEmailTemplate) item.Tag).Guid)).ToArray<string>();
      }
    }

    public void SetSelectedHtmlTemplates(List<string> selectedHtmlTemplates)
    {
      for (int index = 0; index < selectedHtmlTemplates.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.gvTemplates.Items.Count; ++nItemIndex)
        {
          if (((HtmlEmailTemplate) this.gvTemplates.Items[nItemIndex].Tag).Guid == selectedHtmlTemplates[index])
          {
            this.gvTemplates.Items[nItemIndex].Selected = true;
            break;
          }
        }
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
      this.gcTemplates = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvTemplates = new GridView();
      this.toolTip = new ToolTip(this.components);
      this.gcTemplates.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.gcTemplates.Controls.Add((Control) this.pnlToolbar);
      this.gcTemplates.Controls.Add((Control) this.gvTemplates);
      this.gcTemplates.Dock = DockStyle.Fill;
      this.gcTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcTemplates.Location = new Point(0, 0);
      this.gcTemplates.Name = "gcTemplates";
      this.gcTemplates.Size = new Size(468, 277);
      this.gcTemplates.TabIndex = 0;
      this.gcTemplates.Text = "Templates";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnEdit);
      this.pnlToolbar.Controls.Add((Control) this.btnDuplicate);
      this.pnlToolbar.Controls.Add((Control) this.btnAdd);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(368, 3);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(96, 22);
      this.pnlToolbar.TabIndex = 2;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(80, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 17);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 29;
      this.btnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDelete, "Delete Template");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(60, 3);
      this.btnEdit.Margin = new Padding(4, 3, 0, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 18);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 31;
      this.btnEdit.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnEdit, "Edit Template");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(40, 3);
      this.btnDuplicate.Margin = new Padding(4, 3, 0, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 18);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 30;
      this.btnDuplicate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDuplicate, "Duplicate Template");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(20, 3);
      this.btnAdd.Margin = new Padding(4, 3, 0, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 18);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 26;
      this.btnAdd.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnAdd, "Add Template");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(466, 250);
      this.gvTemplates.TabIndex = 0;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.ItemDoubleClick += new GVItemEventHandler(this.gvTemplates_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTemplates);
      this.Name = nameof (EDMEmailControl);
      this.Size = new Size(468, 277);
      this.gcTemplates.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
