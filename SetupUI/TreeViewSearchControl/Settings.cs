// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.Settings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using TreeViewSearchProvider;

#nullable disable
namespace TreeViewSearchControl
{
  public class Settings : Form
  {
    private TreeViewSearchManager tvpm;
    private SearchParameters sp;
    private NodeFormatSettings nfs;
    private bool isDirty;
    private IContainer components;
    private Panel pnlMain;
    private GroupBox gbSearchOptions;
    private CheckBox chkMatchCase;
    private CheckBox chkMatchWord;
    private GroupBox gbFromattingOptions;
    private Label lblBackColor;
    private Label lblForeColor;
    private Panel pnlCommands;
    private Button cmdRestoreDefault;
    private Button cmdClose;
    private ColorDialog cdPicker;
    private Label lblForeColorDisplay;
    private Label lblBackColorDisplay;
    private ImageList ilIcons;
    private Label lblOccurances;
    private ComboBox cmbOccurances;
    private ToolTip ttTips;
    private CheckBox chkBlink;
    private Label lblBlinkRepeat;
    private Panel pnlBlinkOptions;
    private Label lblBlinkSpeed;
    private NumericUpDown nudBlinkRepeat;
    private NumericUpDown nudBlinkSpeed;
    private CheckBox chkClearTrail;
    private NumericUpDown nudClearTrailAfter;

    public Settings(TreeViewSearchManager tvpm)
    {
      this.InitializeComponent();
      this.tvpm = tvpm;
      this.sp = tvpm.SearchParams;
      this.nfs = tvpm.NodeFormat;
      this.populateControls();
    }

    private void populateControls()
    {
      this.chkMatchWord.Checked = this.sp.TextOptions.HasFlag((Enum) SearchParameters.TextOption.MatchWord);
      this.chkMatchCase.Checked = this.sp.TextOptions.HasFlag((Enum) SearchParameters.TextOption.MatchCase);
      this.populateOccuranceControls();
      this.lblForeColorDisplay.BackColor = this.nfs.Color.ForeColor;
      this.lblBackColorDisplay.BackColor = this.nfs.Color.BackColor;
      this.populateBlinkControls();
      this.populateClearTrailControl();
      this.registerIsDirty(this.gbSearchOptions.Controls);
    }

    private void populateOccuranceControls()
    {
      this.cmbOccurances.SelectedIndexChanged -= new System.EventHandler(this.cmbOccurances_SelectedIndexChanged);
      this.cmbOccurances.BindEnumToComboBox<SearchParameters.Occurance>(this.sp.Occurances);
      this.cmbOccurances.SelectedIndexChanged += new System.EventHandler(this.cmbOccurances_SelectedIndexChanged);
    }

    private void populateBlinkControls()
    {
      this.nudBlinkRepeat.ValueChanged -= new System.EventHandler(this.nudBlinkRepeat_ValueChanged);
      this.nudBlinkSpeed.ValueChanged -= new System.EventHandler(this.nudBlinkSpeed_ValueChanged);
      this.chkBlink.CheckedChanged -= new System.EventHandler(this.chkBlink_CheckedChanged);
      this.chkBlink.Checked = this.pnlBlinkOptions.Enabled = this.nfs.Blink.CanBlink;
      this.nudBlinkRepeat.Value = (Decimal) this.nfs.Blink.Repeat;
      this.nudBlinkSpeed.Value = (Decimal) this.nfs.Blink.SpeedInMilliseconds;
      this.chkBlink.CheckedChanged += new System.EventHandler(this.chkBlink_CheckedChanged);
      this.nudBlinkRepeat.ValueChanged += new System.EventHandler(this.nudBlinkRepeat_ValueChanged);
      this.nudBlinkSpeed.ValueChanged += new System.EventHandler(this.nudBlinkSpeed_ValueChanged);
    }

    private void populateClearTrailControl()
    {
      this.nudClearTrailAfter.ValueChanged -= new System.EventHandler(this.nudClearTrailAfter_ValueChanged);
      this.chkClearTrail.CheckedChanged -= new System.EventHandler(this.chkClearTrail_CheckedChanged);
      this.chkClearTrail.Checked = this.nudClearTrailAfter.Enabled = this.nfs.ClearTrail.CanClear;
      this.nudClearTrailAfter.Value = (Decimal) this.nfs.ClearTrail.DelayInMilliseconds;
      this.chkClearTrail.CheckedChanged += new System.EventHandler(this.chkClearTrail_CheckedChanged);
      this.nudClearTrailAfter.ValueChanged += new System.EventHandler(this.nudClearTrailAfter_ValueChanged);
    }

    private void cmdRestoreDefault_Click(object sender, EventArgs e)
    {
      this.tvpm.RestoreDefaultSettings();
      this.DialogResult = DialogResult.OK;
    }

    private void cmdClose_Click(object sender, EventArgs e)
    {
      if (!this.isDirty)
        return;
      this.tvpm.ResetSearch();
      this.DialogResult = DialogResult.OK;
    }

    private void chkMatchWord_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkMatchWord.Checked)
        this.sp.TextOptions |= SearchParameters.TextOption.MatchWord;
      else
        --this.sp.TextOptions;
    }

    private void chkMatchCase_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkMatchCase.Checked)
        this.sp.TextOptions |= SearchParameters.TextOption.MatchCase;
      else
        this.sp.TextOptions -= SearchParameters.TextOption.MatchCase;
    }

    private void lblForeColorDisplay_Click(object sender, EventArgs e)
    {
      this.cdPicker.Color = this.nfs.Color.ForeColor;
      if (this.cdPicker.ShowDialog() != DialogResult.OK)
        return;
      this.nfs.Color.ForeColor = this.lblForeColorDisplay.BackColor = this.cdPicker.Color;
    }

    private void lblBackColorDisplay_Click(object sender, EventArgs e)
    {
      this.cdPicker.Color = this.nfs.Color.BackColor;
      if (this.cdPicker.ShowDialog() != DialogResult.OK)
        return;
      this.nfs.Color.BackColor = this.lblBackColorDisplay.BackColor = this.cdPicker.Color;
    }

    private void cmbOccurances_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.sp.Occurances = (SearchParameters.Occurance) this.cmbOccurances.SelectedValue;
    }

    private void chkBlink_CheckedChanged(object sender, EventArgs e)
    {
      this.nfs.Blink.CanBlink = this.pnlBlinkOptions.Enabled = this.chkBlink.Checked;
    }

    private void nudBlinkRepeat_ValueChanged(object sender, EventArgs e)
    {
      this.nfs.Blink.Repeat = (int) this.nudBlinkRepeat.Value;
    }

    private void nudBlinkSpeed_ValueChanged(object sender, EventArgs e)
    {
      this.nfs.Blink.SpeedInMilliseconds = (int) this.nudBlinkSpeed.Value;
    }

    private void chkClearTrail_CheckedChanged(object sender, EventArgs e)
    {
      this.nfs.ClearTrail.CanClear = this.nudClearTrailAfter.Enabled = this.chkClearTrail.Checked;
    }

    private void nudClearTrailAfter_ValueChanged(object sender, EventArgs e)
    {
      this.nfs.ClearTrail.DelayInMilliseconds = (int) this.nudClearTrailAfter.Value;
    }

    private void registerIsDirty(Control.ControlCollection control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control)
      {
        if (control1.HasChildren)
          this.registerIsDirty(control1.Controls);
        if (control1 is CheckBox)
          (control1 as CheckBox).CheckedChanged += new System.EventHandler(this.setIsDirty);
        if (control1 is ComboBox)
          (control1 as ComboBox).SelectedIndexChanged += new System.EventHandler(this.setIsDirty);
        if (control1 is Label)
          (control1 as Label).BackColorChanged += new System.EventHandler(this.setIsDirty);
        if (control1 is NumericUpDown)
          (control1 as NumericUpDown).ValueChanged += new System.EventHandler(this.setIsDirty);
      }
    }

    private void setIsDirty(object sender, EventArgs e) => this.isDirty = true;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Settings));
      this.pnlMain = new Panel();
      this.gbFromattingOptions = new GroupBox();
      this.nudClearTrailAfter = new NumericUpDown();
      this.chkClearTrail = new CheckBox();
      this.pnlBlinkOptions = new Panel();
      this.nudBlinkSpeed = new NumericUpDown();
      this.lblBlinkSpeed = new Label();
      this.nudBlinkRepeat = new NumericUpDown();
      this.lblBlinkRepeat = new Label();
      this.chkBlink = new CheckBox();
      this.lblBackColorDisplay = new Label();
      this.lblForeColorDisplay = new Label();
      this.lblBackColor = new Label();
      this.lblForeColor = new Label();
      this.gbSearchOptions = new GroupBox();
      this.cmbOccurances = new ComboBox();
      this.lblOccurances = new Label();
      this.chkMatchCase = new CheckBox();
      this.chkMatchWord = new CheckBox();
      this.pnlCommands = new Panel();
      this.cmdRestoreDefault = new Button();
      this.ilIcons = new ImageList(this.components);
      this.cmdClose = new Button();
      this.cdPicker = new ColorDialog();
      this.ttTips = new ToolTip(this.components);
      this.pnlMain.SuspendLayout();
      this.gbFromattingOptions.SuspendLayout();
      this.nudClearTrailAfter.BeginInit();
      this.pnlBlinkOptions.SuspendLayout();
      this.nudBlinkSpeed.BeginInit();
      this.nudBlinkRepeat.BeginInit();
      this.gbSearchOptions.SuspendLayout();
      this.pnlCommands.SuspendLayout();
      this.SuspendLayout();
      this.pnlMain.Controls.Add((Control) this.gbFromattingOptions);
      this.pnlMain.Controls.Add((Control) this.gbSearchOptions);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 31);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(188, 230);
      this.pnlMain.TabIndex = 1;
      this.gbFromattingOptions.Controls.Add((Control) this.nudClearTrailAfter);
      this.gbFromattingOptions.Controls.Add((Control) this.chkClearTrail);
      this.gbFromattingOptions.Controls.Add((Control) this.pnlBlinkOptions);
      this.gbFromattingOptions.Controls.Add((Control) this.chkBlink);
      this.gbFromattingOptions.Controls.Add((Control) this.lblBackColorDisplay);
      this.gbFromattingOptions.Controls.Add((Control) this.lblForeColorDisplay);
      this.gbFromattingOptions.Controls.Add((Control) this.lblBackColor);
      this.gbFromattingOptions.Controls.Add((Control) this.lblForeColor);
      this.gbFromattingOptions.Location = new Point(3, 98);
      this.gbFromattingOptions.Name = "gbFromattingOptions";
      this.gbFromattingOptions.Size = new Size(182, (int) sbyte.MaxValue);
      this.gbFromattingOptions.TabIndex = 2;
      this.gbFromattingOptions.TabStop = false;
      this.gbFromattingOptions.Text = "Formatting Options";
      this.nudClearTrailAfter.Location = new Point(110, 102);
      this.nudClearTrailAfter.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.nudClearTrailAfter.Name = "nudClearTrailAfter";
      this.nudClearTrailAfter.Size = new Size(57, 20);
      this.nudClearTrailAfter.TabIndex = 7;
      this.nudClearTrailAfter.ValueChanged += new System.EventHandler(this.nudClearTrailAfter_ValueChanged);
      this.chkClearTrail.AutoSize = true;
      this.chkClearTrail.Location = new Point(10, 103);
      this.chkClearTrail.Name = "chkClearTrail";
      this.chkClearTrail.Size = new Size(97, 17);
      this.chkClearTrail.TabIndex = 6;
      this.chkClearTrail.Text = "&Clear Trail after";
      this.chkClearTrail.UseVisualStyleBackColor = true;
      this.chkClearTrail.CheckedChanged += new System.EventHandler(this.chkClearTrail_CheckedChanged);
      this.pnlBlinkOptions.BorderStyle = BorderStyle.Fixed3D;
      this.pnlBlinkOptions.Controls.Add((Control) this.nudBlinkSpeed);
      this.pnlBlinkOptions.Controls.Add((Control) this.lblBlinkSpeed);
      this.pnlBlinkOptions.Controls.Add((Control) this.nudBlinkRepeat);
      this.pnlBlinkOptions.Controls.Add((Control) this.lblBlinkRepeat);
      this.pnlBlinkOptions.Location = new Point(58, 47);
      this.pnlBlinkOptions.Name = "pnlBlinkOptions";
      this.pnlBlinkOptions.Size = new Size(115, 50);
      this.pnlBlinkOptions.TabIndex = 5;
      this.nudBlinkSpeed.Location = new Point(51, 23);
      this.nudBlinkSpeed.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.nudBlinkSpeed.Minimum = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.nudBlinkSpeed.Name = "nudBlinkSpeed";
      this.nudBlinkSpeed.Size = new Size(57, 20);
      this.nudBlinkSpeed.TabIndex = 3;
      this.nudBlinkSpeed.Value = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.nudBlinkSpeed.ValueChanged += new System.EventHandler(this.nudBlinkSpeed_ValueChanged);
      this.lblBlinkSpeed.AutoSize = true;
      this.lblBlinkSpeed.Location = new Point(3, 27);
      this.lblBlinkSpeed.Name = "lblBlinkSpeed";
      this.lblBlinkSpeed.Size = new Size(38, 13);
      this.lblBlinkSpeed.TabIndex = 2;
      this.lblBlinkSpeed.Text = "Sp&eed";
      this.nudBlinkRepeat.Location = new Point(51, 1);
      this.nudBlinkRepeat.Maximum = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this.nudBlinkRepeat.Minimum = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.nudBlinkRepeat.Name = "nudBlinkRepeat";
      this.nudBlinkRepeat.Size = new Size(57, 20);
      this.nudBlinkRepeat.TabIndex = 1;
      this.nudBlinkRepeat.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.nudBlinkRepeat.ValueChanged += new System.EventHandler(this.nudBlinkRepeat_ValueChanged);
      this.lblBlinkRepeat.AutoSize = true;
      this.lblBlinkRepeat.Location = new Point(3, 3);
      this.lblBlinkRepeat.Name = "lblBlinkRepeat";
      this.lblBlinkRepeat.Size = new Size(42, 13);
      this.lblBlinkRepeat.TabIndex = 0;
      this.lblBlinkRepeat.Text = "&Repeat";
      this.chkBlink.AutoSize = true;
      this.chkBlink.Location = new Point(10, 63);
      this.chkBlink.Name = "chkBlink";
      this.chkBlink.Size = new Size(49, 17);
      this.chkBlink.TabIndex = 4;
      this.chkBlink.Text = "&Blink";
      this.chkBlink.UseVisualStyleBackColor = true;
      this.chkBlink.CheckedChanged += new System.EventHandler(this.chkBlink_CheckedChanged);
      this.lblBackColorDisplay.BorderStyle = BorderStyle.Fixed3D;
      this.lblBackColorDisplay.Location = new Point(155, 23);
      this.lblBackColorDisplay.Name = "lblBackColorDisplay";
      this.lblBackColorDisplay.Size = new Size(12, 12);
      this.lblBackColorDisplay.TabIndex = 3;
      this.lblBackColorDisplay.Click += new System.EventHandler(this.lblBackColorDisplay_Click);
      this.lblForeColorDisplay.BorderStyle = BorderStyle.Fixed3D;
      this.lblForeColorDisplay.Location = new Point(67, 23);
      this.lblForeColorDisplay.Name = "lblForeColorDisplay";
      this.lblForeColorDisplay.Size = new Size(12, 12);
      this.lblForeColorDisplay.TabIndex = 1;
      this.lblForeColorDisplay.Click += new System.EventHandler(this.lblForeColorDisplay_Click);
      this.lblBackColor.AutoSize = true;
      this.lblBackColor.Location = new Point(90, 22);
      this.lblBackColor.Name = "lblBackColor";
      this.lblBackColor.Size = new Size(59, 13);
      this.lblBackColor.TabIndex = 2;
      this.lblBackColor.Text = "&Back Color";
      this.lblForeColor.AutoSize = true;
      this.lblForeColor.Location = new Point(6, 22);
      this.lblForeColor.Name = "lblForeColor";
      this.lblForeColor.Size = new Size(55, 13);
      this.lblForeColor.TabIndex = 0;
      this.lblForeColor.Text = "&Fore Color";
      this.gbSearchOptions.Controls.Add((Control) this.cmbOccurances);
      this.gbSearchOptions.Controls.Add((Control) this.lblOccurances);
      this.gbSearchOptions.Controls.Add((Control) this.chkMatchCase);
      this.gbSearchOptions.Controls.Add((Control) this.chkMatchWord);
      this.gbSearchOptions.Location = new Point(3, 10);
      this.gbSearchOptions.Name = "gbSearchOptions";
      this.gbSearchOptions.Size = new Size(182, 77);
      this.gbSearchOptions.TabIndex = 1;
      this.gbSearchOptions.TabStop = false;
      this.gbSearchOptions.Text = "Text Options";
      this.cmbOccurances.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOccurances.FormattingEnabled = true;
      this.cmbOccurances.Location = new Point(93, 38);
      this.cmbOccurances.Name = "cmbOccurances";
      this.cmbOccurances.Size = new Size(74, 21);
      this.cmbOccurances.TabIndex = 3;
      this.cmbOccurances.SelectedIndexChanged += new System.EventHandler(this.cmbOccurances_SelectedIndexChanged);
      this.lblOccurances.AutoSize = true;
      this.lblOccurances.Location = new Point(90, 22);
      this.lblOccurances.Name = "lblOccurances";
      this.lblOccurances.Size = new Size(83, 13);
      this.lblOccurances.TabIndex = 2;
      this.lblOccurances.Text = "Find Occurance";
      this.chkMatchCase.Appearance = Appearance.Button;
      this.chkMatchCase.AutoSize = true;
      this.chkMatchCase.FlatAppearance.CheckedBackColor = SystemColors.Highlight;
      this.chkMatchCase.FlatStyle = FlatStyle.System;
      this.chkMatchCase.Location = new Point(9, 48);
      this.chkMatchCase.Name = "chkMatchCase";
      this.chkMatchCase.Size = new Size(30, 23);
      this.chkMatchCase.TabIndex = 1;
      this.chkMatchCase.Text = "Aa";
      this.chkMatchCase.TextAlign = ContentAlignment.MiddleCenter;
      this.ttTips.SetToolTip((Control) this.chkMatchCase, "Match Case");
      this.chkMatchCase.UseVisualStyleBackColor = true;
      this.chkMatchCase.CheckedChanged += new System.EventHandler(this.chkMatchCase_CheckedChanged);
      this.chkMatchWord.Appearance = Appearance.Button;
      this.chkMatchWord.AutoSize = true;
      this.chkMatchWord.FlatAppearance.CheckedBackColor = SystemColors.Highlight;
      this.chkMatchWord.FlatStyle = FlatStyle.System;
      this.chkMatchWord.Location = new Point(9, 22);
      this.chkMatchWord.Name = "chkMatchWord";
      this.chkMatchWord.Size = new Size(30, 23);
      this.chkMatchWord.TabIndex = 0;
      this.chkMatchWord.Text = "Ab";
      this.chkMatchWord.TextAlign = ContentAlignment.MiddleCenter;
      this.ttTips.SetToolTip((Control) this.chkMatchWord, "Match Whole Word");
      this.chkMatchWord.UseVisualStyleBackColor = true;
      this.chkMatchWord.CheckedChanged += new System.EventHandler(this.chkMatchWord_CheckedChanged);
      this.pnlCommands.BackColor = SystemColors.GradientActiveCaption;
      this.pnlCommands.BorderStyle = BorderStyle.FixedSingle;
      this.pnlCommands.Controls.Add((Control) this.cmdRestoreDefault);
      this.pnlCommands.Controls.Add((Control) this.cmdClose);
      this.pnlCommands.Dock = DockStyle.Top;
      this.pnlCommands.Location = new Point(0, 0);
      this.pnlCommands.Name = "pnlCommands";
      this.pnlCommands.Size = new Size(188, 31);
      this.pnlCommands.TabIndex = 0;
      this.cmdRestoreDefault.ImageIndex = 0;
      this.cmdRestoreDefault.ImageList = this.ilIcons;
      this.cmdRestoreDefault.Location = new Point(3, 3);
      this.cmdRestoreDefault.Name = "cmdRestoreDefault";
      this.cmdRestoreDefault.Size = new Size(23, 23);
      this.cmdRestoreDefault.TabIndex = 0;
      this.ttTips.SetToolTip((Control) this.cmdRestoreDefault, "Restore Default Settings");
      this.cmdRestoreDefault.UseVisualStyleBackColor = true;
      this.cmdRestoreDefault.Click += new System.EventHandler(this.cmdRestoreDefault_Click);
      this.ilIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilIcons.ImageStream");
      this.ilIcons.TransparentColor = Color.Transparent;
      this.ilIcons.Images.SetKeyName(0, "reset.png");
      this.ilIcons.Images.SetKeyName(1, "save.png");
      this.cmdClose.DialogResult = DialogResult.Cancel;
      this.cmdClose.ImageIndex = 1;
      this.cmdClose.ImageList = this.ilIcons;
      this.cmdClose.Location = new Point(162, 3);
      this.cmdClose.Name = "cmdClose";
      this.cmdClose.Size = new Size(23, 23);
      this.cmdClose.TabIndex = 1;
      this.ttTips.SetToolTip((Control) this.cmdClose, "Save & Close (Esc)");
      this.cmdClose.UseVisualStyleBackColor = true;
      this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cmdClose;
      this.ClientSize = new Size(188, 261);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.pnlCommands);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Settings);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.Manual;
      this.TopMost = true;
      this.pnlMain.ResumeLayout(false);
      this.gbFromattingOptions.ResumeLayout(false);
      this.gbFromattingOptions.PerformLayout();
      this.nudClearTrailAfter.EndInit();
      this.pnlBlinkOptions.ResumeLayout(false);
      this.pnlBlinkOptions.PerformLayout();
      this.nudBlinkSpeed.EndInit();
      this.nudBlinkRepeat.EndInit();
      this.gbSearchOptions.ResumeLayout(false);
      this.gbSearchOptions.PerformLayout();
      this.pnlCommands.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
