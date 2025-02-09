// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.InvestorSelectionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class InvestorSelectionDialog : Form
  {
    private DocumentOrderType orderType;
    private IContainer components;
    private Label label1;
    private GroupContainer groupContainer1;
    private DialogButtons dlgButtons;
    private GridView gvInvestors;

    public InvestorSelectionDialog(DocumentOrderType orderType)
    {
      this.orderType = orderType;
      this.InitializeComponent();
      this.loadInvestors();
    }

    public Investor GetSelectedInvestor()
    {
      return this.gvInvestors.SelectedItems.Count > 0 ? (Investor) this.gvInvestors.SelectedItems[0].Tag : (Investor) null;
    }

    private void loadInvestors()
    {
      using (CursorActivator.Wait())
      {
        Plan[] companyPlans = Plans.GetCompanyPlans(Session.SessionObjects, this.orderType, true);
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
        foreach (Plan plan in companyPlans)
        {
          Investor investor = plan.GetInvestor();
          if (!investor.IsGeneric && !dictionary.ContainsKey(investor.InvestorCode))
          {
            this.gvInvestors.Items.Add(new GVItem()
            {
              SubItems = {
                [0] = {
                  Text = investor.InvestorCode
                },
                [1] = {
                  Text = investor.Name
                }
              },
              Tag = (object) investor
            });
            dictionary[investor.InvestorCode] = true;
          }
        }
      }
      this.gvInvestors.Sort(1, SortOrder.Ascending);
    }

    private void gvInvestors_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.gvInvestors.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an investor from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
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
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.dlgButtons = new DialogButtons();
      this.gvInvestors = new GridView();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(321, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the investor to be used when drawing closing documents.";
      this.groupContainer1.Controls.Add((Control) this.gvInvestors);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 30);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(332, 287);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Investors";
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 321);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(352, 39);
      this.dlgButtons.TabIndex = 2;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.gvInvestors.AllowMultiselect = false;
      this.gvInvestors.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Code";
      gvColumn1.Width = 75;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = (int) byte.MaxValue;
      this.gvInvestors.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvInvestors.Dock = DockStyle.Fill;
      this.gvInvestors.Location = new Point(1, 26);
      this.gvInvestors.Name = "gvInvestors";
      this.gvInvestors.Size = new Size(330, 260);
      this.gvInvestors.TabIndex = 0;
      this.gvInvestors.ItemDoubleClick += new GVItemEventHandler(this.gvInvestors_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(352, 360);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InvestorSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Investor";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
