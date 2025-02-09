// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SpecialFeatureCodeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
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
  public class SpecialFeatureCodeDialog : Form
  {
    private LoanData loan;
    private GridViewFilterManager filterManager;
    private Dictionary<string, string> currentCodesInLoan = new Dictionary<string, string>();
    private string sourceForm;
    private IContainer components;
    private GridView gvSpecialFeatureCodes;
    private Button ok_btn;
    private Button cancel_btn;
    private GroupContainer groupList;

    public SpecialFeatureCodeDialog(LoanData loan, string type)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.sourceForm = type;
      this.loadCodes();
      this.filterManager = new GridViewFilterManager(Session.DefaultInstance, this.gvSpecialFeatureCodes, true);
      this.filterManager.CreateColumnFilter(2, GridViewFilterControlType.Text);
      this.filterManager.CreateColumnFilter(1, GridViewFilterControlType.Text);
      GroupContainer groupList = this.groupList;
      groupList.Text = groupList.Text + " - " + type;
    }

    private void specialFCodes_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.gvSpecialFeatureCodes.Items.Where<GVItem>((Func<GVItem, bool>) (x => x.SubItems[0].Checked)).Count<GVItem>() <= 10)
        return;
      e.SubItem.Checked = false;
      int num = (int) Utils.Dialog((IWin32Window) this, "ULDD may only submit up to 10 Special Feature Codes.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void loadCodes()
    {
      int specialFeatureCode = this.loan.GetNumberOfSpecialFeatureCode();
      this.currentCodesInLoan.Clear();
      for (int index = 1; index <= specialFeatureCode; ++index)
      {
        string str1 = "SFC" + index.ToString("00");
        string field = this.loan.GetField(str1 + "01");
        string str2 = this.formatSource(this.loan.GetField(str1 + "04"));
        if (str2.Equals(this.sourceForm))
          this.currentCodesInLoan.Add(field, str2);
      }
      IList<SpecialFeatureCodeDefinition> active = Session.SessionObjects.SpecialFeatureCodeManager.GetActive();
      if (active != null)
      {
        foreach (SpecialFeatureCodeDefinition featureCodeDefinition in (IEnumerable<SpecialFeatureCodeDefinition>) active)
        {
          if (featureCodeDefinition.Source.Equals(this.sourceForm))
          {
            GVItem gvItem = new GVItem();
            gvItem.SubItems[0].Tag = (object) featureCodeDefinition.ID;
            gvItem.SubItems[1].Text = featureCodeDefinition.Code;
            gvItem.SubItems[2].Text = featureCodeDefinition.Description;
            gvItem.SubItems[3].Text = featureCodeDefinition.Source;
            if (this.currentCodesInLoan.ContainsKey(featureCodeDefinition.Code) && this.currentCodesInLoan[featureCodeDefinition.Code].Equals(featureCodeDefinition.Source))
              gvItem.SubItems[0].Checked = true;
            this.gvSpecialFeatureCodes.Items.Add(gvItem);
          }
        }
      }
      if (this.gvSpecialFeatureCodes.Items.Where<GVItem>((Func<GVItem, bool>) (x => x.SubItems[0].Checked)).Count<GVItem>() > 0)
        this.gvSpecialFeatureCodes.Sort(0, SortOrder.Descending);
      else
        this.gvSpecialFeatureCodes.Sort(1, SortOrder.Ascending);
    }

    private void ok_BtnClick(object sender, EventArgs e)
    {
      IEnumerable<GVItem> source = this.gvSpecialFeatureCodes.Items.Where<GVItem>((Func<GVItem, bool>) (item => item.SubItems[0].Checked));
      for (int specialFeatureCode = this.loan.GetNumberOfSpecialFeatureCode(); specialFeatureCode > 0; --specialFeatureCode)
      {
        string code = this.loan.GetField("SFC" + specialFeatureCode.ToString("00") + "01");
        string codeSource = this.loan.GetField("SFC" + specialFeatureCode.ToString("00") + "04");
        codeSource = this.formatSource(codeSource);
        if (source.Any<GVItem>((Func<GVItem, bool>) (x => x.SubItems[1].Value.ToString().Equals(code) && codeSource.Equals(this.sourceForm))))
          source = source.Where<GVItem>((Func<GVItem, bool>) (x => !x.SubItems[1].Value.ToString().Equals(code)));
        else if (codeSource.Equals(this.sourceForm))
          this.loan.RemoveSpecialFeatureCodeAt(specialFeatureCode - 1);
      }
      foreach (GVItem gvItem in source)
      {
        int num = this.loan.NewSpecialFeatureCode();
        this.loan.SetCurrentFieldFromCal("SFC" + num.ToString("00") + "01", gvItem.SubItems[1].Value.ToString());
        this.loan.SetCurrentFieldFromCal("SFC" + num.ToString("00") + "02", gvItem.SubItems[2].Value.ToString());
        string val = this.formatSource(gvItem.SubItems[3].Value.ToString());
        this.loan.SetCurrentFieldFromCal("SFC" + num.ToString("00") + "04", val);
      }
      this.loan.Calculator.FormCalculation("SFC0001");
      this.DialogResult = DialogResult.OK;
    }

    private void cancel_BtnClick(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private string formatSource(string source)
    {
      string str = source;
      switch (source)
      {
        case "Fannie Mae":
          str = "FannieMae";
          break;
        case "Freddie Mac":
          str = "FreddieMac";
          break;
        case "FannieMae":
          str = "Fannie Mae";
          break;
        case "FreddieMac":
          str = "Freddie Mac";
          break;
      }
      return str;
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
      GVColumn gvColumn4 = new GVColumn();
      this.gvSpecialFeatureCodes = new GridView();
      this.ok_btn = new Button();
      this.cancel_btn = new Button();
      this.groupList = new GroupContainer();
      this.groupList.SuspendLayout();
      this.SuspendLayout();
      this.gvSpecialFeatureCodes.AllowMultiselect = false;
      this.gvSpecialFeatureCodes.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Checkbox;
      gvColumn1.Text = "Add/Delete";
      gvColumn1.Width = 70;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Code";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 370;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Source";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 80;
      this.gvSpecialFeatureCodes.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvSpecialFeatureCodes.Dock = DockStyle.Fill;
      this.gvSpecialFeatureCodes.FilterVisible = true;
      this.gvSpecialFeatureCodes.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSpecialFeatureCodes.Location = new Point(1, 26);
      this.gvSpecialFeatureCodes.Margin = new Padding(4, 5, 4, 5);
      this.gvSpecialFeatureCodes.Name = "gvSpecialFeatureCodes";
      this.gvSpecialFeatureCodes.Size = new Size(961, 540);
      this.gvSpecialFeatureCodes.TabIndex = 0;
      this.gvSpecialFeatureCodes.SubItemCheck += new GVSubItemEventHandler(this.specialFCodes_SubItemCheck);
      this.ok_btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.ok_btn.Location = new Point(751, 591);
      this.ok_btn.Margin = new Padding(4, 5, 4, 5);
      this.ok_btn.Name = "ok_btn";
      this.ok_btn.Size = new Size(112, 35);
      this.ok_btn.TabIndex = 1;
      this.ok_btn.Text = "OK";
      this.ok_btn.UseVisualStyleBackColor = true;
      this.ok_btn.Click += new EventHandler(this.ok_BtnClick);
      this.cancel_btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancel_btn.Location = new Point(871, 591);
      this.cancel_btn.Margin = new Padding(4, 5, 4, 5);
      this.cancel_btn.Name = "cancel_btn";
      this.cancel_btn.Size = new Size(112, 35);
      this.cancel_btn.TabIndex = 2;
      this.cancel_btn.Text = "Cancel";
      this.cancel_btn.UseVisualStyleBackColor = true;
      this.cancel_btn.Click += new EventHandler(this.cancel_BtnClick);
      this.groupList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupList.Controls.Add((Control) this.gvSpecialFeatureCodes);
      this.groupList.HeaderForeColor = SystemColors.ControlText;
      this.groupList.Location = new Point(17, 14);
      this.groupList.Margin = new Padding(4, 5, 4, 5);
      this.groupList.Name = "groupList";
      this.groupList.Size = new Size(963, 567);
      this.groupList.TabIndex = 6;
      this.groupList.Text = "Special Feature Code List";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(996, 638);
      this.Controls.Add((Control) this.groupList);
      this.Controls.Add((Control) this.cancel_btn);
      this.Controls.Add((Control) this.ok_btn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SpecialFeatureCodeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Special Feature Codes Look-up";
      this.groupList.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
