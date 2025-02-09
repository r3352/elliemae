// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecondaryFieldsSetup
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
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SecondaryFieldsSetup : SettingsUserControl
  {
    private Label lblBaseRate;
    private GridView listViewRate;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnEditRate;
    private StandardIconButton stdIconBtnUpRate;
    private StandardIconButton stdIconBtnDownRate;
    private StandardIconButton stdIconBtnDeleteRate;
    private GroupContainer gcBasePrice;
    private StandardIconButton stdIconBtnNewPrice;
    private GridView listViewPrice;
    private StandardIconButton stdIconBtnEditPrice;
    private Label lblBasePrice;
    private StandardIconButton stdIconBtnUpPrice;
    private StandardIconButton stdIconBtnDownPrice;
    private StandardIconButton stdIconBtnDeletePrice;
    private Label lblBaseMargin;
    private GridView listViewMargin;
    private StandardIconButton stdIconBtnDeleteMargin;
    private StandardIconButton stdIconBtnDownMargin;
    private StandardIconButton stdIconBtnUpMargin;
    private StandardIconButton stdIconBtnEditMargin;
    private StandardIconButton stdIconBtnNewMargin;
    private GroupContainer gcBaseMargin;
    private StandardIconButton stdIconBtnNewRate;
    private GroupContainer gcProfitabilityOption;
    private GridView listViewProfitabilityOption;
    private StandardIconButton stdIconBtnNewProfitabilityOption;
    private StandardIconButton stdIconBtnEditProfitabilityOption;
    private StandardIconButton stdIconBtnUpProfitabilityOption;
    private StandardIconButton stdIconBtnDownProfitabilityOption;
    private StandardIconButton stdIconBtnDeleteProfitabilityOption;
    private Label lblProfitabilityList;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageEx1;
    private TabPageEx tabPageEx2;
    private TabPageEx tabPageEx3;
    private TabPageEx tabPageEx4;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private Panel panel4;
    private Panel panel5;
    private Label label1;
    private GroupContainer gcLockTypeOption;
    private StandardIconButton stdIconBtnNewLockTypeOption;
    private StandardIconButton stdIconBtnEditLockTypeOption;
    private StandardIconButton stdIconBtnUpLockTypeOption;
    private StandardIconButton stdIconBtnDownLockTypeOption;
    private StandardIconButton stdIconBtnDeleteLockTypeOption;
    private GridView listViewLockTypeOption;
    private BorderPanel borderPanel1;
    private TabPageEx tabPageEx5;
    private Sessions.Session session;
    private bool isSettingSync;

    public string[] SelectedComputedOptionValues
    {
      get
      {
        GridView selectedListView = this.getSelectedListView();
        if (selectedListView.SelectedItems.Count == 0)
          return (string[]) null;
        string optionType = this.getSelectedOptionType();
        return selectedListView.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => string.Format("{0}_{1}", (object) item.Text, (object) optionType))).ToArray<string>();
      }
      set
      {
        if (value == null || ((IEnumerable<string>) value).Count<string>() == 0)
          return;
        string optionType = value[0].Substring(value[0].LastIndexOf("_") + 1);
        this.setSelectedOptionType(optionType);
        foreach (GVItem gvItem in this.getSelectedListView().Items.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(string.Format("{0}_{1}", (object) item.SubItems[0].Text, (object) optionType)))))
          gvItem.Selected = true;
      }
    }

    public SecondaryFieldsSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance)
    {
    }

    public SecondaryFieldsSetup(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.Reset();
      this.listViewRate_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewPrice_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewMargin_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewProfitabilityOption_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewLockTypeOption_SelectedIndexChanged((object) this, (EventArgs) null);
      if (setupContainer != null)
        return;
      this.isSettingSync = true;
      this.listViewRate.AllowMultiselect = true;
      this.listViewPrice.AllowMultiselect = true;
      this.listViewMargin.AllowMultiselect = true;
      this.listViewProfitabilityOption.AllowMultiselect = true;
      this.listViewLockTypeOption.AllowMultiselect = true;
      this.stdIconBtnNewPrice.Enabled = false;
      this.stdIconBtnEditPrice.Enabled = false;
      this.stdIconBtnUpPrice.Enabled = false;
      this.stdIconBtnDownPrice.Enabled = false;
      this.stdIconBtnDeletePrice.Enabled = false;
      this.stdIconBtnNewProfitabilityOption.Enabled = false;
      this.stdIconBtnEditProfitabilityOption.Enabled = false;
      this.stdIconBtnUpProfitabilityOption.Enabled = false;
      this.stdIconBtnDownProfitabilityOption.Enabled = false;
      this.stdIconBtnDeleteProfitabilityOption.Enabled = false;
      this.stdIconBtnNewLockTypeOption.Enabled = false;
      this.stdIconBtnEditLockTypeOption.Enabled = false;
      this.stdIconBtnUpLockTypeOption.Enabled = false;
      this.stdIconBtnDownLockTypeOption.Enabled = false;
      this.stdIconBtnDeleteLockTypeOption.Enabled = false;
      this.stdIconBtnNewRate.Enabled = false;
      this.stdIconBtnEditRate.Enabled = false;
      this.stdIconBtnUpRate.Enabled = false;
      this.stdIconBtnDownRate.Enabled = false;
      this.stdIconBtnDeleteRate.Enabled = false;
      this.stdIconBtnNewMargin.Enabled = false;
      this.stdIconBtnEditMargin.Enabled = false;
      this.stdIconBtnUpMargin.Enabled = false;
      this.stdIconBtnDownMargin.Enabled = false;
      this.stdIconBtnDeleteMargin.Enabled = false;
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
      this.listViewRate = new GridView();
      this.lblBaseRate = new Label();
      this.gcBasePrice = new GroupContainer();
      this.listViewPrice = new GridView();
      this.stdIconBtnNewPrice = new StandardIconButton();
      this.stdIconBtnEditPrice = new StandardIconButton();
      this.stdIconBtnUpPrice = new StandardIconButton();
      this.stdIconBtnDownPrice = new StandardIconButton();
      this.stdIconBtnDeletePrice = new StandardIconButton();
      this.lblBasePrice = new Label();
      this.gcBaseRate = new GroupContainer();
      this.stdIconBtnNewRate = new StandardIconButton();
      this.stdIconBtnEditRate = new StandardIconButton();
      this.stdIconBtnUpRate = new StandardIconButton();
      this.stdIconBtnDownRate = new StandardIconButton();
      this.stdIconBtnDeleteRate = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnDeleteMargin = new StandardIconButton();
      this.stdIconBtnDownMargin = new StandardIconButton();
      this.stdIconBtnUpMargin = new StandardIconButton();
      this.stdIconBtnEditMargin = new StandardIconButton();
      this.stdIconBtnNewMargin = new StandardIconButton();
      this.stdIconBtnNewProfitabilityOption = new StandardIconButton();
      this.stdIconBtnEditProfitabilityOption = new StandardIconButton();
      this.stdIconBtnUpProfitabilityOption = new StandardIconButton();
      this.stdIconBtnDownProfitabilityOption = new StandardIconButton();
      this.stdIconBtnDeleteProfitabilityOption = new StandardIconButton();
      this.stdIconBtnNewLockTypeOption = new StandardIconButton();
      this.stdIconBtnEditLockTypeOption = new StandardIconButton();
      this.stdIconBtnUpLockTypeOption = new StandardIconButton();
      this.stdIconBtnDownLockTypeOption = new StandardIconButton();
      this.stdIconBtnDeleteLockTypeOption = new StandardIconButton();
      this.lblBaseMargin = new Label();
      this.listViewMargin = new GridView();
      this.gcBaseMargin = new GroupContainer();
      this.gcProfitabilityOption = new GroupContainer();
      this.listViewProfitabilityOption = new GridView();
      this.lblProfitabilityList = new Label();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageEx1 = new TabPageEx();
      this.panel1 = new Panel();
      this.tabPageEx2 = new TabPageEx();
      this.panel2 = new Panel();
      this.tabPageEx3 = new TabPageEx();
      this.gcLockTypeOption = new GroupContainer();
      this.listViewLockTypeOption = new GridView();
      this.panel3 = new Panel();
      this.label1 = new Label();
      this.tabPageEx4 = new TabPageEx();
      this.panel4 = new Panel();
      this.tabPageEx5 = new TabPageEx();
      this.panel5 = new Panel();
      this.borderPanel1 = new BorderPanel();
      this.gcBasePrice.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNewPrice).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditPrice).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpPrice).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownPrice).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeletePrice).BeginInit();
      this.gcBaseRate.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNewRate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditRate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpRate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownRate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteRate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteMargin).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownMargin).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpMargin).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditMargin).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewMargin).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewProfitabilityOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditProfitabilityOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpProfitabilityOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownProfitabilityOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteProfitabilityOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewLockTypeOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditLockTypeOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpLockTypeOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownLockTypeOption).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLockTypeOption).BeginInit();
      this.gcBaseMargin.SuspendLayout();
      this.gcProfitabilityOption.SuspendLayout();
      this.tabControlEx1.SuspendLayout();
      this.tabPageEx1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tabPageEx2.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tabPageEx3.SuspendLayout();
      this.gcLockTypeOption.SuspendLayout();
      this.panel3.SuspendLayout();
      this.tabPageEx4.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tabPageEx5.SuspendLayout();
      this.panel5.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.listViewRate.AllowMultiselect = false;
      this.listViewRate.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Personas";
      gvColumn1.Width = 400;
      this.listViewRate.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.listViewRate.Dock = DockStyle.Fill;
      this.listViewRate.HeaderHeight = 0;
      this.listViewRate.HeaderVisible = false;
      this.listViewRate.Location = new Point(1, 26);
      this.listViewRate.Name = "listViewRate";
      this.listViewRate.Size = new Size(192, 12);
      this.listViewRate.TabIndex = 0;
      this.listViewRate.SelectedIndexChanged += new EventHandler(this.listViewRate_SelectedIndexChanged);
      this.listViewRate.DoubleClick += new EventHandler(this.listViewRate_DoubleClick);
      this.lblBaseRate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblBaseRate.BackColor = Color.Transparent;
      this.lblBaseRate.Location = new Point(3, 3);
      this.lblBaseRate.Name = "lblBaseRate";
      this.lblBaseRate.Size = new Size(110, 46);
      this.lblBaseRate.TabIndex = 56;
      this.lblBaseRate.Text = "Create dropdown values for the base rate options that users can select on the Lock Request and Secondary Registration tools.";
      this.gcBasePrice.Controls.Add((Control) this.listViewPrice);
      this.gcBasePrice.Controls.Add((Control) this.stdIconBtnNewPrice);
      this.gcBasePrice.Controls.Add((Control) this.stdIconBtnEditPrice);
      this.gcBasePrice.Controls.Add((Control) this.stdIconBtnUpPrice);
      this.gcBasePrice.Controls.Add((Control) this.stdIconBtnDownPrice);
      this.gcBasePrice.Controls.Add((Control) this.stdIconBtnDeletePrice);
      this.gcBasePrice.Dock = DockStyle.Fill;
      this.gcBasePrice.HeaderForeColor = SystemColors.ControlText;
      this.gcBasePrice.Location = new Point(3, 58);
      this.gcBasePrice.Margin = new Padding(0);
      this.gcBasePrice.Name = "gcBasePrice";
      this.gcBasePrice.Size = new Size(994, 429);
      this.gcBasePrice.TabIndex = 7;
      this.gcBasePrice.Text = "Base Price Dropdown List";
      this.listViewPrice.AllowMultiselect = false;
      this.listViewPrice.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Personas";
      gvColumn2.Width = 400;
      this.listViewPrice.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.listViewPrice.Dock = DockStyle.Fill;
      this.listViewPrice.HeaderHeight = 0;
      this.listViewPrice.HeaderVisible = false;
      this.listViewPrice.Location = new Point(1, 26);
      this.listViewPrice.Name = "listViewPrice";
      this.listViewPrice.Size = new Size(992, 402);
      this.listViewPrice.TabIndex = 6;
      this.listViewPrice.SelectedIndexChanged += new EventHandler(this.listViewPrice_SelectedIndexChanged);
      this.listViewPrice.DoubleClick += new EventHandler(this.listViewPrice_DoubleClick);
      this.stdIconBtnNewPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewPrice.BackColor = Color.Transparent;
      this.stdIconBtnNewPrice.Location = new Point(884, 5);
      this.stdIconBtnNewPrice.MouseDownImage = (Image) null;
      this.stdIconBtnNewPrice.Name = "stdIconBtnNewPrice";
      this.stdIconBtnNewPrice.Size = new Size(16, 16);
      this.stdIconBtnNewPrice.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewPrice.TabIndex = 78;
      this.stdIconBtnNewPrice.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewPrice, "New Base Price");
      this.stdIconBtnNewPrice.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditPrice.BackColor = Color.Transparent;
      this.stdIconBtnEditPrice.Location = new Point(906, 5);
      this.stdIconBtnEditPrice.MouseDownImage = (Image) null;
      this.stdIconBtnEditPrice.Name = "stdIconBtnEditPrice";
      this.stdIconBtnEditPrice.Size = new Size(16, 16);
      this.stdIconBtnEditPrice.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditPrice.TabIndex = 77;
      this.stdIconBtnEditPrice.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditPrice, "Edit Base Price");
      this.stdIconBtnEditPrice.Click += new EventHandler(this.stdIconBtnEditPrice_Click);
      this.stdIconBtnUpPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpPrice.BackColor = Color.Transparent;
      this.stdIconBtnUpPrice.Location = new Point(928, 5);
      this.stdIconBtnUpPrice.MouseDownImage = (Image) null;
      this.stdIconBtnUpPrice.Name = "stdIconBtnUpPrice";
      this.stdIconBtnUpPrice.Size = new Size(16, 16);
      this.stdIconBtnUpPrice.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpPrice.TabIndex = 76;
      this.stdIconBtnUpPrice.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpPrice, "Move Base Price Up");
      this.stdIconBtnUpPrice.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownPrice.BackColor = Color.Transparent;
      this.stdIconBtnDownPrice.Location = new Point(950, 5);
      this.stdIconBtnDownPrice.MouseDownImage = (Image) null;
      this.stdIconBtnDownPrice.Name = "stdIconBtnDownPrice";
      this.stdIconBtnDownPrice.Size = new Size(16, 16);
      this.stdIconBtnDownPrice.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownPrice.TabIndex = 75;
      this.stdIconBtnDownPrice.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownPrice, "Move Base Price Down");
      this.stdIconBtnDownPrice.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeletePrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeletePrice.BackColor = Color.Transparent;
      this.stdIconBtnDeletePrice.Location = new Point(972, 5);
      this.stdIconBtnDeletePrice.MouseDownImage = (Image) null;
      this.stdIconBtnDeletePrice.Name = "stdIconBtnDeletePrice";
      this.stdIconBtnDeletePrice.Size = new Size(16, 16);
      this.stdIconBtnDeletePrice.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeletePrice.TabIndex = 74;
      this.stdIconBtnDeletePrice.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeletePrice, "Delete Base Price");
      this.stdIconBtnDeletePrice.Click += new EventHandler(this.buttonDelete_Click);
      this.lblBasePrice.BackColor = Color.Transparent;
      this.lblBasePrice.Location = new Point(3, 3);
      this.lblBasePrice.Name = "lblBasePrice";
      this.lblBasePrice.Size = new Size(554, 46);
      this.lblBasePrice.TabIndex = 58;
      this.lblBasePrice.Text = "Create dropdown values for the base price options that users can select on the Lock Request and Secondary Registration tools.";
      this.gcBaseRate.Controls.Add((Control) this.listViewRate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnNewRate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnEditRate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnUpRate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDownRate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDeleteRate);
      this.gcBaseRate.Dock = DockStyle.Fill;
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(3, 58);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(194, 39);
      this.gcBaseRate.TabIndex = 1;
      this.gcBaseRate.Text = "Base Rate Dropdown List";
      this.stdIconBtnNewRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewRate.BackColor = Color.Transparent;
      this.stdIconBtnNewRate.Location = new Point(83, 4);
      this.stdIconBtnNewRate.MouseDownImage = (Image) null;
      this.stdIconBtnNewRate.Name = "stdIconBtnNewRate";
      this.stdIconBtnNewRate.Size = new Size(16, 16);
      this.stdIconBtnNewRate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewRate.TabIndex = 79;
      this.stdIconBtnNewRate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewRate, "New Base Rate");
      this.stdIconBtnNewRate.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditRate.BackColor = Color.Transparent;
      this.stdIconBtnEditRate.Location = new Point(105, 5);
      this.stdIconBtnEditRate.MouseDownImage = (Image) null;
      this.stdIconBtnEditRate.Name = "stdIconBtnEditRate";
      this.stdIconBtnEditRate.Size = new Size(16, 16);
      this.stdIconBtnEditRate.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditRate.TabIndex = 77;
      this.stdIconBtnEditRate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditRate, "Edit Base Rate");
      this.stdIconBtnEditRate.Click += new EventHandler(this.stdIconBtnEditRate_Click);
      this.stdIconBtnUpRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpRate.BackColor = Color.Transparent;
      this.stdIconBtnUpRate.Location = new Point((int) sbyte.MaxValue, 5);
      this.stdIconBtnUpRate.MouseDownImage = (Image) null;
      this.stdIconBtnUpRate.Name = "stdIconBtnUpRate";
      this.stdIconBtnUpRate.Size = new Size(16, 16);
      this.stdIconBtnUpRate.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpRate.TabIndex = 76;
      this.stdIconBtnUpRate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpRate, "Move Base Rate Up");
      this.stdIconBtnUpRate.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownRate.BackColor = Color.Transparent;
      this.stdIconBtnDownRate.Location = new Point(149, 5);
      this.stdIconBtnDownRate.MouseDownImage = (Image) null;
      this.stdIconBtnDownRate.Name = "stdIconBtnDownRate";
      this.stdIconBtnDownRate.Size = new Size(16, 16);
      this.stdIconBtnDownRate.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownRate.TabIndex = 75;
      this.stdIconBtnDownRate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownRate, "Move Base Rate Down");
      this.stdIconBtnDownRate.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteRate.BackColor = Color.Transparent;
      this.stdIconBtnDeleteRate.Location = new Point(171, 5);
      this.stdIconBtnDeleteRate.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteRate.Name = "stdIconBtnDeleteRate";
      this.stdIconBtnDeleteRate.Size = new Size(16, 16);
      this.stdIconBtnDeleteRate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteRate.TabIndex = 74;
      this.stdIconBtnDeleteRate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteRate, "Delete Base Rate");
      this.stdIconBtnDeleteRate.Click += new EventHandler(this.buttonDelete_Click);
      this.stdIconBtnDeleteMargin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteMargin.BackColor = Color.Transparent;
      this.stdIconBtnDeleteMargin.Location = new Point(172, 5);
      this.stdIconBtnDeleteMargin.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteMargin.Name = "stdIconBtnDeleteMargin";
      this.stdIconBtnDeleteMargin.Size = new Size(16, 16);
      this.stdIconBtnDeleteMargin.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteMargin.TabIndex = 68;
      this.stdIconBtnDeleteMargin.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteMargin, "Delete Base ARM Margin");
      this.stdIconBtnDeleteMargin.Click += new EventHandler(this.buttonDelete_Click);
      this.stdIconBtnDownMargin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownMargin.BackColor = Color.Transparent;
      this.stdIconBtnDownMargin.Location = new Point(150, 5);
      this.stdIconBtnDownMargin.MouseDownImage = (Image) null;
      this.stdIconBtnDownMargin.Name = "stdIconBtnDownMargin";
      this.stdIconBtnDownMargin.Size = new Size(16, 16);
      this.stdIconBtnDownMargin.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownMargin.TabIndex = 70;
      this.stdIconBtnDownMargin.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownMargin, "Move Base ARM Margin Down");
      this.stdIconBtnDownMargin.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnUpMargin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpMargin.BackColor = Color.Transparent;
      this.stdIconBtnUpMargin.Location = new Point(128, 5);
      this.stdIconBtnUpMargin.MouseDownImage = (Image) null;
      this.stdIconBtnUpMargin.Name = "stdIconBtnUpMargin";
      this.stdIconBtnUpMargin.Size = new Size(16, 16);
      this.stdIconBtnUpMargin.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpMargin.TabIndex = 71;
      this.stdIconBtnUpMargin.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpMargin, "Move Base ARM Margin Up");
      this.stdIconBtnUpMargin.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnEditMargin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditMargin.BackColor = Color.Transparent;
      this.stdIconBtnEditMargin.Location = new Point(106, 5);
      this.stdIconBtnEditMargin.MouseDownImage = (Image) null;
      this.stdIconBtnEditMargin.Name = "stdIconBtnEditMargin";
      this.stdIconBtnEditMargin.Size = new Size(16, 16);
      this.stdIconBtnEditMargin.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditMargin.TabIndex = 72;
      this.stdIconBtnEditMargin.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditMargin, "Edit Base ARM Margin");
      this.stdIconBtnEditMargin.Click += new EventHandler(this.stdIconBtnEditMargin_Click);
      this.stdIconBtnNewMargin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewMargin.BackColor = Color.Transparent;
      this.stdIconBtnNewMargin.Location = new Point(84, 5);
      this.stdIconBtnNewMargin.MouseDownImage = (Image) null;
      this.stdIconBtnNewMargin.Name = "stdIconBtnNewMargin";
      this.stdIconBtnNewMargin.Size = new Size(16, 16);
      this.stdIconBtnNewMargin.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewMargin.TabIndex = 73;
      this.stdIconBtnNewMargin.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewMargin, "New Base ARM Margin");
      this.stdIconBtnNewMargin.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnNewProfitabilityOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewProfitabilityOption.BackColor = Color.Transparent;
      this.stdIconBtnNewProfitabilityOption.Location = new Point(84, 5);
      this.stdIconBtnNewProfitabilityOption.MouseDownImage = (Image) null;
      this.stdIconBtnNewProfitabilityOption.Name = "stdIconBtnNewProfitabilityOption";
      this.stdIconBtnNewProfitabilityOption.Size = new Size(16, 16);
      this.stdIconBtnNewProfitabilityOption.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewProfitabilityOption.TabIndex = 73;
      this.stdIconBtnNewProfitabilityOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewProfitabilityOption, "New Profitability");
      this.stdIconBtnNewProfitabilityOption.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditProfitabilityOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditProfitabilityOption.BackColor = Color.Transparent;
      this.stdIconBtnEditProfitabilityOption.Location = new Point(106, 5);
      this.stdIconBtnEditProfitabilityOption.MouseDownImage = (Image) null;
      this.stdIconBtnEditProfitabilityOption.Name = "stdIconBtnEditProfitabilityOption";
      this.stdIconBtnEditProfitabilityOption.Size = new Size(16, 16);
      this.stdIconBtnEditProfitabilityOption.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditProfitabilityOption.TabIndex = 72;
      this.stdIconBtnEditProfitabilityOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditProfitabilityOption, "Edit Profitability");
      this.stdIconBtnEditProfitabilityOption.Click += new EventHandler(this.stdIconBtnEditProfitabilityOption_Click);
      this.stdIconBtnUpProfitabilityOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpProfitabilityOption.BackColor = Color.Transparent;
      this.stdIconBtnUpProfitabilityOption.Location = new Point(128, 5);
      this.stdIconBtnUpProfitabilityOption.MouseDownImage = (Image) null;
      this.stdIconBtnUpProfitabilityOption.Name = "stdIconBtnUpProfitabilityOption";
      this.stdIconBtnUpProfitabilityOption.Size = new Size(16, 16);
      this.stdIconBtnUpProfitabilityOption.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpProfitabilityOption.TabIndex = 71;
      this.stdIconBtnUpProfitabilityOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpProfitabilityOption, "Move Profitability Up");
      this.stdIconBtnUpProfitabilityOption.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownProfitabilityOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownProfitabilityOption.BackColor = Color.Transparent;
      this.stdIconBtnDownProfitabilityOption.Location = new Point(150, 5);
      this.stdIconBtnDownProfitabilityOption.MouseDownImage = (Image) null;
      this.stdIconBtnDownProfitabilityOption.Name = "stdIconBtnDownProfitabilityOption";
      this.stdIconBtnDownProfitabilityOption.Size = new Size(16, 16);
      this.stdIconBtnDownProfitabilityOption.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownProfitabilityOption.TabIndex = 70;
      this.stdIconBtnDownProfitabilityOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownProfitabilityOption, "Move Profitability Down");
      this.stdIconBtnDownProfitabilityOption.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteProfitabilityOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteProfitabilityOption.BackColor = Color.Transparent;
      this.stdIconBtnDeleteProfitabilityOption.Location = new Point(172, 5);
      this.stdIconBtnDeleteProfitabilityOption.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteProfitabilityOption.Name = "stdIconBtnDeleteProfitabilityOption";
      this.stdIconBtnDeleteProfitabilityOption.Size = new Size(16, 16);
      this.stdIconBtnDeleteProfitabilityOption.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteProfitabilityOption.TabIndex = 68;
      this.stdIconBtnDeleteProfitabilityOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteProfitabilityOption, "Delete Profitability");
      this.stdIconBtnDeleteProfitabilityOption.Click += new EventHandler(this.buttonDelete_Click);
      this.stdIconBtnNewLockTypeOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewLockTypeOption.BackColor = Color.Transparent;
      this.stdIconBtnNewLockTypeOption.Location = new Point(84, 5);
      this.stdIconBtnNewLockTypeOption.MouseDownImage = (Image) null;
      this.stdIconBtnNewLockTypeOption.Name = "stdIconBtnNewLockTypeOption";
      this.stdIconBtnNewLockTypeOption.Size = new Size(16, 16);
      this.stdIconBtnNewLockTypeOption.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewLockTypeOption.TabIndex = 73;
      this.stdIconBtnNewLockTypeOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewLockTypeOption, "New Lock Type");
      this.stdIconBtnNewLockTypeOption.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditLockTypeOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditLockTypeOption.BackColor = Color.Transparent;
      this.stdIconBtnEditLockTypeOption.Location = new Point(106, 5);
      this.stdIconBtnEditLockTypeOption.MouseDownImage = (Image) null;
      this.stdIconBtnEditLockTypeOption.Name = "stdIconBtnEditLockTypeOption";
      this.stdIconBtnEditLockTypeOption.Size = new Size(16, 16);
      this.stdIconBtnEditLockTypeOption.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditLockTypeOption.TabIndex = 72;
      this.stdIconBtnEditLockTypeOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditLockTypeOption, "Edit Lock Type");
      this.stdIconBtnEditLockTypeOption.Click += new EventHandler(this.stdIconBtnEditLockTypeOption_Click);
      this.stdIconBtnUpLockTypeOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpLockTypeOption.BackColor = Color.Transparent;
      this.stdIconBtnUpLockTypeOption.Location = new Point(128, 5);
      this.stdIconBtnUpLockTypeOption.MouseDownImage = (Image) null;
      this.stdIconBtnUpLockTypeOption.Name = "stdIconBtnUpLockTypeOption";
      this.stdIconBtnUpLockTypeOption.Size = new Size(16, 16);
      this.stdIconBtnUpLockTypeOption.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpLockTypeOption.TabIndex = 71;
      this.stdIconBtnUpLockTypeOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpLockTypeOption, "Move Lock Type Up");
      this.stdIconBtnUpLockTypeOption.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownLockTypeOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownLockTypeOption.BackColor = Color.Transparent;
      this.stdIconBtnDownLockTypeOption.Location = new Point(150, 5);
      this.stdIconBtnDownLockTypeOption.MouseDownImage = (Image) null;
      this.stdIconBtnDownLockTypeOption.Name = "stdIconBtnDownLockTypeOption";
      this.stdIconBtnDownLockTypeOption.Size = new Size(16, 16);
      this.stdIconBtnDownLockTypeOption.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownLockTypeOption.TabIndex = 70;
      this.stdIconBtnDownLockTypeOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownLockTypeOption, "Move Lock Type Down");
      this.stdIconBtnDownLockTypeOption.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteLockTypeOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteLockTypeOption.BackColor = Color.Transparent;
      this.stdIconBtnDeleteLockTypeOption.Location = new Point(172, 5);
      this.stdIconBtnDeleteLockTypeOption.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteLockTypeOption.Name = "stdIconBtnDeleteLockTypeOption";
      this.stdIconBtnDeleteLockTypeOption.Size = new Size(16, 16);
      this.stdIconBtnDeleteLockTypeOption.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteLockTypeOption.TabIndex = 68;
      this.stdIconBtnDeleteLockTypeOption.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteLockTypeOption, "Delete Lock Type");
      this.stdIconBtnDeleteLockTypeOption.Click += new EventHandler(this.buttonDelete_Click);
      this.lblBaseMargin.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblBaseMargin.BackColor = Color.Transparent;
      this.lblBaseMargin.Location = new Point(3, 3);
      this.lblBaseMargin.Name = "lblBaseMargin";
      this.lblBaseMargin.Size = new Size(110, 46);
      this.lblBaseMargin.TabIndex = 61;
      this.lblBaseMargin.Text = "Create dropdown values for the base ARM margin options that users can select on the Lock Request and Secondary Registration tools.";
      this.listViewMargin.AllowMultiselect = false;
      this.listViewMargin.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Personas";
      gvColumn3.Width = 500;
      this.listViewMargin.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.listViewMargin.Dock = DockStyle.Fill;
      this.listViewMargin.HeaderHeight = 0;
      this.listViewMargin.HeaderVisible = false;
      this.listViewMargin.Location = new Point(1, 26);
      this.listViewMargin.Name = "listViewMargin";
      this.listViewMargin.Size = new Size(192, 12);
      this.listViewMargin.TabIndex = 60;
      this.listViewMargin.SelectedIndexChanged += new EventHandler(this.listViewMargin_SelectedIndexChanged);
      this.listViewMargin.DoubleClick += new EventHandler(this.listViewMargin_DoubleClick);
      this.gcBaseMargin.Controls.Add((Control) this.listViewMargin);
      this.gcBaseMargin.Controls.Add((Control) this.stdIconBtnNewMargin);
      this.gcBaseMargin.Controls.Add((Control) this.stdIconBtnEditMargin);
      this.gcBaseMargin.Controls.Add((Control) this.stdIconBtnUpMargin);
      this.gcBaseMargin.Controls.Add((Control) this.stdIconBtnDownMargin);
      this.gcBaseMargin.Controls.Add((Control) this.stdIconBtnDeleteMargin);
      this.gcBaseMargin.Dock = DockStyle.Fill;
      this.gcBaseMargin.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseMargin.Location = new Point(3, 58);
      this.gcBaseMargin.Name = "gcBaseMargin";
      this.gcBaseMargin.Size = new Size(194, 39);
      this.gcBaseMargin.TabIndex = 70;
      this.gcBaseMargin.Text = "Base ARM Margin Dropdown List";
      this.gcProfitabilityOption.Controls.Add((Control) this.stdIconBtnNewProfitabilityOption);
      this.gcProfitabilityOption.Controls.Add((Control) this.stdIconBtnEditProfitabilityOption);
      this.gcProfitabilityOption.Controls.Add((Control) this.stdIconBtnUpProfitabilityOption);
      this.gcProfitabilityOption.Controls.Add((Control) this.stdIconBtnDownProfitabilityOption);
      this.gcProfitabilityOption.Controls.Add((Control) this.stdIconBtnDeleteProfitabilityOption);
      this.gcProfitabilityOption.Controls.Add((Control) this.listViewProfitabilityOption);
      this.gcProfitabilityOption.Dock = DockStyle.Fill;
      this.gcProfitabilityOption.HeaderForeColor = SystemColors.ControlText;
      this.gcProfitabilityOption.Location = new Point(3, 58);
      this.gcProfitabilityOption.Name = "gcProfitabilityOption";
      this.gcProfitabilityOption.Size = new Size(194, 39);
      this.gcProfitabilityOption.TabIndex = 71;
      this.gcProfitabilityOption.Text = "Profitability Dropdown List";
      this.listViewProfitabilityOption.AllowMultiselect = false;
      this.listViewProfitabilityOption.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Personas";
      gvColumn4.Width = 500;
      this.listViewProfitabilityOption.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.listViewProfitabilityOption.Dock = DockStyle.Fill;
      this.listViewProfitabilityOption.HeaderHeight = 0;
      this.listViewProfitabilityOption.HeaderVisible = false;
      this.listViewProfitabilityOption.Location = new Point(1, 26);
      this.listViewProfitabilityOption.Name = "listViewProfitabilityOption";
      this.listViewProfitabilityOption.Size = new Size(192, 12);
      this.listViewProfitabilityOption.TabIndex = 60;
      this.listViewProfitabilityOption.SelectedIndexChanged += new EventHandler(this.listViewProfitabilityOption_SelectedIndexChanged);
      this.listViewProfitabilityOption.DoubleClick += new EventHandler(this.listViewProfitabilityOption_DoubleClick);
      this.lblProfitabilityList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblProfitabilityList.BackColor = Color.Transparent;
      this.lblProfitabilityList.Location = new Point(3, 3);
      this.lblProfitabilityList.Name = "lblProfitabilityList";
      this.lblProfitabilityList.Size = new Size(110, 46);
      this.lblProfitabilityList.TabIndex = 61;
      this.lblProfitabilityList.Text = "Create dropdown values for the profitability options that users can select on the Secondary Lock Tool.";
      this.tabControlEx1.Dock = DockStyle.Fill;
      this.tabControlEx1.Location = new Point(3, 3);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(1004, 516);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 0;
      this.tabControlEx1.TabPages.Add(this.tabPageEx1);
      this.tabControlEx1.TabPages.Add(this.tabPageEx2);
      this.tabControlEx1.TabPages.Add(this.tabPageEx3);
      this.tabControlEx1.TabPages.Add(this.tabPageEx4);
      this.tabControlEx1.TabPages.Add(this.tabPageEx5);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabPageEx1.BackColor = Color.Transparent;
      this.tabPageEx1.Controls.Add((Control) this.gcBasePrice);
      this.tabPageEx1.Controls.Add((Control) this.panel1);
      this.tabPageEx1.Location = new Point(1, 23);
      this.tabPageEx1.Name = "tabPageEx1";
      this.tabPageEx1.Padding = new Padding(3);
      this.tabPageEx1.TabIndex = 0;
      this.tabPageEx1.TabWidth = 100;
      this.tabPageEx1.Text = "Base Price";
      this.tabPageEx1.Value = (object) "Base Price";
      this.panel1.Controls.Add((Control) this.lblBasePrice);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Margin = new Padding(0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(994, 55);
      this.panel1.TabIndex = 59;
      this.tabPageEx2.BackColor = Color.Transparent;
      this.tabPageEx2.Controls.Add((Control) this.gcProfitabilityOption);
      this.tabPageEx2.Controls.Add((Control) this.panel2);
      this.tabPageEx2.Location = new Point(1, 23);
      this.tabPageEx2.Name = "tabPageEx2";
      this.tabPageEx2.Padding = new Padding(3);
      this.tabPageEx2.TabIndex = 0;
      this.tabPageEx2.TabWidth = 100;
      this.tabPageEx2.Text = "Profitability";
      this.tabPageEx2.Value = (object) "Profitability";
      this.panel2.Controls.Add((Control) this.lblProfitabilityList);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(194, 55);
      this.panel2.TabIndex = 60;
      this.tabPageEx3.BackColor = Color.Transparent;
      this.tabPageEx3.Controls.Add((Control) this.gcLockTypeOption);
      this.tabPageEx3.Controls.Add((Control) this.panel3);
      this.tabPageEx3.Location = new Point(1, 23);
      this.tabPageEx3.Name = "tabPageEx3";
      this.tabPageEx3.Padding = new Padding(3);
      this.tabPageEx3.TabIndex = 0;
      this.tabPageEx3.TabWidth = 100;
      this.tabPageEx3.Text = "Lock Type";
      this.tabPageEx3.Value = (object) "Lock Type";
      this.gcLockTypeOption.Controls.Add((Control) this.stdIconBtnNewLockTypeOption);
      this.gcLockTypeOption.Controls.Add((Control) this.stdIconBtnEditLockTypeOption);
      this.gcLockTypeOption.Controls.Add((Control) this.stdIconBtnUpLockTypeOption);
      this.gcLockTypeOption.Controls.Add((Control) this.stdIconBtnDownLockTypeOption);
      this.gcLockTypeOption.Controls.Add((Control) this.stdIconBtnDeleteLockTypeOption);
      this.gcLockTypeOption.Controls.Add((Control) this.listViewLockTypeOption);
      this.gcLockTypeOption.Dock = DockStyle.Fill;
      this.gcLockTypeOption.HeaderForeColor = SystemColors.ControlText;
      this.gcLockTypeOption.Location = new Point(3, 58);
      this.gcLockTypeOption.Name = "gcLockTypeOption";
      this.gcLockTypeOption.Size = new Size(194, 39);
      this.gcLockTypeOption.TabIndex = 72;
      this.gcLockTypeOption.Text = "Lock Type Dropdown List";
      this.listViewLockTypeOption.AllowMultiselect = false;
      this.listViewLockTypeOption.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Personas";
      gvColumn5.Width = 500;
      this.listViewLockTypeOption.Columns.AddRange(new GVColumn[1]
      {
        gvColumn5
      });
      this.listViewLockTypeOption.Dock = DockStyle.Fill;
      this.listViewLockTypeOption.HeaderHeight = 0;
      this.listViewLockTypeOption.HeaderVisible = false;
      this.listViewLockTypeOption.Location = new Point(1, 26);
      this.listViewLockTypeOption.Name = "listViewLockTypeOption";
      this.listViewLockTypeOption.Size = new Size(192, 12);
      this.listViewLockTypeOption.TabIndex = 60;
      this.listViewLockTypeOption.SelectedIndexChanged += new EventHandler(this.listViewLockTypeOption_SelectedIndexChanged);
      this.listViewLockTypeOption.DoubleClick += new EventHandler(this.listViewLockTypeOption_DoubleClick);
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(3, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(194, 55);
      this.panel3.TabIndex = 60;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(3, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(554, 46);
      this.label1.TabIndex = 59;
      this.label1.Text = "Create dropdown values for the lock type options that users can select on the Secondary Lock Tool.";
      this.tabPageEx4.BackColor = Color.Transparent;
      this.tabPageEx4.Controls.Add((Control) this.gcBaseRate);
      this.tabPageEx4.Controls.Add((Control) this.panel4);
      this.tabPageEx4.Location = new Point(1, 23);
      this.tabPageEx4.Name = "tabPageEx4";
      this.tabPageEx4.Padding = new Padding(3);
      this.tabPageEx4.TabIndex = 0;
      this.tabPageEx4.TabWidth = 100;
      this.tabPageEx4.Text = "Base Rate";
      this.tabPageEx4.Value = (object) "Base Rate";
      this.panel4.Controls.Add((Control) this.lblBaseRate);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(3, 3);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(194, 55);
      this.panel4.TabIndex = 60;
      this.tabPageEx5.BackColor = Color.Transparent;
      this.tabPageEx5.Controls.Add((Control) this.gcBaseMargin);
      this.tabPageEx5.Controls.Add((Control) this.panel5);
      this.tabPageEx5.Location = new Point(1, 23);
      this.tabPageEx5.Name = "tabPageEx5";
      this.tabPageEx5.Padding = new Padding(3);
      this.tabPageEx5.TabIndex = 0;
      this.tabPageEx5.TabWidth = 100;
      this.tabPageEx5.Text = "Base ARM Margin";
      this.tabPageEx5.Value = (object) "Base ARM Margin";
      this.panel5.Controls.Add((Control) this.lblBaseMargin);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(3, 3);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(194, 55);
      this.panel5.TabIndex = 60;
      this.borderPanel1.Controls.Add((Control) this.tabControlEx1);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(2, 2, 0, 0);
      this.borderPanel1.Size = new Size(1008, 520);
      this.borderPanel1.TabIndex = 73;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.borderPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SecondaryFieldsSetup);
      this.Size = new Size(1008, 520);
      this.SizeChanged += new EventHandler(this.SecondaryFieldsSetup_SizeChanged);
      this.gcBasePrice.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNewPrice).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditPrice).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpPrice).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownPrice).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeletePrice).EndInit();
      this.gcBaseRate.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNewRate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditRate).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpRate).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownRate).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteRate).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteMargin).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownMargin).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpMargin).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditMargin).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewMargin).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewProfitabilityOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditProfitabilityOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpProfitabilityOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownProfitabilityOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteProfitabilityOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewLockTypeOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditLockTypeOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpLockTypeOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownLockTypeOption).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLockTypeOption).EndInit();
      this.gcBaseMargin.ResumeLayout(false);
      this.gcProfitabilityOption.ResumeLayout(false);
      this.tabControlEx1.ResumeLayout(false);
      this.tabPageEx1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.tabPageEx2.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.tabPageEx3.ResumeLayout(false);
      this.gcLockTypeOption.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.tabPageEx4.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.tabPageEx5.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initListView(ArrayList fields, GridView listview)
    {
      listview.Items.Clear();
      if (fields == null || fields.Count == 0)
        return;
      listview.BeginUpdate();
      for (int index = 0; index < fields.Count; ++index)
      {
        GVItem gvItem = new GVItem(fields[index].ToString());
        listview.Items.Add(gvItem);
      }
      listview.EndUpdate();
    }

    public override void Reset()
    {
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.BaseRate), this.listViewRate);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.BaseMargin), this.listViewMargin);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.BasePrice), this.listViewPrice);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.ProfitabilityOption), this.listViewProfitabilityOption);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.LockTypeOption), this.listViewLockTypeOption);
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewRate, -1), SecondaryFieldTypes.BaseRate);
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewMargin, -1), SecondaryFieldTypes.BaseMargin);
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewPrice, -1), SecondaryFieldTypes.BasePrice);
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewProfitabilityOption, -1), SecondaryFieldTypes.ProfitabilityOption);
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewLockTypeOption, -1), SecondaryFieldTypes.LockTypeOption);
      this.setDirtyFlag(false);
    }

    private ArrayList collectListView(GridView listview, int excludeMe)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < listview.Items.Count; ++nItemIndex)
      {
        if (nItemIndex != excludeMe)
          arrayList.Add((object) listview.Items[nItemIndex].Text);
      }
      return arrayList;
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnNewRate)
        this.addNewOption(this.listViewRate, SecondaryFieldTypes.BaseRate);
      else if (standardIconButton == this.stdIconBtnNewMargin)
        this.addNewOption(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
      else if (standardIconButton == this.stdIconBtnNewPrice)
        this.addNewOption(this.listViewPrice, SecondaryFieldTypes.BasePrice);
      else if (standardIconButton == this.stdIconBtnNewProfitabilityOption)
      {
        this.addNewOption(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
      }
      else
      {
        if (standardIconButton != this.stdIconBtnNewLockTypeOption)
          return;
        this.addNewOption(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
      }
    }

    private void addNewOption(GridView listview, SecondaryFieldTypes type)
    {
      using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(type, "", this.collectListView(listview, -1)))
      {
        if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listview.BeginUpdate();
        listview.Items.Add(new GVItem(secondaryFieldDatilForm.NewOption)
        {
          Selected = true
        });
        listview.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void editOption(GridView listview, SecondaryFieldTypes type)
    {
      if (this.isSettingSync)
        return;
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(type, listview.SelectedItems[0].Text, this.collectListView(listview, listview.SelectedItems[0].Index)))
        {
          if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          listview.BeginUpdate();
          listview.SelectedItems[0].Text = secondaryFieldDatilForm.NewOption;
          listview.EndUpdate();
          this.setDirtyFlag(true);
        }
      }
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnUpRate)
        this.moveOptionUp(this.listViewRate, SecondaryFieldTypes.BaseRate);
      else if (standardIconButton == this.stdIconBtnUpMargin)
        this.moveOptionUp(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
      else if (standardIconButton == this.stdIconBtnUpPrice)
        this.moveOptionUp(this.listViewPrice, SecondaryFieldTypes.BasePrice);
      else if (standardIconButton == this.stdIconBtnUpProfitabilityOption)
      {
        this.moveOptionUp(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
      }
      else
      {
        if (standardIconButton != this.stdIconBtnUpLockTypeOption)
          return;
        this.moveOptionUp(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
      }
    }

    private void moveOptionUp(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listview.SelectedItems[0].Index == 0)
          return;
        string text = listview.Items[listview.SelectedItems[0].Index - 1].Text;
        listview.Items[listview.SelectedItems[0].Index - 1].Text = listview.SelectedItems[0].Text;
        listview.SelectedItems[0].Text = text;
        listview.Items[listview.SelectedItems[0].Index - 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnDownRate)
        this.moveOptionDown(this.listViewRate, SecondaryFieldTypes.BaseRate);
      else if (standardIconButton == this.stdIconBtnDownMargin)
        this.moveOptionDown(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
      else if (standardIconButton == this.stdIconBtnDownPrice)
        this.moveOptionDown(this.listViewPrice, SecondaryFieldTypes.BasePrice);
      else if (standardIconButton == this.stdIconBtnDownProfitabilityOption)
      {
        this.moveOptionDown(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
      }
      else
      {
        if (standardIconButton != this.stdIconBtnDownLockTypeOption)
          return;
        this.moveOptionDown(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
      }
    }

    private void moveOptionDown(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listview.SelectedItems[0].Index == listview.Items.Count - 1)
          return;
        string text = listview.Items[listview.SelectedItems[0].Index + 1].Text;
        listview.Items[listview.SelectedItems[0].Index + 1].Text = listview.SelectedItems[0].Text;
        listview.SelectedItems[0].Text = text;
        listview.Items[listview.SelectedItems[0].Index + 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnDeleteRate)
        this.deleteOption(this.listViewRate, SecondaryFieldTypes.BaseRate);
      else if (standardIconButton == this.stdIconBtnDeleteMargin)
        this.deleteOption(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
      else if (standardIconButton == this.stdIconBtnDeletePrice)
        this.deleteOption(this.listViewPrice, SecondaryFieldTypes.BasePrice);
      else if (standardIconButton == this.stdIconBtnDeleteProfitabilityOption)
      {
        this.deleteOption(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
      }
      else
      {
        if (standardIconButton != this.stdIconBtnDeleteLockTypeOption)
          return;
        this.deleteOption(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
      }
    }

    private void deleteOption(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.setDirtyFlag(true);
        int index = listview.SelectedItems[0].Index;
        listview.Items.Remove(listview.SelectedItems[0]);
        if (listview.Items.Count == 0)
          return;
        if (index + 1 > listview.Items.Count)
          listview.Items[listview.Items.Count - 1].Selected = true;
        else
          listview.Items[index].Selected = true;
      }
    }

    private void SecondaryFieldsSetup_SizeChanged(object sender, EventArgs e)
    {
    }

    private void listViewRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnUpRate.Enabled = this.stdIconBtnDownRate.Enabled = this.stdIconBtnEditRate.Enabled = this.listViewRate.SelectedItems.Count == 1;
      this.stdIconBtnDeleteRate.Enabled = this.listViewRate.SelectedItems.Count > 0;
    }

    private void listViewPrice_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnUpPrice.Enabled = this.stdIconBtnDownPrice.Enabled = this.stdIconBtnEditPrice.Enabled = this.listViewPrice.SelectedItems.Count == 1;
      this.stdIconBtnDeletePrice.Enabled = this.listViewPrice.SelectedItems.Count > 0;
    }

    private void listViewMargin_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnUpMargin.Enabled = this.stdIconBtnDownMargin.Enabled = this.stdIconBtnEditMargin.Enabled = this.listViewMargin.SelectedItems.Count == 1;
      this.stdIconBtnDeleteMargin.Enabled = this.listViewMargin.SelectedItems.Count > 0;
    }

    private void listViewProfitabilityOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnUpProfitabilityOption.Enabled = this.stdIconBtnDownProfitabilityOption.Enabled = this.stdIconBtnEditProfitabilityOption.Enabled = this.listViewProfitabilityOption.SelectedItems.Count == 1;
      this.stdIconBtnDeleteProfitabilityOption.Enabled = this.listViewProfitabilityOption.SelectedItems.Count > 0;
    }

    private void listViewLockTypeOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingSync)
        return;
      this.stdIconBtnUpLockTypeOption.Enabled = this.stdIconBtnDownLockTypeOption.Enabled = this.stdIconBtnEditLockTypeOption.Enabled = this.listViewLockTypeOption.SelectedItems.Count == 1;
      this.stdIconBtnDeleteLockTypeOption.Enabled = this.listViewLockTypeOption.SelectedItems.Count > 0;
    }

    private void stdIconBtnEditRate_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewRate, SecondaryFieldTypes.BaseRate);
    }

    private void stdIconBtnEditPrice_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewPrice, SecondaryFieldTypes.BasePrice);
    }

    private void stdIconBtnEditMargin_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
    }

    private void stdIconBtnEditProfitabilityOption_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
    }

    private void stdIconBtnEditLockTypeOption_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
    }

    private void listViewRate_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewRate, SecondaryFieldTypes.BaseRate);
    }

    private void listViewPrice_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewPrice, SecondaryFieldTypes.BasePrice);
    }

    private void listViewMargin_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewMargin, SecondaryFieldTypes.BaseMargin);
    }

    private void listViewProfitabilityOption_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewProfitabilityOption, SecondaryFieldTypes.ProfitabilityOption);
    }

    private void listViewLockTypeOption_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewLockTypeOption, SecondaryFieldTypes.LockTypeOption);
    }

    private GridView getSelectedListView()
    {
      GridView selectedListView = (GridView) null;
      if (this.tabControlEx1.SelectedPage == this.tabPageEx1)
        selectedListView = this.listViewPrice;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx2)
        selectedListView = this.listViewProfitabilityOption;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx3)
        selectedListView = this.listViewLockTypeOption;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx4)
        selectedListView = this.listViewRate;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx5)
        selectedListView = this.listViewMargin;
      return selectedListView;
    }

    private string getSelectedOptionType()
    {
      int num = 0;
      if (this.tabControlEx1.SelectedPage == this.tabPageEx1)
        num = 2;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx2)
        num = 5;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx3)
        num = 8;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx4)
        num = 1;
      else if (this.tabControlEx1.SelectedPage == this.tabPageEx5)
        num = 4;
      return num.ToString();
    }

    private void setSelectedOptionType(string optionTypeValue)
    {
      switch ((SecondaryFieldTypes) Enum.Parse(typeof (SecondaryFieldTypes), optionTypeValue))
      {
        case SecondaryFieldTypes.BaseRate:
          this.tabControlEx1.SelectedPage = this.tabPageEx4;
          break;
        case SecondaryFieldTypes.BasePrice:
          this.tabControlEx1.SelectedPage = this.tabPageEx1;
          break;
        case SecondaryFieldTypes.BaseMargin:
          this.tabControlEx1.SelectedPage = this.tabPageEx5;
          break;
        case SecondaryFieldTypes.ProfitabilityOption:
          this.tabControlEx1.SelectedPage = this.tabPageEx2;
          break;
        case SecondaryFieldTypes.LockTypeOption:
          this.tabControlEx1.SelectedPage = this.tabPageEx3;
          break;
      }
    }
  }
}
