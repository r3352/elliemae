// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ExportLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ExportLogWS : UserControl
  {
    private ExportLog exportLog;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label lblExporterCaption;
    private Label lblDTTMCaption;
    private Label lblExporter;
    private Label lblDTTM;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private GridView gvForms;
    private TextBox txtComment;
    private Panel panel1;

    public ExportLogWS(ExportLog exportLog)
    {
      this.exportLog = exportLog;
      this.InitializeComponent();
      string str = string.Empty;
      if (exportLog.Category == ExportLog.ExportCategory.GSEServices)
        str = "GSE Services exported";
      else if (exportLog.Category == ExportLog.ExportCategory.ComplianceServices)
        str = "Compliance Services exported";
      this.groupContainer1.Text = str + " by " + this.exportLog.ExportedByFullName + " (" + this.exportLog.ExportedBy + ")";
      this.groupContainer2.Text = "Exports";
      this.groupContainer3.Text = "Comments";
      this.lblDTTMCaption.Text = "Date and Time Exported:";
      this.lblDTTM.Left = this.lblDTTMCaption.Right + 3;
      this.lblExporterCaption.Text = "Exported by:";
      this.lblExporter.Left = this.lblDTTM.Left;
      this.initialPage();
    }

    private void initialPage()
    {
      this.gvForms.Items.Clear();
      this.txtComment.Text = "";
      this.lblDTTM.Text = this.exportLog.Date.ToString("MM/dd/yyyy hh:mm:ss");
      this.lblExporter.Text = this.exportLog.ExportedByFullName + " (" + this.exportLog.ExportedBy + ")";
      for (int index = 0; index < this.exportLog.ItemList.Count; ++index)
        this.gvForms.Items.Add(string.Concat(this.exportLog.ItemList[index]));
      this.txtComment.Text = this.exportLog.Comments;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }

    private void txtComment_Leave(object sender, EventArgs e)
    {
      this.exportLog.Comments = this.txtComment.Text;
      LoanEventLogList loanEventLog = new LoanEventLogList();
      loanEventLog.InsertNonSystemLog((LogRecordBase) this.exportLog);
      Session.LoanDataMgr.AddLoanEventLogs(loanEventLog);
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
      this.groupContainer1 = new GroupContainer();
      this.lblExporter = new Label();
      this.lblDTTM = new Label();
      this.lblExporterCaption = new Label();
      this.lblDTTMCaption = new Label();
      this.groupContainer2 = new GroupContainer();
      this.gvForms = new GridView();
      this.groupContainer3 = new GroupContainer();
      this.txtComment = new TextBox();
      this.panel1 = new Panel();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.lblExporter);
      this.groupContainer1.Controls.Add((Control) this.lblDTTM);
      this.groupContainer1.Controls.Add((Control) this.lblExporterCaption);
      this.groupContainer1.Controls.Add((Control) this.lblDTTMCaption);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.Font = new Font("Arial", 8.25f);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(501, 75);
      this.groupContainer1.TabIndex = 0;
      this.lblExporter.AutoSize = true;
      this.lblExporter.Font = new Font("Arial", 8.25f);
      this.lblExporter.Location = new Point(198, 52);
      this.lblExporter.Name = "lblExporter";
      this.lblExporter.Size = new Size(48, 14);
      this.lblExporter.TabIndex = 3;
      this.lblExporter.Text = "Exporter";
      this.lblDTTM.AutoSize = true;
      this.lblDTTM.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDTTM.Location = new Point(198, 33);
      this.lblDTTM.Name = "lblDTTM";
      this.lblDTTM.Size = new Size(34, 14);
      this.lblDTTM.TabIndex = 2;
      this.lblDTTM.Text = "DTTM";
      this.lblExporterCaption.AutoSize = true;
      this.lblExporterCaption.Font = new Font("Arial", 8.25f);
      this.lblExporterCaption.Location = new Point(8, 52);
      this.lblExporterCaption.Name = "lblExporterCaption";
      this.lblExporterCaption.Size = new Size(68, 14);
      this.lblExporterCaption.TabIndex = 1;
      this.lblExporterCaption.Text = "Exported by:";
      this.lblDTTMCaption.AutoSize = true;
      this.lblDTTMCaption.Font = new Font("Arial", 8.25f);
      this.lblDTTMCaption.Location = new Point(8, 33);
      this.lblDTTMCaption.Name = "lblDTTMCaption";
      this.lblDTTMCaption.Size = new Size(124, 14);
      this.lblDTTMCaption.TabIndex = 0;
      this.lblDTTMCaption.Text = "Date and Time Exported:";
      this.groupContainer2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.gvForms);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.Font = new Font("Arial", 8.25f);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 75);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(501, 242);
      this.groupContainer2.TabIndex = 1;
      this.gvForms.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Type";
      gvColumn.Width = 499;
      this.gvForms.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvForms.Dock = DockStyle.Fill;
      this.gvForms.Font = new Font("Arial", 8.25f);
      this.gvForms.Location = new Point(1, 25);
      this.gvForms.Name = "gvForms";
      this.gvForms.Size = new Size(499, 216);
      this.gvForms.TabIndex = 0;
      this.groupContainer3.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer3.Controls.Add((Control) this.txtComment);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.Font = new Font("Arial", 8.25f);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 317);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(501, 186);
      this.groupContainer3.TabIndex = 2;
      this.txtComment.BackColor = Color.White;
      this.txtComment.BorderStyle = BorderStyle.None;
      this.txtComment.Dock = DockStyle.Fill;
      this.txtComment.Location = new Point(1, 25);
      this.txtComment.Multiline = true;
      this.txtComment.Name = "txtComment";
      this.txtComment.ScrollBars = ScrollBars.Both;
      this.txtComment.Size = new Size(499, 160);
      this.txtComment.TabIndex = 0;
      this.txtComment.Leave += new EventHandler(this.txtComment_Leave);
      this.panel1.Controls.Add((Control) this.groupContainer3);
      this.panel1.Controls.Add((Control) this.groupContainer2);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(501, 503);
      this.panel1.TabIndex = 3;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (ExportLogWS);
      this.Size = new Size(501, 503);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
