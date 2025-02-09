// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.SystemLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class SystemLogWS : UserControl, IOnlineHelpTarget
  {
    private const string className = "SystemLogWS";
    private Panel topPanel;
    private Label label2;
    private Label label3;
    private DateTimePicker datePciker;
    private Label label4;
    private TextBox commentsBox;
    private TextBox descBox;
    private System.ComponentModel.Container components;
    private Button deleteBtn;
    private Panel panel1;
    private Button closeBtn;
    private SystemLog log;
    private static Font bFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);

    public SystemLogWS(SystemLog log)
    {
      this.log = log;
      this.InitializeComponent();
      this.topPanel.BackColor = AppColors.SandySky;
      this.deleteBtn.BackColor = AppColors.BeachSurf;
      this.closeBtn.BackColor = AppColors.BeachSurf;
      this.Dock = DockStyle.Fill;
      this.descBox.Text = this.log.Description;
      this.commentsBox.Text = this.log.Comments;
      this.datePciker.Value = this.log.Date.Date;
      this.deleteBtn.Click += new EventHandler(this.deleteBtn_Click);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.topPanel = new Panel();
      this.closeBtn = new Button();
      this.deleteBtn = new Button();
      this.label2 = new Label();
      this.label3 = new Label();
      this.datePciker = new DateTimePicker();
      this.label4 = new Label();
      this.commentsBox = new TextBox();
      this.descBox = new TextBox();
      this.panel1 = new Panel();
      this.topPanel.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.topPanel.BackColor = Color.Tomato;
      this.topPanel.Controls.Add((Control) this.closeBtn);
      this.topPanel.Controls.Add((Control) this.deleteBtn);
      this.topPanel.Dock = DockStyle.Top;
      this.topPanel.ForeColor = Color.White;
      this.topPanel.Location = new Point(0, 0);
      this.topPanel.Name = "topPanel";
      this.topPanel.Size = new Size(500, 22);
      this.topPanel.TabIndex = 0;
      this.topPanel.Paint += new PaintEventHandler(this.topPanel_Paint);
      this.closeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.closeBtn.BackColor = Color.DarkSalmon;
      this.closeBtn.ForeColor = Color.Black;
      this.closeBtn.Location = new Point(384, 1);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(56, 20);
      this.closeBtn.TabIndex = 2;
      this.closeBtn.TabStop = false;
      this.closeBtn.Text = "&Close";
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.deleteBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deleteBtn.BackColor = Color.DarkSalmon;
      this.deleteBtn.ForeColor = Color.Black;
      this.deleteBtn.Location = new Point(440, 1);
      this.deleteBtn.Name = "deleteBtn";
      this.deleteBtn.Size = new Size(56, 20);
      this.deleteBtn.TabIndex = 1;
      this.deleteBtn.TabStop = false;
      this.deleteBtn.Text = "&Delete";
      this.label2.Location = new Point(24, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 23);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.Location = new Point(24, 37);
      this.label3.Name = "label3";
      this.label3.Size = new Size(64, 23);
      this.label3.TabIndex = 2;
      this.label3.Text = "Date:";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.datePciker.CustomFormat = "MM/dd/yyyy";
      this.datePciker.Enabled = false;
      this.datePciker.Format = DateTimePickerFormat.Custom;
      this.datePciker.Location = new Point(88, 37);
      this.datePciker.Name = "datePciker";
      this.datePciker.Size = new Size(96, 20);
      this.datePciker.TabIndex = 3;
      this.label4.Location = new Point(24, 68);
      this.label4.Name = "label4";
      this.label4.Size = new Size(68, 23);
      this.label4.TabIndex = 4;
      this.label4.Text = "Details";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.commentsBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.commentsBox.Location = new Point(25, 92);
      this.commentsBox.Multiline = true;
      this.commentsBox.Name = "commentsBox";
      this.commentsBox.ReadOnly = true;
      this.commentsBox.ScrollBars = ScrollBars.Both;
      this.commentsBox.Size = new Size(443, 288);
      this.commentsBox.TabIndex = 5;
      this.commentsBox.Text = "";
      this.descBox.Location = new Point(88, 12);
      this.descBox.Name = "descBox";
      this.descBox.ReadOnly = true;
      this.descBox.Size = new Size(164, 20);
      this.descBox.TabIndex = 2;
      this.descBox.Text = "";
      this.panel1.AutoScroll = true;
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.descBox);
      this.panel1.Controls.Add((Control) this.commentsBox);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.datePciker);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 22);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(500, 418);
      this.panel1.TabIndex = 10;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.topPanel);
      this.Name = nameof (SystemLogWS);
      this.Size = new Size(500, 440);
      this.topPanel.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the entry?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        return;
      Session.LoanData.Dirty = true;
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
      Session.LoanData.GetLogList().RemoveRecord((LogRecordBase) this.log);
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    private void topPanel_Paint(object sender, PaintEventArgs e)
    {
      Graphics graphics = e.Graphics;
      float x = 12f;
      string str1 = this.log.Description + " done";
      int days = (DateTime.Today - this.log.Date.Date).Days;
      string str2;
      if (this.log.Date.Date == DateTime.Today)
      {
        str2 = " today";
      }
      else
      {
        DateTime date = this.log.Date;
        date = date.Date;
        str2 = " on " + date.ToString("MM/dd/yy");
      }
      string s = str1 + str2;
      Brush black = Brushes.Black;
      graphics.DrawString(s, SystemLogWS.bFont, black, new PointF(x, 4f));
    }

    public string GetHelpTargetName() => nameof (SystemLogWS);

    private void closeBtn_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }
  }
}
