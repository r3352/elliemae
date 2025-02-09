// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsOverview
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsOverview : Form
  {
    private Panel pnlConfigurationBase;
    private Panel pnlCompanySettingBase;
    private Panel pnlTP1;
    private Panel pnlTP2;
    private Panel pnlTP3;
    private TreeView tvTP1;
    private TreeView tvTP2;
    private TreeView tvTP3;
    private int MaxTopNodeNum;
    private TreeView sourceTV;
    private SetUpContainer parent;
    private TreeNode currentDestinationNode;
    private TreeNode targetNode;
    private TreeView tvTP4;
    private Font normalFont;
    private Font normalUnderLineFont;
    private Font boldFont;
    private Font boldUnderLineFont;
    private ImageList imgList;
    private Panel pnlCompanyMaintenance;
    private BorderPanel borderBase;
    private BorderPanel bpBase;
    private BorderPanel bpPersonal;
    private Panel gpCompanyCategory;
    private Panel pnlPersonalCategory;
    private GradientPanel gpPersonalConfig;
    private TreeView tvBP1;
    private TableLayoutPanel tlpSettingsHeader;
    private GradientPanel gp2;
    private Label lblEncDesktopSettings;
    private GradientPanel gp1;
    private Label lblEncWebSettings;
    private GradientPanel gp4_2;
    private GradientPanel gp3_2;
    private Label lblMaintenance;
    private GradientPanel gp3;
    private Label lblConfiguration;
    private GradientPanel gp4;
    private Label lblTP2;
    private Label lblTP1;
    private System.Windows.Forms.LinkLabel llEncWebSettings;
    private IContainer components;

    public SettingsOverview(TreeView tv, SetUpContainer parentForm)
    {
      this.InitializeComponent();
      this.normalFont = new Font(this.tvTP1.Font.FontFamily, this.tvTP1.Font.Size, FontStyle.Regular);
      this.normalUnderLineFont = new Font(this.normalFont.FontFamily, this.normalFont.Size, FontStyle.Underline);
      this.boldFont = this.tvTP1.Font;
      this.boldUnderLineFont = new Font(this.boldFont.FontFamily, this.boldFont.Size, FontStyle.Bold | FontStyle.Underline);
      this.parent = parentForm;
      this.sourceTV = tv;
      this.lblConfiguration.BringToFront();
      this.lblTP1.BringToFront();
      this.lblTP2.BringToFront();
      this.setWebSettingVisiblity();
      this.bpBase.SuspendLayout();
      this.buildPersonalSettings();
      this.buildCompanySettings();
      this.refreshPanel();
      this.bpBase.ResumeLayout(true);
    }

    private void setWebSettingVisiblity()
    {
      if (Session.EncompassEdition == EncompassEdition.Banker && (Session.UserInfo.IsAdministrator() || Session.UserInfo.IsSuperAdministrator()))
        return;
      this.tlpSettingsHeader.RowStyles[0].SizeType = SizeType.Absolute;
      this.tlpSettingsHeader.RowStyles[0].Height = 0.0f;
      this.tlpSettingsHeader.RowStyles[1].SizeType = SizeType.Absolute;
      this.tlpSettingsHeader.RowStyles[1].Height = 0.0f;
      TreeView tvTp1 = this.tvTP1;
      TreeView tvTp2 = this.tvTP2;
      TreeView tvTp3 = this.tvTP3;
      TreeView tvTp4 = this.tvTP4;
      Point point1;
      ref Point local = ref point1;
      Point location = this.tvTP4.Location;
      int x = location.X;
      location = this.tvTP4.Location;
      int y = location.Y - 55;
      local = new Point(x, y);
      Point point2 = point1;
      tvTp4.Location = point2;
      Point point3;
      Point point4 = point3 = point1;
      tvTp3.Location = point3;
      Point point5;
      Point point6 = point5 = point4;
      tvTp2.Location = point5;
      Point point7 = point6;
      tvTp1.Location = point7;
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
      this.pnlConfigurationBase = new Panel();
      this.pnlCompanyMaintenance = new Panel();
      this.tvTP4 = new TreeView();
      this.pnlCompanySettingBase = new Panel();
      this.pnlTP3 = new Panel();
      this.tvTP3 = new TreeView();
      this.pnlTP2 = new Panel();
      this.tvTP2 = new TreeView();
      this.pnlTP1 = new Panel();
      this.tvTP1 = new TreeView();
      this.gpCompanyCategory = new Panel();
      this.imgList = new ImageList(this.components);
      this.borderBase = new BorderPanel();
      this.bpBase = new BorderPanel();
      this.tlpSettingsHeader = new TableLayoutPanel();
      this.gp2 = new GradientPanel();
      this.lblEncDesktopSettings = new Label();
      this.gp1 = new GradientPanel();
      this.lblEncWebSettings = new Label();
      this.gp4_2 = new GradientPanel();
      this.gp3_2 = new GradientPanel();
      this.lblMaintenance = new Label();
      this.gp3 = new GradientPanel();
      this.lblConfiguration = new Label();
      this.gp4 = new GradientPanel();
      this.lblTP2 = new Label();
      this.lblTP1 = new Label();
      this.llEncWebSettings = new System.Windows.Forms.LinkLabel();
      this.bpPersonal = new BorderPanel();
      this.gpPersonalConfig = new GradientPanel();
      this.tvBP1 = new TreeView();
      this.pnlPersonalCategory = new Panel();
      this.pnlConfigurationBase.SuspendLayout();
      this.pnlCompanyMaintenance.SuspendLayout();
      this.pnlCompanySettingBase.SuspendLayout();
      this.pnlTP3.SuspendLayout();
      this.pnlTP2.SuspendLayout();
      this.pnlTP1.SuspendLayout();
      this.borderBase.SuspendLayout();
      this.bpBase.SuspendLayout();
      this.tlpSettingsHeader.SuspendLayout();
      this.gp2.SuspendLayout();
      this.gp1.SuspendLayout();
      this.gp3_2.SuspendLayout();
      this.gp3.SuspendLayout();
      this.gp4.SuspendLayout();
      this.bpPersonal.SuspendLayout();
      this.gpPersonalConfig.SuspendLayout();
      this.SuspendLayout();
      this.pnlConfigurationBase.Controls.Add((Control) this.pnlCompanyMaintenance);
      this.pnlConfigurationBase.Controls.Add((Control) this.pnlCompanySettingBase);
      this.pnlConfigurationBase.Controls.Add((Control) this.gpCompanyCategory);
      this.pnlConfigurationBase.Dock = DockStyle.Fill;
      this.pnlConfigurationBase.Location = new Point(0, 0);
      this.pnlConfigurationBase.Margin = new Padding(4, 5, 4, 5);
      this.pnlConfigurationBase.Name = "pnlConfigurationBase";
      this.pnlConfigurationBase.Size = new Size(1308, 734);
      this.pnlConfigurationBase.TabIndex = 1;
      this.pnlCompanyMaintenance.BackColor = Color.FromArgb(176, 197, 219);
      this.pnlCompanyMaintenance.Controls.Add((Control) this.tvTP4);
      this.pnlCompanyMaintenance.Dock = DockStyle.Fill;
      this.pnlCompanyMaintenance.Location = new Point(996, 0);
      this.pnlCompanyMaintenance.Margin = new Padding(4, 5, 4, 5);
      this.pnlCompanyMaintenance.Name = "pnlCompanyMaintenance";
      this.pnlCompanyMaintenance.Size = new Size(312, 734);
      this.pnlCompanyMaintenance.TabIndex = 6;
      this.tvTP4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvTP4.BackColor = Color.FromArgb(176, 197, 219);
      this.tvTP4.BorderStyle = BorderStyle.None;
      this.tvTP4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvTP4.Location = new Point(15, 221);
      this.tvTP4.Margin = new Padding(4, 5, 4, 5);
      this.tvTP4.Name = "tvTP4";
      this.tvTP4.Scrollable = false;
      this.tvTP4.ShowLines = false;
      this.tvTP4.ShowPlusMinus = false;
      this.tvTP4.ShowRootLines = false;
      this.tvTP4.Size = new Size(281, 496);
      this.tvTP4.TabIndex = 2;
      this.tvTP4.BeforeCollapse += new TreeViewCancelEventHandler(this.tvTP4_BeforeCollapse);
      this.tvTP4.MouseDown += new MouseEventHandler(this.tvTP4_MouseDown);
      this.tvTP4.MouseMove += new MouseEventHandler(this.tvTP4_MouseMove);
      this.tvTP4.MouseUp += new MouseEventHandler(this.tvTP4_MouseUp);
      this.pnlCompanySettingBase.Controls.Add((Control) this.pnlTP3);
      this.pnlCompanySettingBase.Controls.Add((Control) this.pnlTP2);
      this.pnlCompanySettingBase.Controls.Add((Control) this.pnlTP1);
      this.pnlCompanySettingBase.Dock = DockStyle.Left;
      this.pnlCompanySettingBase.Location = new Point(51, 0);
      this.pnlCompanySettingBase.Margin = new Padding(4, 5, 4, 5);
      this.pnlCompanySettingBase.Name = "pnlCompanySettingBase";
      this.pnlCompanySettingBase.Size = new Size(945, 734);
      this.pnlCompanySettingBase.TabIndex = 1;
      this.pnlTP3.BackColor = Color.FromArgb(192, 210, 229);
      this.pnlTP3.Controls.Add((Control) this.tvTP3);
      this.pnlTP3.Dock = DockStyle.Fill;
      this.pnlTP3.Location = new Point(645, 0);
      this.pnlTP3.Margin = new Padding(4, 5, 4, 5);
      this.pnlTP3.Name = "pnlTP3";
      this.pnlTP3.Size = new Size(300, 734);
      this.pnlTP3.TabIndex = 4;
      this.tvTP3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvTP3.BackColor = Color.FromArgb(192, 210, 229);
      this.tvTP3.BorderStyle = BorderStyle.None;
      this.tvTP3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvTP3.Location = new Point(15, 221);
      this.tvTP3.Margin = new Padding(4, 5, 4, 5);
      this.tvTP3.Name = "tvTP3";
      this.tvTP3.Scrollable = false;
      this.tvTP3.ShowLines = false;
      this.tvTP3.ShowPlusMinus = false;
      this.tvTP3.ShowRootLines = false;
      this.tvTP3.Size = new Size(270, 496);
      this.tvTP3.TabIndex = 1;
      this.tvTP3.BeforeCollapse += new TreeViewCancelEventHandler(this.tvTP3_BeforeCollapse);
      this.tvTP3.MouseDown += new MouseEventHandler(this.tvTP3_MouseDown);
      this.tvTP3.MouseMove += new MouseEventHandler(this.tvTP3_MouseMove);
      this.tvTP3.MouseUp += new MouseEventHandler(this.tvTP3_MouseUp);
      this.pnlTP2.BackColor = Color.FromArgb(208, 222, 238);
      this.pnlTP2.Controls.Add((Control) this.tvTP2);
      this.pnlTP2.Dock = DockStyle.Left;
      this.pnlTP2.Location = new Point(345, 0);
      this.pnlTP2.Margin = new Padding(4, 5, 4, 5);
      this.pnlTP2.Name = "pnlTP2";
      this.pnlTP2.Size = new Size(300, 734);
      this.pnlTP2.TabIndex = 3;
      this.tvTP2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvTP2.BackColor = Color.FromArgb(208, 222, 238);
      this.tvTP2.BorderStyle = BorderStyle.None;
      this.tvTP2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvTP2.Location = new Point(15, 221);
      this.tvTP2.Margin = new Padding(4, 5, 4, 5);
      this.tvTP2.Name = "tvTP2";
      this.tvTP2.Scrollable = false;
      this.tvTP2.ShowLines = false;
      this.tvTP2.ShowPlusMinus = false;
      this.tvTP2.ShowRootLines = false;
      this.tvTP2.Size = new Size(270, 496);
      this.tvTP2.TabIndex = 1;
      this.tvTP2.BeforeCollapse += new TreeViewCancelEventHandler(this.tvTP2_BeforeCollapse);
      this.tvTP2.MouseDown += new MouseEventHandler(this.tvTP2_MouseDown);
      this.tvTP2.MouseMove += new MouseEventHandler(this.tvTP2_MouseMove);
      this.tvTP2.MouseUp += new MouseEventHandler(this.tvTP2_MouseUp);
      this.pnlTP1.BackColor = Color.FromArgb(221, 233, 249);
      this.pnlTP1.Controls.Add((Control) this.tvTP1);
      this.pnlTP1.Dock = DockStyle.Left;
      this.pnlTP1.Location = new Point(0, 0);
      this.pnlTP1.Margin = new Padding(4, 5, 4, 5);
      this.pnlTP1.Name = "pnlTP1";
      this.pnlTP1.Size = new Size(345, 734);
      this.pnlTP1.TabIndex = 2;
      this.tvTP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvTP1.BackColor = Color.FromArgb(221, 233, 249);
      this.tvTP1.BorderStyle = BorderStyle.None;
      this.tvTP1.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvTP1.Indent = 16;
      this.tvTP1.ItemHeight = 16;
      this.tvTP1.Location = new Point(15, 221);
      this.tvTP1.Margin = new Padding(4, 5, 4, 5);
      this.tvTP1.Name = "tvTP1";
      this.tvTP1.Scrollable = false;
      this.tvTP1.ShowLines = false;
      this.tvTP1.ShowPlusMinus = false;
      this.tvTP1.ShowRootLines = false;
      this.tvTP1.Size = new Size(315, 496);
      this.tvTP1.TabIndex = 1;
      this.tvTP1.BeforeCollapse += new TreeViewCancelEventHandler(this.tvTP1_BeforeCollapse);
      this.tvTP1.MouseDown += new MouseEventHandler(this.tvTP1_MouseDown);
      this.tvTP1.MouseMove += new MouseEventHandler(this.tvTP1_MouseMove);
      this.tvTP1.MouseUp += new MouseEventHandler(this.tvTP1_MouseUp);
      this.gpCompanyCategory.BackColor = Color.FromArgb(233, 242, (int) byte.MaxValue);
      this.gpCompanyCategory.BackgroundImage = (Image) Resources.settings_company;
      this.gpCompanyCategory.BackgroundImageLayout = ImageLayout.Center;
      this.gpCompanyCategory.Dock = DockStyle.Left;
      this.gpCompanyCategory.Location = new Point(0, 0);
      this.gpCompanyCategory.Margin = new Padding(4, 5, 4, 5);
      this.gpCompanyCategory.Name = "gpCompanyCategory";
      this.gpCompanyCategory.Size = new Size(51, 734);
      this.gpCompanyCategory.TabIndex = 7;
      this.imgList.ColorDepth = ColorDepth.Depth8Bit;
      this.imgList.ImageSize = new Size(16, 16);
      this.imgList.TransparentColor = Color.Transparent;
      this.borderBase.BackColor = Color.Transparent;
      this.borderBase.Borders = AnchorStyles.None;
      this.borderBase.Controls.Add((Control) this.pnlConfigurationBase);
      this.borderBase.Dock = DockStyle.Fill;
      this.borderBase.Location = new Point(1, 0);
      this.borderBase.Margin = new Padding(4, 5, 4, 5);
      this.borderBase.Name = "borderBase";
      this.borderBase.Size = new Size(1308, 734);
      this.borderBase.TabIndex = 1;
      this.bpBase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.bpBase.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.bpBase.Controls.Add((Control) this.tlpSettingsHeader);
      this.bpBase.Controls.Add((Control) this.borderBase);
      this.bpBase.Controls.Add((Control) this.bpPersonal);
      this.bpBase.Location = new Point(0, 0);
      this.bpBase.Margin = new Padding(4, 5, 4, 5);
      this.bpBase.Name = "bpBase";
      this.bpBase.Size = new Size(1310, 960);
      this.bpBase.TabIndex = 2;
      this.tlpSettingsHeader.AutoSize = true;
      this.tlpSettingsHeader.ColumnCount = 2;
      this.tlpSettingsHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 994f));
      this.tlpSettingsHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tlpSettingsHeader.Controls.Add((Control) this.gp2, 0, 2);
      this.tlpSettingsHeader.Controls.Add((Control) this.gp1, 0, 0);
      this.tlpSettingsHeader.Controls.Add((Control) this.gp4_2, 1, 4);
      this.tlpSettingsHeader.Controls.Add((Control) this.gp3_2, 1, 3);
      this.tlpSettingsHeader.Controls.Add((Control) this.gp3, 0, 3);
      this.tlpSettingsHeader.Controls.Add((Control) this.gp4, 0, 4);
      this.tlpSettingsHeader.Controls.Add((Control) this.llEncWebSettings, 0, 1);
      this.tlpSettingsHeader.Dock = DockStyle.Top;
      this.tlpSettingsHeader.Location = new Point(1, 0);
      this.tlpSettingsHeader.Margin = new Padding(3, 4, 3, 4);
      this.tlpSettingsHeader.Name = "tlpSettingsHeader";
      this.tlpSettingsHeader.RowCount = 5;
      this.tlpSettingsHeader.RowStyles.Add(new RowStyle());
      this.tlpSettingsHeader.RowStyles.Add(new RowStyle());
      this.tlpSettingsHeader.RowStyles.Add(new RowStyle());
      this.tlpSettingsHeader.RowStyles.Add(new RowStyle());
      this.tlpSettingsHeader.RowStyles.Add(new RowStyle());
      this.tlpSettingsHeader.Size = new Size(1308, 192);
      this.tlpSettingsHeader.TabIndex = 10;
      this.gp2.BackColor = Color.Transparent;
      this.gp2.Borders = AnchorStyles.None;
      this.tlpSettingsHeader.SetColumnSpan((Control) this.gp2, 2);
      this.gp2.Controls.Add((Control) this.lblEncDesktopSettings);
      this.gp2.Dock = DockStyle.Fill;
      this.gp2.GradientColor2 = Color.FromArgb(176, 197, 219);
      this.gp2.Location = new Point(0, 65);
      this.gp2.Margin = new Padding(0);
      this.gp2.Name = "gp2";
      this.gp2.Size = new Size(1308, 39);
      this.gp2.TabIndex = 8;
      this.lblEncDesktopSettings.BackColor = Color.Transparent;
      this.lblEncDesktopSettings.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEncDesktopSettings.ForeColor = Color.Black;
      this.lblEncDesktopSettings.Location = new Point(4, 9);
      this.lblEncDesktopSettings.Margin = new Padding(4, 0, 4, 0);
      this.lblEncDesktopSettings.Name = "lblEncDesktopSettings";
      this.lblEncDesktopSettings.Padding = new Padding(15, 0, 15, 0);
      this.lblEncDesktopSettings.Size = new Size(377, 27);
      this.lblEncDesktopSettings.TabIndex = 1;
      this.lblEncDesktopSettings.Text = "Encompass - Desktop Version Settings";
      this.lblEncDesktopSettings.TextAlign = ContentAlignment.MiddleLeft;
      this.gp1.BackColor = Color.Transparent;
      this.gp1.Borders = AnchorStyles.None;
      this.tlpSettingsHeader.SetColumnSpan((Control) this.gp1, 2);
      this.gp1.Controls.Add((Control) this.lblEncWebSettings);
      this.gp1.Dock = DockStyle.Fill;
      this.gp1.GradientColor2 = Color.FromArgb(176, 197, 219);
      this.gp1.Location = new Point(0, 0);
      this.gp1.Margin = new Padding(0);
      this.gp1.Name = "gp1";
      this.gp1.Size = new Size(1308, 44);
      this.gp1.TabIndex = 6;
      this.lblEncWebSettings.BackColor = Color.Transparent;
      this.lblEncWebSettings.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEncWebSettings.ForeColor = Color.Black;
      this.lblEncWebSettings.Location = new Point(4, 9);
      this.lblEncWebSettings.Margin = new Padding(4, 0, 4, 0);
      this.lblEncWebSettings.Name = "lblEncWebSettings";
      this.lblEncWebSettings.Padding = new Padding(15, 0, 15, 0);
      this.lblEncWebSettings.Size = new Size(332, 31);
      this.lblEncWebSettings.TabIndex = 1;
      this.lblEncWebSettings.Text = "Encompass - Web Version Settings";
      this.lblEncWebSettings.TextAlign = ContentAlignment.MiddleLeft;
      this.gp4_2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gp4_2.Dock = DockStyle.Fill;
      this.gp4_2.GradientColor1 = Color.WhiteSmoke;
      this.gp4_2.GradientColor2 = Color.FromArgb(200, 199, 199);
      this.gp4_2.Location = new Point(994, 144);
      this.gp4_2.Margin = new Padding(0);
      this.gp4_2.Name = "gp4_2";
      this.gp4_2.Size = new Size(314, 48);
      this.gp4_2.TabIndex = 5;
      this.gp3_2.BackColor = Color.Transparent;
      this.gp3_2.Borders = AnchorStyles.None;
      this.gp3_2.Controls.Add((Control) this.lblMaintenance);
      this.gp3_2.Dock = DockStyle.Fill;
      this.gp3_2.GradientColor1 = Color.FromArgb(207, 221, 243);
      this.gp3_2.GradientColor2 = Color.FromArgb(123, 156, 189);
      this.gp3_2.Location = new Point(994, 104);
      this.gp3_2.Margin = new Padding(0);
      this.gp3_2.Name = "gp3_2";
      this.gp3_2.Size = new Size(314, 40);
      this.gp3_2.TabIndex = 4;
      this.lblMaintenance.BackColor = Color.Transparent;
      this.lblMaintenance.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMaintenance.ForeColor = Color.Black;
      this.lblMaintenance.Location = new Point(0, 0);
      this.lblMaintenance.Margin = new Padding(4, 0, 4, 0);
      this.lblMaintenance.Name = "lblMaintenance";
      this.lblMaintenance.Padding = new Padding(15, 0, 15, 0);
      this.lblMaintenance.Size = new Size(215, 40);
      this.lblMaintenance.TabIndex = 0;
      this.lblMaintenance.Text = "Maintenance";
      this.lblMaintenance.TextAlign = ContentAlignment.MiddleLeft;
      this.gp3.BackColor = Color.Transparent;
      this.gp3.Borders = AnchorStyles.None;
      this.gp3.Controls.Add((Control) this.lblConfiguration);
      this.gp3.Dock = DockStyle.Fill;
      this.gp3.GradientColor2 = Color.FromArgb(176, 197, 219);
      this.gp3.Location = new Point(0, 104);
      this.gp3.Margin = new Padding(0);
      this.gp3.Name = "gp3";
      this.gp3.Size = new Size(994, 40);
      this.gp3.TabIndex = 3;
      this.lblConfiguration.BackColor = Color.Transparent;
      this.lblConfiguration.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblConfiguration.ForeColor = Color.Black;
      this.lblConfiguration.Location = new Point(51, 0);
      this.lblConfiguration.Margin = new Padding(4, 0, 4, 0);
      this.lblConfiguration.Name = "lblConfiguration";
      this.lblConfiguration.Padding = new Padding(15, 0, 15, 0);
      this.lblConfiguration.Size = new Size(591, 40);
      this.lblConfiguration.TabIndex = 1;
      this.lblConfiguration.Text = "Configuration";
      this.lblConfiguration.TextAlign = ContentAlignment.MiddleLeft;
      this.gp4.BackColor = Color.Transparent;
      this.gp4.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gp4.Controls.Add((Control) this.lblTP2);
      this.gp4.Controls.Add((Control) this.lblTP1);
      this.gp4.Dock = DockStyle.Fill;
      this.gp4.GradientColor1 = Color.WhiteSmoke;
      this.gp4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gp4.Location = new Point(0, 144);
      this.gp4.Margin = new Padding(0);
      this.gp4.Name = "gp4";
      this.gp4.Size = new Size(994, 48);
      this.gp4.TabIndex = 2;
      this.lblTP2.BackColor = Color.Transparent;
      this.lblTP2.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTP2.ForeColor = Color.Black;
      this.lblTP2.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblTP2.Location = new Point(511, 0);
      this.lblTP2.Margin = new Padding(4, 0, 4, 0);
      this.lblTP2.Name = "lblTP2";
      this.lblTP2.Padding = new Padding(15, 0, 15, 0);
      this.lblTP2.Size = new Size(443, 36);
      this.lblTP2.TabIndex = 0;
      this.lblTP2.TextAlign = ContentAlignment.MiddleCenter;
      this.lblTP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblTP1.BackColor = Color.Transparent;
      this.lblTP1.Font = new Font("Arial", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTP1.ForeColor = Color.Black;
      this.lblTP1.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblTP1.Location = new Point(46, -6);
      this.lblTP1.Margin = new Padding(4, 0, 4, 0);
      this.lblTP1.Name = "lblTP1";
      this.lblTP1.Padding = new Padding(15, 0, 15, 0);
      this.lblTP1.Size = new Size(345, 49);
      this.lblTP1.TabIndex = 0;
      this.lblTP1.TextAlign = ContentAlignment.MiddleLeft;
      this.llEncWebSettings.AutoSize = true;
      this.tlpSettingsHeader.SetColumnSpan((Control) this.llEncWebSettings, 2);
      this.llEncWebSettings.Location = new Point(3, 44);
      this.llEncWebSettings.Name = "llEncWebSettings";
      this.llEncWebSettings.Padding = new Padding(14, 8, 0, 0);
      this.llEncWebSettings.Size = new Size(123, 21);
      this.llEncWebSettings.TabIndex = 7;
      this.llEncWebSettings.TabStop = true;
      this.llEncWebSettings.Text = "Web Version Settings";
      this.llEncWebSettings.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llEncWebSettings_LinkClicked);
      this.bpPersonal.Borders = AnchorStyles.None;
      this.bpPersonal.Controls.Add((Control) this.gpPersonalConfig);
      this.bpPersonal.Controls.Add((Control) this.pnlPersonalCategory);
      this.bpPersonal.Dock = DockStyle.Bottom;
      this.bpPersonal.Location = new Point(1, 734);
      this.bpPersonal.Margin = new Padding(4, 5, 4, 5);
      this.bpPersonal.Name = "bpPersonal";
      this.bpPersonal.Size = new Size(1308, 225);
      this.bpPersonal.TabIndex = 0;
      this.gpPersonalConfig.Borders = AnchorStyles.None;
      this.gpPersonalConfig.Controls.Add((Control) this.tvBP1);
      this.gpPersonalConfig.Dock = DockStyle.Fill;
      this.gpPersonalConfig.GradientColor1 = Color.FromArgb(254, 213, 153);
      this.gpPersonalConfig.GradientColor2 = Color.FromArgb(254, 213, 153);
      this.gpPersonalConfig.GradientPaddingColor = Color.Transparent;
      this.gpPersonalConfig.Location = new Point(51, 0);
      this.gpPersonalConfig.Margin = new Padding(4, 5, 4, 5);
      this.gpPersonalConfig.Name = "gpPersonalConfig";
      this.gpPersonalConfig.Size = new Size(1257, 225);
      this.gpPersonalConfig.TabIndex = 1;
      this.tvBP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.tvBP1.BackColor = Color.FromArgb(254, 213, 153);
      this.tvBP1.BorderStyle = BorderStyle.None;
      this.tvBP1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvBP1.Location = new Point(15, 8);
      this.tvBP1.Margin = new Padding(4, 5, 4, 5);
      this.tvBP1.Name = "tvBP1";
      this.tvBP1.Scrollable = false;
      this.tvBP1.ShowLines = false;
      this.tvBP1.ShowPlusMinus = false;
      this.tvBP1.ShowRootLines = false;
      this.tvBP1.Size = new Size(270, 202);
      this.tvBP1.TabIndex = 100;
      this.tvBP1.BeforeCollapse += new TreeViewCancelEventHandler(this.tvBP1_BeforeCollapse);
      this.tvBP1.BeforeSelect += new TreeViewCancelEventHandler(this.tvBP1_BeforeSelect);
      this.tvBP1.MouseDown += new MouseEventHandler(this.tvBP1_MouseDown);
      this.tvBP1.MouseMove += new MouseEventHandler(this.tvBP1_MouseMove);
      this.tvBP1.MouseUp += new MouseEventHandler(this.tvBP1_MouseUp);
      this.pnlPersonalCategory.BackColor = Color.FromArgb((int) byte.MaxValue, 248, 221);
      this.pnlPersonalCategory.BackgroundImage = (Image) Resources.settings_personal;
      this.pnlPersonalCategory.BackgroundImageLayout = ImageLayout.Center;
      this.pnlPersonalCategory.Dock = DockStyle.Left;
      this.pnlPersonalCategory.Location = new Point(0, 0);
      this.pnlPersonalCategory.Margin = new Padding(4, 5, 4, 5);
      this.pnlPersonalCategory.Name = "pnlPersonalCategory";
      this.pnlPersonalCategory.Size = new Size(51, 225);
      this.pnlPersonalCategory.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1310, 962);
      this.Controls.Add((Control) this.bpBase);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (SettingsOverview);
      this.Text = nameof (SettingsOverview);
      this.SizeChanged += new EventHandler(this.SettingsOverview_SizeChanged);
      this.pnlConfigurationBase.ResumeLayout(false);
      this.pnlCompanyMaintenance.ResumeLayout(false);
      this.pnlCompanySettingBase.ResumeLayout(false);
      this.pnlTP3.ResumeLayout(false);
      this.pnlTP2.ResumeLayout(false);
      this.pnlTP1.ResumeLayout(false);
      this.borderBase.ResumeLayout(false);
      this.bpBase.ResumeLayout(false);
      this.bpBase.PerformLayout();
      this.tlpSettingsHeader.ResumeLayout(false);
      this.tlpSettingsHeader.PerformLayout();
      this.gp2.ResumeLayout(false);
      this.gp1.ResumeLayout(false);
      this.gp3_2.ResumeLayout(false);
      this.gp3.ResumeLayout(false);
      this.gp4.ResumeLayout(false);
      this.bpPersonal.ResumeLayout(false);
      this.gpPersonalConfig.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    internal static int AddTreeNode(TreeNode desParentNode, TreeNode sourceNode, Font nodeFont)
    {
      int num1 = 0;
      TreeNode treeNode = new TreeNode(sourceNode.Text);
      treeNode.NodeFont = nodeFont;
      treeNode.Tag = (object) sourceNode;
      desParentNode.Nodes.Add(treeNode);
      int num2 = num1 + 1;
      foreach (TreeNode node in sourceNode.Nodes)
        num2 += SettingsOverview.AddTreeNode(treeNode, node, nodeFont);
      return num2;
    }

    internal static int BuildTree(TreeView tv, TreeNode node, Font nodeFont)
    {
      int num1 = 0;
      TreeNode treeNode = new TreeNode(node.Text);
      treeNode.Tag = (object) node;
      tv.Nodes.Add(treeNode);
      int num2 = num1 + 1;
      foreach (TreeNode node1 in node.Nodes)
        num2 += SettingsOverview.AddTreeNode(treeNode, node1, nodeFont);
      return num2;
    }

    private void buildCompanySettings()
    {
      int[] numArray = new int[4];
      this.tvTP1.Nodes.Clear();
      this.tvTP2.Nodes.Clear();
      this.tvTP3.Nodes.Clear();
      this.tvTP4.Nodes.Clear();
      this.tvTP1.BeginUpdate();
      this.tvTP2.BeginUpdate();
      this.tvTP3.BeginUpdate();
      this.tvTP4.BeginUpdate();
      foreach (TreeNode node in this.sourceTV.Nodes)
      {
        switch (node.Text)
        {
          case "Additional Services":
          case "System Administration":
            numArray[3] += SettingsOverview.BuildTree(this.tvTP4, node, this.normalFont);
            continue;
          case "Business Rules":
          case "Dynamic Data Management":
            numArray[2] += SettingsOverview.BuildTree(this.tvTP3, node, this.normalFont);
            continue;
          case "Company/User Setup":
          case "Contact Setup":
          case "Docs Setup":
          case "External Company Setup":
          case "Investor Connect Setup":
          case "Loan Setup":
          case "Secondary Setup":
          case "eFolder Setup":
            numArray[0] += SettingsOverview.BuildTree(this.tvTP1, node, this.normalFont);
            continue;
          case "Loan Templates":
          case "Tables and Fees":
            numArray[1] += SettingsOverview.BuildTree(this.tvTP2, node, this.normalFont);
            continue;
          default:
            continue;
        }
      }
      this.tvTP1.ExpandAll();
      this.tvTP2.ExpandAll();
      this.tvTP3.ExpandAll();
      this.tvTP4.ExpandAll();
      this.tvTP4.EndUpdate();
      this.tvTP3.EndUpdate();
      this.tvTP2.EndUpdate();
      this.tvTP1.EndUpdate();
      if (this.tvTP1.Nodes.Count > 0)
        this.lblTP1.Text = "Build Environment";
      if (this.tvTP2.Nodes.Count > 0 || this.tvTP3.Nodes.Count > 0)
        this.lblTP2.Text = "Improve Productivity && Enhance Control";
      for (int index = 0; index < 4; ++index)
      {
        if (numArray[index] > this.MaxTopNodeNum)
          this.MaxTopNodeNum = numArray[index];
      }
    }

    private void buildPersonalSettings()
    {
      int[] numArray = new int[1];
      this.tvBP1.BeginUpdate();
      foreach (TreeNode node in this.sourceTV.Nodes)
      {
        if (node.Text == "Personal Settings")
          numArray[0] += SettingsOverview.BuildTree(this.tvBP1, node, this.normalFont);
      }
      this.tvBP1.ExpandAll();
      this.tvBP1.EndUpdate();
    }

    private void SettingsOverview_SizeChanged(object sender, EventArgs e)
    {
    }

    private void refreshPanel()
    {
      this.pnlCompanySettingBase.SuspendLayout();
      if (this.MaxTopNodeNum < 7)
        this.MaxTopNodeNum = 7;
      this.bpBase.Height = 52 + int.Parse(string.Concat((object) Math.Ceiling(((double) this.tvBP1.ItemHeight + 2.5) * (double) (this.tvBP1.Nodes[0].Nodes.Count + this.MaxTopNodeNum)))) + 65;
      int num1 = 0;
      int num2 = 0;
      if (this.tvTP3.Nodes.Count == 0 && this.tvTP2.Nodes.Count > 0)
        this.pnlTP2.Width += this.pnlTP3.Width;
      if (this.tvTP2.Nodes.Count == 0 && this.tvTP3.Nodes.Count > 0)
      {
        this.pnlTP3.Width += this.pnlTP2.Width;
        this.pnlTP3.Dock = this.pnlTP2.Dock;
        this.pnlTP3.Location = this.pnlTP2.Location;
      }
      if (this.tvTP3.Nodes.Count > 0)
        num2 += this.pnlTP3.Width;
      else
        this.pnlCompanySettingBase.Controls.Remove((Control) this.pnlTP3);
      if (this.tvTP2.Nodes.Count > 0)
        num2 += this.pnlTP2.Width;
      else
        this.pnlCompanySettingBase.Controls.Remove((Control) this.pnlTP2);
      if (this.tvTP1.Nodes.Count > 0 || this.tvBP1.Nodes.Count > 0)
        num2 += this.pnlTP1.Width;
      else
        this.pnlCompanySettingBase.Controls.Remove((Control) this.pnlTP1);
      int num3 = num1 + num2;
      this.pnlCompanySettingBase.Width = num2;
      this.pnlCompanySettingBase.ResumeLayout(true);
      this.SettingsOverview_SizeChanged((object) this, (EventArgs) null);
    }

    private bool HasPublicLoanTemplate()
    {
      return Session.AclGroupManager.CheckPublicAccessPermissionToAny(FeatureSets.FileTypes);
    }

    private bool HasPublicResource()
    {
      return Session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CustomPrintForms);
    }

    private void tvTP1_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP1.GetNodeAt(e.X, e.Y);
      this.tvTP1.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
      if (nodeAt != null)
      {
        if (this.currentDestinationNode == nodeAt)
          return;
        if (this.currentDestinationNode != null && this.currentDestinationNode.NodeFont != null)
          this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = nodeAt;
        if (nodeAt.NodeFont == null)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else if (nodeAt.NodeFont.Bold)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else
          nodeAt.NodeFont = this.normalUnderLineFont;
      }
      else
      {
        if (this.currentDestinationNode == null || this.currentDestinationNode.NodeFont == null)
          return;
        this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = (TreeNode) null;
      }
    }

    private void tvTP1_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvTP1_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP1.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvTP1_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tvTP2_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP2.GetNodeAt(e.X, e.Y);
      this.tvTP2.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
      if (nodeAt != null)
      {
        if (this.currentDestinationNode == nodeAt)
          return;
        if (this.currentDestinationNode != null && this.currentDestinationNode.NodeFont != null)
          this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = nodeAt;
        if (nodeAt.NodeFont == null)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else if (nodeAt.NodeFont.Bold)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else
          nodeAt.NodeFont = this.normalUnderLineFont;
      }
      else
      {
        if (this.currentDestinationNode == null || this.currentDestinationNode.NodeFont == null)
          return;
        this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = (TreeNode) null;
      }
    }

    private void tvTP2_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvTP2_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP2.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvTP2_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tvTP3_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP3.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvTP3_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP3.GetNodeAt(e.X, e.Y);
      this.tvTP3.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
      if (nodeAt != null)
      {
        if (this.currentDestinationNode == nodeAt)
          return;
        if (this.currentDestinationNode != null && this.currentDestinationNode.NodeFont != null)
          this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = nodeAt;
        if (nodeAt.NodeFont == null)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else if (nodeAt.NodeFont.Bold)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else
          nodeAt.NodeFont = this.normalUnderLineFont;
      }
      else
      {
        if (this.currentDestinationNode == null || this.currentDestinationNode.NodeFont == null)
          return;
        this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = (TreeNode) null;
      }
    }

    private void tvTP3_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvTP3_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tvTP4_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tvTP4_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP4.GetNodeAt(e.X, e.Y);
      this.tvTP4.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
      if (nodeAt != null)
      {
        if (this.currentDestinationNode == nodeAt)
          return;
        if (this.currentDestinationNode != null && this.currentDestinationNode.NodeFont != null)
          this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = nodeAt;
        if (nodeAt.NodeFont == null)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else if (nodeAt.NodeFont.Bold)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else
          nodeAt.NodeFont = this.normalUnderLineFont;
      }
      else
      {
        if (this.currentDestinationNode == null || this.currentDestinationNode.NodeFont == null)
          return;
        this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = (TreeNode) null;
      }
    }

    private void tvTP4_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvTP4_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvTP4.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvBP1_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tvBP1_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvBP1.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvBP1_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvBP1.GetNodeAt(e.X, e.Y);
      this.tvBP1.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
      if (nodeAt != null)
      {
        if (this.currentDestinationNode == nodeAt)
          return;
        if (this.currentDestinationNode != null && this.currentDestinationNode.NodeFont != null)
          this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = nodeAt;
        if (nodeAt.NodeFont == null)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else if (nodeAt.NodeFont.Bold)
          nodeAt.NodeFont = this.boldUnderLineFont;
        else
          nodeAt.NodeFont = this.normalUnderLineFont;
      }
      else
      {
        if (this.currentDestinationNode == null || this.currentDestinationNode.NodeFont == null)
          return;
        this.currentDestinationNode.NodeFont = !this.currentDestinationNode.NodeFont.Bold ? this.normalFont : this.boldFont;
        this.currentDestinationNode = (TreeNode) null;
      }
    }

    private void tvBP1_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvBP1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      e.Cancel = true;
      this.lblEncWebSettings.Focus();
    }

    private void llEncWebSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      string scope = "loc";
      string webPageURL = Session.DefaultInstance.StartupInfo.LOConnectUrl + "/sc/admin";
      webPageParams.Add("hostname", (object) "smartclient");
      webPageParams.Add("instanceId", (object) Session.Connection?.Server?.InstanceName);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm loadWebPageForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Encompass - Web Version Settings");
      loadWebPageForm.Height = Convert.ToInt32((double) this.Height * 0.9);
      loadWebPageForm.Width = Convert.ToInt32((double) this.Width * 0.9);
      loadWebPageForm.Show((IWin32Window) this);
    }
  }
}
