// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.SourceModeSelector
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class SourceModeSelector : UserControl
  {
    private static readonly Dictionary<int, MainForm.SourceMode> s_sourceModeMap = new Dictionary<int, MainForm.SourceMode>()
    {
      {
        -1,
        MainForm.SourceMode.None
      },
      {
        0,
        MainForm.SourceMode.Convert
      },
      {
        1,
        MainForm.SourceMode.Transfer
      }
    };
    private int _modeIndex = -1;
    private IContainer components;
    private TableLayoutPanel OptionsLayout;
    private TableLayoutPanel Option2Container;
    private Label Option2Label;
    private PictureBox Option2Icon;
    private TableLayoutPanel Option1Container;
    private Label Option1Label;
    private PictureBox Option1Icon;

    public MainForm.SourceMode SelectedMode => SourceModeSelector.s_sourceModeMap[this._modeIndex];

    public event EventHandler<MainForm.SourceMode> OnSelectionChanged;

    public event EventHandler<MainForm.SourceMode> OnSelectionDoubleClick;

    public SourceModeSelector()
    {
      this.InitializeComponent();
      this.RegisterClickHandlers();
    }

    public void SelectNextOption(bool previous = false)
    {
      int num1 = SourceModeSelector.s_sourceModeMap.Count - 1;
      int num2 = previous ? -1 : 1;
      if (previous && this._modeIndex < 0)
        num2 = 0;
      this.SetOption((this._modeIndex + num2 + num1) % num1);
    }

    private bool IsSelectedMode(int modeIndex) => this._modeIndex == modeIndex;

    private void RegisterClickHandlers()
    {
      foreach (TableLayoutPanel tableLayoutPanel in this.OptionsLayout.Controls.OfType<TableLayoutPanel>())
      {
        foreach (Control clickable in tableLayoutPanel.Controls.OfType<Control>())
          this.SetClickHandlers(clickable);
      }
    }

    private void SetClickHandlers(Control clickable)
    {
      clickable.MouseClick += new MouseEventHandler(this.OnSelect);
      clickable.MouseDoubleClick += new MouseEventHandler(this.OnDoubleClick);
    }

    private void OnSelect(object sender, MouseEventArgs e)
    {
      this.SetOption(this.GetOptionIndex(sender));
    }

    private void OnDoubleClick(object sender, MouseEventArgs e)
    {
      this.HandleDoubleClick(this.GetOptionIndex(sender));
    }

    private int GetOptionIndex(object sender)
    {
      TableLayoutPanel parent = sender is Control control ? (TableLayoutPanel) control.Parent : (TableLayoutPanel) null;
      return ((TableLayoutPanel) parent.Parent).GetColumn((Control) parent) - 1;
    }

    private void SetOption(int modeIndex)
    {
      int num = !this.IsSelectedMode(modeIndex) ? 1 : 0;
      this._modeIndex = modeIndex;
      this.Refresh();
      if (num == 0)
        return;
      EventHandler<MainForm.SourceMode> selectionChanged = this.OnSelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, this.SelectedMode);
    }

    private void HandleDoubleClick(int modeIndex)
    {
      if (!this.IsSelectedMode(modeIndex))
      {
        this.SetOption(modeIndex);
      }
      else
      {
        EventHandler<MainForm.SourceMode> selectionDoubleClick = this.OnSelectionDoubleClick;
        if (selectionDoubleClick == null)
          return;
        selectionDoubleClick((object) this, this.SelectedMode);
      }
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
      TableLayoutPanel tableLayoutPanel = (TableLayoutPanel) sender;
      Pen pen = new Pen(this.IsSelectedMode(((TableLayoutPanel) tableLayoutPanel.Parent).GetColumn((Control) tableLayoutPanel) - 1) ? Color.DarkGray : Color.Transparent)
      {
        Width = 2f
      };
      e.Graphics.DrawRectangle(pen, new Rectangle(1, 1, tableLayoutPanel.Bounds.Width - 2, tableLayoutPanel.Bounds.Height - 2));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SourceModeSelector));
      this.OptionsLayout = new TableLayoutPanel();
      this.Option1Container = new TableLayoutPanel();
      this.Option1Label = new Label();
      this.Option1Icon = new PictureBox();
      this.Option2Container = new TableLayoutPanel();
      this.Option2Label = new Label();
      this.Option2Icon = new PictureBox();
      this.OptionsLayout.SuspendLayout();
      this.Option1Container.SuspendLayout();
      ((ISupportInitialize) this.Option1Icon).BeginInit();
      this.Option2Container.SuspendLayout();
      ((ISupportInitialize) this.Option2Icon).BeginInit();
      this.SuspendLayout();
      this.OptionsLayout.ColumnCount = 4;
      this.OptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
      this.OptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
      this.OptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
      this.OptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
      this.OptionsLayout.Controls.Add((Control) this.Option1Container, 2, 0);
      this.OptionsLayout.Controls.Add((Control) this.Option2Container, 1, 0);
      this.OptionsLayout.Dock = DockStyle.Fill;
      this.OptionsLayout.Location = new Point(0, 0);
      this.OptionsLayout.Margin = new Padding(0);
      this.OptionsLayout.Name = "OptionsLayout";
      this.OptionsLayout.RowCount = 1;
      this.OptionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.OptionsLayout.Size = new Size(720, 240);
      this.OptionsLayout.TabIndex = 0;
      this.Option1Container.ColumnCount = 1;
      this.Option1Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.Option1Container.Controls.Add((Control) this.Option1Label, 0, 1);
      this.Option1Container.Controls.Add((Control) this.Option1Icon, 0, 0);
      this.Option1Container.Dock = DockStyle.Fill;
      this.Option1Container.Location = new Point(362, 2);
      this.Option1Container.Margin = new Padding(2);
      this.Option1Container.Name = "Option1Container";
      this.Option1Container.Padding = new Padding(2, 2, 2, 12);
      this.Option1Container.RowCount = 2;
      this.Option1Container.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.Option1Container.RowStyles.Add(new RowStyle(SizeType.Absolute, 72f));
      this.Option1Container.Size = new Size(176, 236);
      this.Option1Container.TabIndex = 0;
      this.Option1Container.Paint += new PaintEventHandler(this.OnPaint);
      this.Option1Label.AutoSize = true;
      this.Option1Label.Dock = DockStyle.Fill;
      this.Option1Label.Location = new Point(4, 154);
      this.Option1Label.Margin = new Padding(2);
      this.Option1Label.Name = "Option1Label";
      this.Option1Label.Size = new Size(168, 68);
      this.Option1Label.TabIndex = 0;
      this.Option1Label.Text = "Migrate Enhanced Conditions between instances";
      this.Option1Label.TextAlign = ContentAlignment.TopCenter;
      this.Option1Icon.Anchor = AnchorStyles.None;
      this.Option1Icon.Image = (Image) componentResourceManager.GetObject("Option1Icon.Image");
      this.Option1Icon.InitialImage = (Image) null;
      this.Option1Icon.Location = new Point(24, 25);
      this.Option1Icon.Margin = new Padding(2);
      this.Option1Icon.Name = "Option1Icon";
      this.Option1Icon.Size = new Size(128, 104);
      this.Option1Icon.SizeMode = PictureBoxSizeMode.Zoom;
      this.Option1Icon.TabIndex = 1;
      this.Option1Icon.TabStop = false;
      this.Option2Container.ColumnCount = 1;
      this.Option2Container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.Option2Container.Controls.Add((Control) this.Option2Label, 0, 1);
      this.Option2Container.Controls.Add((Control) this.Option2Icon, 0, 0);
      this.Option2Container.Dock = DockStyle.Fill;
      this.Option2Container.Location = new Point(182, 2);
      this.Option2Container.Margin = new Padding(2);
      this.Option2Container.Name = "Option2Container";
      this.Option2Container.Padding = new Padding(2, 2, 2, 12);
      this.Option2Container.RowCount = 2;
      this.Option2Container.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.Option2Container.RowStyles.Add(new RowStyle(SizeType.Absolute, 72f));
      this.Option2Container.Size = new Size(176, 236);
      this.Option2Container.TabIndex = 1;
      this.Option2Container.Paint += new PaintEventHandler(this.OnPaint);
      this.Option2Label.AutoSize = true;
      this.Option2Label.Dock = DockStyle.Fill;
      this.Option2Label.Location = new Point(4, 154);
      this.Option2Label.Margin = new Padding(2);
      this.Option2Label.Name = "Option2Label";
      this.Option2Label.Size = new Size(168, 68);
      this.Option2Label.TabIndex = 0;
      this.Option2Label.Text = "Convert Standard to Enhanced Conditions";
      this.Option2Label.TextAlign = ContentAlignment.TopCenter;
      this.Option2Icon.Anchor = AnchorStyles.None;
      this.Option2Icon.Image = (Image) componentResourceManager.GetObject("Option2Icon.Image");
      this.Option2Icon.InitialImage = (Image) null;
      this.Option2Icon.Location = new Point(24, 25);
      this.Option2Icon.Margin = new Padding(2);
      this.Option2Icon.Name = "Option2Icon";
      this.Option2Icon.Size = new Size(128, 104);
      this.Option2Icon.SizeMode = PictureBoxSizeMode.Zoom;
      this.Option2Icon.TabIndex = 1;
      this.Option2Icon.TabStop = false;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.OptionsLayout);
      this.Margin = new Padding(0);
      this.Name = nameof (SourceModeSelector);
      this.Size = new Size(720, 240);
      this.OptionsLayout.ResumeLayout(false);
      this.Option1Container.ResumeLayout(false);
      this.Option1Container.PerformLayout();
      ((ISupportInitialize) this.Option1Icon).EndInit();
      this.Option2Container.ResumeLayout(false);
      this.Option2Container.PerformLayout();
      ((ISupportInitialize) this.Option2Icon).EndInit();
      this.ResumeLayout(false);
    }
  }
}
