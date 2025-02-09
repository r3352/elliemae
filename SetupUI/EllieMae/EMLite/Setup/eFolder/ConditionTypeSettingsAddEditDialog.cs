// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionTypeSettingsAddEditDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionTypeSettingsAddEditDialog : Form
  {
    private const string className = "ConditionTypeSettingsAddEditDialog";
    private static readonly string sw = Tracing.SwDataEngine;
    public string ID = string.Empty;
    public string Title = string.Empty;
    public EnhancedConditionType conditionType;
    private Sessions.Session session;
    private EnhancedConditionType conditionTypeDetail;
    private bool isAdd;
    private List<string> defaultTrackingList;
    private bool isCustomized;
    private readonly HashSet<string> ExistingTypeNames;
    private Dictionary<string, string> rolesIDWithName = new Dictionary<string, string>();
    private RoleInfo[] rolesInfo;
    private GridView tempGrid;
    private string caption;
    private bool trackingOption;
    private string dialogTitle = string.Empty;
    private bool isDirty;
    private bool isCancelHit;
    private IContainer components;
    private Button btnCancel;
    private Button btnOk;
    private ToolTip tooltip;
    private Panel pnltop;
    private Panel pnlbottom;
    private GroupContainer gcCategory;
    private Label lbltypecount;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnUpCategory;
    private StandardIconButton btnAddCategory;
    private GridView gvCategory;
    private TextBox txtTitle;
    private Label label1;
    private StandardIconButton btnDeleteCategory;
    private StandardIconButton btnDownCategory;
    private GroupContainer gcPriorTo;
    private Label label2;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnDeletePriorTo;
    private StandardIconButton btnDownPriorTo;
    private StandardIconButton btnUpPriorTo;
    private StandardIconButton btnAddPriorTo;
    private GridView gvPriorTo;
    private GroupContainer gcSource;
    private Label label3;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnDeleteSource;
    private StandardIconButton btnDownSource;
    private StandardIconButton btnUpSource;
    private StandardIconButton btnAddSource;
    private GridView gvSource;
    private GroupContainer gcTrackingOwner;
    private Label label6;
    private GridView gvTrackingOwner;
    private GroupContainer gcTrackingOptions;
    private Label label5;
    private FlowLayoutPanel flowLayoutPanel4;
    private StandardIconButton btnDeleteTrackingOptions;
    private StandardIconButton btnDownTrackingOptions;
    private StandardIconButton btnUpTrackingOptions;
    private StandardIconButton btnAddTrackingOptions;
    private GridView gvTrackingOptions;
    private GroupContainer gcRecipient;
    private Label label4;
    private FlowLayoutPanel flowLayoutPanel3;
    private StandardIconButton btnDeleteRecipient;
    private StandardIconButton btnDownRecipient;
    private StandardIconButton btnUpRecipient;
    private StandardIconButton btnAddRecipient;
    private GridView gvRecipient;
    private Label label7;

    public bool IsDuplicateName(string title) => this.ExistingTypeNames.Contains(title);

    public bool IsReservedName(string title)
    {
      return "Investor Delivery".Equals(title, StringComparison.OrdinalIgnoreCase);
    }

    public ConditionDefinitionContract CustomizedConditionDefinition { get; set; }

    public ConditionTypeSettingsAddEditDialog(
      Sessions.Session session,
      EnhancedConditionType conditionTypeDetails = null,
      bool isCustomized = false)
    {
      this.InitializeComponent();
      this.session = session;
      this.isCustomized = isCustomized;
      this.conditionTypeDetail = conditionTypeDetails;
      this.isAdd = conditionTypeDetails == null;
      this.setupData();
      this.enableDisableControl();
    }

    public ConditionTypeSettingsAddEditDialog(
      Sessions.Session session,
      IList<string> existingTypeNames)
      : this(session)
    {
      this.ExistingTypeNames = new HashSet<string>((IEnumerable<string>) existingTypeNames, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private void enableDisableControl()
    {
      this.gvCategory_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gvPriorTo_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gvSource_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gvRecipient_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gvTrackingOptions_SelectedIndexChanged((object) null, (EventArgs) null);
      this.txtTitle.Enabled = this.isAdd;
      this.gvTrackingOptions.SubItemCheck += new GVSubItemEventHandler(this.gvTrackingOptions_SubItemCheck);
      this.gvTrackingOwner.SubItemCheck += new GVSubItemEventHandler(this.gvTrackingOwner_SubItemCheck);
      if (!this.isCustomized)
        return;
      this.pnltop.Visible = false;
      this.pnlbottom.Location = this.pnltop.Location;
      this.Size = new Size(this.Size.Width, this.Size.Height - 35);
      this.Text = "Customize Custom Type Settings";
    }

    private void setupData()
    {
      this.defaultTrackingList = Utils.GetEnhanceConditionsDefaultTrackingOptions();
      if (this.isAdd)
      {
        List<string> conditionsTrackingOptions = Utils.GetEnhanceConditionsTrackingOptions();
        this.setGridData(this.gvCategory, (IEnumerable<string>) Utils.GetEnhanceConditionsCategoryOptions());
        this.setGridData(this.gvPriorTo, (IEnumerable<string>) Utils.GetEnhanceConditionsPriorToOptions());
        this.setGridData(this.gvRecipient, (IEnumerable<string>) Utils.GetEnhanceConditionsRecipientOptions());
        this.setGridData(this.gvSource, (IEnumerable<string>) Utils.GetEnhanceConditionsSourceOptions());
        this.setGridData(this.gvTrackingOptions, (IEnumerable<string>) this.defaultTrackingList, true);
        this.setGridData(this.gvTrackingOptions, (IEnumerable<string>) conditionsTrackingOptions);
        this.loadRoles(conditionsTrackingOptions);
      }
      else
      {
        this.txtTitle.Text = this.conditionTypeDetail.title;
        this.txtTitle.Tag = (object) this.conditionTypeDetail.id;
        this.Text = "Edit a Condition Type Setting";
        if (this.conditionTypeDetail.definitions == null)
        {
          try
          {
            this.conditionTypeDetail = EnhancedConditionRestApiHelper.GetConditionTypeById(this.conditionTypeDetail.id);
            this.setGridData();
          }
          catch (Exception ex)
          {
            Tracing.Log(ConditionTypeSettingsAddEditDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsAddEditDialog), "Error in retrieving condition types, " + ex.Message);
            int num = (int) Utils.Dialog((IWin32Window) this, "Error in retrieving condition types, " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else if (this.isCustomized)
        {
          this.setGridData(this.gvTrackingOptions, (IEnumerable<string>) this.defaultTrackingList, true);
          this.setGridData();
        }
      }
      this.handleDefaultTrackingOptions(this.gvTrackingOptions);
    }

    private void setGridData()
    {
      ConditionDefinitionContract definitions = this.conditionTypeDetail?.definitions;
      this.setGridData(this.gvCategory, (IEnumerable<OptionDefinitionContract>) definitions.categoryDefinitions);
      this.setGridData(this.gvPriorTo, (IEnumerable<OptionDefinitionContract>) definitions.priorToDefinitions);
      this.setGridData(this.gvRecipient, (IEnumerable<OptionDefinitionContract>) definitions.recipientDefinitions);
      this.setGridData(this.gvSource, (IEnumerable<OptionDefinitionContract>) definitions.sourceDefinitions);
      this.setGridData(this.gvTrackingOptions);
    }

    private void handleDefaultTrackingOptions(GridView gv)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gv.Items)
      {
        if (this.defaultTrackingList.Contains(gvItem.Text))
        {
          gvItem.SubItems[1].Checked = true;
          gvItem.SubItems[1].CheckBoxEnabled = false;
          gvItem.ForeColor = Color.Gray;
        }
      }
    }

    private void setGridData(GridView gv, IEnumerable<OptionDefinitionContract> options)
    {
      if (options == null)
        return;
      this.setGridData(gv, options.Select<OptionDefinitionContract, string>((Func<OptionDefinitionContract, string>) (o => o.name)));
    }

    private void setGridData(GridView gv, IEnumerable<string> collections = null, bool isChecked = false)
    {
      if (collections != null)
      {
        foreach (string collection in collections)
          gv.Items.Add(collection);
      }
      else
      {
        if (this.conditionTypeDetail.definitions == null || this.conditionTypeDetail.definitions.trackingDefinitions == null)
          return;
        foreach (TrackingDefinitionContract trackingDefinition in this.conditionTypeDetail.definitions.trackingDefinitions)
        {
          if (!this.isCustomized || gv.Items.FindNextItemIndex(-1, 0, trackingDefinition.name) <= -1)
          {
            GVItem gvItem = new GVItem(trackingDefinition.name);
            GVSubItem gvSubItem = new GVSubItem()
            {
              CheckBoxEnabled = true,
              Checked = isChecked || trackingDefinition.open
            };
            gvItem.SubItems.Add(gvSubItem);
            gv.Items.Add(gvItem);
          }
        }
        this.loadRoles();
      }
    }

    private void loadRoles(List<string> ecTrackingOptions = null)
    {
      if (this.rolesInfo == null || ((IEnumerable<RoleInfo>) this.rolesInfo).Count<RoleInfo>() <= 0)
        this.rolesInfo = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      foreach (RoleInfo roleInfo in this.rolesInfo)
      {
        this.gvTrackingOwner.Items.Add(new GVItem(roleInfo.Name, (object) roleInfo.RoleID));
        this.rolesIDWithName.Add(roleInfo.RoleName, roleInfo.RoleID.ToString());
      }
      if (ecTrackingOptions != null)
      {
        foreach (string ecTrackingOption in ecTrackingOptions)
          this.gvTrackingOwner.Columns.Add(new GVColumn(ecTrackingOption)
          {
            CheckBoxes = true,
            SortMethod = GVSortMethod.Checkbox
          });
      }
      else
      {
        if (this.conditionTypeDetail.definitions == null || this.conditionTypeDetail.definitions.trackingDefinitions == null)
          return;
        foreach (TrackingDefinitionContract trackingDefinition in this.conditionTypeDetail.definitions.trackingDefinitions)
        {
          if (!this.defaultTrackingList.Contains(trackingDefinition.name))
          {
            this.gvTrackingOwner.Columns.Add(new GVColumn(trackingDefinition.name)
            {
              CheckBoxes = true,
              SortMethod = GVSortMethod.Checkbox
            });
            if (trackingDefinition.roles != null)
            {
              foreach (EntityReferenceContract role in trackingDefinition.roles)
              {
                foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrackingOwner.Items)
                {
                  if (gvItem.SubItems[0].Item.Tag.ToString() == role.entityId)
                    gvItem.SubItems[trackingDefinition.name].Checked = true;
                }
              }
            }
          }
        }
      }
    }

    private void updateRoles()
    {
      this.gvTrackingOwner.Columns.Add(new GVColumn(this.gvTrackingOptions.Items[this.gvTrackingOptions.Items.Count - 1].SubItems[0].Text)
      {
        CheckBoxes = true,
        SortMethod = GVSortMethod.Checkbox
      });
    }

    private void updateTrackingOwnersGrid()
    {
      foreach (TrackingDefinitionContract trackingDefinition in this.conditionTypeDetail.definitions.trackingDefinitions)
      {
        foreach (EntityReferenceContract role in trackingDefinition.roles)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrackingOwner.Items)
          {
            if (gvItem.SubItems[0].Item.Tag.ToString() == role.entityId && this.gvTrackingOwner.Columns.Contains(new GVColumn(trackingDefinition.name)))
              gvItem.SubItems[trackingDefinition.name].Checked = true;
          }
        }
      }
    }

    private void deleteRoles(string columnName)
    {
      List<List<bool>> boolListList = new List<List<bool>>();
      int index1 = this.gvTrackingOwner.Columns.GetColumnByName(columnName).Index;
      if (index1 <= 0 || index1 >= this.gvTrackingOwner.Columns.Count)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrackingOwner.Items)
      {
        List<bool> boolList = new List<bool>();
        for (int nItemIndex = index1 + 1; nItemIndex < this.gvTrackingOwner.Columns.Count; ++nItemIndex)
          boolList.Add(gvItem.SubItems[nItemIndex].Checked);
        boolListList.Add(boolList);
      }
      this.gvTrackingOwner.Columns.Remove(this.gvTrackingOwner.Columns[index1]);
      for (int index2 = 0; index2 < boolListList.Count; ++index2)
      {
        int num = 0;
        for (int nItemIndex = index1; nItemIndex < this.gvTrackingOwner.Columns.Count; ++nItemIndex)
        {
          if (index2 < this.gvTrackingOwner.Items.Count)
            this.gvTrackingOwner.Items[index2].SubItems[nItemIndex].Checked = boolListList[index2][num++];
        }
      }
    }

    private OptionDefinitionContract[] setAddContract(GridView gridView)
    {
      OptionDefinitionContract[] definitionContractArray = new OptionDefinitionContract[gridView.Items.Count];
      int num = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gridView.Items)
        definitionContractArray[num++] = new OptionDefinitionContract()
        {
          name = gvItem.Text
        };
      return definitionContractArray;
    }

    private TrackingDefinitionContract[] setAddContractForTracking()
    {
      int num = 0;
      TrackingDefinitionContract[] definitionContractArray = new TrackingDefinitionContract[this.gvTrackingOptions.Items.Count - this.defaultTrackingList.Count];
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrackingOptions.Items)
      {
        if (!this.defaultTrackingList.Contains(gvItem.SubItems[0].Text))
          definitionContractArray[num++] = new TrackingDefinitionContract()
          {
            name = gvItem.SubItems[0].Text,
            open = gvItem.SubItems[1].Checked
          };
      }
      foreach (TrackingDefinitionContract definitionContract in definitionContractArray)
      {
        List<EntityReferenceContract> referenceContractList = new List<EntityReferenceContract>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTrackingOwner.Items)
        {
          if (gvItem.SubItems[definitionContract.name].Checked)
            referenceContractList.Add(new EntityReferenceContract()
            {
              entityId = this.rolesIDWithName[gvItem.SubItems[0].Text]
            });
        }
        definitionContract.roles = referenceContractList.ToArray();
      }
      return definitionContractArray;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string title = this.txtTitle.Text.Trim();
      ConditionDefinitionContract definitionContract = new ConditionDefinitionContract();
      definitionContract.categoryDefinitions = this.setAddContract(this.gvCategory);
      definitionContract.priorToDefinitions = this.setAddContract(this.gvPriorTo);
      definitionContract.sourceDefinitions = this.setAddContract(this.gvSource);
      definitionContract.recipientDefinitions = this.setAddContract(this.gvRecipient);
      definitionContract.trackingDefinitions = this.setAddContractForTracking();
      if (this.isCustomized)
      {
        this.CustomizedConditionDefinition = definitionContract;
        this.isDirty = false;
        this.DialogResult = DialogResult.OK;
      }
      else if (string.IsNullOrEmpty(title))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please provide a Condition Type name", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          if (this.isAdd)
          {
            if (this.IsReservedName(title))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "This is a system reserved condition type. Please select another name for your condition type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (this.IsDuplicateName(title))
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Condition Type already exists. Please provide a unique Name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            ConditionTypeContract conditionType = new ConditionTypeContract()
            {
              title = this.txtTitle.Text,
              definitions = definitionContract
            };
            try
            {
              this.conditionType = EnhancedConditionRestApiHelper.AddEnhancedConditionTypes(conditionType)[0];
              this.conditionType.definitions = (ConditionDefinitionContract) null;
            }
            catch (Exception ex)
            {
              Tracing.Log(ConditionTypeSettingsAddEditDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsAddEditDialog), "Error in adding enhanced condition types, " + ex.InnerException.Message);
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Error in adding enhanced condition types, " + ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          else
          {
            ConditionTypeContract conditionType = new ConditionTypeContract()
            {
              id = this.txtTitle.Tag.ToString(),
              definitions = definitionContract
            };
            try
            {
              EnhancedConditionRestApiHelper.UpdateConditionTypes((object) conditionType);
            }
            catch (Exception ex)
            {
              Tracing.Log(ConditionTypeSettingsAddEditDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsAddEditDialog), "Error in updating enhanced condition types, " + ex.InnerException.Message);
              int num5 = (int) Utils.Dialog((IWin32Window) this, "Error in updating enhanced condition types, " + ex.InnerException.Message);
            }
          }
          this.isDirty = false;
          this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
          int num6 = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message);
        }
      }
    }

    private void btnAddCategory_Click(object sender, EventArgs e)
    {
      StandardIconButton sib = (StandardIconButton) sender;
      this.caption = string.Empty;
      this.trackingOption = false;
      this.setGridView(sib);
      this.add(this.tempGrid);
      this.isDirty = true;
    }

    private GridView setGridView(StandardIconButton sib)
    {
      switch (sib.Name)
      {
        case "btnAddCategory":
          this.caption = "Add Category Option";
          this.tempGrid = this.gvCategory;
          break;
        case "btnAddPriorTo":
          this.caption = "Add Prior To Option";
          this.tempGrid = this.gvPriorTo;
          break;
        case "btnAddRecipient":
          this.caption = "Add Recipient Option";
          this.tempGrid = this.gvRecipient;
          break;
        case "btnAddSource":
          this.caption = "Add Source Option";
          this.tempGrid = this.gvSource;
          break;
        case "btnAddTrackingOptions":
          this.caption = "Add Tracking Option";
          this.trackingOption = true;
          this.tempGrid = this.gvTrackingOptions;
          break;
        case "btnDeleteCategory":
          this.dialogTitle = "Category";
          this.tempGrid = this.gvCategory;
          break;
        case "btnDeletePriorTo":
          this.dialogTitle = "Prior To";
          this.tempGrid = this.gvPriorTo;
          break;
        case "btnDeleteRecipient":
          this.dialogTitle = "Recipient";
          this.tempGrid = this.gvRecipient;
          break;
        case "btnDeleteSource":
          this.dialogTitle = "Source";
          this.tempGrid = this.gvSource;
          break;
        case "btnDeleteTrackingOptions":
          this.dialogTitle = "Tracking Options";
          this.tempGrid = this.gvTrackingOptions;
          break;
        case "btnDownCategory":
        case "btnUpCategory":
          this.tempGrid = this.gvCategory;
          break;
        case "btnDownPriorTo":
        case "btnUpPriorTo":
          this.tempGrid = this.gvPriorTo;
          break;
        case "btnDownRecipient":
        case "btnUpRecipient":
          this.tempGrid = this.gvRecipient;
          break;
        case "btnDownSource":
        case "btnUpSource":
          this.tempGrid = this.gvSource;
          break;
        case "btnDownTrackingOptions":
        case "btnUpTrackingOptions":
          this.tempGrid = this.gvTrackingOptions;
          break;
      }
      return this.tempGrid;
    }

    private void add(GridView gv)
    {
      Dictionary<string, int> keyValues = new Dictionary<string, int>();
      int num = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gv.Items)
        keyValues.Add(gvItem.SubItems[0].Text.Trim().ToLower(), ++num);
      using (ConditionTypeOptionDialog typeOptionDialog = new ConditionTypeOptionDialog(this.caption, this.trackingOption, keyValues))
      {
        if (typeOptionDialog.ShowDialog() != DialogResult.OK)
          return;
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = typeOptionDialog.option.Trim();
        if (this.trackingOption)
          gvItem.SubItems[1].Checked = typeOptionDialog.IsTrackingOption;
        this.DeSelectGridItems(this.gvCategory);
        this.DeSelectGridItems(this.gvPriorTo);
        this.DeSelectGridItems(this.gvSource);
        this.DeSelectGridItems(this.gvRecipient);
        this.DeSelectGridItems(this.gvTrackingOptions);
        gv.Items.Add(gvItem);
        if (gv.Items != null && gv.Items.Count > 0)
          gv.Items[gv.Items.Count - 1].Selected = true;
        if (!this.trackingOption)
          return;
        this.updateRoles();
      }
    }

    private void btnDeleteCategory_Click(object sender, EventArgs e)
    {
      GridView gv = this.setGridView((StandardIconButton) sender);
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this " + this.dialogTitle + " option?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.delete(gv);
      this.isDirty = true;
    }

    private void delete(GridView gv)
    {
      if (gv.SelectedItems.Count <= 0)
        return;
      foreach (GVItem selectedItem in gv.SelectedItems)
      {
        gv.Items.RemoveAt(selectedItem.Index);
        int num = selectedItem.Index + 1 - this.defaultTrackingList.Count;
        if (this.dialogTitle == "Tracking Options")
          this.deleteRoles(selectedItem.Text);
      }
    }

    private void btnUpCategory_Click(object sender, EventArgs e)
    {
      this.moveUp(this.setGridView((StandardIconButton) sender));
    }

    private void moveUp(GridView gv)
    {
      int count = gv.SelectedItems.Count;
      if (gv.SelectedItems.Count != 1)
        return;
      if (gv.Name != "gvTrackingOptions")
      {
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        foreach (GVItem selectedItem in gv.SelectedItems)
          dictionary.Add(selectedItem.Index, selectedItem.Text);
        int num = gv.SelectedItems[0].Index - 1;
        if (num < 0 || this.defaultTrackingList.Contains(gv.Items[num].Text))
          return;
        dictionary.Add(num, gv.Items[num].Text);
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          gv.Items.RemoveAt(num);
          gv.Items.Insert(num++, keyValuePair.Value);
        }
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          if (count != 0)
          {
            gv.Items[keyValuePair.Key - 1].Selected = true;
            --count;
          }
          else
            break;
        }
      }
      else
      {
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        foreach (GVItem selectedItem in gv.SelectedItems)
          dictionary.Add(selectedItem.Index, selectedItem.Text + "|" + selectedItem.SubItems[1].Checked.ToString());
        int num = gv.SelectedItems[0].Index - 1;
        if (num < 0 || this.defaultTrackingList.Contains(gv.Items[num].Text))
          return;
        dictionary.Add(num, gv.Items[num].Text + "|" + gv.Items[num].SubItems[1].Checked.ToString());
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          string[] strArray = keyValuePair.Value.ToString().Split('|');
          gv.Items.RemoveAt(num);
          gv.Items.Insert(num++, new GVItem()
          {
            SubItems = {
              (object) strArray[0],
              new GVSubItem()
              {
                CheckBoxEnabled = true,
                Checked = Convert.ToBoolean(strArray[1])
              }
            }
          });
        }
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          if (count != 0)
          {
            gv.Items[keyValuePair.Key - 1].Selected = true;
            --count;
          }
          else
            break;
        }
      }
      this.isDirty = true;
    }

    private void btnDownCategory_Click(object sender, EventArgs e)
    {
      this.moveDown(this.setGridView((StandardIconButton) sender));
    }

    private void moveDown(GridView gv)
    {
      if (gv.SelectedItems.Count != 1)
        return;
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      int num1 = gv.SelectedItems[gv.SelectedItems.Count - 1].Index + 1;
      if (num1 >= gv.Items.Count)
        return;
      dictionary.Add(num1, gv.Items[num1].Text + "|" + gv.Items[num1].SubItems[1].Checked.ToString());
      foreach (GVItem selectedItem in gv.SelectedItems)
        dictionary.Add(selectedItem.Index, selectedItem.Text + "|" + selectedItem.SubItems[1].Checked.ToString());
      int index = gv.SelectedItems[0].Index;
      foreach (KeyValuePair<int, string> keyValuePair in dictionary)
      {
        string[] strArray = keyValuePair.Value.ToString().Split('|');
        gv.Items.RemoveAt(index);
        gv.Items.Insert(index++, new GVItem()
        {
          SubItems = {
            (object) strArray[0],
            new GVSubItem()
            {
              CheckBoxEnabled = true,
              Checked = Convert.ToBoolean(strArray[1])
            }
          }
        });
      }
      int num2 = 0;
      bool flag = false;
      foreach (KeyValuePair<int, string> keyValuePair in dictionary)
      {
        if (num2++ != 0)
        {
          gv.Items[keyValuePair.Key + 1].Selected = true;
          flag = true;
        }
      }
      if (flag)
        this.setbuttonstatus(gv.Name);
      this.isDirty = true;
    }

    private void setbuttonstatus(string name)
    {
      switch (name)
      {
        case "gvCategory":
          this.gvCategory_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
        case "gvPriorTo":
          this.gvPriorTo_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
        case "gvSource":
          this.gvSource_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
        case "gvRecipient":
          this.gvRecipient_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
        case "gvTrackingOptions":
          this.gvTrackingOptions_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
      }
    }

    private void gvCategory_SubItemMove(object source, GVSubItemMouseEventArgs e)
    {
    }

    private void gvCategory_Leave(object sender, EventArgs e)
    {
    }

    private void OnSelectedIndexChanged(
      GridView gv,
      IconButton btnUp,
      IconButton btnDown,
      IconButton btnAdd,
      IconButton btnDelete)
    {
      bool flag1 = this.isAdd || !this.IsReservedName(this.txtTitle.Text) || gv != this.gvTrackingOptions;
      bool flag2 = flag1 && gv.SelectedItems.Count > 0;
      int num = flag2 ? gv.SelectedItems.First<GVItem>().DisplayIndex : -1;
      btnAdd.Enabled = flag1;
      btnUp.Enabled = flag2 && num > 0;
      btnDown.Enabled = flag2 && num < gv.Items.Count - 1;
      btnDelete.Enabled = flag2 && num > -1;
    }

    private void gvCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(this.gvCategory, (IconButton) this.btnUpCategory, (IconButton) this.btnDownCategory, (IconButton) this.btnAddCategory, (IconButton) this.btnDeleteCategory);
    }

    private void gvPriorTo_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(this.gvPriorTo, (IconButton) this.btnUpPriorTo, (IconButton) this.btnDownPriorTo, (IconButton) this.btnAddPriorTo, (IconButton) this.btnDeletePriorTo);
    }

    private void gvSource_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(this.gvSource, (IconButton) this.btnUpSource, (IconButton) this.btnDownSource, (IconButton) this.btnAddSource, (IconButton) this.btnDeleteSource);
    }

    private void gvRecipient_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(this.gvRecipient, (IconButton) this.btnUpRecipient, (IconButton) this.btnDownRecipient, (IconButton) this.btnAddRecipient, (IconButton) this.btnDeleteRecipient);
    }

    private void gvTrackingOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvTrackingOptions.SelectedItems.Count > 0)
      {
        GVItem selectedItem = this.gvTrackingOptions.SelectedItems[0];
        if (this.defaultTrackingList.Contains(selectedItem.Text))
        {
          selectedItem.Selected = false;
          return;
        }
      }
      this.OnSelectedIndexChanged(this.gvTrackingOptions, (IconButton) this.btnUpTrackingOptions, (IconButton) this.btnDownTrackingOptions, (IconButton) this.btnAddTrackingOptions, (IconButton) this.btnDeleteTrackingOptions);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.isDirty)
      {
        if (Utils.Dialog((IWin32Window) this, "All unsaved changes will be discarded.\nDo you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
        {
          this.isDirty = false;
          this.DialogResult = DialogResult.Cancel;
        }
        else
          this.DialogResult = DialogResult.None;
      }
      else
      {
        this.DialogResult = DialogResult.Cancel;
        this.isCancelHit = true;
      }
    }

    private void ConditionTypeSettingsAddEditDialog_FormClosing(
      object sender,
      FormClosingEventArgs e)
    {
      if (!this.isDirty || this.isCancelHit)
        return;
      if (Utils.Dialog((IWin32Window) this, "All unsaved changes will be discarded.\nDo you want to Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
        this.DialogResult = DialogResult.Cancel;
      else
        this.DialogResult = DialogResult.None;
    }

    private void gvTrackingOptions_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.isDirty = true;
    }

    private void gvTrackingOwner_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.isDirty = true;
    }

    private void textBox1_KeyPress(object sender, KeyPressEventArgs e) => this.isDirty = true;

    private void DeSelectGridItems(GridView gv)
    {
      if (gv.Items == null || gv.Items.Count <= 0 || gv.SelectedItems.Count <= 0)
        return;
      foreach (GVItem selectedItem in gv.SelectedItems)
        selectedItem.Selected = false;
    }

    private void gvTrackingOptions_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      for (int index = 0; index < this.defaultTrackingList.Count; index++)
      {
        GVItem gvItem = this.gvTrackingOptions.Items.FirstOrDefault<GVItem>((Func<GVItem, bool>) (q => q.SubItems[0].Text == this.defaultTrackingList[index]));
        this.gvTrackingOptions.Items.Remove(gvItem);
        this.gvTrackingOptions.Items.Insert(index, gvItem);
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.tooltip = new ToolTip(this.components);
      this.btnUpCategory = new StandardIconButton();
      this.btnAddCategory = new StandardIconButton();
      this.btnDownCategory = new StandardIconButton();
      this.btnDeleteCategory = new StandardIconButton();
      this.btnDeletePriorTo = new StandardIconButton();
      this.btnDownPriorTo = new StandardIconButton();
      this.btnUpPriorTo = new StandardIconButton();
      this.btnAddPriorTo = new StandardIconButton();
      this.btnDeleteSource = new StandardIconButton();
      this.btnDownSource = new StandardIconButton();
      this.btnUpSource = new StandardIconButton();
      this.btnAddSource = new StandardIconButton();
      this.btnDeleteRecipient = new StandardIconButton();
      this.btnDownRecipient = new StandardIconButton();
      this.btnUpRecipient = new StandardIconButton();
      this.btnAddRecipient = new StandardIconButton();
      this.btnDeleteTrackingOptions = new StandardIconButton();
      this.btnDownTrackingOptions = new StandardIconButton();
      this.btnUpTrackingOptions = new StandardIconButton();
      this.btnAddTrackingOptions = new StandardIconButton();
      this.pnltop = new Panel();
      this.txtTitle = new TextBox();
      this.label1 = new Label();
      this.pnlbottom = new Panel();
      this.gcTrackingOwner = new GroupContainer();
      this.label7 = new Label();
      this.label6 = new Label();
      this.gvTrackingOwner = new GridView();
      this.gcTrackingOptions = new GroupContainer();
      this.label5 = new Label();
      this.flowLayoutPanel4 = new FlowLayoutPanel();
      this.gvTrackingOptions = new GridView();
      this.gcRecipient = new GroupContainer();
      this.label4 = new Label();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.gvRecipient = new GridView();
      this.gcSource = new GroupContainer();
      this.label3 = new Label();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.gvSource = new GridView();
      this.gcPriorTo = new GroupContainer();
      this.label2 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.gvPriorTo = new GridView();
      this.gcCategory = new GroupContainer();
      this.lbltypecount = new Label();
      this.pnlToolbar = new FlowLayoutPanel();
      this.gvCategory = new GridView();
      ((ISupportInitialize) this.btnUpCategory).BeginInit();
      ((ISupportInitialize) this.btnAddCategory).BeginInit();
      ((ISupportInitialize) this.btnDownCategory).BeginInit();
      ((ISupportInitialize) this.btnDeleteCategory).BeginInit();
      ((ISupportInitialize) this.btnDeletePriorTo).BeginInit();
      ((ISupportInitialize) this.btnDownPriorTo).BeginInit();
      ((ISupportInitialize) this.btnUpPriorTo).BeginInit();
      ((ISupportInitialize) this.btnAddPriorTo).BeginInit();
      ((ISupportInitialize) this.btnDeleteSource).BeginInit();
      ((ISupportInitialize) this.btnDownSource).BeginInit();
      ((ISupportInitialize) this.btnUpSource).BeginInit();
      ((ISupportInitialize) this.btnAddSource).BeginInit();
      ((ISupportInitialize) this.btnDeleteRecipient).BeginInit();
      ((ISupportInitialize) this.btnDownRecipient).BeginInit();
      ((ISupportInitialize) this.btnUpRecipient).BeginInit();
      ((ISupportInitialize) this.btnAddRecipient).BeginInit();
      ((ISupportInitialize) this.btnDeleteTrackingOptions).BeginInit();
      ((ISupportInitialize) this.btnDownTrackingOptions).BeginInit();
      ((ISupportInitialize) this.btnUpTrackingOptions).BeginInit();
      ((ISupportInitialize) this.btnAddTrackingOptions).BeginInit();
      this.pnltop.SuspendLayout();
      this.pnlbottom.SuspendLayout();
      this.gcTrackingOwner.SuspendLayout();
      this.gcTrackingOptions.SuspendLayout();
      this.flowLayoutPanel4.SuspendLayout();
      this.gcRecipient.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.gcSource.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.gcPriorTo.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gcCategory.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(886, 568);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(805, 568);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 24);
      this.btnOk.TabIndex = 10;
      this.btnOk.Text = "Save";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnSave_Click);
      this.btnUpCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnUpCategory.BackColor = Color.Transparent;
      this.btnUpCategory.Location = new Point(32, 3);
      this.btnUpCategory.Margin = new Padding(4, 3, 0, 3);
      this.btnUpCategory.MouseDownImage = (Image) null;
      this.btnUpCategory.Name = "btnUpCategory";
      this.btnUpCategory.Size = new Size(16, 16);
      this.btnUpCategory.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpCategory.TabIndex = 14;
      this.btnUpCategory.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUpCategory, "Move Up");
      this.btnUpCategory.Click += new EventHandler(this.btnUpCategory_Click);
      this.btnAddCategory.Anchor = AnchorStyles.Left;
      this.btnAddCategory.BackColor = Color.Transparent;
      this.btnAddCategory.Location = new Point(12, 3);
      this.btnAddCategory.Margin = new Padding(4, 3, 0, 3);
      this.btnAddCategory.MouseDownImage = (Image) null;
      this.btnAddCategory.Name = "btnAddCategory";
      this.btnAddCategory.Size = new Size(16, 16);
      this.btnAddCategory.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddCategory.TabIndex = 12;
      this.btnAddCategory.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddCategory, "Add Category");
      this.btnAddCategory.Click += new EventHandler(this.btnAddCategory_Click);
      this.btnDownCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDownCategory.BackColor = Color.Transparent;
      this.btnDownCategory.Location = new Point(52, 3);
      this.btnDownCategory.Margin = new Padding(4, 3, 0, 3);
      this.btnDownCategory.MouseDownImage = (Image) null;
      this.btnDownCategory.Name = "btnDownCategory";
      this.btnDownCategory.Size = new Size(16, 16);
      this.btnDownCategory.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownCategory.TabIndex = 15;
      this.btnDownCategory.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDownCategory, "Move Down");
      this.btnDownCategory.Click += new EventHandler(this.btnDownCategory_Click);
      this.btnDeleteCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDeleteCategory.BackColor = Color.Transparent;
      this.btnDeleteCategory.Location = new Point(72, 3);
      this.btnDeleteCategory.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteCategory.MouseDownImage = (Image) null;
      this.btnDeleteCategory.Name = "btnDeleteCategory";
      this.btnDeleteCategory.Size = new Size(16, 16);
      this.btnDeleteCategory.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteCategory.TabIndex = 16;
      this.btnDeleteCategory.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteCategory, "Delete  Category");
      this.btnDeleteCategory.Click += new EventHandler(this.btnDeleteCategory_Click);
      this.btnDeletePriorTo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDeletePriorTo.BackColor = Color.Transparent;
      this.btnDeletePriorTo.Location = new Point(72, 3);
      this.btnDeletePriorTo.Margin = new Padding(4, 3, 0, 3);
      this.btnDeletePriorTo.MouseDownImage = (Image) null;
      this.btnDeletePriorTo.Name = "btnDeletePriorTo";
      this.btnDeletePriorTo.Size = new Size(16, 16);
      this.btnDeletePriorTo.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeletePriorTo.TabIndex = 16;
      this.btnDeletePriorTo.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeletePriorTo, "Delete  Prior To");
      this.btnDeletePriorTo.Click += new EventHandler(this.btnDeleteCategory_Click);
      this.btnDownPriorTo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDownPriorTo.BackColor = Color.Transparent;
      this.btnDownPriorTo.Location = new Point(52, 3);
      this.btnDownPriorTo.Margin = new Padding(4, 3, 0, 3);
      this.btnDownPriorTo.MouseDownImage = (Image) null;
      this.btnDownPriorTo.Name = "btnDownPriorTo";
      this.btnDownPriorTo.Size = new Size(16, 16);
      this.btnDownPriorTo.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownPriorTo.TabIndex = 15;
      this.btnDownPriorTo.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDownPriorTo, "Move Down");
      this.btnDownPriorTo.Click += new EventHandler(this.btnDownCategory_Click);
      this.btnUpPriorTo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnUpPriorTo.BackColor = Color.Transparent;
      this.btnUpPriorTo.Location = new Point(32, 3);
      this.btnUpPriorTo.Margin = new Padding(4, 3, 0, 3);
      this.btnUpPriorTo.MouseDownImage = (Image) null;
      this.btnUpPriorTo.Name = "btnUpPriorTo";
      this.btnUpPriorTo.Size = new Size(16, 16);
      this.btnUpPriorTo.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpPriorTo.TabIndex = 14;
      this.btnUpPriorTo.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUpPriorTo, "Move Up");
      this.btnUpPriorTo.Click += new EventHandler(this.btnUpCategory_Click);
      this.btnAddPriorTo.Anchor = AnchorStyles.Left;
      this.btnAddPriorTo.BackColor = Color.Transparent;
      this.btnAddPriorTo.Location = new Point(12, 3);
      this.btnAddPriorTo.Margin = new Padding(4, 3, 0, 3);
      this.btnAddPriorTo.MouseDownImage = (Image) null;
      this.btnAddPriorTo.Name = "btnAddPriorTo";
      this.btnAddPriorTo.Size = new Size(16, 16);
      this.btnAddPriorTo.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPriorTo.TabIndex = 12;
      this.btnAddPriorTo.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddPriorTo, "Add Prior To");
      this.btnAddPriorTo.Click += new EventHandler(this.btnAddCategory_Click);
      this.btnDeleteSource.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDeleteSource.BackColor = Color.Transparent;
      this.btnDeleteSource.Location = new Point(72, 3);
      this.btnDeleteSource.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteSource.MouseDownImage = (Image) null;
      this.btnDeleteSource.Name = "btnDeleteSource";
      this.btnDeleteSource.Size = new Size(16, 16);
      this.btnDeleteSource.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSource.TabIndex = 16;
      this.btnDeleteSource.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteSource, "Delete Source");
      this.btnDeleteSource.Click += new EventHandler(this.btnDeleteCategory_Click);
      this.btnDownSource.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDownSource.BackColor = Color.Transparent;
      this.btnDownSource.Location = new Point(52, 3);
      this.btnDownSource.Margin = new Padding(4, 3, 0, 3);
      this.btnDownSource.MouseDownImage = (Image) null;
      this.btnDownSource.Name = "btnDownSource";
      this.btnDownSource.Size = new Size(16, 16);
      this.btnDownSource.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownSource.TabIndex = 15;
      this.btnDownSource.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDownSource, "Move Down");
      this.btnDownSource.Click += new EventHandler(this.btnDownCategory_Click);
      this.btnUpSource.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnUpSource.BackColor = Color.Transparent;
      this.btnUpSource.Location = new Point(32, 3);
      this.btnUpSource.Margin = new Padding(4, 3, 0, 3);
      this.btnUpSource.MouseDownImage = (Image) null;
      this.btnUpSource.Name = "btnUpSource";
      this.btnUpSource.Size = new Size(16, 16);
      this.btnUpSource.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpSource.TabIndex = 14;
      this.btnUpSource.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUpSource, "Move Up");
      this.btnUpSource.Click += new EventHandler(this.btnUpCategory_Click);
      this.btnAddSource.Anchor = AnchorStyles.Left;
      this.btnAddSource.BackColor = Color.Transparent;
      this.btnAddSource.Location = new Point(12, 3);
      this.btnAddSource.Margin = new Padding(4, 3, 0, 3);
      this.btnAddSource.MouseDownImage = (Image) null;
      this.btnAddSource.Name = "btnAddSource";
      this.btnAddSource.Size = new Size(16, 16);
      this.btnAddSource.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSource.TabIndex = 12;
      this.btnAddSource.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddSource, "Add Source");
      this.btnAddSource.Click += new EventHandler(this.btnAddCategory_Click);
      this.btnDeleteRecipient.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDeleteRecipient.BackColor = Color.Transparent;
      this.btnDeleteRecipient.Location = new Point(72, 3);
      this.btnDeleteRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteRecipient.MouseDownImage = (Image) null;
      this.btnDeleteRecipient.Name = "btnDeleteRecipient";
      this.btnDeleteRecipient.Size = new Size(16, 16);
      this.btnDeleteRecipient.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteRecipient.TabIndex = 16;
      this.btnDeleteRecipient.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteRecipient, "Delete Recipient");
      this.btnDeleteRecipient.Click += new EventHandler(this.btnDeleteCategory_Click);
      this.btnDownRecipient.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDownRecipient.BackColor = Color.Transparent;
      this.btnDownRecipient.Location = new Point(52, 3);
      this.btnDownRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnDownRecipient.MouseDownImage = (Image) null;
      this.btnDownRecipient.Name = "btnDownRecipient";
      this.btnDownRecipient.Size = new Size(16, 16);
      this.btnDownRecipient.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownRecipient.TabIndex = 15;
      this.btnDownRecipient.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDownRecipient, "Move Down");
      this.btnDownRecipient.Click += new EventHandler(this.btnDownCategory_Click);
      this.btnUpRecipient.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnUpRecipient.BackColor = Color.Transparent;
      this.btnUpRecipient.Location = new Point(32, 3);
      this.btnUpRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnUpRecipient.MouseDownImage = (Image) null;
      this.btnUpRecipient.Name = "btnUpRecipient";
      this.btnUpRecipient.Size = new Size(16, 16);
      this.btnUpRecipient.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpRecipient.TabIndex = 14;
      this.btnUpRecipient.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUpRecipient, "Move Up");
      this.btnUpRecipient.Click += new EventHandler(this.btnUpCategory_Click);
      this.btnAddRecipient.Anchor = AnchorStyles.Left;
      this.btnAddRecipient.BackColor = Color.Transparent;
      this.btnAddRecipient.Location = new Point(12, 3);
      this.btnAddRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnAddRecipient.MouseDownImage = (Image) null;
      this.btnAddRecipient.Name = "btnAddRecipient";
      this.btnAddRecipient.Size = new Size(16, 16);
      this.btnAddRecipient.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRecipient.TabIndex = 12;
      this.btnAddRecipient.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddRecipient, "Add Recipient");
      this.btnAddRecipient.Click += new EventHandler(this.btnAddCategory_Click);
      this.btnDeleteTrackingOptions.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDeleteTrackingOptions.BackColor = Color.Transparent;
      this.btnDeleteTrackingOptions.Location = new Point(72, 3);
      this.btnDeleteTrackingOptions.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteTrackingOptions.MouseDownImage = (Image) null;
      this.btnDeleteTrackingOptions.Name = "btnDeleteTrackingOptions";
      this.btnDeleteTrackingOptions.Size = new Size(16, 16);
      this.btnDeleteTrackingOptions.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTrackingOptions.TabIndex = 16;
      this.btnDeleteTrackingOptions.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteTrackingOptions, "Delete Tracking");
      this.btnDeleteTrackingOptions.Click += new EventHandler(this.btnDeleteCategory_Click);
      this.btnDownTrackingOptions.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnDownTrackingOptions.BackColor = Color.Transparent;
      this.btnDownTrackingOptions.Location = new Point(52, 3);
      this.btnDownTrackingOptions.Margin = new Padding(4, 3, 0, 3);
      this.btnDownTrackingOptions.MouseDownImage = (Image) null;
      this.btnDownTrackingOptions.Name = "btnDownTrackingOptions";
      this.btnDownTrackingOptions.Size = new Size(16, 16);
      this.btnDownTrackingOptions.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownTrackingOptions.TabIndex = 15;
      this.btnDownTrackingOptions.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDownTrackingOptions, "Move Down");
      this.btnDownTrackingOptions.Click += new EventHandler(this.btnDownCategory_Click);
      this.btnUpTrackingOptions.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.btnUpTrackingOptions.BackColor = Color.Transparent;
      this.btnUpTrackingOptions.Location = new Point(32, 3);
      this.btnUpTrackingOptions.Margin = new Padding(4, 3, 0, 3);
      this.btnUpTrackingOptions.MouseDownImage = (Image) null;
      this.btnUpTrackingOptions.Name = "btnUpTrackingOptions";
      this.btnUpTrackingOptions.Size = new Size(16, 16);
      this.btnUpTrackingOptions.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpTrackingOptions.TabIndex = 14;
      this.btnUpTrackingOptions.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUpTrackingOptions, "Move Up");
      this.btnUpTrackingOptions.Click += new EventHandler(this.btnUpCategory_Click);
      this.btnAddTrackingOptions.Anchor = AnchorStyles.Left;
      this.btnAddTrackingOptions.BackColor = Color.Transparent;
      this.btnAddTrackingOptions.Location = new Point(12, 3);
      this.btnAddTrackingOptions.Margin = new Padding(4, 3, 0, 3);
      this.btnAddTrackingOptions.MouseDownImage = (Image) null;
      this.btnAddTrackingOptions.Name = "btnAddTrackingOptions";
      this.btnAddTrackingOptions.Size = new Size(16, 16);
      this.btnAddTrackingOptions.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddTrackingOptions.TabIndex = 12;
      this.btnAddTrackingOptions.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddTrackingOptions, "Add Tracking");
      this.btnAddTrackingOptions.Click += new EventHandler(this.btnAddCategory_Click);
      this.pnltop.Controls.Add((Control) this.txtTitle);
      this.pnltop.Controls.Add((Control) this.label1);
      this.pnltop.Dock = DockStyle.Top;
      this.pnltop.Location = new Point(0, 0);
      this.pnltop.Name = "pnltop";
      this.pnltop.Size = new Size(973, 35);
      this.pnltop.TabIndex = 8;
      this.pnltop.TabStop = true;
      this.txtTitle.Location = new Point(43, 6);
      this.txtTitle.MaxLength = 64;
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.Size = new Size(919, 20);
      this.txtTitle.TabIndex = 2;
      this.txtTitle.KeyPress += new KeyPressEventHandler(this.textBox1_KeyPress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name";
      this.pnlbottom.BorderStyle = BorderStyle.FixedSingle;
      this.pnlbottom.Controls.Add((Control) this.gcTrackingOwner);
      this.pnlbottom.Controls.Add((Control) this.gcTrackingOptions);
      this.pnlbottom.Controls.Add((Control) this.gcRecipient);
      this.pnlbottom.Controls.Add((Control) this.gcSource);
      this.pnlbottom.Controls.Add((Control) this.gcPriorTo);
      this.pnlbottom.Controls.Add((Control) this.gcCategory);
      this.pnlbottom.Location = new Point(0, 41);
      this.pnlbottom.Name = "pnlbottom";
      this.pnlbottom.Size = new Size(973, 517);
      this.pnlbottom.TabIndex = 9;
      this.gcTrackingOwner.Controls.Add((Control) this.label7);
      this.gcTrackingOwner.Controls.Add((Control) this.label6);
      this.gcTrackingOwner.Controls.Add((Control) this.gvTrackingOwner);
      this.gcTrackingOwner.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcTrackingOwner.HeaderForeColor = SystemColors.ControlText;
      this.gcTrackingOwner.Location = new Point(245, 257);
      this.gcTrackingOwner.Name = "gcTrackingOwner";
      this.gcTrackingOwner.Size = new Size(716, 247);
      this.gcTrackingOwner.TabIndex = 11;
      this.gcTrackingOwner.Text = "Tracking Owners ";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(100, 6);
      this.label7.Name = "label7";
      this.label7.Size = new Size(413, 14);
      this.label7.TabIndex = 3;
      this.label7.Text = "(Requested, Re-requested, Fulfilled Tracking Status will be available for all Roles)";
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(40, 7);
      this.label6.Name = "label6";
      this.label6.Size = new Size(0, 14);
      this.label6.TabIndex = 2;
      this.gvTrackingOwner.AllowMultiselect = false;
      this.gvTrackingOwner.BorderStyle = BorderStyle.None;
      this.gvTrackingOwner.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colRole";
      gvColumn1.Text = "Role";
      gvColumn1.Width = 120;
      this.gvTrackingOwner.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvTrackingOwner.Dock = DockStyle.Fill;
      this.gvTrackingOwner.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrackingOwner.Location = new Point(1, 26);
      this.gvTrackingOwner.Name = "gvTrackingOwner";
      this.gvTrackingOwner.Size = new Size(714, 220);
      this.gvTrackingOwner.TabIndex = 1;
      this.gvTrackingOwner.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gcTrackingOptions.Controls.Add((Control) this.label5);
      this.gcTrackingOptions.Controls.Add((Control) this.flowLayoutPanel4);
      this.gcTrackingOptions.Controls.Add((Control) this.gvTrackingOptions);
      this.gcTrackingOptions.HeaderForeColor = SystemColors.ControlText;
      this.gcTrackingOptions.Location = new Point(4, 256);
      this.gcTrackingOptions.Name = "gcTrackingOptions";
      this.gcTrackingOptions.Size = new Size(234, 247);
      this.gcTrackingOptions.TabIndex = 10;
      this.gcTrackingOptions.Text = "Tracking Options";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(40, 7);
      this.label5.Name = "label5";
      this.label5.Size = new Size(0, 14);
      this.label5.TabIndex = 2;
      this.flowLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel4.BackColor = Color.Transparent;
      this.flowLayoutPanel4.Controls.Add((Control) this.btnDeleteTrackingOptions);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnDownTrackingOptions);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnUpTrackingOptions);
      this.flowLayoutPanel4.Controls.Add((Control) this.btnAddTrackingOptions);
      this.flowLayoutPanel4.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel4.Location = new Point(142, 1);
      this.flowLayoutPanel4.Name = "flowLayoutPanel4";
      this.flowLayoutPanel4.Size = new Size(88, 21);
      this.flowLayoutPanel4.TabIndex = 0;
      this.gvTrackingOptions.AllowMultiselect = false;
      this.gvTrackingOptions.BorderStyle = BorderStyle.None;
      this.gvTrackingOptions.ClearSelectionsOnEmptyRowClick = false;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colStatus";
      gvColumn2.Text = "Status";
      gvColumn2.TextAlignment = ContentAlignment.TopLeft;
      gvColumn2.Width = 120;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "IsOpen";
      gvColumn3.SortMethod = GVSortMethod.Checkbox;
      gvColumn3.Text = "Is Open";
      gvColumn3.Width = 100;
      this.gvTrackingOptions.Columns.AddRange(new GVColumn[2]
      {
        gvColumn2,
        gvColumn3
      });
      this.gvTrackingOptions.Dock = DockStyle.Fill;
      this.gvTrackingOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrackingOptions.Location = new Point(1, 26);
      this.gvTrackingOptions.Name = "gvTrackingOptions";
      this.gvTrackingOptions.Size = new Size(232, 220);
      this.gvTrackingOptions.TabIndex = 1;
      this.gvTrackingOptions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTrackingOptions.SelectedIndexChanged += new EventHandler(this.gvTrackingOptions_SelectedIndexChanged);
      this.gvTrackingOptions.ColumnClick += new GVColumnClickEventHandler(this.gvTrackingOptions_ColumnClick);
      this.gcRecipient.Controls.Add((Control) this.label4);
      this.gcRecipient.Controls.Add((Control) this.flowLayoutPanel3);
      this.gcRecipient.Controls.Add((Control) this.gvRecipient);
      this.gcRecipient.HeaderForeColor = SystemColors.ControlText;
      this.gcRecipient.Location = new Point(726, 4);
      this.gcRecipient.Name = "gcRecipient";
      this.gcRecipient.Size = new Size(235, 247);
      this.gcRecipient.TabIndex = 9;
      this.gcRecipient.Text = "Recipient Options";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(40, 7);
      this.label4.Name = "label4";
      this.label4.Size = new Size(0, 14);
      this.label4.TabIndex = 2;
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.btnDeleteRecipient);
      this.flowLayoutPanel3.Controls.Add((Control) this.btnDownRecipient);
      this.flowLayoutPanel3.Controls.Add((Control) this.btnUpRecipient);
      this.flowLayoutPanel3.Controls.Add((Control) this.btnAddRecipient);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(143, 1);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(88, 21);
      this.flowLayoutPanel3.TabIndex = 0;
      this.gvRecipient.AllowMultiselect = false;
      this.gvRecipient.BorderStyle = BorderStyle.None;
      this.gvRecipient.ClearSelectionsOnEmptyRowClick = false;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colRecipient";
      gvColumn4.Text = "Recipient";
      gvColumn4.Width = 232;
      this.gvRecipient.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gvRecipient.Dock = DockStyle.Fill;
      this.gvRecipient.HeaderHeight = 0;
      this.gvRecipient.HeaderVisible = false;
      this.gvRecipient.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRecipient.Location = new Point(1, 26);
      this.gvRecipient.Name = "gvRecipient";
      this.gvRecipient.Size = new Size(233, 220);
      this.gvRecipient.TabIndex = 1;
      this.gvRecipient.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvRecipient.SelectedIndexChanged += new EventHandler(this.gvRecipient_SelectedIndexChanged);
      this.gcSource.Controls.Add((Control) this.label3);
      this.gcSource.Controls.Add((Control) this.flowLayoutPanel2);
      this.gcSource.Controls.Add((Control) this.gvSource);
      this.gcSource.HeaderForeColor = SystemColors.ControlText;
      this.gcSource.Location = new Point(485, 4);
      this.gcSource.Name = "gcSource";
      this.gcSource.Size = new Size(235, 247);
      this.gcSource.TabIndex = 8;
      this.gcSource.Text = "Source Options";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(40, 7);
      this.label3.Name = "label3";
      this.label3.Size = new Size(0, 14);
      this.label3.TabIndex = 2;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDeleteSource);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDownSource);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnUpSource);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddSource);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(143, 1);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(88, 21);
      this.flowLayoutPanel2.TabIndex = 0;
      this.gvSource.AllowMultiselect = false;
      this.gvSource.BorderStyle = BorderStyle.None;
      this.gvSource.ClearSelectionsOnEmptyRowClick = false;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colSource";
      gvColumn5.Text = "Source";
      gvColumn5.Width = 232;
      this.gvSource.Columns.AddRange(new GVColumn[1]
      {
        gvColumn5
      });
      this.gvSource.Dock = DockStyle.Fill;
      this.gvSource.HeaderHeight = 0;
      this.gvSource.HeaderVisible = false;
      this.gvSource.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSource.Location = new Point(1, 26);
      this.gvSource.Name = "gvSource";
      this.gvSource.Size = new Size(233, 220);
      this.gvSource.TabIndex = 1;
      this.gvSource.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSource.SelectedIndexChanged += new EventHandler(this.gvSource_SelectedIndexChanged);
      this.gcPriorTo.Controls.Add((Control) this.label2);
      this.gcPriorTo.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcPriorTo.Controls.Add((Control) this.gvPriorTo);
      this.gcPriorTo.HeaderForeColor = SystemColors.ControlText;
      this.gcPriorTo.Location = new Point(244, 3);
      this.gcPriorTo.Name = "gcPriorTo";
      this.gcPriorTo.Size = new Size(235, 247);
      this.gcPriorTo.TabIndex = 7;
      this.gcPriorTo.Text = "Prior To Options";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(40, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(0, 14);
      this.label2.TabIndex = 2;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDeletePriorTo);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDownPriorTo);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUpPriorTo);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddPriorTo);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(143, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(88, 21);
      this.flowLayoutPanel1.TabIndex = 0;
      this.gvPriorTo.AllowMultiselect = false;
      this.gvPriorTo.BorderStyle = BorderStyle.None;
      this.gvPriorTo.ClearSelectionsOnEmptyRowClick = false;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colPriorTo";
      gvColumn6.Text = "PriorTo";
      gvColumn6.Width = 232;
      this.gvPriorTo.Columns.AddRange(new GVColumn[1]
      {
        gvColumn6
      });
      this.gvPriorTo.Dock = DockStyle.Fill;
      this.gvPriorTo.HeaderHeight = 0;
      this.gvPriorTo.HeaderVisible = false;
      this.gvPriorTo.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPriorTo.Location = new Point(1, 26);
      this.gvPriorTo.Name = "gvPriorTo";
      this.gvPriorTo.Size = new Size(233, 220);
      this.gvPriorTo.TabIndex = 1;
      this.gvPriorTo.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvPriorTo.SelectedIndexChanged += new EventHandler(this.gvPriorTo_SelectedIndexChanged);
      this.gcCategory.Controls.Add((Control) this.lbltypecount);
      this.gcCategory.Controls.Add((Control) this.pnlToolbar);
      this.gcCategory.Controls.Add((Control) this.gvCategory);
      this.gcCategory.HeaderForeColor = SystemColors.ControlText;
      this.gcCategory.Location = new Point(3, 3);
      this.gcCategory.Name = "gcCategory";
      this.gcCategory.Size = new Size(235, 247);
      this.gcCategory.TabIndex = 6;
      this.gcCategory.Text = "Category Options";
      this.lbltypecount.AutoSize = true;
      this.lbltypecount.BackColor = Color.Transparent;
      this.lbltypecount.Location = new Point(40, 7);
      this.lbltypecount.Name = "lbltypecount";
      this.lbltypecount.Size = new Size(0, 14);
      this.lbltypecount.TabIndex = 2;
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDeleteCategory);
      this.pnlToolbar.Controls.Add((Control) this.btnDownCategory);
      this.pnlToolbar.Controls.Add((Control) this.btnUpCategory);
      this.pnlToolbar.Controls.Add((Control) this.btnAddCategory);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(143, 1);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(88, 21);
      this.pnlToolbar.TabIndex = 0;
      this.gvCategory.AllowMultiselect = false;
      this.gvCategory.BorderStyle = BorderStyle.None;
      this.gvCategory.ClearSelectionsOnEmptyRowClick = false;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colType";
      gvColumn7.Text = "Type";
      gvColumn7.Width = 232;
      this.gvCategory.Columns.AddRange(new GVColumn[1]
      {
        gvColumn7
      });
      this.gvCategory.Dock = DockStyle.Fill;
      this.gvCategory.HeaderHeight = 0;
      this.gvCategory.HeaderVisible = false;
      this.gvCategory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvCategory.Location = new Point(1, 26);
      this.gvCategory.Name = "gvCategory";
      this.gvCategory.Size = new Size(233, 220);
      this.gvCategory.TabIndex = 1;
      this.gvCategory.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvCategory.SelectedIndexChanged += new EventHandler(this.gvCategory_SelectedIndexChanged);
      this.gvCategory.SubItemMove += new GVSubItemMouseEventHandler(this.gvCategory_SubItemMove);
      this.gvCategory.Leave += new EventHandler(this.gvCategory_Leave);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(973, 599);
      this.Controls.Add((Control) this.pnlbottom);
      this.Controls.Add((Control) this.pnltop);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionTypeSettingsAddEditDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add a Condition Type Setting";
      this.FormClosing += new FormClosingEventHandler(this.ConditionTypeSettingsAddEditDialog_FormClosing);
      this.KeyPress += new KeyPressEventHandler(this.textBox1_KeyPress);
      ((ISupportInitialize) this.btnUpCategory).EndInit();
      ((ISupportInitialize) this.btnAddCategory).EndInit();
      ((ISupportInitialize) this.btnDownCategory).EndInit();
      ((ISupportInitialize) this.btnDeleteCategory).EndInit();
      ((ISupportInitialize) this.btnDeletePriorTo).EndInit();
      ((ISupportInitialize) this.btnDownPriorTo).EndInit();
      ((ISupportInitialize) this.btnUpPriorTo).EndInit();
      ((ISupportInitialize) this.btnAddPriorTo).EndInit();
      ((ISupportInitialize) this.btnDeleteSource).EndInit();
      ((ISupportInitialize) this.btnDownSource).EndInit();
      ((ISupportInitialize) this.btnUpSource).EndInit();
      ((ISupportInitialize) this.btnAddSource).EndInit();
      ((ISupportInitialize) this.btnDeleteRecipient).EndInit();
      ((ISupportInitialize) this.btnDownRecipient).EndInit();
      ((ISupportInitialize) this.btnUpRecipient).EndInit();
      ((ISupportInitialize) this.btnAddRecipient).EndInit();
      ((ISupportInitialize) this.btnDeleteTrackingOptions).EndInit();
      ((ISupportInitialize) this.btnDownTrackingOptions).EndInit();
      ((ISupportInitialize) this.btnUpTrackingOptions).EndInit();
      ((ISupportInitialize) this.btnAddTrackingOptions).EndInit();
      this.pnltop.ResumeLayout(false);
      this.pnltop.PerformLayout();
      this.pnlbottom.ResumeLayout(false);
      this.gcTrackingOwner.ResumeLayout(false);
      this.gcTrackingOwner.PerformLayout();
      this.gcTrackingOptions.ResumeLayout(false);
      this.gcTrackingOptions.PerformLayout();
      this.flowLayoutPanel4.ResumeLayout(false);
      this.gcRecipient.ResumeLayout(false);
      this.gcRecipient.PerformLayout();
      this.flowLayoutPanel3.ResumeLayout(false);
      this.gcSource.ResumeLayout(false);
      this.gcSource.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.gcPriorTo.ResumeLayout(false);
      this.gcPriorTo.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gcCategory.ResumeLayout(false);
      this.gcCategory.PerformLayout();
      this.pnlToolbar.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
