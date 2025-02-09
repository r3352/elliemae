// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FormListDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FormListDialog : Form, IHelp
  {
    private const string className = "FormListDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Label label2;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label1;
    private Button cancelBtn;
    private Button saveBtn;
    private Button removeBtn;
    private Button addBtn;
    private System.ComponentModel.Container components;
    private FormTemplate formTemplate;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private GridView gridPredefined;
    private GroupContainer groupContainerSelected;
    private StandardIconButton stdBtnDown;
    private StandardIconButton stdBtnUp;
    private GridView gridSelected;
    private const string dividline = "---- Line ----";
    private bool readOnly;
    private Sessions.Session session;
    private const string BADCHARS = "/:*?<>|.";

    public FormListDialog(FormTemplate formTemplate, bool readOnly)
      : this(formTemplate, Session.DefaultInstance)
    {
      this.readOnly = readOnly;
      if (!this.readOnly)
        return;
      this.nameTxt.ReadOnly = true;
      this.descTxt.ReadOnly = true;
      this.addBtn.Enabled = false;
      this.removeBtn.Enabled = false;
      this.stdBtnUp.Enabled = false;
      this.stdBtnDown.Enabled = false;
      this.saveBtn.Enabled = false;
      this.cancelBtn.Text = "Close";
      this.AcceptButton = (IButtonControl) this.cancelBtn;
    }

    public FormListDialog(FormTemplate formTemplate, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      InputFormInfo[] inputFormInfoArray = (InputFormInfo[]) null;
      try
      {
        inputFormInfoArray = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      }
      catch (Exception ex)
      {
        Tracing.Log(FormListDialog.sw, TraceLevel.Error, nameof (FormListDialog), "FormListDialog: Can't access Form List. Error: " + ex.Message);
      }
      this.gridSelected.Items.Clear();
      ArrayList arrayList = new ArrayList();
      int num1 = -1;
      int num2 = -1;
      XmlStringTable existingForms = formTemplate.GetExistingForms();
      for (int index = 0; index < existingForms.Count; ++index)
      {
        string empty = string.Empty;
        string str;
        try
        {
          str = existingForms[index.ToString()].ToString();
        }
        catch (Exception ex)
        {
          continue;
        }
        if (str != null && !(str == string.Empty) && !InputFormInfo.IsChildForm(str))
        {
          if (str.ToUpper() == "CREDIT DISCLOSURE")
            str = "FACT Act Disclosure";
          else if (str.ToUpper() == "SECTION 32 TIL")
            str = "Section 32 HOEPA";
          if (str == "-")
          {
            this.gridSelected.Items.Add(new GVItem("---- Line ----"));
          }
          else
          {
            this.gridSelected.Items.Add(new GVItem(str));
            arrayList.Add((object) str);
          }
          if (str.ToUpper() == "203K MAX MORTGAGE WS")
            num1 = this.gridSelected.Items.Count;
          else if (str.ToUpper() == "FHA MANAGEMENT")
            num2 = this.gridSelected.Items.Count;
        }
      }
      if (num1 > -1 && num2 == -1)
      {
        this.gridSelected.Items[num1 - 1].Text = "FHA Management";
        arrayList.Add((object) "FHA Management");
      }
      else if (num1 > -1 && num2 > -1)
        this.gridSelected.Items.RemoveAt(num1 - 1);
      if (arrayList.Contains((object) "203k Max Mortgage WS"))
        arrayList.Remove((object) "203k Max Mortgage WS");
      this.gridSelected_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridPredefined_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridPredefined.Items.Clear();
      if (inputFormInfoArray == null)
      {
        this.refreshSelectedCount();
        Tracing.Log(FormListDialog.sw, TraceLevel.Error, nameof (FormListDialog), "exception in loading 'FormTemplate' form list from InputFormList.");
      }
      else
      {
        string clientId = this.session.CompanyInfo.ClientID;
        foreach (InputFormInfo inputFormInfo in inputFormInfoArray)
        {
          if ((!(inputFormInfo.FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE) && (this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(inputFormInfo.FormID)) && !InputFormInfo.IsChildForm(inputFormInfo.Name) && !(inputFormInfo.FormID == "MAX23K") && !arrayList.Contains((object) inputFormInfo.Name))
            this.gridPredefined.Items.Add(inputFormInfo.Name);
        }
        this.gridPredefined.Items.Add(new GVItem("---- Line ----"));
        if (formTemplate != null)
        {
          this.nameTxt.Text = formTemplate.GetForm("DTNAME").Trim();
          this.descTxt.Text = formTemplate.Description;
        }
        this.formTemplate = formTemplate;
        this.refreshSelectedCount();
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.label2 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1 = new GroupContainer();
      this.gridPredefined = new GridView();
      this.groupContainerSelected = new GroupContainer();
      this.stdBtnDown = new StandardIconButton();
      this.stdBtnUp = new StandardIconButton();
      this.gridSelected = new GridView();
      this.groupContainer1.SuspendLayout();
      this.groupContainerSelected.SuspendLayout();
      ((ISupportInitialize) this.stdBtnDown).BeginInit();
      ((ISupportInitialize) this.stdBtnUp).BeginInit();
      this.SuspendLayout();
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.removeBtn.Location = new Point(249, 248);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 23);
      this.removeBtn.TabIndex = 3;
      this.removeBtn.Text = "< Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(249, 219);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 23);
      this.addBtn.TabIndex = 2;
      this.addBtn.Text = "Add >";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(94, 38);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(468, 66);
      this.descTxt.TabIndex = 0;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(94, 12);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(467, 20);
      this.nameTxt.TabIndex = 9;
      this.nameTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Template Name";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(487, 475);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(407, 475);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 7;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Input Form Sets";
      this.emHelpLink1.Location = new Point(12, 476);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.groupContainer1.Controls.Add((Control) this.gridPredefined);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(9, 110);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(235, 360);
      this.groupContainer1.TabIndex = 11;
      this.groupContainer1.Text = "Predefined Input Forms";
      this.gridPredefined.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 233;
      this.gridPredefined.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gridPredefined.Dock = DockStyle.Fill;
      this.gridPredefined.Location = new Point(1, 26);
      this.gridPredefined.Name = "gridPredefined";
      this.gridPredefined.Size = new Size(233, 333);
      this.gridPredefined.TabIndex = 0;
      this.gridPredefined.SelectedIndexChanged += new EventHandler(this.gridPredefined_SelectedIndexChanged);
      this.groupContainerSelected.Controls.Add((Control) this.stdBtnDown);
      this.groupContainerSelected.Controls.Add((Control) this.stdBtnUp);
      this.groupContainerSelected.Controls.Add((Control) this.gridSelected);
      this.groupContainerSelected.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerSelected.Location = new Point(326, 110);
      this.groupContainerSelected.Name = "groupContainerSelected";
      this.groupContainerSelected.Size = new Size(235, 356);
      this.groupContainerSelected.TabIndex = 12;
      this.groupContainerSelected.Text = "Selected Input Forms";
      this.stdBtnDown.BackColor = Color.Transparent;
      this.stdBtnDown.Location = new Point(213, 4);
      this.stdBtnDown.Name = "stdBtnDown";
      this.stdBtnDown.Size = new Size(16, 16);
      this.stdBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdBtnDown.TabIndex = 2;
      this.stdBtnDown.TabStop = false;
      this.stdBtnDown.Click += new EventHandler(this.downBtn_Click);
      this.stdBtnUp.BackColor = Color.Transparent;
      this.stdBtnUp.Location = new Point(190, 4);
      this.stdBtnUp.Name = "stdBtnUp";
      this.stdBtnUp.Size = new Size(16, 16);
      this.stdBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdBtnUp.TabIndex = 1;
      this.stdBtnUp.TabStop = false;
      this.stdBtnUp.Click += new EventHandler(this.upBtn_Click);
      this.gridSelected.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = 233;
      this.gridSelected.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gridSelected.Dock = DockStyle.Fill;
      this.gridSelected.Location = new Point(1, 26);
      this.gridSelected.Name = "gridSelected";
      this.gridSelected.Size = new Size(233, 329);
      this.gridSelected.TabIndex = 0;
      this.gridSelected.SelectedIndexChanged += new EventHandler(this.gridSelected_SelectedIndexChanged);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(574, 509);
      this.Controls.Add((Control) this.removeBtn);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.groupContainerSelected);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormListDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Input Form Set Details";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainerSelected.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnDown).EndInit();
      ((ISupportInitialize) this.stdBtnUp).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      string path = this.nameTxt.Text.Trim();
      if (path == string.Empty || path == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a Input Form Set template name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nameTxt.Focus();
      }
      else if (this.formTemplate != null && this.formTemplate.GetForm("DTNAME").ToUpper() != path.ToUpper() && this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.FormList, new FileSystemEntry(path, FileSystemEntry.Types.File, (string) null)))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The folder already contains a Input Form Set template named '" + path + "'. Please use different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nameTxt.Focus();
      }
      else
      {
        this.formTemplate.CleanFormList();
        this.formTemplate.AddForm("DTNAME", this.nameTxt.Text.Trim());
        this.formTemplate.Description = this.descTxt.Text.Trim();
        bool flag = false;
        int num1 = -1;
        for (int nItemIndex = 0; nItemIndex < this.gridSelected.Items.Count; ++nItemIndex)
        {
          string text = this.gridSelected.Items[nItemIndex].Text;
          if (!(text == "---- Line ----") || flag)
          {
            ++num1;
            if (text == "---- Line ----")
            {
              this.formTemplate.AddForm(num1.ToString(), "-");
            }
            else
            {
              flag = true;
              this.formTemplate.AddForm(num1.ToString(), text);
            }
          }
        }
        if (!flag)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select one form at least.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.DialogResult = DialogResult.OK;
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      bool flag = false;
      int num = 0;
      if (this.gridSelected.SelectedItems.Count > 0)
      {
        num = this.gridSelected.SelectedItems[this.gridSelected.SelectedItems.Count - 1].Index;
        flag = true;
        this.gridSelected.SelectedItems.Clear();
      }
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gridPredefined.SelectedItems)
        arrayList.Add((object) selectedItem);
      this.gridSelected.BeginUpdate();
      this.gridPredefined.BeginUpdate();
      foreach (GVItem gvItem1 in arrayList)
      {
        if (gvItem1.ToString() != "---- Line ----")
          this.gridPredefined.Items.Remove(gvItem1);
        GVItem gvItem2 = new GVItem(gvItem1.ToString());
        if (gvItem1.ToString() != "---- Line ----")
          gvItem2.Selected = true;
        if (!flag)
        {
          this.gridSelected.Items.Add(gvItem2);
        }
        else
        {
          if (num + 1 > this.gridSelected.Items.Count)
            this.gridSelected.Items.Add(gvItem2);
          else
            this.gridSelected.Items.Insert(num + 1, gvItem2);
          ++num;
        }
      }
      this.gridSelected.EndUpdate();
      this.gridPredefined.EndUpdate();
      this.refreshSelectedCount();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gridSelected.SelectedItems)
      {
        if (selectedItem.Text != "---- Line ----")
          arrayList.Add((object) selectedItem);
      }
      this.gridSelected.BeginUpdate();
      this.gridPredefined.BeginUpdate();
      foreach (GVItem gvItem in arrayList)
      {
        this.gridSelected.Items.Remove(gvItem);
        this.gridPredefined.Items.Add(new GVItem(gvItem.ToString()));
      }
      this.gridSelected.EndUpdate();
      this.gridPredefined.EndUpdate();
      if (this.gridSelected.Items.Count == 0)
      {
        this.refreshSelectedCount();
      }
      else
      {
        this.gridSelected.BeginUpdate();
        int num = this.gridSelected.Items.Count - 1;
        for (int nItemIndex = num; nItemIndex >= 0; --nItemIndex)
        {
          if ((nItemIndex == num || nItemIndex == 0) && this.gridSelected.Items[nItemIndex].Text == "---- Line ----")
            this.gridSelected.Items.RemoveAt(nItemIndex);
          else if (nItemIndex > 0 && this.gridSelected.Items[nItemIndex].Text == "---- Line ----" && this.gridSelected.Items[nItemIndex - 1].Text == "---- Line ----")
            this.gridSelected.Items.RemoveAt(nItemIndex);
        }
        this.gridSelected.EndUpdate();
        this.refreshSelectedCount();
      }
    }

    private void refreshSelectedCount()
    {
      int num = 0;
      for (int nItemIndex = 0; nItemIndex < this.gridSelected.Items.Count; ++nItemIndex)
        num += this.gridSelected.Items[nItemIndex].Text != "---- Line ----" ? 1 : 0;
      this.groupContainerSelected.Text = "Selected Input Forms (" + (object) num + ")";
      this.saveBtn.Enabled = !this.readOnly && num > 0;
    }

    private void upBtn_Click(object sender, EventArgs e)
    {
      this.gridSelected.BeginUpdate();
      for (int index1 = 0; index1 < this.gridSelected.SelectedItems.Count; ++index1)
      {
        int index2 = this.gridSelected.SelectedItems[index1].Index;
        int nItemIndex = index2 - 1;
        if (nItemIndex >= 0)
        {
          if (index1 > 0)
          {
            int index3 = this.gridSelected.SelectedItems[index1 - 1].Index;
            if (nItemIndex == index3)
              continue;
          }
          string text = this.gridSelected.Items[nItemIndex].Text;
          this.gridSelected.Items[nItemIndex].Text = this.gridSelected.Items[index2].Text;
          this.gridSelected.Items[index2].Text = text;
          this.gridSelected.Items[nItemIndex].Selected = true;
          this.gridSelected.Items[index2].Selected = false;
        }
      }
      this.gridSelected.EndUpdate();
    }

    private void downBtn_Click(object sender, EventArgs e)
    {
      this.gridSelected.BeginUpdate();
      for (int index1 = this.gridSelected.SelectedItems.Count - 1; index1 >= 0; --index1)
      {
        int index2 = this.gridSelected.SelectedItems[index1].Index;
        int nItemIndex = index2 + 1;
        if (nItemIndex < this.gridSelected.Items.Count)
        {
          if (index1 < this.gridSelected.SelectedItems.Count - 1)
          {
            int index3 = this.gridSelected.SelectedItems[index1 + 1].Index;
            if (nItemIndex == index3)
              continue;
          }
          string text = this.gridSelected.Items[nItemIndex].Text;
          this.gridSelected.Items[nItemIndex].Text = this.gridSelected.Items[index2].Text;
          this.gridSelected.Items[index2].Text = text;
          this.gridSelected.Items[nItemIndex].Selected = true;
          this.gridSelected.Items[index2].Selected = false;
        }
      }
      this.gridSelected.EndUpdate();
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if ("/:*?<>|.".IndexOf(e.KeyChar) == -1)
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('\\'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('"'))
          {
            e.Handled = false;
            return;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (FormListDialog));
    }

    private void gridSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      Button removeBtn = this.removeBtn;
      StandardIconButton stdBtnDown = this.stdBtnDown;
      bool flag1;
      this.stdBtnUp.Enabled = flag1 = !this.readOnly && this.gridSelected.SelectedItems.Count > 0;
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      stdBtnDown.Enabled = num1 != 0;
      int num2 = flag2 ? 1 : 0;
      removeBtn.Enabled = num2 != 0;
    }

    private void gridPredefined_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.addBtn.Enabled = !this.readOnly && this.gridPredefined.SelectedItems.Count > 0;
    }
  }
}
