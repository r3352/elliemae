// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.HtmlEmailLogWS
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
  public class HtmlEmailLogWS : UserControl
  {
    private HtmlEmailLog emailLog;
    private IContainer components;
    private GroupContainer logContainer;
    private GroupContainer emailDetailsContainer;
    private Label label1;
    private TextBox txtCreator;
    private Label lblDate;
    private TextBox txtDescription;
    private Label lblDescription;
    private TextBox txtFrom;
    private Label lblFrom;
    private TextBox txtTo;
    private Label lblTo;
    private HtmlEmailControl htmlEmail;
    private TextBox txtDate;
    private GroupContainer documentsContainer;
    private GridView gvDocuments;
    private CheckBox chkReadReceipt;
    private TextBox txtSubject;
    private Label lblSubject;

    public HtmlEmailLogWS(HtmlEmailLog emailLog)
    {
      this.emailLog = emailLog;
      this.InitializeComponent();
      this.loadDetails();
      this.Dock = DockStyle.Fill;
    }

    private void loadDetails()
    {
      this.logContainer.Text = this.emailLog.Description;
      this.txtDescription.Text = this.emailLog.Description;
      this.txtCreator.Text = this.emailLog.CreatedBy;
      this.txtDate.Text = this.emailLog.Date.ToString("MM/dd/yy hh:mm tt");
      this.txtFrom.Text = this.emailLog.Sender;
      this.txtTo.Text = this.emailLog.Recipient;
      this.txtSubject.Text = this.emailLog.Subject;
      this.chkReadReceipt.Checked = this.emailLog.ReadReceipt;
      this.htmlEmail.ShowToolbar = false;
      if (!string.IsNullOrEmpty(this.emailLog.Body))
        this.htmlEmail.LoadHtml(this.emailLog.Body, true);
      else
        this.htmlEmail.LoadText(string.Empty, true);
      if (this.emailLog.Documents == null)
        this.hideDocumentsContainer();
      else if (this.emailLog.Documents.Length == 0)
      {
        this.hideDocumentsContainer();
      }
      else
      {
        foreach (string document in this.emailLog.Documents)
          this.gvDocuments.Items.Add(document);
      }
    }

    private void hideDocumentsContainer()
    {
      this.documentsContainer.Visible = false;
      this.chkReadReceipt.Visible = false;
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
      this.logContainer = new GroupContainer();
      this.chkReadReceipt = new CheckBox();
      this.txtSubject = new TextBox();
      this.lblSubject = new Label();
      this.txtDate = new TextBox();
      this.label1 = new Label();
      this.txtCreator = new TextBox();
      this.lblDate = new Label();
      this.txtTo = new TextBox();
      this.lblTo = new Label();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.txtFrom = new TextBox();
      this.lblFrom = new Label();
      this.emailDetailsContainer = new GroupContainer();
      this.htmlEmail = new HtmlEmailControl();
      this.documentsContainer = new GroupContainer();
      this.gvDocuments = new GridView();
      this.logContainer.SuspendLayout();
      this.emailDetailsContainer.SuspendLayout();
      this.documentsContainer.SuspendLayout();
      this.SuspendLayout();
      this.logContainer.Controls.Add((Control) this.chkReadReceipt);
      this.logContainer.Controls.Add((Control) this.txtSubject);
      this.logContainer.Controls.Add((Control) this.lblSubject);
      this.logContainer.Controls.Add((Control) this.txtDate);
      this.logContainer.Controls.Add((Control) this.label1);
      this.logContainer.Controls.Add((Control) this.txtCreator);
      this.logContainer.Controls.Add((Control) this.lblDate);
      this.logContainer.Controls.Add((Control) this.txtTo);
      this.logContainer.Controls.Add((Control) this.lblTo);
      this.logContainer.Controls.Add((Control) this.txtDescription);
      this.logContainer.Controls.Add((Control) this.lblDescription);
      this.logContainer.Controls.Add((Control) this.txtFrom);
      this.logContainer.Controls.Add((Control) this.lblFrom);
      this.logContainer.Dock = DockStyle.Top;
      this.logContainer.HeaderForeColor = SystemColors.ControlText;
      this.logContainer.Location = new Point(0, 0);
      this.logContainer.Name = "logContainer";
      this.logContainer.Size = new Size(607, 244);
      this.logContainer.TabIndex = 0;
      this.chkReadReceipt.AutoSize = true;
      this.chkReadReceipt.Enabled = false;
      this.chkReadReceipt.Location = new Point(8, 216);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(92, 17);
      this.chkReadReceipt.TabIndex = 10;
      this.chkReadReceipt.Text = "Read Receipt";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.txtSubject.BackColor = Color.WhiteSmoke;
      this.txtSubject.Location = new Point(76, 185);
      this.txtSubject.Name = "txtSubject";
      this.txtSubject.ReadOnly = true;
      this.txtSubject.Size = new Size(488, 20);
      this.txtSubject.TabIndex = 9;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(8, 188);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(46, 13);
      this.lblSubject.TabIndex = 8;
      this.lblSubject.Text = "Subject:";
      this.txtDate.BackColor = Color.WhiteSmoke;
      this.txtDate.Location = new Point(76, 68);
      this.txtDate.Name = "txtDate";
      this.txtDate.ReadOnly = true;
      this.txtDate.Size = new Size(264, 20);
      this.txtDate.TabIndex = 7;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 100);
      this.label1.Name = "label1";
      this.label1.Size = new Size(44, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Creator:";
      this.txtCreator.BackColor = Color.WhiteSmoke;
      this.txtCreator.Location = new Point(76, 96);
      this.txtCreator.Name = "txtCreator";
      this.txtCreator.ReadOnly = true;
      this.txtCreator.Size = new Size(264, 20);
      this.txtCreator.TabIndex = 4;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(8, 72);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(33, 13);
      this.lblDate.TabIndex = 2;
      this.lblDate.Text = "Date:";
      this.txtTo.BackColor = Color.WhiteSmoke;
      this.txtTo.Location = new Point(76, 155);
      this.txtTo.Name = "txtTo";
      this.txtTo.ReadOnly = true;
      this.txtTo.Size = new Size(488, 20);
      this.txtTo.TabIndex = 3;
      this.lblTo.AutoSize = true;
      this.lblTo.Location = new Point(8, 159);
      this.lblTo.Name = "lblTo";
      this.lblTo.Size = new Size(23, 13);
      this.lblTo.TabIndex = 2;
      this.lblTo.Text = "To:";
      this.txtDescription.BackColor = Color.WhiteSmoke;
      this.txtDescription.Location = new Point(76, 40);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(264, 20);
      this.txtDescription.TabIndex = 1;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(8, 44);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(63, 13);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Description:";
      this.txtFrom.BackColor = Color.WhiteSmoke;
      this.txtFrom.Location = new Point(77, 124);
      this.txtFrom.Name = "txtFrom";
      this.txtFrom.ReadOnly = true;
      this.txtFrom.Size = new Size(488, 20);
      this.txtFrom.TabIndex = 1;
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new Point(8, 128);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new Size(33, 13);
      this.lblFrom.TabIndex = 0;
      this.lblFrom.Text = "From:";
      this.emailDetailsContainer.Controls.Add((Control) this.htmlEmail);
      this.emailDetailsContainer.Dock = DockStyle.Fill;
      this.emailDetailsContainer.HeaderForeColor = SystemColors.ControlText;
      this.emailDetailsContainer.Location = new Point(0, 244);
      this.emailDetailsContainer.Name = "emailDetailsContainer";
      this.emailDetailsContainer.Size = new Size(607, 220);
      this.emailDetailsContainer.TabIndex = 1;
      this.emailDetailsContainer.Text = "Email Details";
      this.htmlEmail.Dock = DockStyle.Fill;
      this.htmlEmail.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmail.Location = new Point(1, 26);
      this.htmlEmail.Name = "htmlEmail";
      this.htmlEmail.Size = new Size(605, 193);
      this.htmlEmail.TabIndex = 6;
      this.documentsContainer.Controls.Add((Control) this.gvDocuments);
      this.documentsContainer.Dock = DockStyle.Bottom;
      this.documentsContainer.HeaderForeColor = SystemColors.ControlText;
      this.documentsContainer.Location = new Point(0, 464);
      this.documentsContainer.Name = "documentsContainer";
      this.documentsContainer.Size = new Size(607, 139);
      this.documentsContainer.TabIndex = 2;
      this.documentsContainer.Text = "Documents";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colTitle";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Title";
      gvColumn.Width = 605;
      this.gvDocuments.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HeaderHeight = 0;
      this.gvDocuments.HeaderVisible = false;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(605, 112);
      this.gvDocuments.TabIndex = 0;
      this.gvDocuments.TabStop = false;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.emailDetailsContainer);
      this.Controls.Add((Control) this.documentsContainer);
      this.Controls.Add((Control) this.logContainer);
      this.Name = nameof (HtmlEmailLogWS);
      this.Size = new Size(607, 603);
      this.logContainer.ResumeLayout(false);
      this.logContainer.PerformLayout();
      this.emailDetailsContainer.ResumeLayout(false);
      this.documentsContainer.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
