// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.EDMLogControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class EDMLogControl : UserControl
  {
    private EDMLog log;
    private IContainer components;
    private GroupContainer gcHeader;
    private TextBox txtDate;
    private Label lblDate;
    private Label lblCreatedBy;
    private TextBox txtCreatedBy;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private GroupContainer gcComments;
    private HtmlEmailControl htmlEmail;

    public EDMLogControl(EDMLog log)
    {
      this.InitializeComponent();
      this.log = log;
      this.loadDetails();
      this.Dock = DockStyle.Fill;
    }

    private void loadDetails()
    {
      this.gcHeader.Text = this.log.Description;
      this.txtCreatedBy.Text = this.log.CreatedBy;
      this.txtDate.Text = this.log.Date.ToString("MM/dd/yy hh:mm tt");
      this.htmlEmail.ShowToolbar = false;
      if (this.log.Comments.ToUpper().Contains("<HTML>"))
        this.htmlEmail.LoadHtml(this.log.Comments, true);
      else
        this.htmlEmail.LoadText(this.log.Comments, true);
      foreach (string document in this.log.Documents)
        this.gvDocuments.Items.Add(document);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.gcHeader = new GroupContainer();
      this.txtDate = new TextBox();
      this.lblDate = new Label();
      this.lblCreatedBy = new Label();
      this.txtCreatedBy = new TextBox();
      this.gcDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.gcComments = new GroupContainer();
      this.htmlEmail = new HtmlEmailControl();
      this.gcHeader.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.gcComments.SuspendLayout();
      this.SuspendLayout();
      this.gcHeader.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcHeader.Controls.Add((Control) this.txtDate);
      this.gcHeader.Controls.Add((Control) this.lblDate);
      this.gcHeader.Controls.Add((Control) this.lblCreatedBy);
      this.gcHeader.Controls.Add((Control) this.txtCreatedBy);
      this.gcHeader.Dock = DockStyle.Top;
      this.gcHeader.HeaderForeColor = SystemColors.ControlText;
      this.gcHeader.Location = new Point(0, 0);
      this.gcHeader.Name = "gcHeader";
      this.gcHeader.Size = new Size(567, 75);
      this.gcHeader.TabIndex = 0;
      this.txtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDate.BackColor = Color.WhiteSmoke;
      this.txtDate.BorderStyle = BorderStyle.None;
      this.txtDate.Location = new Point(76, 52);
      this.txtDate.Name = "txtDate";
      this.txtDate.ReadOnly = true;
      this.txtDate.Size = new Size(483, 13);
      this.txtDate.TabIndex = 4;
      this.txtDate.TabStop = false;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(8, 52);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(32, 14);
      this.lblDate.TabIndex = 3;
      this.lblDate.Text = "Date:";
      this.lblCreatedBy.AutoSize = true;
      this.lblCreatedBy.Location = new Point(8, 32);
      this.lblCreatedBy.Name = "lblCreatedBy";
      this.lblCreatedBy.Size = new Size(64, 14);
      this.lblCreatedBy.TabIndex = 1;
      this.lblCreatedBy.Text = "Created By:";
      this.txtCreatedBy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCreatedBy.BackColor = Color.WhiteSmoke;
      this.txtCreatedBy.BorderStyle = BorderStyle.None;
      this.txtCreatedBy.Location = new Point(76, 32);
      this.txtCreatedBy.Name = "txtCreatedBy";
      this.txtCreatedBy.ReadOnly = true;
      this.txtCreatedBy.Size = new Size(483, 13);
      this.txtCreatedBy.TabIndex = 2;
      this.txtCreatedBy.TabStop = false;
      this.gcDocuments.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Top;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 75);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(567, 242);
      this.gcDocuments.TabIndex = 5;
      this.gcDocuments.Text = "Documents";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colTitle";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Title";
      gvColumn.Width = 565;
      this.gvDocuments.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HeaderHeight = 0;
      this.gvDocuments.HeaderVisible = false;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(565, 216);
      this.gvDocuments.TabIndex = 6;
      this.gvDocuments.TabStop = false;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gcComments.Controls.Add((Control) this.htmlEmail);
      this.gcComments.Dock = DockStyle.Fill;
      this.gcComments.HeaderForeColor = SystemColors.ControlText;
      this.gcComments.Location = new Point(0, 317);
      this.gcComments.Name = "gcComments";
      this.gcComments.Size = new Size(567, 151);
      this.gcComments.TabIndex = 7;
      this.gcComments.Text = "Details";
      this.htmlEmail.Dock = DockStyle.Fill;
      this.htmlEmail.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmail.Location = new Point(1, 26);
      this.htmlEmail.Name = "htmlEmail";
      this.htmlEmail.Size = new Size(565, 124);
      this.htmlEmail.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcComments);
      this.Controls.Add((Control) this.gcDocuments);
      this.Controls.Add((Control) this.gcHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EDMLogControl);
      this.Size = new Size(567, 468);
      this.gcHeader.ResumeLayout(false);
      this.gcHeader.PerformLayout();
      this.gcDocuments.ResumeLayout(false);
      this.gcComments.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
