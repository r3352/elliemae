// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.StatusOnlineLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class StatusOnlineLogWS : UserControl, IOnlineHelpTarget
  {
    private const string className = "StatusOnlineLogWS";
    private Label label2;
    private Label label3;
    private DateTimePicker datePciker;
    private TextBox commentsBox;
    private TextBox descBox;
    private System.ComponentModel.Container components;
    private Panel panel1;
    private StatusOnlineLog log;
    private TextBox creatorBox;
    private Label label1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer3;
    private GridView gvStatusList;
    private StandardIconButton deleteBtn;
    private GroupContainer groupContainer2;
    private static Font bFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);

    public StatusOnlineLogWS(StatusOnlineLog log)
    {
      this.log = log;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.descBox.Text = this.log.Description;
      this.creatorBox.Text = this.log.Creator;
      this.commentsBox.Text = this.log.Comments;
      this.datePciker.Value = this.log.Date.Date;
      GVItem[] items = new GVItem[this.log.ItemList.Count];
      for (int index = 0; index < this.log.ItemList.Count; ++index)
        items[index] = new GVItem((string[]) this.log.ItemList[index]);
      this.gvStatusList.Items.AddRange(items);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.label2 = new Label();
      this.label3 = new Label();
      this.datePciker = new DateTimePicker();
      this.commentsBox = new TextBox();
      this.descBox = new TextBox();
      this.panel1 = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.gvStatusList = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.groupContainer1 = new GroupContainer();
      this.deleteBtn = new StandardIconButton();
      this.creatorBox = new TextBox();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.deleteBtn).BeginInit();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(61, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(29, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Date";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.datePciker.Enabled = false;
      this.datePciker.Format = DateTimePickerFormat.Short;
      this.datePciker.Location = new Point(72, 57);
      this.datePciker.Name = "datePciker";
      this.datePciker.Size = new Size(96, 20);
      this.datePciker.TabIndex = 3;
      this.commentsBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.commentsBox.Location = new Point(10, 36);
      this.commentsBox.Multiline = true;
      this.commentsBox.Name = "commentsBox";
      this.commentsBox.ReadOnly = true;
      this.commentsBox.ScrollBars = ScrollBars.Both;
      this.commentsBox.Size = new Size(612, 104);
      this.commentsBox.TabIndex = 5;
      this.descBox.Location = new Point(72, 35);
      this.descBox.Name = "descBox";
      this.descBox.ReadOnly = true;
      this.descBox.Size = new Size(164, 20);
      this.descBox.TabIndex = 2;
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.groupContainer3);
      this.panel1.Controls.Add((Control) this.groupContainer2);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(635, 587);
      this.panel1.TabIndex = 10;
      this.groupContainer3.Controls.Add((Control) this.gvStatusList);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.Location = new Point(0, 260);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(635, 327);
      this.groupContainer3.TabIndex = 17;
      this.groupContainer3.Text = "Status Online Published";
      this.gvStatusList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Status Description";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Date";
      gvColumn2.Width = 100;
      this.gvStatusList.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStatusList.Dock = DockStyle.Fill;
      this.gvStatusList.Location = new Point(1, 26);
      this.gvStatusList.Name = "gvStatusList";
      this.gvStatusList.Size = new Size(633, 300);
      this.gvStatusList.TabIndex = 7;
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.commentsBox);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.Location = new Point(0, 108);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(635, 152);
      this.groupContainer2.TabIndex = 16;
      this.groupContainer2.Text = "Details";
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.deleteBtn);
      this.groupContainer1.Controls.Add((Control) this.descBox);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.creatorBox);
      this.groupContainer1.Controls.Add((Control) this.datePciker);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(635, 108);
      this.groupContainer1.TabIndex = 15;
      this.groupContainer1.Text = "Status Online Updated";
      this.groupContainer1.Paint += new PaintEventHandler(this.groupContainer1_Paint);
      this.deleteBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deleteBtn.BackColor = Color.Transparent;
      this.deleteBtn.Location = new Point(606, 5);
      this.deleteBtn.Name = "deleteBtn";
      this.deleteBtn.Size = new Size(16, 16);
      this.deleteBtn.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.deleteBtn.TabIndex = 9;
      this.deleteBtn.TabStop = false;
      this.deleteBtn.Click += new EventHandler(this.deleteBtn_Click);
      this.creatorBox.Location = new Point(72, 80);
      this.creatorBox.Name = "creatorBox";
      this.creatorBox.ReadOnly = true;
      this.creatorBox.Size = new Size(164, 20);
      this.creatorBox.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 83);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 14);
      this.label1.TabIndex = 7;
      this.label1.Text = "Creator";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (StatusOnlineLogWS);
      this.Size = new Size(635, 587);
      this.panel1.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.deleteBtn).EndInit();
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
      graphics.DrawString(s, StatusOnlineLogWS.bFont, black, new PointF(x, 4f));
    }

    public string GetHelpTargetName() => nameof (StatusOnlineLogWS);

    private void closeBtn_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }

    private void groupContainer1_Paint(object sender, PaintEventArgs e)
    {
    }
  }
}
