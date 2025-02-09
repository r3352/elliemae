// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactCustomFieldsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
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
  public class ContactCustomFieldsForm : SettingsUserControl
  {
    private const int NUMBER_OF_TABS = 6;
    protected const int FIELDS_PER_PAGE = 20;
    private const int CATEGORY_TAB_INDEX = 5;
    private const string CATEGORY_TAB_NAME = "Custom Category Fields";
    private const string NO_CATEGORY_NAME = "No Category";
    private TabPage selectedTabPage;
    protected EllieMae.EMLite.ContactUI.ContactType contactType;
    protected Label[] lblFieldNumbers = new Label[20];
    private TextBox[] txtFieldDescriptions = new TextBox[20];
    private ComboBox[] cboFieldTypes = new ComboBox[20];
    protected StandardIconButton[] btnFieldOptions = new StandardIconButton[20];
    private TextBox[] txtLoanFieldIds = new TextBox[20];
    private TextBox[] txtLoanFieldDescriptions = new TextBox[20];
    private StandardIconButton[] picLoanFieldSearches = new StandardIconButton[20];
    protected CheckBox[] chkTwoWayTransfers = new CheckBox[20];
    protected CheckBox[] chkSelect = new CheckBox[20];
    private bool initializingControls;
    protected FieldDefinitionCollection loanFields;
    protected CustomFieldsInfo loanCustomFields;
    protected LoanFieldMappingDialog loanFieldMappingDlg;
    protected CustomFieldMappingCollection customFieldMappingCollection;
    protected Hashtable _LabelIDToFieldHash;
    protected Hashtable _LabelIDToFieldHashInit;
    protected string[] pageNames = new string[6];
    protected BizCategory[] categories;
    private GroupContainer gcCustomFieldDefinitions;
    protected Splitter splitter1;
    private CustomFieldsDefinitionCollection categoryFieldsDefinitions = CustomFieldsDefinitionCollection.NewCustomFieldsDefinitionCollection();
    private Panel pnlCustomFieldsInternal;
    private PanelEx pnlExRightMapping;
    private BorderPanel borderPanel1;
    private StandardIconButton picLoanFieldSearch1;
    private StandardIconButton picLoanFieldSearch2;
    private StandardIconButton picLoanFieldSearch3;
    private StandardIconButton picLoanFieldSearch9;
    private StandardIconButton picLoanFieldSearch8;
    private StandardIconButton picLoanFieldSearch7;
    private StandardIconButton picLoanFieldSearch6;
    private StandardIconButton picLoanFieldSearch5;
    private StandardIconButton picLoanFieldSearch4;
    private StandardIconButton picLoanFieldSearch20;
    private StandardIconButton picLoanFieldSearch19;
    private StandardIconButton picLoanFieldSearch18;
    private StandardIconButton picLoanFieldSearch17;
    private StandardIconButton picLoanFieldSearch16;
    private StandardIconButton picLoanFieldSearch15;
    private StandardIconButton picLoanFieldSearch14;
    private StandardIconButton picLoanFieldSearch13;
    private StandardIconButton picLoanFieldSearch12;
    private StandardIconButton picLoanFieldSearch11;
    private StandardIconButton picLoanFieldSearch10;
    private GroupContainer gcCustomTab;
    private BorderPanel borderPanel2;
    private GradientPanel gradientPanel1;
    private GradientPanel gradientPanel2;
    private BorderPanel borderPanel3;
    private Panel pnlData;
    private CheckBox chkSelect20;
    private CheckBox chkSelect15;
    private CheckBox chkSelect6;
    private CheckBox chkSelect14;
    private CheckBox chkSelect5;
    private CheckBox chkSelect19;
    private CheckBox chkSelect4;
    private CheckBox chkSelect7;
    private CheckBox chkSelect13;
    private CheckBox chkSelect3;
    private CheckBox chkSelect2;
    private CheckBox chkSelect16;
    private CheckBox chkSelect8;
    private CheckBox chkSelect12;
    private CheckBox chkSelect1;
    private CheckBox chkSelect18;
    protected Label lblSelect;
    private CheckBox chkSelect9;
    private CheckBox chkSelect11;
    private CheckBox chkSelect10;
    private CheckBox chkSelect17;
    private bool disableSetDirtyFlag;
    private List<string> selectedFieldsForSync = new List<string>();
    private List<string> selectedCategoryFieldsForSync = new List<string>();
    private Sessions.Session session;
    private bool isSettingsSync;
    private IContainer components;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TabPage tabPage3;
    private TabPage tabPage4;
    private TabPage tabPage5;
    protected TabControl tabCustomFields;
    private ToolTip tipCustomField;
    protected Label lblDirections;
    protected TextBox txtPageName;
    private ComboBox cboCategoryName;
    private StandardIconButton btnFieldOption20;
    private StandardIconButton btnFieldOption19;
    private StandardIconButton btnFieldOption18;
    private StandardIconButton btnFieldOption17;
    private StandardIconButton btnFieldOption16;
    private StandardIconButton btnFieldOption15;
    private StandardIconButton btnFieldOption14;
    private StandardIconButton btnFieldOption13;
    private StandardIconButton btnFieldOption12;
    private StandardIconButton btnFieldOption11;
    private StandardIconButton btnFieldOption10;
    private StandardIconButton btnFieldOption9;
    private StandardIconButton btnFieldOption8;
    private StandardIconButton btnFieldOption7;
    private StandardIconButton btnFieldOption6;
    private StandardIconButton btnFieldOption5;
    private StandardIconButton btnFieldOption4;
    private StandardIconButton btnFieldOption3;
    private StandardIconButton btnFieldOption2;
    private StandardIconButton btnFieldOption1;
    private Label lblFieldNumber20;
    private ComboBox cboFieldType20;
    private TextBox txtFieldDescription20;
    private Label lblFieldNumber19;
    private ComboBox cboFieldType19;
    private TextBox txtFieldDescription19;
    private Label lblFieldNumber18;
    private ComboBox cboFieldType18;
    private TextBox txtFieldDescription18;
    private Label lblFieldNumber17;
    private ComboBox cboFieldType17;
    private TextBox txtFieldDescription17;
    private Label lblFieldNumber16;
    private ComboBox cboFieldType16;
    private TextBox txtFieldDescription16;
    private Label lblFieldNumber15;
    private ComboBox cboFieldType15;
    private TextBox txtFieldDescription15;
    private Label lblFieldNumber14;
    private ComboBox cboFieldType14;
    private TextBox txtFieldDescription14;
    private Label lblFieldNumber13;
    private ComboBox cboFieldType13;
    private TextBox txtFieldDescription13;
    private Label lblFieldNumber12;
    private ComboBox cboFieldType12;
    private TextBox txtFieldDescription12;
    private Label lblFieldNumber11;
    private ComboBox cboFieldType11;
    private TextBox txtFieldDescription11;
    private Label lblFieldNumber10;
    private ComboBox cboFieldType10;
    private TextBox txtFieldDescription10;
    private Label lblFieldNumber9;
    private ComboBox cboFieldType9;
    private TextBox txtFieldDescription9;
    private Label lblFieldNumber8;
    private ComboBox cboFieldType8;
    private TextBox txtFieldDescription8;
    private Label lblFieldNumber7;
    private ComboBox cboFieldType7;
    private TextBox txtFieldDescription7;
    private Label lblFieldNumber6;
    private ComboBox cboFieldType6;
    private TextBox txtFieldDescription6;
    private Label lblFieldNumber5;
    private ComboBox cboFieldType5;
    private TextBox txtFieldDescription5;
    private Label lblFieldNumber4;
    private ComboBox cboFieldType4;
    private TextBox txtFieldDescription4;
    private Label lblFieldNumber3;
    private ComboBox cboFieldType3;
    private TextBox txtFieldDescription3;
    private Label lblFieldNumber2;
    private ComboBox cboFieldType2;
    private TextBox txtFieldDescription2;
    private Label lblFieldNumber1;
    private ComboBox cboFieldType1;
    private Label lblFieldType;
    private Label lblFieldDescription;
    private TextBox txtFieldDescription1;
    private CheckBox chkTwoWayTransfer20;
    private CheckBox chkTwoWayTransfer19;
    private CheckBox chkTwoWayTransfer18;
    private CheckBox chkTwoWayTransfer17;
    private CheckBox chkTwoWayTransfer16;
    private CheckBox chkTwoWayTransfer15;
    private CheckBox chkTwoWayTransfer14;
    private CheckBox chkTwoWayTransfer13;
    private CheckBox chkTwoWayTransfer12;
    private CheckBox chkTwoWayTransfer11;
    private CheckBox chkTwoWayTransfer10;
    private CheckBox chkTwoWayTransfer9;
    private CheckBox chkTwoWayTransfer8;
    private CheckBox chkTwoWayTransfer7;
    private CheckBox chkTwoWayTransfer6;
    private CheckBox chkTwoWayTransfer5;
    private CheckBox chkTwoWayTransfer4;
    private CheckBox chkTwoWayTransfer3;
    private CheckBox chkTwoWayTransfer2;
    private CheckBox chkTwoWayTransfer1;
    protected Label lblDirection;
    private TextBox txtLoanFieldDescription20;
    private TextBox txtLoanFieldDescription19;
    private TextBox txtLoanFieldDescription18;
    private TextBox txtLoanFieldDescription17;
    private TextBox txtLoanFieldDescription16;
    private TextBox txtLoanFieldDescription15;
    private TextBox txtLoanFieldDescription14;
    private TextBox txtLoanFieldDescription13;
    private TextBox txtLoanFieldDescription12;
    private TextBox txtLoanFieldDescription11;
    private TextBox txtLoanFieldDescription10;
    private TextBox txtLoanFieldDescription9;
    private TextBox txtLoanFieldDescription8;
    private TextBox txtLoanFieldDescription7;
    private TextBox txtLoanFieldDescription6;
    private TextBox txtLoanFieldDescription5;
    private TextBox txtLoanFieldDescription4;
    private TextBox txtLoanFieldDescription3;
    private TextBox txtLoanFieldDescription2;
    private TextBox txtLoanFieldDescription1;
    private Label lblLoanFieldDescription;
    private TextBox txtLoanFieldId20;
    private TextBox txtLoanFieldId19;
    private TextBox txtLoanFieldId18;
    private TextBox txtLoanFieldId17;
    private TextBox txtLoanFieldId16;
    private TextBox txtLoanFieldId15;
    private TextBox txtLoanFieldId14;
    private TextBox txtLoanFieldId13;
    private TextBox txtLoanFieldId12;
    private TextBox txtLoanFieldId11;
    private TextBox txtLoanFieldId10;
    private TextBox txtLoanFieldId9;
    private TextBox txtLoanFieldId8;
    private TextBox txtLoanFieldId7;
    private TextBox txtLoanFieldId6;
    private TextBox txtLoanFieldId5;
    private TextBox txtLoanFieldId4;
    private TextBox txtLoanFieldId3;
    private TextBox txtLoanFieldId2;
    protected Label lblLoanFieldMapping;
    private Label lblLoanFieldId;
    private TextBox txtLoanFieldId1;

    public ContactCustomFieldsForm(SetUpContainer setupContainer, EllieMae.EMLite.ContactUI.ContactType contactType)
      : this(setupContainer, contactType, Session.DefaultInstance, false)
    {
    }

    public ContactCustomFieldsForm(
      SetUpContainer setupContainer,
      EllieMae.EMLite.ContactUI.ContactType contactType,
      Sessions.Session session,
      bool isSettingsSync)
      : base(setupContainer)
    {
      this.contactType = contactType;
      this.session = session;
      this.isSettingsSync = isSettingsSync;
      this.InitializeComponent();
      this.initializeControls(isSettingsSync);
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      this.gcCustomTab.Focus();
      if (!this.validateContactFields() || !this.validateCategoryFields())
        return;
      ArrayList invalidFieldIds = new ArrayList();
      if ((this.checkContactDataLoss(invalidFieldIds) || this.checkCategoryDataLoss()) && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Deleting a custom field or changing its field type may result in previously entered data being lost. Do you want to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        return;
      this.saveContactFields(invalidFieldIds);
      this.saveCategoryFields();
      this.setDirtyFlag(false);
      this.Reset();
    }

    public override void Reset()
    {
      this.loanFields = StandardFields.All;
      this.loanCustomFields = this.session.ConfigurationManager.GetLoanCustomFields();
      this.customFieldMappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(this.session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower | CustomFieldsType.BizPartner | CustomFieldsType.BizCategoryCustom, false));
      this.loanFieldMappingDlg = new LoanFieldMappingDialog(this.loanFields, this.loanCustomFields, this.contactType, this.customFieldMappingCollection, this.categories);
      ContactCustomFieldInfoCollection customFieldInfo = this.session.ContactManager.GetCustomFieldInfo(this.contactType);
      ContactCustomFieldInfo[] items = customFieldInfo.Items;
      this._LabelIDToFieldHash = new Hashtable(items.Length);
      for (int index = 0; index < items.Length; ++index)
        this._LabelIDToFieldHash.Add((object) items[index].LabelID, (object) items[index]);
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = Utils.DeepClone<ContactCustomFieldInfo[]>(items);
      this._LabelIDToFieldHashInit = new Hashtable(contactCustomFieldInfoArray.Length);
      for (int index = 0; index < contactCustomFieldInfoArray.Length; ++index)
        this._LabelIDToFieldHashInit.Add((object) contactCustomFieldInfoArray[index].LabelID, (object) contactCustomFieldInfoArray[index]);
      if (EllieMae.EMLite.ContactUI.ContactType.BizPartner == this.contactType && this.session.EncompassEdition != EncompassEdition.Broker)
      {
        this.categoryFieldsDefinitions = CustomFieldsDefinitionCollection.NewCustomFieldsDefinitionCollection();
        this.loadCategoryCustomFields();
      }
      this.pageNames[0] = customFieldInfo.Page1Name;
      this.pageNames[1] = customFieldInfo.Page2Name;
      this.pageNames[2] = customFieldInfo.Page3Name;
      this.pageNames[3] = customFieldInfo.Page4Name;
      this.pageNames[4] = customFieldInfo.Page5Name;
      this.pageNames[5] = "Custom Category Fields";
      this.displayCustomFieldTab(this.tabCustomFields.SelectedIndex);
      this.setDirtyFlag(false);
    }

    protected override void setDirtyFlag(bool val)
    {
      if (this.disableSetDirtyFlag)
        return;
      base.setDirtyFlag(val);
      if (this.isSettingsSync)
        return;
      this.setupContainer.ButtonSaveEnabled = this.setupContainer.ButtonResetEnabled = this.IsDirty;
    }

    private void initializeControls(bool isSettingsSync)
    {
      this.disableSetDirtyFlag = true;
      try
      {
        this.initializeControlArrays(isSettingsSync);
        if (!isSettingsSync)
        {
          this.lblSelect.Visible = false;
          for (int index = 0; index < this.chkSelect.Length; ++index)
            this.chkSelect[index].Visible = false;
        }
        for (int index = 0; index < this.cboFieldTypes.Length; ++index)
        {
          this.cboFieldTypes[index].Items.Clear();
          this.cboFieldTypes[index].Items.AddRange(FieldFormatEnumUtil.GetDisplayNames(true, this.contactType != EllieMae.EMLite.ContactUI.ContactType.TPO));
          this.cboFieldTypes[index].Items.Remove((object) "AUDIT");
          this.cboFieldTypes[index].Sorted = true;
        }
        BizCategory[] bizCategories = this.session.ContactManager.GetBizCategories();
        Array.Sort<BizCategory>(bizCategories);
        this.categories = new BizCategory[bizCategories.Length - 1];
        int num = 0;
        foreach (BizCategory bizCategory in bizCategories)
        {
          if (!("No Category" == bizCategory.Name))
            this.categories[num++] = bizCategory;
        }
        if (EllieMae.EMLite.ContactUI.ContactType.BizPartner == this.contactType && this.session.EncompassEdition != EncompassEdition.Broker)
        {
          TabPage tabPage = new TabPage();
          tabPage.BackColor = Color.WhiteSmoke;
          tabPage.AutoScroll = true;
          tabPage.Location = new Point(4, 22);
          tabPage.Name = "tpgCategoryFields";
          tabPage.Size = new Size(592, 598);
          tabPage.TabIndex = 0;
          tabPage.Text = "Custom Category Fields";
          this.tabCustomFields.TabPages.Add(tabPage);
          this.cboCategoryName.DataSource = (object) this.categories;
          this.cboCategoryName.DisplayMember = "Name";
          this.cboCategoryName.ValueMember = "CategoryID";
          this.cboCategoryName.SelectedIndexChanged += new EventHandler(this.cboCategoryName_SelectedIndexChanged);
        }
        this.selectedTabPage = this.tabPage1;
      }
      finally
      {
        this.disableSetDirtyFlag = false;
      }
    }

    private void initializeControlArrays(bool isSettingsSync)
    {
      this.lblFieldNumbers[0] = this.lblFieldNumber1;
      this.lblFieldNumbers[1] = this.lblFieldNumber2;
      this.lblFieldNumbers[2] = this.lblFieldNumber3;
      this.lblFieldNumbers[3] = this.lblFieldNumber4;
      this.lblFieldNumbers[4] = this.lblFieldNumber5;
      this.lblFieldNumbers[5] = this.lblFieldNumber6;
      this.lblFieldNumbers[6] = this.lblFieldNumber7;
      this.lblFieldNumbers[7] = this.lblFieldNumber8;
      this.lblFieldNumbers[8] = this.lblFieldNumber9;
      this.lblFieldNumbers[9] = this.lblFieldNumber10;
      this.lblFieldNumbers[10] = this.lblFieldNumber11;
      this.lblFieldNumbers[11] = this.lblFieldNumber12;
      this.lblFieldNumbers[12] = this.lblFieldNumber13;
      this.lblFieldNumbers[13] = this.lblFieldNumber14;
      this.lblFieldNumbers[14] = this.lblFieldNumber15;
      this.lblFieldNumbers[15] = this.lblFieldNumber16;
      this.lblFieldNumbers[16] = this.lblFieldNumber17;
      this.lblFieldNumbers[17] = this.lblFieldNumber18;
      this.lblFieldNumbers[18] = this.lblFieldNumber19;
      this.lblFieldNumbers[19] = this.lblFieldNumber20;
      this.txtFieldDescriptions[0] = this.txtFieldDescription1;
      this.txtFieldDescriptions[1] = this.txtFieldDescription2;
      this.txtFieldDescriptions[2] = this.txtFieldDescription3;
      this.txtFieldDescriptions[3] = this.txtFieldDescription4;
      this.txtFieldDescriptions[4] = this.txtFieldDescription5;
      this.txtFieldDescriptions[5] = this.txtFieldDescription6;
      this.txtFieldDescriptions[6] = this.txtFieldDescription7;
      this.txtFieldDescriptions[7] = this.txtFieldDescription8;
      this.txtFieldDescriptions[8] = this.txtFieldDescription9;
      this.txtFieldDescriptions[9] = this.txtFieldDescription10;
      this.txtFieldDescriptions[10] = this.txtFieldDescription11;
      this.txtFieldDescriptions[11] = this.txtFieldDescription12;
      this.txtFieldDescriptions[12] = this.txtFieldDescription13;
      this.txtFieldDescriptions[13] = this.txtFieldDescription14;
      this.txtFieldDescriptions[14] = this.txtFieldDescription15;
      this.txtFieldDescriptions[15] = this.txtFieldDescription16;
      this.txtFieldDescriptions[16] = this.txtFieldDescription17;
      this.txtFieldDescriptions[17] = this.txtFieldDescription18;
      this.txtFieldDescriptions[18] = this.txtFieldDescription19;
      this.txtFieldDescriptions[19] = this.txtFieldDescription20;
      this.cboFieldTypes[0] = this.cboFieldType1;
      this.cboFieldTypes[1] = this.cboFieldType2;
      this.cboFieldTypes[2] = this.cboFieldType3;
      this.cboFieldTypes[3] = this.cboFieldType4;
      this.cboFieldTypes[4] = this.cboFieldType5;
      this.cboFieldTypes[5] = this.cboFieldType6;
      this.cboFieldTypes[6] = this.cboFieldType7;
      this.cboFieldTypes[7] = this.cboFieldType8;
      this.cboFieldTypes[8] = this.cboFieldType9;
      this.cboFieldTypes[9] = this.cboFieldType10;
      this.cboFieldTypes[10] = this.cboFieldType11;
      this.cboFieldTypes[11] = this.cboFieldType12;
      this.cboFieldTypes[12] = this.cboFieldType13;
      this.cboFieldTypes[13] = this.cboFieldType14;
      this.cboFieldTypes[14] = this.cboFieldType15;
      this.cboFieldTypes[15] = this.cboFieldType16;
      this.cboFieldTypes[16] = this.cboFieldType17;
      this.cboFieldTypes[17] = this.cboFieldType18;
      this.cboFieldTypes[18] = this.cboFieldType19;
      this.cboFieldTypes[19] = this.cboFieldType20;
      this.btnFieldOptions[0] = this.btnFieldOption1;
      this.btnFieldOptions[1] = this.btnFieldOption2;
      this.btnFieldOptions[2] = this.btnFieldOption3;
      this.btnFieldOptions[3] = this.btnFieldOption4;
      this.btnFieldOptions[4] = this.btnFieldOption5;
      this.btnFieldOptions[5] = this.btnFieldOption6;
      this.btnFieldOptions[6] = this.btnFieldOption7;
      this.btnFieldOptions[7] = this.btnFieldOption8;
      this.btnFieldOptions[8] = this.btnFieldOption9;
      this.btnFieldOptions[9] = this.btnFieldOption10;
      this.btnFieldOptions[10] = this.btnFieldOption11;
      this.btnFieldOptions[11] = this.btnFieldOption12;
      this.btnFieldOptions[12] = this.btnFieldOption13;
      this.btnFieldOptions[13] = this.btnFieldOption14;
      this.btnFieldOptions[14] = this.btnFieldOption15;
      this.btnFieldOptions[15] = this.btnFieldOption16;
      this.btnFieldOptions[16] = this.btnFieldOption17;
      this.btnFieldOptions[17] = this.btnFieldOption18;
      this.btnFieldOptions[18] = this.btnFieldOption19;
      this.btnFieldOptions[19] = this.btnFieldOption20;
      this.txtLoanFieldIds[0] = this.txtLoanFieldId1;
      this.txtLoanFieldIds[1] = this.txtLoanFieldId2;
      this.txtLoanFieldIds[2] = this.txtLoanFieldId3;
      this.txtLoanFieldIds[3] = this.txtLoanFieldId4;
      this.txtLoanFieldIds[4] = this.txtLoanFieldId5;
      this.txtLoanFieldIds[5] = this.txtLoanFieldId6;
      this.txtLoanFieldIds[6] = this.txtLoanFieldId7;
      this.txtLoanFieldIds[7] = this.txtLoanFieldId8;
      this.txtLoanFieldIds[8] = this.txtLoanFieldId9;
      this.txtLoanFieldIds[9] = this.txtLoanFieldId10;
      this.txtLoanFieldIds[10] = this.txtLoanFieldId11;
      this.txtLoanFieldIds[11] = this.txtLoanFieldId12;
      this.txtLoanFieldIds[12] = this.txtLoanFieldId13;
      this.txtLoanFieldIds[13] = this.txtLoanFieldId14;
      this.txtLoanFieldIds[14] = this.txtLoanFieldId15;
      this.txtLoanFieldIds[15] = this.txtLoanFieldId16;
      this.txtLoanFieldIds[16] = this.txtLoanFieldId17;
      this.txtLoanFieldIds[17] = this.txtLoanFieldId18;
      this.txtLoanFieldIds[18] = this.txtLoanFieldId19;
      this.txtLoanFieldIds[19] = this.txtLoanFieldId20;
      this.txtLoanFieldDescriptions[0] = this.txtLoanFieldDescription1;
      this.txtLoanFieldDescriptions[1] = this.txtLoanFieldDescription2;
      this.txtLoanFieldDescriptions[2] = this.txtLoanFieldDescription3;
      this.txtLoanFieldDescriptions[3] = this.txtLoanFieldDescription4;
      this.txtLoanFieldDescriptions[4] = this.txtLoanFieldDescription5;
      this.txtLoanFieldDescriptions[5] = this.txtLoanFieldDescription6;
      this.txtLoanFieldDescriptions[6] = this.txtLoanFieldDescription7;
      this.txtLoanFieldDescriptions[7] = this.txtLoanFieldDescription8;
      this.txtLoanFieldDescriptions[8] = this.txtLoanFieldDescription9;
      this.txtLoanFieldDescriptions[9] = this.txtLoanFieldDescription10;
      this.txtLoanFieldDescriptions[10] = this.txtLoanFieldDescription11;
      this.txtLoanFieldDescriptions[11] = this.txtLoanFieldDescription12;
      this.txtLoanFieldDescriptions[12] = this.txtLoanFieldDescription13;
      this.txtLoanFieldDescriptions[13] = this.txtLoanFieldDescription14;
      this.txtLoanFieldDescriptions[14] = this.txtLoanFieldDescription15;
      this.txtLoanFieldDescriptions[15] = this.txtLoanFieldDescription16;
      this.txtLoanFieldDescriptions[16] = this.txtLoanFieldDescription17;
      this.txtLoanFieldDescriptions[17] = this.txtLoanFieldDescription18;
      this.txtLoanFieldDescriptions[18] = this.txtLoanFieldDescription19;
      this.txtLoanFieldDescriptions[19] = this.txtLoanFieldDescription20;
      this.picLoanFieldSearches[0] = this.picLoanFieldSearch1;
      this.picLoanFieldSearches[1] = this.picLoanFieldSearch2;
      this.picLoanFieldSearches[2] = this.picLoanFieldSearch3;
      this.picLoanFieldSearches[3] = this.picLoanFieldSearch4;
      this.picLoanFieldSearches[4] = this.picLoanFieldSearch5;
      this.picLoanFieldSearches[5] = this.picLoanFieldSearch6;
      this.picLoanFieldSearches[6] = this.picLoanFieldSearch7;
      this.picLoanFieldSearches[7] = this.picLoanFieldSearch8;
      this.picLoanFieldSearches[8] = this.picLoanFieldSearch9;
      this.picLoanFieldSearches[9] = this.picLoanFieldSearch10;
      this.picLoanFieldSearches[10] = this.picLoanFieldSearch11;
      this.picLoanFieldSearches[11] = this.picLoanFieldSearch12;
      this.picLoanFieldSearches[12] = this.picLoanFieldSearch13;
      this.picLoanFieldSearches[13] = this.picLoanFieldSearch14;
      this.picLoanFieldSearches[14] = this.picLoanFieldSearch15;
      this.picLoanFieldSearches[15] = this.picLoanFieldSearch16;
      this.picLoanFieldSearches[16] = this.picLoanFieldSearch17;
      this.picLoanFieldSearches[17] = this.picLoanFieldSearch18;
      this.picLoanFieldSearches[18] = this.picLoanFieldSearch19;
      this.picLoanFieldSearches[19] = this.picLoanFieldSearch20;
      this.chkTwoWayTransfers[0] = this.chkTwoWayTransfer1;
      this.chkTwoWayTransfers[1] = this.chkTwoWayTransfer2;
      this.chkTwoWayTransfers[2] = this.chkTwoWayTransfer3;
      this.chkTwoWayTransfers[3] = this.chkTwoWayTransfer4;
      this.chkTwoWayTransfers[4] = this.chkTwoWayTransfer5;
      this.chkTwoWayTransfers[5] = this.chkTwoWayTransfer6;
      this.chkTwoWayTransfers[6] = this.chkTwoWayTransfer7;
      this.chkTwoWayTransfers[7] = this.chkTwoWayTransfer8;
      this.chkTwoWayTransfers[8] = this.chkTwoWayTransfer9;
      this.chkTwoWayTransfers[9] = this.chkTwoWayTransfer10;
      this.chkTwoWayTransfers[10] = this.chkTwoWayTransfer11;
      this.chkTwoWayTransfers[11] = this.chkTwoWayTransfer12;
      this.chkTwoWayTransfers[12] = this.chkTwoWayTransfer13;
      this.chkTwoWayTransfers[13] = this.chkTwoWayTransfer14;
      this.chkTwoWayTransfers[14] = this.chkTwoWayTransfer15;
      this.chkTwoWayTransfers[15] = this.chkTwoWayTransfer16;
      this.chkTwoWayTransfers[16] = this.chkTwoWayTransfer17;
      this.chkTwoWayTransfers[17] = this.chkTwoWayTransfer18;
      this.chkTwoWayTransfers[18] = this.chkTwoWayTransfer19;
      this.chkTwoWayTransfers[19] = this.chkTwoWayTransfer20;
      this.chkSelect[0] = this.chkSelect1;
      this.chkSelect[1] = this.chkSelect2;
      this.chkSelect[2] = this.chkSelect3;
      this.chkSelect[3] = this.chkSelect4;
      this.chkSelect[4] = this.chkSelect5;
      this.chkSelect[5] = this.chkSelect6;
      this.chkSelect[6] = this.chkSelect7;
      this.chkSelect[7] = this.chkSelect8;
      this.chkSelect[8] = this.chkSelect9;
      this.chkSelect[9] = this.chkSelect10;
      this.chkSelect[10] = this.chkSelect11;
      this.chkSelect[11] = this.chkSelect12;
      this.chkSelect[12] = this.chkSelect13;
      this.chkSelect[13] = this.chkSelect14;
      this.chkSelect[14] = this.chkSelect15;
      this.chkSelect[15] = this.chkSelect16;
      this.chkSelect[16] = this.chkSelect17;
      this.chkSelect[17] = this.chkSelect18;
      this.chkSelect[18] = this.chkSelect19;
      this.chkSelect[19] = this.chkSelect20;
    }

    private void loadCategoryCustomFields()
    {
      int selectedValue = (int) this.cboCategoryName.SelectedValue;
      if (this.categoryFieldsDefinitions.Contains(CustomFieldsType.BizCategoryCustom, selectedValue))
        return;
      this.categoryFieldsDefinitions.Add(CustomFieldsDefinition.GetCustomFieldsDefinition(this.session.SessionObjects, CustomFieldsType.BizCategoryCustom, selectedValue));
    }

    protected void displayCustomFieldTab(int tabIndex)
    {
      this.initializingControls = true;
      this.disableSetDirtyFlag = true;
      this.txtPageName.Text = this.pageNames[tabIndex];
      if ("Custom Category Fields" == this.txtPageName.Text)
        this.displayCategoryCustomFieldTab(tabIndex);
      else
        this.displayContactCustomFieldTab(tabIndex);
      this.disableSetDirtyFlag = false;
      this.initializingControls = false;
    }

    protected bool showMapping
    {
      set => this.gcCustomFieldDefinitions.Visible = value;
    }

    private void displayCategoryCustomFieldTab(int tabIndex)
    {
      this.showMapping = true;
      this.splitter1.Visible = true;
      this.lblDirections.Text = "Select a category with which to associate the custom fields.";
      this.gcCustomTab.Text = "Select a Category";
      this.txtPageName.Visible = false;
      this.cboCategoryName.Visible = true;
      for (int fieldIndex = 0; fieldIndex < 20; ++fieldIndex)
      {
        int fieldNumber1 = fieldIndex + 1;
        this.lblFieldNumbers[fieldIndex].Text = "Custom Field " + fieldNumber1.ToString();
        this.clearUIControls(fieldIndex, true, true);
        int selectedValue = (int) this.cboCategoryName.SelectedValue;
        if (this.categoryFieldsDefinitions.Contains(CustomFieldsType.BizCategoryCustom, selectedValue))
        {
          CustomFieldsDefinition fieldsDefinition = this.categoryFieldsDefinitions.Find(CustomFieldsType.BizCategoryCustom, selectedValue);
          if (fieldsDefinition.CustomFieldDefinitions.ContainsFieldNumber(fieldNumber1))
          {
            CustomFieldDefinition fieldNumber2 = fieldsDefinition.CustomFieldDefinitions.FindFieldNumber(fieldNumber1);
            this.txtFieldDescriptions[fieldIndex].Text = fieldNumber2.FieldDescription;
            this.formatToolTip(this.txtFieldDescriptions[fieldIndex], fieldNumber2.FieldDescription);
            this.cboFieldTypes[fieldIndex].SelectedItem = (object) FieldFormatEnumUtil.ValueToName(fieldNumber2.FieldFormat);
            if (-1 != this.cboFieldTypes[fieldIndex].SelectedIndex && (FieldFormat.DROPDOWN == fieldNumber2.FieldFormat || FieldFormat.DROPDOWNLIST == fieldNumber2.FieldFormat))
              this.btnFieldOptions[fieldIndex].Visible = true;
            if (string.Empty != fieldNumber2.LoanFieldId)
            {
              if (this.loanFieldDefined(fieldNumber2.LoanFieldId))
              {
                this.txtLoanFieldIds[fieldIndex].Text = fieldNumber2.LoanFieldId;
                this.txtLoanFieldDescriptions[fieldIndex].Text = this.getLoanFieldDescription(fieldNumber2.LoanFieldId);
                this.chkTwoWayTransfers[fieldIndex].Checked = fieldNumber2.TwoWayTransfer;
                if (this.isSettingsSync)
                  this.picLoanFieldSearches[fieldIndex].Enabled = false;
              }
              else if (this.isSettingsSync)
                this.disableUIControls(fieldIndex, false, true);
            }
            else if (this.isSettingsSync)
              this.disableUIControls(fieldIndex, false, true);
          }
          else if (this.isSettingsSync)
            this.disableUIControls(fieldIndex, true, true);
        }
        if (this.isSettingsSync && this.selectedCategoryFieldsForSync.Contains(Convert.ToString(selectedValue) + "_" + Convert.ToString(fieldNumber1)))
          this.chkSelect[fieldIndex].Checked = true;
      }
    }

    private bool loanFieldDefined(string loanFieldId)
    {
      return this.loanFields.Contains(loanFieldId) || this.loanCustomFields.GetField(loanFieldId) != null;
    }

    private string getLoanFieldDescription(string loanFieldId)
    {
      string fieldDescription = string.Empty;
      if (this.loanFields.Contains(loanFieldId))
      {
        fieldDescription = this.loanFields[loanFieldId].Description;
      }
      else
      {
        CustomFieldInfo field = this.loanCustomFields.GetField(loanFieldId);
        if (field != null)
          fieldDescription = field.Description;
      }
      return fieldDescription;
    }

    private void clearUIControls(
      int fieldIndex,
      bool clearCustomFieldControls,
      bool clearLoanFieldControls)
    {
      if (clearCustomFieldControls)
      {
        this.txtFieldDescriptions[fieldIndex].Text = string.Empty;
        this.cboFieldTypes[fieldIndex].SelectedIndex = -1;
        this.btnFieldOptions[fieldIndex].Visible = false;
        this.chkSelect[fieldIndex].Checked = false;
      }
      if (clearLoanFieldControls)
      {
        this.txtLoanFieldIds[fieldIndex].Text = string.Empty;
        this.txtLoanFieldDescriptions[fieldIndex].Text = string.Empty;
        this.chkTwoWayTransfers[fieldIndex].Checked = false;
      }
      this.txtFieldDescriptions[fieldIndex].Enabled = true;
      this.cboFieldTypes[fieldIndex].Enabled = true;
      this.btnFieldOptions[fieldIndex].Enabled = true;
      this.chkSelect[fieldIndex].Enabled = true;
      this.txtLoanFieldIds[fieldIndex].Enabled = true;
      this.txtLoanFieldDescriptions[fieldIndex].Enabled = true;
      this.chkTwoWayTransfers[fieldIndex].Enabled = true;
    }

    private void formatToolTip(TextBox textBox, string text)
    {
      using (Graphics graphics = textBox.CreateGraphics())
      {
        if ((double) textBox.Width >= (double) graphics.MeasureString(text, textBox.Font).Width)
          this.tipCustomField.SetToolTip((Control) textBox, string.Empty);
        else
          this.tipCustomField.SetToolTip((Control) textBox, text);
      }
    }

    protected virtual void displayContactCustomFieldTab(int tabIndex)
    {
      if (this.contactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.showMapping = true;
        this.splitter1.Visible = true;
      }
      else
      {
        this.showMapping = false;
        this.splitter1.Visible = false;
      }
      this.gcCustomTab.Text = "Custom Tab Name";
      switch (this.contactType)
      {
        case EllieMae.EMLite.ContactUI.ContactType.Borrower:
          this.lblDirections.Text = "The name of the tab will display on the Borrower Contacts screen.";
          break;
        case EllieMae.EMLite.ContactUI.ContactType.BizPartner:
          this.lblDirections.Text = "The name of the tab will display on the Business Contacts screen.";
          break;
      }
      this.txtPageName.Visible = true;
      this.cboCategoryName.Visible = false;
      int num = tabIndex * 20;
      for (int fieldIndex = 0; fieldIndex < 20; ++fieldIndex)
      {
        int key = fieldIndex + num + 1;
        this.lblFieldNumbers[fieldIndex].Text = "Custom Field " + key.ToString();
        this.clearUIControls(fieldIndex, true, true);
        if (this._LabelIDToFieldHash.ContainsKey((object) key))
        {
          ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key];
          this.txtFieldDescriptions[fieldIndex].Text = contactCustomFieldInfo.Label;
          this.cboFieldTypes[fieldIndex].SelectedItem = (object) FieldFormatEnumUtil.ValueToName(contactCustomFieldInfo.FieldType);
          if (-1 != this.cboFieldTypes[fieldIndex].SelectedIndex && (FieldFormat.DROPDOWN == contactCustomFieldInfo.FieldType || FieldFormat.DROPDOWNLIST == contactCustomFieldInfo.FieldType))
            this.btnFieldOptions[fieldIndex].Visible = true;
          if (string.Empty != contactCustomFieldInfo.LoanFieldId)
          {
            if (this.loanFieldDefined(contactCustomFieldInfo.LoanFieldId))
            {
              this.txtLoanFieldIds[fieldIndex].Text = contactCustomFieldInfo.LoanFieldId;
              this.txtLoanFieldDescriptions[fieldIndex].Text = this.getLoanFieldDescription(contactCustomFieldInfo.LoanFieldId);
              this.chkTwoWayTransfers[fieldIndex].Checked = contactCustomFieldInfo.TwoWayTransfer;
              if (this.isSettingsSync)
                this.picLoanFieldSearches[fieldIndex].Enabled = false;
            }
            else if (this.isSettingsSync)
              this.disableUIControls(fieldIndex, false, true);
          }
          else if (this.isSettingsSync)
            this.disableUIControls(fieldIndex, false, true);
        }
        else if (this.isSettingsSync)
          this.disableUIControls(fieldIndex, true, true);
        if (this.isSettingsSync && this.selectedFieldsForSync.Contains(Convert.ToString(key)))
          this.chkSelect[fieldIndex].Checked = true;
      }
    }

    private bool fieldDescriptionExists(string description, int fieldId)
    {
      if (string.Empty != description)
      {
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in (IEnumerable) this._LabelIDToFieldHash.Values)
        {
          if (string.Compare(contactCustomFieldInfo.Label.Trim(), description, true) == 0 && contactCustomFieldInfo.LabelID != fieldId)
            return true;
        }
      }
      return false;
    }

    private bool validateCategoryFields()
    {
      foreach (CustomFieldsDefinition fieldsDefinition in (CollectionBase) this.categoryFieldsDefinitions)
      {
        if (fieldsDefinition.IsDirty && !fieldsDefinition.IsValid)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, this.getCategoryName(fieldsDefinition.RecordId) + " category: " + fieldsDefinition.GetBrokenRulesString() + ".\nYour changes are not saved!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    private string getCategoryName(int categoryId)
    {
      foreach (BizCategory category in this.categories)
      {
        if (category.CategoryID == categoryId)
          return category.Name;
      }
      return "Unknown";
    }

    private bool validateContactFields()
    {
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in (IEnumerable) this._LabelIDToFieldHash.Values)
      {
        if (string.Empty == contactCustomFieldInfo.Label.Trim())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Field " + contactCustomFieldInfo.LabelID.ToString() + " has an empty description. Please type in your description for the field or set the field's type to NONE. Warning: your changes are not saved yet!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if ((contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWN || contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWNLIST) && contactCustomFieldInfo.FieldOptions.Length == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Field " + contactCustomFieldInfo.LabelID.ToString() + " requires one or more field options. Warning: your changes are not saved yet!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    private bool checkCategoryDataLoss()
    {
      foreach (CustomFieldsDefinition fieldsDefinition in (CollectionBase) this.categoryFieldsDefinitions)
      {
        if (fieldsDefinition.PossibleDataLoss())
          return true;
      }
      return false;
    }

    private bool checkContactDataLoss(ArrayList invalidFieldIds)
    {
      invalidFieldIds.Clear();
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in (IEnumerable) this._LabelIDToFieldHashInit.Values)
      {
        if (!this._LabelIDToFieldHash.Contains((object) contactCustomFieldInfo.LabelID))
          invalidFieldIds.Add((object) contactCustomFieldInfo.LabelID);
        else if (((ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) contactCustomFieldInfo.LabelID]).FieldType != contactCustomFieldInfo.FieldType)
          invalidFieldIds.Add((object) contactCustomFieldInfo.LabelID);
      }
      return 0 < invalidFieldIds.Count;
    }

    private void saveCategoryFields()
    {
      foreach (BusinessBase fieldsDefinition in (CollectionBase) this.categoryFieldsDefinitions)
        fieldsDefinition.Save();
    }

    protected virtual void saveContactFields(ArrayList invalidFieldIds)
    {
      try
      {
        ContactCustomFieldInfoCollection customFields = new ContactCustomFieldInfoCollection();
        ArrayList arrayList = new ArrayList(this._LabelIDToFieldHash.Values);
        customFields.Items = (ContactCustomFieldInfo[]) arrayList.ToArray(typeof (ContactCustomFieldInfo));
        this.pageNames[this.tabCustomFields.SelectedIndex] = this.txtPageName.Text;
        customFields.Page1Name = this.pageNames[0];
        customFields.Page2Name = this.pageNames[1];
        customFields.Page3Name = this.pageNames[2];
        customFields.Page4Name = this.pageNames[3];
        customFields.Page5Name = this.pageNames[4];
        this.session.ContactManager.UpdateCustomFieldInfo(this.contactType, customFields);
        if (0 >= invalidFieldIds.Count)
          return;
        this.session.ContactManager.DeleteContactCustomFieldValues(this.contactType, (int[]) invalidFieldIds.ToArray(typeof (int)));
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Custom Fields cannot be saved! You might not have the required file access rights.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void disableUIControls(
      int fieldIndex,
      bool disableCustomFieldControls,
      bool disableLoanFieldControls)
    {
      if (disableCustomFieldControls)
      {
        this.txtFieldDescriptions[fieldIndex].Enabled = false;
        this.cboFieldTypes[fieldIndex].Enabled = false;
        this.btnFieldOptions[fieldIndex].Enabled = false;
        this.chkSelect[fieldIndex].Enabled = false;
      }
      if (!disableLoanFieldControls)
        return;
      this.txtLoanFieldIds[fieldIndex].Enabled = false;
      this.txtLoanFieldDescriptions[fieldIndex].Enabled = false;
      this.chkTwoWayTransfers[fieldIndex].Enabled = false;
      this.picLoanFieldSearches[fieldIndex].Enabled = false;
    }

    private void ContactCustomFieldsForm_Load(object sender, EventArgs e) => this.Reset();

    private void tabCustomFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.selectedTabPage != null)
        this.selectedTabPage.Controls.Remove((Control) this.pnlData);
      this.selectedTabPage = this.tabCustomFields.SelectedTab;
      this.selectedTabPage.Controls.Add((Control) this.pnlData);
      this.displayCustomFieldTab(this.tabCustomFields.SelectedIndex);
      this.txtFieldDescription1.Focus();
    }

    private void cboCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.loadCategoryCustomFields();
      this.displayCustomFieldTab(this.tabCustomFields.SelectedIndex);
    }

    private void txtPageName_Leave(object sender, EventArgs e)
    {
      this.pageNames[this.tabCustomFields.SelectedIndex] = this.txtPageName.Text.Trim();
    }

    private void txtFieldDescription_Leave(object sender, EventArgs e)
    {
      TextBox txtFieldDescription = (TextBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryFieldDescriptionLeave(txtFieldDescription);
      else
        this.contactFieldDescriptionLeave(txtFieldDescription);
    }

    private void txtFieldDescription_TextChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void categoryFieldDescriptionLeave(TextBox txtFieldDescription)
    {
      CustomFieldsDefinition categoryCustomFields = this.getCategoryCustomFields();
      if (categoryCustomFields == null)
        return;
      int int32 = Convert.ToInt32(txtFieldDescription.Tag.ToString());
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(int32);
      string fieldDescription = txtFieldDescription.Text.Trim();
      if (string.Empty != fieldDescription && categoryCustomFields.CustomFieldDefinitions.ContainsDuplicate(categoryCustomField, fieldDescription))
      {
        txtFieldDescription.Text = categoryCustomField == null || categoryCustomField.FieldDescription == fieldDescription ? string.Empty : categoryCustomField.FieldDescription;
        int num = (int) Utils.Dialog((IWin32Window) this, "The field description \"" + fieldDescription + "\" already exists.\nPlease enter a different description for this field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        txtFieldDescription.Focus();
      }
      else
      {
        int fieldIndex = int32 - 1;
        if (categoryCustomField != null)
        {
          if (string.Empty == fieldDescription)
          {
            if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Setting the field description to blank will delete this custom field entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
            {
              txtFieldDescription.Text = categoryCustomField.FieldDescription;
            }
            else
            {
              this.clearUIControls(fieldIndex, true, true);
              categoryCustomFields.CustomFieldDefinitions.Remove(categoryCustomField);
            }
          }
          else
          {
            categoryCustomField.FieldDescription = fieldDescription;
            if (-1 == this.cboFieldTypes[fieldIndex].SelectedIndex)
              return;
            categoryCustomField.FieldFormat = FieldFormatEnumUtil.NameToValue((string) this.cboFieldTypes[fieldIndex].SelectedItem);
          }
        }
        else
        {
          if (!(string.Empty != fieldDescription))
            return;
          CustomFieldDefinition customFieldDefinition = CustomFieldDefinition.NewCustomFieldDefinition((int) this.cboCategoryName.SelectedValue, int32);
          customFieldDefinition.FieldDescription = fieldDescription;
          categoryCustomFields.CustomFieldDefinitions.Add(customFieldDefinition);
        }
      }
    }

    private CustomFieldDefinition getCategoryCustomField(int fieldNumber)
    {
      CustomFieldDefinition categoryCustomField = (CustomFieldDefinition) null;
      CustomFieldsDefinition categoryCustomFields = this.getCategoryCustomFields();
      if (categoryCustomFields != null && categoryCustomFields.CustomFieldDefinitions.ContainsFieldNumber(fieldNumber))
        categoryCustomField = categoryCustomFields.CustomFieldDefinitions.FindFieldNumber(fieldNumber);
      return categoryCustomField;
    }

    private CustomFieldsDefinition getCategoryCustomFields()
    {
      CustomFieldsDefinition categoryCustomFields = (CustomFieldsDefinition) null;
      int selectedValue = (int) this.cboCategoryName.SelectedValue;
      if (this.categoryFieldsDefinitions.Contains(CustomFieldsType.BizCategoryCustom, selectedValue))
        categoryCustomFields = this.categoryFieldsDefinitions.Find(CustomFieldsType.BizCategoryCustom, selectedValue);
      return categoryCustomFields;
    }

    private bool isCategoryCustomFieldValid(CustomFieldDefinition categoryCustomField)
    {
      return categoryCustomField != null && !(string.Empty == categoryCustomField.FieldDescription) && categoryCustomField.FieldFormat != FieldFormat.NONE && (FieldFormat.DROPDOWN != categoryCustomField.FieldFormat || categoryCustomField.CustomFieldOptions != null && categoryCustomField.CustomFieldOptions.Count != 0) && (FieldFormat.DROPDOWNLIST != categoryCustomField.FieldFormat || categoryCustomField.CustomFieldOptions != null && categoryCustomField.CustomFieldOptions.Count != 0);
    }

    private void contactFieldDescriptionLeave(TextBox txtFieldDescription)
    {
      int num1 = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(txtFieldDescription.Tag.ToString());
      if (this.fieldDescriptionExists(txtFieldDescription.Text.Trim(), num1))
      {
        string str = txtFieldDescription.Text.Trim();
        if (this._LabelIDToFieldHash.ContainsKey((object) num1))
        {
          ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) num1];
          txtFieldDescription.Text = contactCustomFieldInfo.Label;
        }
        else
          txtFieldDescription.Text = string.Empty;
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The field description \"" + str + "\" already exists.\nPlease enter a different description for this field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        txtFieldDescription.Focus();
      }
      else if (this._LabelIDToFieldHash.Contains((object) num1))
      {
        ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) num1];
        if (string.Empty == txtFieldDescription.Text.Trim())
        {
          if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Setting the field description to blank will delete this custom field entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          {
            txtFieldDescription.Text = contactCustomFieldInfo.Label;
          }
          else
          {
            this.clearUIControls(Convert.ToInt32(txtFieldDescription.Tag.ToString()) - 1, true, true);
            this._LabelIDToFieldHash.Remove((object) num1);
          }
        }
        else
          contactCustomFieldInfo.Label = txtFieldDescription.Text;
      }
      else
      {
        if (!(txtFieldDescription.Text.Trim() != string.Empty))
          return;
        ContactCustomFieldInfo contactCustomFieldInfo = new ContactCustomFieldInfo(num1, string.Empty, txtFieldDescription.Text, FieldFormat.NONE, string.Empty, false, new string[0]);
        this._LabelIDToFieldHash.Add((object) num1, (object) contactCustomFieldInfo);
      }
    }

    private ContactCustomFieldInfo getContactCustomField(int fieldNumber)
    {
      ContactCustomFieldInfo contactCustomField = (ContactCustomFieldInfo) null;
      int key = this.tabCustomFields.SelectedIndex * 20 + fieldNumber;
      if (this._LabelIDToFieldHash.Contains((object) key))
        contactCustomField = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key];
      return contactCustomField;
    }

    private bool isContactCustomFieldValid(ContactCustomFieldInfo contactCustomField)
    {
      return !((ContactCustomFieldInfo) null == contactCustomField) && !(string.Empty == contactCustomField.Label) && contactCustomField.FieldType != FieldFormat.NONE && (FieldFormat.DROPDOWN != contactCustomField.FieldType || contactCustomField.FieldOptions != null && contactCustomField.FieldOptions.Length != 0) && (FieldFormat.DROPDOWNLIST != contactCustomField.FieldType || contactCustomField.FieldOptions != null && contactCustomField.FieldOptions.Length != 0);
    }

    private void cboFieldType_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox cboFieldType = (ComboBox) sender;
      if (cboFieldType.SelectedItem != null)
      {
        if (5 == this.tabCustomFields.SelectedIndex)
          this.categoryFieldTypeChanged(cboFieldType);
        else
          this.contactFieldTypeChanged(cboFieldType);
        FieldFormat fieldFormat = cboFieldType.SelectedItem == null ? FieldFormat.NONE : FieldFormatEnumUtil.NameToValue((string) cboFieldType.SelectedItem);
        this.btnFieldOptions[Convert.ToInt32(cboFieldType.Tag.ToString()) - 1].Visible = fieldFormat == FieldFormat.DROPDOWN || fieldFormat == FieldFormat.DROPDOWNLIST;
      }
      this.setDirtyFlag(true);
    }

    private void categoryFieldTypeChanged(ComboBox cboFieldType)
    {
      CustomFieldsDefinition categoryCustomFields = this.getCategoryCustomFields();
      if (categoryCustomFields == null)
        return;
      int int32 = Convert.ToInt32(cboFieldType.Tag.ToString());
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(int32);
      FieldFormat fieldFormat = FieldFormatEnumUtil.NameToValue((string) cboFieldType.SelectedItem);
      int fieldIndex = int32 - 1;
      if (categoryCustomField != null)
      {
        if (categoryCustomField.FieldFormat == fieldFormat)
          return;
        if (fieldFormat == FieldFormat.NONE)
        {
          if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Setting the field format to NONE will delete this custom field entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          {
            cboFieldType.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(categoryCustomField.FieldFormat);
            return;
          }
          this.clearUIControls(fieldIndex, true, true);
          categoryCustomFields.CustomFieldDefinitions.Remove(categoryCustomField);
        }
        else
        {
          if (string.Empty != categoryCustomField.LoanFieldId && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Changing the field format will remove current Loan Field mapping entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          {
            cboFieldType.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(categoryCustomField.FieldFormat);
            return;
          }
          categoryCustomField.FieldFormat = fieldFormat;
          categoryCustomField.LoanFieldId = string.Empty;
          categoryCustomField.TwoWayTransfer = false;
          this.clearUIControls(fieldIndex, false, true);
        }
        this.updateCustomFieldMappingCollection(CustomFieldsType.BizCategoryCustom, (int) this.cboCategoryName.SelectedValue, int32, categoryCustomField.FieldId, categoryCustomField.FieldFormat, string.Empty, false);
      }
      else
      {
        if (fieldFormat == FieldFormat.NONE)
          return;
        CustomFieldDefinition customFieldDefinition = CustomFieldDefinition.NewCustomFieldDefinition((int) this.cboCategoryName.SelectedValue, int32);
        customFieldDefinition.FieldFormat = fieldFormat;
        customFieldDefinition.FieldDescription = this.txtFieldDescriptions[fieldIndex].Text;
        categoryCustomFields.CustomFieldDefinitions.Add(customFieldDefinition);
      }
    }

    private void contactFieldTypeChanged(ComboBox cboFieldType)
    {
      int num = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(cboFieldType.Tag.ToString());
      FieldFormat fieldType = FieldFormatEnumUtil.NameToValue((string) cboFieldType.SelectedItem);
      if (this._LabelIDToFieldHash.Contains((object) num))
      {
        ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) num];
        int fieldIndex = Convert.ToInt32(cboFieldType.Tag.ToString()) - 1;
        if (contactCustomFieldInfo.FieldType == fieldType)
          return;
        if (fieldType == FieldFormat.NONE)
        {
          if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Setting the field format to NONE will delete this custom field entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          {
            cboFieldType.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(contactCustomFieldInfo.FieldType);
            return;
          }
          this.clearUIControls(fieldIndex, true, true);
          this._LabelIDToFieldHash.Remove((object) num);
        }
        else
        {
          if (string.Empty != contactCustomFieldInfo.LoanFieldId && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Changing the field format will remove current Loan Field mapping entry.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          {
            cboFieldType.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(contactCustomFieldInfo.FieldType);
            return;
          }
          contactCustomFieldInfo.FieldType = fieldType;
          contactCustomFieldInfo.LoanFieldId = string.Empty;
          contactCustomFieldInfo.TwoWayTransfer = false;
          this.clearUIControls(fieldIndex, false, true);
        }
        this.updateCustomFieldMappingCollection(this.contactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? CustomFieldsType.Borrower : CustomFieldsType.BizPartner, 0, num, 0, contactCustomFieldInfo.FieldType, string.Empty, false);
      }
      else
      {
        if (fieldType == FieldFormat.NONE)
          return;
        ContactCustomFieldInfo contactCustomFieldInfo = new ContactCustomFieldInfo(num, string.Empty, string.Empty, fieldType, string.Empty, false, new string[0]);
        this._LabelIDToFieldHash.Add((object) num, (object) contactCustomFieldInfo);
      }
    }

    private void btnFieldOption_Click(object sender, EventArgs e)
    {
      StandardIconButton btnFieldOption = (StandardIconButton) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryFieldOptionClick(btnFieldOption);
      else
        this.contactFieldOptionClick(btnFieldOption);
    }

    private void categoryFieldOptionClick(StandardIconButton btnFieldOption)
    {
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(Convert.ToInt32(btnFieldOption.Tag.ToString()));
      if (categoryCustomField == null)
        return;
      ContactCustomFieldOptionsForm fieldOptionsForm = new ContactCustomFieldOptionsForm(categoryCustomField.CustomFieldOptions.GetOptionValues());
      if (DialogResult.OK != fieldOptionsForm.ShowDialog())
        return;
      categoryCustomField.CustomFieldOptions.SetOptionValues(fieldOptionsForm.FieldOptions);
      if (!fieldOptionsForm.IsDirty)
        return;
      this.setDirtyFlag(true);
    }

    private void contactFieldOptionClick(StandardIconButton btnFieldOption)
    {
      int key = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(btnFieldOption.Tag.ToString());
      if (!this._LabelIDToFieldHash.Contains((object) key))
        return;
      ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key];
      ContactCustomFieldOptionsForm fieldOptionsForm = new ContactCustomFieldOptionsForm(contactCustomFieldInfo.FieldOptions);
      if (DialogResult.OK == fieldOptionsForm.ShowDialog())
      {
        contactCustomFieldInfo.FieldOptions = fieldOptionsForm.FieldOptions;
        if (fieldOptionsForm.IsDirty)
          this.setDirtyFlag(true);
      }
      Convert.ToInt32(btnFieldOption.Tag.ToString());
    }

    private void txtLoanFieldId_Enter(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        return;
      TextBox txtLoanFieldId = (TextBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryLoanFieldIdEnter(txtLoanFieldId);
      else
        this.contactLoanFieldIdEnter(txtLoanFieldId);
    }

    private void categoryLoanFieldIdEnter(TextBox txtLoanFieldId)
    {
      int int32 = Convert.ToInt32(txtLoanFieldId.Tag.ToString());
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(int32);
      if (categoryCustomField != null && categoryCustomField.FieldFormat != FieldFormat.NONE)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.cboFieldTypes[int32 - 1].Focus();
    }

    private void contactLoanFieldIdEnter(TextBox txtLoanFieldId)
    {
      int key = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(txtLoanFieldId.Tag.ToString());
      if (this._LabelIDToFieldHash.Contains((object) key) && ((ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key]).FieldType != FieldFormat.NONE)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.cboFieldTypes[Convert.ToInt32(txtLoanFieldId.Tag.ToString()) - 1].Focus();
    }

    private void txtLoanFieldId_Leave(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        return;
      TextBox txtLoanFieldId = (TextBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryLoanFieldIdLeave(txtLoanFieldId);
      else
        this.contactLoanFieldIdLeave(txtLoanFieldId);
    }

    private void categoryLoanFieldIdLeave(TextBox txtLoanFieldId)
    {
      int int32 = Convert.ToInt32(txtLoanFieldId.Tag.ToString());
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(int32);
      if (categoryCustomField == null)
        return;
      string loanFieldId = txtLoanFieldId.Text.Trim();
      this.categoryLoanFieldIdChanged(int32, loanFieldId, categoryCustomField);
    }

    private void categoryLoanFieldIdChanged(
      int fieldNumber,
      string loanFieldId,
      CustomFieldDefinition categoryCustomField)
    {
      int fieldIndex = fieldNumber - 1;
      if (!(categoryCustomField.LoanFieldId != loanFieldId))
        return;
      if (string.Empty != loanFieldId)
      {
        this.loanFieldMappingDlg.LoanFieldId = loanFieldId;
        this.loanFieldMappingDlg.CategoryCustomField = categoryCustomField;
        if (!this.loanFieldMappingDlg.ValidateLoanFieldSelection((IWin32Window) this))
        {
          if (DialogResult.Cancel == this.loanFieldMappingDlg.ShowDialog())
          {
            this.txtLoanFieldIds[fieldIndex].Text = categoryCustomField.LoanFieldId;
            this.txtLoanFieldIds[fieldIndex].Focus();
            return;
          }
          loanFieldId = this.loanFieldMappingDlg.LoanFieldId;
        }
      }
      if (string.Empty != loanFieldId)
      {
        categoryCustomField.LoanFieldId = loanFieldId;
        categoryCustomField.TwoWayTransfer = this.chkTwoWayTransfers[fieldIndex].Checked;
        this.txtLoanFieldIds[fieldIndex].Text = loanFieldId;
        this.txtLoanFieldDescriptions[fieldIndex].Text = this.getLoanFieldDescription(loanFieldId);
      }
      else
      {
        categoryCustomField.LoanFieldId = string.Empty;
        categoryCustomField.TwoWayTransfer = false;
        this.clearUIControls(fieldIndex, false, true);
      }
      this.updateCustomFieldMappingCollection(CustomFieldsType.BizCategoryCustom, (int) this.cboCategoryName.SelectedValue, fieldNumber, categoryCustomField.FieldId, categoryCustomField.FieldFormat, categoryCustomField.LoanFieldId, categoryCustomField.TwoWayTransfer);
    }

    protected virtual void updateCustomFieldMappingCollection(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber,
      int recordId,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer)
    {
      CustomFieldMapping customFieldMapping = this.customFieldMappingCollection.Find(customFieldsType, categoryId, fieldNumber);
      if (string.Empty != loanFieldId && customFieldMapping == null)
        this.customFieldMappingCollection.Add(CustomFieldMapping.NewCustomFieldMapping(customFieldsType, categoryId, fieldNumber, recordId, fieldFormat, loanFieldId, twoWayTransfer));
      else if (string.Empty != loanFieldId && customFieldMapping != null)
      {
        customFieldMapping.LoanFieldId = loanFieldId;
        customFieldMapping.TwoWayTransfer = twoWayTransfer;
      }
      else
      {
        if (!(string.Empty == loanFieldId) || customFieldMapping == null)
          return;
        this.customFieldMappingCollection.Remove(customFieldMapping);
      }
    }

    private void contactLoanFieldIdLeave(TextBox txtLoanFieldId)
    {
      int key = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(txtLoanFieldId.Tag.ToString());
      if (!this._LabelIDToFieldHash.Contains((object) key))
        return;
      this.contactLoanFieldIdChanged(Convert.ToInt32(txtLoanFieldId.Tag.ToString()), txtLoanFieldId.Text.Trim(), (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key]);
    }

    private void contactLoanFieldIdChanged(
      int fieldNumber,
      string loanFieldId,
      ContactCustomFieldInfo contactCustomField)
    {
      int fieldIndex = fieldNumber - 1;
      if (!(contactCustomField.LoanFieldId != loanFieldId))
        return;
      if (string.Empty != loanFieldId)
      {
        this.loanFieldMappingDlg.LoanFieldId = loanFieldId;
        this.loanFieldMappingDlg.ContactCustomField = contactCustomField;
        if (!this.loanFieldMappingDlg.ValidateLoanFieldSelection((IWin32Window) this))
        {
          if (DialogResult.Cancel == this.loanFieldMappingDlg.ShowDialog())
          {
            this.txtLoanFieldIds[fieldIndex].Text = contactCustomField.LoanFieldId;
            this.txtLoanFieldIds[fieldIndex].Focus();
            return;
          }
          loanFieldId = this.loanFieldMappingDlg.LoanFieldId;
        }
      }
      if (string.Empty != loanFieldId)
      {
        contactCustomField.LoanFieldId = loanFieldId;
        contactCustomField.TwoWayTransfer = this.chkTwoWayTransfers[fieldIndex].Checked;
        this.txtLoanFieldIds[fieldIndex].Text = loanFieldId;
        this.txtLoanFieldDescriptions[fieldIndex].Text = this.getLoanFieldDescription(loanFieldId);
      }
      else
      {
        contactCustomField.LoanFieldId = string.Empty;
        contactCustomField.TwoWayTransfer = false;
        this.clearUIControls(fieldIndex, false, true);
      }
      int fieldNumber1 = fieldNumber + this.tabCustomFields.SelectedIndex * 20;
      this.updateCustomFieldMappingCollection(this.contactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? CustomFieldsType.Borrower : CustomFieldsType.BizPartner, 0, fieldNumber1, 0, contactCustomField.FieldType, contactCustomField.LoanFieldId, contactCustomField.TwoWayTransfer);
    }

    private void picLoanFieldSearch_Click(object sender, EventArgs e)
    {
      PictureBox picLoanFieldSearch = (PictureBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryLoanSearchClicked(picLoanFieldSearch);
      else
        this.contactLoanSearchClicked(picLoanFieldSearch);
    }

    private void categoryLoanSearchClicked(PictureBox picLoanFieldSearch)
    {
      int int32 = Convert.ToInt32(picLoanFieldSearch.Tag.ToString());
      int index = int32 - 1;
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(int32);
      if (categoryCustomField != null && categoryCustomField.FieldFormat != FieldFormat.NONE)
      {
        this.loanFieldMappingDlg.LoanFieldId = this.txtLoanFieldIds[index].Text;
        this.loanFieldMappingDlg.CategoryCustomField = categoryCustomField;
        if (DialogResult.OK != this.loanFieldMappingDlg.ShowDialog())
          return;
        this.categoryLoanFieldIdChanged(int32, this.loanFieldMappingDlg.LoanFieldId, categoryCustomField);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboFieldTypes[index].Focus();
      }
    }

    private void contactLoanSearchClicked(PictureBox picLoanFieldSearch)
    {
      int key = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(picLoanFieldSearch.Tag.ToString());
      int int32 = Convert.ToInt32(picLoanFieldSearch.Tag.ToString());
      int index = int32 - 1;
      ContactCustomFieldInfo contactCustomField = (ContactCustomFieldInfo) null;
      if (this._LabelIDToFieldHash.Contains((object) key))
        contactCustomField = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key];
      if ((ContactCustomFieldInfo) null != contactCustomField && contactCustomField.FieldType != FieldFormat.NONE)
      {
        this.loanFieldMappingDlg.LoanFieldId = this.txtLoanFieldIds[index].Text;
        this.loanFieldMappingDlg.ContactCustomField = contactCustomField;
        if (DialogResult.OK != this.loanFieldMappingDlg.ShowDialog())
          return;
        this.contactLoanFieldIdChanged(int32, this.loanFieldMappingDlg.LoanFieldId, contactCustomField);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboFieldTypes[index].Focus();
      }
    }

    private void chkTwoWayTransfer_Enter(object sender, EventArgs e)
    {
      if (this.initializingControls)
        return;
      CheckBox chkTwoWayTransfer = (CheckBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryTwoWayTransferEnter(chkTwoWayTransfer);
      else
        this.contactTwoWayTransferEnter(chkTwoWayTransfer);
    }

    private void categoryTwoWayTransferEnter(CheckBox chkTwoWayTransfer)
    {
      int int32 = Convert.ToInt32(chkTwoWayTransfer.Tag.ToString());
      if (this.getCategoryCustomField(int32) != null)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.cboFieldTypes[int32 - 1].Focus();
    }

    private void contactTwoWayTransferEnter(CheckBox chkTwoWayTransfer)
    {
      if (this._LabelIDToFieldHash.Contains((object) (this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(chkTwoWayTransfer.Tag.ToString()))))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please define the Field Type before mapping to a loan field.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.cboFieldTypes[Convert.ToInt32(chkTwoWayTransfer.Tag.ToString()) - 1].Focus();
    }

    private void chkTwoWayTransfer_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initializingControls)
        return;
      CheckBox chkTwoWayTransfer = (CheckBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categoryTwoWayTransferCheckedChanged(chkTwoWayTransfer);
      else
        this.contactTwoWayTransferCheckedChanged(chkTwoWayTransfer);
      this.setDirtyFlag(true);
    }

    private void categoryTwoWayTransferCheckedChanged(CheckBox chkTwoWayTransfer)
    {
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(Convert.ToInt32(chkTwoWayTransfer.Tag.ToString()));
      if (categoryCustomField == null)
        return;
      categoryCustomField.TwoWayTransfer = chkTwoWayTransfer.Checked;
    }

    private void contactTwoWayTransferCheckedChanged(CheckBox chkTwoWayTransfer)
    {
      int key = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(chkTwoWayTransfer.Tag.ToString());
      if (!this._LabelIDToFieldHash.Contains((object) key))
        return;
      ((ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) key]).TwoWayTransfer = chkTwoWayTransfer.Checked;
    }

    private void categorySelectCheckedChanged(CheckBox chkSelect)
    {
      CustomFieldDefinition categoryCustomField = this.getCategoryCustomField(Convert.ToInt32(chkSelect.Tag.ToString()));
      if (categoryCustomField == null)
        return;
      if (chkSelect.Checked)
        this.selectedCategoryFieldsForSync.Add(Convert.ToString(categoryCustomField.CategoryId) + "_" + Convert.ToString(categoryCustomField.FieldNumber));
      else
        this.selectedCategoryFieldsForSync.Remove(Convert.ToString(categoryCustomField.CategoryId) + "_" + Convert.ToString(categoryCustomField.FieldNumber));
    }

    private void contactSelectCheckedChanged(CheckBox chkSelect)
    {
      int num = this.tabCustomFields.SelectedIndex * 20 + Convert.ToInt32(chkSelect.Tag.ToString());
      if (chkSelect.Checked)
      {
        if (this.selectedFieldsForSync.Contains(num.ToString()))
          return;
        this.selectedFieldsForSync.Add(num.ToString());
      }
      else
        this.selectedFieldsForSync.Remove(num.ToString());
    }

    private void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initializingControls)
        return;
      CheckBox chkSelect = (CheckBox) sender;
      if (5 == this.tabCustomFields.SelectedIndex)
        this.categorySelectCheckedChanged(chkSelect);
      else
        this.contactSelectCheckedChanged(chkSelect);
      this.setDirtyFlag(true);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.tabCustomFields = new TabControl();
      this.tabPage1 = new TabPage();
      this.pnlData = new Panel();
      this.gcCustomTab = new GroupContainer();
      this.borderPanel2 = new BorderPanel();
      this.pnlCustomFieldsInternal = new Panel();
      this.chkSelect20 = new CheckBox();
      this.chkSelect15 = new CheckBox();
      this.chkSelect6 = new CheckBox();
      this.chkSelect14 = new CheckBox();
      this.chkSelect5 = new CheckBox();
      this.chkSelect19 = new CheckBox();
      this.chkSelect4 = new CheckBox();
      this.chkSelect7 = new CheckBox();
      this.chkSelect13 = new CheckBox();
      this.chkSelect3 = new CheckBox();
      this.chkSelect2 = new CheckBox();
      this.chkSelect16 = new CheckBox();
      this.chkSelect8 = new CheckBox();
      this.chkSelect12 = new CheckBox();
      this.chkSelect1 = new CheckBox();
      this.chkSelect18 = new CheckBox();
      this.lblSelect = new Label();
      this.chkSelect9 = new CheckBox();
      this.chkSelect11 = new CheckBox();
      this.chkSelect10 = new CheckBox();
      this.chkSelect17 = new CheckBox();
      this.btnFieldOption20 = new StandardIconButton();
      this.txtFieldDescription1 = new TextBox();
      this.cboFieldType13 = new ComboBox();
      this.txtFieldDescription13 = new TextBox();
      this.btnFieldOption19 = new StandardIconButton();
      this.lblFieldNumber13 = new Label();
      this.lblFieldNumber12 = new Label();
      this.txtFieldDescription14 = new TextBox();
      this.btnFieldOption18 = new StandardIconButton();
      this.cboFieldType12 = new ComboBox();
      this.cboFieldType14 = new ComboBox();
      this.txtFieldDescription12 = new TextBox();
      this.btnFieldOption17 = new StandardIconButton();
      this.lblFieldNumber14 = new Label();
      this.lblFieldNumber11 = new Label();
      this.txtFieldDescription15 = new TextBox();
      this.cboFieldType11 = new ComboBox();
      this.btnFieldOption16 = new StandardIconButton();
      this.cboFieldType15 = new ComboBox();
      this.txtFieldDescription11 = new TextBox();
      this.lblFieldDescription = new Label();
      this.lblFieldNumber15 = new Label();
      this.lblFieldNumber10 = new Label();
      this.btnFieldOption15 = new StandardIconButton();
      this.txtFieldDescription16 = new TextBox();
      this.cboFieldType10 = new ComboBox();
      this.lblFieldType = new Label();
      this.cboFieldType16 = new ComboBox();
      this.txtFieldDescription10 = new TextBox();
      this.btnFieldOption14 = new StandardIconButton();
      this.lblFieldNumber16 = new Label();
      this.lblFieldNumber9 = new Label();
      this.cboFieldType1 = new ComboBox();
      this.txtFieldDescription17 = new TextBox();
      this.cboFieldType9 = new ComboBox();
      this.btnFieldOption13 = new StandardIconButton();
      this.cboFieldType17 = new ComboBox();
      this.txtFieldDescription9 = new TextBox();
      this.lblFieldNumber1 = new Label();
      this.lblFieldNumber17 = new Label();
      this.lblFieldNumber8 = new Label();
      this.btnFieldOption12 = new StandardIconButton();
      this.txtFieldDescription18 = new TextBox();
      this.cboFieldType8 = new ComboBox();
      this.txtFieldDescription2 = new TextBox();
      this.cboFieldType18 = new ComboBox();
      this.txtFieldDescription8 = new TextBox();
      this.btnFieldOption11 = new StandardIconButton();
      this.lblFieldNumber18 = new Label();
      this.lblFieldNumber7 = new Label();
      this.cboFieldType2 = new ComboBox();
      this.txtFieldDescription19 = new TextBox();
      this.cboFieldType7 = new ComboBox();
      this.btnFieldOption10 = new StandardIconButton();
      this.cboFieldType19 = new ComboBox();
      this.txtFieldDescription7 = new TextBox();
      this.lblFieldNumber2 = new Label();
      this.lblFieldNumber19 = new Label();
      this.lblFieldNumber6 = new Label();
      this.btnFieldOption9 = new StandardIconButton();
      this.txtFieldDescription20 = new TextBox();
      this.cboFieldType6 = new ComboBox();
      this.txtFieldDescription3 = new TextBox();
      this.cboFieldType20 = new ComboBox();
      this.txtFieldDescription6 = new TextBox();
      this.btnFieldOption8 = new StandardIconButton();
      this.lblFieldNumber20 = new Label();
      this.lblFieldNumber5 = new Label();
      this.cboFieldType3 = new ComboBox();
      this.btnFieldOption1 = new StandardIconButton();
      this.btnFieldOption7 = new StandardIconButton();
      this.cboFieldType5 = new ComboBox();
      this.btnFieldOption2 = new StandardIconButton();
      this.lblFieldNumber3 = new Label();
      this.txtFieldDescription5 = new TextBox();
      this.btnFieldOption3 = new StandardIconButton();
      this.btnFieldOption6 = new StandardIconButton();
      this.lblFieldNumber4 = new Label();
      this.btnFieldOption4 = new StandardIconButton();
      this.txtFieldDescription4 = new TextBox();
      this.cboFieldType4 = new ComboBox();
      this.btnFieldOption5 = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.lblDirections = new Label();
      this.txtPageName = new TextBox();
      this.cboCategoryName = new ComboBox();
      this.gcCustomFieldDefinitions = new GroupContainer();
      this.borderPanel3 = new BorderPanel();
      this.pnlExRightMapping = new PanelEx();
      this.picLoanFieldSearch20 = new StandardIconButton();
      this.picLoanFieldSearch19 = new StandardIconButton();
      this.picLoanFieldSearch18 = new StandardIconButton();
      this.picLoanFieldSearch17 = new StandardIconButton();
      this.picLoanFieldSearch16 = new StandardIconButton();
      this.picLoanFieldSearch15 = new StandardIconButton();
      this.picLoanFieldSearch14 = new StandardIconButton();
      this.picLoanFieldSearch13 = new StandardIconButton();
      this.picLoanFieldSearch12 = new StandardIconButton();
      this.picLoanFieldSearch11 = new StandardIconButton();
      this.picLoanFieldSearch10 = new StandardIconButton();
      this.picLoanFieldSearch9 = new StandardIconButton();
      this.picLoanFieldSearch8 = new StandardIconButton();
      this.picLoanFieldSearch7 = new StandardIconButton();
      this.picLoanFieldSearch6 = new StandardIconButton();
      this.picLoanFieldSearch5 = new StandardIconButton();
      this.picLoanFieldSearch4 = new StandardIconButton();
      this.picLoanFieldSearch3 = new StandardIconButton();
      this.picLoanFieldSearch2 = new StandardIconButton();
      this.picLoanFieldSearch1 = new StandardIconButton();
      this.chkTwoWayTransfer20 = new CheckBox();
      this.lblLoanFieldId = new Label();
      this.txtLoanFieldDescription10 = new TextBox();
      this.txtLoanFieldId4 = new TextBox();
      this.txtLoanFieldDescription4 = new TextBox();
      this.txtLoanFieldId13 = new TextBox();
      this.txtLoanFieldDescription9 = new TextBox();
      this.txtLoanFieldDescription19 = new TextBox();
      this.chkTwoWayTransfer15 = new CheckBox();
      this.chkTwoWayTransfer6 = new CheckBox();
      this.chkTwoWayTransfer14 = new CheckBox();
      this.chkTwoWayTransfer5 = new CheckBox();
      this.chkTwoWayTransfer19 = new CheckBox();
      this.txtLoanFieldDescription11 = new TextBox();
      this.txtLoanFieldId14 = new TextBox();
      this.txtLoanFieldDescription8 = new TextBox();
      this.txtLoanFieldId12 = new TextBox();
      this.txtLoanFieldId5 = new TextBox();
      this.txtLoanFieldDescription20 = new TextBox();
      this.chkTwoWayTransfer4 = new CheckBox();
      this.txtLoanFieldId15 = new TextBox();
      this.txtLoanFieldDescription7 = new TextBox();
      this.txtLoanFieldDescription18 = new TextBox();
      this.txtLoanFieldId3 = new TextBox();
      this.chkTwoWayTransfer7 = new CheckBox();
      this.chkTwoWayTransfer13 = new CheckBox();
      this.chkTwoWayTransfer3 = new CheckBox();
      this.txtLoanFieldId1 = new TextBox();
      this.txtLoanFieldDescription12 = new TextBox();
      this.txtLoanFieldId16 = new TextBox();
      this.txtLoanFieldDescription6 = new TextBox();
      this.txtLoanFieldId11 = new TextBox();
      this.txtLoanFieldId6 = new TextBox();
      this.chkTwoWayTransfer2 = new CheckBox();
      this.txtLoanFieldId17 = new TextBox();
      this.txtLoanFieldDescription5 = new TextBox();
      this.txtLoanFieldDescription17 = new TextBox();
      this.chkTwoWayTransfer16 = new CheckBox();
      this.chkTwoWayTransfer8 = new CheckBox();
      this.chkTwoWayTransfer12 = new CheckBox();
      this.chkTwoWayTransfer1 = new CheckBox();
      this.chkTwoWayTransfer18 = new CheckBox();
      this.txtLoanFieldDescription13 = new TextBox();
      this.txtLoanFieldId18 = new TextBox();
      this.txtLoanFieldId10 = new TextBox();
      this.txtLoanFieldId7 = new TextBox();
      this.lblDirection = new Label();
      this.txtLoanFieldDescription3 = new TextBox();
      this.txtLoanFieldId19 = new TextBox();
      this.txtLoanFieldDescription16 = new TextBox();
      this.txtLoanFieldId2 = new TextBox();
      this.chkTwoWayTransfer9 = new CheckBox();
      this.chkTwoWayTransfer11 = new CheckBox();
      this.txtLoanFieldDescription2 = new TextBox();
      this.txtLoanFieldId20 = new TextBox();
      this.txtLoanFieldDescription14 = new TextBox();
      this.txtLoanFieldId9 = new TextBox();
      this.txtLoanFieldId8 = new TextBox();
      this.txtLoanFieldDescription15 = new TextBox();
      this.txtLoanFieldDescription1 = new TextBox();
      this.lblLoanFieldDescription = new Label();
      this.chkTwoWayTransfer10 = new CheckBox();
      this.chkTwoWayTransfer17 = new CheckBox();
      this.gradientPanel2 = new GradientPanel();
      this.lblLoanFieldMapping = new Label();
      this.tabPage2 = new TabPage();
      this.tabPage3 = new TabPage();
      this.tabPage4 = new TabPage();
      this.tabPage5 = new TabPage();
      this.splitter1 = new Splitter();
      this.tipCustomField = new ToolTip(this.components);
      this.borderPanel1 = new BorderPanel();
      this.tabCustomFields.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.pnlData.SuspendLayout();
      this.gcCustomTab.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.pnlCustomFieldsInternal.SuspendLayout();
      ((ISupportInitialize) this.btnFieldOption20).BeginInit();
      ((ISupportInitialize) this.btnFieldOption19).BeginInit();
      ((ISupportInitialize) this.btnFieldOption18).BeginInit();
      ((ISupportInitialize) this.btnFieldOption17).BeginInit();
      ((ISupportInitialize) this.btnFieldOption16).BeginInit();
      ((ISupportInitialize) this.btnFieldOption15).BeginInit();
      ((ISupportInitialize) this.btnFieldOption14).BeginInit();
      ((ISupportInitialize) this.btnFieldOption13).BeginInit();
      ((ISupportInitialize) this.btnFieldOption12).BeginInit();
      ((ISupportInitialize) this.btnFieldOption11).BeginInit();
      ((ISupportInitialize) this.btnFieldOption10).BeginInit();
      ((ISupportInitialize) this.btnFieldOption9).BeginInit();
      ((ISupportInitialize) this.btnFieldOption8).BeginInit();
      ((ISupportInitialize) this.btnFieldOption1).BeginInit();
      ((ISupportInitialize) this.btnFieldOption7).BeginInit();
      ((ISupportInitialize) this.btnFieldOption2).BeginInit();
      ((ISupportInitialize) this.btnFieldOption3).BeginInit();
      ((ISupportInitialize) this.btnFieldOption6).BeginInit();
      ((ISupportInitialize) this.btnFieldOption4).BeginInit();
      ((ISupportInitialize) this.btnFieldOption5).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.gcCustomFieldDefinitions.SuspendLayout();
      this.borderPanel3.SuspendLayout();
      this.pnlExRightMapping.SuspendLayout();
      ((ISupportInitialize) this.picLoanFieldSearch20).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch19).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch18).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch17).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch16).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch15).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch14).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch13).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch12).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch11).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch10).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch9).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch8).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch7).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch6).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch5).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch4).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch3).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch2).BeginInit();
      ((ISupportInitialize) this.picLoanFieldSearch1).BeginInit();
      this.gradientPanel2.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tabCustomFields.Controls.Add((Control) this.tabPage1);
      this.tabCustomFields.Controls.Add((Control) this.tabPage2);
      this.tabCustomFields.Controls.Add((Control) this.tabPage3);
      this.tabCustomFields.Controls.Add((Control) this.tabPage4);
      this.tabCustomFields.Controls.Add((Control) this.tabPage5);
      this.tabCustomFields.Dock = DockStyle.Fill;
      this.tabCustomFields.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabCustomFields.Location = new Point(2, 1);
      this.tabCustomFields.Name = "tabCustomFields";
      this.tabCustomFields.SelectedIndex = 0;
      this.tabCustomFields.Size = new Size(787, 575);
      this.tabCustomFields.TabIndex = 1;
      this.tabCustomFields.SelectedIndexChanged += new EventHandler(this.tabCustomFields_SelectedIndexChanged);
      this.tabPage1.AutoScroll = true;
      this.tabPage1.BackColor = Color.WhiteSmoke;
      this.tabPage1.Controls.Add((Control) this.pnlData);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Size = new Size(779, 549);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Page 1";
      this.pnlData.AutoScroll = true;
      this.pnlData.BackColor = Color.WhiteSmoke;
      this.pnlData.Controls.Add((Control) this.gcCustomTab);
      this.pnlData.Controls.Add((Control) this.gcCustomFieldDefinitions);
      this.pnlData.Dock = DockStyle.Fill;
      this.pnlData.Location = new Point(0, 0);
      this.pnlData.Name = "pnlData";
      this.pnlData.Size = new Size(779, 549);
      this.pnlData.TabIndex = 353;
      this.gcCustomTab.Controls.Add((Control) this.borderPanel2);
      this.gcCustomTab.Controls.Add((Control) this.gradientPanel1);
      this.gcCustomTab.Controls.Add((Control) this.txtPageName);
      this.gcCustomTab.Controls.Add((Control) this.cboCategoryName);
      this.gcCustomTab.HeaderForeColor = SystemColors.ControlText;
      this.gcCustomTab.Location = new Point(0, 0);
      this.gcCustomTab.Name = "gcCustomTab";
      this.gcCustomTab.Size = new Size(436, 534);
      this.gcCustomTab.TabIndex = 352;
      this.gcCustomTab.Text = "Custom Tab Name";
      this.borderPanel2.Borders = AnchorStyles.Top;
      this.borderPanel2.Controls.Add((Control) this.pnlCustomFieldsInternal);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(1, 56);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(434, 477);
      this.borderPanel2.TabIndex = 152;
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect20);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect15);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect6);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect14);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect5);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect19);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect4);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect7);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect13);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect3);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect2);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect16);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect8);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect12);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect1);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect18);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblSelect);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect9);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect11);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect10);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.chkSelect17);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption20);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription1);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType13);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription13);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption19);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber13);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber12);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription14);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption18);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType12);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType14);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription12);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption17);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber14);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber11);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription15);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType11);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption16);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType15);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription11);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldDescription);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber15);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber10);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption15);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription16);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType10);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldType);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType16);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription10);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption14);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber16);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber9);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType1);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription17);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType9);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption13);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType17);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription9);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber1);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber17);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber8);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption12);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription18);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType8);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription2);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType18);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription8);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption11);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber18);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber7);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType2);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription19);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType7);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption10);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType19);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription7);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber2);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber19);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber6);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption9);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription20);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType6);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription3);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType20);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription6);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption8);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber20);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber5);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType3);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption1);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption7);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType5);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption2);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber3);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription5);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption3);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption6);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.lblFieldNumber4);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption4);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.txtFieldDescription4);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.cboFieldType4);
      this.pnlCustomFieldsInternal.Controls.Add((Control) this.btnFieldOption5);
      this.pnlCustomFieldsInternal.Dock = DockStyle.Fill;
      this.pnlCustomFieldsInternal.Location = new Point(0, 1);
      this.pnlCustomFieldsInternal.Name = "pnlCustomFieldsInternal";
      this.pnlCustomFieldsInternal.Size = new Size(434, 476);
      this.pnlCustomFieldsInternal.TabIndex = 153;
      this.chkSelect20.AutoSize = true;
      this.chkSelect20.Location = new Point(15, 444);
      this.chkSelect20.Name = "chkSelect20";
      this.chkSelect20.Size = new Size(15, 14);
      this.chkSelect20.TabIndex = 435;
      this.chkSelect20.Tag = (object) "20";
      this.chkSelect20.UseVisualStyleBackColor = true;
      this.chkSelect20.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect15.AutoSize = true;
      this.chkSelect15.Location = new Point(15, 334);
      this.chkSelect15.Name = "chkSelect15";
      this.chkSelect15.Size = new Size(15, 14);
      this.chkSelect15.TabIndex = 430;
      this.chkSelect15.Tag = (object) "15";
      this.chkSelect15.UseVisualStyleBackColor = true;
      this.chkSelect15.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect6.AutoSize = true;
      this.chkSelect6.Location = new Point(15, 136);
      this.chkSelect6.Name = "chkSelect6";
      this.chkSelect6.Size = new Size(15, 14);
      this.chkSelect6.TabIndex = 421;
      this.chkSelect6.Tag = (object) "6";
      this.chkSelect6.UseVisualStyleBackColor = true;
      this.chkSelect6.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect14.AutoSize = true;
      this.chkSelect14.Location = new Point(15, 312);
      this.chkSelect14.Name = "chkSelect14";
      this.chkSelect14.Size = new Size(15, 14);
      this.chkSelect14.TabIndex = 429;
      this.chkSelect14.Tag = (object) "14";
      this.chkSelect14.UseVisualStyleBackColor = true;
      this.chkSelect14.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect5.AutoSize = true;
      this.chkSelect5.Location = new Point(15, 114);
      this.chkSelect5.Name = "chkSelect5";
      this.chkSelect5.Size = new Size(15, 14);
      this.chkSelect5.TabIndex = 420;
      this.chkSelect5.Tag = (object) "5";
      this.chkSelect5.UseVisualStyleBackColor = true;
      this.chkSelect5.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect19.AutoSize = true;
      this.chkSelect19.Location = new Point(15, 422);
      this.chkSelect19.Name = "chkSelect19";
      this.chkSelect19.Size = new Size(15, 14);
      this.chkSelect19.TabIndex = 434;
      this.chkSelect19.Tag = (object) "19";
      this.chkSelect19.UseVisualStyleBackColor = true;
      this.chkSelect19.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect4.AutoSize = true;
      this.chkSelect4.Location = new Point(15, 92);
      this.chkSelect4.Name = "chkSelect4";
      this.chkSelect4.Size = new Size(15, 14);
      this.chkSelect4.TabIndex = 419;
      this.chkSelect4.Tag = (object) "4";
      this.chkSelect4.UseVisualStyleBackColor = true;
      this.chkSelect4.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect7.AutoSize = true;
      this.chkSelect7.Location = new Point(15, 158);
      this.chkSelect7.Name = "chkSelect7";
      this.chkSelect7.Size = new Size(15, 14);
      this.chkSelect7.TabIndex = 422;
      this.chkSelect7.Tag = (object) "7";
      this.chkSelect7.UseVisualStyleBackColor = true;
      this.chkSelect7.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect13.AutoSize = true;
      this.chkSelect13.Location = new Point(15, 290);
      this.chkSelect13.Name = "chkSelect13";
      this.chkSelect13.Size = new Size(15, 14);
      this.chkSelect13.TabIndex = 428;
      this.chkSelect13.Tag = (object) "13";
      this.chkSelect13.UseVisualStyleBackColor = true;
      this.chkSelect13.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect3.AutoSize = true;
      this.chkSelect3.Location = new Point(15, 70);
      this.chkSelect3.Name = "chkSelect3";
      this.chkSelect3.Size = new Size(15, 14);
      this.chkSelect3.TabIndex = 418;
      this.chkSelect3.Tag = (object) "3";
      this.chkSelect3.UseVisualStyleBackColor = true;
      this.chkSelect3.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect2.AutoSize = true;
      this.chkSelect2.Location = new Point(15, 48);
      this.chkSelect2.Name = "chkSelect2";
      this.chkSelect2.Size = new Size(15, 14);
      this.chkSelect2.TabIndex = 417;
      this.chkSelect2.Tag = (object) "2";
      this.chkSelect2.UseVisualStyleBackColor = true;
      this.chkSelect2.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect16.AutoSize = true;
      this.chkSelect16.Location = new Point(15, 356);
      this.chkSelect16.Name = "chkSelect16";
      this.chkSelect16.Size = new Size(15, 14);
      this.chkSelect16.TabIndex = 431;
      this.chkSelect16.Tag = (object) "16";
      this.chkSelect16.UseVisualStyleBackColor = true;
      this.chkSelect16.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect8.AutoSize = true;
      this.chkSelect8.Location = new Point(15, 180);
      this.chkSelect8.Name = "chkSelect8";
      this.chkSelect8.Size = new Size(15, 14);
      this.chkSelect8.TabIndex = 423;
      this.chkSelect8.Tag = (object) "8";
      this.chkSelect8.UseVisualStyleBackColor = true;
      this.chkSelect8.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect12.AutoSize = true;
      this.chkSelect12.Location = new Point(15, 268);
      this.chkSelect12.Name = "chkSelect12";
      this.chkSelect12.Size = new Size(15, 14);
      this.chkSelect12.TabIndex = 427;
      this.chkSelect12.Tag = (object) "12";
      this.chkSelect12.UseVisualStyleBackColor = true;
      this.chkSelect12.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect1.AutoSize = true;
      this.chkSelect1.Location = new Point(15, 26);
      this.chkSelect1.Name = "chkSelect1";
      this.chkSelect1.Size = new Size(15, 14);
      this.chkSelect1.TabIndex = 416;
      this.chkSelect1.Tag = (object) "1";
      this.chkSelect1.UseVisualStyleBackColor = true;
      this.chkSelect1.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect18.AutoSize = true;
      this.chkSelect18.Location = new Point(15, 400);
      this.chkSelect18.Name = "chkSelect18";
      this.chkSelect18.Size = new Size(15, 14);
      this.chkSelect18.TabIndex = 433;
      this.chkSelect18.Tag = (object) "18";
      this.chkSelect18.UseVisualStyleBackColor = true;
      this.chkSelect18.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.lblSelect.Location = new Point(-2, 5);
      this.lblSelect.Name = "lblSelect";
      this.lblSelect.Size = new Size(59, 13);
      this.lblSelect.TabIndex = 415;
      this.lblSelect.Text = "Select";
      this.lblSelect.TextAlign = ContentAlignment.MiddleCenter;
      this.chkSelect9.AutoSize = true;
      this.chkSelect9.Location = new Point(15, 202);
      this.chkSelect9.Name = "chkSelect9";
      this.chkSelect9.Size = new Size(15, 14);
      this.chkSelect9.TabIndex = 424;
      this.chkSelect9.Tag = (object) "9";
      this.chkSelect9.UseVisualStyleBackColor = true;
      this.chkSelect9.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect11.AutoSize = true;
      this.chkSelect11.Location = new Point(15, 246);
      this.chkSelect11.Name = "chkSelect11";
      this.chkSelect11.Size = new Size(15, 14);
      this.chkSelect11.TabIndex = 426;
      this.chkSelect11.Tag = (object) "11";
      this.chkSelect11.UseVisualStyleBackColor = true;
      this.chkSelect11.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect10.AutoSize = true;
      this.chkSelect10.Location = new Point(15, 224);
      this.chkSelect10.Name = "chkSelect10";
      this.chkSelect10.Size = new Size(15, 14);
      this.chkSelect10.TabIndex = 425;
      this.chkSelect10.Tag = (object) "10";
      this.chkSelect10.UseVisualStyleBackColor = true;
      this.chkSelect10.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.chkSelect17.AutoSize = true;
      this.chkSelect17.Location = new Point(15, 378);
      this.chkSelect17.Name = "chkSelect17";
      this.chkSelect17.Size = new Size(15, 14);
      this.chkSelect17.TabIndex = 432;
      this.chkSelect17.Tag = (object) "17";
      this.chkSelect17.UseVisualStyleBackColor = true;
      this.chkSelect17.CheckedChanged += new EventHandler(this.chkSelect_CheckedChanged);
      this.btnFieldOption20.BackColor = Color.Transparent;
      this.btnFieldOption20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption20.Location = new Point(411, 443);
      this.btnFieldOption20.MouseDownImage = (Image) null;
      this.btnFieldOption20.Name = "btnFieldOption20";
      this.btnFieldOption20.Size = new Size(16, 16);
      this.btnFieldOption20.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption20.TabIndex = 234;
      this.btnFieldOption20.TabStop = false;
      this.btnFieldOption20.Tag = (object) "20";
      this.btnFieldOption20.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption20, "Edit");
      this.btnFieldOption20.Click += new EventHandler(this.btnFieldOption_Click);
      this.txtFieldDescription1.Location = new Point(130, 23);
      this.txtFieldDescription1.MaxLength = 50;
      this.txtFieldDescription1.Name = "txtFieldDescription1";
      this.txtFieldDescription1.Size = new Size(128, 20);
      this.txtFieldDescription1.TabIndex = 156;
      this.txtFieldDescription1.Tag = (object) "1";
      this.txtFieldDescription1.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription1.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType13.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType13.ItemHeight = 13;
      this.cboFieldType13.Location = new Point(264, 287);
      this.cboFieldType13.Name = "cboFieldType13";
      this.cboFieldType13.Size = new Size(143, 21);
      this.cboFieldType13.TabIndex = 205;
      this.cboFieldType13.Tag = (object) "13";
      this.cboFieldType13.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription13.Location = new Point(130, 287);
      this.txtFieldDescription13.MaxLength = 50;
      this.txtFieldDescription13.Name = "txtFieldDescription13";
      this.txtFieldDescription13.Size = new Size(128, 20);
      this.txtFieldDescription13.TabIndex = 204;
      this.txtFieldDescription13.Tag = (object) "13";
      this.txtFieldDescription13.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription13.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption19.BackColor = Color.Transparent;
      this.btnFieldOption19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption19.Location = new Point(411, 421);
      this.btnFieldOption19.MouseDownImage = (Image) null;
      this.btnFieldOption19.Name = "btnFieldOption19";
      this.btnFieldOption19.Size = new Size(16, 16);
      this.btnFieldOption19.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption19.TabIndex = 230;
      this.btnFieldOption19.TabStop = false;
      this.btnFieldOption19.Tag = (object) "19";
      this.btnFieldOption19.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption19, "Edit");
      this.btnFieldOption19.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber13.AutoSize = true;
      this.lblFieldNumber13.Location = new Point(36, 289);
      this.lblFieldNumber13.Name = "lblFieldNumber13";
      this.lblFieldNumber13.Size = new Size(82, 13);
      this.lblFieldNumber13.TabIndex = 203;
      this.lblFieldNumber13.Tag = (object) "13";
      this.lblFieldNumber13.Text = "Custom Field 13";
      this.lblFieldNumber13.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber12.AutoSize = true;
      this.lblFieldNumber12.Location = new Point(36, 267);
      this.lblFieldNumber12.Name = "lblFieldNumber12";
      this.lblFieldNumber12.Size = new Size(82, 13);
      this.lblFieldNumber12.TabIndex = 199;
      this.lblFieldNumber12.Tag = (object) "12";
      this.lblFieldNumber12.Text = "Custom Field 12";
      this.lblFieldNumber12.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFieldDescription14.Location = new Point(130, 309);
      this.txtFieldDescription14.MaxLength = 50;
      this.txtFieldDescription14.Name = "txtFieldDescription14";
      this.txtFieldDescription14.Size = new Size(128, 20);
      this.txtFieldDescription14.TabIndex = 208;
      this.txtFieldDescription14.Tag = (object) "14";
      this.txtFieldDescription14.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription14.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption18.BackColor = Color.Transparent;
      this.btnFieldOption18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption18.Location = new Point(411, 399);
      this.btnFieldOption18.MouseDownImage = (Image) null;
      this.btnFieldOption18.Name = "btnFieldOption18";
      this.btnFieldOption18.Size = new Size(16, 16);
      this.btnFieldOption18.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption18.TabIndex = 226;
      this.btnFieldOption18.TabStop = false;
      this.btnFieldOption18.Tag = (object) "18";
      this.btnFieldOption18.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption18, "Edit");
      this.btnFieldOption18.Click += new EventHandler(this.btnFieldOption_Click);
      this.cboFieldType12.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType12.ItemHeight = 13;
      this.cboFieldType12.Location = new Point(264, 265);
      this.cboFieldType12.Name = "cboFieldType12";
      this.cboFieldType12.Size = new Size(143, 21);
      this.cboFieldType12.TabIndex = 201;
      this.cboFieldType12.Tag = (object) "12";
      this.cboFieldType12.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.cboFieldType14.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType14.ItemHeight = 13;
      this.cboFieldType14.Location = new Point(264, 309);
      this.cboFieldType14.Name = "cboFieldType14";
      this.cboFieldType14.Size = new Size(143, 21);
      this.cboFieldType14.TabIndex = 209;
      this.cboFieldType14.Tag = (object) "14";
      this.cboFieldType14.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription12.Location = new Point(130, 265);
      this.txtFieldDescription12.MaxLength = 50;
      this.txtFieldDescription12.Name = "txtFieldDescription12";
      this.txtFieldDescription12.Size = new Size(128, 20);
      this.txtFieldDescription12.TabIndex = 200;
      this.txtFieldDescription12.Tag = (object) "12";
      this.txtFieldDescription12.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription12.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption17.BackColor = Color.Transparent;
      this.btnFieldOption17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption17.Location = new Point(411, 377);
      this.btnFieldOption17.MouseDownImage = (Image) null;
      this.btnFieldOption17.Name = "btnFieldOption17";
      this.btnFieldOption17.Size = new Size(16, 16);
      this.btnFieldOption17.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption17.TabIndex = 222;
      this.btnFieldOption17.TabStop = false;
      this.btnFieldOption17.Tag = (object) "17";
      this.btnFieldOption17.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption17, "Edit");
      this.btnFieldOption17.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber14.AutoSize = true;
      this.lblFieldNumber14.Location = new Point(36, 311);
      this.lblFieldNumber14.Name = "lblFieldNumber14";
      this.lblFieldNumber14.Size = new Size(82, 13);
      this.lblFieldNumber14.TabIndex = 207;
      this.lblFieldNumber14.Tag = (object) "14";
      this.lblFieldNumber14.Text = "Custom Field 14";
      this.lblFieldNumber14.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber11.AutoSize = true;
      this.lblFieldNumber11.Location = new Point(36, 245);
      this.lblFieldNumber11.Name = "lblFieldNumber11";
      this.lblFieldNumber11.Size = new Size(82, 13);
      this.lblFieldNumber11.TabIndex = 195;
      this.lblFieldNumber11.Tag = (object) "11";
      this.lblFieldNumber11.Text = "Custom Field 11";
      this.lblFieldNumber11.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFieldDescription15.Location = new Point(130, 331);
      this.txtFieldDescription15.MaxLength = 50;
      this.txtFieldDescription15.Name = "txtFieldDescription15";
      this.txtFieldDescription15.Size = new Size(128, 20);
      this.txtFieldDescription15.TabIndex = 212;
      this.txtFieldDescription15.Tag = (object) "15";
      this.txtFieldDescription15.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription15.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType11.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType11.ItemHeight = 13;
      this.cboFieldType11.Location = new Point(264, 243);
      this.cboFieldType11.Name = "cboFieldType11";
      this.cboFieldType11.Size = new Size(143, 21);
      this.cboFieldType11.TabIndex = 197;
      this.cboFieldType11.Tag = (object) "11";
      this.cboFieldType11.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption16.BackColor = Color.Transparent;
      this.btnFieldOption16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption16.Location = new Point(411, 355);
      this.btnFieldOption16.MouseDownImage = (Image) null;
      this.btnFieldOption16.Name = "btnFieldOption16";
      this.btnFieldOption16.Size = new Size(16, 16);
      this.btnFieldOption16.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption16.TabIndex = 218;
      this.btnFieldOption16.TabStop = false;
      this.btnFieldOption16.Tag = (object) "16";
      this.btnFieldOption16.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption16, "Edit");
      this.btnFieldOption16.Click += new EventHandler(this.btnFieldOption_Click);
      this.cboFieldType15.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType15.ItemHeight = 13;
      this.cboFieldType15.Location = new Point(264, 331);
      this.cboFieldType15.Name = "cboFieldType15";
      this.cboFieldType15.Size = new Size(143, 21);
      this.cboFieldType15.TabIndex = 213;
      this.cboFieldType15.Tag = (object) "15";
      this.cboFieldType15.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription11.Location = new Point(130, 243);
      this.txtFieldDescription11.MaxLength = 50;
      this.txtFieldDescription11.Name = "txtFieldDescription11";
      this.txtFieldDescription11.Size = new Size(128, 20);
      this.txtFieldDescription11.TabIndex = 196;
      this.txtFieldDescription11.Tag = (object) "11";
      this.txtFieldDescription11.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription11.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.lblFieldDescription.Location = new Point((int) sbyte.MaxValue, 3);
      this.lblFieldDescription.Name = "lblFieldDescription";
      this.lblFieldDescription.Size = new Size(128, 16);
      this.lblFieldDescription.TabIndex = 153;
      this.lblFieldDescription.Text = "Field Description";
      this.lblFieldDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber15.AutoSize = true;
      this.lblFieldNumber15.Location = new Point(36, 333);
      this.lblFieldNumber15.Name = "lblFieldNumber15";
      this.lblFieldNumber15.Size = new Size(82, 13);
      this.lblFieldNumber15.TabIndex = 211;
      this.lblFieldNumber15.Tag = (object) "15";
      this.lblFieldNumber15.Text = "Custom Field 15";
      this.lblFieldNumber15.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber10.AutoSize = true;
      this.lblFieldNumber10.Location = new Point(36, 223);
      this.lblFieldNumber10.Name = "lblFieldNumber10";
      this.lblFieldNumber10.Size = new Size(82, 13);
      this.lblFieldNumber10.TabIndex = 191;
      this.lblFieldNumber10.Tag = (object) "10";
      this.lblFieldNumber10.Text = "Custom Field 10";
      this.lblFieldNumber10.TextAlign = ContentAlignment.MiddleLeft;
      this.btnFieldOption15.BackColor = Color.Transparent;
      this.btnFieldOption15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption15.Location = new Point(411, 333);
      this.btnFieldOption15.MouseDownImage = (Image) null;
      this.btnFieldOption15.Name = "btnFieldOption15";
      this.btnFieldOption15.Size = new Size(16, 16);
      this.btnFieldOption15.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption15.TabIndex = 214;
      this.btnFieldOption15.TabStop = false;
      this.btnFieldOption15.Tag = (object) "15";
      this.btnFieldOption15.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption15, "Edit");
      this.btnFieldOption15.Click += new EventHandler(this.btnFieldOption_Click);
      this.txtFieldDescription16.Location = new Point(130, 353);
      this.txtFieldDescription16.MaxLength = 50;
      this.txtFieldDescription16.Name = "txtFieldDescription16";
      this.txtFieldDescription16.Size = new Size(128, 20);
      this.txtFieldDescription16.TabIndex = 216;
      this.txtFieldDescription16.Tag = (object) "16";
      this.txtFieldDescription16.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription16.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType10.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType10.ItemHeight = 13;
      this.cboFieldType10.Location = new Point(264, 221);
      this.cboFieldType10.Name = "cboFieldType10";
      this.cboFieldType10.Size = new Size(143, 21);
      this.cboFieldType10.TabIndex = 193;
      this.cboFieldType10.Tag = (object) "10";
      this.cboFieldType10.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.lblFieldType.Location = new Point(262, 3);
      this.lblFieldType.Name = "lblFieldType";
      this.lblFieldType.Size = new Size(143, 16);
      this.lblFieldType.TabIndex = 154;
      this.lblFieldType.Text = "Field Type";
      this.lblFieldType.TextAlign = ContentAlignment.MiddleLeft;
      this.cboFieldType16.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType16.ItemHeight = 13;
      this.cboFieldType16.Location = new Point(264, 353);
      this.cboFieldType16.Name = "cboFieldType16";
      this.cboFieldType16.Size = new Size(143, 21);
      this.cboFieldType16.TabIndex = 217;
      this.cboFieldType16.Tag = (object) "16";
      this.cboFieldType16.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription10.Location = new Point(130, 221);
      this.txtFieldDescription10.MaxLength = 50;
      this.txtFieldDescription10.Name = "txtFieldDescription10";
      this.txtFieldDescription10.Size = new Size(128, 20);
      this.txtFieldDescription10.TabIndex = 192;
      this.txtFieldDescription10.Tag = (object) "10";
      this.txtFieldDescription10.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription10.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption14.BackColor = Color.Transparent;
      this.btnFieldOption14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption14.Location = new Point(411, 311);
      this.btnFieldOption14.MouseDownImage = (Image) null;
      this.btnFieldOption14.Name = "btnFieldOption14";
      this.btnFieldOption14.Size = new Size(16, 16);
      this.btnFieldOption14.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption14.TabIndex = 210;
      this.btnFieldOption14.TabStop = false;
      this.btnFieldOption14.Tag = (object) "14";
      this.btnFieldOption14.Text = "...";
      this.btnFieldOption14.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber16.AutoSize = true;
      this.lblFieldNumber16.Location = new Point(36, 355);
      this.lblFieldNumber16.Name = "lblFieldNumber16";
      this.lblFieldNumber16.Size = new Size(82, 13);
      this.lblFieldNumber16.TabIndex = 215;
      this.lblFieldNumber16.Tag = (object) "16";
      this.lblFieldNumber16.Text = "Custom Field 16";
      this.lblFieldNumber16.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber9.AutoSize = true;
      this.lblFieldNumber9.Location = new Point(36, 201);
      this.lblFieldNumber9.Name = "lblFieldNumber9";
      this.lblFieldNumber9.Size = new Size(82, 13);
      this.lblFieldNumber9.TabIndex = 187;
      this.lblFieldNumber9.Tag = (object) "9";
      this.lblFieldNumber9.Text = "Custom Field 09";
      this.lblFieldNumber9.TextAlign = ContentAlignment.MiddleLeft;
      this.cboFieldType1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType1.ItemHeight = 13;
      this.cboFieldType1.Location = new Point(264, 23);
      this.cboFieldType1.Name = "cboFieldType1";
      this.cboFieldType1.Size = new Size(143, 21);
      this.cboFieldType1.TabIndex = 157;
      this.cboFieldType1.Tag = (object) "1";
      this.cboFieldType1.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription17.Location = new Point(130, 375);
      this.txtFieldDescription17.MaxLength = 50;
      this.txtFieldDescription17.Name = "txtFieldDescription17";
      this.txtFieldDescription17.Size = new Size(128, 20);
      this.txtFieldDescription17.TabIndex = 220;
      this.txtFieldDescription17.Tag = (object) "17";
      this.txtFieldDescription17.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription17.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType9.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType9.ItemHeight = 13;
      this.cboFieldType9.Location = new Point(264, 199);
      this.cboFieldType9.Name = "cboFieldType9";
      this.cboFieldType9.Size = new Size(143, 21);
      this.cboFieldType9.TabIndex = 189;
      this.cboFieldType9.Tag = (object) "9";
      this.cboFieldType9.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption13.BackColor = Color.Transparent;
      this.btnFieldOption13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption13.Location = new Point(411, 289);
      this.btnFieldOption13.MouseDownImage = (Image) null;
      this.btnFieldOption13.Name = "btnFieldOption13";
      this.btnFieldOption13.Size = new Size(16, 16);
      this.btnFieldOption13.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption13.TabIndex = 206;
      this.btnFieldOption13.TabStop = false;
      this.btnFieldOption13.Tag = (object) "13";
      this.btnFieldOption13.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption13, "Edit");
      this.btnFieldOption13.Click += new EventHandler(this.btnFieldOption_Click);
      this.cboFieldType17.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType17.ItemHeight = 13;
      this.cboFieldType17.Location = new Point(264, 375);
      this.cboFieldType17.Name = "cboFieldType17";
      this.cboFieldType17.Size = new Size(143, 21);
      this.cboFieldType17.TabIndex = 221;
      this.cboFieldType17.Tag = (object) "17";
      this.cboFieldType17.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription9.Location = new Point(130, 199);
      this.txtFieldDescription9.MaxLength = 50;
      this.txtFieldDescription9.Name = "txtFieldDescription9";
      this.txtFieldDescription9.Size = new Size(128, 20);
      this.txtFieldDescription9.TabIndex = 188;
      this.txtFieldDescription9.Tag = (object) "9";
      this.txtFieldDescription9.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription9.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.lblFieldNumber1.AutoSize = true;
      this.lblFieldNumber1.Location = new Point(36, 25);
      this.lblFieldNumber1.Name = "lblFieldNumber1";
      this.lblFieldNumber1.Size = new Size(82, 13);
      this.lblFieldNumber1.TabIndex = 155;
      this.lblFieldNumber1.Tag = (object) "1";
      this.lblFieldNumber1.Text = "Custom Field 01";
      this.lblFieldNumber1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber17.AutoSize = true;
      this.lblFieldNumber17.Location = new Point(36, 377);
      this.lblFieldNumber17.Name = "lblFieldNumber17";
      this.lblFieldNumber17.Size = new Size(82, 13);
      this.lblFieldNumber17.TabIndex = 219;
      this.lblFieldNumber17.Tag = (object) "17";
      this.lblFieldNumber17.Text = "Custom Field 17";
      this.lblFieldNumber17.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber8.AutoSize = true;
      this.lblFieldNumber8.Location = new Point(36, 179);
      this.lblFieldNumber8.Name = "lblFieldNumber8";
      this.lblFieldNumber8.Size = new Size(82, 13);
      this.lblFieldNumber8.TabIndex = 183;
      this.lblFieldNumber8.Tag = (object) "8";
      this.lblFieldNumber8.Text = "Custom Field 08";
      this.lblFieldNumber8.TextAlign = ContentAlignment.MiddleLeft;
      this.btnFieldOption12.BackColor = Color.Transparent;
      this.btnFieldOption12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption12.Location = new Point(411, 267);
      this.btnFieldOption12.MouseDownImage = (Image) null;
      this.btnFieldOption12.Name = "btnFieldOption12";
      this.btnFieldOption12.Size = new Size(16, 16);
      this.btnFieldOption12.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption12.TabIndex = 202;
      this.btnFieldOption12.TabStop = false;
      this.btnFieldOption12.Tag = (object) "12";
      this.btnFieldOption12.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption12, "Edit");
      this.btnFieldOption12.Click += new EventHandler(this.btnFieldOption_Click);
      this.txtFieldDescription18.Location = new Point(130, 397);
      this.txtFieldDescription18.MaxLength = 50;
      this.txtFieldDescription18.Name = "txtFieldDescription18";
      this.txtFieldDescription18.Size = new Size(128, 20);
      this.txtFieldDescription18.TabIndex = 224;
      this.txtFieldDescription18.Tag = (object) "18";
      this.txtFieldDescription18.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription18.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType8.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType8.ItemHeight = 13;
      this.cboFieldType8.Location = new Point(264, 177);
      this.cboFieldType8.Name = "cboFieldType8";
      this.cboFieldType8.Size = new Size(143, 21);
      this.cboFieldType8.TabIndex = 185;
      this.cboFieldType8.Tag = (object) "8";
      this.cboFieldType8.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription2.Location = new Point(130, 45);
      this.txtFieldDescription2.MaxLength = 50;
      this.txtFieldDescription2.Name = "txtFieldDescription2";
      this.txtFieldDescription2.Size = new Size(128, 20);
      this.txtFieldDescription2.TabIndex = 160;
      this.txtFieldDescription2.Tag = (object) "2";
      this.txtFieldDescription2.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription2.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType18.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType18.ItemHeight = 13;
      this.cboFieldType18.Location = new Point(264, 397);
      this.cboFieldType18.Name = "cboFieldType18";
      this.cboFieldType18.Size = new Size(143, 21);
      this.cboFieldType18.TabIndex = 225;
      this.cboFieldType18.Tag = (object) "18";
      this.cboFieldType18.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription8.Location = new Point(130, 177);
      this.txtFieldDescription8.MaxLength = 50;
      this.txtFieldDescription8.Name = "txtFieldDescription8";
      this.txtFieldDescription8.Size = new Size(128, 20);
      this.txtFieldDescription8.TabIndex = 184;
      this.txtFieldDescription8.Tag = (object) "8";
      this.txtFieldDescription8.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription8.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption11.BackColor = Color.Transparent;
      this.btnFieldOption11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption11.Location = new Point(411, 245);
      this.btnFieldOption11.MouseDownImage = (Image) null;
      this.btnFieldOption11.Name = "btnFieldOption11";
      this.btnFieldOption11.Size = new Size(16, 16);
      this.btnFieldOption11.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption11.TabIndex = 198;
      this.btnFieldOption11.TabStop = false;
      this.btnFieldOption11.Tag = (object) "11";
      this.btnFieldOption11.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption11, "Edit");
      this.btnFieldOption11.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber18.AutoSize = true;
      this.lblFieldNumber18.Location = new Point(36, 399);
      this.lblFieldNumber18.Name = "lblFieldNumber18";
      this.lblFieldNumber18.Size = new Size(82, 13);
      this.lblFieldNumber18.TabIndex = 223;
      this.lblFieldNumber18.Tag = (object) "18";
      this.lblFieldNumber18.Text = "Custom Field 18";
      this.lblFieldNumber18.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber7.AutoSize = true;
      this.lblFieldNumber7.Location = new Point(36, 157);
      this.lblFieldNumber7.Name = "lblFieldNumber7";
      this.lblFieldNumber7.Size = new Size(82, 13);
      this.lblFieldNumber7.TabIndex = 179;
      this.lblFieldNumber7.Tag = (object) "7";
      this.lblFieldNumber7.Text = "Custom Field 07";
      this.lblFieldNumber7.TextAlign = ContentAlignment.MiddleLeft;
      this.cboFieldType2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType2.ItemHeight = 13;
      this.cboFieldType2.Location = new Point(264, 45);
      this.cboFieldType2.Name = "cboFieldType2";
      this.cboFieldType2.Size = new Size(143, 21);
      this.cboFieldType2.TabIndex = 161;
      this.cboFieldType2.Tag = (object) "2";
      this.cboFieldType2.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription19.Location = new Point(130, 419);
      this.txtFieldDescription19.MaxLength = 50;
      this.txtFieldDescription19.Name = "txtFieldDescription19";
      this.txtFieldDescription19.Size = new Size(128, 20);
      this.txtFieldDescription19.TabIndex = 228;
      this.txtFieldDescription19.Tag = (object) "19";
      this.txtFieldDescription19.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription19.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType7.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType7.ItemHeight = 13;
      this.cboFieldType7.Location = new Point(264, 155);
      this.cboFieldType7.Name = "cboFieldType7";
      this.cboFieldType7.Size = new Size(143, 21);
      this.cboFieldType7.TabIndex = 181;
      this.cboFieldType7.Tag = (object) "7";
      this.cboFieldType7.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption10.BackColor = Color.Transparent;
      this.btnFieldOption10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption10.Location = new Point(411, 223);
      this.btnFieldOption10.MouseDownImage = (Image) null;
      this.btnFieldOption10.Name = "btnFieldOption10";
      this.btnFieldOption10.Size = new Size(16, 16);
      this.btnFieldOption10.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption10.TabIndex = 194;
      this.btnFieldOption10.TabStop = false;
      this.btnFieldOption10.Tag = (object) "10";
      this.btnFieldOption10.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption10, "Edit");
      this.btnFieldOption10.Click += new EventHandler(this.btnFieldOption_Click);
      this.cboFieldType19.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType19.ItemHeight = 13;
      this.cboFieldType19.Location = new Point(264, 419);
      this.cboFieldType19.Name = "cboFieldType19";
      this.cboFieldType19.Size = new Size(143, 21);
      this.cboFieldType19.TabIndex = 229;
      this.cboFieldType19.Tag = (object) "19";
      this.cboFieldType19.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription7.Location = new Point(130, 155);
      this.txtFieldDescription7.MaxLength = 50;
      this.txtFieldDescription7.Name = "txtFieldDescription7";
      this.txtFieldDescription7.Size = new Size(128, 20);
      this.txtFieldDescription7.TabIndex = 180;
      this.txtFieldDescription7.Tag = (object) "7";
      this.txtFieldDescription7.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription7.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.lblFieldNumber2.AutoSize = true;
      this.lblFieldNumber2.Location = new Point(36, 47);
      this.lblFieldNumber2.Name = "lblFieldNumber2";
      this.lblFieldNumber2.Size = new Size(82, 13);
      this.lblFieldNumber2.TabIndex = 159;
      this.lblFieldNumber2.Tag = (object) "2";
      this.lblFieldNumber2.Text = "Custom Field 02";
      this.lblFieldNumber2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber19.AutoSize = true;
      this.lblFieldNumber19.Location = new Point(36, 421);
      this.lblFieldNumber19.Name = "lblFieldNumber19";
      this.lblFieldNumber19.Size = new Size(82, 13);
      this.lblFieldNumber19.TabIndex = 227;
      this.lblFieldNumber19.Tag = (object) "19";
      this.lblFieldNumber19.Text = "Custom Field 19";
      this.lblFieldNumber19.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber6.AutoSize = true;
      this.lblFieldNumber6.Location = new Point(36, 135);
      this.lblFieldNumber6.Name = "lblFieldNumber6";
      this.lblFieldNumber6.Size = new Size(82, 13);
      this.lblFieldNumber6.TabIndex = 175;
      this.lblFieldNumber6.Tag = (object) "6";
      this.lblFieldNumber6.Text = "Custom Field 06";
      this.lblFieldNumber6.TextAlign = ContentAlignment.MiddleLeft;
      this.btnFieldOption9.BackColor = Color.Transparent;
      this.btnFieldOption9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption9.Location = new Point(411, 201);
      this.btnFieldOption9.MouseDownImage = (Image) null;
      this.btnFieldOption9.Name = "btnFieldOption9";
      this.btnFieldOption9.Size = new Size(16, 16);
      this.btnFieldOption9.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption9.TabIndex = 190;
      this.btnFieldOption9.TabStop = false;
      this.btnFieldOption9.Tag = (object) "9";
      this.btnFieldOption9.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption9, "Edit");
      this.btnFieldOption9.Click += new EventHandler(this.btnFieldOption_Click);
      this.txtFieldDescription20.Location = new Point(130, 441);
      this.txtFieldDescription20.MaxLength = 50;
      this.txtFieldDescription20.Name = "txtFieldDescription20";
      this.txtFieldDescription20.Size = new Size(128, 20);
      this.txtFieldDescription20.TabIndex = 232;
      this.txtFieldDescription20.Tag = (object) "20";
      this.txtFieldDescription20.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription20.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType6.ItemHeight = 13;
      this.cboFieldType6.Location = new Point(264, 133);
      this.cboFieldType6.Name = "cboFieldType6";
      this.cboFieldType6.Size = new Size(143, 21);
      this.cboFieldType6.TabIndex = 177;
      this.cboFieldType6.Tag = (object) "6";
      this.cboFieldType6.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription3.Location = new Point(130, 67);
      this.txtFieldDescription3.MaxLength = 50;
      this.txtFieldDescription3.Name = "txtFieldDescription3";
      this.txtFieldDescription3.Size = new Size(128, 20);
      this.txtFieldDescription3.TabIndex = 164;
      this.txtFieldDescription3.Tag = (object) "3";
      this.txtFieldDescription3.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription3.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType20.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType20.ItemHeight = 13;
      this.cboFieldType20.Location = new Point(264, 441);
      this.cboFieldType20.Name = "cboFieldType20";
      this.cboFieldType20.Size = new Size(143, 21);
      this.cboFieldType20.TabIndex = 233;
      this.cboFieldType20.Tag = (object) "20";
      this.cboFieldType20.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.txtFieldDescription6.Location = new Point(130, 133);
      this.txtFieldDescription6.MaxLength = 50;
      this.txtFieldDescription6.Name = "txtFieldDescription6";
      this.txtFieldDescription6.Size = new Size(128, 20);
      this.txtFieldDescription6.TabIndex = 176;
      this.txtFieldDescription6.Tag = (object) "6";
      this.txtFieldDescription6.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription6.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption8.BackColor = Color.Transparent;
      this.btnFieldOption8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption8.Location = new Point(411, 179);
      this.btnFieldOption8.MouseDownImage = (Image) null;
      this.btnFieldOption8.Name = "btnFieldOption8";
      this.btnFieldOption8.Size = new Size(16, 16);
      this.btnFieldOption8.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption8.TabIndex = 186;
      this.btnFieldOption8.TabStop = false;
      this.btnFieldOption8.Tag = (object) "8";
      this.btnFieldOption8.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption8, "Edit");
      this.btnFieldOption8.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber20.AutoSize = true;
      this.lblFieldNumber20.Location = new Point(36, 443);
      this.lblFieldNumber20.Name = "lblFieldNumber20";
      this.lblFieldNumber20.Size = new Size(82, 13);
      this.lblFieldNumber20.TabIndex = 231;
      this.lblFieldNumber20.Tag = (object) "20";
      this.lblFieldNumber20.Text = "Custom Field 20";
      this.lblFieldNumber20.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldNumber5.AutoSize = true;
      this.lblFieldNumber5.Location = new Point(36, 113);
      this.lblFieldNumber5.Name = "lblFieldNumber5";
      this.lblFieldNumber5.Size = new Size(82, 13);
      this.lblFieldNumber5.TabIndex = 171;
      this.lblFieldNumber5.Tag = (object) "5";
      this.lblFieldNumber5.Text = "Custom Field 05";
      this.lblFieldNumber5.TextAlign = ContentAlignment.MiddleLeft;
      this.cboFieldType3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType3.ItemHeight = 13;
      this.cboFieldType3.Location = new Point(264, 67);
      this.cboFieldType3.Name = "cboFieldType3";
      this.cboFieldType3.Size = new Size(143, 21);
      this.cboFieldType3.TabIndex = 165;
      this.cboFieldType3.Tag = (object) "3";
      this.cboFieldType3.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption1.BackColor = Color.Transparent;
      this.btnFieldOption1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption1.Location = new Point(411, 25);
      this.btnFieldOption1.MouseDownImage = (Image) null;
      this.btnFieldOption1.Name = "btnFieldOption1";
      this.btnFieldOption1.Size = new Size(16, 16);
      this.btnFieldOption1.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption1.TabIndex = 158;
      this.btnFieldOption1.TabStop = false;
      this.btnFieldOption1.Tag = (object) "1";
      this.btnFieldOption1.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption1, "Edit");
      this.btnFieldOption1.Click += new EventHandler(this.btnFieldOption_Click);
      this.btnFieldOption7.BackColor = Color.Transparent;
      this.btnFieldOption7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption7.Location = new Point(411, 157);
      this.btnFieldOption7.MouseDownImage = (Image) null;
      this.btnFieldOption7.Name = "btnFieldOption7";
      this.btnFieldOption7.Size = new Size(16, 16);
      this.btnFieldOption7.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption7.TabIndex = 182;
      this.btnFieldOption7.TabStop = false;
      this.btnFieldOption7.Tag = (object) "7";
      this.btnFieldOption7.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption7, "Edit");
      this.btnFieldOption7.Click += new EventHandler(this.btnFieldOption_Click);
      this.cboFieldType5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType5.ItemHeight = 13;
      this.cboFieldType5.Location = new Point(264, 111);
      this.cboFieldType5.Name = "cboFieldType5";
      this.cboFieldType5.Size = new Size(143, 21);
      this.cboFieldType5.TabIndex = 173;
      this.cboFieldType5.Tag = (object) "5";
      this.cboFieldType5.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption2.BackColor = Color.Transparent;
      this.btnFieldOption2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption2.Location = new Point(411, 47);
      this.btnFieldOption2.MouseDownImage = (Image) null;
      this.btnFieldOption2.Name = "btnFieldOption2";
      this.btnFieldOption2.Size = new Size(16, 16);
      this.btnFieldOption2.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption2.TabIndex = 162;
      this.btnFieldOption2.TabStop = false;
      this.btnFieldOption2.Tag = (object) "2";
      this.btnFieldOption2.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption2, "Edit");
      this.btnFieldOption2.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber3.AutoSize = true;
      this.lblFieldNumber3.Location = new Point(36, 69);
      this.lblFieldNumber3.Name = "lblFieldNumber3";
      this.lblFieldNumber3.Size = new Size(82, 13);
      this.lblFieldNumber3.TabIndex = 163;
      this.lblFieldNumber3.Tag = (object) "3";
      this.lblFieldNumber3.Text = "Custom Field 03";
      this.lblFieldNumber3.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFieldDescription5.Location = new Point(130, 111);
      this.txtFieldDescription5.MaxLength = 50;
      this.txtFieldDescription5.Name = "txtFieldDescription5";
      this.txtFieldDescription5.Size = new Size(128, 20);
      this.txtFieldDescription5.TabIndex = 172;
      this.txtFieldDescription5.Tag = (object) "5";
      this.txtFieldDescription5.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription5.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.btnFieldOption3.BackColor = Color.Transparent;
      this.btnFieldOption3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption3.Location = new Point(411, 69);
      this.btnFieldOption3.MouseDownImage = (Image) null;
      this.btnFieldOption3.Name = "btnFieldOption3";
      this.btnFieldOption3.Size = new Size(16, 16);
      this.btnFieldOption3.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption3.TabIndex = 166;
      this.btnFieldOption3.TabStop = false;
      this.btnFieldOption3.Tag = (object) "3";
      this.btnFieldOption3.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption3, "Edit");
      this.btnFieldOption3.Click += new EventHandler(this.btnFieldOption_Click);
      this.btnFieldOption6.BackColor = Color.Transparent;
      this.btnFieldOption6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption6.Location = new Point(411, 135);
      this.btnFieldOption6.MouseDownImage = (Image) null;
      this.btnFieldOption6.Name = "btnFieldOption6";
      this.btnFieldOption6.Size = new Size(16, 16);
      this.btnFieldOption6.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption6.TabIndex = 178;
      this.btnFieldOption6.TabStop = false;
      this.btnFieldOption6.Tag = (object) "6";
      this.btnFieldOption6.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption6, "Edit");
      this.btnFieldOption6.Click += new EventHandler(this.btnFieldOption_Click);
      this.lblFieldNumber4.AutoSize = true;
      this.lblFieldNumber4.Location = new Point(36, 91);
      this.lblFieldNumber4.Name = "lblFieldNumber4";
      this.lblFieldNumber4.Size = new Size(82, 13);
      this.lblFieldNumber4.TabIndex = 167;
      this.lblFieldNumber4.Tag = (object) "4";
      this.lblFieldNumber4.Text = "Custom Field 04";
      this.lblFieldNumber4.TextAlign = ContentAlignment.MiddleLeft;
      this.btnFieldOption4.BackColor = Color.Transparent;
      this.btnFieldOption4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption4.Location = new Point(411, 91);
      this.btnFieldOption4.MouseDownImage = (Image) null;
      this.btnFieldOption4.Name = "btnFieldOption4";
      this.btnFieldOption4.Size = new Size(16, 16);
      this.btnFieldOption4.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption4.TabIndex = 170;
      this.btnFieldOption4.TabStop = false;
      this.btnFieldOption4.Tag = (object) "4";
      this.btnFieldOption4.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption4, "Edit");
      this.btnFieldOption4.Click += new EventHandler(this.btnFieldOption_Click);
      this.txtFieldDescription4.Location = new Point(130, 89);
      this.txtFieldDescription4.MaxLength = 50;
      this.txtFieldDescription4.Name = "txtFieldDescription4";
      this.txtFieldDescription4.Size = new Size(128, 20);
      this.txtFieldDescription4.TabIndex = 168;
      this.txtFieldDescription4.Tag = (object) "4";
      this.txtFieldDescription4.TextChanged += new EventHandler(this.txtFieldDescription_TextChanged);
      this.txtFieldDescription4.Leave += new EventHandler(this.txtFieldDescription_Leave);
      this.cboFieldType4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType4.ItemHeight = 13;
      this.cboFieldType4.Location = new Point(264, 89);
      this.cboFieldType4.Name = "cboFieldType4";
      this.cboFieldType4.Size = new Size(143, 21);
      this.cboFieldType4.TabIndex = 169;
      this.cboFieldType4.Tag = (object) "4";
      this.cboFieldType4.SelectedIndexChanged += new EventHandler(this.cboFieldType_SelectedIndexChanged);
      this.btnFieldOption5.BackColor = Color.Transparent;
      this.btnFieldOption5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnFieldOption5.Location = new Point(411, 113);
      this.btnFieldOption5.MouseDownImage = (Image) null;
      this.btnFieldOption5.Name = "btnFieldOption5";
      this.btnFieldOption5.Size = new Size(16, 16);
      this.btnFieldOption5.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnFieldOption5.TabIndex = 174;
      this.btnFieldOption5.TabStop = false;
      this.btnFieldOption5.Tag = (object) "5";
      this.btnFieldOption5.Text = "...";
      this.tipCustomField.SetToolTip((Control) this.btnFieldOption5, "Edit");
      this.btnFieldOption5.Click += new EventHandler(this.btnFieldOption_Click);
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.lblDirections);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(434, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.lblDirections.BackColor = Color.Transparent;
      this.lblDirections.Dock = DockStyle.Fill;
      this.lblDirections.Font = new Font("Arial", 8.25f);
      this.lblDirections.Location = new Point(0, 0);
      this.lblDirections.Name = "lblDirections";
      this.lblDirections.Padding = new Padding(10, 0, 0, 0);
      this.lblDirections.Size = new Size(434, 30);
      this.lblDirections.TabIndex = 148;
      this.lblDirections.Text = "The name of the tab will display on the <Borrower/Business> Contacts screen.";
      this.lblDirections.TextAlign = ContentAlignment.MiddleLeft;
      this.txtPageName.Location = new Point(120, 3);
      this.txtPageName.MaxLength = 250;
      this.txtPageName.Name = "txtPageName";
      this.txtPageName.Size = new Size(173, 20);
      this.txtPageName.TabIndex = 149;
      this.txtPageName.Tag = (object) "1";
      this.txtPageName.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtPageName.Leave += new EventHandler(this.txtPageName_Leave);
      this.cboCategoryName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategoryName.Location = new Point(115, 3);
      this.cboCategoryName.Name = "cboCategoryName";
      this.cboCategoryName.Size = new Size(173, 21);
      this.cboCategoryName.TabIndex = 151;
      this.cboCategoryName.Visible = false;
      this.gcCustomFieldDefinitions.Controls.Add((Control) this.borderPanel3);
      this.gcCustomFieldDefinitions.Controls.Add((Control) this.gradientPanel2);
      this.gcCustomFieldDefinitions.HeaderForeColor = SystemColors.ControlText;
      this.gcCustomFieldDefinitions.Location = new Point(437, 0);
      this.gcCustomFieldDefinitions.Margin = new Padding(0);
      this.gcCustomFieldDefinitions.Name = "gcCustomFieldDefinitions";
      this.gcCustomFieldDefinitions.Size = new Size(338, 533);
      this.gcCustomFieldDefinitions.TabIndex = 351;
      this.gcCustomFieldDefinitions.Text = "Map Custom Fields to Loan Fields (Optional)";
      this.gcCustomFieldDefinitions.Visible = false;
      this.borderPanel3.Borders = AnchorStyles.Top;
      this.borderPanel3.Controls.Add((Control) this.pnlExRightMapping);
      this.borderPanel3.Dock = DockStyle.Fill;
      this.borderPanel3.Location = new Point(1, 56);
      this.borderPanel3.Name = "borderPanel3";
      this.borderPanel3.Size = new Size(336, 476);
      this.borderPanel3.TabIndex = 437;
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch20);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch19);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch18);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch17);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch16);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch15);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch14);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch13);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch12);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch11);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch10);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch9);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch8);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch7);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch6);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch5);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch4);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch3);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch2);
      this.pnlExRightMapping.Controls.Add((Control) this.picLoanFieldSearch1);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer20);
      this.pnlExRightMapping.Controls.Add((Control) this.lblLoanFieldId);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription10);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId4);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription4);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId13);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription9);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription19);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer15);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer6);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer14);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer5);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer19);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription11);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId14);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription8);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId12);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId5);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription20);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer4);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId15);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription7);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription18);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId3);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer7);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer13);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer3);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId1);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription12);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId16);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription6);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId11);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId6);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer2);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId17);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription5);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription17);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer16);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer8);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer12);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer1);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer18);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription13);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId18);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId10);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId7);
      this.pnlExRightMapping.Controls.Add((Control) this.lblDirection);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription3);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId19);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription16);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId2);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer9);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer11);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription2);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId20);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription14);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId9);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldId8);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription15);
      this.pnlExRightMapping.Controls.Add((Control) this.txtLoanFieldDescription1);
      this.pnlExRightMapping.Controls.Add((Control) this.lblLoanFieldDescription);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer10);
      this.pnlExRightMapping.Controls.Add((Control) this.chkTwoWayTransfer17);
      this.pnlExRightMapping.Dock = DockStyle.Fill;
      this.pnlExRightMapping.Location = new Point(0, 1);
      this.pnlExRightMapping.Name = "pnlExRightMapping";
      this.pnlExRightMapping.Size = new Size(336, 475);
      this.pnlExRightMapping.TabIndex = 435;
      this.picLoanFieldSearch20.BackColor = Color.Transparent;
      this.picLoanFieldSearch20.Location = new Point(113, 443);
      this.picLoanFieldSearch20.MouseDownImage = (Image) null;
      this.picLoanFieldSearch20.Name = "picLoanFieldSearch20";
      this.picLoanFieldSearch20.Size = new Size(16, 16);
      this.picLoanFieldSearch20.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch20.TabIndex = 454;
      this.picLoanFieldSearch20.TabStop = false;
      this.picLoanFieldSearch20.Tag = (object) "20";
      this.picLoanFieldSearch20.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch19.BackColor = Color.Transparent;
      this.picLoanFieldSearch19.Location = new Point(113, 421);
      this.picLoanFieldSearch19.MouseDownImage = (Image) null;
      this.picLoanFieldSearch19.Name = "picLoanFieldSearch19";
      this.picLoanFieldSearch19.Size = new Size(16, 16);
      this.picLoanFieldSearch19.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch19.TabIndex = 453;
      this.picLoanFieldSearch19.TabStop = false;
      this.picLoanFieldSearch19.Tag = (object) "19";
      this.picLoanFieldSearch19.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch18.BackColor = Color.Transparent;
      this.picLoanFieldSearch18.Location = new Point(113, 397);
      this.picLoanFieldSearch18.MouseDownImage = (Image) null;
      this.picLoanFieldSearch18.Name = "picLoanFieldSearch18";
      this.picLoanFieldSearch18.Size = new Size(16, 16);
      this.picLoanFieldSearch18.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch18.TabIndex = 452;
      this.picLoanFieldSearch18.TabStop = false;
      this.picLoanFieldSearch18.Tag = (object) "18";
      this.picLoanFieldSearch18.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch17.BackColor = Color.Transparent;
      this.picLoanFieldSearch17.Location = new Point(113, 375);
      this.picLoanFieldSearch17.MouseDownImage = (Image) null;
      this.picLoanFieldSearch17.Name = "picLoanFieldSearch17";
      this.picLoanFieldSearch17.Size = new Size(16, 16);
      this.picLoanFieldSearch17.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch17.TabIndex = 451;
      this.picLoanFieldSearch17.TabStop = false;
      this.picLoanFieldSearch17.Tag = (object) "17";
      this.picLoanFieldSearch17.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch16.BackColor = Color.Transparent;
      this.picLoanFieldSearch16.Location = new Point(113, 354);
      this.picLoanFieldSearch16.MouseDownImage = (Image) null;
      this.picLoanFieldSearch16.Name = "picLoanFieldSearch16";
      this.picLoanFieldSearch16.Size = new Size(16, 16);
      this.picLoanFieldSearch16.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch16.TabIndex = 450;
      this.picLoanFieldSearch16.TabStop = false;
      this.picLoanFieldSearch16.Tag = (object) "16";
      this.picLoanFieldSearch16.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch15.BackColor = Color.Transparent;
      this.picLoanFieldSearch15.Location = new Point(113, 333);
      this.picLoanFieldSearch15.MouseDownImage = (Image) null;
      this.picLoanFieldSearch15.Name = "picLoanFieldSearch15";
      this.picLoanFieldSearch15.Size = new Size(16, 16);
      this.picLoanFieldSearch15.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch15.TabIndex = 449;
      this.picLoanFieldSearch15.TabStop = false;
      this.picLoanFieldSearch15.Tag = (object) "15";
      this.picLoanFieldSearch15.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch14.BackColor = Color.Transparent;
      this.picLoanFieldSearch14.Location = new Point(113, 309);
      this.picLoanFieldSearch14.MouseDownImage = (Image) null;
      this.picLoanFieldSearch14.Name = "picLoanFieldSearch14";
      this.picLoanFieldSearch14.Size = new Size(16, 16);
      this.picLoanFieldSearch14.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch14.TabIndex = 448;
      this.picLoanFieldSearch14.TabStop = false;
      this.picLoanFieldSearch14.Tag = (object) "14";
      this.picLoanFieldSearch14.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch13.BackColor = Color.Transparent;
      this.picLoanFieldSearch13.Location = new Point(113, 289);
      this.picLoanFieldSearch13.MouseDownImage = (Image) null;
      this.picLoanFieldSearch13.Name = "picLoanFieldSearch13";
      this.picLoanFieldSearch13.Size = new Size(16, 16);
      this.picLoanFieldSearch13.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch13.TabIndex = 447;
      this.picLoanFieldSearch13.TabStop = false;
      this.picLoanFieldSearch13.Tag = (object) "13";
      this.picLoanFieldSearch13.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch12.BackColor = Color.Transparent;
      this.picLoanFieldSearch12.Location = new Point(113, 266);
      this.picLoanFieldSearch12.MouseDownImage = (Image) null;
      this.picLoanFieldSearch12.Name = "picLoanFieldSearch12";
      this.picLoanFieldSearch12.Size = new Size(16, 16);
      this.picLoanFieldSearch12.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch12.TabIndex = 446;
      this.picLoanFieldSearch12.TabStop = false;
      this.picLoanFieldSearch12.Tag = (object) "12";
      this.picLoanFieldSearch12.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch11.BackColor = Color.Transparent;
      this.picLoanFieldSearch11.Location = new Point(113, 244);
      this.picLoanFieldSearch11.MouseDownImage = (Image) null;
      this.picLoanFieldSearch11.Name = "picLoanFieldSearch11";
      this.picLoanFieldSearch11.Size = new Size(16, 16);
      this.picLoanFieldSearch11.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch11.TabIndex = 445;
      this.picLoanFieldSearch11.TabStop = false;
      this.picLoanFieldSearch11.Tag = (object) "11";
      this.picLoanFieldSearch11.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch10.BackColor = Color.Transparent;
      this.picLoanFieldSearch10.Location = new Point(113, 221);
      this.picLoanFieldSearch10.MouseDownImage = (Image) null;
      this.picLoanFieldSearch10.Name = "picLoanFieldSearch10";
      this.picLoanFieldSearch10.Size = new Size(16, 16);
      this.picLoanFieldSearch10.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch10.TabIndex = 444;
      this.picLoanFieldSearch10.TabStop = false;
      this.picLoanFieldSearch10.Tag = (object) "10";
      this.picLoanFieldSearch10.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch9.BackColor = Color.Transparent;
      this.picLoanFieldSearch9.Location = new Point(113, 201);
      this.picLoanFieldSearch9.MouseDownImage = (Image) null;
      this.picLoanFieldSearch9.Name = "picLoanFieldSearch9";
      this.picLoanFieldSearch9.Size = new Size(16, 16);
      this.picLoanFieldSearch9.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch9.TabIndex = 443;
      this.picLoanFieldSearch9.TabStop = false;
      this.picLoanFieldSearch9.Tag = (object) "9";
      this.picLoanFieldSearch9.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch8.BackColor = Color.Transparent;
      this.picLoanFieldSearch8.Location = new Point(113, 177);
      this.picLoanFieldSearch8.MouseDownImage = (Image) null;
      this.picLoanFieldSearch8.Name = "picLoanFieldSearch8";
      this.picLoanFieldSearch8.Size = new Size(16, 16);
      this.picLoanFieldSearch8.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch8.TabIndex = 442;
      this.picLoanFieldSearch8.TabStop = false;
      this.picLoanFieldSearch8.Tag = (object) "8";
      this.picLoanFieldSearch8.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch7.BackColor = Color.Transparent;
      this.picLoanFieldSearch7.Location = new Point(113, 155);
      this.picLoanFieldSearch7.MouseDownImage = (Image) null;
      this.picLoanFieldSearch7.Name = "picLoanFieldSearch7";
      this.picLoanFieldSearch7.Size = new Size(16, 16);
      this.picLoanFieldSearch7.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch7.TabIndex = 441;
      this.picLoanFieldSearch7.TabStop = false;
      this.picLoanFieldSearch7.Tag = (object) "7";
      this.picLoanFieldSearch7.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch6.BackColor = Color.Transparent;
      this.picLoanFieldSearch6.Location = new Point(113, 134);
      this.picLoanFieldSearch6.MouseDownImage = (Image) null;
      this.picLoanFieldSearch6.Name = "picLoanFieldSearch6";
      this.picLoanFieldSearch6.Size = new Size(16, 16);
      this.picLoanFieldSearch6.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch6.TabIndex = 440;
      this.picLoanFieldSearch6.TabStop = false;
      this.picLoanFieldSearch6.Tag = (object) "6";
      this.picLoanFieldSearch6.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch5.BackColor = Color.Transparent;
      this.picLoanFieldSearch5.Location = new Point(113, 113);
      this.picLoanFieldSearch5.MouseDownImage = (Image) null;
      this.picLoanFieldSearch5.Name = "picLoanFieldSearch5";
      this.picLoanFieldSearch5.Size = new Size(16, 16);
      this.picLoanFieldSearch5.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch5.TabIndex = 439;
      this.picLoanFieldSearch5.TabStop = false;
      this.picLoanFieldSearch5.Tag = (object) "5";
      this.picLoanFieldSearch5.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch4.BackColor = Color.Transparent;
      this.picLoanFieldSearch4.Location = new Point(113, 91);
      this.picLoanFieldSearch4.MouseDownImage = (Image) null;
      this.picLoanFieldSearch4.Name = "picLoanFieldSearch4";
      this.picLoanFieldSearch4.Size = new Size(16, 16);
      this.picLoanFieldSearch4.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch4.TabIndex = 438;
      this.picLoanFieldSearch4.TabStop = false;
      this.picLoanFieldSearch4.Tag = (object) "4";
      this.picLoanFieldSearch4.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch3.BackColor = Color.Transparent;
      this.picLoanFieldSearch3.Location = new Point(113, 68);
      this.picLoanFieldSearch3.MouseDownImage = (Image) null;
      this.picLoanFieldSearch3.Name = "picLoanFieldSearch3";
      this.picLoanFieldSearch3.Size = new Size(16, 16);
      this.picLoanFieldSearch3.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch3.TabIndex = 437;
      this.picLoanFieldSearch3.TabStop = false;
      this.picLoanFieldSearch3.Tag = (object) "3";
      this.picLoanFieldSearch3.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch2.BackColor = Color.Transparent;
      this.picLoanFieldSearch2.Location = new Point(113, 46);
      this.picLoanFieldSearch2.MouseDownImage = (Image) null;
      this.picLoanFieldSearch2.Name = "picLoanFieldSearch2";
      this.picLoanFieldSearch2.Size = new Size(16, 16);
      this.picLoanFieldSearch2.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch2.TabIndex = 436;
      this.picLoanFieldSearch2.TabStop = false;
      this.picLoanFieldSearch2.Tag = (object) "2";
      this.picLoanFieldSearch2.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.picLoanFieldSearch1.BackColor = Color.Transparent;
      this.picLoanFieldSearch1.Location = new Point(113, 25);
      this.picLoanFieldSearch1.MouseDownImage = (Image) null;
      this.picLoanFieldSearch1.Name = "picLoanFieldSearch1";
      this.picLoanFieldSearch1.Size = new Size(16, 16);
      this.picLoanFieldSearch1.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.picLoanFieldSearch1.TabIndex = 435;
      this.picLoanFieldSearch1.TabStop = false;
      this.picLoanFieldSearch1.Tag = (object) "1";
      this.picLoanFieldSearch1.Click += new EventHandler(this.picLoanFieldSearch_Click);
      this.chkTwoWayTransfer20.AutoSize = true;
      this.chkTwoWayTransfer20.Location = new Point(286, 445);
      this.chkTwoWayTransfer20.Name = "chkTwoWayTransfer20";
      this.chkTwoWayTransfer20.Size = new Size(15, 14);
      this.chkTwoWayTransfer20.TabIndex = 414;
      this.chkTwoWayTransfer20.Tag = (object) "20";
      this.chkTwoWayTransfer20.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer20.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer20.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.lblLoanFieldId.Location = new Point(9, 6);
      this.lblLoanFieldId.Name = "lblLoanFieldId";
      this.lblLoanFieldId.Size = new Size(95, 13);
      this.lblLoanFieldId.TabIndex = 352;
      this.lblLoanFieldId.Text = "Loan Field ID";
      this.lblLoanFieldId.TextAlign = ContentAlignment.MiddleLeft;
      this.txtLoanFieldDescription10.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription10.Location = new Point(135, 222);
      this.txtLoanFieldDescription10.MaxLength = 50;
      this.txtLoanFieldDescription10.Name = "txtLoanFieldDescription10";
      this.txtLoanFieldDescription10.ReadOnly = true;
      this.txtLoanFieldDescription10.Size = new Size(128, 20);
      this.txtLoanFieldDescription10.TabIndex = 383;
      this.txtLoanFieldDescription10.TabStop = false;
      this.txtLoanFieldDescription10.Tag = (object) "10";
      this.txtLoanFieldId4.BackColor = SystemColors.Window;
      this.txtLoanFieldId4.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId4.Location = new Point(12, 90);
      this.txtLoanFieldId4.MaxLength = 30;
      this.txtLoanFieldId4.Name = "txtLoanFieldId4";
      this.txtLoanFieldId4.Size = new Size(95, 20);
      this.txtLoanFieldId4.TabIndex = 364;
      this.txtLoanFieldId4.Tag = (object) "4";
      this.txtLoanFieldId4.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId4.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId4.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription4.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription4.Location = new Point(135, 90);
      this.txtLoanFieldDescription4.MaxLength = 50;
      this.txtLoanFieldDescription4.Name = "txtLoanFieldDescription4";
      this.txtLoanFieldDescription4.ReadOnly = true;
      this.txtLoanFieldDescription4.Size = new Size(128, 20);
      this.txtLoanFieldDescription4.TabIndex = 365;
      this.txtLoanFieldDescription4.TabStop = false;
      this.txtLoanFieldDescription4.Tag = (object) "4";
      this.txtLoanFieldId13.BackColor = SystemColors.Window;
      this.txtLoanFieldId13.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId13.Location = new Point(12, 288);
      this.txtLoanFieldId13.MaxLength = 30;
      this.txtLoanFieldId13.Name = "txtLoanFieldId13";
      this.txtLoanFieldId13.Size = new Size(95, 20);
      this.txtLoanFieldId13.TabIndex = 391;
      this.txtLoanFieldId13.Tag = (object) "13";
      this.txtLoanFieldId13.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId13.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId13.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription9.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription9.Location = new Point(135, 200);
      this.txtLoanFieldDescription9.MaxLength = 50;
      this.txtLoanFieldDescription9.Name = "txtLoanFieldDescription9";
      this.txtLoanFieldDescription9.ReadOnly = true;
      this.txtLoanFieldDescription9.Size = new Size(128, 20);
      this.txtLoanFieldDescription9.TabIndex = 380;
      this.txtLoanFieldDescription9.TabStop = false;
      this.txtLoanFieldDescription9.Tag = (object) "9";
      this.txtLoanFieldDescription19.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription19.Location = new Point(135, 420);
      this.txtLoanFieldDescription19.MaxLength = 50;
      this.txtLoanFieldDescription19.Name = "txtLoanFieldDescription19";
      this.txtLoanFieldDescription19.ReadOnly = true;
      this.txtLoanFieldDescription19.Size = new Size(128, 20);
      this.txtLoanFieldDescription19.TabIndex = 410;
      this.txtLoanFieldDescription19.TabStop = false;
      this.txtLoanFieldDescription19.Tag = (object) "19";
      this.chkTwoWayTransfer15.AutoSize = true;
      this.chkTwoWayTransfer15.Location = new Point(286, 335);
      this.chkTwoWayTransfer15.Name = "chkTwoWayTransfer15";
      this.chkTwoWayTransfer15.Size = new Size(15, 14);
      this.chkTwoWayTransfer15.TabIndex = 399;
      this.chkTwoWayTransfer15.Tag = (object) "15";
      this.chkTwoWayTransfer15.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer15.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer15.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer6.AutoSize = true;
      this.chkTwoWayTransfer6.Location = new Point(286, 137);
      this.chkTwoWayTransfer6.Name = "chkTwoWayTransfer6";
      this.chkTwoWayTransfer6.Size = new Size(15, 14);
      this.chkTwoWayTransfer6.TabIndex = 372;
      this.chkTwoWayTransfer6.Tag = (object) "6";
      this.chkTwoWayTransfer6.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer6.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer6.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer14.AutoSize = true;
      this.chkTwoWayTransfer14.Location = new Point(286, 313);
      this.chkTwoWayTransfer14.Name = "chkTwoWayTransfer14";
      this.chkTwoWayTransfer14.Size = new Size(15, 14);
      this.chkTwoWayTransfer14.TabIndex = 396;
      this.chkTwoWayTransfer14.Tag = (object) "14";
      this.chkTwoWayTransfer14.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer14.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer14.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer5.AutoSize = true;
      this.chkTwoWayTransfer5.Location = new Point(286, 115);
      this.chkTwoWayTransfer5.Name = "chkTwoWayTransfer5";
      this.chkTwoWayTransfer5.Size = new Size(15, 14);
      this.chkTwoWayTransfer5.TabIndex = 369;
      this.chkTwoWayTransfer5.Tag = (object) "5";
      this.chkTwoWayTransfer5.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer5.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer5.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer19.AutoSize = true;
      this.chkTwoWayTransfer19.Location = new Point(286, 423);
      this.chkTwoWayTransfer19.Name = "chkTwoWayTransfer19";
      this.chkTwoWayTransfer19.Size = new Size(15, 14);
      this.chkTwoWayTransfer19.TabIndex = 411;
      this.chkTwoWayTransfer19.Tag = (object) "19";
      this.chkTwoWayTransfer19.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer19.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer19.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldDescription11.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription11.Location = new Point(135, 244);
      this.txtLoanFieldDescription11.MaxLength = 50;
      this.txtLoanFieldDescription11.Name = "txtLoanFieldDescription11";
      this.txtLoanFieldDescription11.ReadOnly = true;
      this.txtLoanFieldDescription11.Size = new Size(128, 20);
      this.txtLoanFieldDescription11.TabIndex = 386;
      this.txtLoanFieldDescription11.TabStop = false;
      this.txtLoanFieldDescription11.Tag = (object) "11";
      this.txtLoanFieldId14.BackColor = SystemColors.Window;
      this.txtLoanFieldId14.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId14.Location = new Point(12, 310);
      this.txtLoanFieldId14.MaxLength = 30;
      this.txtLoanFieldId14.Name = "txtLoanFieldId14";
      this.txtLoanFieldId14.Size = new Size(95, 20);
      this.txtLoanFieldId14.TabIndex = 394;
      this.txtLoanFieldId14.Tag = (object) "14";
      this.txtLoanFieldId14.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId14.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId14.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription8.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription8.Location = new Point(135, 178);
      this.txtLoanFieldDescription8.MaxLength = 50;
      this.txtLoanFieldDescription8.Name = "txtLoanFieldDescription8";
      this.txtLoanFieldDescription8.ReadOnly = true;
      this.txtLoanFieldDescription8.Size = new Size(128, 20);
      this.txtLoanFieldDescription8.TabIndex = 377;
      this.txtLoanFieldDescription8.TabStop = false;
      this.txtLoanFieldDescription8.Tag = (object) "8";
      this.txtLoanFieldId12.BackColor = SystemColors.Window;
      this.txtLoanFieldId12.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId12.Location = new Point(12, 266);
      this.txtLoanFieldId12.MaxLength = 30;
      this.txtLoanFieldId12.Name = "txtLoanFieldId12";
      this.txtLoanFieldId12.Size = new Size(95, 20);
      this.txtLoanFieldId12.TabIndex = 388;
      this.txtLoanFieldId12.Tag = (object) "12";
      this.txtLoanFieldId12.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId12.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId12.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldId5.BackColor = SystemColors.Window;
      this.txtLoanFieldId5.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId5.Location = new Point(12, 112);
      this.txtLoanFieldId5.MaxLength = 30;
      this.txtLoanFieldId5.Name = "txtLoanFieldId5";
      this.txtLoanFieldId5.Size = new Size(95, 20);
      this.txtLoanFieldId5.TabIndex = 367;
      this.txtLoanFieldId5.Tag = (object) "5";
      this.txtLoanFieldId5.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId5.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId5.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription20.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription20.Location = new Point(135, 442);
      this.txtLoanFieldDescription20.MaxLength = 50;
      this.txtLoanFieldDescription20.Name = "txtLoanFieldDescription20";
      this.txtLoanFieldDescription20.ReadOnly = true;
      this.txtLoanFieldDescription20.Size = new Size(128, 20);
      this.txtLoanFieldDescription20.TabIndex = 413;
      this.txtLoanFieldDescription20.TabStop = false;
      this.txtLoanFieldDescription20.Tag = (object) "20";
      this.chkTwoWayTransfer4.AutoSize = true;
      this.chkTwoWayTransfer4.Location = new Point(286, 93);
      this.chkTwoWayTransfer4.Name = "chkTwoWayTransfer4";
      this.chkTwoWayTransfer4.Size = new Size(15, 14);
      this.chkTwoWayTransfer4.TabIndex = 366;
      this.chkTwoWayTransfer4.Tag = (object) "4";
      this.chkTwoWayTransfer4.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer4.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer4.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldId15.BackColor = SystemColors.Window;
      this.txtLoanFieldId15.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId15.Location = new Point(12, 332);
      this.txtLoanFieldId15.MaxLength = 30;
      this.txtLoanFieldId15.Name = "txtLoanFieldId15";
      this.txtLoanFieldId15.Size = new Size(95, 20);
      this.txtLoanFieldId15.TabIndex = 397;
      this.txtLoanFieldId15.Tag = (object) "15";
      this.txtLoanFieldId15.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId15.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId15.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription7.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription7.Location = new Point(135, 156);
      this.txtLoanFieldDescription7.MaxLength = 50;
      this.txtLoanFieldDescription7.Name = "txtLoanFieldDescription7";
      this.txtLoanFieldDescription7.ReadOnly = true;
      this.txtLoanFieldDescription7.Size = new Size(128, 20);
      this.txtLoanFieldDescription7.TabIndex = 374;
      this.txtLoanFieldDescription7.TabStop = false;
      this.txtLoanFieldDescription7.Tag = (object) "7";
      this.txtLoanFieldDescription18.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription18.Location = new Point(135, 398);
      this.txtLoanFieldDescription18.MaxLength = 50;
      this.txtLoanFieldDescription18.Name = "txtLoanFieldDescription18";
      this.txtLoanFieldDescription18.ReadOnly = true;
      this.txtLoanFieldDescription18.Size = new Size(128, 20);
      this.txtLoanFieldDescription18.TabIndex = 407;
      this.txtLoanFieldDescription18.TabStop = false;
      this.txtLoanFieldDescription18.Tag = (object) "18";
      this.txtLoanFieldId3.BackColor = SystemColors.Window;
      this.txtLoanFieldId3.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId3.Location = new Point(12, 68);
      this.txtLoanFieldId3.MaxLength = 30;
      this.txtLoanFieldId3.Name = "txtLoanFieldId3";
      this.txtLoanFieldId3.Size = new Size(95, 20);
      this.txtLoanFieldId3.TabIndex = 361;
      this.txtLoanFieldId3.Tag = (object) "3";
      this.txtLoanFieldId3.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId3.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId3.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.chkTwoWayTransfer7.AutoSize = true;
      this.chkTwoWayTransfer7.Location = new Point(286, 159);
      this.chkTwoWayTransfer7.Name = "chkTwoWayTransfer7";
      this.chkTwoWayTransfer7.Size = new Size(15, 14);
      this.chkTwoWayTransfer7.TabIndex = 375;
      this.chkTwoWayTransfer7.Tag = (object) "7";
      this.chkTwoWayTransfer7.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer7.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer7.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer13.AutoSize = true;
      this.chkTwoWayTransfer13.Location = new Point(286, 291);
      this.chkTwoWayTransfer13.Name = "chkTwoWayTransfer13";
      this.chkTwoWayTransfer13.Size = new Size(15, 14);
      this.chkTwoWayTransfer13.TabIndex = 393;
      this.chkTwoWayTransfer13.Tag = (object) "13";
      this.chkTwoWayTransfer13.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer13.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer13.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer3.AutoSize = true;
      this.chkTwoWayTransfer3.Location = new Point(286, 71);
      this.chkTwoWayTransfer3.Name = "chkTwoWayTransfer3";
      this.chkTwoWayTransfer3.Size = new Size(15, 14);
      this.chkTwoWayTransfer3.TabIndex = 363;
      this.chkTwoWayTransfer3.Tag = (object) "3";
      this.chkTwoWayTransfer3.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer3.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer3.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldId1.BackColor = SystemColors.Window;
      this.txtLoanFieldId1.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId1.Location = new Point(12, 24);
      this.txtLoanFieldId1.MaxLength = 30;
      this.txtLoanFieldId1.Name = "txtLoanFieldId1";
      this.txtLoanFieldId1.Size = new Size(95, 20);
      this.txtLoanFieldId1.TabIndex = 355;
      this.txtLoanFieldId1.Tag = (object) "1";
      this.txtLoanFieldId1.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId1.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId1.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription12.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription12.Location = new Point(135, 266);
      this.txtLoanFieldDescription12.MaxLength = 50;
      this.txtLoanFieldDescription12.Name = "txtLoanFieldDescription12";
      this.txtLoanFieldDescription12.ReadOnly = true;
      this.txtLoanFieldDescription12.Size = new Size(128, 20);
      this.txtLoanFieldDescription12.TabIndex = 389;
      this.txtLoanFieldDescription12.TabStop = false;
      this.txtLoanFieldDescription12.Tag = (object) "12";
      this.txtLoanFieldId16.BackColor = SystemColors.Window;
      this.txtLoanFieldId16.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId16.Location = new Point(12, 354);
      this.txtLoanFieldId16.MaxLength = 30;
      this.txtLoanFieldId16.Name = "txtLoanFieldId16";
      this.txtLoanFieldId16.Size = new Size(95, 20);
      this.txtLoanFieldId16.TabIndex = 400;
      this.txtLoanFieldId16.Tag = (object) "16";
      this.txtLoanFieldId16.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId16.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId16.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription6.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription6.Location = new Point(135, 134);
      this.txtLoanFieldDescription6.MaxLength = 50;
      this.txtLoanFieldDescription6.Name = "txtLoanFieldDescription6";
      this.txtLoanFieldDescription6.ReadOnly = true;
      this.txtLoanFieldDescription6.Size = new Size(128, 20);
      this.txtLoanFieldDescription6.TabIndex = 371;
      this.txtLoanFieldDescription6.TabStop = false;
      this.txtLoanFieldDescription6.Tag = (object) "6";
      this.txtLoanFieldId11.BackColor = SystemColors.Window;
      this.txtLoanFieldId11.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId11.Location = new Point(12, 244);
      this.txtLoanFieldId11.MaxLength = 30;
      this.txtLoanFieldId11.Name = "txtLoanFieldId11";
      this.txtLoanFieldId11.Size = new Size(95, 20);
      this.txtLoanFieldId11.TabIndex = 385;
      this.txtLoanFieldId11.Tag = (object) "11";
      this.txtLoanFieldId11.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId11.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId11.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldId6.BackColor = SystemColors.Window;
      this.txtLoanFieldId6.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId6.Location = new Point(12, 134);
      this.txtLoanFieldId6.MaxLength = 30;
      this.txtLoanFieldId6.Name = "txtLoanFieldId6";
      this.txtLoanFieldId6.Size = new Size(95, 20);
      this.txtLoanFieldId6.TabIndex = 370;
      this.txtLoanFieldId6.Tag = (object) "6";
      this.txtLoanFieldId6.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId6.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId6.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.chkTwoWayTransfer2.AutoSize = true;
      this.chkTwoWayTransfer2.Location = new Point(286, 49);
      this.chkTwoWayTransfer2.Name = "chkTwoWayTransfer2";
      this.chkTwoWayTransfer2.Size = new Size(15, 14);
      this.chkTwoWayTransfer2.TabIndex = 360;
      this.chkTwoWayTransfer2.Tag = (object) "2";
      this.chkTwoWayTransfer2.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer2.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer2.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldId17.BackColor = SystemColors.Window;
      this.txtLoanFieldId17.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId17.Location = new Point(12, 376);
      this.txtLoanFieldId17.MaxLength = 30;
      this.txtLoanFieldId17.Name = "txtLoanFieldId17";
      this.txtLoanFieldId17.Size = new Size(95, 20);
      this.txtLoanFieldId17.TabIndex = 403;
      this.txtLoanFieldId17.Tag = (object) "17";
      this.txtLoanFieldId17.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId17.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId17.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription5.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription5.Location = new Point(135, 112);
      this.txtLoanFieldDescription5.MaxLength = 50;
      this.txtLoanFieldDescription5.Name = "txtLoanFieldDescription5";
      this.txtLoanFieldDescription5.ReadOnly = true;
      this.txtLoanFieldDescription5.Size = new Size(128, 20);
      this.txtLoanFieldDescription5.TabIndex = 368;
      this.txtLoanFieldDescription5.TabStop = false;
      this.txtLoanFieldDescription5.Tag = (object) "5";
      this.txtLoanFieldDescription17.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription17.Location = new Point(135, 376);
      this.txtLoanFieldDescription17.MaxLength = 50;
      this.txtLoanFieldDescription17.Name = "txtLoanFieldDescription17";
      this.txtLoanFieldDescription17.ReadOnly = true;
      this.txtLoanFieldDescription17.Size = new Size(128, 20);
      this.txtLoanFieldDescription17.TabIndex = 404;
      this.txtLoanFieldDescription17.TabStop = false;
      this.txtLoanFieldDescription17.Tag = (object) "17";
      this.chkTwoWayTransfer16.AutoSize = true;
      this.chkTwoWayTransfer16.Location = new Point(286, 357);
      this.chkTwoWayTransfer16.Name = "chkTwoWayTransfer16";
      this.chkTwoWayTransfer16.Size = new Size(15, 14);
      this.chkTwoWayTransfer16.TabIndex = 402;
      this.chkTwoWayTransfer16.Tag = (object) "16";
      this.chkTwoWayTransfer16.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer16.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer16.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer8.AutoSize = true;
      this.chkTwoWayTransfer8.Location = new Point(286, 181);
      this.chkTwoWayTransfer8.Name = "chkTwoWayTransfer8";
      this.chkTwoWayTransfer8.Size = new Size(15, 14);
      this.chkTwoWayTransfer8.TabIndex = 378;
      this.chkTwoWayTransfer8.Tag = (object) "8";
      this.chkTwoWayTransfer8.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer8.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer8.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer12.AutoSize = true;
      this.chkTwoWayTransfer12.Location = new Point(286, 269);
      this.chkTwoWayTransfer12.Name = "chkTwoWayTransfer12";
      this.chkTwoWayTransfer12.Size = new Size(15, 14);
      this.chkTwoWayTransfer12.TabIndex = 390;
      this.chkTwoWayTransfer12.Tag = (object) "12";
      this.chkTwoWayTransfer12.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer12.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer12.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer1.AutoSize = true;
      this.chkTwoWayTransfer1.Location = new Point(286, 27);
      this.chkTwoWayTransfer1.Name = "chkTwoWayTransfer1";
      this.chkTwoWayTransfer1.Size = new Size(15, 14);
      this.chkTwoWayTransfer1.TabIndex = 357;
      this.chkTwoWayTransfer1.Tag = (object) "1";
      this.chkTwoWayTransfer1.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer1.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer1.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer18.AutoSize = true;
      this.chkTwoWayTransfer18.Location = new Point(286, 401);
      this.chkTwoWayTransfer18.Name = "chkTwoWayTransfer18";
      this.chkTwoWayTransfer18.Size = new Size(15, 14);
      this.chkTwoWayTransfer18.TabIndex = 408;
      this.chkTwoWayTransfer18.Tag = (object) "18";
      this.chkTwoWayTransfer18.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer18.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer18.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldDescription13.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription13.Location = new Point(135, 288);
      this.txtLoanFieldDescription13.MaxLength = 50;
      this.txtLoanFieldDescription13.Name = "txtLoanFieldDescription13";
      this.txtLoanFieldDescription13.ReadOnly = true;
      this.txtLoanFieldDescription13.Size = new Size(128, 20);
      this.txtLoanFieldDescription13.TabIndex = 392;
      this.txtLoanFieldDescription13.TabStop = false;
      this.txtLoanFieldDescription13.Tag = (object) "13";
      this.txtLoanFieldId18.BackColor = SystemColors.Window;
      this.txtLoanFieldId18.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId18.Location = new Point(12, 398);
      this.txtLoanFieldId18.MaxLength = 30;
      this.txtLoanFieldId18.Name = "txtLoanFieldId18";
      this.txtLoanFieldId18.Size = new Size(95, 20);
      this.txtLoanFieldId18.TabIndex = 406;
      this.txtLoanFieldId18.Tag = (object) "18";
      this.txtLoanFieldId18.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId18.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId18.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldId10.BackColor = SystemColors.Window;
      this.txtLoanFieldId10.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId10.Location = new Point(12, 222);
      this.txtLoanFieldId10.MaxLength = 30;
      this.txtLoanFieldId10.Name = "txtLoanFieldId10";
      this.txtLoanFieldId10.Size = new Size(95, 20);
      this.txtLoanFieldId10.TabIndex = 382;
      this.txtLoanFieldId10.Tag = (object) "10";
      this.txtLoanFieldId10.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId10.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId10.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldId7.BackColor = SystemColors.Window;
      this.txtLoanFieldId7.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId7.Location = new Point(12, 156);
      this.txtLoanFieldId7.MaxLength = 30;
      this.txtLoanFieldId7.Name = "txtLoanFieldId7";
      this.txtLoanFieldId7.Size = new Size(95, 20);
      this.txtLoanFieldId7.TabIndex = 373;
      this.txtLoanFieldId7.Tag = (object) "7";
      this.txtLoanFieldId7.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId7.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId7.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.lblDirection.Location = new Point(269, 6);
      this.lblDirection.Name = "lblDirection";
      this.lblDirection.Size = new Size(59, 13);
      this.lblDirection.TabIndex = 354;
      this.lblDirection.Text = "Both Ways";
      this.lblDirection.TextAlign = ContentAlignment.MiddleCenter;
      this.txtLoanFieldDescription3.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription3.Location = new Point(135, 68);
      this.txtLoanFieldDescription3.MaxLength = 50;
      this.txtLoanFieldDescription3.Name = "txtLoanFieldDescription3";
      this.txtLoanFieldDescription3.ReadOnly = true;
      this.txtLoanFieldDescription3.Size = new Size(128, 20);
      this.txtLoanFieldDescription3.TabIndex = 362;
      this.txtLoanFieldDescription3.TabStop = false;
      this.txtLoanFieldDescription3.Tag = (object) "3";
      this.txtLoanFieldId19.BackColor = SystemColors.Window;
      this.txtLoanFieldId19.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId19.Location = new Point(12, 420);
      this.txtLoanFieldId19.MaxLength = 30;
      this.txtLoanFieldId19.Name = "txtLoanFieldId19";
      this.txtLoanFieldId19.Size = new Size(95, 20);
      this.txtLoanFieldId19.TabIndex = 409;
      this.txtLoanFieldId19.Tag = (object) "19";
      this.txtLoanFieldId19.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId19.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId19.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription16.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription16.Location = new Point(135, 354);
      this.txtLoanFieldDescription16.MaxLength = 50;
      this.txtLoanFieldDescription16.Name = "txtLoanFieldDescription16";
      this.txtLoanFieldDescription16.ReadOnly = true;
      this.txtLoanFieldDescription16.Size = new Size(128, 20);
      this.txtLoanFieldDescription16.TabIndex = 401;
      this.txtLoanFieldDescription16.TabStop = false;
      this.txtLoanFieldDescription16.Tag = (object) "16";
      this.txtLoanFieldId2.BackColor = SystemColors.Window;
      this.txtLoanFieldId2.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId2.Location = new Point(12, 46);
      this.txtLoanFieldId2.MaxLength = 30;
      this.txtLoanFieldId2.Name = "txtLoanFieldId2";
      this.txtLoanFieldId2.Size = new Size(95, 20);
      this.txtLoanFieldId2.TabIndex = 358;
      this.txtLoanFieldId2.Tag = (object) "2";
      this.txtLoanFieldId2.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId2.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId2.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.chkTwoWayTransfer9.AutoSize = true;
      this.chkTwoWayTransfer9.Location = new Point(286, 203);
      this.chkTwoWayTransfer9.Name = "chkTwoWayTransfer9";
      this.chkTwoWayTransfer9.Size = new Size(15, 14);
      this.chkTwoWayTransfer9.TabIndex = 381;
      this.chkTwoWayTransfer9.Tag = (object) "9";
      this.chkTwoWayTransfer9.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer9.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer9.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer11.AutoSize = true;
      this.chkTwoWayTransfer11.Location = new Point(286, 247);
      this.chkTwoWayTransfer11.Name = "chkTwoWayTransfer11";
      this.chkTwoWayTransfer11.Size = new Size(15, 14);
      this.chkTwoWayTransfer11.TabIndex = 387;
      this.chkTwoWayTransfer11.Tag = (object) "11";
      this.chkTwoWayTransfer11.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer11.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer11.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.txtLoanFieldDescription2.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription2.Location = new Point(135, 46);
      this.txtLoanFieldDescription2.MaxLength = 50;
      this.txtLoanFieldDescription2.Name = "txtLoanFieldDescription2";
      this.txtLoanFieldDescription2.ReadOnly = true;
      this.txtLoanFieldDescription2.Size = new Size(128, 20);
      this.txtLoanFieldDescription2.TabIndex = 359;
      this.txtLoanFieldDescription2.TabStop = false;
      this.txtLoanFieldDescription2.Tag = (object) "2";
      this.txtLoanFieldId20.BackColor = SystemColors.Window;
      this.txtLoanFieldId20.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId20.Location = new Point(12, 442);
      this.txtLoanFieldId20.MaxLength = 30;
      this.txtLoanFieldId20.Name = "txtLoanFieldId20";
      this.txtLoanFieldId20.Size = new Size(95, 20);
      this.txtLoanFieldId20.TabIndex = 412;
      this.txtLoanFieldId20.Tag = (object) "20";
      this.txtLoanFieldId20.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId20.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId20.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription14.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription14.Location = new Point(135, 310);
      this.txtLoanFieldDescription14.MaxLength = 50;
      this.txtLoanFieldDescription14.Name = "txtLoanFieldDescription14";
      this.txtLoanFieldDescription14.ReadOnly = true;
      this.txtLoanFieldDescription14.Size = new Size(128, 20);
      this.txtLoanFieldDescription14.TabIndex = 395;
      this.txtLoanFieldDescription14.TabStop = false;
      this.txtLoanFieldDescription14.Tag = (object) "14";
      this.txtLoanFieldId9.BackColor = SystemColors.Window;
      this.txtLoanFieldId9.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId9.Location = new Point(12, 200);
      this.txtLoanFieldId9.MaxLength = 30;
      this.txtLoanFieldId9.Name = "txtLoanFieldId9";
      this.txtLoanFieldId9.Size = new Size(95, 20);
      this.txtLoanFieldId9.TabIndex = 379;
      this.txtLoanFieldId9.Tag = (object) "9";
      this.txtLoanFieldId9.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId9.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId9.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldId8.BackColor = SystemColors.Window;
      this.txtLoanFieldId8.CharacterCasing = CharacterCasing.Upper;
      this.txtLoanFieldId8.Location = new Point(12, 178);
      this.txtLoanFieldId8.MaxLength = 30;
      this.txtLoanFieldId8.Name = "txtLoanFieldId8";
      this.txtLoanFieldId8.Size = new Size(95, 20);
      this.txtLoanFieldId8.TabIndex = 376;
      this.txtLoanFieldId8.Tag = (object) "8";
      this.txtLoanFieldId8.TextChanged += new EventHandler(this.txtField_TextChanged);
      this.txtLoanFieldId8.Enter += new EventHandler(this.txtLoanFieldId_Enter);
      this.txtLoanFieldId8.Leave += new EventHandler(this.txtLoanFieldId_Leave);
      this.txtLoanFieldDescription15.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription15.Location = new Point(135, 332);
      this.txtLoanFieldDescription15.MaxLength = 50;
      this.txtLoanFieldDescription15.Name = "txtLoanFieldDescription15";
      this.txtLoanFieldDescription15.ReadOnly = true;
      this.txtLoanFieldDescription15.Size = new Size(128, 20);
      this.txtLoanFieldDescription15.TabIndex = 398;
      this.txtLoanFieldDescription15.TabStop = false;
      this.txtLoanFieldDescription15.Tag = (object) "15";
      this.txtLoanFieldDescription1.BackColor = SystemColors.Window;
      this.txtLoanFieldDescription1.Location = new Point(135, 24);
      this.txtLoanFieldDescription1.MaxLength = 50;
      this.txtLoanFieldDescription1.Name = "txtLoanFieldDescription1";
      this.txtLoanFieldDescription1.ReadOnly = true;
      this.txtLoanFieldDescription1.Size = new Size(128, 20);
      this.txtLoanFieldDescription1.TabIndex = 356;
      this.txtLoanFieldDescription1.TabStop = false;
      this.txtLoanFieldDescription1.Tag = (object) "1";
      this.lblLoanFieldDescription.Location = new Point(132, 6);
      this.lblLoanFieldDescription.Name = "lblLoanFieldDescription";
      this.lblLoanFieldDescription.Size = new Size(128, 13);
      this.lblLoanFieldDescription.TabIndex = 353;
      this.lblLoanFieldDescription.Text = "Loan Field Description";
      this.lblLoanFieldDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.chkTwoWayTransfer10.AutoSize = true;
      this.chkTwoWayTransfer10.Location = new Point(286, 225);
      this.chkTwoWayTransfer10.Name = "chkTwoWayTransfer10";
      this.chkTwoWayTransfer10.Size = new Size(15, 14);
      this.chkTwoWayTransfer10.TabIndex = 384;
      this.chkTwoWayTransfer10.Tag = (object) "10";
      this.chkTwoWayTransfer10.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer10.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer10.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.chkTwoWayTransfer17.AutoSize = true;
      this.chkTwoWayTransfer17.Location = new Point(286, 379);
      this.chkTwoWayTransfer17.Name = "chkTwoWayTransfer17";
      this.chkTwoWayTransfer17.Size = new Size(15, 14);
      this.chkTwoWayTransfer17.TabIndex = 405;
      this.chkTwoWayTransfer17.Tag = (object) "17";
      this.chkTwoWayTransfer17.UseVisualStyleBackColor = true;
      this.chkTwoWayTransfer17.CheckedChanged += new EventHandler(this.chkTwoWayTransfer_CheckedChanged);
      this.chkTwoWayTransfer17.Enter += new EventHandler(this.chkTwoWayTransfer_Enter);
      this.gradientPanel2.Borders = AnchorStyles.None;
      this.gradientPanel2.Controls.Add((Control) this.lblLoanFieldMapping);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 26);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(336, 30);
      this.gradientPanel2.TabIndex = 436;
      this.lblLoanFieldMapping.BackColor = Color.Transparent;
      this.lblLoanFieldMapping.Dock = DockStyle.Fill;
      this.lblLoanFieldMapping.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblLoanFieldMapping.Location = new Point(0, 0);
      this.lblLoanFieldMapping.Name = "lblLoanFieldMapping";
      this.lblLoanFieldMapping.Padding = new Padding(10, 0, 0, 0);
      this.lblLoanFieldMapping.Size = new Size(336, 30);
      this.lblLoanFieldMapping.TabIndex = 350;
      this.lblLoanFieldMapping.Text = "\"One Way\" writes custom field data to the loan file, and \"Both Ways\" also writes data from the loan file back to the custom field.";
      this.lblLoanFieldMapping.TextAlign = ContentAlignment.MiddleLeft;
      this.tabPage2.AutoScroll = true;
      this.tabPage2.BackColor = Color.WhiteSmoke;
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Size = new Size(779, 549);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Page 2";
      this.tabPage2.Visible = false;
      this.tabPage3.AutoScroll = true;
      this.tabPage3.BackColor = Color.WhiteSmoke;
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new Size(779, 549);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Page 3";
      this.tabPage3.Visible = false;
      this.tabPage4.AutoScroll = true;
      this.tabPage4.BackColor = Color.WhiteSmoke;
      this.tabPage4.Location = new Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Size = new Size(779, 549);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Page 4";
      this.tabPage4.Visible = false;
      this.tabPage5.AutoScroll = true;
      this.tabPage5.BackColor = Color.WhiteSmoke;
      this.tabPage5.Location = new Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Size = new Size(779, 549);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "Page 5";
      this.splitter1.Location = new Point(0, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 577);
      this.splitter1.TabIndex = 353;
      this.splitter1.TabStop = false;
      this.borderPanel1.Controls.Add((Control) this.tabCustomFields);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(1, 0, 0, 0);
      this.borderPanel1.Size = new Size(790, 577);
      this.borderPanel1.TabIndex = 2;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.borderPanel1);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ContactCustomFieldsForm);
      this.Size = new Size(790, 577);
      this.Load += new EventHandler(this.ContactCustomFieldsForm_Load);
      this.tabCustomFields.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.pnlData.ResumeLayout(false);
      this.gcCustomTab.ResumeLayout(false);
      this.gcCustomTab.PerformLayout();
      this.borderPanel2.ResumeLayout(false);
      this.pnlCustomFieldsInternal.ResumeLayout(false);
      this.pnlCustomFieldsInternal.PerformLayout();
      ((ISupportInitialize) this.btnFieldOption20).EndInit();
      ((ISupportInitialize) this.btnFieldOption19).EndInit();
      ((ISupportInitialize) this.btnFieldOption18).EndInit();
      ((ISupportInitialize) this.btnFieldOption17).EndInit();
      ((ISupportInitialize) this.btnFieldOption16).EndInit();
      ((ISupportInitialize) this.btnFieldOption15).EndInit();
      ((ISupportInitialize) this.btnFieldOption14).EndInit();
      ((ISupportInitialize) this.btnFieldOption13).EndInit();
      ((ISupportInitialize) this.btnFieldOption12).EndInit();
      ((ISupportInitialize) this.btnFieldOption11).EndInit();
      ((ISupportInitialize) this.btnFieldOption10).EndInit();
      ((ISupportInitialize) this.btnFieldOption9).EndInit();
      ((ISupportInitialize) this.btnFieldOption8).EndInit();
      ((ISupportInitialize) this.btnFieldOption1).EndInit();
      ((ISupportInitialize) this.btnFieldOption7).EndInit();
      ((ISupportInitialize) this.btnFieldOption2).EndInit();
      ((ISupportInitialize) this.btnFieldOption3).EndInit();
      ((ISupportInitialize) this.btnFieldOption6).EndInit();
      ((ISupportInitialize) this.btnFieldOption4).EndInit();
      ((ISupportInitialize) this.btnFieldOption5).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gcCustomFieldDefinitions.ResumeLayout(false);
      this.borderPanel3.ResumeLayout(false);
      this.pnlExRightMapping.ResumeLayout(false);
      this.pnlExRightMapping.PerformLayout();
      ((ISupportInitialize) this.picLoanFieldSearch20).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch19).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch18).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch17).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch16).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch15).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch14).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch13).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch12).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch11).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch10).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch9).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch8).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch7).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch6).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch5).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch4).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch3).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch2).EndInit();
      ((ISupportInitialize) this.picLoanFieldSearch1).EndInit();
      this.gradientPanel2.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void txtField_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    public string[] SelectedFields
    {
      get
      {
        return this.selectedFieldsForSync.Count == 0 ? (string[]) null : this.selectedFieldsForSync.ToArray();
      }
      set
      {
        this.selectedFieldsForSync = new List<string>((IEnumerable<string>) value);
        int num = this.TabIndex * 20;
        for (int index = 0; index < 20; ++index)
        {
          if (this.selectedFieldsForSync.Contains(Convert.ToString(index + num + 1)))
            this.chkSelect[index].Checked = true;
        }
      }
    }

    public string[] SelectedCategoryFields
    {
      get
      {
        return this.selectedCategoryFieldsForSync.Count == 0 ? (string[]) null : this.selectedCategoryFieldsForSync.ToArray();
      }
      set => this.selectedCategoryFieldsForSync = new List<string>((IEnumerable<string>) value);
    }
  }
}
