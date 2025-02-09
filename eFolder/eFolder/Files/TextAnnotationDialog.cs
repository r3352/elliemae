// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.TextAnnotationDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class TextAnnotationDialog : Form
  {
    private PdfTextAnnotation annotation;
    private IContainer components;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblContents;
    private TextBox txtContents;
    private Button btnCancel;
    private Button btnSave;

    public TextAnnotationDialog(PdfTextAnnotation annotation, bool isSDC = false)
    {
      this.InitializeComponent();
      this.annotation = annotation;
      this.txtTitle.Text = annotation.Title;
      if (isSDC)
        this.txtTitle.Text = SDCUtils.GetAnnotationTitleWithoutSeconds(annotation.Title);
      this.txtContents.Text = annotation.Contents;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string str = this.txtContents.Text.Trim();
      if (str == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter the contents of the annotation.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.annotation.Contents = str;
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
      this.lblTitle = new Label();
      this.txtTitle = new TextBox();
      this.lblContents = new Label();
      this.txtContents = new TextBox();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(8, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(26, 14);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Title";
      this.txtTitle.Location = new Point(64, 8);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(232, 20);
      this.txtTitle.TabIndex = 1;
      this.txtTitle.TabStop = false;
      this.lblContents.AutoSize = true;
      this.lblContents.Location = new Point(8, 36);
      this.lblContents.Name = "lblContents";
      this.lblContents.Size = new Size(50, 14);
      this.lblContents.TabIndex = 2;
      this.lblContents.Text = "Contents";
      this.txtContents.Location = new Point(64, 36);
      this.txtContents.Multiline = true;
      this.txtContents.Name = "txtContents";
      this.txtContents.ScrollBars = ScrollBars.Vertical;
      this.txtContents.Size = new Size(232, 76);
      this.txtContents.TabIndex = 3;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(222, 120);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Location = new Point(146, 120);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Add";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(304, 152);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.txtContents);
      this.Controls.Add((Control) this.lblContents);
      this.Controls.Add((Control) this.txtTitle);
      this.Controls.Add((Control) this.lblTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TextAnnotationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Note";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
