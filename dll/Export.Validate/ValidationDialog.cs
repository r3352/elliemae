// Decompiled with JetBrains decompiler
// Type: Encompass.Export.ValidationDialog
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class ValidationDialog : Form
  {
    private IBam loanData;
    private Button cancelBtn;
    private Label label1;
    private System.ComponentModel.Container components;
    private ListView missingLvw;
    private ColumnHeader columnHeader1;
    private Button printBtn;
    private Button continueBtn;
    private ColumnHeader columnHeader2;

    internal ValidationDialog(string requireList, bool allowContinue, IBam loanData)
    {
      this.InitializeComponent();
      this.loanData = loanData;
      if (!allowContinue)
      {
        this.label1.Text = "The following fields are missing. Please click the cancel button and return to the application to complete the fields listed below.";
        this.continueBtn.Visible = false;
      }
      this.loadMissingFields(requireList);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.continueBtn = new Button();
      this.label1 = new Label();
      this.missingLvw = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.printBtn = new Button();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(239, 300);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(73, 24);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "Cancel";
      this.continueBtn.DialogResult = DialogResult.OK;
      this.continueBtn.Location = new Point(159, 300);
      this.continueBtn.Name = "continueBtn";
      this.continueBtn.Size = new Size(73, 24);
      this.continueBtn.TabIndex = 3;
      this.continueBtn.Text = "&Continue";
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(516, 28);
      this.label1.TabIndex = 5;
      this.label1.Text = "The following fields are missing. You may continue with this loan, or click the cancel button and return to the application to complete the fields listed below.";
      this.missingLvw.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.missingLvw.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader1,
        this.columnHeader2
      });
      this.missingLvw.FullRowSelect = true;
      this.missingLvw.HideSelection = false;
      this.missingLvw.Location = new Point(8, 44);
      this.missingLvw.MultiSelect = false;
      this.missingLvw.Name = "missingLvw";
      this.missingLvw.Size = new Size(516, 248);
      this.missingLvw.TabIndex = 39;
      this.missingLvw.View = View.Details;
      this.columnHeader1.Text = "Field ID";
      this.columnHeader1.Width = 150;
      this.columnHeader2.Text = "Description";
      this.columnHeader2.Width = 330;
      this.printBtn.Location = new Point(321, 300);
      this.printBtn.Name = "printBtn";
      this.printBtn.Size = new Size(71, 24);
      this.printBtn.TabIndex = 40;
      this.printBtn.Text = "&Print";
      this.printBtn.Click += new EventHandler(this.printBtn_Click);
      this.AcceptButton = (IButtonControl) this.continueBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(534, 335);
      this.Controls.Add((Control) this.printBtn);
      this.Controls.Add((Control) this.missingLvw);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.continueBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ValidationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Incomplete Loan Application";
      this.TopMost = true;
      this.ResumeLayout(false);
    }

    private void loadMissingFields(string reqList)
    {
      string[] strArray1 = reqList.Split('|');
      if (strArray1.Length == 0)
        strArray1 = new string[1]{ reqList };
      this.missingLvw.Items.Clear();
      for (int index = 0; index < strArray1.Length; ++index)
      {
        string[] strArray2 = strArray1[index].Split('@');
        string[] strArray3 = strArray2[1].Split('?');
        this.missingLvw.Items.Add(new ListViewItem(strArray2[0])
        {
          SubItems = {
            strArray3[0]
          }
        });
      }
      if (this.missingLvw.Items.Count == 0)
        this.printBtn.Enabled = false;
      else
        this.printBtn.Enabled = true;
    }

    private void printBtn_Click(object sender, EventArgs e)
    {
      try
      {
        string printFile = this.createPrintFile();
        Cursor.Current = Cursors.WaitCursor;
        this.printBtn.Enabled = false;
        new Process()
        {
          StartInfo = new ProcessStartInfo("notepad.exe", "/p " + printFile)
          {
            WindowStyle = ProcessWindowStyle.Hidden
          }
        }.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Print Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      Cursor.Current = Cursors.Default;
      this.printBtn.Enabled = true;
    }

    private string createPrintFile()
    {
      string str1 = new string('-', 80);
      string str2 = "\r\n\r\n" + "Loan Information:\r\n" + str1 + "\r\n" + "Borrower name:\t\t" + this.loanData.GetSimpleField("36") + " " + this.loanData.GetSimpleField("37") + "\r\n" + "  Loan Number:\t\t" + this.loanData.GetSimpleField("364") + "\r\n" + " Loan Purpose:\t\t" + this.loanData.GetSimpleField("19") + "\r\n" + "    Loan Type:\t\t" + this.loanData.GetSimpleField("1172") + "\r\n" + "  Loan Amount:\t\t" + this.loanData.GetField("2") + "\r\n" + "Interest Rate:\t\t" + this.loanData.GetField("3") + " %\r\n" + "    Loan Term:\t\t" + this.loanData.GetField("4") + "\r\n" + str1 + "\r\n\r\n" + "Missing Field List:\r\n\r\n";
      foreach (ListViewItem listViewItem in this.missingLvw.Items)
        str2 = str2 + listViewItem.SubItems[0].Text + "\t\t" + listViewItem.SubItems[1].Text + "\r\n";
      string str3 = str2 + str1 + "\r\n" + "\r\nThere are " + this.missingLvw.Items.Count.ToString() + " field(s) missing.";
      string tempFileName = Path.GetTempFileName();
      using (StreamWriter streamWriter = new StreamWriter(tempFileName))
        streamWriter.WriteLine(str3);
      return tempFileName;
    }
  }
}
