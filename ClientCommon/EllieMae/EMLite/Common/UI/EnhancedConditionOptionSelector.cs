// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.EnhancedConditionOptionSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class EnhancedConditionOptionSelector : Form
  {
    private string selectedOption = "";
    private IContainer components;
    private GridView gridView1;
    private GridView gridView2;
    private Label label1;
    private Label label2;
    private Label label3;
    private RadioButton radioButton1;
    private RadioButton radioButton2;
    private RadioButton radioButton3;
    private Panel panel1;
    private Button button1;
    private Button button2;
    private Label label4;
    private Panel panel2;
    private RadioButton radioButton4;
    private RadioButton radioButton5;
    private RadioButton radioButton6;

    public EnhancedConditionOptionSelector(string attribute)
    {
      this.InitializeComponent();
      this.populateCriteria1(attribute);
      this.populateCriteria2();
    }

    public string SelectedOption => this.selectedOption;

    private void populateCriteria1(string attribute)
    {
      this.gridView1.Sort(0, SortOrder.Ascending);
      this.gridView1.Columns[0].SortMethod = GVSortMethod.Text;
      this.gridView1.HeaderVisible = true;
      this.gridView1.SortOption = GVSortOption.Auto;
      this.gridView1.HeaderHeight = 20;
      this.label1.Text += attribute;
      if (attribute == "Tracking Owner")
      {
        RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
        if (allRoleFunctions == null || allRoleFunctions.Length == 0)
          return;
        foreach (RoleInfo roleInfo in allRoleFunctions)
          this.gridView1.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Text = roleInfo.Name
              }
            }
          });
      }
      else
      {
        foreach (string str in Session.ConfigurationManager.GetOptionsFromConditionType(attribute))
          this.gridView1.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Text = str
              }
            },
            Tag = (object) str
          });
      }
    }

    private void populateCriteria2()
    {
      List<string> conditionTypeList = Session.ConfigurationManager.GetEnhancedConditionTypeList(false);
      this.gridView2.Columns[0].Text = "Condition Type";
      this.gridView2.Sort(0, SortOrder.Ascending);
      this.gridView2.Columns[0].SortMethod = GVSortMethod.Text;
      this.gridView2.HeaderVisible = true;
      this.gridView2.SortOption = GVSortOption.Auto;
      this.gridView2.HeaderHeight = 20;
      this.gridView2.Items.Add("All");
      foreach (string str in conditionTypeList)
        this.gridView2.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = str
            }
          },
          Tag = (object) str
        });
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Count == 0 || this.gridView2.SelectedItems.Count == 0 || !this.radioButton1.Checked && !this.radioButton2.Checked && !this.radioButton3.Checked || !this.radioButton4.Checked && !this.radioButton5.Checked && !this.radioButton6.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select all four criteria", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string str1 = "";
        if (this.radioButton1.Checked)
          str1 = this.radioButton1.Tag.ToString();
        else if (this.radioButton2.Checked)
          str1 = this.radioButton2.Tag.ToString();
        else if (this.radioButton3.Checked)
          str1 = this.radioButton3.Tag.ToString();
        string str2 = "";
        if (this.radioButton4.Checked)
          str2 = this.radioButton4.Tag.ToString();
        else if (this.radioButton5.Checked)
          str2 = this.radioButton5.Tag.ToString();
        else if (this.radioButton6.Checked)
          str2 = this.radioButton6.Tag.ToString();
        this.selectedOption = str1 + "~%cbiz%~" + this.gridView2.SelectedItems[0].Text.Trim() + "~%cbiz%~" + str2 + "~%cbiz%~" + this.gridView1.SelectedItems[0].Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void label4_Click(object sender, EventArgs e)
    {
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
      this.gridView1 = new GridView();
      this.gridView2 = new GridView();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.radioButton1 = new RadioButton();
      this.radioButton2 = new RadioButton();
      this.radioButton3 = new RadioButton();
      this.panel1 = new Panel();
      this.button1 = new Button();
      this.button2 = new Button();
      this.label4 = new Label();
      this.panel2 = new Panel();
      this.radioButton4 = new RadioButton();
      this.radioButton5 = new RadioButton();
      this.radioButton6 = new RadioButton();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.gridView1.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 465;
      this.gridView1.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(24, 50);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(468, 178);
      this.gridView1.TabIndex = 0;
      this.gridView2.AllowMultiselect = false;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "";
      gvColumn2.Width = 465;
      this.gridView2.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gridView2.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView2.Location = new Point(24, 268);
      this.gridView2.Name = "gridView2";
      this.gridView2.Size = new Size(468, 178);
      this.gridView2.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(90, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Select Criteria 1 : ";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(21, 241);
      this.label2.Name = "label2";
      this.label2.Size = new Size(161, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Select Criteria 2 : Condition Type";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(24, 469);
      this.label3.Name = "label3";
      this.label3.Size = new Size(84, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Select Criteria 3:";
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new Point(3, 3);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(139, 17);
      this.radioButton1.TabIndex = 5;
      this.radioButton1.TabStop = true;
      this.radioButton1.Tag = (object) "External";
      this.radioButton1.Text = "External Conditions Only";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new Point(3, 26);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new Size(136, 17);
      this.radioButton2.TabIndex = 6;
      this.radioButton2.TabStop = true;
      this.radioButton2.Tag = (object) "Internal";
      this.radioButton2.Text = "Internal Conditions Only";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new Point(3, 49);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new Size(47, 17);
      this.radioButton3.TabIndex = 7;
      this.radioButton3.TabStop = true;
      this.radioButton3.Tag = (object) "All";
      this.radioButton3.Text = "Both";
      this.radioButton3.UseVisualStyleBackColor = true;
      this.panel1.Controls.Add((Control) this.radioButton1);
      this.panel1.Controls.Add((Control) this.radioButton3);
      this.panel1.Controls.Add((Control) this.radioButton2);
      this.panel1.Location = new Point(113, 466);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(185, 84);
      this.panel1.TabIndex = 8;
      this.button1.Location = new Point(323, 649);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "Insert";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Location = new Point(417, 649);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 10;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(24, 552);
      this.label4.Name = "label4";
      this.label4.Size = new Size(84, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Select Criteria 4:";
      this.label4.Click += new EventHandler(this.label4_Click);
      this.panel2.Controls.Add((Control) this.radioButton4);
      this.panel2.Controls.Add((Control) this.radioButton5);
      this.panel2.Controls.Add((Control) this.radioButton6);
      this.panel2.Location = new Point(113, 552);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(185, 79);
      this.panel2.TabIndex = 12;
      this.radioButton4.AutoSize = true;
      this.radioButton4.Location = new Point(3, 3);
      this.radioButton4.Name = "radioButton4";
      this.radioButton4.Size = new Size((int) sbyte.MaxValue, 17);
      this.radioButton4.TabIndex = 5;
      this.radioButton4.TabStop = true;
      this.radioButton4.Tag = (object) "OP";
      this.radioButton4.Text = "Open Conditions Only";
      this.radioButton4.UseVisualStyleBackColor = true;
      this.radioButton5.AutoSize = true;
      this.radioButton5.Location = new Point(3, 49);
      this.radioButton5.Name = "radioButton5";
      this.radioButton5.Size = new Size(47, 17);
      this.radioButton5.TabIndex = 7;
      this.radioButton5.TabStop = true;
      this.radioButton5.Tag = (object) "OC";
      this.radioButton5.Text = "Both";
      this.radioButton5.UseVisualStyleBackColor = true;
      this.radioButton6.AutoSize = true;
      this.radioButton6.Location = new Point(3, 26);
      this.radioButton6.Name = "radioButton6";
      this.radioButton6.Size = new Size(133, 17);
      this.radioButton6.TabIndex = 6;
      this.radioButton6.TabStop = true;
      this.radioButton6.Tag = (object) "CL";
      this.radioButton6.Text = "Closed Conditions Only";
      this.radioButton6.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(517, 687);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.gridView2);
      this.Controls.Add((Control) this.gridView1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (EnhancedConditionOptionSelector);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Options";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
