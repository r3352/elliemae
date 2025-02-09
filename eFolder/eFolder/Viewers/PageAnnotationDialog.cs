// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.PageAnnotationDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class PageAnnotationDialog : Form
  {
    private PageAnnotation annotation;
    private int left;
    private int top;
    private int width;
    private int height;
    private IContainer components;
    private Button btnCancel;
    private Button btnAdd;
    private TextBox txtContents;
    private Label lblContents;
    private TextBox txtCreatedBy;
    private Label lblCreatedBy;
    private ComboBox cboVisibility;
    private Label label1;

    public PageAnnotationDialog(int left, int top, int width, int height)
    {
      this.InitializeComponent();
      this.left = left;
      this.top = top;
      this.width = width;
      this.height = height;
      this.txtCreatedBy.Text = Session.UserInfo.FullName;
      this.cboVisibility.SelectedItem = (object) "Internal";
    }

    public PageAnnotation Annotation => this.annotation;

    private void btnAdd_Click(object sender, EventArgs e)
    {
      string text = this.txtContents.Text.Trim();
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter the contents of the note.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        PageAnnotationVisibilityType visibility = (PageAnnotationVisibilityType) Enum.Parse(typeof (PageAnnotationVisibilityType), this.cboVisibility.Text);
        this.annotation = new PageAnnotation(this.txtCreatedBy.Text, text, this.left, this.top, this.width, this.height, visibility);
        this.DialogResult = DialogResult.OK;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.txtContents = new TextBox();
      this.lblContents = new Label();
      this.txtCreatedBy = new TextBox();
      this.lblCreatedBy = new Label();
      this.cboVisibility = new ComboBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(230, 160);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Location = new Point(154, 160);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.txtContents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContents.Location = new Point(72, 66);
      this.txtContents.Multiline = true;
      this.txtContents.Name = "txtContents";
      this.txtContents.ScrollBars = ScrollBars.Vertical;
      this.txtContents.Size = new Size(232, 86);
      this.txtContents.TabIndex = 3;
      this.lblContents.AutoSize = true;
      this.lblContents.Location = new Point(12, 66);
      this.lblContents.Name = "lblContents";
      this.lblContents.Size = new Size(50, 14);
      this.lblContents.TabIndex = 2;
      this.lblContents.Text = "Contents";
      this.txtCreatedBy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCreatedBy.Location = new Point(72, 8);
      this.txtCreatedBy.Name = "txtCreatedBy";
      this.txtCreatedBy.ReadOnly = true;
      this.txtCreatedBy.Size = new Size(232, 20);
      this.txtCreatedBy.TabIndex = 1;
      this.txtCreatedBy.TabStop = false;
      this.lblCreatedBy.AutoSize = true;
      this.lblCreatedBy.Location = new Point(8, 12);
      this.lblCreatedBy.Name = "lblCreatedBy";
      this.lblCreatedBy.Size = new Size(61, 14);
      this.lblCreatedBy.TabIndex = 0;
      this.lblCreatedBy.Text = "Created By";
      this.cboVisibility.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboVisibility.FormattingEnabled = true;
      this.cboVisibility.Items.AddRange(new object[3]
      {
        (object) "Personal",
        (object) "Internal",
        (object) "Public"
      });
      this.cboVisibility.Location = new Point(72, 34);
      this.cboVisibility.Name = "cboVisibility";
      this.cboVisibility.Size = new Size(228, 22);
      this.cboVisibility.TabIndex = 6;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 14);
      this.label1.TabIndex = 7;
      this.label1.Text = "Visibility";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(312, 192);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboVisibility);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.txtContents);
      this.Controls.Add((Control) this.lblContents);
      this.Controls.Add((Control) this.txtCreatedBy);
      this.Controls.Add((Control) this.lblCreatedBy);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PageAnnotationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Note";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
