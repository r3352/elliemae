// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PrequalDetailDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PrequalDetailDialog : Form
  {
    private Button okBtn;
    private System.ComponentModel.Container components;
    private TabControl tabControl1;
    private TextBox statusBox;
    private PictureBox pictureYellow;
    private PictureBox pictureRed;
    private PictureBox pictureGreen;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ListView detailtListview;
    private ListView lvwSummary;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private TabPage pageStatement;
    private TabPage pageDetail;
    private TabPage pageSummary;
    private LoanData loan;
    private Label labelReason;
    private int col;
    private string fieldID = string.Empty;

    public PrequalDetailDialog(int col, LoanData loan, bool forPrequal)
    {
      this.col = col;
      this.loan = loan;
      this.InitializeComponent();
      if (!forPrequal)
        this.Text = "Loan Comparison Results";
      this.pictureRed.Location = this.pictureYellow.Location;
      this.pictureGreen.Location = this.pictureYellow.Location;
      this.refreshStatus(col, forPrequal);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    internal string FieldID => this.fieldID;

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PrequalDetailDialog));
      this.okBtn = new Button();
      this.tabControl1 = new TabControl();
      this.pageStatement = new TabPage();
      this.pictureGreen = new PictureBox();
      this.pictureRed = new PictureBox();
      this.pictureYellow = new PictureBox();
      this.statusBox = new TextBox();
      this.pageDetail = new TabPage();
      this.detailtListview = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.pageSummary = new TabPage();
      this.lvwSummary = new ListView();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.labelReason = new Label();
      this.tabControl1.SuspendLayout();
      this.pageStatement.SuspendLayout();
      ((ISupportInitialize) this.pictureGreen).BeginInit();
      ((ISupportInitialize) this.pictureRed).BeginInit();
      ((ISupportInitialize) this.pictureYellow).BeginInit();
      this.pageDetail.SuspendLayout();
      this.pageSummary.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(398, 278);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 10;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.pageStatement);
      this.tabControl1.Controls.Add((Control) this.pageDetail);
      this.tabControl1.Controls.Add((Control) this.pageSummary);
      this.tabControl1.Location = new Point(5, 8);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(471, 260);
      this.tabControl1.TabIndex = 11;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.pageStatement.Controls.Add((Control) this.pictureGreen);
      this.pageStatement.Controls.Add((Control) this.pictureRed);
      this.pageStatement.Controls.Add((Control) this.pictureYellow);
      this.pageStatement.Controls.Add((Control) this.statusBox);
      this.pageStatement.Location = new Point(4, 22);
      this.pageStatement.Name = "pageStatement";
      this.pageStatement.Size = new Size(463, 234);
      this.pageStatement.TabIndex = 0;
      this.pageStatement.Text = "Result";
      this.pictureGreen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureGreen.Image = (Image) componentResourceManager.GetObject("pictureGreen.Image");
      this.pictureGreen.Location = new Point(417, 112);
      this.pictureGreen.Name = "pictureGreen";
      this.pictureGreen.Size = new Size(34, 45);
      this.pictureGreen.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureGreen.TabIndex = 3;
      this.pictureGreen.TabStop = false;
      this.pictureRed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureRed.Image = (Image) componentResourceManager.GetObject("pictureRed.Image");
      this.pictureRed.Location = new Point(417, 60);
      this.pictureRed.Name = "pictureRed";
      this.pictureRed.Size = new Size(34, 45);
      this.pictureRed.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureRed.TabIndex = 2;
      this.pictureRed.TabStop = false;
      this.pictureYellow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureYellow.Image = (Image) componentResourceManager.GetObject("pictureYellow.Image");
      this.pictureYellow.Location = new Point(418, 8);
      this.pictureYellow.Name = "pictureYellow";
      this.pictureYellow.Size = new Size(34, 45);
      this.pictureYellow.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureYellow.TabIndex = 1;
      this.pictureYellow.TabStop = false;
      this.statusBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.statusBox.Location = new Point(5, 8);
      this.statusBox.Multiline = true;
      this.statusBox.Name = "statusBox";
      this.statusBox.ReadOnly = true;
      this.statusBox.ScrollBars = ScrollBars.Both;
      this.statusBox.Size = new Size(404, 218);
      this.statusBox.TabIndex = 0;
      this.pageDetail.Controls.Add((Control) this.detailtListview);
      this.pageDetail.Location = new Point(4, 22);
      this.pageDetail.Name = "pageDetail";
      this.pageDetail.Size = new Size(463, 234);
      this.pageDetail.TabIndex = 1;
      this.pageDetail.Text = "Reason";
      this.detailtListview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.detailtListview.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.detailtListview.FullRowSelect = true;
      this.detailtListview.GridLines = true;
      this.detailtListview.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.detailtListview.Location = new Point(5, 8);
      this.detailtListview.MultiSelect = false;
      this.detailtListview.Name = "detailtListview";
      this.detailtListview.Size = new Size(451, 220);
      this.detailtListview.TabIndex = 37;
      this.detailtListview.UseCompatibleStateImageBehavior = false;
      this.detailtListview.View = View.Details;
      this.detailtListview.DoubleClick += new EventHandler(this.detailtListview_DoubleClick);
      this.columnHeader1.Text = "";
      this.columnHeader1.Width = 28;
      this.columnHeader2.Text = "Reason";
      this.columnHeader2.Width = 314;
      this.columnHeader3.Text = "Field ID";
      this.columnHeader3.Width = 92;
      this.pageSummary.Controls.Add((Control) this.lvwSummary);
      this.pageSummary.Location = new Point(4, 22);
      this.pageSummary.Name = "pageSummary";
      this.pageSummary.Size = new Size(463, 234);
      this.pageSummary.TabIndex = 2;
      this.pageSummary.Text = "Summary";
      this.lvwSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwSummary.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6,
        this.columnHeader7
      });
      this.lvwSummary.FullRowSelect = true;
      this.lvwSummary.GridLines = true;
      this.lvwSummary.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvwSummary.Location = new Point(5, 8);
      this.lvwSummary.MultiSelect = false;
      this.lvwSummary.Name = "lvwSummary";
      this.lvwSummary.Size = new Size(451, 220);
      this.lvwSummary.TabIndex = 38;
      this.lvwSummary.UseCompatibleStateImageBehavior = false;
      this.lvwSummary.View = View.Details;
      this.columnHeader4.Text = "Check Items";
      this.columnHeader4.Width = 130;
      this.columnHeader5.Text = "Actual";
      this.columnHeader5.TextAlign = HorizontalAlignment.Right;
      this.columnHeader5.Width = 110;
      this.columnHeader6.Text = "Requirement";
      this.columnHeader6.TextAlign = HorizontalAlignment.Right;
      this.columnHeader6.Width = 110;
      this.columnHeader7.Text = "Within Limits";
      this.columnHeader7.Width = 80;
      this.labelReason.Location = new Point(8, 280);
      this.labelReason.Name = "labelReason";
      this.labelReason.Size = new Size(244, 16);
      this.labelReason.TabIndex = 12;
      this.labelReason.Text = "Double-click a Field ID to locate that field.";
      this.labelReason.Visible = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(486, 319);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.labelReason);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (PrequalDetailDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Prequalification Results";
      this.tabControl1.ResumeLayout(false);
      this.pageStatement.ResumeLayout(false);
      this.pageStatement.PerformLayout();
      ((ISupportInitialize) this.pictureGreen).EndInit();
      ((ISupportInitialize) this.pictureRed).EndInit();
      ((ISupportInitialize) this.pictureYellow).EndInit();
      this.pageDetail.ResumeLayout(false);
      this.pageSummary.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void refreshStatus(int col, bool forPrequal)
    {
      string simpleField = this.loan.GetSimpleField("PREQUAL.X274");
      switch (col)
      {
        case 1:
          simpleField = this.loan.GetSimpleField("PREQUAL.X303");
          break;
        case 2:
          simpleField = this.loan.GetSimpleField("PREQUAL.X304");
          break;
      }
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = this.loan.Calculator.GetQualificationDetail(col);
      int length1 = str3.IndexOf("\r\n*** Qualified Check ***\r\n");
      if (length1 > -1)
      {
        str2 = str3.Substring(length1 + 28);
        str3 = str3.Substring(0, length1);
      }
      int length2 = str3.IndexOf("*** Detail ***");
      if (length2 > -1)
      {
        str1 = str3.Substring(length2 + 14);
        str3 = str3.Substring(0, length2);
      }
      string str4 = "";
      if (!forPrequal)
        str4 = "Scenario: " + (col + 1).ToString() + "\r\n\r\n";
      switch (simpleField)
      {
        case "Green":
          this.statusBox.Text = str4 + "Congratulations! You are prequalified.\r\n\r\n" + str3;
          this.pictureRed.Visible = false;
          this.pictureYellow.Visible = false;
          break;
        case "Red":
          this.statusBox.Text = str4 + "Reasons for not qualifying:\r\n\r\n" + str3;
          this.pictureGreen.Visible = false;
          this.pictureYellow.Visible = false;
          break;
        default:
          this.statusBox.Text = str4 + "Not enough information has been entered to qualify. Please check detail.\r\n\r\n" + str3;
          this.pictureRed.Visible = false;
          this.pictureGreen.Visible = false;
          break;
      }
      this.detailtListview.Items.Clear();
      if (str1 != string.Empty)
      {
        string[] strArray = str1.Replace("\r\n", "\r").Split('\r');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!(strArray[index] == string.Empty))
          {
            ListViewItem listViewItem;
            if (strArray[index].StartsWith(">>"))
            {
              if (this.detailtListview.Items.Count > 0)
                this.detailtListview.Items.Add(new ListViewItem(""));
              listViewItem = new ListViewItem(">>");
              listViewItem.SubItems.Add(strArray[index].Substring(2).Trim());
              listViewItem.SubItems.Add("");
            }
            else
            {
              listViewItem = new ListViewItem("");
              int num = strArray[index].IndexOf('\t');
              if (num > -1)
              {
                listViewItem.SubItems.Add(strArray[index].Substring(0, num).Trim());
                string text = strArray[index].Substring(num).Replace("\t", "").Trim();
                listViewItem.SubItems.Add(text);
              }
              else
              {
                listViewItem.SubItems.Add(strArray[index]);
                listViewItem.SubItems.Add("");
              }
            }
            this.detailtListview.Items.Add(listViewItem);
          }
        }
      }
      else
        this.tabControl1.TabPages.Remove(this.pageDetail);
      string[] strArray1 = new string[8]
      {
        "Maximum Loan Amount",
        "Maximum Front Ratio",
        "Maximum Back Ratio",
        "Maximum LTV",
        "Maximum CLTV",
        "Minimum Monthly Income",
        "Maximum Monthly Debt",
        "Cash to Close"
      };
      string[] strArray2 = new string[8]
      {
        "2",
        "740",
        "742",
        "353",
        "976",
        "1389",
        "1742",
        "915"
      };
      string[] strArray3 = new string[8]
      {
        "PREQUAL.X202",
        "1790",
        "1791",
        "PREQUAL.X209",
        "PREQUAL.X210",
        "PREQUAL.X7",
        "PREQUAL.X8",
        "142"
      };
      string[] strArray4 = new string[8]
      {
        "PREQUAL.X308",
        "PREQUAL.X309",
        "PREQUAL.X310",
        "PREQUAL.X311",
        "PREQUAL.X312",
        "PREQUAL.X313",
        "PREQUAL.X314",
        "PREQUAL.X315"
      };
      string[] strArray5 = new string[8]
      {
        "PREQUAL.X299",
        "PREQUAL.X237",
        "PREQUAL.X238",
        "PREQUAL.X262",
        "PREQUAL.X264",
        "PREQUAL.X266",
        "PREQUAL.X258",
        "PREQUAL.X290"
      };
      string[] strArray6 = new string[8]
      {
        "PREQUAL.X260",
        "LP0149",
        "LP0150",
        "LP0146",
        "LP0147",
        "PREQUAL.X56",
        "PREQUAL.X57",
        "PREQUAL.X44"
      };
      string[] strArray7 = new string[8]
      {
        "PREQUAL.X300",
        "PREQUAL.X243",
        "PREQUAL.X244",
        "PREQUAL.X263",
        "PREQUAL.X265",
        "PREQUAL.X267",
        "PREQUAL.X259",
        "PREQUAL.X291"
      };
      string[] strArray8 = new string[8]
      {
        "PREQUAL.X261",
        "LP0249",
        "LP0250",
        "LP0246",
        "LP0247",
        "PREQUAL.X96",
        "PREQUAL.X97",
        "PREQUAL.X84"
      };
      string[] strArray9 = str2.Replace("\r", "").Split('\n');
      string[] strArray10;
      string[] strArray11;
      switch (col)
      {
        case 1:
          strArray10 = strArray5;
          strArray11 = strArray6;
          break;
        case 2:
          strArray10 = strArray7;
          strArray11 = strArray8;
          break;
        default:
          strArray10 = strArray2;
          strArray11 = strArray3;
          break;
      }
      Font font = this.lvwSummary.Font;
      Color black = Color.Black;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        string text1 = col != 0 ? strArray9[index] : this.loan.GetField(strArray4[index]);
        Color foreColor = !(text1.ToLower() == "no") ? Color.Black : Color.Red;
        ListViewItem listViewItem = new ListViewItem(strArray1[index]);
        listViewItem.SubItems.Add(this.loan.GetField(strArray10[index]), foreColor, Color.White, font);
        string text2 = this.loan.GetField(strArray11[index]);
        if (index == strArray1.Length - 1 && Utils.ParseDecimal((object) text2) < 0M && text2 != string.Empty)
          text2 = "0.00";
        listViewItem.SubItems.Add(text2, foreColor, Color.White, font);
        listViewItem.SubItems.Add(text1, foreColor, Color.White, font);
        listViewItem.UseItemStyleForSubItems = false;
        this.lvwSummary.Items.Add(listViewItem);
      }
    }

    private void detailtListview_DoubleClick(object sender, EventArgs e)
    {
      if (this.detailtListview.SelectedItems.Count == 0)
        return;
      this.fieldID = this.detailtListview.SelectedItems[0].SubItems[2].Text.Trim();
      if (this.fieldID == string.Empty)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void okBtn_Click(object sender, EventArgs e) => this.fieldID = string.Empty;

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl1.SelectedTab.Name == "pageDetail")
        this.labelReason.Visible = true;
      else
        this.labelReason.Visible = false;
    }
  }
}
