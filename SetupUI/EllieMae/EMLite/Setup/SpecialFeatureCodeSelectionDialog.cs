// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SpecialFeatureCodeSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SpecialFeatureCodeSelectionDialog : Form
  {
    private Sessions.Session session;
    private readonly ISpecialFeatureCodeManager specialFeatureCodeManager;
    public List<SpecialFeatureCodeDefinition> listSpecialFeatureCodeDefinition;
    private IContainer components;
    private GroupContainer groupContainerTemplate;
    private GridView gridViewSPcodes;
    private Button btnCancel;
    private Button btnOK;

    public SpecialFeatureCodeSelectionDialog(
      Sessions.Session session,
      Dictionary<string, string> selectedSpecialFeatureCodes)
    {
      this.session = session;
      this.InitializeComponent();
      this.specialFeatureCodeManager = session.SpecialFeatureCodeManager;
      this.LoadSpecialFeatureCodeToGridView(selectedSpecialFeatureCodes);
    }

    private void LoadSpecialFeatureCodeToGridView(
      Dictionary<string, string> selectedSpecialFeatureCodes)
    {
      this.gridViewSPcodes.Items.Clear();
      IOrderedEnumerable<SpecialFeatureCodeDefinition> source = this.specialFeatureCodeManager.GetAll().OrderByDescending<SpecialFeatureCodeDefinition, bool>((Func<SpecialFeatureCodeDefinition, bool>) (s => s.IsActive));
      this.groupContainerTemplate.Text = string.Format("Special Feature Codes ({0})", (object) source.Count<SpecialFeatureCodeDefinition>());
      foreach (SpecialFeatureCodeDefinition featureCodeDefinition in (IEnumerable<SpecialFeatureCodeDefinition>) source)
      {
        // ISSUE: explicit non-virtual call
        GVItem gvItem = new GVItem((object) featureCodeDefinition)
        {
          SubItems = {
            [0] = {
              Text = ""
            }
          },
          Checked = selectedSpecialFeatureCodes != null && __nonvirtual (selectedSpecialFeatureCodes.ContainsKey(featureCodeDefinition.ID))
        };
        gvItem.SubItems[1].Value = (object) featureCodeDefinition.ID;
        gvItem.SubItems[2].Text = featureCodeDefinition.Code;
        gvItem.SubItems[3].Text = featureCodeDefinition.Description;
        gvItem.SubItems[4].Text = featureCodeDefinition.Comment;
        gvItem.SubItems[5].Text = featureCodeDefinition.Source;
        gvItem.SubItems[6].Value = featureCodeDefinition.IsActive ? (object) "Active" : (object) "Inactive";
        gvItem.Tag = (object) featureCodeDefinition;
        this.gridViewSPcodes.Items.Add(gvItem);
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.listSpecialFeatureCodeDefinition = new List<SpecialFeatureCodeDefinition>();
      for (int nItemIndex = 0; nItemIndex < this.gridViewSPcodes.Items.Count; ++nItemIndex)
      {
        if (this.gridViewSPcodes.Items[nItemIndex].Checked)
          this.listSpecialFeatureCodeDefinition.Add((SpecialFeatureCodeDefinition) this.gridViewSPcodes.Items[nItemIndex].Tag);
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.btnCancel = new Button();
      this.groupContainerTemplate = new GroupContainer();
      this.gridViewSPcodes = new GridView();
      this.btnOK = new Button();
      this.groupContainerTemplate.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1378, 642);
      this.btnCancel.Margin = new Padding(6);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(150, 44);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainerTemplate.Controls.Add((Control) this.gridViewSPcodes);
      this.groupContainerTemplate.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTemplate.Location = new Point(15, 15);
      this.groupContainerTemplate.Margin = new Padding(6);
      this.groupContainerTemplate.Name = "groupContainerTemplate";
      this.groupContainerTemplate.Size = new Size(1513, 615);
      this.groupContainerTemplate.TabIndex = 0;
      this.groupContainerTemplate.Text = "Special Feature Codes";
      this.gridViewSPcodes.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colSelect";
      gvColumn1.SortMethod = GVSortMethod.Checkbox;
      gvColumn1.Text = "Select";
      gvColumn1.Width = 45;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colID";
      gvColumn2.Text = "ID";
      gvColumn2.Width = 0;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colCode";
      gvColumn3.Text = "Code";
      gvColumn3.Width = 80;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colDescription";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 229;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colComment";
      gvColumn5.Text = "Comments";
      gvColumn5.Width = 220;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colSource";
      gvColumn6.Text = "Source";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colIsActive";
      gvColumn7.Text = "Status";
      gvColumn7.Width = 60;
      this.gridViewSPcodes.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridViewSPcodes.Dock = DockStyle.Fill;
      this.gridViewSPcodes.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewSPcodes.Location = new Point(1, 26);
      this.gridViewSPcodes.Margin = new Padding(6);
      this.gridViewSPcodes.Name = "gridViewSPcodes";
      this.gridViewSPcodes.Size = new Size(1511, 588);
      this.gridViewSPcodes.TabIndex = 0;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(1166, 643);
      this.btnOK.Margin = new Padding(6);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(150, 44);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(12f, 25f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1543, 691);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainerTemplate);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(6);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SpecialFeatureCodeSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Special Feature Codes";
      this.groupContainerTemplate.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
