// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableRules
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

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
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableRules : Form
  {
    private string dtName;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvRulesList;
    private Button btnClose;
    private Label lblNoOfRules;

    public DataTableRules(DDMDataTableInfo dtInfo)
    {
      this.InitializeComponent();
      this.dtName = dtInfo.Name;
      this.displayPage();
    }

    private void displayPage()
    {
      this.gvRulesList.Items.Clear();
      DDMFeeRulesBpmManager bpmManager1 = (DDMFeeRulesBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
      DDMFieldRulesBpmManager bpmManager2 = (DDMFieldRulesBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
      DDMFeeRule[] feeRuleByDataTable = bpmManager1.GetDDMFeeRuleByDataTable(this.dtName, true);
      DDMFieldRule[] fieldRuleByDataTable = bpmManager2.GetDDMFieldRuleByDataTable(this.dtName, true);
      Dictionary<DateTime, object> source = new Dictionary<DateTime, object>();
      foreach (DDMFeeRule ddmFeeRule in feeRuleByDataTable)
        source.Add(Convert.ToDateTime(ddmFeeRule.UpdateDt), (object) ddmFeeRule);
      foreach (DDMFieldRule ddmFieldRule in fieldRuleByDataTable)
        source.Add(Convert.ToDateTime(ddmFieldRule.UpdateDt), (object) ddmFieldRule);
      int num = 0;
      foreach (KeyValuePair<DateTime, object> keyValuePair in (IEnumerable<KeyValuePair<DateTime, object>>) source.OrderByDescending<KeyValuePair<DateTime, object>, DateTime>((Func<KeyValuePair<DateTime, object>, DateTime>) (i => i.Key)))
      {
        try
        {
          DDMFeeRule ddmFeeRule = (DDMFeeRule) keyValuePair.Value;
          Dictionary<string, int> dictionary = new Dictionary<string, int>();
          foreach (DDMFeeRuleScenario scenario in ddmFeeRule.Scenarios)
          {
            if (dictionary.ContainsKey(scenario.RuleName))
              dictionary[scenario.RuleName]++;
            else
              dictionary[scenario.RuleName] = 1;
          }
          using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              GVItem gvItem = new GVItem(ddmFeeRule.RuleName);
              gvItem.SubItems.Add((object) "Fee Rule");
              gvItem.SubItems.Add((object) current);
              gvItem.SubItems.Add((object) dictionary[current]);
              num += dictionary[current];
              this.gvRulesList.Items.Add(gvItem);
            }
            continue;
          }
        }
        catch
        {
        }
        try
        {
          DDMFieldRule ddmFieldRule = (DDMFieldRule) keyValuePair.Value;
          Dictionary<string, int> dictionary = new Dictionary<string, int>();
          foreach (DDMFieldRuleScenario scenario in ddmFieldRule.Scenarios)
          {
            if (dictionary.ContainsKey(scenario.RuleName))
              dictionary[scenario.RuleName]++;
            else
              dictionary[scenario.RuleName] = 1;
          }
          foreach (string key in dictionary.Keys)
          {
            GVItem gvItem = new GVItem(ddmFieldRule.RuleName);
            gvItem.SubItems.Add((object) "Field Rule");
            gvItem.SubItems.Add((object) key);
            gvItem.SubItems.Add((object) dictionary[key]);
            num += dictionary[key];
            this.gvRulesList.Items.Add(gvItem);
          }
        }
        catch
        {
        }
      }
      this.lblNoOfRules.Text = "Total number of references to this Data table: " + num.ToString();
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

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
      this.groupContainer1 = new GroupContainer();
      this.gvRulesList = new GridView();
      this.btnClose = new Button();
      this.lblNoOfRules = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvRulesList);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1186, 692);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Scenarios using this Table";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "RuleName";
      gvColumn1.Text = "Rule Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "RuleType";
      gvColumn2.Text = "Rule Type";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ScenarioNames";
      gvColumn3.Text = "Scenario Name";
      gvColumn3.Width = 230;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "RefScenario";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "# of References";
      gvColumn4.Width = 552;
      this.gvRulesList.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvRulesList.Dock = DockStyle.Fill;
      this.gvRulesList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRulesList.Location = new Point(1, 26);
      this.gvRulesList.Name = "gvRulesList";
      this.gvRulesList.Size = new Size(1184, 665);
      this.gvRulesList.TabIndex = 0;
      this.btnClose.Location = new Point(1062, 756);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(112, 35);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.lblNoOfRules.AutoSize = true;
      this.lblNoOfRules.Location = new Point(816, 717);
      this.lblNoOfRules.Name = "lblNoOfRules";
      this.lblNoOfRules.Size = new Size(36, 20);
      this.lblNoOfRules.TabIndex = 1;
      this.lblNoOfRules.Text = "test";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1186, 803);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.lblNoOfRules);
      this.Controls.Add((Control) this.btnClose);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DataTableRules);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Data Table Rules";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
