// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ZipcodeSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ZipcodeSetupControl : UserControl
  {
    private Sessions.Session session;
    private IContainer components;
    private StandardIconButton stdIconBtnNew;
    private ToolTip toolTip1;
    private GroupContainer gcZipcode;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private GridView lvwZipcode;

    public ZipcodeSetupControl()
      : this(Session.DefaultInstance)
    {
    }

    public ZipcodeSetupControl(Sessions.Session session)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.session = session;
      this.initForm();
    }

    private void initForm()
    {
      this.lvwZipcode.Items.Clear();
      ZipcodeInfoUserDefined[] zipcodeUserDefined = this.session.ConfigurationManager.GetZipcodeUserDefined((string) null);
      if (zipcodeUserDefined == null)
        return;
      this.lvwZipcode.BeginUpdate();
      for (int index = 0; index < zipcodeUserDefined.Length; ++index)
        this.buildGVItem((GVItem) null, zipcodeUserDefined[index], false);
      this.lvwZipcode.EndUpdate();
    }

    private void buildGVItem(GVItem item, ZipcodeInfoUserDefined zipcodeRec, bool selected)
    {
      if (item == null)
      {
        for (int index = 0; index < this.lvwZipcode.Columns.Count; ++index)
        {
          if (index == 0)
            item = new GVItem("");
          else
            item.SubItems.Add((object) "");
        }
        this.lvwZipcode.Items.Add(item);
      }
      item.Text = zipcodeRec.Zipcode + (zipcodeRec.ZipcodeExtension != string.Empty ? "-" + zipcodeRec.ZipcodeExtension : "");
      item.SubItems[1].Text = zipcodeRec.ZipInfo.City;
      item.SubItems[2].Text = zipcodeRec.ZipInfo.State;
      item.SubItems[3].Text = zipcodeRec.ZipInfo.County;
      item.Tag = (object) zipcodeRec;
      item.Selected = selected;
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (EditZipcodeForm editZipcodeForm = new EditZipcodeForm((ZipcodeInfoUserDefined) null, this.session))
      {
        if (editZipcodeForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.session.ConfigurationManager.UpdateZipcodeUserDefined(editZipcodeForm.NewZipcodeInfo, (ZipcodeInfoUserDefined) null);
        this.lvwZipcode.SelectedItems.Clear();
        this.buildGVItem((GVItem) null, editZipcodeForm.NewZipcodeInfo, true);
      }
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.lvwZipcode.SelectedItems.Count == 0)
        return;
      ZipcodeInfoUserDefined tag = (ZipcodeInfoUserDefined) this.lvwZipcode.SelectedItems[0].Tag;
      using (EditZipcodeForm editZipcodeForm = new EditZipcodeForm(tag, this.session))
      {
        if (editZipcodeForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.session.ConfigurationManager.UpdateZipcodeUserDefined(editZipcodeForm.NewZipcodeInfo, tag);
        this.buildGVItem(this.lvwZipcode.SelectedItems[0], editZipcodeForm.NewZipcodeInfo, true);
      }
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvwZipcode.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a zipcode.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected zipcode " + (this.lvwZipcode.SelectedItems.Count > 1 ? "records" : "record") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        List<ZipcodeInfoUserDefined> zipcodeInfoUserDefinedList = new List<ZipcodeInfoUserDefined>();
        for (int index = 0; index < this.lvwZipcode.SelectedItems.Count; ++index)
          zipcodeInfoUserDefinedList.Add((ZipcodeInfoUserDefined) this.lvwZipcode.SelectedItems[index].Tag);
        this.session.ConfigurationManager.DeleteZipcodeUserDefineds(zipcodeInfoUserDefinedList.ToArray());
        int index1 = this.lvwZipcode.SelectedItems[0].Index;
        int num2 = this.lvwZipcode.Items.Count - 1;
        this.lvwZipcode.BeginUpdate();
        for (int nItemIndex = num2; nItemIndex >= 0; --nItemIndex)
        {
          if (this.lvwZipcode.Items[nItemIndex].Selected)
            this.lvwZipcode.Items.Remove(this.lvwZipcode.Items[nItemIndex]);
        }
        this.lvwZipcode.EndUpdate();
        if (this.lvwZipcode.Items.Count == 0)
          return;
        if (index1 + 1 >= this.lvwZipcode.Items.Count)
          this.lvwZipcode.Items[this.lvwZipcode.Items.Count - 1].Selected = true;
        else
          this.lvwZipcode.Items[index1].Selected = true;
      }
    }

    private void lvwZipcode_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.lvwZipcode.SelectedItems.Count > 0;
      this.stdIconBtnEdit.Enabled = this.lvwZipcode.SelectedItems.Count == 1;
    }

    private ZipcodeInfoUserDefined[] GetSelectedZipcodes()
    {
      List<ZipcodeInfoUserDefined> zipcodeInfoUserDefinedList = new List<ZipcodeInfoUserDefined>();
      foreach (GVItem selectedItem in this.lvwZipcode.SelectedItems)
        zipcodeInfoUserDefinedList.Add((ZipcodeInfoUserDefined) selectedItem.Tag);
      return zipcodeInfoUserDefinedList.ToArray();
    }

    public List<string> SelectedZipcodes
    {
      get
      {
        ZipcodeInfoUserDefined[] selectedZipcodes1 = this.GetSelectedZipcodes();
        if (selectedZipcodes1 == null)
          return (List<string>) null;
        List<string> selectedZipcodes2 = new List<string>();
        foreach (ZipcodeInfoUserDefined zipcodeInfoUserDefined in selectedZipcodes1)
          selectedZipcodes2.Add(string.Format("{0}_{1}_{2}_{3}", (object) zipcodeInfoUserDefined.Zipcode, string.IsNullOrEmpty(zipcodeInfoUserDefined.ZipcodeExtension) ? (object) "0" : (object) zipcodeInfoUserDefined.ZipcodeExtension, (object) zipcodeInfoUserDefined.ZipInfo.City, (object) zipcodeInfoUserDefined.ZipInfo.State));
        return selectedZipcodes2;
      }
    }

    public void SetSelectedZipcodes(List<string> selectedZipcodes)
    {
      for (int index = 0; index < selectedZipcodes.Count && selectedZipcodes[index].IndexOf("_") >= 0; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.lvwZipcode.Items.Count; ++nItemIndex)
        {
          ZipcodeInfoUserDefined tag = (ZipcodeInfoUserDefined) this.lvwZipcode.Items[nItemIndex].Tag;
          if (tag.Zipcode + "_" + (string.IsNullOrEmpty(tag.ZipcodeExtension) ? "0" : tag.ZipcodeExtension) + "_" + tag.ZipInfo.City + "_" + tag.ZipInfo.State == selectedZipcodes[index])
          {
            this.lvwZipcode.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.gcZipcode = new GroupContainer();
      this.lvwZipcode = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gcZipcode.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.gcZipcode.Controls.Add((Control) this.stdIconBtnNew);
      this.gcZipcode.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcZipcode.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcZipcode.Controls.Add((Control) this.lvwZipcode);
      this.gcZipcode.Dock = DockStyle.Fill;
      this.gcZipcode.HeaderForeColor = SystemColors.ControlText;
      this.gcZipcode.Location = new Point(0, 0);
      this.gcZipcode.Name = "gcZipcode";
      this.gcZipcode.Size = new Size(843, 520);
      this.gcZipcode.TabIndex = 11;
      this.lvwZipcode.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Zipcode";
      gvColumn1.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "City";
      gvColumn2.Width = 260;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "State";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 80;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "County";
      gvColumn4.Width = 351;
      this.lvwZipcode.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.lvwZipcode.Dock = DockStyle.Fill;
      this.lvwZipcode.Location = new Point(1, 26);
      this.lvwZipcode.Name = "lvwZipcode";
      this.lvwZipcode.Size = new Size(841, 493);
      this.lvwZipcode.TabIndex = 9;
      this.lvwZipcode.TextTrimming = StringTrimming.EllipsisWord;
      this.lvwZipcode.SelectedIndexChanged += new EventHandler(this.lvwZipcode_SelectedIndexChanged);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(778, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 12;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(800, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(822, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 10;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcZipcode);
      this.Name = nameof (ZipcodeSetupControl);
      this.Size = new Size(843, 520);
      this.gcZipcode.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
