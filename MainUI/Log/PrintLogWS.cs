// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.PrintLogWS
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
  public class PrintLogWS : UserControl
  {
    private PrintLog printLog;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label lblPrinterCaption;
    private Label lblDTTMCaption;
    private Label lblPrinter;
    private Label lblDTTM;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private GridView gvForms;
    private TextBox txtComment;
    private Panel panel1;

    public PrintLogWS(PrintLog printLog)
    {
      this.printLog = printLog;
      this.InitializeComponent();
      string str = "Printed";
      if (printLog.Action == PrintLog.PrintAction.Preview)
        str = "Previewed";
      else if (printLog.Action == PrintLog.PrintAction.PrintToFile)
        str = "Printed to File";
      this.groupContainer1.Text = "Forms " + str + " by " + this.printLog.PrintedByFullName + " (" + this.printLog.PrintedBy + ")";
      this.groupContainer2.Text = "Forms";
      this.groupContainer3.Text = "Comments";
      this.lblDTTMCaption.Text = "Date and Time " + str + ":";
      this.lblDTTM.Left = this.lblDTTMCaption.Right + 3;
      this.lblPrinterCaption.Text = str + " by:";
      this.lblPrinter.Left = this.lblDTTM.Left;
      this.initialPage();
    }

    private void initialPage()
    {
      this.gvForms.Items.Clear();
      this.txtComment.Text = "";
      this.lblDTTM.Text = this.printLog.Date.ToString("MM/dd/yyyy hh:mm:ss");
      this.lblPrinter.Text = this.printLog.PrintedByFullName + " (" + this.printLog.PrintedBy + ")";
      for (int index = 0; index < this.printLog.ItemList.Count; ++index)
        this.gvForms.Items.Add(string.Concat(this.printLog.ItemList[index]));
      this.txtComment.Text = this.printLog.Comments;
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
      this.printLog.Comments = this.txtComment.Text;
      LoanEventLogList loanEventLog = new LoanEventLogList();
      loanEventLog.InsertNonSystemLog((LogRecordBase) this.printLog);
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
      this.lblPrinter = new Label();
      this.lblDTTM = new Label();
      this.lblPrinterCaption = new Label();
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
      this.groupContainer1.Controls.Add((Control) this.lblPrinter);
      this.groupContainer1.Controls.Add((Control) this.lblDTTM);
      this.groupContainer1.Controls.Add((Control) this.lblPrinterCaption);
      this.groupContainer1.Controls.Add((Control) this.lblDTTMCaption);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.Font = new Font("Arial", 8.25f);
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(501, 75);
      this.groupContainer1.TabIndex = 0;
      this.lblPrinter.AutoSize = true;
      this.lblPrinter.Font = new Font("Arial", 8.25f);
      this.lblPrinter.Location = new Point(120, 52);
      this.lblPrinter.Name = "lblPrinter";
      this.lblPrinter.Size = new Size(38, 14);
      this.lblPrinter.TabIndex = 3;
      this.lblPrinter.Text = "Printer";
      this.lblDTTM.AutoSize = true;
      this.lblDTTM.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDTTM.Location = new Point(120, 33);
      this.lblDTTM.Name = "lblDTTM";
      this.lblDTTM.Size = new Size(34, 14);
      this.lblDTTM.TabIndex = 2;
      this.lblDTTM.Text = "DTTM";
      this.lblPrinterCaption.AutoSize = true;
      this.lblPrinterCaption.Font = new Font("Arial", 8.25f);
      this.lblPrinterCaption.Location = new Point(8, 52);
      this.lblPrinterCaption.Name = "lblPrinterCaption";
      this.lblPrinterCaption.Size = new Size(58, 14);
      this.lblPrinterCaption.TabIndex = 1;
      this.lblPrinterCaption.Text = "Printed by:";
      this.lblDTTMCaption.AutoSize = true;
      this.lblDTTMCaption.Font = new Font("Arial", 8.25f);
      this.lblDTTMCaption.Location = new Point(8, 33);
      this.lblDTTMCaption.Name = "lblDTTMCaption";
      this.lblDTTMCaption.Size = new Size(114, 14);
      this.lblDTTMCaption.TabIndex = 0;
      this.lblDTTMCaption.Text = "Date and Time Printed:";
      this.groupContainer2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.gvForms);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.Font = new Font("Arial", 8.25f);
      this.groupContainer2.Location = new Point(0, 75);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(501, 242);
      this.groupContainer2.TabIndex = 1;
      this.gvForms.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Form Name";
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
      this.Name = nameof (PrintLogWS);
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
