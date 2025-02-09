// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.ClientIDsManagementForm
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class ClientIDsManagementForm : Form
  {
    private bool _dirty;
    private IContainer components;
    private ListBox listBoxClientIDs;
    private Button btnClose;
    private Button btnUp;
    private Button btnDown;
    private Button btnDelete;
    private Button btnAdd;
    private Button btnSave;

    private bool dirty
    {
      get => this._dirty;
      set
      {
        this._dirty = value;
        this.btnSave.Enabled = this._dirty;
      }
    }

    public ClientIDsManagementForm(string[] clientIDs)
    {
      this.InitializeComponent();
      this.listBoxClientIDs.Items.Clear();
      if (clientIDs != null)
        this.listBoxClientIDs.Items.AddRange((object[]) clientIDs);
      this.dirty = false;
      this.listBoxClientIDs_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void listBoxClientIDs_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listBoxClientIDs.SelectedItems.Count == 0)
        this.btnDelete.Enabled = this.btnUp.Enabled = this.btnDown.Enabled = false;
      else if (this.listBoxClientIDs.SelectedItems.Count == 1)
      {
        this.btnDelete.Enabled = true;
        this.btnUp.Enabled = this.listBoxClientIDs.SelectedIndices[0] != 0;
        this.btnDown.Enabled = this.listBoxClientIDs.SelectedIndices[0] != this.listBoxClientIDs.Items.Count - 1;
      }
      else
      {
        this.btnDown.Enabled = true;
        this.btnUp.Enabled = this.btnDown.Enabled = false;
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (!this.dirty || MessageBox.Show("Do you want to save the changes?", "Encompass SmartClient", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        return;
      this.save();
    }

    private void setRegistrySmartClientIDs(string[] clientIDs)
    {
      if (clientIDs == null)
        return;
      string subkey = "Software\\Ellie Mae\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
      try
      {
        using (RegistryKey subKey = BasicUtils.GetRegistryHive((string) null).CreateSubKey(subkey))
        {
          string str = string.Join(", ", clientIDs);
          subKey.SetValue("SmartClientIDs", (object) str);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error setting client IDs in registry: " + ex.Message, "Encompass SmartClient");
      }
    }

    private void save()
    {
      this.dirty = false;
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.listBoxClientIDs.Items.Count; ++index)
        stringList.Add((string) this.listBoxClientIDs.Items[index]);
      this.setRegistrySmartClientIDs(stringList.ToArray());
    }

    private void btnSave_Click(object sender, EventArgs e) => this.save();

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      ClientIDInputForm clientIdInputForm = new ClientIDInputForm();
      if (clientIdInputForm.ShowDialog() == DialogResult.Cancel)
        return;
      for (int index = 0; index < this.listBoxClientIDs.Items.Count; ++index)
      {
        if (string.Compare(clientIdInputForm.ClientID, (string) this.listBoxClientIDs.Items[index], true) == 0)
        {
          int num = (int) MessageBox.Show("SmartClient ID '" + clientIdInputForm.ClientID + "' is already in the list", "Encompass SmartClient");
          return;
        }
      }
      this.listBoxClientIDs.Items.Insert(0, (object) clientIdInputForm.ClientID);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.listBoxClientIDs.SelectedItems.Count; ++index)
        stringList.Add((string) this.listBoxClientIDs.SelectedItems[index]);
      foreach (object obj in stringList)
        this.listBoxClientIDs.Items.Remove(obj);
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (this.listBoxClientIDs.SelectedIndices.Count != 1 || this.listBoxClientIDs.SelectedIndices[0] == 0)
        return;
      this.dirty = true;
      int selectedIndex = this.listBoxClientIDs.SelectedIndices[0];
      object selectedItem = this.listBoxClientIDs.SelectedItems[0];
      this.listBoxClientIDs.Items.RemoveAt(this.listBoxClientIDs.SelectedIndices[0]);
      this.listBoxClientIDs.Items.Insert(selectedIndex - 1, selectedItem);
      this.listBoxClientIDs.SetSelected(selectedIndex - 1, true);
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (this.listBoxClientIDs.SelectedIndices.Count != 1 || this.listBoxClientIDs.SelectedIndices[0] == this.listBoxClientIDs.Items.Count - 1)
        return;
      this.dirty = true;
      int selectedIndex = this.listBoxClientIDs.SelectedIndices[0];
      object selectedItem = this.listBoxClientIDs.SelectedItems[0];
      this.listBoxClientIDs.Items.RemoveAt(this.listBoxClientIDs.SelectedIndices[0]);
      this.listBoxClientIDs.Items.Insert(selectedIndex + 1, selectedItem);
      this.listBoxClientIDs.SetSelected(selectedIndex + 1, true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ClientIDsManagementForm));
      this.listBoxClientIDs = new ListBox();
      this.btnClose = new Button();
      this.btnUp = new Button();
      this.btnDown = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.btnSave = new Button();
      this.SuspendLayout();
      this.listBoxClientIDs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listBoxClientIDs.FormattingEnabled = true;
      this.listBoxClientIDs.Location = new Point(12, 11);
      this.listBoxClientIDs.Name = "listBoxClientIDs";
      this.listBoxClientIDs.SelectionMode = SelectionMode.MultiExtended;
      this.listBoxClientIDs.Size = new Size(237, 134);
      this.listBoxClientIDs.TabIndex = 0;
      this.listBoxClientIDs.SelectedIndexChanged += new EventHandler(this.listBoxClientIDs_SelectedIndexChanged);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(258, 162);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUp.Location = new Point(258, 35);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(75, 23);
      this.btnUp.TabIndex = 2;
      this.btnUp.Text = "^ Up";
      this.btnUp.UseVisualStyleBackColor = true;
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.btnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDown.Location = new Point(258, 65);
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(75, 23);
      this.btnDown.TabIndex = 3;
      this.btnDown.Text = "v Down";
      this.btnDown.UseVisualStyleBackColor = true;
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDelete.Location = new Point(93, 162);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 4;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Location = new Point(12, 162);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 5;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(174, 162);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(345, 197);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.btnDown);
      this.Controls.Add((Control) this.btnUp);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.listBoxClientIDs);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ClientIDsManagementForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Manage SmartClient IDs";
      this.ResumeLayout(false);
    }
  }
}
