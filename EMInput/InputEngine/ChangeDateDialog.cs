// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ChangeDateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ChangeDateDialog : Form
  {
    private Label label1;
    private Label label2;
    private MonthCalendar monthCalendar;
    private System.ComponentModel.Container components;
    private DialogButtons dlgButtons;
    private DateTime selected;

    public DateTime Selected => this.selected;

    public ChangeDateDialog(DateTime selectedDate)
    {
      this.InitializeComponent();
      this.monthCalendar.SelectionStart = selectedDate;
      this.selected = selectedDate;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.monthCalendar = new MonthCalendar();
      this.label1 = new Label();
      this.label2 = new Label();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.monthCalendar.Location = new Point(54, 36);
      this.monthCalendar.MaxSelectionCount = 1;
      this.monthCalendar.Name = "monthCalendar";
      this.monthCalendar.TabIndex = 0;
      this.monthCalendar.DateChanged += new DateRangeEventHandler(this.monthCalendar_DateChanged);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(91, 22);
      this.label1.TabIndex = 1;
      this.label1.Text = "Date selected:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(96, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(179, 22);
      this.label2.TabIndex = 2;
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.dlgButtons.ButtonAlignment = HorizontalAlignment.Center;
      this.dlgButtons.DialogResult = DialogResult.OK;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 207);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(278, 44);
      this.dlgButtons.TabIndex = 3;
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(278, 251);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.monthCalendar);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangeDateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select a New Date";
      this.KeyPress += new KeyPressEventHandler(this.ChangeDateDialog_KeyPress);
      this.ResumeLayout(false);
    }

    private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
    {
      this.selected = this.monthCalendar.SelectionStart;
      this.label2.Text = this.selected.ToLongDateString();
    }

    private void ChangeDateDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}
