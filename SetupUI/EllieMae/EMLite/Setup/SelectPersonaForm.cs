// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectPersonaForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SelectPersonaForm : Form
  {
    private Button buttonCancel;
    private Button buttonSelect;
    private System.ComponentModel.Container components;
    private GridView gvPersonas;
    private RoleDetailForm parentForm;

    public SelectPersonaForm(int[] personaList, RoleDetailForm parent)
    {
      this.parentForm = parent;
      this.InitializeComponent();
      this.initForm(personaList);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.buttonCancel = new Button();
      this.buttonSelect = new Button();
      this.gvPersonas = new GridView();
      this.SuspendLayout();
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(278, 41);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "&Cancel";
      this.buttonSelect.Location = new Point(278, 10);
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(75, 23);
      this.buttonSelect.TabIndex = 1;
      this.buttonSelect.Text = "&Select";
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Personas";
      gvColumn.Width = 254;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvPersonas.Location = new Point(10, 10);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(256, 324);
      this.gvPersonas.TabIndex = 29;
      this.gvPersonas.ItemDoubleClick += new GVItemEventHandler(this.gvPersonas_ItemDoubleClick);
      this.AcceptButton = (IButtonControl) this.buttonSelect;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(364, 346);
      this.Controls.Add((Control) this.gvPersonas);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonSelect);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectPersonaForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Personas";
      this.ResumeLayout(false);
    }

    private void initForm(int[] groupIds)
    {
      this.gvPersonas.Items.Clear();
      foreach (int groupId in groupIds)
      {
        if (this.parentForm.PersonaSettings.ContainsKey((object) groupId))
          this.gvPersonas.Items.Add(new GVItem(this.parentForm.PersonaSettings[(object) groupId].ToString())
          {
            Tag = (object) groupId
          });
      }
      this.gvPersonas.Sort(0, SortOrder.Ascending);
    }

    public int[] GetSelectedPersonas()
    {
      List<int> intList = new List<int>();
      foreach (GVItem selectedItem in this.gvPersonas.SelectedItems)
        intList.Add((int) selectedItem.Tag);
      return intList.ToArray();
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      if (this.gvPersonas.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more Personas from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void gvPersonas_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!e.Item.Selected)
        return;
      this.DialogResult = DialogResult.OK;
    }
  }
}
