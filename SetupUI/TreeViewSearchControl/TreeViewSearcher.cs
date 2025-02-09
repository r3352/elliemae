// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.TreeViewSearcher
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TreeViewSearchProvider;

#nullable disable
namespace TreeViewSearchControl
{
  public class TreeViewSearcher : UserControl
  {
    private TreeViewSearchManager _tvpm;
    private List<TreeNode> _matchedNodes;
    private int _nodeCounter;
    private bool _isSearchComplete;
    private UIController _uic;
    private IMessageWriter _msg;
    private IContainer components;
    private Panel pnlMain;
    private Label lblSearch;
    private Label lblMessage;
    private TextBox txtSearch;
    private Button cmdSearch;
    private Button cmdClear;
    private ImageList ilIcons;
    private Button cmdSettings;
    private ToolTip ttTips;
    private FlowLayoutPanel flpSearchOptions;
    private FlowLayoutPanel flpSearchInputs;
    private FlowLayoutPanel flpSearchControls;
    private ComboBox cmbHighlight;

    public TreeViewSearchManager SearchProvider => this._tvpm;

    public Publisher UICEvents { get; } = new Publisher();

    public TreeViewSearcher()
      : this(UIController.Custom)
    {
    }

    public TreeViewSearcher(UIController uiControls)
    {
      this.InitializeComponent();
      this._uic = uiControls;
      this.initializeControlCenter();
      this.initializeTVPM();
    }

    private void populateControls()
    {
      this.cmbHighlight.SelectedIndexChanged -= new System.EventHandler(this.cmbHighlight_SelectedIndexChanged);
      this.cmbHighlight.BindEnumToComboBox<NodeFormatSettings.Highlight>(this._tvpm.NodeFormat.Highlighting);
      this.cmbHighlight.SelectedIndexChanged += new System.EventHandler(this.cmbHighlight_SelectedIndexChanged);
    }

    private void initializeControlCenter()
    {
      if (this.lblSearch.Visible = this._uic.SearchLabel)
        this.lblSearch.Text = this._uic.SearchLabelText;
      if ((this.cmdSearch.Visible = this._uic.SearchButton) && !this._uic.SearchButtonImage)
      {
        this.cmdSearch.ImageIndex = -1;
        this.cmdSearch.Text = this._uic.SearchButtonText;
        this.cmdSearch.Width = 50;
      }
      if ((this.cmdClear.Visible = this._uic.ClearButton) && !this._uic.ClearButtonImage)
      {
        this.cmdClear.ImageIndex = -1;
        this.cmdClear.Text = this._uic.ClearButtonText;
        this.cmdClear.Width = 40;
      }
      this.cmdSettings.Visible = this._uic.SettingsButton;
      if (this._uic.SearchTextboxWitdh > 0)
        this.txtSearch.Width = this._uic.SearchTextboxWitdh;
      this.Width = this.flpSearchControls.Width;
      switch (this._uic.MessageMedium)
      {
        case MessageMedium.Ghost:
          this._msg = MessageWriterFactory.GetGhostWriter();
          this.lblMessage.Visible = false;
          this.Height -= this.lblMessage.Height;
          break;
        case MessageMedium.Tooltip:
          this._msg = MessageWriterFactory.GetToolTipWriter(this.ttTips, (IWin32Window) this.txtSearch, this.getToolTipLocation(), 1500);
          this.lblMessage.Visible = false;
          this.Height -= this.lblMessage.Height;
          break;
        case MessageMedium.Label:
          this._msg = MessageWriterFactory.GetLabelWriter(this.lblMessage);
          this.lblMessage.Location = new Point(this.txtSearch.Location.X, this.lblMessage.Location.Y);
          break;
      }
      if (this._uic.ParentForm != null)
      {
        this._uic.ParentForm.KeyPreview = true;
        this._uic.ParentForm.KeyUp += new KeyEventHandler(this.ParentForm_KeyUp);
        this.ttTips.SetToolTip((Control) this.txtSearch, string.Format("Search ({0})", (object) this._uic.Key));
      }
      this.resetLocalSearchValues();
    }

    private void initializeTVPM()
    {
      this._tvpm = new TreeViewSearchManager();
      this._tvpm.SearchEvents.ValidationError += new Publisher.ValidationErrorHandler(this.Publish_ValidationError);
      this._tvpm.SearchEvents.EndOfSearch += new Publisher.SearchHandler(this.Publish_EndOfSearch);
      this._tvpm.SearchEvents.MatchFound += new Publisher.SearchHandler(this.Publish_MatchFound);
    }

    public void AddTrees(List<TreeView> treeViews, bool DoClear = false)
    {
      this._tvpm.AddTrees(treeViews, DoClear);
    }

    public void ClearTrees() => this._tvpm.ClearTrees();

    public void SetFocus() => this.txtSearch.Focus();

    private void TreeViewSearcher_Load(object sender, EventArgs e) => this.populateControls();

    private void cmdSearch_Click(object sender, EventArgs e) => this.searchAndFormat();

    private void cmdClear_Click(object sender, EventArgs e)
    {
      this.txtSearch.Clear();
      this.resetAll();
      this.txtSearch.Focus();
    }

    private void cmdSettings_Click(object sender, EventArgs e)
    {
      using (Settings settings = new Settings(this._tvpm))
      {
        settings.Location = this.getSettingsDialogLocation(settings.Size);
        if (DialogResult.OK != settings.ShowDialog((IWin32Window) this))
          return;
        this.resetLocalSearchValues();
      }
    }

    private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.searchAndFormat();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      this.resetAll();
      if (!this._uic.RealTime || string.IsNullOrEmpty(this.txtSearch.Text))
        return;
      this.searchAndFormat();
    }

    private void ParentForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != this._uic.Key)
        return;
      this.searchAndFormat();
    }

    private void cmbHighlight_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._tvpm.NodeFormat.Highlighting = (NodeFormatSettings.Highlight) this.cmbHighlight.SelectedValue;
      this.resetLocalFormatValues();
      this.applyFormat();
    }

    private void Publish_ValidationError(string message) => this._msg.Write(message);

    private void Publish_EndOfSearch(List<TreeNode> matchedTreeNodes, string message)
    {
      this._matchedNodes = matchedTreeNodes;
      this._isSearchComplete = true;
    }

    private void Publish_MatchFound(List<TreeNode> matchedNodes, string message)
    {
      this._matchedNodes = matchedNodes;
    }

    private void searchAndFormat()
    {
      if (!this._isSearchComplete)
        this.search();
      this.applyFormat();
    }

    private void search()
    {
      this._tvpm.Search(this.txtSearch.Text);
      this.txtSearch.Focus();
    }

    private void applyFormat()
    {
      if (this._matchedNodes == null)
        return;
      if (this._matchedNodes.Count == 0)
        this._msg.Write(this.getMessage(3, this._matchedNodes.Count));
      else if (this._tvpm.SearchParams.Occurances == SearchParameters.Occurance.NextOne || this._tvpm.NodeFormat.Highlighting == NodeFormatSettings.Highlight.NextOne)
      {
        if (this._nodeCounter >= this._matchedNodes.Count)
          this._nodeCounter = 0;
        this._tvpm.ApplyFormat(this._matchedNodes[this._nodeCounter]);
        if (this._tvpm.SearchParams.Occurances == SearchParameters.Occurance.NextOne)
          this._msg.Write(this.getMessage(1, this._nodeCounter + 1));
        else if (this._nodeCounter + 1 < this._matchedNodes.Count)
          this._msg.Write(this.getMessage(2, this._matchedNodes.Count, this._nodeCounter + 1));
        else
          this._msg.Write(this.getMessage(4, this._matchedNodes.Count, this._nodeCounter + 1));
        this.UICEvents.OnNext(this._matchedNodes[this._nodeCounter]);
        ++this._nodeCounter;
      }
      else
      {
        this._tvpm.ApplyFormat(this._matchedNodes);
        this.UICEvents.OnNext(this._matchedNodes[this._matchedNodes.Count - 1]);
        this._msg.Write(this.getMessage(3, this._matchedNodes.Count));
      }
    }

    private string getMessage(int msgType, int nodeCount, int traverseNodeCount = 0)
    {
      string message = (string) null;
      switch (msgType)
      {
        case 1:
          string str1 = nodeCount != 1 ? string.Format("{0} matches found.", (object) nodeCount) : string.Format("{0} match found.", (object) nodeCount);
          message = this._uic.Key != Keys.None ? string.Format("{0} {1} for next.", (object) str1, (object) this._uic.Key) : string.Format("{0} Find next.", (object) str1);
          break;
        case 2:
          string str2 = nodeCount != 1 ? string.Format("{0} of {1} matches.", (object) traverseNodeCount, (object) nodeCount) : string.Format("{0} of {1} match.", (object) traverseNodeCount, (object) nodeCount);
          message = this._uic.Key != Keys.None ? string.Format("{0} {1} for next.", (object) str2, (object) this._uic.Key) : string.Format("{0} Find next.", (object) str2);
          break;
        case 3:
          message = string.Format("{0} Search complete.", nodeCount != 1 ? (object) string.Format("{0} matches found.", (object) nodeCount) : (object) string.Format("{0} match found.", (object) nodeCount));
          break;
        case 4:
          message = string.Format("{0} Search complete.", nodeCount != 1 ? (object) string.Format("{0} of {1} matches.", (object) traverseNodeCount, (object) nodeCount) : (object) string.Format("{0} of {1} match.", (object) traverseNodeCount, (object) nodeCount));
          break;
      }
      return message;
    }

    private void resetAll()
    {
      this.resetLocalSearchValues();
      this._tvpm.ResetSearch();
    }

    private void resetLocalSearchValues()
    {
      this._isSearchComplete = false;
      this._matchedNodes = (List<TreeNode>) null;
      this.resetLocalFormatValues();
    }

    private void resetLocalFormatValues()
    {
      this._msg.Write(string.Empty);
      this._nodeCounter = 0;
    }

    private Point getSettingsDialogLocation(Size settingsSize)
    {
      Size size = Screen.GetWorkingArea((Control) this).Size;
      Point screen = this.cmdSettings.PointToScreen(new Point(0, 0));
      int x = screen.X + this.cmdSettings.Width;
      int y = screen.Y;
      if (screen.X + this.cmdSettings.Width + settingsSize.Width > size.Width)
      {
        x = screen.X + this.cmdSettings.Width - settingsSize.Width;
        y = screen.Y + this.cmdSettings.Height;
      }
      if (screen.Y + this.cmdSettings.Height + settingsSize.Height > size.Height)
        y = screen.Y - settingsSize.Height;
      return new Point(x, y);
    }

    private Point getToolTipLocation() => new Point(0, this.txtSearch.Height);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TreeViewSearcher));
      this.pnlMain = new Panel();
      this.flpSearchControls = new FlowLayoutPanel();
      this.flpSearchInputs = new FlowLayoutPanel();
      this.lblSearch = new Label();
      this.txtSearch = new TextBox();
      this.flpSearchOptions = new FlowLayoutPanel();
      this.cmdSearch = new Button();
      this.ilIcons = new ImageList(this.components);
      this.cmdClear = new Button();
      this.cmdSettings = new Button();
      this.cmbHighlight = new ComboBox();
      this.lblMessage = new Label();
      this.ttTips = new ToolTip(this.components);
      this.pnlMain.SuspendLayout();
      this.flpSearchControls.SuspendLayout();
      this.flpSearchInputs.SuspendLayout();
      this.flpSearchOptions.SuspendLayout();
      this.SuspendLayout();
      this.pnlMain.Controls.Add((Control) this.flpSearchControls);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 0);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(615, 29);
      this.pnlMain.TabIndex = 0;
      this.flpSearchControls.AutoSize = true;
      this.flpSearchControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpSearchControls.BackColor = Color.Transparent;
      this.flpSearchControls.Controls.Add((Control) this.flpSearchInputs);
      this.flpSearchControls.Controls.Add((Control) this.flpSearchOptions);
      this.flpSearchControls.Controls.Add((Control) this.lblMessage);
      this.flpSearchControls.Dock = DockStyle.Fill;
      this.flpSearchControls.Location = new Point(0, 0);
      this.flpSearchControls.Name = "flpSearchControls";
      this.flpSearchControls.Size = new Size(615, 29);
      this.flpSearchControls.TabIndex = 3;
      this.flpSearchInputs.AutoSize = true;
      this.flpSearchInputs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpSearchInputs.BackColor = Color.Transparent;
      this.flpSearchInputs.Controls.Add((Control) this.lblSearch);
      this.flpSearchInputs.Controls.Add((Control) this.txtSearch);
      this.flpSearchInputs.Location = new Point(0, 2);
      this.flpSearchInputs.Margin = new Padding(0, 2, 0, 0);
      this.flpSearchInputs.Name = "flpSearchInputs";
      this.flpSearchInputs.Size = new Size(202, 22);
      this.flpSearchInputs.TabIndex = 2;
      this.lblSearch.AutoSize = true;
      this.lblSearch.Location = new Point(0, 4);
      this.lblSearch.Margin = new Padding(0, 4, 0, 0);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new Size(50, 13);
      this.lblSearch.TabIndex = 0;
      this.lblSearch.Text = "@search";
      this.lblSearch.TextAlign = ContentAlignment.MiddleCenter;
      this.txtSearch.Location = new Point(51, 1);
      this.txtSearch.Margin = new Padding(1);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(150, 20);
      this.txtSearch.TabIndex = 1;
      this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
      this.txtSearch.KeyPress += new KeyPressEventHandler(this.txtSearch_KeyPress);
      this.flpSearchOptions.AutoSize = true;
      this.flpSearchOptions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpSearchOptions.BackColor = Color.Transparent;
      this.flpSearchOptions.Controls.Add((Control) this.cmdSearch);
      this.flpSearchOptions.Controls.Add((Control) this.cmdClear);
      this.flpSearchOptions.Controls.Add((Control) this.cmdSettings);
      this.flpSearchOptions.Controls.Add((Control) this.cmbHighlight);
      this.flpSearchOptions.Location = new Point(204, 1);
      this.flpSearchOptions.Margin = new Padding(2, 1, 4, 0);
      this.flpSearchOptions.Name = "flpSearchOptions";
      this.flpSearchOptions.Size = new Size(178, 24);
      this.flpSearchOptions.TabIndex = 0;
      this.cmdSearch.ImageIndex = 0;
      this.cmdSearch.ImageList = this.ilIcons;
      this.cmdSearch.Location = new Point(1, 1);
      this.cmdSearch.Margin = new Padding(1);
      this.cmdSearch.Name = "cmdSearch";
      this.cmdSearch.Size = new Size(30, 22);
      this.cmdSearch.TabIndex = 2;
      this.ttTips.SetToolTip((Control) this.cmdSearch, "Search");
      this.cmdSearch.UseVisualStyleBackColor = true;
      this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
      this.ilIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilIcons.ImageStream");
      this.ilIcons.TransparentColor = Color.Transparent;
      this.ilIcons.Images.SetKeyName(0, "search.png");
      this.ilIcons.Images.SetKeyName(1, "search-over.png");
      this.ilIcons.Images.SetKeyName(2, "search-disabled.png");
      this.ilIcons.Images.SetKeyName(3, "delete.png");
      this.ilIcons.Images.SetKeyName(4, "delete-ovr.png");
      this.ilIcons.Images.SetKeyName(5, "delete-disabled.png");
      this.cmdClear.ImageIndex = 3;
      this.cmdClear.ImageList = this.ilIcons;
      this.cmdClear.Location = new Point(33, 1);
      this.cmdClear.Margin = new Padding(1);
      this.cmdClear.Name = "cmdClear";
      this.cmdClear.Size = new Size(30, 22);
      this.cmdClear.TabIndex = 3;
      this.ttTips.SetToolTip((Control) this.cmdClear, "Clear");
      this.cmdClear.UseVisualStyleBackColor = true;
      this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
      this.cmdSettings.ImageIndex = 0;
      this.cmdSettings.Location = new Point(65, 1);
      this.cmdSettings.Margin = new Padding(1);
      this.cmdSettings.Name = "cmdSettings";
      this.cmdSettings.Size = new Size(30, 22);
      this.cmdSettings.TabIndex = 4;
      this.cmdSettings.Text = "...";
      this.ttTips.SetToolTip((Control) this.cmdSettings, "Settings");
      this.cmdSettings.UseVisualStyleBackColor = true;
      this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
      this.cmbHighlight.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbHighlight.FormattingEnabled = true;
      this.cmbHighlight.Location = new Point(97, 2);
      this.cmbHighlight.Margin = new Padding(1, 2, 1, 1);
      this.cmbHighlight.Name = "cmbHighlight";
      this.cmbHighlight.Size = new Size(80, 21);
      this.cmbHighlight.TabIndex = 5;
      this.cmbHighlight.SelectedIndexChanged += new System.EventHandler(this.cmbHighlight_SelectedIndexChanged);
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new Point(387, 7);
      this.lblMessage.Margin = new Padding(1, 7, 1, 1);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(60, 13);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "@message";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlMain);
      this.Name = nameof (TreeViewSearcher);
      this.Size = new Size(615, 29);
      this.Load += new System.EventHandler(this.TreeViewSearcher_Load);
      this.pnlMain.ResumeLayout(false);
      this.pnlMain.PerformLayout();
      this.flpSearchControls.ResumeLayout(false);
      this.flpSearchControls.PerformLayout();
      this.flpSearchInputs.ResumeLayout(false);
      this.flpSearchInputs.PerformLayout();
      this.flpSearchOptions.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
