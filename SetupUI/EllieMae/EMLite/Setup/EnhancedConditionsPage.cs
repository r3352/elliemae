// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EnhancedConditionsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
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
  public class EnhancedConditionsPage : Form, IPersonaSecurityPage
  {
    private Sessions.Session session;
    private int personaId = -1;
    private bool bIsUserSetup;
    private string userId;
    private Persona[] personas;
    private int gridViewCurrSelectedIdx;
    private bool EnhancedConditionTypeSettingsDirty;
    private PipelineConfiguration pipelineConfiguration;
    private Dictionary<Guid, EnhancedConditionsAccessPage> formLookup;
    private Guid currentEnhancedConditionType;
    private Guid previousEnhancedConditionType;
    private Guid investorDeliveryConditionType;
    private bool isReadOnly;
    protected Hashtable featureToNodeTbl;
    protected Hashtable nodeToFeature;
    protected Hashtable nodeToUpdateStatus;
    protected TreeNode canAccessNode;
    protected TreeNode addConditionsNode;
    protected TreeNode addAutomatedConditionsNode;
    protected TreeNode importConditionsNode;
    protected TreeNode editConditionsNode;
    private IContainer components;
    private Panel conditionTypePnl;
    private Panel accessPnl;
    private GroupContainer groupContainer1;
    private GridView gridView1;
    private Splitter splitter1;

    public event EventHandler DirtyFlagChanged;

    public EnhancedConditionsPage(
      Sessions.Session session,
      int personaID,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.personaId = personaID;
      this.DirtyFlagChanged = dirtyFlagChanged;
      this.pipelineConfiguration = pipelineConfiguration;
      this.InitializeComponent();
      this.init(false, (string) null, (Persona[]) null, this.personaId, dirtyFlagChanged, pipelineConfiguration);
    }

    public EnhancedConditionsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration)
    {
      this.session = session;
      this.userId = userId;
      this.DirtyFlagChanged = dirtyFlagChanged;
      this.pipelineConfiguration = pipelineConfiguration;
      this.InitializeComponent();
      this.init(true, userId, personas, -1, dirtyFlagChanged, (PipelineConfiguration) null);
    }

    public void SetPersona(int personaID)
    {
      if (this.personaId != personaID)
      {
        this.personaId = personaID;
        foreach (Guid key in this.formLookup.Keys.ToList<Guid>())
        {
          EnhancedConditionsAccessPage conditionsAccessPage = this.formLookup[key];
          this.formLookup.Remove(key);
          conditionsAccessPage.Dispose();
        }
        this.formLookup.Clear();
        if (this.gridView1.Items.Count > 0)
        {
          this.gridViewCurrSelectedIdx = 0;
          this.gridView1.Items[this.gridViewCurrSelectedIdx].Selected = true;
          this.currentEnhancedConditionType = Guid.Parse(this.gridView1.Items[this.gridViewCurrSelectedIdx].Tag.ToString());
          this.formLookup[this.currentEnhancedConditionType] = this.createEnhancedConditionsAccessPage(this.session, (string) null, (Persona[]) null, this.personaId, this.currentEnhancedConditionType, this.DirtyFlagChanged, this.pipelineConfiguration);
          this.accessPnl.Controls.Clear();
          this.accessPnl.Controls.Add((Control) this.formLookup[this.currentEnhancedConditionType]);
        }
      }
      this.ResetData();
    }

    public void ResetData()
    {
      foreach (KeyValuePair<Guid, EnhancedConditionsAccessPage> keyValuePair in this.formLookup)
        keyValuePair.Value.ResetData();
    }

    public bool NeedToSaveData()
    {
      foreach (KeyValuePair<Guid, EnhancedConditionsAccessPage> keyValuePair in this.formLookup)
      {
        if (keyValuePair.Value.NeedToSaveData())
          return true;
      }
      return false;
    }

    public void SaveData()
    {
      foreach (KeyValuePair<Guid, EnhancedConditionsAccessPage> keyValuePair in this.formLookup)
      {
        EnhancedConditionsAccessPage conditionsAccessPage = keyValuePair.Value;
        conditionsAccessPage.SaveData();
        conditionsAccessPage.resetDirtyFlag(false);
      }
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      foreach (KeyValuePair<Guid, EnhancedConditionsAccessPage> keyValuePair in this.formLookup)
        keyValuePair.Value.MakeReadOnly(makeReadOnly);
      this.isReadOnly = makeReadOnly;
    }

    private void init(
      bool isUserSetup,
      string userId,
      Persona[] personas,
      int personaId,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration)
    {
      this.formLookup = new Dictionary<Guid, EnhancedConditionsAccessPage>();
      this.populateConditions();
      this.bIsUserSetup = isUserSetup;
      this.userId = userId;
      this.personas = personas;
      if (!isUserSetup)
      {
        if (this.gridView1.Items.Count == 0)
        {
          this.currentEnhancedConditionType = Guid.NewGuid();
        }
        else
        {
          this.gridView1.Items[this.gridViewCurrSelectedIdx].Selected = true;
          this.currentEnhancedConditionType = Guid.Parse(this.gridView1.Items[this.gridViewCurrSelectedIdx].Tag.ToString());
        }
        if (this.gridView1.Items.Count == 0)
        {
          EnhancedConditionsAccessPage conditionsAccessPage = this.createEnhancedConditionsAccessPage(this.session, (string) null, (Persona[]) null, personaId, this.currentEnhancedConditionType, dirtyFlagChanged, pipelineConfiguration);
          conditionsAccessPage.Enabled = false;
          this.accessPnl.Controls.Add((Control) conditionsAccessPage);
        }
        else
          this.accessPnl.Controls.Add((Control) this.formLookup[this.currentEnhancedConditionType]);
      }
      else
      {
        if (this.gridView1.Items.Count == 0)
        {
          this.currentEnhancedConditionType = Guid.NewGuid();
        }
        else
        {
          this.gridView1.Items[this.gridViewCurrSelectedIdx].Selected = true;
          this.currentEnhancedConditionType = Guid.Parse(this.gridView1.Items[this.gridViewCurrSelectedIdx].Tag.ToString());
        }
        if (this.gridView1.Items.Count == 0)
        {
          EnhancedConditionsAccessPage conditionsAccessPage = this.createEnhancedConditionsAccessPage(this.session, this.userId, personas, -1, this.currentEnhancedConditionType, dirtyFlagChanged, pipelineConfiguration);
          conditionsAccessPage.Enabled = false;
          this.accessPnl.Controls.Add((Control) conditionsAccessPage);
        }
        else
          this.accessPnl.Controls.Add((Control) this.formLookup[this.currentEnhancedConditionType]);
      }
    }

    private void populateConditions()
    {
      try
      {
        foreach (EnhancedConditionType enhancedConditionType in EnhancedConditionRestApiHelper.GetEnhancedConditionTypes())
        {
          this.gridView1.Items.Add(enhancedConditionType.title).Tag = (object) enhancedConditionType.id;
          if (enhancedConditionType.title.Equals("Investor Delivery"))
            this.investorDeliveryConditionType = Guid.Parse(enhancedConditionType.id);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error retrieving Condition Types. " + ex.Message);
      }
    }

    public EnhancedConditionsAccessPage createEnhancedConditionsAccessPage(
      Sessions.Session session,
      string userid,
      Persona[] peronas,
      int personaId,
      Guid currentEnhancedConditionType,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration)
    {
      bool isInvestorDelivery = false;
      Guid deliveryConditionType = this.investorDeliveryConditionType;
      if (currentEnhancedConditionType.Equals(this.investorDeliveryConditionType))
        isInvestorDelivery = true;
      EnhancedConditionsAccessPage conditionsAccessPage = this.bIsUserSetup ? new EnhancedConditionsAccessPage(session, this.userId, this.personas, currentEnhancedConditionType, dirtyFlagChanged, pipelineConfiguration, isInvestorDelivery) : new EnhancedConditionsAccessPage(session, personaId, currentEnhancedConditionType, dirtyFlagChanged, pipelineConfiguration, isInvestorDelivery);
      conditionsAccessPage.TopLevel = false;
      conditionsAccessPage.ShowGroupContainer = false;
      conditionsAccessPage.Visible = true;
      conditionsAccessPage.Dock = DockStyle.Fill;
      return conditionsAccessPage;
    }

    public bool IsDirty
    {
      get
      {
        return this.formLookup[this.currentEnhancedConditionType].NeedToSaveData() || this.EnhancedConditionTypeSettingsDirty;
      }
    }

    private void setDirtyFlag(bool val)
    {
      this.EnhancedConditionTypeSettingsDirty = val;
      this.formLookup[this.currentEnhancedConditionType].resetDirtyFlag(val);
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, this.IsDirty ? new EventArgs() : (EventArgs) null);
    }

    private void gridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Count == 0)
        return;
      this.previousEnhancedConditionType = this.currentEnhancedConditionType;
      this.currentEnhancedConditionType = Guid.Parse(this.gridView1.SelectedItems[0].Tag.ToString());
      if (this.formLookup.ContainsKey(this.currentEnhancedConditionType))
      {
        this.accessPnl.Controls.Clear();
        this.accessPnl.Controls.Add((Control) this.formLookup[this.currentEnhancedConditionType]);
      }
      else
      {
        EnhancedConditionsAccessPage conditionsAccessPage = this.bIsUserSetup ? this.createEnhancedConditionsAccessPage(this.session, this.userId, this.personas, -1, this.currentEnhancedConditionType, this.DirtyFlagChanged, this.pipelineConfiguration) : this.createEnhancedConditionsAccessPage(this.session, (string) null, (Persona[]) null, this.personaId, this.currentEnhancedConditionType, this.DirtyFlagChanged, this.pipelineConfiguration);
        this.formLookup.Add(this.currentEnhancedConditionType, conditionsAccessPage);
        if (this.bIsUserSetup)
          this.MakeReadOnly(this.isReadOnly);
        this.accessPnl.Controls.Clear();
        this.accessPnl.Controls.Add((Control) conditionsAccessPage);
      }
      this.setDirtyFlag(this.IsDirty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.conditionTypePnl = new Panel();
      this.gridView1 = new GridView();
      this.accessPnl = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.splitter1 = new Splitter();
      this.conditionTypePnl.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.conditionTypePnl.BackColor = SystemColors.Window;
      this.conditionTypePnl.BorderStyle = BorderStyle.FixedSingle;
      this.conditionTypePnl.Controls.Add((Control) this.gridView1);
      this.conditionTypePnl.Dock = DockStyle.Left;
      this.conditionTypePnl.Location = new Point(1, 26);
      this.conditionTypePnl.Margin = new Padding(2);
      this.conditionTypePnl.Name = "conditionTypePnl";
      this.conditionTypePnl.Size = new Size(245, 265);
      this.conditionTypePnl.TabIndex = 0;
      this.gridView1.AllowMultiselect = false;
      this.gridView1.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Condition Types";
      gvColumn.Width = 240;
      this.gridView1.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridView1.Dock = DockStyle.Fill;
      this.gridView1.GridLines = GVGridLines.None;
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(0, 0);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(243, 263);
      this.gridView1.TabIndex = 13;
      this.gridView1.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.accessPnl.Dock = DockStyle.Fill;
      this.accessPnl.Location = new Point(246, 26);
      this.accessPnl.Name = "accessPnl";
      this.accessPnl.Size = new Size(286, 265);
      this.accessPnl.TabIndex = 1;
      this.groupContainer1.Controls.Add((Control) this.splitter1);
      this.groupContainer1.Controls.Add((Control) this.accessPnl);
      this.groupContainer1.Controls.Add((Control) this.conditionTypePnl);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(533, 292);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Condition Type Settings";
      this.splitter1.Location = new Point(246, 26);
      this.splitter1.Margin = new Padding(2);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(2, 265);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(533, 292);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (EnhancedConditionsPage);
      this.Text = "Enhanced Conditions";
      this.conditionTypePnl.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
