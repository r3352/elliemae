// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DataTracLogControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DataTracLogControl : UserControl
  {
    private DataTracLog log;
    private IContainer components;
    private GroupContainer gcComments;
    private TextBox txtComments;
    private GroupContainer gcHeader;
    private TextBox txtCreatedBy;
    private Label lblDate;
    private Label lblCreatedBy;
    private TextBox txtDate;
    private Label label1;
    private TextBox txtFileID;

    public DataTracLogControl(DataTracLog log)
    {
      this.InitializeComponent();
      this.log = log;
      this.loadDetails();
      this.Dock = DockStyle.Fill;
    }

    private void loadDetails()
    {
      this.txtCreatedBy.Text = this.log.CreatedBy;
      this.txtDate.Text = this.log.Date.ToString("MM/dd/yy hh:mm tt");
      this.txtComments.Text = this.log.Message;
      this.txtFileID.Text = this.log.FileID;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcComments = new GroupContainer();
      this.txtComments = new TextBox();
      this.gcHeader = new GroupContainer();
      this.txtFileID = new TextBox();
      this.label1 = new Label();
      this.txtCreatedBy = new TextBox();
      this.lblDate = new Label();
      this.lblCreatedBy = new Label();
      this.txtDate = new TextBox();
      this.gcComments.SuspendLayout();
      this.gcHeader.SuspendLayout();
      this.SuspendLayout();
      this.gcComments.Controls.Add((Control) this.txtComments);
      this.gcComments.Dock = DockStyle.Fill;
      this.gcComments.HeaderForeColor = SystemColors.ControlText;
      this.gcComments.Location = new Point(0, 107);
      this.gcComments.Name = "gcComments";
      this.gcComments.Size = new Size(557, 411);
      this.gcComments.TabIndex = 9;
      this.gcComments.Text = "Messages";
      this.txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComments.BackColor = SystemColors.Window;
      this.txtComments.Location = new Point(0, 25);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ReadOnly = true;
      this.txtComments.ScrollBars = ScrollBars.Vertical;
      this.txtComments.Size = new Size(557, 386);
      this.txtComments.TabIndex = 8;
      this.txtComments.TabStop = false;
      this.gcHeader.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcHeader.Controls.Add((Control) this.txtFileID);
      this.gcHeader.Controls.Add((Control) this.label1);
      this.gcHeader.Controls.Add((Control) this.txtCreatedBy);
      this.gcHeader.Controls.Add((Control) this.lblDate);
      this.gcHeader.Controls.Add((Control) this.lblCreatedBy);
      this.gcHeader.Controls.Add((Control) this.txtDate);
      this.gcHeader.Dock = DockStyle.Top;
      this.gcHeader.HeaderForeColor = SystemColors.ControlText;
      this.gcHeader.Location = new Point(0, 0);
      this.gcHeader.Name = "gcHeader";
      this.gcHeader.Size = new Size(557, 107);
      this.gcHeader.TabIndex = 8;
      this.gcHeader.Text = "Submitted to DataTrac";
      this.txtFileID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFileID.BackColor = Color.WhiteSmoke;
      this.txtFileID.BorderStyle = BorderStyle.None;
      this.txtFileID.Location = new Point(142, 77);
      this.txtFileID.Name = "txtFileID";
      this.txtFileID.ReadOnly = true;
      this.txtFileID.Size = new Size(407, 13);
      this.txtFileID.TabIndex = 6;
      this.txtFileID.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 77);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 14);
      this.label1.TabIndex = 5;
      this.label1.Text = "Loan Number in DataTrac:";
      this.txtCreatedBy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCreatedBy.BackColor = Color.WhiteSmoke;
      this.txtCreatedBy.BorderStyle = BorderStyle.None;
      this.txtCreatedBy.Location = new Point(142, 55);
      this.txtCreatedBy.Name = "txtCreatedBy";
      this.txtCreatedBy.ReadOnly = true;
      this.txtCreatedBy.Size = new Size(407, 13);
      this.txtCreatedBy.TabIndex = 4;
      this.txtCreatedBy.TabStop = false;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(8, 55);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(73, 14);
      this.lblDate.TabIndex = 3;
      this.lblDate.Text = "Submitted By:";
      this.lblCreatedBy.AutoSize = true;
      this.lblCreatedBy.Location = new Point(8, 34);
      this.lblCreatedBy.Name = "lblCreatedBy";
      this.lblCreatedBy.Size = new Size(128, 14);
      this.lblCreatedBy.TabIndex = 1;
      this.lblCreatedBy.Text = "Date and Time Submitted:";
      this.txtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDate.BackColor = Color.WhiteSmoke;
      this.txtDate.BorderStyle = BorderStyle.None;
      this.txtDate.Location = new Point(142, 34);
      this.txtDate.Name = "txtDate";
      this.txtDate.ReadOnly = true;
      this.txtDate.Size = new Size(407, 13);
      this.txtDate.TabIndex = 2;
      this.txtDate.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.gcComments);
      this.Controls.Add((Control) this.gcHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DataTracLogControl);
      this.Size = new Size(557, 518);
      this.gcComments.ResumeLayout(false);
      this.gcComments.PerformLayout();
      this.gcHeader.ResumeLayout(false);
      this.gcHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
