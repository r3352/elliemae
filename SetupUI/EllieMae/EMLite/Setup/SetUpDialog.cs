// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SetUpDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class SetUpDialog : UserControl
  {
    private Panel contentPanel;
    private IContainer components;
    private GradientPanel gradientPanel2;
    private Label lblSubTitle;
    private GradientPanel gpHeader;
    private Label lblHeader;
    private StandardIconButton stdIconBtnReset;
    private StandardIconButton stdIconBtnSave;
    private ToolTip toolTip;
    public Control dialog;

    public SetUpDialog()
    {
      this.InitializeComponent();
      this.gradientPanel2.GradientColor1 = EncompassColors.Neutral5;
      this.gradientPanel2.GradientColor2 = EncompassColors.Secondary2;
      this.Dock = DockStyle.Fill;
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
      this.contentPanel = new Panel();
      this.toolTip = new ToolTip(this.components);
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnSave = new StandardIconButton();
      this.gradientPanel2 = new GradientPanel();
      this.lblSubTitle = new Label();
      this.gpHeader = new GradientPanel();
      this.lblHeader = new Label();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      this.gradientPanel2.SuspendLayout();
      this.gpHeader.SuspendLayout();
      this.SuspendLayout();
      this.contentPanel.AutoScroll = true;
      this.contentPanel.BackColor = SystemColors.ControlLightLight;
      this.contentPanel.Dock = DockStyle.Fill;
      this.contentPanel.Location = new Point(0, 62);
      this.contentPanel.Name = "contentPanel";
      this.contentPanel.Size = new Size(1213, 494);
      this.contentPanel.TabIndex = 1;
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(1187, 8);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 2;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.saveResetIconBtns_Click);
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(1165, 9);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 1;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.saveResetIconBtns_Click);
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblSubTitle);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.gradientPanel2.Location = new Point(0, 31);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(1213, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 4;
      this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubTitle.AutoEllipsis = true;
      this.lblSubTitle.BackColor = Color.Transparent;
      this.lblSubTitle.Location = new Point(10, 9);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(1193, 14);
      this.lblSubTitle.TabIndex = 0;
      this.lblSubTitle.Text = "subTitle";
      this.lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.gpHeader.BackColor = Color.WhiteSmoke;
      this.gpHeader.BackColorGlassyStyle = true;
      this.gpHeader.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpHeader.Controls.Add((Control) this.stdIconBtnReset);
      this.gpHeader.Controls.Add((Control) this.stdIconBtnSave);
      this.gpHeader.Controls.Add((Control) this.lblHeader);
      this.gpHeader.Dock = DockStyle.Top;
      this.gpHeader.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gpHeader.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gpHeader.Location = new Point(0, 0);
      this.gpHeader.Name = "gpHeader";
      this.gpHeader.Size = new Size(1213, 31);
      this.gpHeader.Style = GradientPanel.PanelStyle.PageHeader;
      this.gpHeader.TabIndex = 5;
      this.lblHeader.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHeader.BackColor = Color.Transparent;
      this.lblHeader.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblHeader.Location = new Point(10, 4);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(1149, 24);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "pageHeader";
      this.lblHeader.TextAlign = ContentAlignment.MiddleLeft;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.contentPanel);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gpHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SetUpDialog);
      this.Size = new Size(1213, 556);
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      this.gradientPanel2.ResumeLayout(false);
      this.gpHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void saveResetIconBtns_Click(object sender, EventArgs e)
    {
      if (!(this.contentPanel.Controls[0] is SettingsUserControl control))
      {
        int num = (int) MessageBox.Show("Not valid.");
      }
      else
      {
        StandardIconButton standardIconButton = sender as StandardIconButton;
        if (standardIconButton.StandardButtonType == StandardIconButton.ButtonType.SaveButton)
        {
          control.Save();
        }
        else
        {
          if (standardIconButton.StandardButtonType != StandardIconButton.ButtonType.ResetButton || ResetConfirmDialog.ShowDialog((IWin32Window) this.TopLevelControl, (string) null) == DialogResult.No)
            return;
          control.Reset();
        }
      }
    }

    public bool ButtonSaveEnabled
    {
      get => this.stdIconBtnSave.Enabled;
      set => this.stdIconBtnSave.Enabled = value;
    }

    public bool ButtonResetEnabled
    {
      get => this.stdIconBtnReset.Enabled;
      set => this.stdIconBtnReset.Enabled = value;
    }

    public Control CurrentContent
    {
      get => this.contentPanel.Controls.Count > 0 ? this.contentPanel.Controls[0] : (Control) null;
    }

    public void SetSubTitle(string subTitle) => this.lblSubTitle.Text = subTitle;

    public void ChangeContent(
      string title,
      string subTitle,
      SetupPage.ContainerSaveResetButtons saveResetBtns,
      Control control)
    {
      this.lblHeader.Text = title;
      this.lblSubTitle.Text = subTitle;
      if (string.IsNullOrEmpty(subTitle.Trim()))
        this.gradientPanel2.Visible = false;
      else
        this.gradientPanel2.Visible = true;
      this.dialog = control;
      switch (saveResetBtns)
      {
        case SetupPage.ContainerSaveResetButtons.ShowBoth:
          this.stdIconBtnSave.Visible = this.stdIconBtnReset.Visible = true;
          break;
        case SetupPage.ContainerSaveResetButtons.ShowBothButDisableReset:
          this.stdIconBtnReset.Enabled = false;
          goto case SetupPage.ContainerSaveResetButtons.ShowBoth;
        case SetupPage.ContainerSaveResetButtons.DoNotShow:
          this.stdIconBtnSave.Visible = this.stdIconBtnReset.Visible = false;
          break;
      }
      this.dialog.Visible = false;
      this.dialog.BackColor = Color.WhiteSmoke;
      this.dialog.Dock = DockStyle.Fill;
      if (this.dialog is Form dialog)
      {
        dialog.TopLevel = false;
        dialog.Parent = (Control) this.contentPanel;
      }
      this.contentPanel.Controls.Clear();
      this.contentPanel.Controls.Add(this.dialog);
      this.dialog.Visible = true;
    }

    public void RemoveContent() => this.contentPanel.Controls.Clear();

    public bool TopBorder
    {
      get => (this.gpHeader.Borders & AnchorStyles.Top) == AnchorStyles.Top;
      set
      {
        if (value)
          this.gpHeader.Borders |= AnchorStyles.Top;
        else
          this.gpHeader.Borders = this.gpHeader.Borders & AnchorStyles.Left | this.gpHeader.Borders & AnchorStyles.Right | this.gpHeader.Borders & AnchorStyles.Bottom;
      }
    }

    public bool SubTitleBottomBorder
    {
      get => (this.gradientPanel2.Borders & AnchorStyles.Bottom) == AnchorStyles.Bottom;
      set
      {
        if (value)
          this.gradientPanel2.Borders |= AnchorStyles.Bottom;
        else
          this.gradientPanel2.Borders = this.gradientPanel2.Borders & AnchorStyles.Left | this.gradientPanel2.Borders & AnchorStyles.Right | this.gradientPanel2.Borders & AnchorStyles.Top;
      }
    }

    public void AlignSubTitle(int xLocation, int yLocation)
    {
      this.lblSubTitle.AutoSize = true;
      this.lblSubTitle.Location = new Point(xLocation, yLocation);
      this.lblSubTitle.Size = new Size(800, 14);
    }
  }
}
