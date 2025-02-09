// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusOnline.StatusOnlineTriggersControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.StatusOnline
{
  public class StatusOnlineTriggersControl : UserControl
  {
    private Sessions.Session session;
    private StatusOnlineSetup setup;
    private string userID;
    private TriggerPortalType portalType;
    private StatusOnlineSetup companySetup;
    private GridViewDataManager gvTriggersMgr;
    private Dictionary<string, HtmlEmailTemplate[]> emailTable;
    private IContainer components;
    private GroupContainer gcTriggers;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnDelete;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnAdd;
    private GridView gvTriggers;
    private ToolTip toolTip;

    public string[] SelectedGUIDs
    {
      get
      {
        return this.gvTriggers.SelectedItems.Count == 0 ? (string[]) null : this.gvTriggers.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => ((StatusOnlineTrigger) item.Tag).Guid)).ToArray<string>();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        foreach (GVItem gvItem in this.gvTriggers.Items.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(((StatusOnlineTrigger) item.Tag).Guid))))
          gvItem.Selected = true;
      }
    }

    public StatusOnlineTriggersControl(
      Sessions.Session session,
      StatusOnlineSetup setup,
      TriggerPortalType portalType,
      string userID,
      bool isSettingSync)
    {
      this.InitializeComponent();
      this.session = session;
      this.setup = setup;
      this.portalType = portalType;
      this.userID = userID;
      this.gvTriggers.AllowMultiselect = isSettingSync;
      if (!string.IsNullOrEmpty(userID))
        this.companySetup = session.ConfigurationManager.GetStatusOnlineSetup((string) null);
      this.emailTable = new Dictionary<string, HtmlEmailTemplate[]>();
      this.initTriggerList();
      this.loadTriggerList((StatusOnlineTrigger) null);
    }

    private void initTriggerList()
    {
      this.gvTriggersMgr = new GridViewDataManager(this.session, this.gvTriggers, (LoanDataMgr) null);
      this.gvTriggersMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.StatusTriggerColumn,
        GridViewDataManager.UpdateMethodColumn,
        GridViewDataManager.EmailMessageColumn
      });
      this.gvTriggers.Sort(0, SortOrder.Ascending);
    }

    private void loadTriggerList(StatusOnlineTrigger defaultTrigger)
    {
      this.gvTriggersMgr.ClearItems();
      if (this.companySetup != null)
      {
        foreach (StatusOnlineTrigger trigger in (CollectionBase) this.companySetup.Triggers)
        {
          if (trigger.PortalType == this.portalType)
          {
            HtmlEmailTemplate emailTemplate = this.getEmailTemplate(trigger);
            this.gvTriggersMgr.AddItem(trigger, emailTemplate).ForeColor = Color.Gray;
          }
        }
      }
      foreach (StatusOnlineTrigger trigger in (CollectionBase) this.setup.Triggers)
      {
        if (trigger.PortalType == this.portalType)
        {
          HtmlEmailTemplate emailTemplate = this.getEmailTemplate(trigger);
          GVItem gvItem = this.gvTriggersMgr.AddItem(trigger, emailTemplate);
          if (trigger == defaultTrigger)
            gvItem.Selected = true;
        }
      }
      this.gvTriggers.ReSort();
    }

    public void ProcessEmailTemplateChange(HtmlEmailTemplate template)
    {
      string key = template.OwnerID ?? string.Empty;
      if (!this.emailTable.ContainsKey(key))
        return;
      this.emailTable.Remove(key);
      StatusOnlineTrigger defaultTrigger = (StatusOnlineTrigger) null;
      if (this.gvTriggers.SelectedItems.Count > 0)
        defaultTrigger = (StatusOnlineTrigger) this.gvTriggers.SelectedItems[0].Tag;
      this.loadTriggerList(defaultTrigger);
    }

    private HtmlEmailTemplate getEmailTemplate(StatusOnlineTrigger trigger)
    {
      if (string.IsNullOrEmpty(trigger.EmailTemplate))
        return (HtmlEmailTemplate) null;
      string str = trigger.EmailTemplateOwner ?? string.Empty;
      if (!this.emailTable.ContainsKey(str))
        this.emailTable[str] = this.session.ConfigurationManager.GetHtmlEmailTemplates(str, HtmlEmailTemplateType.StatusOnline);
      foreach (HtmlEmailTemplate emailTemplate in this.emailTable[str])
      {
        if (emailTemplate.Guid == trigger.EmailTemplate)
          return emailTemplate;
      }
      return (HtmlEmailTemplate) null;
    }

    private void gvTriggers_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvTriggers_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvTriggers.SelectedItems.Count;
      this.btnDuplicate.Enabled = count == 1;
      this.btnEdit.Enabled = count == 1;
      this.btnDelete.Enabled = count == 1;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      StatusOnlineTrigger statusOnlineTrigger = new StatusOnlineTrigger(this.userID, this.portalType);
      using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(this.session, this.setup, statusOnlineTrigger))
      {
        if (onlineTriggerDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadTriggerList(statusOnlineTrigger);
      }
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      StatusOnlineTrigger tag = (StatusOnlineTrigger) this.gvTriggers.SelectedItems[0].Tag;
      StatusOnlineTrigger statusOnlineTrigger = tag.Clone(this.userID, tag.PortalType);
      statusOnlineTrigger.Name = string.Empty;
      using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(this.session, this.setup, statusOnlineTrigger))
      {
        if (onlineTriggerDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadTriggerList(statusOnlineTrigger);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvTriggers.SelectedItems.Count != 1)
        return;
      StatusOnlineTrigger tag = (StatusOnlineTrigger) this.gvTriggers.SelectedItems[0].Tag;
      if (!string.IsNullOrEmpty(this.userID) && string.IsNullOrEmpty(tag.OwnerID))
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You cannot edit a company template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(this.session, this.setup, tag))
        {
          if (onlineTriggerDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.loadTriggerList(tag);
        }
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      StatusOnlineTrigger tag = (StatusOnlineTrigger) this.gvTriggers.SelectedItems[0].Tag;
      if (!string.IsNullOrEmpty(this.userID) && string.IsNullOrEmpty(tag.OwnerID))
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You cannot delete a company status template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete this status template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
        this.setup.Triggers.Remove(tag);
        this.session.ConfigurationManager.SaveStatusOnlineSetup(this.userID, this.setup);
        this.loadTriggerList((StatusOnlineTrigger) null);
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
      this.gcTriggers = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvTriggers = new GridView();
      this.toolTip = new ToolTip(this.components);
      this.gcTriggers.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.gcTriggers.Controls.Add((Control) this.pnlToolbar);
      this.gcTriggers.Controls.Add((Control) this.gvTriggers);
      this.gcTriggers.Dock = DockStyle.Fill;
      this.gcTriggers.HeaderForeColor = SystemColors.ControlText;
      this.gcTriggers.Location = new Point(0, 0);
      this.gcTriggers.Name = "gcTriggers";
      this.gcTriggers.Size = new Size(496, 312);
      this.gcTriggers.TabIndex = 0;
      this.gcTriggers.Text = "Status Online Templates";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnEdit);
      this.pnlToolbar.Controls.Add((Control) this.btnDuplicate);
      this.pnlToolbar.Controls.Add((Control) this.btnAdd);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(392, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(100, 22);
      this.pnlToolbar.TabIndex = 1;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(84, 3);
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
      this.btnEdit.Location = new Point(64, 3);
      this.btnEdit.Margin = new Padding(4, 3, 0, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 18);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 28;
      this.btnEdit.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnEdit, "Edit Template");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(44, 3);
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
      this.btnAdd.Location = new Point(24, 3);
      this.btnAdd.Margin = new Padding(4, 3, 0, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 18);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 26;
      this.btnAdd.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnAdd, "Add Template");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvTriggers.AllowMultiselect = false;
      this.gvTriggers.BorderStyle = BorderStyle.None;
      this.gvTriggers.Dock = DockStyle.Fill;
      this.gvTriggers.Location = new Point(1, 26);
      this.gvTriggers.Name = "gvTriggers";
      this.gvTriggers.Size = new Size(494, 285);
      this.gvTriggers.TabIndex = 0;
      this.gvTriggers.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTriggers.SelectedIndexChanged += new EventHandler(this.gvTriggers_SelectedIndexChanged);
      this.gvTriggers.ItemDoubleClick += new GVItemEventHandler(this.gvTriggers_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTriggers);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StatusOnlineTriggersControl);
      this.Size = new Size(496, 312);
      this.gcTriggers.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
