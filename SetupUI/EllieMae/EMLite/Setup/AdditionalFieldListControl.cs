// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AdditionalFieldListControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AdditionalFieldListControl : SettingsUserControl
  {
    private Sessions.Session session;
    private FieldSettings fieldSettings;
    private bool forLoanField;
    private Hashtable existRequestFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable existLoanFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private List<string> fieldsNotAllowed = new List<string>();
    private Hashtable reportingFields;
    private IContainer components;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageEx1;
    private GroupContainer grpContainerLock;
    private TabPageEx tabPageEx2;
    private Label label1;
    private GridView gridViewLockFields;
    private Label label2;
    private GroupContainer grpContainerLoan;
    private GridView gridViewLoanFields;
    private StandardIconButton iconBtnLockDown;
    private StandardIconButton iconBtnLockNew;
    private StandardIconButton iconBtnLockSearch;
    private StandardIconButton iconBtnLockDelete;
    private StandardIconButton iconBtnLockUp;
    private VerticalSeparator verticalSeparator1;
    private Button btnLockPreview;
    private StandardIconButton iconBtnFieldNew;
    private StandardIconButton iconBtnFieldSearch;
    private StandardIconButton iconBtnFieldDelete;
    private StandardIconButton iconBtnFieldUp;
    private VerticalSeparator verticalSeparator2;
    private Button btnFieldPreview;
    private StandardIconButton iconBtnFieldDown;
    private Panel panel2;
    private Panel panel1;
    private Panel panel4;
    private Panel panel3;
    private BorderPanel borderPanel1;
    private ToolTip toolTip1;

    public AdditionalFieldListControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.setupContainer = setupContainer;
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[24]
      {
        "3",
        "4000",
        "4001",
        "4002",
        "4003",
        "4004",
        "4005",
        "4006",
        "4007",
        "65",
        "97",
        "36",
        "37",
        "68",
        "69",
        "1109",
        "1045",
        "1765",
        "1107",
        "SYS.X11",
        "2",
        "1826",
        "1760",
        "1172"
      });
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[26]
      {
        "1401",
        "1785",
        "MORNET.X67",
        "1484",
        "1502",
        "VASUMM.X23",
        "11",
        "12",
        "14",
        "15",
        "13",
        "1041",
        "1821",
        "356",
        "1811",
        "19",
        "608",
        "1267",
        "1266",
        "995",
        "994",
        "420",
        "4",
        "325",
        "2293",
        "2294"
      });
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[10]
      {
        "675",
        "2217",
        "763",
        "2861",
        "427",
        "428",
        "1732",
        "136",
        "1130",
        "562"
      });
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[6]
      {
        "353",
        "976",
        "740",
        "742",
        "2398",
        "142"
      });
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[24]
      {
        "2866",
        "2867",
        "2940",
        "2941",
        "2853",
        "2942",
        "2943",
        "2945",
        "2946",
        "2944",
        "2947",
        "2948",
        "2949",
        "2950",
        "2951",
        "2952",
        "2953",
        "2954",
        "2955",
        "2956",
        "2957",
        "2958",
        "2959",
        "2960"
      });
      this.fieldsNotAllowed.AddRange((IEnumerable<string>) new string[21]
      {
        "2961",
        "2962",
        "2963",
        "2964",
        "2965",
        "2966",
        "2967",
        "3035",
        "3036",
        "3037",
        "3038",
        "3041",
        "3043",
        "3044",
        "3045",
        "3046",
        "3049",
        "3056",
        "3047",
        "3241",
        "3242"
      });
      for (int index = 2868; index <= 2939; ++index)
        this.fieldsNotAllowed.Add(string.Concat((object) index));
      this.InitializeComponent();
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      LoanXDBTableList loanXdbTableList = this.session.LoanManager.GetLoanXDBTableList(false);
      if (loanXdbTableList != null)
        this.reportingFields = loanXdbTableList.GetFields();
      this.refresh();
      base.Reset();
    }

    private void btnFieldPreview_Click(object sender, EventArgs e)
    {
      this.previewScreen(this.gridViewLoanFields);
    }

    private void iconBtnFieldDown_Click(object sender, EventArgs e)
    {
      this.gridViewLoanFields.SelectedIndexChanged -= new EventHandler(this.gridViewLoanFields_SelectedIndexChanged);
      this.moveFieldDown(this.gridViewLoanFields);
      this.gridViewLoanFields.SelectedIndexChanged += new EventHandler(this.gridViewLoanFields_SelectedIndexChanged);
    }

    private void iconBtnFieldUp_Click(object sender, EventArgs e)
    {
      this.gridViewLoanFields.SelectedIndexChanged -= new EventHandler(this.gridViewLoanFields_SelectedIndexChanged);
      this.moveFieldUp(this.gridViewLoanFields);
      this.gridViewLoanFields.SelectedIndexChanged += new EventHandler(this.gridViewLoanFields_SelectedIndexChanged);
    }

    private void iconBtnFieldDelete_Click(object sender, EventArgs e)
    {
      this.deleteField(this.gridViewLoanFields, true);
      this.refreshControls(true);
    }

    private void iconBtnFieldSearch_Click(object sender, EventArgs e)
    {
      this.searchField(this.gridViewLoanFields, true);
      this.refreshControls(true);
    }

    private void iconBtnFieldNew_Click(object sender, EventArgs e)
    {
      this.addFields(this.gridViewLoanFields, (List<string>) null);
      this.refreshControls(true);
    }

    private void gridViewLoanFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshControls(true);
    }

    private void btnLockPreview_Click(object sender, EventArgs e)
    {
      this.previewScreen(this.gridViewLockFields);
    }

    private void iconBtnLockDown_Click(object sender, EventArgs e)
    {
      this.gridViewLockFields.SelectedIndexChanged -= new EventHandler(this.gridViewLockFields_SelectedIndexChanged);
      this.moveFieldDown(this.gridViewLockFields);
      this.gridViewLockFields.SelectedIndexChanged += new EventHandler(this.gridViewLockFields_SelectedIndexChanged);
    }

    private void iconBtnLockUp_Click(object sender, EventArgs e)
    {
      this.gridViewLockFields.SelectedIndexChanged -= new EventHandler(this.gridViewLockFields_SelectedIndexChanged);
      this.moveFieldUp(this.gridViewLockFields);
      this.gridViewLockFields.SelectedIndexChanged += new EventHandler(this.gridViewLockFields_SelectedIndexChanged);
    }

    private void iconBtnLockDelete_Click(object sender, EventArgs e)
    {
      this.deleteField(this.gridViewLockFields, false);
      this.refreshControls(false);
    }

    private void iconBtnLockSearch_Click(object sender, EventArgs e)
    {
      this.searchField(this.gridViewLockFields, false);
      this.refreshControls(false);
    }

    private void iconBtnLockNew_Click(object sender, EventArgs e)
    {
      this.addFields(this.gridViewLockFields, this.fieldsNotAllowed);
      this.refreshControls(false);
    }

    private void gridViewLockFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshControls(false);
    }

    public override void Reset()
    {
      this.gridViewLoanFields.Items.Clear();
      this.gridViewLockFields.Items.Clear();
      this.existRequestFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.existLoanFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.fieldsNotAllowed = new List<string>();
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      LoanXDBTableList loanXdbTableList = this.session.LoanManager.GetLoanXDBTableList(false);
      if (loanXdbTableList != null)
        this.reportingFields = loanXdbTableList.GetFields();
      this.refresh();
    }

    private void refresh()
    {
      LRAdditionalFields additionalFields = this.session.ConfigurationManager.GetLRAdditionalFields();
      this.buildGridViewItem(additionalFields.GetFields(false), false, this.gridViewLoanFields, true);
      this.buildGridViewItem(additionalFields.GetFields(true), false, this.gridViewLockFields, false);
      this.refreshControls(true);
      this.refreshControls(false);
      this.setDirtyFlag(false);
    }

    public bool isLockRequestTabSelected => this.tabControlEx1.SelectedPage == this.tabPageEx1;

    public bool SelectLockRequestTab
    {
      set
      {
        if (value)
          this.tabPageEx1.Select();
        else
          this.tabPageEx2.Select();
      }
    }

    public string[] SelectedFields
    {
      get
      {
        List<string> stringList = new List<string>();
        if (this.tabControlEx1.SelectedPage == this.tabPageEx1)
        {
          foreach (GVItem selectedItem in this.gridViewLockFields.SelectedItems)
          {
            if (selectedItem.Selected)
              stringList.Add(selectedItem.Text + "1");
          }
        }
        else
        {
          foreach (GVItem selectedItem in this.gridViewLoanFields.SelectedItems)
          {
            if (selectedItem.Selected)
              stringList.Add(selectedItem.Text + "0");
          }
        }
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        if (value[0].EndsWith("1"))
        {
          this.tabControlEx1.SelectedPage = this.tabPageEx1;
          foreach (string str1 in value)
          {
            string str2 = str1.Substring(0, str1.Length - 1);
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewLockFields.Items)
            {
              if (gvItem.Text == str2)
              {
                gvItem.Selected = true;
                break;
              }
            }
          }
        }
        else
        {
          this.tabControlEx1.SelectedPage = this.tabPageEx2;
          foreach (string str3 in value)
          {
            string str4 = str3.Substring(0, str3.Length - 1);
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewLoanFields.Items)
            {
              if (gvItem.Text == str4)
              {
                gvItem.Selected = true;
                break;
              }
            }
          }
        }
      }
    }

    public override void Save()
    {
      LRAdditionalFields fields = new LRAdditionalFields();
      for (int nItemIndex = 0; nItemIndex < this.gridViewLoanFields.Items.Count; ++nItemIndex)
        fields.AddField(this.gridViewLoanFields.Items[nItemIndex].Text, false);
      for (int nItemIndex = 0; nItemIndex < this.gridViewLockFields.Items.Count; ++nItemIndex)
        fields.AddField(this.gridViewLockFields.Items[nItemIndex].Text, true);
      this.session.ConfigurationManager.UpdateLRAdditionalFields(fields);
      base.Save();
      this.setDirtyFlag(false);
    }

    private void previewScreen(GridView targetGridView)
    {
      string[] requiredFields = new string[targetGridView.Items.Count];
      for (int nItemIndex = 0; nItemIndex < targetGridView.Items.Count; ++nItemIndex)
        requiredFields[nItemIndex] = targetGridView.Items[nItemIndex].Text;
      using (FieldQuickEditForm fieldQuickEditForm = new FieldQuickEditForm(this.session, requiredFields))
      {
        int num = (int) fieldQuickEditForm.ShowDialog((IWin32Window) this);
      }
    }

    private void moveFieldDown(GridView targetGridView)
    {
      if (targetGridView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field from the list to move down.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        targetGridView.BeginUpdate();
        for (int index1 = targetGridView.SelectedItems.Count - 1; index1 >= 0; --index1)
        {
          int index2 = targetGridView.SelectedItems[index1].Index;
          int num2 = index2 + 1;
          if (num2 < targetGridView.Items.Count)
          {
            if (index1 < targetGridView.SelectedItems.Count - 1)
            {
              int index3 = targetGridView.SelectedItems[index1 + 1].Index;
              if (num2 == index3)
                continue;
            }
            GVItem gvItem = targetGridView.Items[index2];
            targetGridView.Items.RemoveAt(index2);
            targetGridView.Items.Insert(num2, gvItem);
            targetGridView.Items[num2].Selected = true;
          }
        }
        targetGridView.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void moveFieldUp(GridView targetGridView)
    {
      if (targetGridView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field from the list to move up.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        targetGridView.BeginUpdate();
        for (int index1 = 0; index1 < targetGridView.SelectedItems.Count; ++index1)
        {
          int index2 = targetGridView.SelectedItems[index1].Index;
          int num2 = index2 - 1;
          if (num2 >= 0)
          {
            if (index1 > 0)
            {
              int index3 = targetGridView.SelectedItems[index1 - 1].Index;
              if (num2 == index3)
                continue;
            }
            GVItem gvItem = targetGridView.Items[index2];
            targetGridView.Items.RemoveAt(index2);
            targetGridView.Items.Insert(num2, gvItem);
            targetGridView.Items[num2].Selected = true;
          }
        }
        targetGridView.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void deleteField(GridView targetGridView, bool forLoanView)
    {
      if (targetGridView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index1 = targetGridView.SelectedItems[0].Index;
        List<GVItem> gvItemList = new List<GVItem>();
        for (int index2 = 0; index2 < targetGridView.SelectedItems.Count; ++index2)
        {
          if (!forLoanView && this.reportingFields != null && (this.reportingFields.ContainsKey((object) LockRequestCustomField.GenerateCustomFieldID(targetGridView.SelectedItems[index2].Text)) || this.reportingFields.ContainsKey((object) RateLockField.GenerateRateLockFieldID(targetGridView.SelectedItems[index2].Text, RateLockField.RateLockOrder.MostRecent)) || this.reportingFields.ContainsKey((object) RateLockField.GenerateRateLockFieldID(targetGridView.SelectedItems[index2].Text, RateLockField.RateLockOrder.Previous)) || this.reportingFields.ContainsKey((object) RateLockField.GenerateRateLockFieldID(targetGridView.SelectedItems[index2].Text, RateLockField.RateLockOrder.Previous2)) || this.reportingFields.ContainsKey((object) RateLockField.GenerateRateLockFieldID(targetGridView.SelectedItems[index2].Text, RateLockField.RateLockOrder.MostRecentRequest))))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Field '" + LockRequestCustomField.GenerateCustomFieldID(targetGridView.SelectedItems[index2].Text) + "' has been used in reporting database. Please remove this field from reporting database first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          gvItemList.Add(targetGridView.SelectedItems[index2]);
        }
        for (int index3 = 0; index3 < gvItemList.Count; ++index3)
        {
          if (forLoanView)
          {
            if (this.existLoanFields.ContainsKey((object) gvItemList[index3].Text))
              this.existLoanFields.Remove((object) gvItemList[index3].Text);
          }
          else if (this.existRequestFields.ContainsKey((object) gvItemList[index3].Text))
            this.existRequestFields.Remove((object) gvItemList[index3].Text);
          targetGridView.Items.Remove(gvItemList[index3]);
        }
        this.setDirtyFlag(true);
        if (targetGridView.Items.Count == 0)
          return;
        if (index1 + 1 > targetGridView.Items.Count)
          targetGridView.Items[targetGridView.Items.Count - 1].Selected = true;
        else
          targetGridView.Items[index1].Selected = true;
      }
    }

    private void addFields(GridView targetGridView, List<string> notAllowed)
    {
      targetGridView.SelectedItems.Clear();
      using (AddFields addFields = new AddFields(this.session, notAllowed))
      {
        addFields.Options = AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowButtons | AddFieldOptions.AllowHiddenFields;
        this.forLoanField = targetGridView.Columns.Count < 4;
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.buildGridViewItem(addFields.SelectedFieldIDs, true, targetGridView, this.forLoanField);
        this.refreshControls(this.forLoanField);
      }
    }

    private void searchField(GridView targetGridView, bool forLoanView)
    {
      targetGridView.SelectedItems.Clear();
      string[] existingFields = new string[targetGridView.Items.Count];
      for (int nItemIndex = 0; nItemIndex < targetGridView.Items.Count; ++nItemIndex)
        existingFields[nItemIndex] = targetGridView.Items[nItemIndex].Text;
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, existingFields, true, string.Empty, false, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
          return;
        this.buildGridViewItem(ruleFindFieldDialog.SelectedRequiredFields, true, targetGridView, forLoanView);
      }
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null || addFields.SelectedFieldIDs == null)
        return;
      this.buildGridViewItem(addFields.SelectedFieldIDs, true, this.forLoanField ? this.gridViewLoanFields : this.gridViewLockFields, this.forLoanField);
      this.refreshControls(this.forLoanField);
    }

    private void buildGridViewItem(
      string[] fieldIDs,
      bool selected,
      GridView targetGridView,
      bool forLoanView)
    {
      int count = targetGridView.Items.Count;
      targetGridView.BeginUpdate();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < fieldIDs.Length; ++index)
      {
        if (this.fieldsNotAllowed.Contains(fieldIDs[index]))
        {
          if (empty2 != string.Empty)
            empty2 += ",";
          empty2 += fieldIDs[index];
        }
        else if (!forLoanView && LockRequestLog.LoanInfoSnapshotFields.Contains(fieldIDs[index]))
        {
          if (empty1 != string.Empty)
            empty1 += ",";
          empty1 += fieldIDs[index];
        }
        else
        {
          GVItem gvItem = new GVItem(fieldIDs[index]);
          gvItem.SubItems.Add((object) this.getFieldDescription(fieldIDs[index]));
          if (forLoanView)
          {
            if (!this.existLoanFields.ContainsKey((object) fieldIDs[index]))
              this.existLoanFields.Add((object) fieldIDs[index], (object) "");
            else
              continue;
          }
          else
          {
            gvItem.SubItems.Add((object) LockRequestCustomField.GenerateCustomFieldID(fieldIDs[index]));
            if (!this.existRequestFields.ContainsKey((object) fieldIDs[index]))
              this.existRequestFields.Add((object) fieldIDs[index], (object) "");
            else
              continue;
          }
          if (CustomFieldInfo.IsCustomFieldID(fieldIDs[index]))
            gvItem.SubItems.Add((object) "Custom");
          else
            gvItem.SubItems.Add((object) "Standard");
          gvItem.Selected = selected;
          targetGridView.Items.Add(gvItem);
        }
      }
      targetGridView.EndUpdate();
      if (((empty1 != string.Empty ? 1 : (empty2 != string.Empty ? 1 : 0)) & (selected ? 1 : 0)) != 0)
      {
        string text = string.Empty;
        if (empty2 != string.Empty)
          text = "The following fields can't be added to standard Lock Request Form:\r\n" + empty2;
        if (text != string.Empty && empty1 != string.Empty)
          text += "\r\n\r\n";
        if (empty1 != string.Empty)
          text = text + "The standard Lock Request Form already contains the following fields:\r\n\r\n" + empty1 + "\r\n\r\nThese fields will not be added to list.";
        int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (count == targetGridView.Items.Count)
        return;
      this.setDirtyFlag(true);
    }

    private string getFieldDescription(string id)
    {
      FieldDefinition fieldDefinition = EncompassFields.GetField(id, this.fieldSettings);
      if (fieldDefinition == null && StandardFields.AllVirtualFields.Contains(id))
        fieldDefinition = StandardFields.AllVirtualFields[id];
      if (id.ToLower().StartsWith("cx."))
      {
        if (this.fieldSettings.CustomFields.GetField(id) == null)
          return string.Empty;
        CustomField customField = new CustomField(this.fieldSettings.CustomFields.GetField(id));
        return customField != null ? customField.Description : string.Empty;
      }
      return fieldDefinition != null ? fieldDefinition.Description : string.Empty;
    }

    private void refreshControls(bool forLoanView)
    {
      if (forLoanView)
      {
        this.grpContainerLoan.Text = "Additional Fields (" + (object) this.gridViewLoanFields.Items.Count + ")";
        int count = this.gridViewLoanFields.SelectedItems.Count;
        this.iconBtnFieldDelete.Enabled = count > 0;
        this.iconBtnFieldDown.Enabled = count > 0;
        this.iconBtnFieldUp.Enabled = count > 0;
      }
      else
      {
        this.grpContainerLock.Text = "Additional Fields (" + (object) this.gridViewLockFields.Items.Count + ")";
        int count = this.gridViewLockFields.SelectedItems.Count;
        this.iconBtnLockDelete.Enabled = count > 0;
        this.iconBtnLockDown.Enabled = count > 0;
        this.iconBtnLockUp.Enabled = count > 0;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AdditionalFieldListControl));
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageEx1 = new TabPageEx();
      this.panel2 = new Panel();
      this.grpContainerLock = new GroupContainer();
      this.iconBtnLockNew = new StandardIconButton();
      this.iconBtnLockSearch = new StandardIconButton();
      this.iconBtnLockDelete = new StandardIconButton();
      this.iconBtnLockUp = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnLockPreview = new Button();
      this.iconBtnLockDown = new StandardIconButton();
      this.gridViewLockFields = new GridView();
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.tabPageEx2 = new TabPageEx();
      this.panel4 = new Panel();
      this.grpContainerLoan = new GroupContainer();
      this.iconBtnFieldNew = new StandardIconButton();
      this.iconBtnFieldSearch = new StandardIconButton();
      this.iconBtnFieldDelete = new StandardIconButton();
      this.iconBtnFieldUp = new StandardIconButton();
      this.verticalSeparator2 = new VerticalSeparator();
      this.btnFieldPreview = new Button();
      this.iconBtnFieldDown = new StandardIconButton();
      this.gridViewLoanFields = new GridView();
      this.panel3 = new Panel();
      this.label2 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.tabControlEx1.SuspendLayout();
      this.tabPageEx1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grpContainerLock.SuspendLayout();
      ((ISupportInitialize) this.iconBtnLockNew).BeginInit();
      ((ISupportInitialize) this.iconBtnLockSearch).BeginInit();
      ((ISupportInitialize) this.iconBtnLockDelete).BeginInit();
      ((ISupportInitialize) this.iconBtnLockUp).BeginInit();
      ((ISupportInitialize) this.iconBtnLockDown).BeginInit();
      this.panel1.SuspendLayout();
      this.tabPageEx2.SuspendLayout();
      this.panel4.SuspendLayout();
      this.grpContainerLoan.SuspendLayout();
      ((ISupportInitialize) this.iconBtnFieldNew).BeginInit();
      ((ISupportInitialize) this.iconBtnFieldSearch).BeginInit();
      ((ISupportInitialize) this.iconBtnFieldDelete).BeginInit();
      ((ISupportInitialize) this.iconBtnFieldUp).BeginInit();
      ((ISupportInitialize) this.iconBtnFieldDown).BeginInit();
      this.panel3.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tabControlEx1.Dock = DockStyle.Fill;
      this.tabControlEx1.Location = new Point(3, 3);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(613, 443);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 0;
      this.tabControlEx1.TabPages.Add(this.tabPageEx1);
      this.tabControlEx1.TabPages.Add(this.tabPageEx2);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabPageEx1.BackColor = Color.Transparent;
      this.tabPageEx1.Controls.Add((Control) this.panel2);
      this.tabPageEx1.Controls.Add((Control) this.panel1);
      this.tabPageEx1.Location = new Point(1, 23);
      this.tabPageEx1.Name = "tabPageEx1";
      this.tabPageEx1.Padding = new Padding(3);
      this.tabPageEx1.TabIndex = 0;
      this.tabPageEx1.TabWidth = 100;
      this.tabPageEx1.Text = "Lock Request Form";
      this.tabPageEx1.Value = (object) "Lock Request Form";
      this.panel2.Controls.Add((Control) this.grpContainerLock);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(3, 58);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(603, 356);
      this.panel2.TabIndex = 3;
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockNew);
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockSearch);
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockDelete);
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockUp);
      this.grpContainerLock.Controls.Add((Control) this.verticalSeparator1);
      this.grpContainerLock.Controls.Add((Control) this.btnLockPreview);
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockDown);
      this.grpContainerLock.Controls.Add((Control) this.gridViewLockFields);
      this.grpContainerLock.Dock = DockStyle.Fill;
      this.grpContainerLock.HeaderForeColor = SystemColors.ControlText;
      this.grpContainerLock.Location = new Point(0, 0);
      this.grpContainerLock.Name = "grpContainerLock";
      this.grpContainerLock.Size = new Size(603, 356);
      this.grpContainerLock.TabIndex = 0;
      this.grpContainerLock.Text = "Additional Fields (0)";
      this.iconBtnLockNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockNew.BackColor = Color.Transparent;
      this.iconBtnLockNew.Location = new Point(403, 4);
      this.iconBtnLockNew.MouseDownImage = (Image) null;
      this.iconBtnLockNew.Name = "iconBtnLockNew";
      this.iconBtnLockNew.Size = new Size(16, 17);
      this.iconBtnLockNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.iconBtnLockNew.TabIndex = 8;
      this.iconBtnLockNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockNew, "New");
      this.iconBtnLockNew.Click += new EventHandler(this.iconBtnLockNew_Click);
      this.iconBtnLockSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockSearch.BackColor = Color.Transparent;
      this.iconBtnLockSearch.Location = new Point(425, 4);
      this.iconBtnLockSearch.MouseDownImage = (Image) null;
      this.iconBtnLockSearch.Name = "iconBtnLockSearch";
      this.iconBtnLockSearch.Size = new Size(16, 17);
      this.iconBtnLockSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.iconBtnLockSearch.TabIndex = 7;
      this.iconBtnLockSearch.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockSearch, "Find");
      this.iconBtnLockSearch.Click += new EventHandler(this.iconBtnLockSearch_Click);
      this.iconBtnLockDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockDelete.BackColor = Color.Transparent;
      this.iconBtnLockDelete.Location = new Point(447, 4);
      this.iconBtnLockDelete.MouseDownImage = (Image) null;
      this.iconBtnLockDelete.Name = "iconBtnLockDelete";
      this.iconBtnLockDelete.Size = new Size(16, 17);
      this.iconBtnLockDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnLockDelete.TabIndex = 5;
      this.iconBtnLockDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockDelete, "Delete");
      this.iconBtnLockDelete.Click += new EventHandler(this.iconBtnLockDelete_Click);
      this.iconBtnLockUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockUp.BackColor = Color.Transparent;
      this.iconBtnLockUp.Location = new Point(469, 4);
      this.iconBtnLockUp.MouseDownImage = (Image) null;
      this.iconBtnLockUp.Name = "iconBtnLockUp";
      this.iconBtnLockUp.Size = new Size(16, 17);
      this.iconBtnLockUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.iconBtnLockUp.TabIndex = 4;
      this.iconBtnLockUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockUp, "Move Up");
      this.iconBtnLockUp.Click += new EventHandler(this.iconBtnLockUp_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(513, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 3;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnLockPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLockPreview.BackColor = SystemColors.Control;
      this.btnLockPreview.Location = new Point(521, 2);
      this.btnLockPreview.Name = "btnLockPreview";
      this.btnLockPreview.Size = new Size(75, 22);
      this.btnLockPreview.TabIndex = 2;
      this.btnLockPreview.Text = "Preview";
      this.btnLockPreview.UseVisualStyleBackColor = true;
      this.btnLockPreview.Click += new EventHandler(this.btnLockPreview_Click);
      this.iconBtnLockDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockDown.BackColor = Color.Transparent;
      this.iconBtnLockDown.Location = new Point(491, 4);
      this.iconBtnLockDown.MouseDownImage = (Image) null;
      this.iconBtnLockDown.Name = "iconBtnLockDown";
      this.iconBtnLockDown.Size = new Size(16, 17);
      this.iconBtnLockDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.iconBtnLockDown.TabIndex = 1;
      this.iconBtnLockDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockDown, "Move Down");
      this.iconBtnLockDown.Click += new EventHandler(this.iconBtnLockDown_Click);
      this.gridViewLockFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Original Field ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 260;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "New Field ID";
      gvColumn3.Width = 120;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 80;
      this.gridViewLockFields.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridViewLockFields.Dock = DockStyle.Fill;
      this.gridViewLockFields.Location = new Point(1, 26);
      this.gridViewLockFields.Name = "gridViewLockFields";
      this.gridViewLockFields.Size = new Size(601, 329);
      this.gridViewLockFields.SortIconVisible = false;
      this.gridViewLockFields.SortOption = GVSortOption.None;
      this.gridViewLockFields.TabIndex = 0;
      this.gridViewLockFields.SelectedIndexChanged += new EventHandler(this.gridViewLockFields_SelectedIndexChanged);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(603, 55);
      this.panel1.TabIndex = 2;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(592, 46);
      this.label1.TabIndex = 1;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.tabPageEx2.BackColor = Color.Transparent;
      this.tabPageEx2.Controls.Add((Control) this.panel4);
      this.tabPageEx2.Controls.Add((Control) this.panel3);
      this.tabPageEx2.Location = new Point(1, 23);
      this.tabPageEx2.Name = "tabPageEx2";
      this.tabPageEx2.Padding = new Padding(3);
      this.tabPageEx2.TabIndex = 0;
      this.tabPageEx2.TabWidth = 100;
      this.tabPageEx2.Text = "Loan Snapshot";
      this.tabPageEx2.Value = (object) "Loan Snapshot";
      this.panel4.Controls.Add((Control) this.grpContainerLoan);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(3, 54);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(194, 43);
      this.panel4.TabIndex = 5;
      this.grpContainerLoan.Controls.Add((Control) this.iconBtnFieldNew);
      this.grpContainerLoan.Controls.Add((Control) this.iconBtnFieldSearch);
      this.grpContainerLoan.Controls.Add((Control) this.iconBtnFieldDelete);
      this.grpContainerLoan.Controls.Add((Control) this.iconBtnFieldUp);
      this.grpContainerLoan.Controls.Add((Control) this.verticalSeparator2);
      this.grpContainerLoan.Controls.Add((Control) this.btnFieldPreview);
      this.grpContainerLoan.Controls.Add((Control) this.iconBtnFieldDown);
      this.grpContainerLoan.Controls.Add((Control) this.gridViewLoanFields);
      this.grpContainerLoan.Dock = DockStyle.Fill;
      this.grpContainerLoan.HeaderForeColor = SystemColors.ControlText;
      this.grpContainerLoan.Location = new Point(0, 0);
      this.grpContainerLoan.Name = "grpContainerLoan";
      this.grpContainerLoan.Size = new Size(194, 43);
      this.grpContainerLoan.TabIndex = 2;
      this.grpContainerLoan.Text = "Additional Fields (0)";
      this.iconBtnFieldNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnFieldNew.BackColor = Color.Transparent;
      this.iconBtnFieldNew.Location = new Point(16, 5);
      this.iconBtnFieldNew.MouseDownImage = (Image) null;
      this.iconBtnFieldNew.Name = "iconBtnFieldNew";
      this.iconBtnFieldNew.Size = new Size(16, 16);
      this.iconBtnFieldNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.iconBtnFieldNew.TabIndex = 16;
      this.iconBtnFieldNew.TabStop = false;
      this.iconBtnFieldNew.Click += new EventHandler(this.iconBtnFieldNew_Click);
      this.iconBtnFieldSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnFieldSearch.BackColor = Color.Transparent;
      this.iconBtnFieldSearch.Location = new Point(35, 5);
      this.iconBtnFieldSearch.MouseDownImage = (Image) null;
      this.iconBtnFieldSearch.Name = "iconBtnFieldSearch";
      this.iconBtnFieldSearch.Size = new Size(16, 16);
      this.iconBtnFieldSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.iconBtnFieldSearch.TabIndex = 15;
      this.iconBtnFieldSearch.TabStop = false;
      this.iconBtnFieldSearch.Click += new EventHandler(this.iconBtnFieldSearch_Click);
      this.iconBtnFieldDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnFieldDelete.BackColor = Color.Transparent;
      this.iconBtnFieldDelete.Location = new Point(54, 5);
      this.iconBtnFieldDelete.MouseDownImage = (Image) null;
      this.iconBtnFieldDelete.Name = "iconBtnFieldDelete";
      this.iconBtnFieldDelete.Size = new Size(16, 16);
      this.iconBtnFieldDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnFieldDelete.TabIndex = 13;
      this.iconBtnFieldDelete.TabStop = false;
      this.iconBtnFieldDelete.Click += new EventHandler(this.iconBtnFieldDelete_Click);
      this.iconBtnFieldUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnFieldUp.BackColor = Color.Transparent;
      this.iconBtnFieldUp.Location = new Point(73, 5);
      this.iconBtnFieldUp.MouseDownImage = (Image) null;
      this.iconBtnFieldUp.Name = "iconBtnFieldUp";
      this.iconBtnFieldUp.Size = new Size(16, 16);
      this.iconBtnFieldUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.iconBtnFieldUp.TabIndex = 12;
      this.iconBtnFieldUp.TabStop = false;
      this.iconBtnFieldUp.Click += new EventHandler(this.iconBtnFieldUp_Click);
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(113, 5);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 11;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.btnFieldPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFieldPreview.Location = new Point(117, 2);
      this.btnFieldPreview.Name = "btnFieldPreview";
      this.btnFieldPreview.Size = new Size(75, 22);
      this.btnFieldPreview.TabIndex = 10;
      this.btnFieldPreview.Text = "Preview";
      this.btnFieldPreview.UseVisualStyleBackColor = true;
      this.btnFieldPreview.Click += new EventHandler(this.btnFieldPreview_Click);
      this.iconBtnFieldDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnFieldDown.BackColor = Color.Transparent;
      this.iconBtnFieldDown.Location = new Point(92, 5);
      this.iconBtnFieldDown.MouseDownImage = (Image) null;
      this.iconBtnFieldDown.Name = "iconBtnFieldDown";
      this.iconBtnFieldDown.Size = new Size(16, 16);
      this.iconBtnFieldDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.iconBtnFieldDown.TabIndex = 9;
      this.iconBtnFieldDown.TabStop = false;
      this.iconBtnFieldDown.Click += new EventHandler(this.iconBtnFieldDown_Click);
      this.gridViewLoanFields.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Original Field ID";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.Text = "Description";
      gvColumn6.Width = 260;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column3";
      gvColumn7.Text = "Type";
      gvColumn7.Width = 80;
      this.gridViewLoanFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridViewLoanFields.Dock = DockStyle.Fill;
      this.gridViewLoanFields.Location = new Point(1, 26);
      this.gridViewLoanFields.Name = "gridViewLoanFields";
      this.gridViewLoanFields.Size = new Size(192, 16);
      this.gridViewLoanFields.SortOption = GVSortOption.None;
      this.gridViewLoanFields.TabIndex = 0;
      this.gridViewLoanFields.SelectedIndexChanged += new EventHandler(this.gridViewLoanFields_SelectedIndexChanged);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(3, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(194, 51);
      this.panel3.TabIndex = 4;
      this.label2.Location = new Point(6, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(607, 43);
      this.label2.TabIndex = 3;
      this.label2.Text = componentResourceManager.GetString("label2.Text");
      this.borderPanel1.Controls.Add((Control) this.tabControlEx1);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(2, 2, 0, 0);
      this.borderPanel1.Size = new Size(617, 447);
      this.borderPanel1.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.borderPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AdditionalFieldListControl);
      this.Size = new Size(617, 447);
      this.tabControlEx1.ResumeLayout(false);
      this.tabPageEx1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.grpContainerLock.ResumeLayout(false);
      ((ISupportInitialize) this.iconBtnLockNew).EndInit();
      ((ISupportInitialize) this.iconBtnLockSearch).EndInit();
      ((ISupportInitialize) this.iconBtnLockDelete).EndInit();
      ((ISupportInitialize) this.iconBtnLockUp).EndInit();
      ((ISupportInitialize) this.iconBtnLockDown).EndInit();
      this.panel1.ResumeLayout(false);
      this.tabPageEx2.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.grpContainerLoan.ResumeLayout(false);
      ((ISupportInitialize) this.iconBtnFieldNew).EndInit();
      ((ISupportInitialize) this.iconBtnFieldSearch).EndInit();
      ((ISupportInitialize) this.iconBtnFieldDelete).EndInit();
      ((ISupportInitialize) this.iconBtnFieldUp).EndInit();
      ((ISupportInitialize) this.iconBtnFieldDown).EndInit();
      this.panel3.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
