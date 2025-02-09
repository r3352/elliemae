// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TipControl
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class TipControl : Control
  {
    private const int cornerRadius = 10;
    private const int pointerHeight = 29;
    private const int pointerWidth = 16;
    private const int pointerOffset = 15;
    private const int controlOffset = 4;
    private const int gutterWidth = 4;
    private const int borderWidth = 2;
    private static readonly Color borderColor = Color.FromArgb(102, 153, 204);
    private static readonly Color interiorColor = Color.FromArgb(234, 247, (int) byte.MaxValue);
    private TipPointerPosition pointerPos;
    private Rectangle regionToPoint = Rectangle.Empty;
    private Rectangle bodyRect = Rectangle.Empty;
    private Rectangle pointerRect = Rectangle.Empty;
    private Label lblHeader;
    private PictureBox pictureBox1;
    private PictureBox pictureBox2;
    private Label lblText;
    private PictureBox pictureBox3;
    private Panel pnlContent;
    private Image rolloverCloseImage;
    private PictureBox pbClose;
    private PictureBox pbCloseOver;
    private System.Windows.Forms.LinkLabel lnkMore;
    private Image normalCloseImage;
    private CheckBox chkHide;
    private TipParameters parameters;
    private Hashtable visitedTips = new Hashtable();
    private object referenceObject;

    public event TipContinueEventHandler Continue;

    [DllImport("user32.dll")]
    private static extern bool GetMenuItemRect(
      IntPtr hWnd,
      IntPtr hMenu,
      uint uItem,
      out TipControl.RECT lprcItem);

    public TipControl()
    {
      this.InitializeComponent();
      this.Visible = false;
      this.normalCloseImage = this.pbClose.Image;
      this.rolloverCloseImage = this.pbCloseOver.Image;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (TipControl));
      this.lblHeader = new Label();
      this.pictureBox1 = new PictureBox();
      this.pictureBox2 = new PictureBox();
      this.pnlContent = new Panel();
      this.pbClose = new PictureBox();
      this.pictureBox3 = new PictureBox();
      this.chkHide = new CheckBox();
      this.lnkMore = new System.Windows.Forms.LinkLabel();
      this.lblText = new Label();
      this.pbCloseOver = new PictureBox();
      this.pnlContent.SuspendLayout();
      this.SuspendLayout();
      this.lblHeader.BackColor = Color.Transparent;
      this.lblHeader.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblHeader.ForeColor = Color.FromArgb(228, 133, 12);
      this.lblHeader.Location = new Point(4, 22);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(232, 20);
      this.lblHeader.TabIndex = 1;
      this.lblHeader.Text = "(This is the header)";
      this.lblHeader.TextAlign = ContentAlignment.MiddleLeft;
      this.pictureBox1.Dock = DockStyle.Top;
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(240, 22);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.pictureBox2.Image = (Image) resourceManager.GetObject("pictureBox2.Image");
      this.pictureBox2.Location = new Point(4, 77);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(13, 13);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 3;
      this.pictureBox2.TabStop = false;
      this.pnlContent.BackColor = Color.FromArgb(234, 247, (int) byte.MaxValue);
      this.pnlContent.Controls.Add((Control) this.pbClose);
      this.pnlContent.Controls.Add((Control) this.pictureBox3);
      this.pnlContent.Controls.Add((Control) this.chkHide);
      this.pnlContent.Controls.Add((Control) this.lnkMore);
      this.pnlContent.Controls.Add((Control) this.lblText);
      this.pnlContent.Controls.Add((Control) this.lblHeader);
      this.pnlContent.Controls.Add((Control) this.pictureBox1);
      this.pnlContent.Controls.Add((Control) this.pictureBox2);
      this.pnlContent.Controls.Add((Control) this.pbCloseOver);
      this.pnlContent.Location = new Point(60, 34);
      this.pnlContent.Name = "pnlContent";
      this.pnlContent.Size = new Size(240, 119);
      this.pnlContent.TabIndex = 5;
      this.pbClose.Image = (Image) resourceManager.GetObject("pbClose.Image");
      this.pbClose.Location = new Point(169, 100);
      this.pbClose.Name = "pbClose";
      this.pbClose.Size = new Size(69, 18);
      this.pbClose.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pbClose.TabIndex = 9;
      this.pbClose.TabStop = false;
      this.pbClose.Click += new EventHandler(this.pbClose_Click);
      this.pbClose.MouseEnter += new EventHandler(this.pbClose_MouseEnter);
      this.pbClose.MouseLeave += new EventHandler(this.pbClose_MouseLeave);
      this.pictureBox3.Image = (Image) resourceManager.GetObject("pictureBox3.Image");
      this.pictureBox3.Location = new Point(0, 95);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new Size(240, 1);
      this.pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox3.TabIndex = 8;
      this.pictureBox3.TabStop = false;
      this.chkHide.Location = new Point(4, 100);
      this.chkHide.Name = "chkHide";
      this.chkHide.Size = new Size(158, 18);
      this.chkHide.TabIndex = 7;
      this.chkHide.Text = "Don't show me again";
      this.lnkMore.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lnkMore.Location = new Point(21, 77);
      this.lnkMore.Name = "lnkMore";
      this.lnkMore.Size = new Size(100, 14);
      this.lnkMore.TabIndex = 5;
      this.lnkMore.TabStop = true;
      this.lnkMore.Text = "More Info";
      this.lnkMore.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkMore_LinkClicked);
      this.lblText.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblText.Location = new Point(4, 42);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(232, 32);
      this.lblText.TabIndex = 4;
      this.lblText.Text = "This is the text of the tip.";
      this.pbCloseOver.Image = (Image) resourceManager.GetObject("pbCloseOver.Image");
      this.pbCloseOver.Location = new Point(169, 100);
      this.pbCloseOver.Name = "pbCloseOver";
      this.pbCloseOver.Size = new Size(69, 18);
      this.pbCloseOver.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pbCloseOver.TabIndex = 10;
      this.pbCloseOver.TabStop = false;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.pnlContent);
      this.Name = nameof (TipControl);
      this.Size = new Size(358, 188);
      this.ParentChanged += new EventHandler(this.TipControl_ParentChanged);
      this.pnlContent.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public bool ShowTip(Control controlToReference, TipParameters parms)
    {
      this.referenceObject = (object) controlToReference;
      return this.ShowTip(this.getRectangleForControl(controlToReference), parms);
    }

    public bool ShowTip(MenuItem menuItem, TipParameters parms)
    {
      TipControl.RECT lprcItem;
      if (!TipControl.GetMenuItemRect(menuItem.GetMainMenu().GetForm().Handle, menuItem.Parent.Handle, (uint) menuItem.Index, out lprcItem))
        return false;
      this.referenceObject = (object) menuItem;
      Rectangle regionToPoint = this.Parent.RectangleToClient(new Rectangle(lprcItem.Left, lprcItem.Top, lprcItem.Right - lprcItem.Left, lprcItem.Bottom - lprcItem.Top));
      regionToPoint = new Rectangle(regionToPoint.X, 0, regionToPoint.Width, 0);
      return this.ShowTip(regionToPoint, parms);
    }

    public bool ShowTip(Point placeToPoint, TipParameters parms)
    {
      return this.ShowTip(new Rectangle(placeToPoint, new Size(0, 0)), parms);
    }

    public bool ShowTip(Rectangle regionToPoint, TipParameters parms)
    {
      return this.ShowTip(regionToPoint, parms, this.getOptimalPointerPosition(regionToPoint));
    }

    public bool ShowTip(
      Control controlToReference,
      TipParameters parms,
      TipPointerPosition pointerPosition)
    {
      this.referenceObject = (object) controlToReference;
      return this.ShowTip(this.getRectangleForControl(controlToReference), parms, pointerPosition);
    }

    public bool ShowTip(
      Point placeToPoint,
      TipParameters parms,
      TipPointerPosition pointerPosition)
    {
      return this.ShowTip(new Rectangle(placeToPoint, new Size(0, 0)), parms, pointerPosition);
    }

    public bool ShowTip(
      Rectangle regionToPoint,
      TipParameters parms,
      TipPointerPosition pointerPosition)
    {
      if (this.Visible || (Session.GetPrivateProfileString("TIPS", parms.TipID) ?? "") == "0" || this.visitedTips.Contains((object) parms.TipID))
        return false;
      this.visitedTips[(object) parms.TipID] = (object) null;
      this.regionToPoint = regionToPoint;
      this.pointerPos = pointerPosition;
      this.parameters = parms;
      this.lblText.Text = parms.Text;
      this.lblHeader.Text = parms.Header;
      this.chkHide.Checked = false;
      this.positionChildElements();
      this.positionTipWindow();
      this.BringToFront();
      this.Visible = true;
      this.Focus();
      return true;
    }

    public void Close()
    {
      this.setTipStatus();
      this.Hide();
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ExStyle |= 32;
        return createParams;
      }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      try
      {
        Application.DoEvents();
        Brush brush1 = (Brush) new SolidBrush(TipControl.borderColor);
        Pen pen = new Pen(brush1, 2f);
        Brush brush2 = (Brush) new SolidBrush(TipControl.interiorColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.FillRectangle(brush2, this.bodyRect.Left + 10, this.bodyRect.Top, this.bodyRect.Width - 20, 10);
        e.Graphics.FillRectangle(brush2, this.bodyRect.Left + 10, this.bodyRect.Bottom - 10, this.bodyRect.Width - 20, 10);
        e.Graphics.FillRectangle(brush2, this.bodyRect.Left, this.bodyRect.Top + 10, 10, this.bodyRect.Height - 20);
        e.Graphics.FillRectangle(brush2, this.bodyRect.Right - 10, this.bodyRect.Top + 10, 10, this.bodyRect.Height - 20);
        Rectangle rect = new Rectangle(this.bodyRect.Left + 10, this.bodyRect.Top + 10, this.bodyRect.Width - 20, this.bodyRect.Height - 20);
        e.Graphics.FillRectangle(brush2, rect);
        e.Graphics.FillPie(brush2, this.bodyRect.Left, this.bodyRect.Top, 20, 20, 180, 90);
        e.Graphics.DrawArc(pen, this.bodyRect.Left, this.bodyRect.Top, 20, 20, 180, 90);
        e.Graphics.FillPie(brush2, this.bodyRect.Right - 20 - 1, this.bodyRect.Top, 20, 20, 270, 90);
        e.Graphics.DrawArc(pen, this.bodyRect.Right - 20 - 1, this.bodyRect.Top, 20, 20, 270, 90);
        e.Graphics.FillPie(brush2, this.bodyRect.Right - 20 - 1, this.bodyRect.Bottom - 20 - 1, 20, 20, 0, 90);
        e.Graphics.DrawArc(pen, this.bodyRect.Right - 20 - 1, this.bodyRect.Bottom - 20 - 1, 20, 20, 0, 90);
        e.Graphics.FillPie(brush2, this.bodyRect.Left, this.bodyRect.Bottom - 20 - 1, 20, 20, 90, 90);
        e.Graphics.DrawArc(pen, this.bodyRect.Left, this.bodyRect.Bottom - 20 - 1, 20, 20, 90, 90);
        e.Graphics.DrawLine(pen, this.bodyRect.Left + 10 + 2 - 1, this.bodyRect.Top + 2 - 1, this.bodyRect.Right - 10, this.bodyRect.Top + 2 - 1);
        e.Graphics.DrawLine(pen, this.bodyRect.Left + 10, this.bodyRect.Bottom - 1, this.bodyRect.Right - 10, this.bodyRect.Bottom - 1);
        e.Graphics.DrawLine(pen, this.bodyRect.Left + 2 - 1, this.bodyRect.Top + 10, this.bodyRect.Left + 2 - 1, this.bodyRect.Bottom - 10);
        e.Graphics.DrawLine(pen, this.bodyRect.Right - 1, this.bodyRect.Top + 10, this.bodyRect.Right - 1, this.bodyRect.Bottom - 10);
        Bitmap bitmap = new Bitmap(16, 31);
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        {
          Point[] points = new Point[3]
          {
            new Point(1, 1),
            new Point(8, 29),
            new Point(15, 1)
          };
          graphics.FillPolygon(brush2, points);
          graphics.DrawLine(pen, points[0], points[1]);
          graphics.DrawLine(pen, points[1], points[2]);
        }
        switch (this.pointerPos)
        {
          case TipPointerPosition.LeftTop:
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            e.Graphics.DrawImage((Image) bitmap, pen.Width - 1f, 15f);
            break;
          case TipPointerPosition.LeftBottom:
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            e.Graphics.DrawImage((Image) bitmap, pen.Width - 1f, (float) (this.Height - bitmap.Height - 15));
            break;
          case TipPointerPosition.TopLeft:
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            e.Graphics.DrawImage((Image) bitmap, 15f, pen.Width);
            break;
          case TipPointerPosition.TopRight:
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            e.Graphics.DrawImage((Image) bitmap, (float) (this.Width - bitmap.Width - 15), pen.Width - 1f);
            break;
          case TipPointerPosition.RightTop:
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
            e.Graphics.DrawImage((Image) bitmap, (float) ((double) (this.Width - bitmap.Width) - (double) pen.Width + 1.0), 15f);
            break;
          case TipPointerPosition.RightBottom:
            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            e.Graphics.DrawImage((Image) bitmap, (float) ((double) (this.Width - bitmap.Width) - (double) pen.Width + 1.0), (float) (this.Height - bitmap.Height - 15));
            break;
          case TipPointerPosition.BottomLeft:
            e.Graphics.DrawImage((Image) bitmap, 15f, (float) ((double) (this.Height - bitmap.Height) - (double) pen.Width + 1.0));
            break;
          case TipPointerPosition.BottomRight:
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            e.Graphics.DrawImage((Image) bitmap, (float) (this.Width - bitmap.Width - 15), (float) ((double) (this.Height - bitmap.Height) - (double) pen.Width + 1.0));
            break;
        }
        e.Graphics.SmoothingMode = SmoothingMode.Default;
        foreach (Control control in (ArrangedElementCollection) this.Controls)
          control.Refresh();
        bitmap.Dispose();
        brush2.Dispose();
        pen.Dispose();
        brush1.Dispose();
      }
      catch
      {
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Hide();

    private Rectangle getRectangleForControl(Control controlToReference)
    {
      return this.Parent.RectangleToClient(controlToReference.Parent.RectangleToScreen(controlToReference.Bounds));
    }

    private void positionChildElements()
    {
      this.bodyRect = new Rectangle(0, 0, this.pnlContent.Width + 8 + 4, this.pnlContent.Height + 8 + 4);
      switch (this.pointerPos)
      {
        case TipPointerPosition.LeftTop:
        case TipPointerPosition.LeftBottom:
          this.bodyRect.Offset(29, 0);
          this.pointerRect = new Rectangle(0, 0, 29, this.bodyRect.Height);
          break;
        case TipPointerPosition.TopLeft:
        case TipPointerPosition.TopRight:
          this.bodyRect.Offset(0, 29);
          this.pointerRect = new Rectangle(0, 0, this.bodyRect.Width, 29);
          break;
        case TipPointerPosition.RightTop:
        case TipPointerPosition.RightBottom:
          this.bodyRect.Offset(0, 0);
          this.pointerRect = new Rectangle(this.bodyRect.Width, 0, 29, this.bodyRect.Height);
          break;
        case TipPointerPosition.BottomLeft:
        case TipPointerPosition.BottomRight:
          this.bodyRect.Offset(0, 0);
          this.pointerRect = new Rectangle(0, this.bodyRect.Height, this.bodyRect.Width, 29);
          break;
      }
      switch (this.pointerPos)
      {
        case TipPointerPosition.LeftTop:
        case TipPointerPosition.LeftBottom:
        case TipPointerPosition.RightTop:
        case TipPointerPosition.RightBottom:
          this.ClientSize = new Size(this.bodyRect.Width + this.pointerRect.Width, this.bodyRect.Height);
          break;
        case TipPointerPosition.TopLeft:
        case TipPointerPosition.TopRight:
        case TipPointerPosition.BottomLeft:
        case TipPointerPosition.BottomRight:
          this.ClientSize = new Size(this.bodyRect.Width, this.bodyRect.Height + this.pointerRect.Height);
          break;
      }
      this.pnlContent.Location = new Point(this.bodyRect.Left + 2 + 4, this.bodyRect.Top + 2 + 4);
    }

    private TipPointerPosition getOptimalPointerPosition(Rectangle rect)
    {
      Rectangle clientRectangle = this.Parent.ClientRectangle;
      int num1 = rect.Top + rect.Height / 2 - 15;
      int num2 = clientRectangle.Height - rect.Bottom - rect.Height / 2 + 15;
      int num3 = rect.Left + rect.Width / 2 - 15;
      int num4 = clientRectangle.Width - rect.Right - rect.Width / 2 + 15;
      int num5 = this.pnlContent.Width + 8 + 6 + 29;
      return num1 > num2 ? (num3 > num4 ? (num2 < 0 || num4 >= 0 && num3 <= num5 ? TipPointerPosition.BottomRight : TipPointerPosition.RightBottom) : (num2 < 0 || num3 >= 0 && num4 <= num5 ? TipPointerPosition.BottomLeft : TipPointerPosition.LeftBottom)) : (num3 > num4 ? (num1 < 0 || num4 >= 0 && num3 <= num5 ? TipPointerPosition.TopRight : TipPointerPosition.RightTop) : (num1 < 0 || num3 >= 0 && num4 <= num5 ? TipPointerPosition.TopLeft : TipPointerPosition.LeftTop));
    }

    private void positionTipWindow()
    {
      int num1 = this.regionToPoint.Left + this.regionToPoint.Width / 2;
      int num2 = this.regionToPoint.Top + this.regionToPoint.Height / 2;
      int num3 = 23;
      switch (this.pointerPos)
      {
        case TipPointerPosition.LeftTop:
          this.Location = new Point(this.regionToPoint.Right + 4, num2 - num3);
          break;
        case TipPointerPosition.LeftBottom:
          this.Location = new Point(this.regionToPoint.Right + 4, num2 - this.Height + num3);
          break;
        case TipPointerPosition.TopLeft:
          this.Location = new Point(num1 - num3, this.regionToPoint.Bottom + 4);
          break;
        case TipPointerPosition.TopRight:
          this.Location = new Point(num1 - this.Width + num3, this.regionToPoint.Bottom + 4);
          break;
        case TipPointerPosition.RightTop:
          this.Location = new Point(this.regionToPoint.Left - this.Width - 4, num2 - num3);
          break;
        case TipPointerPosition.RightBottom:
          this.Location = new Point(this.regionToPoint.Left - this.Width - 4, num2 - this.Height + num3);
          break;
        case TipPointerPosition.BottomLeft:
          this.Location = new Point(num1 - num3, this.regionToPoint.Top - this.Height - 4);
          break;
        case TipPointerPosition.BottomRight:
          this.Location = new Point(num1 - this.Width + num3, this.regionToPoint.Top - this.Height - 4);
          break;
      }
    }

    protected override void OnLeave(EventArgs e)
    {
      try
      {
        base.OnLeave(e);
        this.Close();
      }
      catch
      {
      }
    }

    private void TipControl_ParentChanged(object sender, EventArgs e)
    {
      this.Parent.Resize += new EventHandler(this.Parent_Resize);
    }

    private void Parent_Resize(object sender, EventArgs e)
    {
      try
      {
        if (this.referenceObject is Control referenceObject && !referenceObject.IsDisposed)
        {
          this.regionToPoint = this.getRectangleForControl(referenceObject);
          this.positionChildElements();
          this.positionTipWindow();
        }
        this.Invalidate();
      }
      catch
      {
      }
    }

    private void pbClose_MouseEnter(object sender, EventArgs e)
    {
      this.pbClose.Image = this.rolloverCloseImage;
    }

    private void pbClose_MouseLeave(object sender, EventArgs e)
    {
      this.pbClose.Image = this.normalCloseImage;
    }

    private void pbClose_Click(object sender, EventArgs e) => this.Close();

    private void lnkMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.Close();
      if (this.Continue == null)
        return;
      this.Continue((object) this, new TipContinueEventArgs(this.parameters.TipID));
    }

    private void setTipStatus()
    {
      if (!this.chkHide.Checked)
        return;
      Session.WritePrivateProfileString("TIPS", this.parameters.TipID, "0");
    }

    [Serializable]
    private struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }
  }
}
