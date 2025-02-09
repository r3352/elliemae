// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HomePage
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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class HomePage : Form, IPersonaSecurityPage
  {
    private const string className = "HomePage";
    private static readonly string sw = Tracing.SwEpass;
    private Hashtable htModuleList;
    private string personaName;
    private int personaId;
    private int maxAllowed;
    private const int defaultMaxAllowed = 12;
    private int defaultCount;
    private int accessibleCount;
    private HomePersonaMgr personaMgrPage;
    private bool isUserSetup;
    private Sessions.Session session;
    private IContainer components;
    private Panel panelHomePage;
    private GridView gridView1;
    private Label label2;
    public GradientPanel gradientPanel2;
    private GroupContainer gcHomeModules;
    private Panel pnlPersonaHomeManage;

    public event EventHandler DirtyFlagChanged;

    public HomePage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.personaMgrPage = new HomePersonaMgr(session, personaId, dirtyFlagChanged);
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.gridView1.SubItemCheck += new GVSubItemEventHandler(this.gridView1_SubItemCheck);
      this.initPage(this.isUserSetup, "", (Persona[]) null, personaId, dirtyFlagChanged);
    }

    public HomePage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.personaMgrPage = new HomePersonaMgr(session, userId, personas, dirtyFlagChanged);
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.isUserSetup = true;
      this.gridView1.SubItemCheck += new GVSubItemEventHandler(this.gridView1_SubItemCheck);
      this.initPage(this.isUserSetup, userId, personas, -1, dirtyFlagChanged);
    }

    private void initPage(
      bool isUserSetup,
      string userId,
      Persona[] personas,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      if (isUserSetup)
      {
        this.panelHomePage.Visible = false;
        this.pnlPersonaHomeManage.Dock = DockStyle.Fill;
      }
      else
      {
        this.panelHomePage.Visible = true;
        this.SetPersona(personaId);
      }
      this.personaMgrPage.TopLevel = false;
      this.personaMgrPage.Dock = DockStyle.Fill;
      this.personaMgrPage.Visible = true;
      this.pnlPersonaHomeManage.Controls.Add((Control) this.personaMgrPage);
    }

    public virtual void SetPersona(int personaId)
    {
      try
      {
        this.personaMgrPage.SetPersona(personaId);
        this.gridView1.Enabled = false;
        if (this.gridView1.Items.Count > 0)
          this.gridView1.Items.Clear();
        if (this.htModuleList != null)
          this.htModuleList.Clear();
        this.personaId = personaId;
        this.personaName = this.getPersonaName(personaId);
        this.htModuleList = HomePageSettings.GetModuleSettings(this.session, this.personaName, personaId, out this.maxAllowed);
        if (this.htModuleList == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to get the persona settings:\n\n Modules were not returned from HomePage.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (this.htModuleList.Count == 0)
            return;
          if (this.maxAllowed == 0)
            this.maxAllowed = 12;
          this.defaultCount = 0;
          this.accessibleCount = 0;
          foreach (string key in (IEnumerable) this.htModuleList.Keys)
          {
            HomePageModuleSettings htModule = (HomePageModuleSettings) this.htModuleList[(object) key];
            GVItem gvItem = new GVItem(htModule.Category);
            gvItem.SubItems.Add(new GVSubItem()
            {
              Text = htModule.Title
            });
            if (htModule.IsDefault && this.defaultCount == this.maxAllowed)
              htModule.SetIsDefault(false);
            else if (htModule.IsDefault)
              ++this.defaultCount;
            gvItem.SubItems.Add(new GVSubItem()
            {
              Checked = htModule.IsLocked
            });
            gvItem.SubItems.Add(new GVSubItem()
            {
              Checked = htModule.IsDefault
            });
            gvItem.SubItems.Add(new GVSubItem()
            {
              Checked = htModule.IsAccessible
            });
            if (htModule.IsAccessible)
              ++this.accessibleCount;
            gvItem.SubItems.Add(new GVSubItem()
            {
              Text = htModule.Description
            });
            gvItem.Tag = (object) htModule.ModuleID;
            this.gridView1.Items.Add(gvItem);
          }
          this.gridView1.Sort(1, SortOrder.Ascending);
          this.gridView1.Enabled = true;
          this.gcHomeModules.Text = this.getHeaderText();
          this.label2.Text = this.getSubHeaderText();
          if (this.DirtyFlagChanged == null)
            return;
          this.DirtyFlagChanged((object) this, (EventArgs) null);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePage.sw, TraceLevel.Error, nameof (HomePage), "SetPersona: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to get the persona settings:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void gridView1_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      try
      {
        GVItem parent = e.SubItem.Parent;
        GVSubItem subItem1 = parent.SubItems[2];
        GVSubItem subItem2 = parent.SubItems[3];
        GVSubItem subItem3 = parent.SubItems[4];
        HomePageModuleSettings htModule = (HomePageModuleSettings) this.htModuleList[(object) (string) parent.Tag];
        string text = string.Format("You have already selected the maximum of {0} modules.\r\nYou must deselect other modules to be able to select this module.", (object) this.maxAllowed);
        switch (e.SubItem.Index)
        {
          case 2:
            if (subItem1.Checked && !subItem2.Checked && this.defaultCount == this.maxAllowed)
            {
              int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              subItem1.Checked = false;
            }
            else
            {
              htModule.SetIsLocked(subItem1.Checked);
              subItem2.Checked = htModule.IsDefault;
              subItem3.Checked = htModule.IsAccessible;
            }
            this.setDefaultCount();
            this.gcHomeModules.Text = this.getHeaderText();
            break;
          case 3:
            if (subItem1.Checked)
            {
              subItem2.Checked = true;
              break;
            }
            if (subItem2.Checked && this.defaultCount == this.maxAllowed)
            {
              int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              subItem2.Checked = false;
            }
            else
            {
              htModule.SetIsDefault(subItem2.Checked);
              subItem2.Checked = htModule.IsDefault;
              subItem3.Checked = htModule.IsAccessible;
            }
            this.setDefaultCount();
            this.gcHomeModules.Text = this.getHeaderText();
            break;
          case 4:
            this.setAccessibleCount();
            htModule.SetIsAccessible(subItem3.Checked, this.accessibleCount);
            subItem2.Checked = htModule.IsDefault;
            subItem3.Checked = htModule.IsAccessible;
            break;
          default:
            throw new Exception("Invalid checkbox index: " + (object) e.SubItem.Index);
        }
        if (this.DirtyFlagChanged == null)
          return;
        this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePage.sw, TraceLevel.Error, nameof (HomePage), "SetPersona: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to set the checkbox:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void setAccessibleCount()
    {
      this.accessibleCount = 0;
      for (int nItemIndex = 0; nItemIndex < this.gridView1.Items.Count; ++nItemIndex)
        this.accessibleCount += this.gridView1.Items[nItemIndex].SubItems[4].Checked ? 1 : 0;
    }

    private void setDefaultCount()
    {
      this.defaultCount = 0;
      for (int nItemIndex = 0; nItemIndex < this.gridView1.Items.Count; ++nItemIndex)
        this.defaultCount += this.gridView1.Items[nItemIndex].SubItems[3].Checked ? 1 : 0;
    }

    private string getPersonaName(int personaId)
    {
      string personaName = string.Empty;
      foreach (Persona allPersona in this.session.PersonaManager.GetAllPersonas())
      {
        if (allPersona.ID == personaId)
        {
          personaName = allPersona.Name;
          break;
        }
      }
      return personaName;
    }

    public void SaveData()
    {
      if (!this.NeedToSaveData())
        return;
      this.personaMgrPage.Save();
      if (!this.personaMgrPage.Visible)
        return;
      if (!this.isUserSetup)
        HomePageSettings.SaveModuleSettings(this.session, this.personaName, this.personaId, this.htModuleList);
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void ResetData()
    {
      try
      {
        this.personaMgrPage.Reset();
        if (!this.personaMgrPage.Visible)
          return;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridView1.Items)
        {
          HomePageModuleSettings htModule = (HomePageModuleSettings) this.htModuleList[(object) (string) gvItem.Tag];
          htModule.ResetAll();
          for (int nItemIndex = 2; nItemIndex < 5; ++nItemIndex)
          {
            GVSubItem subItem = gvItem.SubItems[nItemIndex];
            switch (nItemIndex - 2)
            {
              case 0:
                subItem.Checked = htModule.IsLocked;
                break;
              case 1:
                subItem.Checked = htModule.IsDefault;
                break;
              case 2:
                subItem.Checked = htModule.IsAccessible;
                break;
            }
          }
        }
        if (this.DirtyFlagChanged == null)
          return;
        this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePage.sw, TraceLevel.Error, nameof (HomePage), "ResetData: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when resetting the module settings:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public bool NeedToSaveData()
    {
      try
      {
        if (this.personaMgrPage.NeedToSaveData())
          return true;
        if (this.panelHomePage.Visible)
        {
          if (this.htModuleList != null)
          {
            foreach (string key in (IEnumerable) this.htModuleList.Keys)
            {
              if (((HomePageModuleSettings) this.htModuleList[(object) key]).IsModified)
                return true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePage.sw, TraceLevel.Error, nameof (HomePage), "NeedToSaveData: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when checking module settings:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }

    public void MakeReadOnly(bool makeReadonly) => this.personaMgrPage.MakeReadOnly(makeReadonly);

    private string getHeaderText()
    {
      return string.Format("Home Page Modules ({0})      Default Modules ({1})", (object) this.htModuleList.Count, (object) this.defaultCount);
    }

    private string getSubHeaderText()
    {
      return string.Format("Configure must-have modules, default modules ({0} maximum), and accessible modules for the persona.", (object) this.maxAllowed);
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
      GVColumn gvColumn6 = new GVColumn();
      this.panelHomePage = new Panel();
      this.gcHomeModules = new GroupContainer();
      this.gridView1 = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.label2 = new Label();
      this.pnlPersonaHomeManage = new Panel();
      this.panelHomePage.SuspendLayout();
      this.gcHomeModules.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.panelHomePage.Controls.Add((Control) this.gcHomeModules);
      this.panelHomePage.Dock = DockStyle.Fill;
      this.panelHomePage.Location = new Point(0, 0);
      this.panelHomePage.Name = "panelHomePage";
      this.panelHomePage.Size = new Size(639, 416);
      this.panelHomePage.TabIndex = 0;
      this.gcHomeModules.Controls.Add((Control) this.gridView1);
      this.gcHomeModules.Controls.Add((Control) this.gradientPanel2);
      this.gcHomeModules.Dock = DockStyle.Fill;
      this.gcHomeModules.HeaderForeColor = SystemColors.ControlText;
      this.gcHomeModules.Location = new Point(0, 0);
      this.gcHomeModules.Name = "gcHomeModules";
      this.gcHomeModules.Size = new Size(639, 416);
      this.gcHomeModules.TabIndex = 6;
      this.gcHomeModules.Text = "Home Page Modules (x)      Default Modules (x)";
      this.gridView1.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colCategory";
      gvColumn1.Text = "Category";
      gvColumn1.Width = 60;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colModuleName";
      gvColumn2.Text = "Module Name";
      gvColumn2.Width = 200;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colMustHave";
      gvColumn3.Text = "Must Have";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 65;
      gvColumn4.CheckBoxes = true;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colDefault";
      gvColumn4.Text = "Show by Default";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 90;
      gvColumn5.CheckBoxes = true;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colAccessible";
      gvColumn5.Text = "Accessible";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 67;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colDescription";
      gvColumn6.Text = "Module Description";
      gvColumn6.Width = 335;
      this.gridView1.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridView1.Dock = DockStyle.Fill;
      this.gridView1.Location = new Point(1, 60);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(637, 355);
      this.gridView1.TabIndex = 5;
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.White;
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 26);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Padding = new Padding(10, 11, 0, 11);
      this.gradientPanel2.Size = new Size(637, 34);
      this.gradientPanel2.TabIndex = 4;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Dock = DockStyle.Top;
      this.label2.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(10, 11);
      this.label2.Name = "label2";
      this.label2.Size = new Size(500, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Configure must-have modules, default modules (x maximum), and accessible modules for the persona.";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlPersonaHomeManage.Dock = DockStyle.Bottom;
      this.pnlPersonaHomeManage.Location = new Point(0, 416);
      this.pnlPersonaHomeManage.Name = "pnlPersonaHomeManage";
      this.pnlPersonaHomeManage.Padding = new Padding(0, 10, 0, 0);
      this.pnlPersonaHomeManage.Size = new Size(639, 42);
      this.pnlPersonaHomeManage.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(639, 458);
      this.Controls.Add((Control) this.panelHomePage);
      this.Controls.Add((Control) this.pnlPersonaHomeManage);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (HomePage);
      this.Text = "Home Page";
      this.panelHomePage.ResumeLayout(false);
      this.gcHomeModules.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
