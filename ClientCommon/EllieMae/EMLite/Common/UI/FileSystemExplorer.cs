// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FileSystemExplorer
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FileSystemExplorer : UserControl
  {
    public const string className = "FileSystemExplorer";
    public static string sw = Tracing.SwCommon;
    public static readonly FileSystemEntry PublicPrivateRoot = new FileSystemEntry("\\", FileSystemEntry.Types.Folder, "@root");
    private Sessions.Session session;
    private IFileSystem fileSystem;
    private FileSystemEntry publicRoot;
    private FileSystemEntry privateRoot;
    private bool displayFoldersOnly;
    private FileSystemEntry currentFolder;
    private int explorerWidth;
    private Dictionary<FileFolderAction, bool> actionVisibleStates = new Dictionary<FileFolderAction, bool>();
    private Dictionary<FileFolderAction, bool> actionEnabledStates = new Dictionary<FileFolderAction, bool>();
    private bool actionBarVisibleState = true;
    private bool isCut;
    private FileSystemEntry[] cutCopyEntries;
    private TextBoxFormatter editorFormatter;
    private bool suspendEvents;
    private Point contextMenuPosition = Point.Empty;
    private List<Control> actionControls;
    private IContainer components;
    private GroupContainer gcListView;
    private StandardIconButton btnAdd;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNewFolder;
    private StandardIconButton btnOpen;
    private StandardIconButton btnPaste;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnCopy;
    private StandardIconButton btnCut;
    private Button btnExport;
    private Button btnDeploy;
    private Button btnRename;
    private Button btnImport;
    private StandardIconButton btnUpFolder;
    private Label lblFolder;
    private ComboBoxEx cboFolder;
    private ContextMenu ctxFileFolderMenu;
    private ToolTip tipFSExplorer;
    private FlowLayoutPanel flpButtons;
    private VerticalSeparator vsButtonSeparator;
    private MenuItem menuItemDeploy;
    private MenuItem menuItemExport;
    private MenuItem menuItemPaste;
    private MenuItem menuItemRefresh;
    private MenuItem menuItemOpen;
    private MenuItem menuItemCopy;
    private MenuItem menuItemCut;
    private MenuItem menuItemDelete;
    private MenuItem menuItemRename;
    private MenuItem menuItemCreateFolder;
    private MenuItem menuItemNew;
    private MenuItem menuItemImport;
    private MenuItem menuItemDuplicate;
    private MenuItem menuItemSetAsDefault;
    private GridView gvDirectory;
    private ImageList imgsListView;
    private Button btnSetAsDefault;
    private GradientPanel pnlDescription;
    private Label lblPurchaseAdvice;
    private GradientPanel pnlFolder;

    [Category("Behavior")]
    public event EventHandler FolderChanged;

    [Category("Behavior")]
    public event EventHandler SelectedEntryChanged;

    [Category("Behavior")]
    public event CancelableFileSystemEventHandler BeforeEntryRenamed;

    [Category("Behavior")]
    public event FileSystemEventHandler AfterEntryRenamed;

    [Category("Behavior")]
    public event FileSystemEventHandler BeforeEntryDeleted;

    [Category("Behavior")]
    public event FileSystemEventHandler AfterEntryDeleted;

    [Category("Behavior")]
    public event EventHandler FolderContentsChanged;

    [Category("Behavior")]
    public event EventHandler SetAsDefaultButtonClick;

    [Category("Behavior")]
    public event FileSystemEventHandler EntryDoubleClick;

    [Category("Data")]
    public event ExplorerListItemEventHandler PopulateListItem;

    public FileSystemExplorer()
    {
      this.InitializeComponent();
      this.initializeInstanceData();
    }

    private void initializeInstanceData()
    {
      GVColumn column = this.gvDirectory.Columns[0];
      column.Tag = (object) new ExplorerColumn(column)
      {
        Width = Convert.ToInt32(0.9 * (double) this.gvDirectory.ClientRectangle.Width)
      };
      this.explorerWidth = this.gvDirectory.ClientRectangle.Width;
      this.actionControls = new List<Control>();
      this.actionControls.Add((Control) this.btnAdd);
      this.actionControls.Add((Control) this.btnCut);
      this.actionControls.Add((Control) this.btnCopy);
      this.actionControls.Add((Control) this.btnDuplicate);
      this.actionControls.Add((Control) this.btnPaste);
      this.actionControls.Add((Control) this.btnOpen);
      this.actionControls.Add((Control) this.btnNewFolder);
      this.actionControls.Add((Control) this.btnDelete);
      this.actionControls.Add((Control) this.btnRename);
      this.actionControls.Add((Control) this.btnDeploy);
      this.actionControls.Add((Control) this.btnImport);
      this.actionControls.Add((Control) this.btnExport);
      this.actionControls.Add((Control) this.btnSetAsDefault);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FileSystemExplorer));
      GVColumn gvColumn = new GVColumn();
      this.ctxFileFolderMenu = new ContextMenu();
      this.menuItemNew = new MenuItem();
      this.menuItemDuplicate = new MenuItem();
      this.menuItemCut = new MenuItem();
      this.menuItemCopy = new MenuItem();
      this.menuItemPaste = new MenuItem();
      this.menuItemOpen = new MenuItem();
      this.menuItemCreateFolder = new MenuItem();
      this.menuItemDelete = new MenuItem();
      this.menuItemRename = new MenuItem();
      this.menuItemDeploy = new MenuItem();
      this.menuItemImport = new MenuItem();
      this.menuItemExport = new MenuItem();
      this.menuItemSetAsDefault = new MenuItem();
      this.menuItemRefresh = new MenuItem();
      this.imgsListView = new ImageList(this.components);
      this.lblFolder = new Label();
      this.cboFolder = new ComboBoxEx();
      this.pnlFolder = new GradientPanel();
      this.btnUpFolder = new StandardIconButton();
      this.tipFSExplorer = new ToolTip(this.components);
      this.btnDelete = new StandardIconButton();
      this.btnNewFolder = new StandardIconButton();
      this.btnOpen = new StandardIconButton();
      this.btnPaste = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnCopy = new StandardIconButton();
      this.btnCut = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gcListView = new GroupContainer();
      this.flpButtons = new FlowLayoutPanel();
      this.btnSetAsDefault = new Button();
      this.btnExport = new Button();
      this.btnImport = new Button();
      this.btnDeploy = new Button();
      this.btnRename = new Button();
      this.vsButtonSeparator = new VerticalSeparator();
      this.gvDirectory = new GridView();
      this.pnlDescription = new GradientPanel();
      this.lblPurchaseAdvice = new Label();
      this.pnlFolder.SuspendLayout();
      ((ISupportInitialize) this.btnUpFolder).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNewFolder).BeginInit();
      ((ISupportInitialize) this.btnOpen).BeginInit();
      ((ISupportInitialize) this.btnPaste).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnCopy).BeginInit();
      ((ISupportInitialize) this.btnCut).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.gcListView.SuspendLayout();
      this.flpButtons.SuspendLayout();
      this.pnlDescription.SuspendLayout();
      this.SuspendLayout();
      this.ctxFileFolderMenu.MenuItems.AddRange(new MenuItem[14]
      {
        this.menuItemNew,
        this.menuItemDuplicate,
        this.menuItemCut,
        this.menuItemCopy,
        this.menuItemPaste,
        this.menuItemOpen,
        this.menuItemCreateFolder,
        this.menuItemDelete,
        this.menuItemRename,
        this.menuItemDeploy,
        this.menuItemImport,
        this.menuItemExport,
        this.menuItemSetAsDefault,
        this.menuItemRefresh
      });
      this.ctxFileFolderMenu.Popup += new EventHandler(this.ctxFileFolderMenu_Popup);
      this.menuItemNew.Index = 0;
      this.menuItemNew.Tag = (object) "AddNewFile";
      this.menuItemNew.Text = "Add";
      this.menuItemNew.Click += new EventHandler(this.newFileEventHandler);
      this.menuItemDuplicate.Index = 1;
      this.menuItemDuplicate.Tag = (object) "DuplicateFile";
      this.menuItemDuplicate.Text = "Duplicate";
      this.menuItemDuplicate.Click += new EventHandler(this.duplicateItemEventHandler);
      this.menuItemCut.Index = 2;
      this.menuItemCut.Tag = (object) "CutFolderOrFile";
      this.menuItemCut.Text = "Cut";
      this.menuItemCut.Click += new EventHandler(this.cutItemEventHandler);
      this.menuItemCopy.Index = 3;
      this.menuItemCopy.Tag = (object) "CopyFolderOrFile";
      this.menuItemCopy.Text = "Copy";
      this.menuItemCopy.Click += new EventHandler(this.copyItemEventHandler);
      this.menuItemPaste.Index = 4;
      this.menuItemPaste.Tag = (object) "PasteFolderOrFile";
      this.menuItemPaste.Text = "Paste";
      this.menuItemPaste.Click += new EventHandler(this.pasteItemEventHandler);
      this.menuItemOpen.Index = 5;
      this.menuItemOpen.Tag = (object) "EditFile";
      this.menuItemOpen.Text = "Edit";
      this.menuItemOpen.Click += new EventHandler(this.editFileEventHandler);
      this.menuItemCreateFolder.Index = 6;
      this.menuItemCreateFolder.Tag = (object) "CreateFolder";
      this.menuItemCreateFolder.Text = "New Folder";
      this.menuItemCreateFolder.Click += new EventHandler(this.createFolderEventHandler);
      this.menuItemDelete.Index = 7;
      this.menuItemDelete.Tag = (object) "DeleteFolderOrFile";
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.deleteItemEventHandler);
      this.menuItemRename.Index = 8;
      this.menuItemRename.Tag = (object) "RenameFolderOrFile";
      this.menuItemRename.Text = "Rename";
      this.menuItemRename.Click += new EventHandler(this.renameItemEventHandler);
      this.menuItemDeploy.Index = 9;
      this.menuItemDeploy.Tag = (object) "DeployFile";
      this.menuItemDeploy.Text = "Deploy";
      this.menuItemDeploy.Visible = false;
      this.menuItemDeploy.Click += new EventHandler(this.deployFileEventHandler);
      this.menuItemImport.Index = 10;
      this.menuItemImport.Tag = (object) "ImportFile";
      this.menuItemImport.Text = "Import";
      this.menuItemImport.Click += new EventHandler(this.importItemEventHandler);
      this.menuItemExport.Index = 11;
      this.menuItemExport.Tag = (object) "ExportFile";
      this.menuItemExport.Text = "Export";
      this.menuItemExport.Click += new EventHandler(this.exportFileEventHandler);
      this.menuItemSetAsDefault.Index = 12;
      this.menuItemSetAsDefault.Tag = (object) "SetAsDefault";
      this.menuItemSetAsDefault.Text = "Set as Default";
      this.menuItemSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.menuItemRefresh.Index = 13;
      this.menuItemRefresh.Tag = (object) "RefreshFolder";
      this.menuItemRefresh.Text = "Refresh";
      this.menuItemRefresh.Click += new EventHandler(this.refreshFolderEventHandler);
      this.imgsListView.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgsListView.ImageStream");
      this.imgsListView.TransparentColor = Color.Transparent;
      this.imgsListView.Images.SetKeyName(0, "share-folder");
      this.imgsListView.Images.SetKeyName(1, "file");
      this.imgsListView.Images.SetKeyName(2, "folder");
      this.lblFolder.AutoSize = true;
      this.lblFolder.BackColor = Color.Transparent;
      this.lblFolder.Location = new Point(6, 9);
      this.lblFolder.Name = "lblFolder";
      this.lblFolder.Size = new Size(37, 14);
      this.lblFolder.TabIndex = 1;
      this.lblFolder.Text = "Folder";
      this.lblFolder.TextAlign = ContentAlignment.MiddleLeft;
      this.cboFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFolder.Location = new Point(45, 5);
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.SelectedBGColor = SystemColors.Highlight;
      this.cboFolder.Size = new Size(654, 21);
      this.cboFolder.TabIndex = 0;
      this.cboFolder.SelectedIndexChanged += new EventHandler(this.cboFolder_SelectedIndexChanged);
      this.cboFolder.KeyDown += new KeyEventHandler(this.cboFolder_KeyDown);
      this.pnlFolder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFolder.Controls.Add((Control) this.btnUpFolder);
      this.pnlFolder.Controls.Add((Control) this.lblFolder);
      this.pnlFolder.Controls.Add((Control) this.cboFolder);
      this.pnlFolder.Dock = DockStyle.Top;
      this.pnlFolder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlFolder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlFolder.Location = new Point(0, 0);
      this.pnlFolder.Name = "pnlFolder";
      this.pnlFolder.Size = new Size(733, 31);
      this.pnlFolder.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlFolder.TabIndex = 16;
      this.btnUpFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpFolder.BackColor = Color.Transparent;
      this.btnUpFolder.Location = new Point(705, 7);
      this.btnUpFolder.MouseDownImage = (Image) null;
      this.btnUpFolder.Name = "btnUpFolder";
      this.btnUpFolder.Size = new Size(16, 16);
      this.btnUpFolder.StandardButtonType = StandardIconButton.ButtonType.UpFolderButton;
      this.btnUpFolder.TabIndex = 2;
      this.btnUpFolder.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnUpFolder, "Up One Level");
      this.btnUpFolder.Click += new EventHandler(this.btnUpFolder_Click);
      this.tipFSExplorer.ShowAlways = true;
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(150, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 26;
      this.btnDelete.TabStop = false;
      this.btnDelete.Tag = (object) "DeleteFolderOrFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.buttons_Click);
      this.btnNewFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewFolder.BackColor = Color.Transparent;
      this.btnNewFolder.Location = new Point(129, 3);
      this.btnNewFolder.Margin = new Padding(3, 3, 2, 3);
      this.btnNewFolder.MouseDownImage = (Image) null;
      this.btnNewFolder.Name = "btnNewFolder";
      this.btnNewFolder.Size = new Size(16, 16);
      this.btnNewFolder.StandardButtonType = StandardIconButton.ButtonType.NewFolderButton;
      this.btnNewFolder.TabIndex = 25;
      this.btnNewFolder.TabStop = false;
      this.btnNewFolder.Tag = (object) "CreateFolder";
      this.btnNewFolder.Text = "&New Folder";
      this.tipFSExplorer.SetToolTip((Control) this.btnNewFolder, "New Folder");
      this.btnNewFolder.Click += new EventHandler(this.buttons_Click);
      this.btnOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOpen.BackColor = Color.Transparent;
      this.btnOpen.Location = new Point(108, 3);
      this.btnOpen.Margin = new Padding(3, 3, 2, 3);
      this.btnOpen.MouseDownImage = (Image) null;
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new Size(16, 16);
      this.btnOpen.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnOpen.TabIndex = 24;
      this.btnOpen.TabStop = false;
      this.btnOpen.Tag = (object) "EditFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnOpen, "Edit");
      this.btnOpen.Click += new EventHandler(this.buttons_Click);
      this.btnPaste.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPaste.BackColor = Color.Transparent;
      this.btnPaste.Location = new Point(87, 3);
      this.btnPaste.Margin = new Padding(3, 3, 2, 3);
      this.btnPaste.MouseDownImage = (Image) null;
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new Size(16, 16);
      this.btnPaste.StandardButtonType = StandardIconButton.ButtonType.PasteButton;
      this.btnPaste.TabIndex = 23;
      this.btnPaste.TabStop = false;
      this.btnPaste.Tag = (object) "PasteFolderOrFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnPaste, "Paste");
      this.btnPaste.Click += new EventHandler(this.buttons_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(24, 3);
      this.btnDuplicate.Margin = new Padding(3, 3, 2, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 22;
      this.btnDuplicate.TabStop = false;
      this.btnDuplicate.Tag = (object) "DuplicateFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnDuplicate, "Duplicate");
      this.btnDuplicate.Click += new EventHandler(this.buttons_Click);
      this.btnCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCopy.BackColor = Color.Transparent;
      this.btnCopy.Location = new Point(66, 3);
      this.btnCopy.Margin = new Padding(3, 3, 2, 3);
      this.btnCopy.MouseDownImage = (Image) null;
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(16, 16);
      this.btnCopy.StandardButtonType = StandardIconButton.ButtonType.CopyButton;
      this.btnCopy.TabIndex = 21;
      this.btnCopy.TabStop = false;
      this.btnCopy.Tag = (object) "CopyFolderOrFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnCopy, "Copy");
      this.btnCopy.Click += new EventHandler(this.buttons_Click);
      this.btnCut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCut.BackColor = Color.Transparent;
      this.btnCut.Location = new Point(45, 3);
      this.btnCut.Margin = new Padding(3, 3, 2, 3);
      this.btnCut.MouseDownImage = (Image) null;
      this.btnCut.Name = "btnCut";
      this.btnCut.Size = new Size(16, 16);
      this.btnCut.StandardButtonType = StandardIconButton.ButtonType.CutButton;
      this.btnCut.TabIndex = 20;
      this.btnCut.TabStop = false;
      this.btnCut.Tag = (object) "CutFolderOrFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnCut, "Cut");
      this.btnCut.Click += new EventHandler(this.buttons_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(3, 3);
      this.btnAdd.Margin = new Padding(3, 3, 2, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 19;
      this.btnAdd.TabStop = false;
      this.btnAdd.Tag = (object) "AddNewFile";
      this.tipFSExplorer.SetToolTip((Control) this.btnAdd, "New");
      this.btnAdd.Click += new EventHandler(this.buttons_Click);
      this.gcListView.Controls.Add((Control) this.flpButtons);
      this.gcListView.Controls.Add((Control) this.gvDirectory);
      this.gcListView.Controls.Add((Control) this.pnlDescription);
      this.gcListView.Dock = DockStyle.Fill;
      this.gcListView.HeaderForeColor = SystemColors.ControlText;
      this.gcListView.Location = new Point(0, 31);
      this.gcListView.Name = "gcListView";
      this.gcListView.Size = new Size(733, 451);
      this.gcListView.TabIndex = 19;
      this.gcListView.Text = "<title>";
      this.flpButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpButtons.AutoSize = true;
      this.flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpButtons.BackColor = Color.Transparent;
      this.flpButtons.Controls.Add((Control) this.btnSetAsDefault);
      this.flpButtons.Controls.Add((Control) this.btnExport);
      this.flpButtons.Controls.Add((Control) this.btnImport);
      this.flpButtons.Controls.Add((Control) this.btnDeploy);
      this.flpButtons.Controls.Add((Control) this.btnRename);
      this.flpButtons.Controls.Add((Control) this.vsButtonSeparator);
      this.flpButtons.Controls.Add((Control) this.btnDelete);
      this.flpButtons.Controls.Add((Control) this.btnNewFolder);
      this.flpButtons.Controls.Add((Control) this.btnOpen);
      this.flpButtons.Controls.Add((Control) this.btnPaste);
      this.flpButtons.Controls.Add((Control) this.btnCopy);
      this.flpButtons.Controls.Add((Control) this.btnCut);
      this.flpButtons.Controls.Add((Control) this.btnDuplicate);
      this.flpButtons.Controls.Add((Control) this.btnAdd);
      this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
      this.flpButtons.Location = new Point(216, 2);
      this.flpButtons.Name = "flpButtons";
      this.flpButtons.Size = new Size(512, 22);
      this.flpButtons.TabIndex = 30;
      this.btnSetAsDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSetAsDefault.BackColor = SystemColors.Control;
      this.btnSetAsDefault.Location = new Point(424, 0);
      this.btnSetAsDefault.Margin = new Padding(0);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Size = new Size(88, 22);
      this.btnSetAsDefault.TabIndex = 28;
      this.btnSetAsDefault.Tag = (object) "SetAsDefault";
      this.btnSetAsDefault.Text = "Set as Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = SystemColors.Control;
      this.btnExport.Location = new Point(362, 0);
      this.btnExport.Margin = new Padding(0);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(62, 22);
      this.btnExport.TabIndex = 15;
      this.btnExport.Tag = (object) "ExportFile";
      this.btnExport.Text = "&Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.buttons_Click);
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.BackColor = SystemColors.Control;
      this.btnImport.Location = new Point(300, 0);
      this.btnImport.Margin = new Padding(0);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(62, 22);
      this.btnImport.TabIndex = 11;
      this.btnImport.Tag = (object) "ImportFile";
      this.btnImport.Text = "&Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.buttons_Click);
      this.btnDeploy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeploy.BackColor = SystemColors.Control;
      this.btnDeploy.Location = new Point(238, 0);
      this.btnDeploy.Margin = new Padding(0);
      this.btnDeploy.Name = "btnDeploy";
      this.btnDeploy.Size = new Size(62, 22);
      this.btnDeploy.TabIndex = 14;
      this.btnDeploy.Tag = (object) "DeployFile";
      this.btnDeploy.Text = "Deploy";
      this.btnDeploy.UseVisualStyleBackColor = true;
      this.btnDeploy.Click += new EventHandler(this.buttons_Click);
      this.btnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRename.BackColor = SystemColors.Control;
      this.btnRename.Location = new Point(176, 0);
      this.btnRename.Margin = new Padding(0);
      this.btnRename.Name = "btnRename";
      this.btnRename.Size = new Size(62, 22);
      this.btnRename.TabIndex = 9;
      this.btnRename.Tag = (object) "RenameFolderOrFile";
      this.btnRename.Text = "Rena&me";
      this.btnRename.UseVisualStyleBackColor = true;
      this.btnRename.Click += new EventHandler(this.buttons_Click);
      this.vsButtonSeparator.Location = new Point(171, 3);
      this.vsButtonSeparator.MaximumSize = new Size(2, 16);
      this.vsButtonSeparator.MinimumSize = new Size(2, 16);
      this.vsButtonSeparator.Name = "vsButtonSeparator";
      this.vsButtonSeparator.Size = new Size(2, 16);
      this.vsButtonSeparator.TabIndex = 29;
      this.vsButtonSeparator.Text = "verticalSeparator1";
      this.gvDirectory.AllowDrop = true;
      this.gvDirectory.BorderStyle = BorderStyle.None;
      gvColumn.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Name";
      gvColumn.Width = 280;
      this.gvDirectory.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvDirectory.ContextMenu = this.ctxFileFolderMenu;
      this.gvDirectory.Dock = DockStyle.Fill;
      this.gvDirectory.ImageList = this.imgsListView;
      this.gvDirectory.Location = new Point(1, 57);
      this.gvDirectory.Name = "gvDirectory";
      this.gvDirectory.Size = new Size(731, 393);
      this.gvDirectory.TabIndex = 2;
      this.gvDirectory.UseCompatibleEditingBehavior = true;
      this.gvDirectory.Resize += new EventHandler(this.gvDirectory_Resize);
      this.gvDirectory.EditorOpening += new GVSubItemEditingEventHandler(this.gvDirectory_EditorOpening);
      this.gvDirectory.SelectedIndexChanged += new EventHandler(this.gvDirectory_SelectedIndexChanged);
      this.gvDirectory.MouseDown += new MouseEventHandler(this.gvDirectory_MouseDown);
      this.gvDirectory.KeyPress += new KeyPressEventHandler(this.gvDirectory_KeyPress);
      this.gvDirectory.KeyUp += new KeyEventHandler(this.gvDirectory_KeyUp);
      this.gvDirectory.EditorClosing += new GVSubItemEditingEventHandler(this.gvDirectory_EditorClosing);
      this.gvDirectory.ItemDoubleClick += new GVItemEventHandler(this.gvDirectory_ItemDoubleClick);
      this.pnlDescription.Borders = AnchorStyles.Bottom;
      this.pnlDescription.Controls.Add((Control) this.lblPurchaseAdvice);
      this.pnlDescription.Dock = DockStyle.Top;
      this.pnlDescription.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlDescription.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlDescription.Location = new Point(1, 26);
      this.pnlDescription.Name = "pnlDescription";
      this.pnlDescription.Size = new Size(731, 31);
      this.pnlDescription.TabIndex = 29;
      this.pnlDescription.Visible = false;
      this.lblPurchaseAdvice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblPurchaseAdvice.BackColor = Color.Transparent;
      this.lblPurchaseAdvice.Location = new Point(11, 3);
      this.lblPurchaseAdvice.Name = "lblPurchaseAdvice";
      this.lblPurchaseAdvice.Size = new Size(709, 24);
      this.lblPurchaseAdvice.TabIndex = 0;
      this.lblPurchaseAdvice.Text = "Create and edit templates that populate payout descriptions and expected payout amounts on the Purchase Advice Form.";
      this.Controls.Add((Control) this.gcListView);
      this.Controls.Add((Control) this.pnlFolder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (FileSystemExplorer);
      this.Size = new Size(733, 482);
      this.pnlFolder.ResumeLayout(false);
      this.pnlFolder.PerformLayout();
      ((ISupportInitialize) this.btnUpFolder).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNewFolder).EndInit();
      ((ISupportInitialize) this.btnOpen).EndInit();
      ((ISupportInitialize) this.btnPaste).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnCopy).EndInit();
      ((ISupportInitialize) this.btnCut).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.gcListView.ResumeLayout(false);
      this.gcListView.PerformLayout();
      this.flpButtons.ResumeLayout(false);
      this.pnlDescription.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    [Category("Appearance")]
    public string Title
    {
      get => this.gcListView.Text;
      set => this.gcListView.Text = value;
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool AllowMultiselect
    {
      get => this.gvDirectory.AllowMultiselect;
      set => this.gvDirectory.AllowMultiselect = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FileSystemEntry CurrentFolder
    {
      get => this.currentFolder;
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.setCurrentFolder(value);
      }
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DisplayFoldersOnly
    {
      get => this.displayFoldersOnly;
      set => this.displayFoldersOnly = value;
    }

    [Category("Appearance")]
    [DefaultValue(true)]
    public bool ActionBarVisible
    {
      get => this.flpButtons.Visible;
      set
      {
        this.flpButtons.Visible = value;
        this.actionBarVisibleState = value;
      }
    }

    public Sessions.Session GetSession() => this.session;

    public void SetSession(Sessions.Session session) => this.session = session;

    public virtual void SetActionEnabled(FileFolderAction action, bool enabled)
    {
      Control buttonForAction = this.getButtonForAction(action);
      if (buttonForAction != null)
        buttonForAction.Enabled = enabled;
      MenuItem menuItemForAction = this.getMenuItemForAction(action);
      if (menuItemForAction != null)
        menuItemForAction.Enabled = enabled;
      this.actionEnabledStates[action] = enabled;
    }

    public virtual void EnableAction(FileFolderAction action)
    {
      this.SetActionEnabled(action, true);
    }

    public virtual void DisableAction(FileFolderAction action)
    {
      this.SetActionEnabled(action, false);
    }

    public virtual void DisableAllActions(bool disableRefresh)
    {
      foreach (FileFolderAction action in Enum.GetValues(typeof (FileFolderAction)))
      {
        if (action != FileFolderAction.None && (disableRefresh || action != FileFolderAction.RefreshFolder))
          this.DisableAction(action);
      }
    }

    public virtual void SetActionVisible(FileFolderAction action, bool visible)
    {
      Control buttonForAction = this.getButtonForAction(action);
      if (buttonForAction != null)
        buttonForAction.Visible = visible;
      MenuItem menuItemForAction = this.getMenuItemForAction(action);
      if (menuItemForAction != null)
        menuItemForAction.Visible = visible;
      this.actionVisibleStates[action] = visible;
      this.showHideButtonSeperator();
    }

    public virtual void HideAction(FileFolderAction action) => this.SetActionVisible(action, false);

    public virtual void HideAllActions(bool hideRefresh)
    {
      foreach (FileFolderAction action in Enum.GetValues(typeof (FileFolderAction)))
      {
        if (action != FileFolderAction.None && (hideRefresh || action != FileFolderAction.RefreshFolder))
          this.HideAction(action);
      }
    }

    public virtual void ShowAction(FileFolderAction action) => this.SetActionVisible(action, true);

    public virtual void ShowActions(params FileFolderAction[] actions)
    {
      this.HideAllActions(true);
      foreach (FileFolderAction action in actions)
        this.SetActionVisible(action, true);
      this.showHideButtonSeperator();
    }

    private void showHideButtonSeperator()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      for (int index = 0; index < this.flpButtons.Controls.Count; ++index)
      {
        Control control = this.flpButtons.Controls[index];
        FileFolderAction buttonAction = this.getButtonAction(control);
        if (control == this.vsButtonSeparator)
          flag3 = true;
        else if (this.IsActionVisible(buttonAction) & flag3)
          flag1 = true;
        else if (this.IsActionVisible(buttonAction) && !flag3)
          flag2 = true;
        if (flag1 & flag2)
          break;
      }
      this.vsButtonSeparator.Visible = flag1 & flag2;
    }

    private FileFolderAction getMenuItemAction(MenuItem item)
    {
      return (FileFolderAction) Enum.Parse(typeof (FileFolderAction), string.Concat(item.Tag), true);
    }

    private Control getButtonForAction(FileFolderAction action)
    {
      foreach (Control actionControl in this.actionControls)
      {
        if (this.getButtonAction(actionControl) == action)
          return actionControl;
      }
      return (Control) null;
    }

    private MenuItem getMenuItemForAction(FileFolderAction action)
    {
      foreach (MenuItem menuItem in this.ctxFileFolderMenu.MenuItems)
      {
        if (this.getMenuItemAction(menuItem) == action)
          return menuItem;
      }
      return (MenuItem) null;
    }

    private FileFolderAction getButtonAction(Control ctrl)
    {
      try
      {
        return (FileFolderAction) Enum.Parse(typeof (FileFolderAction), string.Concat(ctrl.Tag), true);
      }
      catch
      {
        return FileFolderAction.None;
      }
    }

    public void AttachFileSystem(IFileSystem fileSys)
    {
      this.AttachFileSystem(Session.DefaultInstance, fileSys);
    }

    public void AttachFileSystem(Sessions.Session session, IFileSystem fileSys)
    {
      this.AttachFileSystem(session, fileSys, (FileSystemEntry) null);
    }

    public void AttachFileSystem(
      Sessions.Session session,
      IFileSystem fileSys,
      FileSystemEntry defaultFolder)
    {
      if (session == null)
        throw new ArgumentNullException(nameof (session));
      if (fileSys == null)
        throw new ArgumentNullException(nameof (fileSys));
      this.session = session;
      this.fileSystem = fileSys;
      this.publicRoot = fileSys.GetPropertiesAndRights((FileSystemEntry) FileSystemEntry.PublicRoot.Clone());
      this.privateRoot = FileSystemEntry.PrivateRoot(session.UserID);
      this.privateRoot.Access = AclResourceAccess.ReadWrite;
      this.fileSystem.ConfigureExplorer(this);
      this.setFileSystemActionStates();
      if (this.fileSystem.AllowPublicAccess && this.publicRoot != null)
        this.setCurrentFolder(this.publicRoot);
      else
        this.setCurrentFolder(FileSystemEntry.PrivateRoot(this.session.UserID));
      this.Title = this.fileSystem.RootObjectDisplayName;
      try
      {
        if (defaultFolder != null)
          this.CurrentFolder = defaultFolder;
      }
      catch
      {
      }
      if (this.currentFolder != null)
        return;
      if (this.fileSystem.AllowPublicAccess)
        this.CurrentFolder = this.publicRoot;
      else
        this.CurrentFolder = this.privateRoot;
    }

    private void setFileSystemActionStates()
    {
      foreach (FileFolderAction action in Enum.GetValues(typeof (FileFolderAction)))
      {
        if (action != FileFolderAction.None && this.IsActionVisible(action))
          this.SetActionVisible(action, this.fileSystem.IsActionSupported(action));
      }
    }

    public ExplorerColumn AddColumn(string headerText)
    {
      GVColumn gvColumn = new GVColumn(headerText);
      gvColumn.Tag = (object) new ExplorerColumn(gvColumn);
      this.gvDirectory.Columns.Add(gvColumn);
      return (ExplorerColumn) gvColumn.Tag;
    }

    public ExplorerColumn AddColumn(string headerText, int width)
    {
      return this.AddColumn(headerText, width, GVSortMethod.Text);
    }

    public ExplorerColumn AddColumn(string headerText, int width, GVSortMethod sortMethod)
    {
      ExplorerColumn explorerColumn = this.AddColumn(headerText);
      explorerColumn.Width = width;
      explorerColumn.SortMethod = sortMethod;
      return explorerColumn;
    }

    public ExplorerColumn AddColumn(string headerText, string dataProperty, int width)
    {
      return this.AddColumn(headerText, dataProperty, width, GVSortMethod.Text);
    }

    public ExplorerColumn AddColumn(
      string headerText,
      string dataProperty,
      int width,
      GVSortMethod sortMethod)
    {
      ExplorerColumn explorerColumn = this.AddColumn(headerText, width, sortMethod);
      explorerColumn.DataProperty = dataProperty;
      return explorerColumn;
    }

    public ExplorerColumn AddColumn(
      string headerText,
      string dataProperty,
      string dataFormat,
      int width,
      GVSortMethod sortMethod)
    {
      ExplorerColumn explorerColumn = this.AddColumn(headerText, dataProperty, width, sortMethod);
      explorerColumn.DataFormat = dataFormat;
      return explorerColumn;
    }

    public void ResizeColumn(int index, int size) => this.GetColumn(index).Width = size;

    public ExplorerColumn GetColumn(int index)
    {
      return (ExplorerColumn) this.gvDirectory.Columns[index].Tag;
    }

    public void SetColumnSortOrder(ExplorerColumnSortOrder sortOrder)
    {
      this.SetColumnSortOrder(new ExplorerColumnSortOrder[1]
      {
        sortOrder
      });
    }

    public void SetColumnSortOrder(ExplorerColumnSortOrder[] sortOrder)
    {
      List<GVColumnSort> gvColumnSortList = new List<GVColumnSort>();
      foreach (ExplorerColumnSortOrder explorerColumnSortOrder in sortOrder)
        gvColumnSortList.Add(explorerColumnSortOrder.ToColumnSort());
      this.gvDirectory.Sort(gvColumnSortList.ToArray());
    }

    public ExplorerListItem GetItem(int index)
    {
      return (ExplorerListItem) this.gvDirectory.Items[index].Tag;
    }

    public ExplorerListItem[] GetCurrentFolderContents()
    {
      List<ExplorerListItem> explorerListItemList = new List<ExplorerListItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
        explorerListItemList.Add((ExplorerListItem) gvItem.Tag);
      return explorerListItemList.ToArray();
    }

    public ExplorerListItem[] GetSelectedItems()
    {
      List<ExplorerListItem> explorerListItemList = new List<ExplorerListItem>();
      foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
        explorerListItemList.Add((ExplorerListItem) selectedItem.Tag);
      return explorerListItemList.ToArray();
    }

    private GVItem createListItem(FileSystemEntry fsEntry)
    {
      GVItem listItem = new GVItem();
      listItem.Tag = (object) new ExplorerListItem(listItem, fsEntry);
      this.populateListItemProperties(listItem);
      return listItem;
    }

    protected Image GetListItemImage(FileSystemEntry entry)
    {
      Image customDisplayIcon = this.fileSystem.GetCustomDisplayIcon(entry);
      if (customDisplayIcon != null)
        return customDisplayIcon;
      if (entry.Type == FileSystemEntry.Types.Folder && entry.IsPublic)
        return this.imgsListView.Images["share-folder"];
      return entry.Type == FileSystemEntry.Types.Folder ? this.imgsListView.Images["folder"] : this.imgsListView.Images["file"];
    }

    private void populateListItemProperties(GVItem item)
    {
      ExplorerListItem tag1 = (ExplorerListItem) item.Tag;
      tag1.DisplayName = this.GetDisplayName(tag1.FileFolderEntry);
      tag1.DisplayIcon = this.GetListItemImage(tag1.FileFolderEntry);
      for (int index = 1; index < this.gvDirectory.Columns.Count; ++index)
      {
        ExplorerColumn tag2 = (ExplorerColumn) this.gvDirectory.Columns[index].Tag;
        if ((tag2.DataProperty ?? "") != "")
        {
          if ((tag2.DataFormat ?? "") != "")
            tag1.SetColumnText(index, string.Format(tag2.DataFormat, tag1.FileFolderEntry.Properties[(object) tag2.DataProperty]));
          else
            tag1.SetColumnText(index, string.Concat(tag1.FileFolderEntry.Properties[(object) tag2.DataProperty]));
        }
      }
      this.fileSystem.CustomizeListItem(tag1);
      this.OnPopulateListItem(new ExplorerListItemEventArgs(tag1));
    }

    private void setCurrentFolder(FileSystemEntry folderEntry)
    {
      if (this.currentFolder != null && this.currentFolder.Equals((object) folderEntry))
        return;
      if (FileSystemExplorer.isPublicPrivateRoot(folderEntry))
        throw new ArgumentException("The Public/Private root cannot be set as the current folder");
      if (folderEntry.Type != FileSystemEntry.Types.Folder)
        throw new Exception("The specified entry does not represent a folder");
      FileSystemEntry currentFolder = this.currentFolder;
      try
      {
        this.currentFolder = this.fileSystem.GetPropertiesAndRights(folderEntry);
        this.RefreshFolderContents();
      }
      catch
      {
        this.currentFolder = currentFolder;
        throw;
      }
      this.refreshFolderDropdown();
      this.setCurrentFolderActionState();
      this.OnFolderChanged(EventArgs.Empty);
    }

    public void RefreshFolderContents()
    {
      FileSystemEntry[] fileSystemEntries = this.fileSystem.GetFileSystemEntries(this.currentFolder);
      this.gvDirectory.Items.Clear();
      for (int index = 0; index < fileSystemEntries.Length; ++index)
      {
        if (fileSystemEntries[index].Type == FileSystemEntry.Types.Folder || !this.displayFoldersOnly)
          this.gvDirectory.Items.Add(this.createListItem(fileSystemEntries[index]));
      }
      this.gvDirectory.ReSort();
    }

    private void setCurrentFolderActionState()
    {
      AclResourceAccess access = this.currentFolder.Access;
      this.SetActionEnabled(FileFolderAction.AddNewFile, access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.PasteFolderOrFile, this.cutCopyEntries != null && access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.CreateFolder, access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.ImportFile, access == AclResourceAccess.ReadWrite);
      this.setCurrentSelectionActionState();
      this.btnUpFolder.Enabled = !this.isAtTopMostFolder();
    }

    private void setCurrentSelectionActionState()
    {
      AclResourceAccess access = this.currentFolder.Access;
      AclResourceAccess aclResourceAccess = AclResourceAccess.None;
      int count = this.gvDirectory.SelectedItems.Count;
      FileSystemEntry.Types types = FileSystemEntry.Types.None;
      foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
      {
        ExplorerListItem tag = (ExplorerListItem) selectedItem.Tag;
        types |= tag.FileFolderEntry.Type;
        if (tag.FileFolderEntry.Access == AclResourceAccess.ReadOnly)
          aclResourceAccess = AclResourceAccess.ReadOnly;
        else if (aclResourceAccess == AclResourceAccess.None)
          aclResourceAccess = tag.FileFolderEntry.Access;
      }
      this.SetActionEnabled(FileFolderAction.OpenFileOrFolder, count == 1);
      this.SetActionEnabled(FileFolderAction.EditFile, count == 1 && types == FileSystemEntry.Types.File && aclResourceAccess == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.CutFolderOrFile, count > 0 && aclResourceAccess == AclResourceAccess.ReadWrite && access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.CopyFolderOrFile, count > 0);
      this.SetActionEnabled(FileFolderAction.DeleteFolderOrFile, count > 0 && aclResourceAccess == AclResourceAccess.ReadWrite && access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.DuplicateFile, count == 1 && access == AclResourceAccess.ReadWrite);
      this.SetActionEnabled(FileFolderAction.RenameFolderOrFile, count == 1 && aclResourceAccess == AclResourceAccess.ReadWrite);
    }

    private static bool isPublicPrivateRoot(FileSystemEntry entry)
    {
      return entry.Equals((object) FileSystemExplorer.PublicPrivateRoot);
    }

    private bool isAtTopMostFolder() => this.currentFolder.IsRootFolder;

    protected void OnFolderChanged(EventArgs e)
    {
      if (this.FolderChanged == null)
        return;
      this.FolderChanged((object) this, e);
    }

    private void refreshFolderDropdown()
    {
      this.suspendEvents = true;
      try
      {
        this.cboFolder.Items.Clear();
        if (this.fileSystem.AllowPublicAccess && !this.currentFolder.IsPublic && this.publicRoot != null)
          this.addFolderHierarchyToDropdown(this.publicRoot);
        if (this.currentFolder.IsPublic)
          this.addFolderHierarchyToDropdown(this.currentFolder);
        if (this.fileSystem.AllowPrivateAccess && this.currentFolder.IsPublic)
          this.addFolderHierarchyToDropdown(this.privateRoot);
        if (!this.currentFolder.IsPublic)
          this.addFolderHierarchyToDropdown(this.currentFolder);
        foreach (ExplorerComboItem explorerComboItem in this.cboFolder.Items)
        {
          if (explorerComboItem.FileSystemEntry.Equals((object) this.currentFolder))
          {
            this.cboFolder.SelectedItem = (object) explorerComboItem;
            break;
          }
        }
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private void addFolderHierarchyToDropdown(FileSystemEntry folder)
    {
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      fileSystemEntryList.Add(folder);
      for (; folder.ParentFolder != null; folder = folder.ParentFolder)
        fileSystemEntryList.Add(folder.ParentFolder);
      for (int index = fileSystemEntryList.Count - 1; index >= 0; --index)
        this.cboFolder.Items.Add((object) this.createExplorerComboItem(fileSystemEntryList[index], fileSystemEntryList.Count - 1 - index));
    }

    private ExplorerComboItem createExplorerComboItem(FileSystemEntry fsEntry, int depth)
    {
      return new ExplorerComboItem(fsEntry, this.getDisplayPath(fsEntry), 16 * depth, this.GetListItemImage(fsEntry));
    }

    protected void OnFolderContentsChanged(EventArgs e)
    {
      if (this.FolderContentsChanged == null)
        return;
      this.FolderContentsChanged((object) this, e);
    }

    private void ctxFileFolderMenu_Popup(object sender, EventArgs e)
    {
      this.gvDirectory.GetItemAt(this.contextMenuPosition.X, this.contextMenuPosition.Y);
      foreach (MenuItem menuItem in this.ctxFileFolderMenu.MenuItems)
      {
        Control buttonForAction = this.getButtonForAction(this.getMenuItemAction(menuItem));
        if (buttonForAction == null)
          menuItem.Visible = true;
        else if (!buttonForAction.Visible)
          menuItem.Visible = false;
        else
          menuItem.Enabled = buttonForAction.Enabled;
      }
    }

    private void btnUpFolder_Click(object sender, EventArgs e)
    {
      if (this.isAtTopMostFolder())
        return;
      this.setCurrentFolder(this.currentFolder.ParentFolder);
    }

    private void gvDirectory_ItemDoubleClick(object sender, GVItemEventArgs e)
    {
      ExplorerListItem tag = (ExplorerListItem) e.Item.Tag;
      this.OnEntryDoubleClick(new FileSystemEventArgs(tag.FileFolderEntry));
      if (tag.FileFolderEntry.Type == FileSystemEntry.Types.Folder)
      {
        this.setCurrentFolder(tag.FileFolderEntry);
      }
      else
      {
        if (!this.IsActionEnabled(FileFolderAction.EditFile))
          return;
        this.openFile(tag);
      }
    }

    protected void OnEntryDoubleClick(FileSystemEventArgs e)
    {
      if (this.EntryDoubleClick == null)
        return;
      this.EntryDoubleClick((object) this, e);
    }

    private void openFile(ExplorerListItem listItem)
    {
      if (listItem.FileFolderEntry.Access != AclResourceAccess.ReadWrite)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not the required access rights to edit the item.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.fileSystem.OpenFile((IWin32Window) this, listItem.FileFolderEntry))
          return;
        this.RefreshFolderContents();
        this.FindItem(listItem.FileFolderEntry);
      }
    }

    private void openEntry(ExplorerListItem listItem)
    {
      if (listItem.FileFolderEntry.Type == FileSystemEntry.Types.File)
        this.openFile(listItem);
      else
        this.setCurrentFolder(listItem.FileFolderEntry);
    }

    private void editFileEventHandler(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        return;
      ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag;
      if (tag.FileFolderEntry.Type != FileSystemEntry.Types.File)
        return;
      this.openFile(tag);
    }

    private void openItemEventHandler(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        return;
      this.openEntry((ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag);
    }

    private void refreshFolderEventHandler(object sender, EventArgs e)
    {
      this.RefreshFolderContents();
    }

    private void deleteItemEventHandler(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        return;
      if (Utils.Dialog((IWin32Window) this, this.gvDirectory.SelectedItems.Count != 1 ? "Are you sure you want to premanently delete the " + (object) this.gvDirectory.SelectedItems.Count + " selected items?" : "Are you sure you want to permanently delete '" + ((ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag).DisplayName + "'?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      for (int index = 0; index < this.gvDirectory.SelectedItems.Count; ++index)
      {
        ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[index].Tag;
        try
        {
          if (tag.FileFolderEntry.Type == FileSystemEntry.Types.Folder && this.fileSystem.GetFileSystemEntries(tag.FileFolderEntry).Length != 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The folder '" + tag.DisplayName + "' cannot be deleted because it is not empty.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          CancelableFileSystemEventArgs e1 = new CancelableFileSystemEventArgs(tag.FileFolderEntry);
          this.OnBeforeEntryDeleted(e1);
          if (e1.Cancel || !this.fileSystem.DeleteEntry((IWin32Window) this, tag.FileFolderEntry))
            return;
          this.OnAfterEntryDeleted(new FileSystemEventArgs(tag.FileFolderEntry));
        }
        catch (Exception ex)
        {
          Tracing.Log(FileSystemExplorer.sw, nameof (FileSystemExplorer), TraceLevel.Error, "Error deleting file system entry '" + (object) tag.FileFolderEntry + "': " + (object) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "Error attempting to delete '" + tag.FileFolderEntry.Name + "': " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        }
      }
      this.ExecuteAction(FileFolderAction.RefreshFolder);
    }

    protected void OnBeforeEntryDeleted(CancelableFileSystemEventArgs e)
    {
      if (this.BeforeEntryDeleted == null)
        return;
      this.BeforeEntryDeleted((object) this, (FileSystemEventArgs) e);
    }

    protected void OnAfterEntryDeleted(FileSystemEventArgs e)
    {
      if (this.AfterEntryDeleted == null)
        return;
      this.AfterEntryDeleted((object) this, e);
    }

    private void createFolderEventHandler(object sender, EventArgs e)
    {
      if (FileSystemExplorer.isPublicPrivateRoot(this.currentFolder))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "New folder cannot be added to the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry entry = new FileSystemEntry(this.currentFolder.Path, "New Folder", FileSystemEntry.Types.Folder, this.currentFolder.Owner);
        int num2 = 2;
        while (this.fileSystem.EntryExistsOfAnyType(entry))
        {
          entry = new FileSystemEntry(this.currentFolder.Path, "New Folder (" + (object) num2 + ")", FileSystemEntry.Types.Folder, this.currentFolder.Owner);
          ++num2;
        }
        if (!this.fileSystem.CreateFolder((IWin32Window) this, entry))
          return;
        GVItem listItem = this.createListItem(this.fileSystem.GetPropertiesAndRights(entry));
        this.gvDirectory.Items.Add(listItem);
        this.gvDirectory.SelectedItems.Clear();
        this.gvDirectory.ReSort();
        listItem.Selected = true;
        this.OnFolderContentsChanged(EventArgs.Empty);
        listItem.BeginEdit();
      }
    }

    protected virtual string GetDisplayName(FileSystemEntry entry)
    {
      string customDisplayName = this.fileSystem.GetCustomDisplayName(entry);
      if ((customDisplayName ?? "") != "")
        return customDisplayName;
      if (FileSystemExplorer.isPublicPrivateRoot(entry))
        return "All " + this.fileSystem.RootObjectDisplayName;
      if (entry.IsRootFolder && entry.IsPublic)
        return "Public " + this.fileSystem.RootObjectDisplayName;
      if (entry.IsRootFolder)
        return "Personal " + this.fileSystem.RootObjectDisplayName;
      return entry.Type == FileSystemEntry.Types.Folder || this.fileSystem.DisplayExtensions ? entry.Name : FileSystem.GetFilenameWithoutExtension(entry.Name);
    }

    private string getDisplayPath(FileSystemEntry entry)
    {
      string str = !entry.IsPublic ? "\\\\Personal " + this.fileSystem.RootObjectDisplayName : "\\\\Public " + this.fileSystem.RootObjectDisplayName;
      return entry.IsRootFolder ? str : str + entry.ParentFolder.Path + this.GetDisplayName(entry);
    }

    private void renameItemEventHandler(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.gvDirectory.SelectedItems[0];
      if (((ExplorerListItem) selectedItem.Tag).FileFolderEntry.Access != AclResourceAccess.ReadWrite)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to rename this item.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        selectedItem.BeginEdit();
    }

    private void gvDirectory_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.contextMenuPosition = e.Location;
    }

    private void startCutCopy(bool isCutAction)
    {
      if (this.gvDirectory.SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one file or folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.isCut = isCutAction;
        this.cutCopyEntries = new FileSystemEntry[this.gvDirectory.SelectedItems.Count];
        for (int index = 0; index < this.gvDirectory.SelectedItems.Count; ++index)
        {
          ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[index].Tag;
          this.cutCopyEntries[index] = tag.FileFolderEntry;
        }
        this.setCurrentFolderActionState();
      }
    }

    private void cutItemEventHandler(object sender, EventArgs e) => this.startCutCopy(true);

    private void copyItemEventHandler(object sender, EventArgs e) => this.startCutCopy(false);

    private bool copyOrCut(FileSystemEntry source, FileSystemEntry target, bool isCut)
    {
      if (!this.fileSystem.EntryExistsOfAnyType(source))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot find the source file or folder '" + source.Name + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      target = new FileSystemEntry(target.Path, source.Name, source.Type, target.Owner);
      if (source.Equals((object) target) && this.isCut)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to perform paste operation because the destination is the same as the source.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (source.Equals((object) target))
      {
        FileSystemEntry entry = new FileSystemEntry(target.ParentFolder.Path + "Copy of " + target.Name, target.Type, target.Owner);
        int num = 2;
        while (this.fileSystem.EntryExistsOfAnyType(entry))
        {
          entry = new FileSystemEntry(target.ParentFolder.Path + "Copy of (" + (object) num + ") " + target.Name, target.Type, target.Owner);
          ++num;
        }
        target = entry;
      }
      if (this.fileSystem.EntryExistsOfAnyType(target))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot create or replace '" + target.Name + "'. There is already a file or folder with the same name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (source.Type == FileSystemEntry.Types.Folder && target.ToString().ToLower().IndexOf(source.ToString().ToLower()) >= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot move or copy the folder '" + source.Name + "' because the destination is a subdirectory of the source.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        return this.isCut ? this.fileSystem.MoveEntry((IWin32Window) this, source, target) : this.fileSystem.CopyEntry((IWin32Window) this, source, target);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void pasteItemEventHandler(object sender, EventArgs e)
    {
      if (this.cutCopyEntries == null)
        return;
      FileSystemEntry target = this.currentFolder;
      if (this.gvDirectory.SelectedItems.Count > 0)
      {
        ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag;
        if (tag.FileFolderEntry.Type == FileSystemEntry.Types.Folder)
          target = tag.FileFolderEntry;
      }
      int index = 0;
      while (index < this.cutCopyEntries.Length && this.copyOrCut(this.cutCopyEntries[index], target, this.isCut))
        ++index;
      if (this.isCut)
        this.cutCopyEntries = (FileSystemEntry[]) null;
      this.RefreshFolderContents();
      this.setCurrentSelectionActionState();
    }

    private void newFileEventHandler(object sender, EventArgs e)
    {
      string str = "New " + this.fileSystem.FileEntryDisplayName;
      FileSystemEntry entry = new FileSystemEntry(this.currentFolder.Path, str + this.fileSystem.DefaultExtension, FileSystemEntry.Types.File, this.currentFolder.Owner);
      int num = 2;
      while (this.fileSystem.EntryExistsOfAnyType(entry))
      {
        entry = new FileSystemEntry(this.currentFolder.Path, str + " (" + (object) num + ")" + this.fileSystem.DefaultExtension, FileSystemEntry.Types.File, this.currentFolder.Owner);
        ++num;
      }
      if (!this.fileSystem.CreateFile((IWin32Window) this, entry))
        return;
      GVItem listItem = this.createListItem(this.fileSystem.GetPropertiesAndRights(entry));
      this.gvDirectory.Items.Add(listItem);
      this.gvDirectory.SelectedItems.Clear();
      this.gvDirectory.ReSort();
      listItem.Selected = true;
      this.OnFolderContentsChanged(EventArgs.Empty);
      listItem.BeginEdit();
    }

    private void cboFolder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents || this.cboFolder.SelectedItem == null)
        return;
      ExplorerComboItem selectedItem = (ExplorerComboItem) this.cboFolder.SelectedItem;
      try
      {
        this.setCurrentFolder(selectedItem.FileSystemEntry);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    public void SelectAll() => this.gvDirectory.Items.SelectAll();

    public bool SelectFirstItemOfType(FileSystemEntry.Types type)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
      {
        if ((((ExplorerListItem) gvItem.Tag).FileFolderEntry.Type & type) != FileSystemEntry.Types.None)
        {
          gvItem.Selected = true;
          return true;
        }
      }
      return false;
    }

    public bool SelectItem(FileSystemEntry selection)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
      {
        if (((ExplorerListItem) gvItem.Tag).FileFolderEntry.Equals((object) selection))
        {
          gvItem.Selected = true;
          return true;
        }
      }
      return false;
    }

    public bool EnsureVisible(FileSystemEntry entry)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
      {
        if (((ExplorerListItem) gvItem.Tag).FileFolderEntry.Equals((object) entry))
        {
          this.gvDirectory.EnsureVisible(gvItem.Index);
          return true;
        }
      }
      return false;
    }

    public void SelectItems(FileSystemEntry[] selections)
    {
      Dictionary<FileSystemEntry, bool> dictionary = new Dictionary<FileSystemEntry, bool>();
      foreach (FileSystemEntry selection in selections)
        dictionary[selection] = true;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
      {
        ExplorerListItem tag = (ExplorerListItem) gvItem.Tag;
        if (dictionary.ContainsKey(tag.FileFolderEntry))
          gvItem.Selected = true;
      }
    }

    public bool FindItem(FileSystemEntry target)
    {
      FileSystemEntry fileSystemEntry = target.Type != FileSystemEntry.Types.File ? target : target.ParentFolder;
      if (!this.fileSystem.EntryExists(fileSystemEntry))
        return false;
      this.setCurrentFolder(fileSystemEntry);
      this.ClearSelections();
      if (target.Type == FileSystemEntry.Types.Folder)
        return true;
      if (!this.SelectItem(target))
        return false;
      this.EnsureVisible(target);
      return true;
    }

    public void ClearSelections() => this.gvDirectory.SelectedItems.Clear();

    private void gvDirectory_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\u0001')
        this.SelectAll();
      else if (e.KeyChar == '\u0003' && this.IsActionEnabled(FileFolderAction.CopyFolderOrFile))
        this.copyItemEventHandler((object) null, (EventArgs) null);
      else if (e.KeyChar == '\u0004' && this.IsActionEnabled(FileFolderAction.DeleteFolderOrFile))
        this.deleteItemEventHandler((object) null, (EventArgs) null);
      else if (e.KeyChar == '\u0016' && this.IsActionEnabled(FileFolderAction.PasteFolderOrFile))
        this.pasteItemEventHandler((object) null, (EventArgs) null);
      else if (e.KeyChar == '\u0016' && this.cutCopyEntries != null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary rights to paste items into this folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (e.KeyChar == '\u0018' && this.IsActionEnabled(FileFolderAction.CutFolderOrFile))
      {
        this.cutItemEventHandler((object) null, (EventArgs) null);
      }
      else
      {
        if (e.KeyChar != '\u0018' || this.gvDirectory.SelectedItems.Count <= 0)
          return;
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights for the selected files to perform this function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void importItemEventHandler(object sender, EventArgs e)
    {
      FileSystemEntry fileEntry = this.currentFolder;
      if (this.gvDirectory.SelectedItems.Count != 0)
      {
        ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag;
        if (tag.FileFolderEntry.Type == FileSystemEntry.Types.Folder)
          fileEntry = tag.FileFolderEntry;
      }
      if (!this.fileSystem.Import((IWin32Window) this, fileEntry) || fileEntry != this.currentFolder)
        return;
      this.RefreshFolderContents();
    }

    private void gvDirectory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setCurrentSelectionActionState();
      this.OnSelectedEntryChanged(EventArgs.Empty);
    }

    protected void OnSelectedEntryChanged(EventArgs e)
    {
      if (this.SelectedEntryChanged == null)
        return;
      this.SelectedEntryChanged((object) this, e);
    }

    private void gvDirectory_Resize(object sender, EventArgs e)
    {
      try
      {
        Decimal num = (Decimal) this.gvDirectory.ClientRectangle.Width * 1M / (Decimal) Math.Max(this.explorerWidth, this.gvDirectory.Columns.Count);
        foreach (GVColumn column in this.gvDirectory.Columns)
          column.Width = Math.Max(Convert.ToInt32(num * (Decimal) column.Width), 1);
        this.explorerWidth = this.gvDirectory.ClientRectangle.Width;
      }
      catch
      {
      }
    }

    public void SetFocusToFileList() => this.gvDirectory.Focus();

    public void ExecuteAction(FileFolderAction func)
    {
      switch (func)
      {
        case FileFolderAction.OpenFileOrFolder:
          this.openItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.CreateFolder:
          this.createFolderEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.AddNewFile:
          this.newFileEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.EditFile:
          this.editFileEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.CutFolderOrFile:
          this.cutItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.CopyFolderOrFile:
          this.copyItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.PasteFolderOrFile:
          this.pasteItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.DeleteFolderOrFile:
          this.deleteItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.RenameFolderOrFile:
          this.renameItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.ImportFile:
          this.importItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.RefreshFolder:
          this.refreshFolderEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.DuplicateFile:
          this.duplicateItemEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.DeployFile:
          this.deployFileEventHandler((object) null, (EventArgs) null);
          break;
        case FileFolderAction.ExportFile:
          this.exportFileEventHandler((object) null, (EventArgs) null);
          break;
      }
    }

    public bool IsActionEnabled(FileFolderAction action)
    {
      if (!this.IsActionVisible(action))
        return false;
      return !this.actionEnabledStates.ContainsKey(action) || this.actionEnabledStates[action];
    }

    public bool IsActionVisible(FileFolderAction action)
    {
      if (!this.actionBarVisibleState)
        return false;
      return !this.actionVisibleStates.ContainsKey(action) || this.actionVisibleStates[action];
    }

    private void buttons_Click(object sender, EventArgs e)
    {
      this.ExecuteAction(this.getButtonAction((Control) sender));
    }

    private void gvDirectory_KeyUp(object sender, KeyEventArgs e)
    {
      FileFolderAction fileFolderAction = FileFolderAction.None;
      if (e.KeyCode == Keys.Return)
        fileFolderAction = FileFolderAction.OpenFileOrFolder;
      else if (e.KeyCode == Keys.Delete)
        fileFolderAction = FileFolderAction.DeleteFolderOrFile;
      if (fileFolderAction == FileFolderAction.None || !this.IsActionEnabled(fileFolderAction))
        return;
      this.ExecuteAction(fileFolderAction);
    }

    private void gvDirectory_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (!this.IsActionEnabled(FileFolderAction.RenameFolderOrFile))
      {
        e.Cancel = true;
      }
      else
      {
        ExplorerListItem tag = (ExplorerListItem) e.SubItem.Item.Tag;
        if (this.currentFolder.IsPublic && tag.FileFolderEntry.Access != AclResourceAccess.ReadWrite)
        {
          e.Cancel = true;
        }
        else
        {
          CancelableFileSystemEventArgs e1 = new CancelableFileSystemEventArgs(tag.FileFolderEntry);
          this.OnBeforeEntryRenamed(e1);
          if (e1.Cancel)
          {
            e.Cancel = true;
          }
          else
          {
            if (this.editorFormatter != null)
              this.editorFormatter.Dispose();
            this.editorFormatter = (TextBoxFormatter) new FileSystemEntryFormatter(e.EditorControl as TextBox);
          }
        }
      }
    }

    private void resetCopyPaste() => this.cutCopyEntries = (FileSystemEntry[]) null;

    private void gvDirectory_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      ExplorerListItem tag = (ExplorerListItem) e.SubItem.Item.Tag;
      FileSystemEntry fileFolderEntry = tag.FileFolderEntry;
      string str = e.EditorControl.Text.Trim();
      if (str.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify an new name for this item.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
      else if (str.IndexOf("\\") > -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot contain the \"\\\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
      else if (str.Replace(".", "") == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name must contain characters other than '.'. ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
      else if (str.StartsWith("."))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name must start with a character other than a period. ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
      else if (fileFolderEntry.Type == FileSystemEntry.Types.File && str.Length > this.fileSystem.MaxFileNameLength)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "File name is limited to " + (object) this.fileSystem.MaxFileNameLength + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
      if (e.Cancel)
      {
        e.EditorControl.Text = fileFolderEntry.Name;
      }
      else
      {
        e.Handled = true;
        if (!this.fileSystem.DisplayExtensions && fileFolderEntry.Type == FileSystemEntry.Types.File)
          str += Path.GetExtension(fileFolderEntry.Name);
        FileSystemEntry fileSystemEntry = new FileSystemEntry(fileFolderEntry.ParentFolder.Path, str, fileFolderEntry.Type, fileFolderEntry.Owner);
        fileSystemEntry.Access = fileFolderEntry.Access;
        fileSystemEntry.Properties = fileFolderEntry.Properties;
        if (string.Compare(fileFolderEntry.Name, str, false) == 0)
          return;
        if (this.fileSystem.EntryExistsOfAnyType(fileSystemEntry))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The selected item cannot be renamed because an item in this folder already exists with the specified name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          e.Cancel = true;
        }
        else
        {
          this.resetCopyPaste();
          try
          {
            if (!this.fileSystem.MoveEntry((IWin32Window) this, fileFolderEntry, fileSystemEntry))
            {
              e.Cancel = true;
              return;
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          tag.FileFolderEntry = fileSystemEntry;
          this.populateListItemProperties(e.SubItem.Item);
          this.OnAfterEntryRenamed(new FileSystemEventArgs(fileSystemEntry));
        }
      }
    }

    protected void OnBeforeEntryRenamed(CancelableFileSystemEventArgs e)
    {
      if (this.BeforeEntryRenamed == null)
        return;
      this.BeforeEntryRenamed((object) this, e);
    }

    protected void OnAfterEntryRenamed(FileSystemEventArgs e)
    {
      if (this.AfterEntryRenamed == null)
        return;
      this.AfterEntryRenamed((object) this, e);
    }

    private void cboFolder_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Form parentForm = this.ParentForm;
      if (!(parentForm is IHelp))
        return;
      ((IHelp) parentForm).ShowHelp();
    }

    private void duplicateItemEventHandler(object sender, EventArgs e)
    {
      this.ExecuteAction(FileFolderAction.CopyFolderOrFile);
      this.gvDirectory.SelectedItems.Clear();
      this.ExecuteAction(FileFolderAction.PasteFolderOrFile);
    }

    private void deployFileEventHandler(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count != 1)
        return;
      ExplorerListItem tag = (ExplorerListItem) this.gvDirectory.SelectedItems[0].Tag;
      if (tag.FileFolderEntry.Type != FileSystemEntry.Types.File || !this.fileSystem.Deploy((IWin32Window) this, tag.FileFolderEntry))
        return;
      this.ClearSelections();
    }

    private void exportFileEventHandler(object sender, EventArgs e)
    {
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
      {
        ExplorerListItem tag = (ExplorerListItem) selectedItem.Tag;
        if (FileSystemEntry.Types.File == tag.FileFolderEntry.Type)
          fileSystemEntryList.Add(tag.FileFolderEntry);
      }
      if (fileSystemEntryList.Count == 0)
        return;
      this.fileSystem.Export((IWin32Window) this, fileSystemEntryList.ToArray());
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      this.OnSetAsDefaultClick(EventArgs.Empty);
    }

    protected void OnSetAsDefaultClick(EventArgs e)
    {
      if (this.SetAsDefaultButtonClick == null)
        return;
      this.SetAsDefaultButtonClick((object) this, e);
    }

    protected void OnPopulateListItem(ExplorerListItemEventArgs e)
    {
      if (this.PopulateListItem == null)
        return;
      this.PopulateListItem((object) this, e);
    }

    public void NavigateToParentFolder()
    {
      if (!this.btnUpFolder.Enabled)
        return;
      this.btnUpFolder_Click((object) null, (EventArgs) null);
    }

    public enum DialogMode
    {
      Unspecified,
      SelectFiles,
      SaveFiles,
      ManageFiles,
    }
  }
}
