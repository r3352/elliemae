// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DocumentLabel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DocumentLabel : UserControl
  {
    private DocumentLog doc;
    private ListViewItem viewItem;
    private bool isRequired;
    private bool isAttachRequired;
    private IContainer components;
    private Label labelDocName;
    private Label labelStatus;
    private PictureBox picIsRequired;
    private PictureBox picAttachedRequired;
    private ToolTip toolTip1;

    public DocumentLabel(DocumentLog doc, ListViewItem viewItem)
    {
      this.doc = doc;
      this.viewItem = viewItem;
      this.InitializeComponent();
      this.picAttachedRequired.Top = this.picIsRequired.Top;
      this.picAttachedRequired.Left = this.picIsRequired.Left;
      this.RefreshDocumentStatus();
    }

    public void RefreshDocumentStatus()
    {
      this.picIsRequired.Visible = this.isRequired && !this.isAttachRequired;
      this.picAttachedRequired.Visible = this.isRequired && this.isAttachRequired;
      string str = this.doc.Title;
      if (this.doc is VerifLog)
        str = str + "-" + this.doc.RequestedFrom;
      this.labelDocName.Text = str;
      if (this.doc.Status != "needed" && this.doc.Status != "")
        this.labelStatus.Text = "  " + this.doc.Status + " : " + this.doc.Date.ToString("MM/dd/yy");
      else
        this.labelStatus.Text = string.Empty;
      this.labelStatus.Left = this.labelDocName.Left + this.labelDocName.Width;
    }

    public bool IsRequired
    {
      get => this.isRequired;
      set => this.isRequired = value;
    }

    public bool IsAttachRequired
    {
      get => this.isAttachRequired;
      set => this.isAttachRequired = value;
    }

    private void labelDocName_Click(object sender, EventArgs e)
    {
      if (this.doc == null)
        return;
      DocumentDetailsDialog.ShowInstance(Session.LoanDataMgr, this.doc);
      this.viewItem.Checked = this.doc.Received;
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      this.RefreshDocumentStatus();
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
      this.labelDocName = new Label();
      this.labelStatus = new Label();
      this.picAttachedRequired = new PictureBox();
      this.picIsRequired = new PictureBox();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picAttachedRequired).BeginInit();
      ((ISupportInitialize) this.picIsRequired).BeginInit();
      this.SuspendLayout();
      this.labelDocName.AutoSize = true;
      this.labelDocName.Cursor = Cursors.Hand;
      this.labelDocName.ForeColor = SystemColors.HotTrack;
      this.labelDocName.Location = new Point(12, 1);
      this.labelDocName.Name = "labelDocName";
      this.labelDocName.Size = new Size(64, 13);
      this.labelDocName.TabIndex = 0;
      this.labelDocName.Text = "(Doc Name)";
      this.labelDocName.Click += new EventHandler(this.labelDocName_Click);
      this.labelStatus.AutoSize = true;
      this.labelStatus.Location = new Point(80, 1);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new Size(66, 13);
      this.labelStatus.TabIndex = 1;
      this.labelStatus.Text = "(Doc Status)";
      this.picAttachedRequired.Image = (Image) Resources.attachment_required;
      this.picAttachedRequired.Location = new Point(157, -1);
      this.picAttachedRequired.Name = "picAttachedRequired";
      this.picAttachedRequired.Size = new Size(16, 16);
      this.picAttachedRequired.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picAttachedRequired.TabIndex = 7;
      this.picAttachedRequired.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.picAttachedRequired, "Attachment is required");
      this.picAttachedRequired.Visible = false;
      this.picIsRequired.Image = (Image) Resources.required;
      this.picIsRequired.Location = new Point(0, 0);
      this.picIsRequired.Name = "picIsRequired";
      this.picIsRequired.Size = new Size(16, 16);
      this.picIsRequired.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picIsRequired.TabIndex = 6;
      this.picIsRequired.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.picIsRequired, "Document is required");
      this.picIsRequired.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.picAttachedRequired);
      this.Controls.Add((Control) this.picIsRequired);
      this.Controls.Add((Control) this.labelStatus);
      this.Controls.Add((Control) this.labelDocName);
      this.Name = nameof (DocumentLabel);
      this.Size = new Size(330, 14);
      ((ISupportInitialize) this.picAttachedRequired).EndInit();
      ((ISupportInitialize) this.picIsRequired).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
