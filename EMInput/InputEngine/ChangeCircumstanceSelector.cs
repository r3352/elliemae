// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ChangeCircumstanceSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ChangeCircumstanceSelector : Form
  {
    private readonly int[] LE_REASON = new int[3]
    {
      -1,
      0,
      13
    };
    private readonly int[] CD_REASON = new int[9]
    {
      -1,
      0,
      7,
      8,
      9,
      10,
      11,
      12,
      13
    };
    private readonly int[] GFE_LE_REASON = new int[9]
    {
      -1,
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      13
    };
    private readonly int[] GFE_CD_REASON = new int[7]
    {
      -1,
      0,
      1,
      2,
      3,
      4,
      13
    };
    private bool multiselect;
    private string appliesTo = "";
    private bool isFeeLevel = true;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private GridView listViewOptions;
    private CheckBox chk_ShowAllCoC;

    public ChangeCircumstanceSelector()
    {
      this.InitializeComponent();
      this.isFeeLevel = false;
      this.chk_ShowAllCoC.Visible = false;
      this.initForm();
    }

    public ChangeCircumstanceSelector(bool multiselect, string _appliesTo, bool IsFeeLevel)
    {
      this.appliesTo = _appliesTo;
      this.isFeeLevel = IsFeeLevel;
      this.InitializeComponent();
      this.multiselect = multiselect;
      this.listViewOptions.AllowMultiselect = multiselect;
      if (!this.isFeeLevel)
        this.chk_ShowAllCoC.Visible = false;
      this.initForm();
    }

    private void initForm()
    {
      List<ChangeCircumstanceSettings> circumstanceSettings1 = Session.ConfigurationManager.GetAllChangeCircumstanceSettings();
      List<ChangeCircumstanceSettings> circumstanceSettingsList1 = new List<ChangeCircumstanceSettings>();
      List<ChangeCircumstanceSettings> circumstanceSettingsList2 = circumstanceSettings1;
      List<string[]> strArrayList = new List<string[]>();
      foreach (ChangeCircumstanceSettings circumstanceSettings2 in circumstanceSettingsList2)
      {
        if (this.isFeeLevel && !this.chk_ShowAllCoC.Checked)
        {
          if (this.listViewOptions.AllowMultiselect)
          {
            if (this.appliesTo.ToUpperInvariant() == "CD" && !((IEnumerable<int>) this.CD_REASON).Contains<int>(circumstanceSettings2.Reason) || this.appliesTo.ToUpperInvariant() == "LE" && !((IEnumerable<int>) this.LE_REASON).Contains<int>(circumstanceSettings2.Reason))
              continue;
          }
          else if (this.appliesTo.ToUpperInvariant() == "CD" && !((IEnumerable<int>) this.GFE_CD_REASON).Contains<int>(circumstanceSettings2.Reason) || this.appliesTo.ToUpperInvariant() == "LE" && !((IEnumerable<int>) this.GFE_LE_REASON).Contains<int>(circumstanceSettings2.Reason))
            continue;
        }
        string[] strArray = new string[4]
        {
          circumstanceSettings2.Code,
          circumstanceSettings2.Description,
          circumstanceSettings2.Comment,
          circumstanceSettings2.Reason.ToString()
        };
        strArrayList.Add(strArray);
      }
      this.listViewOptions.BeginUpdate();
      for (int index = 0; index < strArrayList.Count; ++index)
        this.listViewOptions.Items.Add(new GVItem(strArrayList[index][1])
        {
          SubItems = {
            (object) strArrayList[index][2],
            (object) strArrayList[index][0]
          },
          Tag = (object) strArrayList[index]
        });
      this.listViewOptions.EndUpdate();
      this.listViewOptions_SelectedIndexChanged((object) null, (EventArgs) null);
      if (this.listViewOptions.AllowMultiselect)
        return;
      this.Text = "Select a changed circumstance below.";
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.multiselect)
        this.dialogButtons1.OKButton.Enabled = this.listViewOptions.SelectedItems.Count >= 1;
      else
        this.dialogButtons1.OKButton.Enabled = this.listViewOptions.SelectedItems.Count == 1;
    }

    public string OptionCode => this.listViewOptions.SelectedItems[0].SubItems[2].Text.Trim();

    public string OptionValue => this.listViewOptions.SelectedItems[0].Text.Trim();

    public string OptionComment => this.listViewOptions.SelectedItems[0].SubItems[1].Text.Trim();

    public string OptionReason => ((string[]) this.listViewOptions.SelectedItems[0].Tag)[3];

    public List<string[]> AllOptions
    {
      get
      {
        List<string[]> allOptions = new List<string[]>();
        if (this.listViewOptions.AllowMultiselect)
        {
          foreach (GVItem selectedItem in this.listViewOptions.SelectedItems)
            allOptions.Add((string[]) selectedItem.Tag);
        }
        return allOptions;
      }
    }

    private void chk_ShowAllCoC_CheckedChanged(object sender, EventArgs e)
    {
      this.listViewOptions.Items.Clear();
      this.initForm();
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
      this.listViewOptions = new GridView();
      this.dialogButtons1 = new DialogButtons();
      this.chk_ShowAllCoC = new CheckBox();
      this.SuspendLayout();
      this.listViewOptions.AllowMultiselect = false;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Changed Circumstance ";
      gvColumn1.Width = 400;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Comments";
      gvColumn2.Width = 554;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Code";
      gvColumn3.Width = 100;
      this.listViewOptions.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(0, 0);
      this.listViewOptions.Margin = new Padding(4, 5, 4, 5);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(1054, 535);
      this.listViewOptions.TabIndex = 1;
      this.listViewOptions.SelectedIndexChanged += new EventHandler(this.listViewOptions_SelectedIndexChanged);
      this.listViewOptions.DoubleClick += new EventHandler(this.dialogButtons1_OK);
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 535);
      this.dialogButtons1.Margin = new Padding(6, 8, 6, 8);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(1054, 68);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.chk_ShowAllCoC.AutoSize = true;
      this.chk_ShowAllCoC.Location = new Point(18, 562);
      this.chk_ShowAllCoC.Name = "chk_ShowAllCoC";
      this.chk_ShowAllCoC.Size = new Size(202, 24);
      this.chk_ShowAllCoC.TabIndex = 2;
      this.chk_ShowAllCoC.Text = "Show All COC Reasons";
      this.chk_ShowAllCoC.UseVisualStyleBackColor = true;
      this.chk_ShowAllCoC.CheckedChanged += new EventHandler(this.chk_ShowAllCoC_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1054, 603);
      this.Controls.Add((Control) this.chk_ShowAllCoC);
      this.Controls.Add((Control) this.listViewOptions);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Margin = new Padding(4, 5, 4, 5);
      this.MinimizeBox = false;
      this.Name = nameof (ChangeCircumstanceSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select one or more changed circumstances below. Press Ctrl or Shift key to click and select multiple options.";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
