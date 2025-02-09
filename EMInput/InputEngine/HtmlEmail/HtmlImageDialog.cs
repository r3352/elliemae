// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlImageDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlImageDialog : Form
  {
    private List<ImageInfo> companyImageInfo;
    private List<ImageInfo> userImageInfo;
    private ResourceAccessType accessType = ResourceAccessType.Company;
    private string url = string.Empty;
    private bool showUserImage = true;
    private Sessions.Session session;
    private const string NAME = "NAME";
    private IContainer components;
    private GroupContainer gcImages;
    private GridView gvImages;
    private GradientPanel gradImages;
    private PictureBox pictureSelection;
    private RadioButton radioUser;
    private RadioButton radioCompany;
    private StandardIconButton btnDeleteImage;
    private StandardIconButton btnAddImage;
    private ToolTip tooltip;
    private Label lblFileName;
    private Label lblSizeValue;
    private Label lblSize;
    private Label lblFileNameValue;
    private Label lblUploadedDateValue;
    private Label lblUploadedDate;
    private Label lblUploadedByValue;
    private Label lblUploadedBy;
    private Button btnInsert;
    private Button btnCancel;
    private EMHelpLink emHelpLink;

    public string Url => this.url;

    public HtmlImageDialog(
      List<ImageInfo> companyImageInfo,
      List<ImageInfo> userImageInfo,
      bool showUserImage)
      : this(companyImageInfo, userImageInfo, showUserImage, Session.DefaultInstance)
    {
    }

    public HtmlImageDialog(
      List<ImageInfo> companyImageInfo,
      List<ImageInfo> userImageInfo,
      bool showUserImage,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.companyImageInfo = companyImageInfo;
      this.userImageInfo = userImageInfo;
      this.showUserImage = showUserImage;
      this.session = session;
      if (!this.showUserImage)
        this.emHelpLink.HelpTag = "Company Status Online";
      this.initImageList();
      this.initImageAccess();
      this.applySecurity();
    }

    private void initImageAccess()
    {
      this.radioCompany.Checked = true;
      this.setResourceAccessType();
    }

    private void setResourceAccessType()
    {
      if (this.radioCompany.Checked)
        this.accessType = ResourceAccessType.Company;
      else if (this.radioUser.Checked)
        this.accessType = ResourceAccessType.User;
      else
        this.accessType = ResourceAccessType.None;
    }

    private void applySecurity()
    {
      if (this.showUserImage)
        return;
      this.radioUser.Visible = false;
    }

    private void initImageList()
    {
      this.createLayout(new TableLayout.Column[1]
      {
        HtmlImageDialog.nameColumn
      });
    }

    private void loadImageList(ImageInfo selectedImageInfo)
    {
      this.gvImages.Items.Clear();
      if (this.accessType == ResourceAccessType.Company)
      {
        foreach (ImageInfo imageInfo in this.companyImageInfo)
        {
          GVItem gvItem = this.addItem(imageInfo);
          if (this.companyImageInfo.Contains(selectedImageInfo))
            gvItem.Selected = true;
        }
      }
      if (this.accessType == ResourceAccessType.User)
      {
        foreach (ImageInfo imageInfo in this.userImageInfo)
        {
          GVItem gvItem = this.addItem(imageInfo);
          if (this.userImageInfo.Contains(selectedImageInfo))
            gvItem.Selected = true;
        }
      }
      this.gvImages.Sort(0, SortOrder.Ascending);
      if (this.gvImages.Items.Count > 0 && selectedImageInfo == null)
        this.gvImages.Items[0].Selected = true;
      this.displayImageValues();
    }

    private void displayImageValues()
    {
      if (this.gvImages.Items.Count > 0 && this.gvImages.SelectedItems.Count > 0)
      {
        GVItem selectedItem = this.gvImages.SelectedItems[0];
        if (selectedItem != null)
        {
          ImageInfo tag = (ImageInfo) selectedItem.Tag;
          if (tag != null)
          {
            this.pictureSelection.ImageLocation = tag.Url;
            this.lblFileNameValue.Text = tag.Filename;
            this.lblSizeValue.Text = tag.Filesize.ToString() + " KB";
            this.lblUploadedByValue.Text = tag.UploadedBy;
            if (tag.UploadedDate != DateTime.MinValue)
            {
              this.lblUploadedDateValue.Text = tag.UploadedDate.ToString();
              return;
            }
            this.lblUploadedDateValue.Text = string.Empty;
            return;
          }
        }
      }
      this.pictureSelection.ImageLocation = string.Empty;
      this.lblFileNameValue.Text = string.Empty;
      this.lblSizeValue.Text = string.Empty;
      this.lblUploadedByValue.Text = string.Empty;
      this.lblUploadedDateValue.Text = string.Empty;
    }

    private static TableLayout.Column nameColumn
    {
      get => new TableLayout.Column("NAME", "Name", HorizontalAlignment.Left, 225);
    }

    private GVItem addItem(ImageInfo imageInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) imageInfo;
      foreach (GVColumn column in this.gvImages.Columns)
      {
        GVSubItem subItem = gvItem.SubItems[column.Index];
        if (((TableLayout.Column) column.Tag).ColumnID == "NAME")
          subItem.Value = (object) imageInfo.Filename;
      }
      this.gvImages.Items.Add(gvItem);
      return gvItem;
    }

    private void createLayout(TableLayout.Column[] columnList)
    {
      GridViewLayoutManager.ApplyLayoutToGridView(this.gvImages, new TableLayout(columnList));
      if (this.gvImages.HeaderVisible)
        return;
      this.gvImages.Columns[0].SpringToFit = true;
    }

    private void radioCompany_CheckedChanged(object sender, EventArgs e)
    {
      this.setResourceAccessType();
      if (this.accessType != ResourceAccessType.Company)
        return;
      if (!"admin".Equals(this.session.UserID, StringComparison.InvariantCultureIgnoreCase))
      {
        this.btnAddImage.Visible = false;
        this.btnDeleteImage.Visible = false;
      }
      this.loadImageList((ImageInfo) null);
    }

    private void radioUser_CheckedChanged(object sender, EventArgs e)
    {
      this.setResourceAccessType();
      if (this.accessType != ResourceAccessType.User)
        return;
      this.btnAddImage.Visible = true;
      this.btnDeleteImage.Visible = true;
      this.loadImageList((ImageInfo) null);
    }

    private void btnAddImage_Click(object sender, EventArgs e)
    {
      using (ImageLibraryClient imageLibraryClient = new ImageLibraryClient())
      {
        List<string> stringList = new List<string>();
        if (this.accessType == ResourceAccessType.Company)
        {
          foreach (ImageInfo imageInfo in this.companyImageInfo)
            stringList.Add(imageInfo.Filename);
        }
        if (this.accessType == ResourceAccessType.User)
        {
          foreach (ImageInfo imageInfo in this.userImageInfo)
            stringList.Add(imageInfo.Filename);
        }
        ImageInfo imageInfo1 = imageLibraryClient.UploadImage(this.accessType, stringList.ToArray(), this.session);
        if (imageInfo1 == null)
          return;
        if (this.accessType == ResourceAccessType.Company)
          this.companyImageInfo.Add(imageInfo1);
        else if (this.accessType == ResourceAccessType.User)
          this.userImageInfo.Add(imageInfo1);
        this.addItem(imageInfo1).Selected = true;
        this.gvImages.Sort(0, SortOrder.Ascending);
        this.displayImageValues();
      }
    }

    private void btnDeleteImage_Click(object sender, EventArgs e)
    {
      if (this.gvImages.Items.Count <= 0 || this.gvImages.SelectedItems.Count <= 0)
        return;
      GVItem selectedItem = this.gvImages.SelectedItems[0];
      if (selectedItem == null)
        return;
      ImageInfo tag = (ImageInfo) selectedItem.Tag;
      if (tag == null)
        return;
      using (ImageLibraryClient imageLibraryClient = new ImageLibraryClient())
      {
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the image '" + tag.Filename + "' from " + this.accessType.ToString() + " images", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No || !imageLibraryClient.DeleteImage(this.accessType, tag.Filename, this.session))
          return;
        switch (this.accessType)
        {
          case ResourceAccessType.Company:
            this.companyImageInfo.Remove(tag);
            this.gvImages.Items.Remove(selectedItem);
            if (this.gvImages.Items.Count <= 0)
              break;
            this.gvImages.Items[0].Selected = true;
            this.displayImageValues();
            break;
          case ResourceAccessType.User:
            this.userImageInfo.Remove(tag);
            this.gvImages.Items.Remove(selectedItem);
            if (this.gvImages.Items.Count <= 0)
              break;
            this.gvImages.Items[0].Selected = true;
            this.displayImageValues();
            break;
        }
      }
    }

    private void btnInsert_Click(object sender, EventArgs e)
    {
      if (this.gvImages.Items.Count <= 0 || this.gvImages.SelectedItems.Count <= 0)
        return;
      GVItem selectedItem = this.gvImages.SelectedItems[0];
      if (selectedItem == null)
        return;
      ImageInfo tag = (ImageInfo) selectedItem.Tag;
      if (tag == null)
        return;
      this.url = tag.Url;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void gvImages_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.displayImageValues();
    }

    private void HtmlImageDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.session.Application.DisplayHelp(this.emHelpLink.HelpTag);
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
      this.gcImages = new GroupContainer();
      this.radioUser = new RadioButton();
      this.radioCompany = new RadioButton();
      this.btnDeleteImage = new StandardIconButton();
      this.btnAddImage = new StandardIconButton();
      this.pictureSelection = new PictureBox();
      this.gvImages = new GridView();
      this.gradImages = new GradientPanel();
      this.lblUploadedByValue = new Label();
      this.lblUploadedBy = new Label();
      this.lblUploadedDateValue = new Label();
      this.lblUploadedDate = new Label();
      this.lblSizeValue = new Label();
      this.lblSize = new Label();
      this.lblFileNameValue = new Label();
      this.lblFileName = new Label();
      this.tooltip = new ToolTip(this.components);
      this.btnInsert = new Button();
      this.btnCancel = new Button();
      this.emHelpLink = new EMHelpLink();
      this.gcImages.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteImage).BeginInit();
      ((ISupportInitialize) this.btnAddImage).BeginInit();
      ((ISupportInitialize) this.pictureSelection).BeginInit();
      this.gradImages.SuspendLayout();
      this.SuspendLayout();
      this.gcImages.Anchor = AnchorStyles.Left;
      this.gcImages.Controls.Add((Control) this.radioUser);
      this.gcImages.Controls.Add((Control) this.radioCompany);
      this.gcImages.Controls.Add((Control) this.btnDeleteImage);
      this.gcImages.Controls.Add((Control) this.btnAddImage);
      this.gcImages.Controls.Add((Control) this.pictureSelection);
      this.gcImages.Controls.Add((Control) this.gvImages);
      this.gcImages.Controls.Add((Control) this.gradImages);
      this.gcImages.HeaderForeColor = SystemColors.ControlText;
      this.gcImages.Location = new Point(0, 0);
      this.gcImages.Name = "gcImages";
      this.gcImages.Size = new Size(721, 433);
      this.gcImages.TabIndex = 0;
      this.radioUser.AutoSize = true;
      this.radioUser.BackColor = Color.Transparent;
      this.radioUser.Location = new Point(118, 3);
      this.radioUser.Name = "radioUser";
      this.radioUser.Size = new Size(85, 18);
      this.radioUser.TabIndex = 6;
      this.radioUser.TabStop = true;
      this.radioUser.Text = "User Images";
      this.radioUser.UseVisualStyleBackColor = false;
      this.radioUser.CheckedChanged += new EventHandler(this.radioUser_CheckedChanged);
      this.radioCompany.AutoSize = true;
      this.radioCompany.BackColor = Color.Transparent;
      this.radioCompany.Location = new Point(6, 3);
      this.radioCompany.Name = "radioCompany";
      this.radioCompany.Size = new Size(107, 18);
      this.radioCompany.TabIndex = 5;
      this.radioCompany.TabStop = true;
      this.radioCompany.Text = "Company Images";
      this.radioCompany.UseVisualStyleBackColor = false;
      this.radioCompany.CheckedChanged += new EventHandler(this.radioCompany_CheckedChanged);
      this.btnDeleteImage.BackColor = Color.Transparent;
      this.btnDeleteImage.Location = new Point(695, 4);
      this.btnDeleteImage.MouseDownImage = (Image) null;
      this.btnDeleteImage.Name = "btnDeleteImage";
      this.btnDeleteImage.Size = new Size(16, 16);
      this.btnDeleteImage.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteImage.TabIndex = 4;
      this.btnDeleteImage.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteImage, "Delete Image");
      this.btnDeleteImage.Click += new EventHandler(this.btnDeleteImage_Click);
      this.btnAddImage.BackColor = Color.Transparent;
      this.btnAddImage.Location = new Point(671, 4);
      this.btnAddImage.MouseDownImage = (Image) null;
      this.btnAddImage.Name = "btnAddImage";
      this.btnAddImage.Size = new Size(16, 16);
      this.btnAddImage.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddImage.TabIndex = 3;
      this.btnAddImage.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddImage, "Add Image");
      this.btnAddImage.Click += new EventHandler(this.btnAddImage_Click);
      this.pictureSelection.Location = new Point(299, 88);
      this.pictureSelection.Name = "pictureSelection";
      this.pictureSelection.Size = new Size(388, 315);
      this.pictureSelection.TabIndex = 2;
      this.pictureSelection.TabStop = false;
      this.gvImages.AllowMultiselect = false;
      this.gvImages.BorderStyle = BorderStyle.FixedSingle;
      this.gvImages.ClearSelectionsOnEmptyRowClick = false;
      this.gvImages.Dock = DockStyle.Left;
      this.gvImages.Location = new Point(1, 60);
      this.gvImages.Name = "gvImages";
      this.gvImages.Size = new Size(269, 372);
      this.gvImages.TabIndex = 0;
      this.gvImages.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvImages.SelectedIndexChanged += new EventHandler(this.gvImages_SelectedIndexChanged);
      this.gradImages.Borders = AnchorStyles.Bottom;
      this.gradImages.Controls.Add((Control) this.lblUploadedByValue);
      this.gradImages.Controls.Add((Control) this.lblUploadedBy);
      this.gradImages.Controls.Add((Control) this.lblUploadedDateValue);
      this.gradImages.Controls.Add((Control) this.lblUploadedDate);
      this.gradImages.Controls.Add((Control) this.lblSizeValue);
      this.gradImages.Controls.Add((Control) this.lblSize);
      this.gradImages.Controls.Add((Control) this.lblFileNameValue);
      this.gradImages.Controls.Add((Control) this.lblFileName);
      this.gradImages.Dock = DockStyle.Top;
      this.gradImages.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradImages.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradImages.Location = new Point(1, 26);
      this.gradImages.Name = "gradImages";
      this.gradImages.Size = new Size(719, 34);
      this.gradImages.TabIndex = 1;
      this.lblUploadedByValue.AutoSize = true;
      this.lblUploadedByValue.BackColor = Color.Transparent;
      this.lblUploadedByValue.Location = new Point(586, 9);
      this.lblUploadedByValue.Name = "lblUploadedByValue";
      this.lblUploadedByValue.Size = new Size(0, 14);
      this.lblUploadedByValue.TabIndex = 7;
      this.lblUploadedBy.AutoSize = true;
      this.lblUploadedBy.BackColor = Color.Transparent;
      this.lblUploadedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblUploadedBy.Location = new Point(497, 9);
      this.lblUploadedBy.Name = "lblUploadedBy";
      this.lblUploadedBy.Size = new Size(83, 13);
      this.lblUploadedBy.TabIndex = 6;
      this.lblUploadedBy.Text = "Uploaded By:";
      this.lblUploadedDateValue.AutoSize = true;
      this.lblUploadedDateValue.BackColor = Color.Transparent;
      this.lblUploadedDateValue.Location = new Point(370, 9);
      this.lblUploadedDateValue.Name = "lblUploadedDateValue";
      this.lblUploadedDateValue.Size = new Size(0, 14);
      this.lblUploadedDateValue.TabIndex = 5;
      this.lblUploadedDate.AutoSize = true;
      this.lblUploadedDate.BackColor = Color.Transparent;
      this.lblUploadedDate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblUploadedDate.Location = new Point(270, 9);
      this.lblUploadedDate.Name = "lblUploadedDate";
      this.lblUploadedDate.Size = new Size(96, 13);
      this.lblUploadedDate.TabIndex = 4;
      this.lblUploadedDate.Text = "Uploaded Date:";
      this.lblSizeValue.AutoSize = true;
      this.lblSizeValue.BackColor = Color.Transparent;
      this.lblSizeValue.Location = new Point(222, 9);
      this.lblSizeValue.Name = "lblSizeValue";
      this.lblSizeValue.Size = new Size(0, 14);
      this.lblSizeValue.TabIndex = 3;
      this.lblSize.AutoSize = true;
      this.lblSize.BackColor = Color.Transparent;
      this.lblSize.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSize.Location = new Point(183, 9);
      this.lblSize.Name = "lblSize";
      this.lblSize.Size = new Size(35, 13);
      this.lblSize.TabIndex = 2;
      this.lblSize.Text = "Size:";
      this.lblFileNameValue.AutoSize = true;
      this.lblFileNameValue.BackColor = Color.Transparent;
      this.lblFileNameValue.Location = new Point(82, 9);
      this.lblFileNameValue.Name = "lblFileNameValue";
      this.lblFileNameValue.Size = new Size(0, 14);
      this.lblFileNameValue.TabIndex = 1;
      this.lblFileName.AutoSize = true;
      this.lblFileName.BackColor = Color.Transparent;
      this.lblFileName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFileName.Location = new Point(10, 9);
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.Size = new Size(67, 13);
      this.lblFileName.TabIndex = 0;
      this.lblFileName.Text = "File Name:";
      this.btnInsert.Location = new Point(536, 444);
      this.btnInsert.Name = "btnInsert";
      this.btnInsert.Size = new Size(75, 23);
      this.btnInsert.TabIndex = 1;
      this.btnInsert.Text = "Insert";
      this.btnInsert.UseVisualStyleBackColor = true;
      this.btnInsert.Click += new EventHandler(this.btnInsert_Click);
      this.btnCancel.Location = new Point(634, 444);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.emHelpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink.BackColor = Color.Transparent;
      this.emHelpLink.Cursor = Cursors.Hand;
      this.emHelpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink.Location = new Point(8, 444);
      this.emHelpLink.Name = "emHelpLink";
      this.emHelpLink.Size = new Size(90, 17);
      this.emHelpLink.TabIndex = 3;
      this.emHelpLink.HelpTag = "HTML Email Templates";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(719, 476);
      this.Controls.Add((Control) this.emHelpLink);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnInsert);
      this.Controls.Add((Control) this.gcImages);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HtmlImageDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.ShowInTaskbar = false;
      this.Text = "Insert Image";
      this.KeyDown += new KeyEventHandler(this.HtmlImageDialog_KeyDown);
      this.gcImages.ResumeLayout(false);
      this.gcImages.PerformLayout();
      ((ISupportInitialize) this.btnDeleteImage).EndInit();
      ((ISupportInitialize) this.btnAddImage).EndInit();
      ((ISupportInitialize) this.pictureSelection).EndInit();
      this.gradImages.ResumeLayout(false);
      this.gradImages.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
