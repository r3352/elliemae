// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.RateLockDropdownBox
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
  public class RateLockDropdownBox : UserControl
  {
    private bool includeRequestData;
    private IContainer components;
    private ComboBoxEx cboLock;
    private ComboBox cboAutoHeight;
    private DropdownButton dbtnDrop;
    private BorderPanel pnlDisplay;

    public event EventHandler SelectedIndexChanged;

    public RateLockDropdownBox(bool includeRequestData)
    {
      this.InitializeComponent();
      this.includeRequestData = includeRequestData;
      this.cboLock.Items.Add((object) "");
      if (includeRequestData)
      {
        this.addItem("PendingRequest", "All Pending Lock Requests", (Image) Resources.rate_lock_request);
        this.addItem("NotLocked-NoRequest", "Unlocked", (Image) Resources.rate_unlocked);
        this.addItem("Cancelled", "Lock Cancelled", (Image) Resources.rate_lock_cancelled);
        this.addItem("NotLocked-Request", "Lock Requested", (Image) Resources.rate_unlocked_request);
        this.addItem("Locked-NoRequest", "Locked", (Image) Resources.rate_locked);
        this.addItem("Locked-Request", "Locked, New Lock Requested", (Image) Resources.rate_locked_request);
        this.addItem("Locked-Extension-Request", "Locked, Extension Requested", (Image) Resources.rate_locked_extension);
        this.addItem("Expired-NoRequest", "Expired", (Image) Resources.rate_expired);
        this.addItem("Expired-Request", "Expired, New Lock Requested", (Image) Resources.rate_expired_request);
        this.addItem("Expired-Extension-Request", "Expired, Extension Requested", (Image) Resources.rate_expired__extension_request);
        this.addItem("Locked-Cancellation-Request", "Locked, Cancellation Requested", (Image) Resources.rate_lock_cancel_request);
        this.addItem("Voided", "Voided", (Image) Resources.rate_unlocked);
      }
      else
      {
        this.addItem("NotLocked", "Unlocked", (Image) Resources.rate_unlocked);
        this.addItem("Locked", "Locked", (Image) Resources.rate_locked);
        this.addItem("Expired", "Expired", (Image) Resources.rate_expired);
        this.addItem("Cancelled", "Lock Cancelled", (Image) Resources.rate_lock_cancelled);
        this.addItem("Voided", "Voided", (Image) Resources.rate_unlocked);
      }
      this.cboLock.Left = this.cboLock.Top = 0;
      this.cboLock.Width = 200;
    }

    public bool IncludesRequestStatus => this.includeRequestData;

    public string[] GetSelectedLockStatuses()
    {
      if (!(this.cboLock.SelectedItem is RateLockDropdownBox.LockListItem selectedItem))
        return new string[0];
      return string.Concat(selectedItem.Key) == "PendingRequest" ? new string[6]
      {
        "NotLocked-Request",
        "Locked-Request",
        "Expired-Request",
        "Expired-Extension-Request",
        "Locked-Extension-Request",
        "Locked-Cancellation-Request"
      } : new string[1]{ selectedItem.Key.ToString() };
    }

    public void Clear() => this.cboLock.SelectedIndex = 0;

    private void cboLock_SelectedIndexChanged(object sender, EventArgs e)
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
      return new Size(proposedSize.Width, this.cboLock.Height);
    }

    protected override void OnResize(EventArgs e)
    {
      if (this.Height != this.cboAutoHeight.Height)
        this.Height = this.cboAutoHeight.Height;
      this.cboLock.ItemHeight = this.Height - 6;
      base.OnResize(e);
    }

    private void RateLockDropdownBox_Load(object sender, EventArgs e)
    {
      this.Height = this.cboAutoHeight.Height;
      this.cboAutoHeight.Visible = false;
    }

    private void addItem(string key, string text, Image image)
    {
      this.cboLock.Items.Add((object) new RateLockDropdownBox.LockListItem(key, text, image));
    }

    private void dbtnDrop_Click(object sender, EventArgs e) => this.cboLock.DroppedDown = true;

    private void pnlDisplay_Paint(object sender, PaintEventArgs e)
    {
      ItemDrawArgs args = new ItemDrawArgs(e.Graphics, this.Font, this.ForeColor, this.pnlDisplay.BackColor, ControlDraw.GetRectangleInterior(this.pnlDisplay.DisplayRectangle, new Padding(3, 2, 0, 2)), ControlDraw.CreateDefaultStringFormat(ContentAlignment.MiddleLeft));
      if (!(this.cboLock.SelectedItem is RateLockDropdownBox.LockListItem selectedItem))
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
      this.cboLock = new ComboBoxEx();
      this.cboAutoHeight = new ComboBox();
      this.dbtnDrop = new DropdownButton();
      this.pnlDisplay = new BorderPanel();
      this.pnlDisplay.SuspendLayout();
      this.SuspendLayout();
      this.cboLock.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLock.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboLock.FormattingEnabled = true;
      this.cboLock.ItemHeight = 16;
      this.cboLock.Location = new Point(14, 2);
      this.cboLock.Name = "cboLock";
      this.cboLock.SelectedBGColor = SystemColors.Highlight;
      this.cboLock.Size = new Size(59, 22);
      this.cboLock.TabIndex = 1;
      this.cboLock.SelectedIndexChanged += new EventHandler(this.cboLock_SelectedIndexChanged);
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
      this.Controls.Add((Control) this.cboLock);
      this.Controls.Add((Control) this.cboAutoHeight);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (RateLockDropdownBox);
      this.Size = new Size(59, 32);
      this.Load += new EventHandler(this.RateLockDropdownBox_Load);
      this.pnlDisplay.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private class LockListItem : ObjectWithImage
    {
      private string key;

      public LockListItem(string key, string text, Image image)
        : base(text, image)
      {
        this.key = key;
      }

      public object Key => (object) this.key;

      public override bool Equals(object obj)
      {
        return obj is RateLockDropdownBox.LockListItem lockListItem && lockListItem.key == this.key;
      }

      public override int GetHashCode() => this.key.GetHashCode();
    }
  }
}
