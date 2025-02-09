// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOConnectConfig
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
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
  public class LOConnectConfig : UserControl, IPersonaSecurityPage
  {
    private const int idxCheck = 0;
    private const int idxName = 1;
    private LoConnectServiceAclManager loConnectAclManager;
    private StandardWebFormAclManager standardWebFormAclManager;
    private bool customFormToolsSettingsDirty;
    private Dictionary<LoConnectCustomServiceInfo, string> customServices;
    private bool disableCheckBoxes;
    private int personaID = -1;
    private string userid;
    private Persona[] personas;
    private LOStandardFeaturesPage loStandardFeaturesPage;
    private bool isStandardFeaturesPageVisible;
    private bool bIsUserSetup;
    internal LoConnectCustomServiceInfo[] loConnectServices;
    private FeaturesAclManager featuresAclMgr;
    private Sessions.Session session;
    private IContainer components;
    private PanelEx pnlExTools;
    private ContextMenuStrip ctxMenuInputForms;
    private ToolStripMenuItem tsMenuItemConnect;
    private ToolStripMenuItem tsMenuItemDisconnect;
    private Label lblNoAccess;
    private Panel pnlBase;
    private PanelEx panelEx2;
    private PanelEx panelEx1;
    private GroupContainer gcCustomForms;
    private GridView gvCustomForms;
    private GridView gvCustomGlobal;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer1;
    private GridView gvCustomTool;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private GroupContainer groupContainer3;
    private GridView gvStandardWebForms;
    private GroupContainer groupContainer4;
    private RadioButton rdbTaskPipelineOption;
    private RadioButton rdbLoanPipelineOption;
    private PanelEx pnlxStandardFeatures;

    public event EventHandler DirtyFlagChanged;

    public LOConnectConfig(Sessions.Session session, int personaID, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      if (string.Equals(this.session.ConfigurationManager.GetCompanySetting("FEATURE", "ENABLEWORKFLOWTASKS"), "true", StringComparison.CurrentCultureIgnoreCase))
      {
        this.isStandardFeaturesPageVisible = true;
        this.loStandardFeaturesPage = new LOStandardFeaturesPage(this.session, personaID, dirtyFlagChanged);
      }
      this.bIsUserSetup = false;
      this.featuresAclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.init((string) null, (Persona[]) null, personaID, dirtyFlagChanged);
    }

    public LOConnectConfig(
      Sessions.Session session,
      string userid,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      if (string.Equals(this.session.ConfigurationManager.GetCompanySetting("FEATURE", "ENABLEWORKFLOWTASKS"), "true", StringComparison.CurrentCultureIgnoreCase))
      {
        this.isStandardFeaturesPageVisible = true;
        this.loStandardFeaturesPage = new LOStandardFeaturesPage(this.session, userid, personas, dirtyFlagChanged);
      }
      this.bIsUserSetup = true;
      this.featuresAclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.init(userid, personas, -1, dirtyFlagChanged);
    }

    private void setDirtyFlag(bool val)
    {
      this.customFormToolsSettingsDirty = val;
      if (!val && this.isStandardFeaturesPageVisible)
        this.loStandardFeaturesPage.ResetDirtyFlag();
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
        this.personaID = personaID;
      }
      else
      {
        this.userid = userid;
        this.personas = personas;
      }
      if (this.isStandardFeaturesPageVisible)
      {
        this.loStandardFeaturesPage.Dock = DockStyle.Fill;
        this.loStandardFeaturesPage.TopLevel = false;
        this.loStandardFeaturesPage.Visible = true;
        this.pnlxStandardFeatures.Controls.Add((Control) this.loStandardFeaturesPage);
      }
      this.loConnectAclManager = (LoConnectServiceAclManager) this.session.ACL.GetAclManager(AclCategory.LOConnectCustomServices);
      this.standardWebFormAclManager = (StandardWebFormAclManager) this.session.ACL.GetAclManager(AclCategory.StandardWebforms);
      this.populateCustomService();
      this.populateStandardWebForm();
      this.PopulatePipelineDefaultOption();
      this.DirtyFlagChanged = dirtyFlagChanged;
      this.setDirtyFlag(false);
    }

    public void PopulatePipelineDefaultOption()
    {
      bool flag = true;
      if (this.personaID >= 1)
        flag = this.featuresAclMgr.GetPermission(AclFeature.LOConnectTab_LoanPipelineOption, this.personaID);
      else if (this.personaID <= 0 && !string.IsNullOrEmpty(this.userid))
        flag = this.featuresAclMgr.CheckPermission(AclFeature.LOConnectTab_LoanPipelineOption, this.userid);
      this.rdbLoanPipelineOption.Checked = flag;
      this.rdbTaskPipelineOption.Checked = !flag;
    }

    public void populateStandardWebForm()
    {
      if (this.bIsUserSetup)
        this.populateStandardWebFormForUser();
      else
        this.populateStandardWebFormForPersona();
    }

    public void populateStandardWebFormForUser()
    {
      Hashtable permissionsByUser = this.standardWebFormAclManager.GetPermissionsByUser(this.userid);
      int[] personaIDs = new int[this.personas.Length];
      for (int index = 0; index < this.personas.Length; ++index)
        personaIDs[index] = this.personas[index].ID;
      Hashtable permissionsByPersonas = this.standardWebFormAclManager.GetPermissionsByPersonas(personaIDs);
      bool connected = true;
      this.gvStandardWebForms.Items.Clear();
      foreach (StandardWebFormInfo activeForm in this.standardWebFormAclManager.GetActiveForms())
      {
        bool accessible = false;
        if (permissionsByUser.ContainsKey((object) activeForm.FormID))
        {
          accessible = (bool) permissionsByUser[(object) activeForm.FormID];
          connected = false;
        }
        else if (permissionsByPersonas.ContainsKey((object) activeForm.FormID))
        {
          accessible = (bool) permissionsByPersonas[(object) activeForm.FormID];
          connected = true;
        }
        this.gvStandardWebForms.Items.Add(this.getStandardFormItem(activeForm.FormName, activeForm.FormID, connected, accessible));
      }
    }

    public void populateStandardWebFormForPersona()
    {
      List<StandardWebFormInfo> standardWebFormInfoList = new List<StandardWebFormInfo>();
      List<StandardWebFormInfo> formsByPersona = this.standardWebFormAclManager.GetFormsByPersona(this.personaID);
      this.gvStandardWebForms.Items.Clear();
      foreach (StandardWebFormInfo standardWebFormInfo in formsByPersona)
        this.gvStandardWebForms.Items.Add(this.getStandardFormItem(standardWebFormInfo.FormName, standardWebFormInfo.FormID, true, standardWebFormInfo.Access));
    }

    public void populateCustomService()
    {
      if (this.customServices == null)
      {
        try
        {
          this.customServices = LOConnectRestAPIHelper.GetCustomServices();
          this.loConnectServices = this.customServices.Keys.ToArray<LoConnectCustomServiceInfo>();
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Error retrieving LOConnect Custom Services. " + ex.Message);
        }
      }
      if (this.customServices == null)
        return;
      if (this.bIsUserSetup)
        this.populateCustomFormsAndToolsForUser();
      else
        this.populateCustomFormsAndTools();
    }

    public void populateCustomFormsAndTools()
    {
      Hashtable permissionsByPersona = this.loConnectAclManager.GetPermissionsByPersona(this.loConnectServices, this.personaID);
      this.gvCustomGlobal.Items.Clear();
      this.gvCustomTool.Items.Clear();
      this.gvCustomForms.Items.Clear();
      foreach (KeyValuePair<LoConnectCustomServiceInfo, string> customService in this.customServices)
      {
        bool accessible = false;
        if (permissionsByPersona.Contains((object) customService.Key.CustomEntityId))
          accessible = (bool) permissionsByPersona[(object) customService.Key.CustomEntityId];
        if (customService.Key.ServiceType == LoServiceType.Tool)
          this.gvCustomTool.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, false, accessible));
        else if (customService.Key.ServiceType == LoServiceType.GlobalTool)
          this.gvCustomGlobal.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, false, accessible));
        else if (customService.Key.ServiceType == LoServiceType.Form)
          this.gvCustomForms.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, false, accessible));
      }
    }

    public void populateCustomFormsAndToolsForUser()
    {
      Hashtable permissionByPersonas = this.loConnectAclManager.GetUserPermissionByPersonas(this.personas);
      Hashtable permissionsByUser = this.loConnectAclManager.GetPermissionsByUser(this.loConnectServices, this.userid);
      bool connected = true;
      this.gvCustomGlobal.Items.Clear();
      this.gvCustomTool.Items.Clear();
      this.gvCustomForms.Items.Clear();
      foreach (KeyValuePair<LoConnectCustomServiceInfo, string> customService in this.customServices)
      {
        bool accessible = false;
        if (permissionsByUser.ContainsKey((object) customService.Key.CustomEntityId))
        {
          accessible = (bool) permissionsByUser[(object) customService.Key.CustomEntityId];
          connected = false;
        }
        else if (permissionByPersonas.ContainsKey((object) customService.Key.CustomEntityId))
          accessible = (bool) permissionByPersonas[(object) customService.Key.CustomEntityId];
        if (customService.Key.ServiceType == LoServiceType.Tool)
          this.gvCustomTool.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, connected, accessible));
        else if (customService.Key.ServiceType == LoServiceType.GlobalTool)
          this.gvCustomGlobal.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, connected, accessible));
        else if (customService.Key.ServiceType == LoServiceType.Form)
          this.gvCustomForms.Items.Add(this.getViewItem(customService.Value.ToString(), customService.Key, connected, accessible));
      }
    }

    private void saveCustomSettings()
    {
      if (this.bIsUserSetup)
      {
        List<LoConnectCustomServiceInfo> res1 = new List<LoConnectCustomServiceInfo>();
        this.addtoUserUpdateList(this.gvCustomForms, res1);
        this.addtoUserUpdateList(this.gvCustomTool, res1);
        this.addtoUserUpdateList(this.gvCustomGlobal, res1);
        List<StandardWebFormInfo> res2 = new List<StandardWebFormInfo>();
        this.addtoUserUpdateList_StandardWebForm(this.gvStandardWebForms, res2);
        if (res1.Count > 0)
          this.loConnectAclManager.SetPermissions(res1.ToArray(), this.userid);
        if (res2.Count <= 0)
          return;
        this.standardWebFormAclManager.SetPermissions(res2.ToArray(), this.userid);
      }
      else
      {
        List<LoConnectCustomServiceInfo> res3 = new List<LoConnectCustomServiceInfo>();
        this.addtoUpdateList(this.gvCustomForms, res3);
        this.addtoUpdateList(this.gvCustomTool, res3);
        this.addtoUpdateList(this.gvCustomGlobal, res3);
        List<StandardWebFormInfo> res4 = new List<StandardWebFormInfo>();
        this.addToUpdateList_StandardWebForm(this.gvStandardWebForms, res4);
        if (res3.Count > 0)
          this.loConnectAclManager.SetPermissions(res3.ToArray(), this.personaID);
        if (res4.Count <= 0)
          return;
        this.standardWebFormAclManager.SetPermissions(res4.ToArray(), this.personaID);
      }
    }

    private void addtoUpdateList(GridView gvCustom, List<LoConnectCustomServiceInfo> res)
    {
      for (int nItemIndex = 0; nItemIndex < gvCustom.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = gvCustom.Items[nItemIndex];
        LoConnectCustomServiceInfo tag = (LoConnectCustomServiceInfo) gvItem.Tag;
        LoConnectCustomServiceInfo customServiceInfo = new LoConnectCustomServiceInfo(tag.CustomEntityId, tag.ServiceType, gvItem.SubItems[0].Checked);
        res.Add(customServiceInfo);
      }
    }

    private void addtoUserUpdateList(GridView gvCustom, List<LoConnectCustomServiceInfo> res)
    {
      for (int nItemIndex = 0; nItemIndex < gvCustom.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = gvCustom.Items[nItemIndex];
        if (!((PersonaLinkImg) gvItem.SubItems[1].Value).IsLinked)
        {
          LoConnectCustomServiceInfo tag = (LoConnectCustomServiceInfo) gvItem.Tag;
          LoConnectCustomServiceInfo customServiceInfo = new LoConnectCustomServiceInfo(tag.CustomEntityId, tag.ServiceType, gvItem.SubItems[0].Checked);
          res.Add(customServiceInfo);
        }
      }
    }

    private void addToUpdateList_StandardWebForm(GridView gvForms, List<StandardWebFormInfo> res)
    {
      for (int nItemIndex = 0; nItemIndex < gvForms.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = gvForms.Items[nItemIndex];
        res.Add(new StandardWebFormInfo((int) gvItem.Tag, gvItem.SubItems[0].Checked, gvItem.Text)
        {
          LastModifiedBy = this.session.UserID
        });
      }
    }

    private void addtoUserUpdateList_StandardWebForm(GridView gvForm, List<StandardWebFormInfo> res)
    {
      for (int nItemIndex = 0; nItemIndex < gvForm.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = gvForm.Items[nItemIndex];
        if (!((PersonaLinkImg) gvItem.SubItems[1].Value).IsLinked)
          res.Add(new StandardWebFormInfo((int) gvItem.Tag, gvItem.SubItems[0].Checked, gvItem.Text)
          {
            LastModifiedBy = this.session.UserID
          });
      }
    }

    private GVItem getStandardFormItem(
      string formName,
      int formID,
      bool connected,
      bool accessible)
    {
      int nItemIndex = 0;
      GVItem standardFormItem;
      if (this.bIsUserSetup)
      {
        standardFormItem = new GVItem();
        standardFormItem.SubItems[nItemIndex].Text = "";
        standardFormItem.SubItems[1].Value = (object) new PersonaLinkImg(formName, connected, true);
      }
      else
        standardFormItem = new GVItem(new string[2]
        {
          "",
          formName
        });
      standardFormItem.SubItems[nItemIndex].Checked = accessible;
      standardFormItem.Tag = (object) formID;
      return standardFormItem;
    }

    private GVItem getViewItem(
      string formName,
      LoConnectCustomServiceInfo formId,
      bool connected,
      bool accessible)
    {
      int nItemIndex = 0;
      GVItem viewItem;
      if (this.bIsUserSetup)
      {
        viewItem = new GVItem();
        viewItem.SubItems[nItemIndex].Text = "";
        viewItem.SubItems[1].Value = (object) new PersonaLinkImg(formName, connected, true);
      }
      else
        viewItem = new GVItem(new string[2]{ "", formName });
      viewItem.SubItems[nItemIndex].Checked = accessible;
      viewItem.Tag = (object) formId;
      return viewItem;
    }

    public void SetPersona(int personaID)
    {
      this.personaID = personaID;
      if (this.isStandardFeaturesPageVisible)
        this.loStandardFeaturesPage.SetPersona(personaID);
      this.populateCustomService();
      this.populateStandardWebForm();
      this.PopulatePipelineDefaultOption();
      this.setDirtyFlag(false);
    }

    public void MakeReadOnly(bool readOnly)
    {
      if (this.isStandardFeaturesPageVisible)
        this.loStandardFeaturesPage.MakeReadOnly(readOnly);
      this.disableCheckBoxes = readOnly;
      if (readOnly)
        this.populateCustomService();
      this.disableCheckBoxes = readOnly;
      this.disableCustomFormTool(this.gvCustomForms, readOnly);
      this.disableCustomFormTool(this.gvCustomTool, readOnly);
      this.disableCustomFormTool(this.gvCustomGlobal, readOnly);
      this.disableCustomFormTool(this.gvStandardWebForms, readOnly);
      this.tsMenuItemConnect.Enabled = this.tsMenuItemDisconnect.Enabled = !readOnly;
      this.rdbLoanPipelineOption.Enabled = !readOnly;
      this.rdbTaskPipelineOption.Enabled = !readOnly;
    }

    private void disableCustomFormTool(GridView gv, bool readOnly)
    {
      gv.SuspendLayout();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gv.Items)
        gvItem.SubItems[0].CheckBoxEnabled = !readOnly;
      gv.ResumeLayout();
    }

    public void Save()
    {
      if (this.isStandardFeaturesPageVisible)
      {
        this.loStandardFeaturesPage.SaveData();
        if (this.customFormToolsSettingsDirty)
        {
          Hashtable featureAccesses = new Hashtable();
          featureAccesses.Add((object) AclFeature.LOConnectTab_LoanPipelineOption, (object) (AclTriState) (this.rdbLoanPipelineOption.Checked ? 1 : 0));
          featureAccesses.Add((object) AclFeature.LOConnectTab_TaskPipelineOption, (object) (AclTriState) (this.rdbTaskPipelineOption.Checked ? 1 : 0));
          if (this.personaID > 0)
            this.featuresAclMgr.SetPermissions(featureAccesses, this.personaID);
          else if (this.personaID <= 0 && !string.IsNullOrEmpty(this.userid))
            this.featuresAclMgr.SetPermissions(featureAccesses, this.userid);
        }
      }
      this.saveCustomSettings();
      this.setDirtyFlag(false);
    }

    public void Reset()
    {
      if (this.isStandardFeaturesPageVisible)
        this.loStandardFeaturesPage.ResetData();
      this.populateCustomService();
      this.populateStandardWebForm();
      this.PopulatePipelineDefaultOption();
      this.setDirtyFlag(false);
    }

    public bool IsDirty
    {
      get
      {
        return this.isStandardFeaturesPageVisible && this.loStandardFeaturesPage.NeedToSaveData() || this.customFormToolsSettingsDirty;
      }
    }

    private void gvStandardWebForms_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.bIsUserSetup)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStandardWebForms.Items)
          gvItem.Selected = gvItem.SubItems[0] == e.SubItem;
        this.tsMenuItemDisconnect_Click(source, (EventArgs) e);
      }
      this.setDirtyFlag(true);
    }

    private void gvCustomForms_Click(object sender, EventArgs e)
    {
    }

    private void gvforms_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.bIsUserSetup)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvCustomForms.Items)
          gvItem.Selected = gvItem.SubItems[0] == e.SubItem;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvCustomGlobal.Items)
          gvItem.Selected = gvItem.SubItems[0] == e.SubItem;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvCustomTool.Items)
          gvItem.Selected = gvItem.SubItems[0] == e.SubItem;
        this.tsMenuItemDisconnect_Click(source, (EventArgs) e);
      }
      this.setDirtyFlag(true);
    }

    private void tsMenuItemDisconnect_Click(object sender, EventArgs e)
    {
      this.gvCustomForms.BeginUpdate();
      foreach (GVItem selectedItem in this.gvCustomForms.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (personaLinkImg.IsLinked)
        {
          personaLinkImg.Disconnect();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
        }
      }
      this.gvCustomForms.EndUpdate();
      this.gvCustomGlobal.BeginUpdate();
      foreach (GVItem selectedItem in this.gvCustomGlobal.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (personaLinkImg.IsLinked)
        {
          personaLinkImg.Disconnect();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
        }
      }
      this.gvCustomGlobal.EndUpdate();
      this.gvCustomTool.BeginUpdate();
      foreach (GVItem selectedItem in this.gvCustomTool.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (personaLinkImg.IsLinked)
        {
          personaLinkImg.Disconnect();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
        }
      }
      this.gvCustomTool.EndUpdate();
      this.gvStandardWebForms.BeginUpdate();
      foreach (GVItem selectedItem in this.gvStandardWebForms.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (personaLinkImg.IsLinked)
        {
          personaLinkImg.Disconnect();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
        }
      }
      this.gvStandardWebForms.EndUpdate();
      this.setDirtyFlag(true);
    }

    private void tsMenuItemConnect_Click(object sender, EventArgs e)
    {
      Hashtable permissionByPersonas = this.loConnectAclManager.GetUserPermissionByPersonas(this.personas);
      this.linkCustomServices(this.gvCustomTool, permissionByPersonas);
      this.linkCustomServices(this.gvCustomGlobal, permissionByPersonas);
      this.linkCustomServices(this.gvCustomForms, permissionByPersonas);
      this.setDirtyFlag(true);
    }

    private void linkCustomServices(GridView gvCustomTool, Hashtable personaPerms)
    {
      this.gvCustomTool.BeginUpdate();
      foreach (GVItem selectedItem in this.gvCustomTool.SelectedItems)
      {
        PersonaLinkImg personaLinkImg = (PersonaLinkImg) selectedItem.SubItems[1].Value;
        if (!personaLinkImg.IsLinked)
        {
          LoConnectCustomServiceInfo tag = (LoConnectCustomServiceInfo) selectedItem.Tag;
          personaLinkImg.Link();
          selectedItem.SubItems[1].Value = (object) personaLinkImg;
          IEnumerator enumerator = personaPerms.Keys.GetEnumerator();
          if (!selectedItem.SubItems[0].Checked)
          {
            while (enumerator.MoveNext())
            {
              if (string.Concat(enumerator.Current) == tag.CustomEntityId)
              {
                if ((bool) personaPerms[enumerator.Current])
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
              if (string.Concat(enumerator.Current) == tag.CustomEntityId)
              {
                if (!(bool) personaPerms[enumerator.Current])
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
      this.gvCustomTool.EndUpdate();
    }

    private void LOConnectConfig_SizeChanged(object sender, EventArgs e)
    {
      this.panelEx2.Width = this.Width / 3;
      this.panelEx1.Width = this.Width / 3;
      this.pnlExTools.Width = this.Width / 3;
    }

    private void splitContainer1_Paint(object sender, PaintEventArgs e)
    {
      if (!(sender is SplitContainer splitContainer))
        return;
      e.Graphics.FillRectangle(Brushes.LightGray, splitContainer.SplitterRectangle);
    }

    private void splitContainer2_Paint(object sender, PaintEventArgs e)
    {
      if (!(sender is SplitContainer splitContainer))
        return;
      e.Graphics.FillRectangle(Brushes.LightGray, splitContainer.SplitterRectangle);
    }

    private void PipelineDefault_CheckedChanged(object sender, EventArgs e)
    {
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
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.pnlExTools = new PanelEx();
      this.gcCustomForms = new GroupContainer();
      this.splitContainer2 = new SplitContainer();
      this.gvCustomForms = new GridView();
      this.ctxMenuInputForms = new ContextMenuStrip(this.components);
      this.tsMenuItemConnect = new ToolStripMenuItem();
      this.tsMenuItemDisconnect = new ToolStripMenuItem();
      this.groupContainer3 = new GroupContainer();
      this.gvStandardWebForms = new GridView();
      this.lblNoAccess = new Label();
      this.pnlBase = new Panel();
      this.panelEx2 = new PanelEx();
      this.groupContainer4 = new GroupContainer();
      this.rdbTaskPipelineOption = new RadioButton();
      this.rdbLoanPipelineOption = new RadioButton();
      this.panelEx1 = new PanelEx();
      this.groupContainer1 = new GroupContainer();
      this.splitContainer1 = new SplitContainer();
      this.gvCustomTool = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.gvCustomGlobal = new GridView();
      this.pnlxStandardFeatures = new PanelEx();
      this.pnlExTools.SuspendLayout();
      this.gcCustomForms.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.ctxMenuInputForms.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.panelEx2.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.panelEx1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.pnlExTools.Controls.Add((Control) this.gcCustomForms);
      this.pnlExTools.Dock = DockStyle.Left;
      this.pnlExTools.Location = new Point(0, 0);
      this.pnlExTools.Name = "pnlExTools";
      this.pnlExTools.Size = new Size(238, 455);
      this.pnlExTools.TabIndex = 2;
      this.gcCustomForms.Controls.Add((Control) this.splitContainer2);
      this.gcCustomForms.Dock = DockStyle.Fill;
      this.gcCustomForms.HeaderForeColor = SystemColors.ControlText;
      this.gcCustomForms.ImeMode = ImeMode.KatakanaHalf;
      this.gcCustomForms.Location = new Point(0, 0);
      this.gcCustomForms.Name = "gcCustomForms";
      this.gcCustomForms.Size = new Size(238, 455);
      this.gcCustomForms.TabIndex = 1;
      this.gcCustomForms.Text = "Custom Input Forms";
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(1, 26);
      this.splitContainer2.Margin = new Padding(2);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.AutoScroll = true;
      this.splitContainer2.Panel1.Controls.Add((Control) this.gvCustomForms);
      this.splitContainer2.Panel2.AutoScroll = true;
      this.splitContainer2.Panel2.Controls.Add((Control) this.groupContainer3);
      this.splitContainer2.Size = new Size(236, 428);
      this.splitContainer2.SplitterDistance = 174;
      this.splitContainer2.SplitterWidth = 1;
      this.splitContainer2.TabIndex = 0;
      this.splitContainer2.Paint += new PaintEventHandler(this.splitContainer2_Paint);
      this.gvCustomForms.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colAccessible";
      gvColumn1.Text = "Accessible";
      gvColumn1.Width = 65;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colName";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 225;
      this.gvCustomForms.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvCustomForms.ContextMenuStrip = this.ctxMenuInputForms;
      this.gvCustomForms.Dock = DockStyle.Fill;
      this.gvCustomForms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCustomForms.ImeMode = ImeMode.NoControl;
      this.gvCustomForms.Location = new Point(0, 0);
      this.gvCustomForms.Name = "gvCustomForms";
      this.gvCustomForms.Size = new Size(236, 174);
      this.gvCustomForms.TabIndex = 0;
      this.gvCustomForms.SubItemCheck += new GVSubItemEventHandler(this.gvforms_SubItemCheck);
      this.gvCustomForms.Click += new EventHandler(this.gvCustomForms_Click);
      this.ctxMenuInputForms.ImageScalingSize = new Size(24, 24);
      this.ctxMenuInputForms.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsMenuItemConnect,
        (ToolStripItem) this.tsMenuItemDisconnect
      });
      this.ctxMenuInputForms.Name = "ctxMenuInputForms";
      this.ctxMenuInputForms.Size = new Size(244, 48);
      this.tsMenuItemConnect.Name = "tsMenuItemConnect";
      this.tsMenuItemConnect.Size = new Size(243, 22);
      this.tsMenuItemConnect.Text = "Link with Persona Rights";
      this.tsMenuItemConnect.Click += new EventHandler(this.tsMenuItemConnect_Click);
      this.tsMenuItemDisconnect.Name = "tsMenuItemDisconnect";
      this.tsMenuItemDisconnect.Size = new Size(243, 22);
      this.tsMenuItemDisconnect.Text = "Disconnect from Persona Rights";
      this.tsMenuItemDisconnect.Click += new EventHandler(this.tsMenuItemDisconnect_Click);
      this.groupContainer3.Borders = AnchorStyles.Top;
      this.groupContainer3.Controls.Add((Control) this.gvStandardWebForms);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 0);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(236, 253);
      this.groupContainer3.TabIndex = 1;
      this.groupContainer3.Text = "Standard Web Forms";
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colAccess";
      gvColumn3.Text = "Access";
      gvColumn3.Width = 65;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colName";
      gvColumn4.Text = "Name";
      gvColumn4.Width = 225;
      this.gvStandardWebForms.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvStandardWebForms.Dock = DockStyle.Fill;
      this.gvStandardWebForms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvStandardWebForms.Location = new Point(0, 26);
      this.gvStandardWebForms.Name = "gvStandardWebForms";
      this.gvStandardWebForms.Size = new Size(236, 227);
      this.gvStandardWebForms.TabIndex = 0;
      this.gvStandardWebForms.SubItemCheck += new GVSubItemEventHandler(this.gvStandardWebForms_SubItemCheck);
      this.lblNoAccess.BackColor = Color.Transparent;
      this.lblNoAccess.Dock = DockStyle.Top;
      this.lblNoAccess.Location = new Point(0, 0);
      this.lblNoAccess.Name = "lblNoAccess";
      this.lblNoAccess.Size = new Size(659, 26);
      this.lblNoAccess.TabIndex = 1;
      this.lblNoAccess.Text = "The persona does not have access to the Pipeline, Loan, Forms and Tools, ePass tabs.";
      this.lblNoAccess.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNoAccess.Visible = false;
      this.pnlBase.Controls.Add((Control) this.panelEx2);
      this.pnlBase.Controls.Add((Control) this.panelEx1);
      this.pnlBase.Controls.Add((Control) this.pnlExTools);
      this.pnlBase.Dock = DockStyle.Fill;
      this.pnlBase.Location = new Point(0, 26);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(659, 455);
      this.pnlBase.TabIndex = 2;
      this.panelEx2.Controls.Add((Control) this.pnlxStandardFeatures);
      this.panelEx2.Controls.Add((Control) this.groupContainer4);
      this.panelEx2.Dock = DockStyle.Fill;
      this.panelEx2.Location = new Point(547, 0);
      this.panelEx2.Name = "panelEx2";
      this.panelEx2.Size = new Size(112, 455);
      this.panelEx2.TabIndex = 4;
      this.groupContainer4.Controls.Add((Control) this.rdbTaskPipelineOption);
      this.groupContainer4.Controls.Add((Control) this.rdbLoanPipelineOption);
      this.groupContainer4.Dock = DockStyle.Bottom;
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(0, 268);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(112, 187);
      this.groupContainer4.TabIndex = 3;
      this.groupContainer4.Text = "Pipeline Default";
      this.rdbTaskPipelineOption.AutoSize = true;
      this.rdbTaskPipelineOption.Location = new Point(4, 53);
      this.rdbTaskPipelineOption.Name = "rdbTaskPipelineOption";
      this.rdbTaskPipelineOption.Size = new Size(89, 17);
      this.rdbTaskPipelineOption.TabIndex = 1;
      this.rdbTaskPipelineOption.TabStop = true;
      this.rdbTaskPipelineOption.Text = "Task Pipeline";
      this.rdbTaskPipelineOption.UseVisualStyleBackColor = true;
      this.rdbTaskPipelineOption.CheckedChanged += new EventHandler(this.PipelineDefault_CheckedChanged);
      this.rdbLoanPipelineOption.AutoSize = true;
      this.rdbLoanPipelineOption.Location = new Point(4, 30);
      this.rdbLoanPipelineOption.Name = "rdbLoanPipelineOption";
      this.rdbLoanPipelineOption.Size = new Size(89, 17);
      this.rdbLoanPipelineOption.TabIndex = 0;
      this.rdbLoanPipelineOption.TabStop = true;
      this.rdbLoanPipelineOption.Text = "Loan Pipeline";
      this.rdbLoanPipelineOption.UseVisualStyleBackColor = true;
      this.rdbLoanPipelineOption.CheckedChanged += new EventHandler(this.PipelineDefault_CheckedChanged);
      this.panelEx1.Controls.Add((Control) this.groupContainer1);
      this.panelEx1.Dock = DockStyle.Left;
      this.panelEx1.Location = new Point(238, 0);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(309, 455);
      this.panelEx1.TabIndex = 3;
      this.groupContainer1.Controls.Add((Control) this.splitContainer1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.ImeMode = ImeMode.KatakanaHalf;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(309, 455);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Custom Tools";
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(1, 26);
      this.splitContainer1.Margin = new Padding(2);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.AutoScroll = true;
      this.splitContainer1.Panel1.Controls.Add((Control) this.gvCustomTool);
      this.splitContainer1.Panel2.AutoScroll = true;
      this.splitContainer1.Panel2.Controls.Add((Control) this.groupContainer2);
      this.splitContainer1.Size = new Size(307, 428);
      this.splitContainer1.SplitterDistance = 174;
      this.splitContainer1.SplitterWidth = 1;
      this.splitContainer1.TabIndex = 0;
      this.splitContainer1.Paint += new PaintEventHandler(this.splitContainer1_Paint);
      this.gvCustomTool.BorderStyle = BorderStyle.None;
      gvColumn5.CheckBoxes = true;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colAccessible";
      gvColumn5.Text = "Accessible";
      gvColumn5.Width = 65;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colName";
      gvColumn6.Text = "Name";
      gvColumn6.Width = 225;
      this.gvCustomTool.Columns.AddRange(new GVColumn[2]
      {
        gvColumn5,
        gvColumn6
      });
      this.gvCustomTool.ContextMenuStrip = this.ctxMenuInputForms;
      this.gvCustomTool.Dock = DockStyle.Fill;
      this.gvCustomTool.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCustomTool.ImeMode = ImeMode.NoControl;
      this.gvCustomTool.Location = new Point(0, 0);
      this.gvCustomTool.Name = "gvCustomTool";
      this.gvCustomTool.Size = new Size(307, 174);
      this.gvCustomTool.TabIndex = 0;
      this.gvCustomTool.SubItemCheck += new GVSubItemEventHandler(this.gvforms_SubItemCheck);
      this.groupContainer2.Borders = AnchorStyles.Top;
      this.groupContainer2.Controls.Add((Control) this.gvCustomGlobal);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(307, 253);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "Global Custom Tools";
      this.gvCustomGlobal.BorderColor = Color.Gray;
      this.gvCustomGlobal.BorderStyle = BorderStyle.None;
      gvColumn7.CheckBoxes = true;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colAccessible";
      gvColumn7.Text = "Accessible";
      gvColumn7.Width = 65;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colName";
      gvColumn8.Text = "Name";
      gvColumn8.Width = 225;
      this.gvCustomGlobal.Columns.AddRange(new GVColumn[2]
      {
        gvColumn7,
        gvColumn8
      });
      this.gvCustomGlobal.ContextMenuStrip = this.ctxMenuInputForms;
      this.gvCustomGlobal.Dock = DockStyle.Fill;
      this.gvCustomGlobal.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCustomGlobal.ImeMode = ImeMode.NoControl;
      this.gvCustomGlobal.Location = new Point(0, 26);
      this.gvCustomGlobal.Name = "gvCustomGlobal";
      this.gvCustomGlobal.Size = new Size(307, 227);
      this.gvCustomGlobal.TabIndex = 1;
      this.gvCustomGlobal.SubItemCheck += new GVSubItemEventHandler(this.gvforms_SubItemCheck);
      this.pnlxStandardFeatures.Dock = DockStyle.Fill;
      this.pnlxStandardFeatures.Location = new Point(0, 0);
      this.pnlxStandardFeatures.Name = "pnlxStandardFeatures";
      this.pnlxStandardFeatures.Size = new Size(167, 412);
      this.pnlxStandardFeatures.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.pnlBase);
      this.Controls.Add((Control) this.lblNoAccess);
      this.Name = nameof (LOConnectConfig);
      this.Size = new Size(659, 481);
      this.SizeChanged += new EventHandler(this.LOConnectConfig_SizeChanged);
      this.pnlExTools.ResumeLayout(false);
      this.gcCustomForms.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.ctxMenuInputForms.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.pnlBase.ResumeLayout(false);
      this.panelEx2.ResumeLayout(false);
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.panelEx1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
