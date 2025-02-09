// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.PrintIndexLogWS
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
  public class PrintIndexLogWS : UserControl
  {
    private GetIndexLog getindexlog;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label lblRequest;
    private Label lblDTTR;
    private Label lblRequestCaption;
    private Label lblDTTRCaption;
    private Label lblIVCaption;
    private Label lblIV;
    private Label lblITCaption;
    private Label lblIT;

    public PrintIndexLogWS(GetIndexLog getindexLog)
    {
      this.getindexlog = getindexLog;
      this.InitializeComponent();
      this.groupContainer1.Text = "Index requested by " + this.getindexlog.UserName;
      this.lblDTTR.Text = this.getindexlog.Date.ToString("MM/dd/yyyy hh:mm:ss");
      this.lblRequest.Text = this.getindexlog.UserName + " (" + this.getindexlog.UserId + ")";
      this.lblIT.Text = this.getindexlog.IndexType;
      this.lblIV.Text = this.getindexlog.IndexValue;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.lblIVCaption = new Label();
      this.lblIV = new Label();
      this.lblITCaption = new Label();
      this.lblIT = new Label();
      this.lblRequest = new Label();
      this.lblDTTR = new Label();
      this.lblRequestCaption = new Label();
      this.lblDTTRCaption = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.lblIVCaption);
      this.groupContainer1.Controls.Add((Control) this.lblIV);
      this.groupContainer1.Controls.Add((Control) this.lblITCaption);
      this.groupContainer1.Controls.Add((Control) this.lblIT);
      this.groupContainer1.Controls.Add((Control) this.lblRequest);
      this.groupContainer1.Controls.Add((Control) this.lblDTTR);
      this.groupContainer1.Controls.Add((Control) this.lblRequestCaption);
      this.groupContainer1.Controls.Add((Control) this.lblDTTRCaption);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Font = new Font("Arial", 8.25f);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(501, 503);
      this.groupContainer1.TabIndex = 1;
      this.lblIVCaption.AutoSize = true;
      this.lblIVCaption.Font = new Font("Arial", 8.25f);
      this.lblIVCaption.Location = new Point(4, 92);
      this.lblIVCaption.Name = "lblIVCaption";
      this.lblIVCaption.Size = new Size(63, 14);
      this.lblIVCaption.TabIndex = 11;
      this.lblIVCaption.Text = "Index Value";
      this.lblIV.AutoSize = true;
      this.lblIV.Font = new Font("Arial", 8.25f);
      this.lblIV.Location = new Point(143, 92);
      this.lblIV.Name = "lblIV";
      this.lblIV.Size = new Size(63, 14);
      this.lblIV.TabIndex = 10;
      this.lblIV.Text = "Index Value";
      this.lblITCaption.AutoSize = true;
      this.lblITCaption.Font = new Font("Arial", 8.25f);
      this.lblITCaption.Location = new Point(4, 73);
      this.lblITCaption.Name = "lblITCaption";
      this.lblITCaption.Size = new Size(62, 14);
      this.lblITCaption.TabIndex = 9;
      this.lblITCaption.Text = "Index Type:";
      this.lblIT.AutoSize = true;
      this.lblIT.Font = new Font("Arial", 8.25f);
      this.lblIT.Location = new Point(143, 73);
      this.lblIT.Name = "lblIT";
      this.lblIT.Size = new Size(59, 14);
      this.lblIT.TabIndex = 8;
      this.lblIT.Text = "Index Type";
      this.lblRequest.AutoSize = true;
      this.lblRequest.Font = new Font("Arial", 8.25f);
      this.lblRequest.Location = new Point(143, 54);
      this.lblRequest.Name = "lblRequest";
      this.lblRequest.Size = new Size(30, 14);
      this.lblRequest.TabIndex = 7;
      this.lblRequest.Text = "User";
      this.lblDTTR.AutoSize = true;
      this.lblDTTR.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDTTR.Location = new Point(143, 35);
      this.lblDTTR.Name = "lblDTTR";
      this.lblDTTR.Size = new Size(33, 14);
      this.lblDTTR.TabIndex = 6;
      this.lblDTTR.Text = "DTTR";
      this.lblRequestCaption.AutoSize = true;
      this.lblRequestCaption.Font = new Font("Arial", 8.25f);
      this.lblRequestCaption.Location = new Point(4, 54);
      this.lblRequestCaption.Name = "lblRequestCaption";
      this.lblRequestCaption.Size = new Size(77, 14);
      this.lblRequestCaption.TabIndex = 5;
      this.lblRequestCaption.Text = "Requested by:";
      this.lblDTTRCaption.AutoSize = true;
      this.lblDTTRCaption.Font = new Font("Arial", 8.25f);
      this.lblDTTRCaption.Location = new Point(4, 35);
      this.lblDTTRCaption.Name = "lblDTTRCaption";
      this.lblDTTRCaption.Size = new Size(133, 14);
      this.lblDTTRCaption.TabIndex = 4;
      this.lblDTTRCaption.Text = "Date and Time Requested:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (PrintIndexLogWS);
      this.Size = new Size(501, 503);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
