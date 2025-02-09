// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlEmailControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using Elli.Web.Host;
using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlEmailControl : UserControl
  {
    private bool allowPersonalImages;
    private bool showToolbar = true;
    private bool showFieldButton = true;
    private bool allowSignatures = true;
    private bool readOnly;
    private bool isConsumerConnect;
    private Sessions.Session session;
    private IContainer components;
    private EncWebFormBrowserControl browser;
    private GradientPanel pnlTop;
    private BorderPanel pnlBrowser;
    private FlowLayoutPanel pnlToolbar;
    private ComboBox cboFontName;
    private ComboBox cboFontSize;
    private VerticalSeparator seperator1;
    private IconCheckBox chkBold;
    private IconCheckBox chkItalic;
    private IconCheckBox chkUnderline;
    private IconButton btnColor;
    private VerticalSeparator seperator2;
    private IconButton btnImage;
    private IconButton btnLink;
    private IconButton btnField;
    private ToolTip tooltip;
    private ContextMenuStrip contextMenu;
    private ToolStripMenuItem mnuEditUndo;
    private ToolStripMenuItem mnuEditRedo;
    private ToolStripSeparator mnuItemSeparator1;
    private ToolStripMenuItem mnuEditCut;
    private ToolStripMenuItem mnuEditCopy;
    private ToolStripMenuItem mnuEditPaste;
    private ToolStripMenuItem mnuEditDelete;
    private ToolStripSeparator mnuItemSeparator2;
    private ToolStripMenuItem mnuViewSource;

    public HtmlEmailControl()
      : this(Session.DefaultInstance)
    {
    }

    public HtmlEmailControl(Sessions.Session session)
    {
      this.browser = (EncWebFormBrowserControl) new EncWebBrowser();
      this.browser.SetOpaqueBackground();
      this.InitializeComponent();
      this.browser.ProcessSelectionChange += new EventHandler(this.processSelectionChange);
      this.browser.ProcessKeyDown += new EventHandler<HtmlEditorKeyDownEventArgs>(this.processKeyDown);
      this.browser.ProcessReadyStateChange += new EventHandler(this.processReadyStateChange);
      this.browser.ContentChanged += new EventHandler(this.OnContentChanged);
      this.session = session;
      this.initFontList();
    }

    [Browsable(false)]
    public string Html => this.browser.GetBrowserHtml(System.Type.Missing);

    [Browsable(false)]
    public string HtmlBodyText => this.browser.GetHtmlBodyText(System.Type.Missing);

    [Description("Indicates if the user can add personal images")]
    [Category("Appearance")]
    [DefaultValue(false)]
    [Browsable(true)]
    public bool AllowPersonalImages
    {
      get => this.allowPersonalImages;
      set => this.allowPersonalImages = value;
    }

    [Description("Indicates if the user can add signatures")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool AllowSignatures
    {
      get => this.allowSignatures;
      set => this.allowSignatures = value;
    }

    [Description("Indicates if the editing toolbar is visible")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowToolbar
    {
      get => this.showToolbar;
      set
      {
        this.showToolbar = value;
        this.pnlTop.Visible = value;
      }
    }

    [Description("Indicates if the insert field toolbar button is visible")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowFieldButton
    {
      get => this.showFieldButton;
      set
      {
        this.showFieldButton = value;
        this.btnField.Visible = value;
      }
    }

    [Description("Indicates if the loan source is consumer connect")]
    [Category("Appearance")]
    [DefaultValue(false)]
    [Browsable(false)]
    public bool IsConsumerConnect
    {
      get => this.isConsumerConnect;
      set => this.isConsumerConnect = value;
    }

    public void LoadText(string text, bool readOnly)
    {
      this.LoadHtml("<HTML><BODY><FONT face=\"Arial\" size=\"2\">" + HttpUtility.HtmlEncode(text).Replace("\r\n", "<br>").Replace("\n", "<br>") + "</FONT></BODY></HTML>", readOnly);
    }

    public void LoadHtml(string html, bool readOnly)
    {
      if (!this.IsHandleCreated)
        this.CreateHandle();
      this.browser.LoadHtml(html, readOnly);
      this.browser.AddDomEvents("readystatechange", (object) this);
      this.browser.AddDomEvents("selectionchange", (object) this);
      this.browser.AddDomEvents("keydown", (object) this);
      this.browser.AddDomEvents("keypress", (object) this);
      this.readOnly = readOnly;
    }

    [DispId(0)]
    public void InvokeHtmlEvent() => this.browser.InvokeHtmlEvent();

    private void processKeyDown(object sender, HtmlEditorKeyDownEventArgs e)
    {
      if (e.IsChromiumBrowser || e.KeyCode != 13)
        return;
      this.browser.ExecuteCommand("InsertHtml", (object) "<br>");
      this.OnContentChanged((object) this, EventArgs.Empty);
      e.ReturnValue = false;
    }

    private void processReadyStateChange(object sender, EventArgs e)
    {
      this.browser.AddDomEvents("drop", (object) this);
    }

    private void executeCommand(string cmdID, object value)
    {
      if (!this.browser.IsQueryCommandEnabled(cmdID) || !this.browser.ExecuteCommand(cmdID, value) || !(cmdID != "Copy"))
        return;
      this.OnContentChanged((object) this, EventArgs.Empty);
      this.processSelectionChange((object) this, EventArgs.Empty);
    }

    private void browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      string cmdID = (string) null;
      if (e.KeyCode == Keys.Delete)
        cmdID = "Delete";
      else if (e.Control && !e.Alt && !e.Shift)
      {
        switch (e.KeyCode)
        {
          case Keys.B:
            cmdID = "Bold";
            break;
          case Keys.C:
            cmdID = "Copy";
            break;
          case Keys.I:
            cmdID = "Italic";
            break;
          case Keys.U:
            cmdID = "Underline";
            break;
          case Keys.V:
            cmdID = "Paste";
            break;
          case Keys.X:
            cmdID = "Cut";
            break;
          case Keys.Z:
            cmdID = "Undo";
            break;
        }
      }
      if (cmdID == null)
        return;
      this.executeCommand(cmdID, (object) null);
    }

    private void browser_Navigating(object sender, BeforeNavigationEventArgs e)
    {
      if (!this.browser.IsDocumentExists || !(this.browser is EncWebBrowser))
        return;
      e.Cancel = true;
    }

    private void initFontList()
    {
      if (this.DesignMode)
        return;
      using (InstalledFontCollection installedFontCollection = new InstalledFontCollection())
      {
        foreach (FontFamily family in installedFontCollection.Families)
          this.cboFontName.Items.Add((object) family.Name);
      }
      this.cboFontSize.Items.Add((object) "8");
      this.cboFontSize.Items.Add((object) "10");
      this.cboFontSize.Items.Add((object) "12");
      this.cboFontSize.Items.Add((object) "14");
      this.cboFontSize.Items.Add((object) "18");
      this.cboFontSize.Items.Add((object) "24");
      this.cboFontSize.Items.Add((object) "36");
    }

    private void processSelectionChange(object sender, EventArgs e)
    {
      string empty = string.Empty;
      int result = -1;
      string queryCommandValue = this.browser.GetQueryCommandValue("FontName");
      int.TryParse(this.browser.GetQueryCommandValue("FontSize"), out result);
      bool boolean1 = Convert.ToBoolean(this.browser.GetQueryCommandValue("Bold"));
      bool boolean2 = Convert.ToBoolean(this.browser.GetQueryCommandValue("Italic"));
      bool boolean3 = Convert.ToBoolean(this.browser.GetQueryCommandValue("Underline"));
      string str = queryCommandValue.IndexOf('"') > -1 ? queryCommandValue.Replace("\"", string.Empty) : queryCommandValue;
      this.cboFontName.SelectedItem = !this.cboFontName.Items.Contains((object) str) ? (object) null : (object) str;
      if (result >= 1 && result <= 7)
        this.cboFontSize.SelectedIndex = result - 1;
      else
        this.cboFontSize.SelectedIndex = -1;
      this.chkBold.Checked = boolean1;
      this.chkItalic.Checked = boolean2;
      this.chkUnderline.Checked = boolean3;
    }

    private void cboFontName_SelectionChangeCommitted(object sender, EventArgs e)
    {
      string selectedItem = (string) this.cboFontName.SelectedItem;
      if (selectedItem == null)
        return;
      this.executeCommand("FontName", (object) selectedItem);
    }

    private void cboFontSize_SelectionChangeCommitted(object sender, EventArgs e)
    {
      int num = this.cboFontSize.SelectedIndex + 1;
      if (num < 1)
        return;
      this.executeCommand("FontSize", (object) num);
    }

    private void chkBold_Click(object sender, EventArgs e)
    {
      this.executeCommand("Bold", (object) this.chkBold.Checked);
    }

    private void chkItalic_Click(object sender, EventArgs e)
    {
      this.executeCommand("Italic", (object) this.chkItalic.Checked);
    }

    private void chkUnderline_Click(object sender, EventArgs e)
    {
      this.executeCommand("Underline", (object) this.chkUnderline.Checked);
    }

    private void btnColor_Click(object sender, EventArgs e)
    {
      Color color = Color.Empty;
      try
      {
        color = this.browser.GetSelectedFontColor();
      }
      catch (Exception ex)
      {
      }
      using (ColorDialog colorDialog = new ColorDialog())
      {
        colorDialog.AllowFullOpen = true;
        colorDialog.Color = color;
        colorDialog.FullOpen = true;
        if (colorDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        this.executeCommand("ForeColor", (object) ColorTranslator.ToHtml(colorDialog.Color));
      }
    }

    private void btnImage_Click(object sender, EventArgs e)
    {
      using (ImageLibraryClient imageLibraryClient = new ImageLibraryClient())
      {
        List<ImageInfo> companyImageInfo = new List<ImageInfo>();
        List<ImageInfo> userImageInfo = new List<ImageInfo>();
        if (!imageLibraryClient.GetAllImageList(companyImageInfo, userImageInfo, this.session))
          return;
        using (HtmlImageDialog htmlImageDialog = new HtmlImageDialog(companyImageInfo, userImageInfo, this.allowPersonalImages, this.session))
        {
          if (htmlImageDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
            return;
          this.executeCommand("InsertImage", (object) htmlImageDialog.Url);
        }
      }
    }

    private void btnLink_Click(object sender, EventArgs e)
    {
      using (HtmlLinkDialog htmlLinkDialog = new HtmlLinkDialog())
      {
        if (htmlLinkDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        this.executeCommand("CreateLink", (object) htmlLinkDialog.LinkUrl);
      }
    }

    private void btnField_Click(object sender, EventArgs e)
    {
      using (HtmlFieldDialog htmlFieldDialog = new HtmlFieldDialog(this.allowSignatures, this.session, this.isConsumerConnect))
      {
        if (htmlFieldDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        HtmlFieldMerge.InsertField(this.browser, htmlFieldDialog.FieldID, htmlFieldDialog.FieldName);
        this.OnContentChanged((object) this, EventArgs.Empty);
      }
    }

    public event EventHandler ContentChanged;

    protected virtual void OnContentChanged(object sender, EventArgs e)
    {
      if (this.ContentChanged == null)
        return;
      this.ContentChanged((object) this, e);
    }

    private void contextMenu_Opening(object sender, CancelEventArgs e)
    {
      this.mnuEditCopy.Enabled = this.browser.IsQueryCommandEnabled("Copy");
      this.mnuEditCut.Enabled = this.browser.IsQueryCommandEnabled("Cut");
      this.mnuEditDelete.Enabled = this.browser.IsQueryCommandEnabled("Delete");
      this.mnuEditPaste.Enabled = this.browser.IsQueryCommandEnabled("Paste");
      this.mnuEditRedo.Enabled = this.browser.IsQueryCommandEnabled("Redo");
      this.mnuEditUndo.Enabled = this.browser.IsQueryCommandEnabled("Undo");
    }

    private void mnuEdit_Click(object sender, EventArgs e)
    {
      string cmdID = (string) null;
      if (sender == this.mnuEditCopy)
        cmdID = "Copy";
      else if (sender == this.mnuEditCut)
        cmdID = "Cut";
      else if (sender == this.mnuEditDelete)
        cmdID = "Delete";
      else if (sender == this.mnuEditPaste)
        cmdID = "Paste";
      else if (sender == this.mnuEditRedo)
        cmdID = "Redo";
      else if (sender == this.mnuEditUndo)
        cmdID = "Undo";
      if (cmdID == null)
        return;
      this.executeCommand(cmdID, (object) null);
    }

    private void mnuViewSource_Click(object sender, EventArgs e)
    {
      using (HtmlSourceDialog htmlSourceDialog = new HtmlSourceDialog(this.Html, this.readOnly))
      {
        if (htmlSourceDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.LoadHtml(htmlSourceDialog.Html, this.readOnly);
        this.OnContentChanged((object) this, EventArgs.Empty);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.browser?.Dispose();
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.pnlTop = new GradientPanel();
      this.pnlToolbar = new FlowLayoutPanel();
      this.cboFontName = new ComboBox();
      this.cboFontSize = new ComboBox();
      this.seperator1 = new VerticalSeparator();
      this.chkBold = new IconCheckBox();
      this.chkItalic = new IconCheckBox();
      this.chkUnderline = new IconCheckBox();
      this.btnColor = new IconButton();
      this.seperator2 = new VerticalSeparator();
      this.btnImage = new IconButton();
      this.btnLink = new IconButton();
      this.btnField = new IconButton();
      this.pnlBrowser = new BorderPanel();
      this.contextMenu = new ContextMenuStrip(this.components);
      this.mnuEditUndo = new ToolStripMenuItem();
      this.mnuEditRedo = new ToolStripMenuItem();
      this.mnuItemSeparator1 = new ToolStripSeparator();
      this.mnuEditCut = new ToolStripMenuItem();
      this.mnuEditCopy = new ToolStripMenuItem();
      this.mnuEditPaste = new ToolStripMenuItem();
      this.mnuEditDelete = new ToolStripMenuItem();
      this.mnuItemSeparator2 = new ToolStripSeparator();
      this.mnuViewSource = new ToolStripMenuItem();
      this.tooltip = new ToolTip(this.components);
      this.pnlTop.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.chkBold).BeginInit();
      ((ISupportInitialize) this.chkItalic).BeginInit();
      ((ISupportInitialize) this.chkUnderline).BeginInit();
      ((ISupportInitialize) this.btnColor).BeginInit();
      ((ISupportInitialize) this.btnImage).BeginInit();
      ((ISupportInitialize) this.btnLink).BeginInit();
      ((ISupportInitialize) this.btnField).BeginInit();
      this.pnlBrowser.SuspendLayout();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      this.pnlTop.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlTop.Controls.Add((Control) this.pnlToolbar);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlTop.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Padding = new Padding(5, 5, 5, 4);
      this.pnlTop.Size = new Size(554, 31);
      this.pnlTop.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlTop.TabIndex = 0;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.cboFontName);
      this.pnlToolbar.Controls.Add((Control) this.cboFontSize);
      this.pnlToolbar.Controls.Add((Control) this.seperator1);
      this.pnlToolbar.Controls.Add((Control) this.chkBold);
      this.pnlToolbar.Controls.Add((Control) this.chkItalic);
      this.pnlToolbar.Controls.Add((Control) this.chkUnderline);
      this.pnlToolbar.Controls.Add((Control) this.btnColor);
      this.pnlToolbar.Controls.Add((Control) this.seperator2);
      this.pnlToolbar.Controls.Add((Control) this.btnImage);
      this.pnlToolbar.Controls.Add((Control) this.btnLink);
      this.pnlToolbar.Controls.Add((Control) this.btnField);
      this.pnlToolbar.Dock = DockStyle.Fill;
      this.pnlToolbar.Location = new Point(5, 5);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(544, 22);
      this.pnlToolbar.TabIndex = 0;
      this.cboFontName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFontName.FormattingEnabled = true;
      this.cboFontName.Location = new Point(0, 0);
      this.cboFontName.Margin = new Padding(0, 0, 4, 0);
      this.cboFontName.Name = "cboFontName";
      this.cboFontName.Size = new Size(192, 22);
      this.cboFontName.TabIndex = 0;
      this.cboFontName.TabStop = false;
      this.cboFontName.SelectionChangeCommitted += new EventHandler(this.cboFontName_SelectionChangeCommitted);
      this.cboFontSize.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFontSize.FormattingEnabled = true;
      this.cboFontSize.Location = new Point(196, 0);
      this.cboFontSize.Margin = new Padding(0, 0, 4, 0);
      this.cboFontSize.Name = "cboFontSize";
      this.cboFontSize.Size = new Size(58, 22);
      this.cboFontSize.TabIndex = 1;
      this.cboFontSize.TabStop = false;
      this.cboFontSize.SelectionChangeCommitted += new EventHandler(this.cboFontSize_SelectionChangeCommitted);
      this.seperator1.Location = new Point(258, 3);
      this.seperator1.Margin = new Padding(0, 3, 4, 3);
      this.seperator1.MaximumSize = new Size(2, 16);
      this.seperator1.MinimumSize = new Size(2, 16);
      this.seperator1.Name = "seperator1";
      this.seperator1.Size = new Size(2, 16);
      this.seperator1.TabIndex = 2;
      this.chkBold.Checked = false;
      this.chkBold.CheckedImage = (Image) Resources.bold_over;
      this.chkBold.CheckedMouseOverImage = (Image) Resources.bold_over;
      this.chkBold.DisabledImage = (Image) null;
      this.chkBold.Image = (Image) Resources.bold;
      this.chkBold.Location = new Point(264, 3);
      this.chkBold.Margin = new Padding(0, 3, 4, 3);
      this.chkBold.Name = "chkBold";
      this.chkBold.Size = new Size(16, 16);
      this.chkBold.TabIndex = 3;
      this.chkBold.TabStop = false;
      this.tooltip.SetToolTip((Control) this.chkBold, "Bold");
      this.chkBold.UncheckedImage = (Image) Resources.bold;
      this.chkBold.UncheckedMouseOverImage = (Image) Resources.bold_over;
      this.chkBold.Click += new EventHandler(this.chkBold_Click);
      this.chkItalic.Checked = false;
      this.chkItalic.CheckedImage = (Image) Resources.italic_over;
      this.chkItalic.CheckedMouseOverImage = (Image) Resources.italic_over;
      this.chkItalic.DisabledImage = (Image) null;
      this.chkItalic.Image = (Image) Resources.italic;
      this.chkItalic.Location = new Point(284, 3);
      this.chkItalic.Margin = new Padding(0, 3, 4, 3);
      this.chkItalic.Name = "chkItalic";
      this.chkItalic.Size = new Size(16, 16);
      this.chkItalic.TabIndex = 4;
      this.chkItalic.TabStop = false;
      this.tooltip.SetToolTip((Control) this.chkItalic, "Italic");
      this.chkItalic.UncheckedImage = (Image) Resources.italic;
      this.chkItalic.UncheckedMouseOverImage = (Image) Resources.italic_over;
      this.chkItalic.Click += new EventHandler(this.chkItalic_Click);
      this.chkUnderline.Checked = false;
      this.chkUnderline.CheckedImage = (Image) Resources.underline_over;
      this.chkUnderline.CheckedMouseOverImage = (Image) Resources.underline_over;
      this.chkUnderline.DisabledImage = (Image) null;
      this.chkUnderline.Image = (Image) Resources.underline;
      this.chkUnderline.Location = new Point(304, 3);
      this.chkUnderline.Margin = new Padding(0, 3, 4, 3);
      this.chkUnderline.Name = "chkUnderline";
      this.chkUnderline.Size = new Size(16, 16);
      this.chkUnderline.TabIndex = 5;
      this.chkUnderline.TabStop = false;
      this.tooltip.SetToolTip((Control) this.chkUnderline, "Underline");
      this.chkUnderline.UncheckedImage = (Image) Resources.underline;
      this.chkUnderline.UncheckedMouseOverImage = (Image) Resources.underline_over;
      this.chkUnderline.Click += new EventHandler(this.chkUnderline_Click);
      this.btnColor.DisabledImage = (Image) null;
      this.btnColor.Image = (Image) Resources.font_color;
      this.btnColor.Location = new Point(324, 3);
      this.btnColor.Margin = new Padding(0, 3, 4, 3);
      this.btnColor.MouseDownImage = (Image) null;
      this.btnColor.MouseOverImage = (Image) Resources.font_color_over;
      this.btnColor.Name = "btnColor";
      this.btnColor.Size = new Size(16, 16);
      this.btnColor.TabIndex = 6;
      this.btnColor.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnColor, "Color");
      this.btnColor.Click += new EventHandler(this.btnColor_Click);
      this.seperator2.Location = new Point(344, 3);
      this.seperator2.Margin = new Padding(0, 3, 4, 3);
      this.seperator2.MaximumSize = new Size(2, 16);
      this.seperator2.MinimumSize = new Size(2, 16);
      this.seperator2.Name = "seperator2";
      this.seperator2.Size = new Size(2, 16);
      this.seperator2.TabIndex = 3;
      this.seperator2.TabStop = false;
      this.btnImage.DisabledImage = (Image) null;
      this.btnImage.Image = (Image) Resources.image;
      this.btnImage.Location = new Point(350, 3);
      this.btnImage.Margin = new Padding(0, 3, 4, 3);
      this.btnImage.MouseDownImage = (Image) null;
      this.btnImage.MouseOverImage = (Image) Resources.image_over;
      this.btnImage.Name = "btnImage";
      this.btnImage.Size = new Size(16, 16);
      this.btnImage.TabIndex = 8;
      this.btnImage.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnImage, "Add Image");
      this.btnImage.Click += new EventHandler(this.btnImage_Click);
      this.btnLink.DisabledImage = (Image) null;
      this.btnLink.Image = (Image) Resources.insert_hyperlink;
      this.btnLink.Location = new Point(370, 3);
      this.btnLink.Margin = new Padding(0, 3, 4, 3);
      this.btnLink.MouseDownImage = (Image) null;
      this.btnLink.MouseOverImage = (Image) Resources.insert_hyperlink_over;
      this.btnLink.Name = "btnLink";
      this.btnLink.Size = new Size(16, 16);
      this.btnLink.TabIndex = 9;
      this.btnLink.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnLink, "Add Link");
      this.btnLink.Click += new EventHandler(this.btnLink_Click);
      this.btnField.DisabledImage = (Image) null;
      this.btnField.Image = (Image) Resources.insert_field;
      this.btnField.Location = new Point(390, 3);
      this.btnField.Margin = new Padding(0, 3, 4, 3);
      this.btnField.MouseDownImage = (Image) null;
      this.btnField.MouseOverImage = (Image) Resources.insert_field_over;
      this.btnField.Name = "btnField";
      this.btnField.Size = new Size(16, 16);
      this.btnField.TabIndex = 10;
      this.btnField.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnField, "Add Field");
      this.btnField.Click += new EventHandler(this.btnField_Click);
      this.pnlBrowser.Controls.Add((Control) this.browser);
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 31);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(554, 267);
      this.pnlBrowser.TabIndex = 1;
      this.browser.AllowDrop = false;
      this.browser.ContextMenuStrip = this.contextMenu;
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(1, 1);
      this.browser.MinimumSize = new Size(20, 22);
      this.browser.Name = "browser";
      this.browser.Size = new Size(552, 265);
      this.browser.TabIndex = 0;
      this.browser.ShowContextMenu();
      this.browser.BeforeNavigation += new EventHandler<BeforeNavigationEventArgs>(this.browser_Navigating);
      this.browser.PreviewKeyDown += new PreviewKeyDownEventHandler(this.browser_PreviewKeyDown);
      this.contextMenu.Items.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.mnuEditUndo,
        (ToolStripItem) this.mnuEditRedo,
        (ToolStripItem) this.mnuItemSeparator1,
        (ToolStripItem) this.mnuEditCut,
        (ToolStripItem) this.mnuEditCopy,
        (ToolStripItem) this.mnuEditPaste,
        (ToolStripItem) this.mnuEditDelete,
        (ToolStripItem) this.mnuItemSeparator2,
        (ToolStripItem) this.mnuViewSource
      });
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new Size(139, 170);
      this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
      this.mnuEditUndo.Name = "mnuEditUndo";
      this.mnuEditUndo.Size = new Size(138, 22);
      this.mnuEditUndo.Text = "Undo";
      this.mnuEditUndo.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuEditRedo.Name = "mnuEditRedo";
      this.mnuEditRedo.Size = new Size(138, 22);
      this.mnuEditRedo.Text = "Redo";
      this.mnuEditRedo.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuItemSeparator1.Name = "mnuItemSeparator1";
      this.mnuItemSeparator1.Size = new Size(135, 6);
      this.mnuEditCut.Name = "mnuEditCut";
      this.mnuEditCut.Size = new Size(138, 22);
      this.mnuEditCut.Text = "Cut";
      this.mnuEditCut.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuEditCopy.Name = "mnuEditCopy";
      this.mnuEditCopy.Size = new Size(138, 22);
      this.mnuEditCopy.Text = "Copy";
      this.mnuEditCopy.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuEditPaste.Name = "mnuEditPaste";
      this.mnuEditPaste.Size = new Size(138, 22);
      this.mnuEditPaste.Text = "Paste";
      this.mnuEditPaste.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuEditDelete.Name = "mnuEditDelete";
      this.mnuEditDelete.Size = new Size(138, 22);
      this.mnuEditDelete.Text = "Delete";
      this.mnuEditDelete.Click += new EventHandler(this.mnuEdit_Click);
      this.mnuItemSeparator2.Name = "mnuItemSeparator2";
      this.mnuItemSeparator2.Size = new Size(135, 6);
      this.mnuViewSource.Name = "mnuViewSource";
      this.mnuViewSource.Size = new Size(138, 22);
      this.mnuViewSource.Text = "View Source";
      this.mnuViewSource.Click += new EventHandler(this.mnuViewSource_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBrowser);
      this.Controls.Add((Control) this.pnlTop);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (HtmlEmailControl);
      this.Size = new Size(554, 298);
      this.pnlTop.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.chkBold).EndInit();
      ((ISupportInitialize) this.chkItalic).EndInit();
      ((ISupportInitialize) this.chkUnderline).EndInit();
      ((ISupportInitialize) this.btnColor).EndInit();
      ((ISupportInitialize) this.btnImage).EndInit();
      ((ISupportInitialize) this.btnLink).EndInit();
      ((ISupportInitialize) this.btnField).EndInit();
      this.pnlBrowser.ResumeLayout(false);
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
