// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFieldErrors
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddFieldErrors : Form
  {
    private ArrayList errorList = new ArrayList();
    private AddFields.ImportableLoanXDBFieldTable importedErrorFieldTable;
    private IContainer components;
    private Panel panelTop;
    private Panel panelBottom;
    private GroupContainer grpList;
    private GridView listViewField;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton exportBtn;
    private Button closeBtn;
    private Label lblErrorMessage;
    private Panel panel1;

    public AddFieldErrors(
      Sessions.Session session,
      string errMsg,
      ArrayList errorList,
      AddFields.ImportableLoanXDBFieldTable importedErrorFieldTable)
    {
      this.InitializeComponent();
      this.lblErrorMessage.Text = "";
      if ((errMsg ?? "") != "")
        this.lblErrorMessage.Text = errMsg;
      if (errorList != null)
        this.errorList = errorList;
      if (importedErrorFieldTable != null)
        this.importedErrorFieldTable = importedErrorFieldTable;
      this.initializeErrorFields();
    }

    private void initializeErrorFields()
    {
      this.Cursor = Cursors.WaitCursor;
      this.listViewField.BeginUpdate();
      for (int index = 0; index < this.errorList.Count; ++index)
      {
        string[] error = (string[]) this.errorList[index];
        this.listViewField.Items.Add(new GVItem(error[0])
        {
          SubItems = {
            (object) error[1],
            (object) error[2]
          }
        });
      }
      this.listViewField.EndUpdate();
      this.listViewField.ReSort();
      this.Cursor = Cursors.Default;
    }

    private void closeBtn_Click(object sender, EventArgs e) => this.Close();

    private void exportBtn_Click(object sender, EventArgs e)
    {
      if (this.importedErrorFieldTable == null)
        return;
      string str1 = string.Empty;
      string str2 = string.Empty;
      using (ExportToLocalDialog exportToLocalDialog = new ExportToLocalDialog("Export Fields", "ErrorFields-" + DateTime.Now.ToString("MMddyyy") + ".txt"))
      {
        if (exportToLocalDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        str1 = exportToLocalDialog.SelectedFolder;
        str2 = exportToLocalDialog.SelectedFileName;
      }
      try
      {
        FileStream fileStream = new FileStream(str1 + "\\" + str2, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] bytes = Encoding.ASCII.GetBytes(this.importedErrorFieldTable.ToCSV());
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access rights to write file to " + str1 + " folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The file has been saved to " + str1 + "\\" + str2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
      GVColumn gvColumn3 = new GVColumn();
      this.panelTop = new Panel();
      this.grpList = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.exportBtn = new StandardIconButton();
      this.listViewField = new GridView();
      this.panelBottom = new Panel();
      this.closeBtn = new Button();
      this.lblErrorMessage = new Label();
      this.panel1 = new Panel();
      this.panelTop.SuspendLayout();
      this.grpList.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.exportBtn).BeginInit();
      this.panelBottom.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panelTop.Controls.Add((Control) this.panel1);
      this.panelTop.Controls.Add((Control) this.grpList);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(417, 426);
      this.panelTop.TabIndex = 0;
      this.grpList.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpList.Controls.Add((Control) this.listViewField);
      this.grpList.Dock = DockStyle.Bottom;
      this.grpList.HeaderForeColor = SystemColors.ControlText;
      this.grpList.Location = new Point(0, 32);
      this.grpList.Name = "grpList";
      this.grpList.Size = new Size(417, 394);
      this.grpList.TabIndex = 0;
      this.grpList.Text = "Field List";
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.exportBtn);
      this.flowLayoutPanel1.Location = new Point(388, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(24, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.exportBtn.BackColor = Color.Transparent;
      this.exportBtn.Location = new Point(3, 3);
      this.exportBtn.MouseDownImage = (Image) null;
      this.exportBtn.Name = "exportBtn";
      this.exportBtn.Size = new Size(16, 16);
      this.exportBtn.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.exportBtn.TabIndex = 0;
      this.exportBtn.TabStop = false;
      this.exportBtn.Click += new EventHandler(this.exportBtn_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pair";
      gvColumn2.Width = 50;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 350;
      this.listViewField.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewField.Dock = DockStyle.Fill;
      this.listViewField.Location = new Point(1, 26);
      this.listViewField.Name = "listViewField";
      this.listViewField.Size = new Size(415, 367);
      this.listViewField.TabIndex = 0;
      this.panelBottom.Controls.Add((Control) this.closeBtn);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 389);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(417, 37);
      this.panelBottom.TabIndex = 1;
      this.closeBtn.Location = new Point(335, 8);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 22);
      this.closeBtn.TabIndex = 0;
      this.closeBtn.Text = "&Close";
      this.closeBtn.UseVisualStyleBackColor = true;
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.lblErrorMessage.AutoSize = true;
      this.lblErrorMessage.Dock = DockStyle.Fill;
      this.lblErrorMessage.Location = new Point(0, 0);
      this.lblErrorMessage.Margin = new Padding(3);
      this.lblErrorMessage.Name = "lblErrorMessage";
      this.lblErrorMessage.Size = new Size(35, 13);
      this.lblErrorMessage.TabIndex = 1;
      this.lblErrorMessage.Text = "label1";
      this.panel1.Controls.Add((Control) this.lblErrorMessage);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(417, 32);
      this.panel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(417, 426);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panelTop);
      this.Name = nameof (AddFieldErrors);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Invalid Field ID";
      this.panelTop.ResumeLayout(false);
      this.grpList.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.exportBtn).EndInit();
      this.panelBottom.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
