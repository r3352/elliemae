// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LockValidationStatusDropdownBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LockValidationStatusDropdownBox : UserControl
  {
    private IContainer components;
    private ComboBoxEx cboLockValidationStatus;
    private ComboBox cboAutoHeight;
    private DropdownButton dbtnDrop;
    private BorderPanel pnlDisplay;

    public event EventHandler SelectedIndexChanged;

    public LockValidationStatusDropdownBox()
    {
      this.InitializeComponent();
      this.cboLockValidationStatus.Items.Add((object) "");
      this.addItem("Needs Validation", "Needs Validation", (Image) Resources.status_alert);
      this.addItem("Price Changed, still qualifies", "Price Changed, still qualifies", (Image) Resources.status_warning);
      this.addItem("Loan no longer qualifies", "Loan no longer qualifies", (Image) Resources.status_alert);
      this.addItem("No Price change, still qualifies", "No Price change, still qualifies", (Image) Resources.status_info);
      this.cboLockValidationStatus.Left = this.cboLockValidationStatus.Top = 0;
      this.cboLockValidationStatus.Width = 200;
    }

    public string[] GetSelectedValidationStatuses()
    {
      if (!(this.cboLockValidationStatus.SelectedItem is LockValidationStatusDropdownBox.ValidationStatusListItem selectedItem))
        return new string[0];
      return new string[1]{ selectedItem.Key.ToString() };
    }

    public void Clear() => this.cboLockValidationStatus.SelectedIndex = 0;

    private void cboValidationStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.pnlDisplay.Refresh();
      this.OnSelectedIndexChanged(e);
    }

    protected void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, e);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
      return new Size(proposedSize.Width, this.cboLockValidationStatus.Height);
    }

    protected override void OnResize(EventArgs e)
    {
      if (this.Height != this.cboAutoHeight.Height)
        this.Height = this.cboAutoHeight.Height;
      this.cboLockValidationStatus.ItemHeight = this.Height - 6;
      base.OnResize(e);
    }

    private void LockValidationStatusDropdownBox_Load(object sender, EventArgs e)
    {
      this.Height = this.cboAutoHeight.Height;
      this.cboAutoHeight.Visible = false;
    }

    private void addItem(string key, string text, Image image)
    {
      this.cboLockValidationStatus.Items.Add((object) new LockValidationStatusDropdownBox.ValidationStatusListItem(key, text, image));
    }

    private void dbtnDrop_Click(object sender, EventArgs e)
    {
      this.cboLockValidationStatus.DroppedDown = true;
    }

    private void pnlDisplay_Paint(object sender, PaintEventArgs e)
    {
      ItemDrawArgs args = new ItemDrawArgs(e.Graphics, this.Font, this.ForeColor, this.pnlDisplay.BackColor, ControlDraw.GetRectangleInterior(this.pnlDisplay.DisplayRectangle, new Padding(3, 2, 0, 2)), ControlDraw.CreateDefaultStringFormat(ContentAlignment.MiddleLeft));
      if (!(this.cboLockValidationStatus.SelectedItem is LockValidationStatusDropdownBox.ValidationStatusListItem selectedItem))
        return;
      ControlDraw.DrawImage(selectedItem.Image, args);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboLockValidationStatus = new ComboBoxEx();
      this.cboAutoHeight = new ComboBox();
      this.dbtnDrop = new DropdownButton();
      this.pnlDisplay = new BorderPanel();
      this.pnlDisplay.SuspendLayout();
      this.SuspendLayout();
      this.cboLockValidationStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLockValidationStatus.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboLockValidationStatus.FormattingEnabled = true;
      this.cboLockValidationStatus.Location = new Point(14, 2);
      this.cboLockValidationStatus.Name = "cboValidationStatus";
      this.cboLockValidationStatus.SelectedBGColor = SystemColors.Highlight;
      this.cboLockValidationStatus.Size = new Size(59, 22);
      this.cboLockValidationStatus.TabIndex = 1;
      this.cboLockValidationStatus.SelectedIndexChanged += new EventHandler(this.cboValidationStatus_SelectedIndexChanged);
      this.cboAutoHeight.FormattingEnabled = true;
      this.cboAutoHeight.Location = new Point(36, 22);
      this.cboAutoHeight.Name = "cboAutoHeight";
      this.cboAutoHeight.Size = new Size(10, 22);
      this.cboAutoHeight.TabIndex = 2;
      this.dbtnDrop.Dock = DockStyle.Right;
      this.dbtnDrop.Location = new Point(41, 1);
      this.dbtnDrop.Name = "dbtnDrop";
      this.dbtnDrop.Size = new Size(17, 30);
      this.dbtnDrop.TabIndex = 3;
      this.dbtnDrop.TabStop = false;
      this.dbtnDrop.Text = "dropdownButton1";
      this.dbtnDrop.Click += new EventHandler(this.dbtnDrop_Click);
      this.pnlDisplay.BackColor = SystemColors.Window;
      this.pnlDisplay.Controls.Add((Control) this.dbtnDrop);
      this.pnlDisplay.Dock = DockStyle.Fill;
      this.pnlDisplay.Location = new Point(0, 0);
      this.pnlDisplay.Name = "pnlDisplay";
      this.pnlDisplay.Size = new Size(59, 32);
      this.pnlDisplay.TabIndex = 4;
      this.pnlDisplay.Paint += new PaintEventHandler(this.pnlDisplay_Paint);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlDisplay);
      this.Controls.Add((Control) this.cboLockValidationStatus);
      this.Controls.Add((Control) this.cboAutoHeight);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LockValidationStatusDropdownBox);
      this.Size = new Size(59, 32);
      this.Load += new EventHandler(this.LockValidationStatusDropdownBox_Load);
      this.pnlDisplay.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private class ValidationStatusListItem : ObjectWithImage
    {
      private string key;

      public ValidationStatusListItem(string key, string text, Image image)
        : base(text, image)
      {
        this.key = key;
      }

      public object Key => (object) this.key;

      public override bool Equals(object obj)
      {
        return obj is LockValidationStatusDropdownBox.ValidationStatusListItem validationStatusListItem && validationStatusListItem.key == this.key;
      }

      public override int GetHashCode() => this.key.GetHashCode();
    }
  }
}
