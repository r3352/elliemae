// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProfilePictureEdit
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ProfilePictureEdit : Form
  {
    public Image newImage;
    public string url = "";
    private Rectangle _drawRect;
    private bool _mouseIsOnRectange;
    private Point _startingMousePoint;
    private bool dirtyflag;
    private IContainer components;
    private PictureBox pictureBox;
    private TextBox picFilePath;
    private Button browseBtn;
    private Button saveBtn;
    private Button closeBtn;
    private Label label1;
    private Label label2;

    public ProfilePictureEdit(Image img)
    {
      this.InitializeComponent();
      this.newImage = img;
      this.pictureBox.Image = img;
      this.pictureBox.Paint += new PaintEventHandler(this.pictureBox_Paint);
      this.refreshPictureBox();
      this.drawRectInit(300, 300);
    }

    private void pictureBox_Paint(object sender, PaintEventArgs e)
    {
      using (Pen pen = new Pen(Brushes.Cornsilk, 3f))
        e.Graphics.DrawRectangle(pen, this._drawRect);
    }

    private void refreshPictureBox()
    {
      this.pictureBox.Height = this.pictureBox.Width = 300;
      int Height;
      if (this.pictureBox.Image.Height > this.pictureBox.Height && this.pictureBox.Image.Height > 400)
      {
        Height = 400;
        this.pictureBox.Height = 400;
      }
      else
      {
        this.pictureBox.Height = this.pictureBox.Image.Height;
        Height = this.pictureBox.Height;
      }
      int Width;
      if (this.pictureBox.Image.Width > this.pictureBox.Width && this.pictureBox.Image.Width > 600)
      {
        this.pictureBox.Width = 600;
        Width = 600;
      }
      else
      {
        this.pictureBox.Width = this.pictureBox.Image.Width;
        Width = this.pictureBox.Width;
      }
      this.pictureBox.Image = FileImageHelper.FixedSize(this.pictureBox.Image, Width, Height);
    }

    private static Image FixedSize(Image imgPhoto, int Width, int Height)
    {
      int width1 = imgPhoto.Width;
      int height1 = imgPhoto.Height;
      int x1 = 0;
      int y1 = 0;
      int x2 = 0;
      int y2 = 0;
      float num1 = (float) Width / (float) width1;
      float num2 = (float) Height / (float) height1;
      float num3;
      if ((double) num2 < (double) num1)
      {
        num3 = num2;
        x2 = (int) Convert.ToInt16((float) (((double) Width - (double) width1 * (double) num3) / 2.0));
      }
      else
      {
        num3 = num1;
        y2 = (int) Convert.ToInt16((float) (((double) Height - (double) height1 * (double) num3) / 2.0));
      }
      int width2 = (int) ((double) width1 * (double) num3);
      int height2 = (int) ((double) height1 * (double) num3);
      Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
      bitmap.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.Clear(Color.Transparent);
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.DrawImage(imgPhoto, new Rectangle(x2, y2, width2, height2), new Rectangle(x1, y1, width1, height1), GraphicsUnit.Pixel);
      graphics.Dispose();
      return (Image) bitmap;
    }

    private void browseBtn_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Upload Photo";
        openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        string fileName = openFileDialog.FileName;
        this.picFilePath.Text = fileName;
        if (FileImageHelper.ValidPhoto(fileName))
        {
          Image image = Image.FromFile(fileName);
          if (!FileImageHelper.ValidDimensions(image.Width, image.Height))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Profile picture size does not meet the minimum size requirements of 300 pixels length and 300 pixels width.  We recommend that you upload a picture that is larger in size.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.pictureBox.Image = image;
          this.refreshPictureBox();
          this.drawRectInit(300, 300);
        }
        else
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Profile photo must be between 3kb and 2MB.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      if (this.pictureBox.Image == null)
        return;
      Image image1 = this.pictureBox.Image;
      using (Bitmap bitmap = new Bitmap(image1))
      {
        string filename = Path.Combine(EnConfigurationSettings.GlobalSettings.AppSettingsReportDownloadsDirectory, string.Format("croppedimage_{0}.PNG", (object) new Random((int) DateTime.Now.Ticks).Next()));
        Image image2;
        if (this._drawRect.Width != 0 && this._drawRect.Height != 0)
        {
          image2 = FileImageHelper.FixedSize((Image) bitmap.Clone(this._drawRect, bitmap.PixelFormat), 300, 300);
          this._drawRect = new Rectangle();
        }
        else
          image2 = image1.Width > 300 || image1.Height > 300 ? FileImageHelper.FixedSize(image1, 300, 300) : image1;
        image2.Save(filename, ImageFormat.Png);
        this.pictureBox.Image = Image.FromFile(filename);
        this.newImage = this.pictureBox.Image;
        this.dirtyflag = true;
      }
    }

    private void pictureBox_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.pictureBox.Image == null || this.pictureBox.Image.Height <= 300 && this.pictureBox.Image.Width <= 300 || e.Button != MouseButtons.Left)
        return;
      this._startingMousePoint = e.Location;
      this._mouseIsOnRectange = this.IsClickedOnRectangle(this._startingMousePoint);
      if (this._mouseIsOnRectange)
        return;
      this._drawRect = new Rectangle();
      this._drawRect.X = this._startingMousePoint.X;
      this._drawRect.Y = this._startingMousePoint.Y;
      this._drawRect.Width = this._drawRect.Height = 0;
    }

    private bool IsClickedOnRectangle(Point mousePoint)
    {
      return mousePoint.X >= this._drawRect.Left && mousePoint.X <= this._drawRect.Right && mousePoint.Y >= this._drawRect.Top && mousePoint.Y <= this._drawRect.Bottom;
    }

    private void pictureBox_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.pictureBox.Image == null || this.pictureBox.Image.Height <= 300 && this.pictureBox.Image.Width <= 300 || e.Button != MouseButtons.Left)
        return;
      Point location = e.Location;
      if (location.X < this._drawRect.X)
        this._drawRect.X = location.X;
      if (location.Y < this._drawRect.Y)
        this._drawRect.Y = location.Y;
      this._drawRect.Width = Math.Abs(location.X - this._drawRect.X);
      this._drawRect.Height = Math.Abs(location.Y - this._drawRect.Y);
      this.pictureBox.Invalidate();
    }

    private void drawRectInit(int width, int height)
    {
      if (this.pictureBox.Image == null || this.pictureBox.Image.Width <= 300 && this.pictureBox.Image.Height <= 300)
        return;
      if (this.pictureBox.Image.Width <= 300)
      {
        this._drawRect.X = 0;
        this._drawRect.Width = this.pictureBox.Image.Width;
      }
      else
      {
        this._drawRect.X = (this.pictureBox.Image.Width - 300) / 2;
        this._drawRect.Width = width;
      }
      if (this.pictureBox.Image.Height <= 300)
      {
        this._drawRect.Y = 0;
        this._drawRect.Height = this.pictureBox.Image.Height;
      }
      else
      {
        this._drawRect.Y = (this.pictureBox.Image.Height - 300) / 2;
        this._drawRect.Height = height;
      }
      this.pictureBox.Invalidate();
    }

    private void ProfilePictureEdit_Load(object sender, EventArgs e)
    {
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      if (!this.dirtyflag)
        this.newImage = (Image) null;
      this.Close();
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBox = new PictureBox();
      this.picFilePath = new TextBox();
      this.browseBtn = new Button();
      this.saveBtn = new Button();
      this.closeBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      ((ISupportInitialize) this.pictureBox).BeginInit();
      this.SuspendLayout();
      this.pictureBox.Location = new Point(42, 54);
      this.pictureBox.MaximumSize = new Size(600, 400);
      this.pictureBox.MinimumSize = new Size(300, 300);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new Size(600, 400);
      this.pictureBox.TabIndex = 0;
      this.pictureBox.TabStop = false;
      this.pictureBox.MouseDown += new MouseEventHandler(this.pictureBox_MouseDown);
      this.pictureBox.MouseUp += new MouseEventHandler(this.pictureBox_MouseUp);
      this.picFilePath.Location = new Point(59, 495);
      this.picFilePath.Name = "picFilePath";
      this.picFilePath.Size = new Size(246, 20);
      this.picFilePath.TabIndex = 1;
      this.browseBtn.Location = new Point(311, 492);
      this.browseBtn.Name = "browseBtn";
      this.browseBtn.Size = new Size(122, 23);
      this.browseBtn.TabIndex = 2;
      this.browseBtn.Text = "Change Profile Picture";
      this.browseBtn.UseVisualStyleBackColor = true;
      this.browseBtn.Click += new EventHandler(this.browseBtn_Click);
      this.saveBtn.Location = new Point(486, 492);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 23);
      this.saveBtn.TabIndex = 3;
      this.saveBtn.Text = "Save";
      this.saveBtn.UseVisualStyleBackColor = true;
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.closeBtn.Location = new Point(567, 492);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 23);
      this.closeBtn.TabIndex = 4;
      this.closeBtn.Text = "Close";
      this.closeBtn.UseVisualStyleBackColor = true;
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(39, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(373, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Drag & reposition  the square below and click Save to save the profile picture.  ";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(39, 30);
      this.label2.Name = "label2";
      this.label2.Size = new Size(330, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "To upload a different picture, click the Change Profile Picture button.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(688, 567);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.browseBtn);
      this.Controls.Add((Control) this.picFilePath);
      this.Controls.Add((Control) this.pictureBox);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProfilePictureEdit);
      this.ShowIcon = false;
      this.Text = "Edit Profile Picture";
      this.Load += new EventHandler(this.ProfilePictureEdit_Load);
      ((ISupportInitialize) this.pictureBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
