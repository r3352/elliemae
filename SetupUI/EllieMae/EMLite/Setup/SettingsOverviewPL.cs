// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsOverviewPL
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsOverviewPL : Form
  {
    private TreeView sourceTV;
    private int MaxBottomNodeNum;
    private SetUpContainer parent;
    private TreeNode currentDestinationNode;
    private TreeNode targetNode;
    private Font normalFont;
    private Font normalUnderLineFont;
    private Font boldFont;
    private Font boldUnderLineFont;
    private ImageList imgList;
    private Panel pnlRight;
    private TreeView tvBP1;
    private Panel pnlHeaderRightTop;
    private Label lblConfiguration;
    private Panel pnlMaintenance;
    private Label lblMaintenance;
    private Panel pnlBase;
    private Panel pnlCenter;
    private TreeView tvFileAccess;
    private Panel pnlRightBase;
    private IContainer components;

    public SettingsOverviewPL(TreeView tv, SetUpContainer parentForm)
    {
      this.InitializeComponent();
      this.normalFont = new Font(this.tvBP1.Font.FontFamily, this.tvBP1.Font.Size, FontStyle.Regular);
      this.normalUnderLineFont = new Font(this.normalFont.FontFamily, this.normalFont.Size, FontStyle.Underline);
      this.boldFont = this.tvBP1.Font;
      this.boldUnderLineFont = new Font(this.boldFont.FontFamily, this.boldFont.Size, FontStyle.Bold | FontStyle.Underline);
      this.parent = parentForm;
      this.sourceTV = tv;
      this.lblConfiguration.BringToFront();
      this.pnlBase.SuspendLayout();
      this.buildPersonalSettings();
      this.pnlBase.ResumeLayout(true);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SettingsOverviewPL));
      this.imgList = new ImageList(this.components);
      this.pnlMaintenance = new Panel();
      this.lblMaintenance = new Label();
      this.pnlRight = new Panel();
      this.tvFileAccess = new TreeView();
      this.lblConfiguration = new Label();
      this.pnlHeaderRightTop = new Panel();
      this.tvBP1 = new TreeView();
      this.pnlBase = new Panel();
      this.pnlRightBase = new Panel();
      this.pnlCenter = new Panel();
      this.pnlMaintenance.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlHeaderRightTop.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.pnlRightBase.SuspendLayout();
      this.pnlCenter.SuspendLayout();
      this.SuspendLayout();
      this.imgList.ColorDepth = ColorDepth.Depth8Bit;
      this.imgList.ImageSize = new Size(16, 16);
      this.imgList.TransparentColor = Color.Transparent;
      this.pnlMaintenance.BackgroundImage = (Image) componentResourceManager.GetObject("pnlMaintenance.BackgroundImage");
      this.pnlMaintenance.Controls.Add((Control) this.lblMaintenance);
      this.pnlMaintenance.Dock = DockStyle.Top;
      this.pnlMaintenance.Location = new Point(0, 0);
      this.pnlMaintenance.Name = "pnlMaintenance";
      this.pnlMaintenance.Size = new Size(558, 21);
      this.pnlMaintenance.TabIndex = 4;
      this.lblMaintenance.BackColor = Color.Transparent;
      this.lblMaintenance.Dock = DockStyle.Fill;
      this.lblMaintenance.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMaintenance.ForeColor = Color.White;
      this.lblMaintenance.Location = new Point(0, 0);
      this.lblMaintenance.Name = "lblMaintenance";
      this.lblMaintenance.Size = new Size(558, 21);
      this.lblMaintenance.TabIndex = 0;
      this.lblMaintenance.Text = "Maintenance";
      this.lblMaintenance.TextAlign = ContentAlignment.MiddleCenter;
      this.pnlRight.BackColor = Color.FromArgb((int) byte.MaxValue, 192, 128);
      this.pnlRight.Controls.Add((Control) this.pnlMaintenance);
      this.pnlRight.Controls.Add((Control) this.tvFileAccess);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(0, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(558, 510);
      this.pnlRight.TabIndex = 1;
      this.tvFileAccess.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvFileAccess.BackColor = Color.FromArgb((int) byte.MaxValue, 192, 128);
      this.tvFileAccess.BorderStyle = BorderStyle.None;
      this.tvFileAccess.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvFileAccess.Location = new Point(8, 27);
      this.tvFileAccess.Name = "tvFileAccess";
      this.tvFileAccess.Scrollable = false;
      this.tvFileAccess.ShowLines = false;
      this.tvFileAccess.ShowPlusMinus = false;
      this.tvFileAccess.ShowRootLines = false;
      this.tvFileAccess.Size = new Size(535, 470);
      this.tvFileAccess.TabIndex = 0;
      this.tvFileAccess.BeforeCollapse += new TreeViewCancelEventHandler(this.tvFileAccess_BeforeCollapse);
      this.tvFileAccess.MouseUp += new MouseEventHandler(this.tvFileAccess_MouseUp);
      this.tvFileAccess.MouseMove += new MouseEventHandler(this.tvFileAccess_MouseMove);
      this.tvFileAccess.MouseDown += new MouseEventHandler(this.tvFileAccess_MouseDown);
      this.lblConfiguration.BackColor = Color.Transparent;
      this.lblConfiguration.Dock = DockStyle.Fill;
      this.lblConfiguration.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblConfiguration.ForeColor = Color.White;
      this.lblConfiguration.Location = new Point(0, 0);
      this.lblConfiguration.Name = "lblConfiguration";
      this.lblConfiguration.Size = new Size(200, 21);
      this.lblConfiguration.TabIndex = 1;
      this.lblConfiguration.Text = "Configuration";
      this.lblConfiguration.TextAlign = ContentAlignment.MiddleCenter;
      this.pnlHeaderRightTop.BackColor = Color.Transparent;
      this.pnlHeaderRightTop.BackgroundImage = (Image) componentResourceManager.GetObject("pnlHeaderRightTop.BackgroundImage");
      this.pnlHeaderRightTop.Controls.Add((Control) this.lblConfiguration);
      this.pnlHeaderRightTop.Dock = DockStyle.Top;
      this.pnlHeaderRightTop.Location = new Point(0, 0);
      this.pnlHeaderRightTop.Name = "pnlHeaderRightTop";
      this.pnlHeaderRightTop.Size = new Size(200, 21);
      this.pnlHeaderRightTop.TabIndex = 3;
      this.tvBP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvBP1.BackColor = Color.FromArgb(252, 206, 137);
      this.tvBP1.BorderStyle = BorderStyle.None;
      this.tvBP1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tvBP1.Location = new Point(6, 27);
      this.tvBP1.Name = "tvBP1";
      this.tvBP1.Scrollable = false;
      this.tvBP1.ShowLines = false;
      this.tvBP1.ShowPlusMinus = false;
      this.tvBP1.ShowRootLines = false;
      this.tvBP1.Size = new Size(188, 469);
      this.tvBP1.TabIndex = 1;
      this.tvBP1.BeforeCollapse += new TreeViewCancelEventHandler(this.tvBP1_BeforeCollapse);
      this.tvBP1.MouseUp += new MouseEventHandler(this.tvBP1_MouseUp);
      this.tvBP1.MouseMove += new MouseEventHandler(this.tvBP1_MouseMove);
      this.tvBP1.MouseDown += new MouseEventHandler(this.tvBP1_MouseDown);
      this.pnlBase.AutoScroll = true;
      this.pnlBase.Controls.Add((Control) this.pnlRightBase);
      this.pnlBase.Controls.Add((Control) this.pnlCenter);
      this.pnlBase.Dock = DockStyle.Fill;
      this.pnlBase.Location = new Point(0, 0);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(758, 510);
      this.pnlBase.TabIndex = 0;
      this.pnlRightBase.Controls.Add((Control) this.pnlRight);
      this.pnlRightBase.Dock = DockStyle.Fill;
      this.pnlRightBase.Location = new Point(200, 0);
      this.pnlRightBase.Name = "pnlRightBase";
      this.pnlRightBase.Size = new Size(558, 510);
      this.pnlRightBase.TabIndex = 5;
      this.pnlCenter.BackColor = Color.FromArgb(252, 206, 137);
      this.pnlCenter.Controls.Add((Control) this.tvBP1);
      this.pnlCenter.Controls.Add((Control) this.pnlHeaderRightTop);
      this.pnlCenter.Dock = DockStyle.Left;
      this.pnlCenter.Location = new Point(0, 0);
      this.pnlCenter.Name = "pnlCenter";
      this.pnlCenter.Size = new Size(200, 510);
      this.pnlCenter.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(758, 510);
      this.Controls.Add((Control) this.pnlBase);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (SettingsOverviewPL);
      this.Text = "Personal Settings";
      this.pnlMaintenance.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlHeaderRightTop.ResumeLayout(false);
      this.pnlBase.ResumeLayout(false);
      this.pnlRightBase.ResumeLayout(false);
      this.pnlCenter.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void buildPersonalSettings()
    {
      int[] numArray = new int[1];
      this.tvBP1.BeginUpdate();
      this.tvFileAccess.BeginUpdate();
      foreach (TreeNode node in this.sourceTV.Nodes)
      {
        if (node.Text == "Personal Settings")
          numArray[0] += SettingsOverview.BuildTree(this.tvBP1, node, this.normalFont);
      }
      this.tvBP1.ExpandAll();
      this.tvFileAccess.ExpandAll();
      this.tvFileAccess.EndUpdate();
      this.tvBP1.EndUpdate();
      for (int index = 0; index < 1; ++index)
      {
        if (numArray[index] > this.MaxBottomNodeNum)
          this.MaxBottomNodeNum = numArray[index];
      }
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

    private void tvFileAccess_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvFileAccess.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.targetNode = nodeAt;
    }

    private void tvFileAccess_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvFileAccess.GetNodeAt(e.X, e.Y);
      this.tvFileAccess.Cursor = nodeAt != null ? Cursors.Hand : Cursors.Default;
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

    private void tvFileAccess_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.targetNode == null)
        return;
      this.parent.ShowSelectedLink(this.targetNode);
    }

    private void tvFileAccess_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
    {
      e.Cancel = true;
    }
  }
}
