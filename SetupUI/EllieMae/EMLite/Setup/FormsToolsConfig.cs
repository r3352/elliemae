// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FormsToolsConfig
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FormsToolsConfig : UserControl, IPersonaSecurityPage
  {
    private const int idxCheck = 0;
    private const int idxName = 1;
    private bool suspendFormItemCheckEventHandler;
    private bool disableCheckBoxes;
    private bool formSettingsDirty;
    private bool bIsUserSetup;
    private int personaID = -1;
    private string userid;
    private Persona[] personas;
    private InputFormInfo[] allFormInfos;
    private ToolsPage toolsPage;
    private PipelineConfiguration pipelineConfiguration;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcInputForms;
    private Splitter splitter1;
    private PanelEx pnlExTools;
    private GridView gvInputForms;
    private ContextMenuStrip ctxMenuInputForms;
    private ToolStripMenuItem tsMenuItemConnect;
    private ToolStripMenuItem tsMenuItemDisconnect;
    private Label lblNoAccess;
    private Panel pnlBase;

    public event EventHandler DirtyFlagChanged;

    public FormsToolsConfig(
      Sessions.Session session,
      int personaID,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.toolsPage = new ToolsPage(this.session, personaID, pipelineConfiguration, dirtyFlagChanged);
      this.pipelineConfiguration = pipelineConfiguration;
      this.init((string) null, (Persona[]) null, personaID, dirtyFlagChanged);
      this.gvInputForms.ContextMenuStrip = (ContextMenuStrip) null;
    }

    public FormsToolsConfig(
      Sessions.Session session,
      string userid,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.toolsPage = new ToolsPage(this.session, userid, personas, pipelineConfiguration, dirtyFlagChanged);
      this.pipelineConfiguration = pipelineConfiguration;
      this.init(userid, personas, -1, dirtyFlagChanged);
    }

    private void setDirtyFlag(bool val)
    {
      this.formSettingsDirty = val;
      if (!val)
        this.toolsPage.ResetDirtyFlag();
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, this.IsDirty ? new EventArgs() : (EventArgs) null);
    }

    private void init(
      string userid,
      Persona[] personas,
      int personaID,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      if (userid == null)
      {
        this.bIsUserSetup = false;
        this.personaID = personaID;
      }
      else
      {
        this.bIsUserSetup = true;
        this.userid = userid;
        this.personas = personas;
      }
      this.loadFormLists();
      this.toolsPage.Dock = DockStyle.Fill;
      this.toolsPage.TopLevel = false;
      this.toolsPage.Visible = true;
      this.pnlExTools.Controls.Add((Control) this.toolsPage);
      this.pipelineConfiguration.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelineConfiguration_FeatureStateChanged);
      this.pipelineConfiguration_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, this.pipelineConfiguration.HasPipelineLoanTabAccess() ? AclTriState.True : AclTriState.False, false);
      this.DirtyFlagChanged = dirtyFlagChanged;
    }

    private void pipelineConfiguration_FeatureStateChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContact)
    {
      if (feature != AclFeature.GlobalTab_Pipeline)
        return;
      if (state == AclTriState.True)
      {
        this.pnlBase.Visible = true;
        this.lblNoAccess.Visible = false;
      }
      else
      {
        this.pnlBase.Visible = false;
        this.lblNoAccess.Visible = true;
      }
    }

    private void loadFormLists()
    {
      this.suspendFormItemCheckEventHandler = true;
      try
      {
        if (this.bIsUserSetup)
          this.loadUserFormLists();
        else
          this.loadPersonaFormLists();
      }
      finally
      {
        this.suspendFormItemCheckEventHandler = false;
      }
    }

    private void loadUserFormLists()
    {
      InputFormsAclManager aclManager = (InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms);
      Hashtable permissionsForAllForms1 = aclManager.GetPermissionsForAllForms(this.personas);
      Hashtable permissionsForAllForms2 = aclManager.GetPermissionsForAllForms(this.userid);
      this.allFormInfos = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      this.gvInputForms.Items.Clear();
      string clientId = this.session.CompanyInfo.ClientID;
      for (int index = 0; index < this.allFormInfos.Length; ++index)
      {
        InputFormInfo allFormInfo = this.allFormInfos[index];
        if (!InputFormInfo.IsChildForm(allFormInfo.FormID) && (!(allFormInfo.FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE) && (this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(allFormInfo.FormID)))
          this.gvInputForms.Items.Add(!permissionsForAllForms2.Contains((object) allFormInfo.FormID) ? this.getViewItemFromFormInfo(allFormInfo, true, permissionsForAllForms1.Contains((object) allFormInfo.FormID) && (bool) permissionsForAllForms1[(object) allFormInfo.FormID]) : this.getViewItemFromFormInfo(allFormInfo, false, (bool) permissionsForAllForms2[(object) allFormInfo.FormID]));
      }
      this.gvInputForms.Sort(1, SortOrder.Ascending);
    }

    private GVItem getViewItemFromFormInfo(InputFormInfo formInfo, bool connected, bool accessible)
    {
      GVItem itemFromFormInfo;
      if (this.bIsUserSetup)
      {
        itemFromFormInfo = new GVItem();
        itemFromFormInfo.SubItems[0].Text = "";
        itemFromFormInfo.SubItems[1].Value = (object) new PersonaLinkImg(InputFormInfo.GetCorrectFormName(formInfo.FormID, formInfo.ToString()), connected, true);
      }
      else
        itemFromFormInfo = new GVItem(new string[2]
        {
          "",
          InputFormInfo.GetCorrectFormName(formInfo.FormID, formInfo.ToString())
        });
      itemFromFormInfo.SubItems[0].Checked = accessible;
      itemFromFormInfo.Tag = (object) formInfo;
      return itemFromFormInfo;
    }

    private void loadPersonaFormLists()
    {
      Hashtable permissionsForAllForms = ((InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms)).GetPermissionsForAllForms(this.personaID);
      this.allFormInfos = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      this.gvInputForms.Items.Clear();
      string clientId = this.session.CompanyInfo.ClientID;
      for (int index = 0; index < this.allFormInfos.Length; ++index)
      {
        InputFormInfo allFormInfo = this.allFormInfos[index];
        if ((!(allFormInfo.FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE) && !InputFormInfo.IsChildForm(allFormInfo.FormID) && (this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(allFormInfo.FormID)))
          this.gvInputForms.Items.Add(this.getViewItemFromFormInfo(allFormInfo, false, permissionsForAllForms.Contains((object) allFormInfo.FormID) && (bool) permissionsForAllForms[(object) allFormInfo.FormID]));
      }
      this.gvInputForms.Sort(1, SortOrder.Ascending);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvInputForms.Items)
        gvItem.SubItems[0].CheckBoxEnabled = !this.disableCheckBoxes;
    }

    public void SetPersona(int personaID)
    {
      this.personaID = personaID;
      this.loadFormLists();
      this.toolsPage.SetPersona(personaID);
      this.setDirtyFlag(false);
    }

    public void MakeReadOnly(bool readOnly)
    {
      this.toolsPage.MakeReadOnly(readOnly);
      if (readOnly)
        this.loadFormLists();
      this.disableCheckBoxes = readOnly;
      this.gvInputForms.SuspendLayout();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvInputForms.Items)
        gvItem.SubItems[0].CheckBoxEnabled = !readOnly;
      this.gvInputForms.ResumeLayout();
      this.tsMenuItemConnect.Enabled = this.tsMenuItemDisconnect.Enabled = !readOnly;
    }

    private void saveFormSettings()
    {
      if (!this.formSettingsDirty)
        return;
      InputFormsAclManager aclManager = (InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms);
      if (this.bIsUserSetup)
      {
        foreach (object key in (IEnumerable) aclManager.GetPermissionsForAllForms(this.userid).Keys)
          aclManager.SetPermission(string.Concat(key), this.userid, (object) null);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvInputForms.Items)
        {
          if (!((PersonaLinkImg) gvItem.SubItems[1].Value).IsLinked)
          {
            InputFormInfo tag = (InputFormInfo) gvItem.Tag;
            aclManager.SetPermission(tag.FormID, this.userid, (object) gvItem.SubItems[0].Checked);
          }
        }
      }
      else
      {
        string[] forms = new string[this.gvInputForms.Items.Count];
        bool[] accesses = new bool[this.gvInputForms.Items.Count];
        for (int nItemIndex = 0; nItemIndex < this.gvInputForms.Items.Count; ++nItemIndex)
        {
          GVItem gvItem = this.gvInputForms.Items[nItemIndex];
          InputFormInfo tag = (InputFormInfo) gvItem.Tag;
          forms[nItemIndex] = tag.FormID;
          accesses[nItemIndex] = gvItem.SubItems[0].Checked;
        }
        aclManager.SetPermission(forms, this.personaID, accesses);
      }
      this.setDirtyFlag(false);
    }

    public void Save()
    {
      this.saveFormSettings();
      this.toolsPage.SaveData();
      this.setDirtyFlag(false);
    }

    public void Reset()
    {
      this.loadFormLists();
      this.toolsPage.ResetData();
      this.setDirtyFlag(false);
    }

    public bool IsDirty => this.toolsPage.NeedToSaveData() || this.formSettingsDirty;

    private void FormsToolsConfig_SizeChanged(object sender, EventArgs e)
    {
      this.gcInputForms.Width = this.Width / 2;
    }

    private void gvInputForms_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.disableCheckBoxes || this.suspendFormItemCheckEventHandler)
        return;
      if (this.bIsUserSetup)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvInputForms.Items)
          gvItem.Selected = gvItem.SubItems[0] == e.SubItem;
        this.tsMenuItemDisconnect_Click(source, (EventArgs) e);
      }
      this.setDirtyFlag(true);
    }

    private void tsMenuItemConnect_Click(object sender, EventArgs e)
    {
      this.gvInputForms.BeginUpdate();
      Hashtable permissionsForAllForms = ((InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms)).GetPermissionsForAllForms(this.personas);
      foreach (GVItem selectedItem in this.gvInputForms.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (!personaLinkImg.IsLinked)
        {
          InputFormInfo tag = (InputFormInfo) selectedItem.Tag;
          personaLinkImg.Link();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
          IEnumerator enumerator = permissionsForAllForms.Keys.GetEnumerator();
          if (!selectedItem.SubItems[0].Checked)
          {
            while (enumerator.MoveNext())
            {
              if (string.Concat(enumerator.Current) == tag.FormID)
              {
                if ((bool) permissionsForAllForms[enumerator.Current])
                {
                  selectedItem.SubItems[0].Checked = true;
                  break;
                }
                break;
              }
            }
          }
          else
          {
            bool flag = false;
            while (enumerator.MoveNext())
            {
              if (string.Concat(enumerator.Current) == tag.FormID)
              {
                if (!(bool) permissionsForAllForms[enumerator.Current])
                  selectedItem.SubItems[0].Checked = false;
                flag = true;
                break;
              }
            }
            if (!flag)
              selectedItem.SubItems[0].Checked = false;
          }
        }
      }
      this.gvInputForms.EndUpdate();
      this.setDirtyFlag(true);
    }

    private void tsMenuItemDisconnect_Click(object sender, EventArgs e)
    {
      this.gvInputForms.BeginUpdate();
      foreach (GVItem selectedItem in this.gvInputForms.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (personaLinkImg.IsLinked)
        {
          personaLinkImg.Disconnect();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
        }
      }
      this.gvInputForms.EndUpdate();
      this.setDirtyFlag(true);
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
      this.splitter1 = new Splitter();
      this.pnlExTools = new PanelEx();
      this.gcInputForms = new GroupContainer();
      this.gvInputForms = new GridView();
      this.ctxMenuInputForms = new ContextMenuStrip(this.components);
      this.tsMenuItemConnect = new ToolStripMenuItem();
      this.tsMenuItemDisconnect = new ToolStripMenuItem();
      this.lblNoAccess = new Label();
      this.pnlBase = new Panel();
      this.gcInputForms.SuspendLayout();
      this.ctxMenuInputForms.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.SuspendLayout();
      this.splitter1.Location = new Point(328, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 455);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.pnlExTools.Dock = DockStyle.Fill;
      this.pnlExTools.Location = new Point(331, 0);
      this.pnlExTools.Name = "pnlExTools";
      this.pnlExTools.Size = new Size(328, 455);
      this.pnlExTools.TabIndex = 2;
      this.gcInputForms.Controls.Add((Control) this.gvInputForms);
      this.gcInputForms.Dock = DockStyle.Left;
      this.gcInputForms.HeaderForeColor = SystemColors.ControlText;
      this.gcInputForms.Location = new Point(0, 0);
      this.gcInputForms.Name = "gcInputForms";
      this.gcInputForms.Size = new Size(328, 455);
      this.gcInputForms.TabIndex = 0;
      this.gcInputForms.Text = "Input Forms";
      this.gvInputForms.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colAccessible";
      gvColumn1.Text = "Accessible";
      gvColumn1.Width = 65;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colName";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 250;
      this.gvInputForms.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvInputForms.ContextMenuStrip = this.ctxMenuInputForms;
      this.gvInputForms.Dock = DockStyle.Fill;
      this.gvInputForms.Location = new Point(1, 26);
      this.gvInputForms.Name = "gvInputForms";
      this.gvInputForms.Size = new Size(326, 428);
      this.gvInputForms.TabIndex = 0;
      this.gvInputForms.SubItemCheck += new GVSubItemEventHandler(this.gvInputForms_SubItemCheck);
      this.ctxMenuInputForms.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsMenuItemConnect,
        (ToolStripItem) this.tsMenuItemDisconnect
      });
      this.ctxMenuInputForms.Name = "ctxMenuInputForms";
      this.ctxMenuInputForms.Size = new Size(238, 48);
      this.tsMenuItemConnect.Name = "tsMenuItemConnect";
      this.tsMenuItemConnect.Size = new Size(237, 22);
      this.tsMenuItemConnect.Text = "Link with Persona Rights";
      this.tsMenuItemConnect.Click += new EventHandler(this.tsMenuItemConnect_Click);
      this.tsMenuItemDisconnect.Name = "tsMenuItemDisconnect";
      this.tsMenuItemDisconnect.Size = new Size(237, 22);
      this.tsMenuItemDisconnect.Text = "Disconnect from Persona Rights";
      this.tsMenuItemDisconnect.Click += new EventHandler(this.tsMenuItemDisconnect_Click);
      this.lblNoAccess.BackColor = Color.Transparent;
      this.lblNoAccess.Dock = DockStyle.Top;
      this.lblNoAccess.Location = new Point(0, 0);
      this.lblNoAccess.Name = "lblNoAccess";
      this.lblNoAccess.Size = new Size(659, 26);
      this.lblNoAccess.TabIndex = 1;
      this.lblNoAccess.Text = "The persona does not have access to the Pipeline, Loan, Forms and Tools, ePass tabs.";
      this.lblNoAccess.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNoAccess.Visible = false;
      this.pnlBase.Controls.Add((Control) this.pnlExTools);
      this.pnlBase.Controls.Add((Control) this.splitter1);
      this.pnlBase.Controls.Add((Control) this.gcInputForms);
      this.pnlBase.Dock = DockStyle.Fill;
      this.pnlBase.Location = new Point(0, 26);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(659, 455);
      this.pnlBase.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.pnlBase);
      this.Controls.Add((Control) this.lblNoAccess);
      this.Name = nameof (FormsToolsConfig);
      this.Size = new Size(659, 481);
      this.SizeChanged += new EventHandler(this.FormsToolsConfig_SizeChanged);
      this.gcInputForms.ResumeLayout(false);
      this.ctxMenuInputForms.ResumeLayout(false);
      this.pnlBase.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
