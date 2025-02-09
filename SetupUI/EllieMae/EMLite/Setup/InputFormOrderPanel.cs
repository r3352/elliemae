// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputFormOrderPanel
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InputFormOrderPanel : SettingsUserControl
  {
    private Sessions.Session session;
    private const string className = "InputFormOrderPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IContainer components;
    private GridView listViewForms;
    private Button defaultBtn;
    private Button notDefaultBtn;
    private GroupContainer gcInputForms;
    private StandardIconButton stdIconBtnDown;
    private StandardIconButton stdIconBtnUp;
    private ToolTip toolTip1;
    private VerticalSeparator verticalSeparator2;
    private InputFormInfo[] formInfo;
    private InputFormInfo vaForm;
    private Dictionary<string, InputFormInfo> urlaForms;
    private bool isSettingsSync;

    public InputFormOrderPanel(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public InputFormOrderPanel(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool isSettingsSync)
      : base(setupContainer)
    {
      this.session = session;
      this.isSettingsSync = isSettingsSync;
      this.InitializeComponent();
      this.Reset();
      this.listViewForms_SelectedIndexChanged((object) this, (EventArgs) null);
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
      this.notDefaultBtn = new Button();
      this.defaultBtn = new Button();
      this.listViewForms = new GridView();
      this.gcInputForms = new GroupContainer();
      this.verticalSeparator2 = new VerticalSeparator();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcInputForms.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      this.SuspendLayout();
      this.notDefaultBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.notDefaultBtn.BackColor = SystemColors.Control;
      this.notDefaultBtn.Location = new Point(580, 2);
      this.notDefaultBtn.Name = "notDefaultBtn";
      this.notDefaultBtn.Size = new Size(77, 22);
      this.notDefaultBtn.TabIndex = 4;
      this.notDefaultBtn.Text = "&Not Default";
      this.notDefaultBtn.UseVisualStyleBackColor = true;
      this.notDefaultBtn.Click += new EventHandler(this.notDefaultBtn_Click);
      this.defaultBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.defaultBtn.BackColor = SystemColors.Control;
      this.defaultBtn.Location = new Point(503, 2);
      this.defaultBtn.Name = "defaultBtn";
      this.defaultBtn.Size = new Size(75, 22);
      this.defaultBtn.TabIndex = 3;
      this.defaultBtn.Text = "D&efault";
      this.defaultBtn.UseVisualStyleBackColor = true;
      this.defaultBtn.Click += new EventHandler(this.defaultBtn_Click);
      this.listViewForms.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 242;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Default List";
      gvColumn2.Width = 73;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Order";
      gvColumn3.Width = 0;
      this.listViewForms.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewForms.Dock = DockStyle.Fill;
      this.listViewForms.Location = new Point(1, 26);
      this.listViewForms.Name = "listViewForms";
      this.listViewForms.Size = new Size(660, 501);
      this.listViewForms.TabIndex = 19;
      this.listViewForms.SelectedIndexChanged += new EventHandler(this.listViewForms_SelectedIndexChanged);
      this.gcInputForms.Controls.Add((Control) this.verticalSeparator2);
      this.gcInputForms.Controls.Add((Control) this.stdIconBtnDown);
      this.gcInputForms.Controls.Add((Control) this.stdIconBtnUp);
      this.gcInputForms.Controls.Add((Control) this.notDefaultBtn);
      this.gcInputForms.Controls.Add((Control) this.listViewForms);
      this.gcInputForms.Controls.Add((Control) this.defaultBtn);
      this.gcInputForms.Dock = DockStyle.Fill;
      this.gcInputForms.Location = new Point(0, 0);
      this.gcInputForms.Name = "gcInputForms";
      this.gcInputForms.Size = new Size(662, 528);
      this.gcInputForms.TabIndex = 8;
      this.gcInputForms.Text = "Input Forms (#)   Default (#)";
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(497, 5);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 25;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Location = new Point(476, 5);
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 23;
      this.stdIconBtnDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDown, "Move Down");
      this.stdIconBtnDown.Click += new EventHandler(this.downBtn_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Location = new Point(455, 5);
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 22;
      this.stdIconBtnUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUp, "Move Up");
      this.stdIconBtnUp.Click += new EventHandler(this.upBtn_Click);
      this.Controls.Add((Control) this.gcInputForms);
      this.Name = nameof (InputFormOrderPanel);
      this.Size = new Size(662, 528);
      this.gcInputForms.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      this.ResumeLayout(false);
    }

    private void setTitle()
    {
      int num = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewForms.Items)
      {
        if (gvItem.SubItems[1].Text == "Yes")
          ++num;
      }
      this.gcInputForms.Text = "Input Forms (" + (object) this.listViewForms.Items.Count + ")    Default (" + (object) num + ")";
    }

    public override void Reset()
    {
      this.listViewForms.Items.Clear();
      try
      {
        this.formInfo = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputFormOrderPanel.sw, TraceLevel.Error, nameof (InputFormOrderPanel), "initForm: Can't access Form List. Error: " + ex.Message);
        return;
      }
      string clientId = this.session.CompanyInfo.ClientID;
      for (int index = 0; index < this.formInfo.Length; ++index)
      {
        if (this.formInfo[index].Category == InputFormCategory.Form && !(this.formInfo[index].FormID == "MAX23K"))
        {
          if (!this.session.StartupInfo.AllowURLA2020 && ShipInDarkValidation.IsURLA2020Form(this.formInfo[index].FormID))
          {
            if (this.urlaForms == null)
              this.urlaForms = new Dictionary<string, InputFormInfo>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
            if (!this.urlaForms.ContainsKey(this.formInfo[index].FormID))
              this.urlaForms.Add(this.formInfo[index].FormID, this.formInfo[index]);
          }
          else if ((!(this.formInfo[index].FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE) && !InputFormInfo.IsChildForm(this.formInfo[index].FormID))
            this.listViewForms.Items.Add(new GVItem(this.formInfo[index].Name)
            {
              SubItems = {
                this.formInfo[index].IsDefault ? (object) "Yes" : (object) "No",
                (object) index.ToString()
              },
              Tag = (object) this.formInfo[index]
            });
        }
      }
      this.setTitle();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      InputFormInfo inputFormInfo = (InputFormInfo) null;
      List<InputFormInfo> inputFormInfoList = new List<InputFormInfo>();
      for (int nItemIndex = 0; nItemIndex < this.listViewForms.Items.Count; ++nItemIndex)
      {
        inputFormInfo = (InputFormInfo) this.listViewForms.Items[nItemIndex].Tag;
        inputFormInfoList.Add(inputFormInfo);
        if (inputFormInfo.FormID == "FHAPROCESSMGT" && this.vaForm != (InputFormInfo) null)
          inputFormInfoList.Add(this.vaForm);
      }
      if (!this.session.StartupInfo.AllowURLA2020 && this.urlaForms != null && this.urlaForms.Count > 0)
      {
        int num = inputFormInfoList.Count - 1;
        List<string> stringList = new List<string>();
        for (int index = num; index >= 0; --index)
        {
          string newUrlaFormId = ShipInDarkValidation.GetNewURLAFormID(inputFormInfo.FormID);
          if (newUrlaFormId != null && this.urlaForms.ContainsKey(newUrlaFormId))
          {
            inputFormInfoList.Add(this.urlaForms[newUrlaFormId]);
            stringList.Add(newUrlaFormId);
          }
        }
        foreach (KeyValuePair<string, InputFormInfo> urlaForm in this.urlaForms)
        {
          if (!stringList.Contains(urlaForm.Key))
            inputFormInfoList.Add(urlaForm.Value);
        }
      }
      InputFormInfo[] array = inputFormInfoList.ToArray();
      try
      {
        if (array.Length != 0)
          this.session.FormManager.SetFormOrder(array);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputFormOrderPanel.sw, TraceLevel.Error, nameof (InputFormOrderPanel), "saveBtn_Click: Can't save form order. Error: " + ex.Message);
      }
      this.setDirtyFlag(false);
    }

    private void upBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewForms.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a form from the list to move up.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        for (int index1 = 0; index1 < this.listViewForms.SelectedItems.Count; ++index1)
        {
          int index2 = this.listViewForms.SelectedItems[index1].Index;
          int num2 = index2 - 1;
          if (num2 >= 0)
          {
            if (index1 > 0)
            {
              int index3 = this.listViewForms.SelectedItems[index1 - 1].Index;
              if (num2 == index3)
                continue;
            }
            GVItem gvItem = this.listViewForms.Items[index2];
            this.listViewForms.Items.RemoveAt(index2);
            this.listViewForms.Items.Insert(num2, gvItem);
            this.listViewForms.Items[num2].Selected = true;
          }
        }
        this.setDirtyFlag(true);
      }
    }

    private void downBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewForms.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a form from the list to move down.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        for (int index1 = this.listViewForms.SelectedItems.Count - 1; index1 >= 0; --index1)
        {
          int index2 = this.listViewForms.SelectedItems[index1].Index;
          int num2 = index2 + 1;
          if (num2 < this.listViewForms.Items.Count)
          {
            if (index1 < this.listViewForms.SelectedItems.Count - 1)
            {
              int index3 = this.listViewForms.SelectedItems[index1 + 1].Index;
              if (num2 == index3)
                continue;
            }
            GVItem gvItem = this.listViewForms.Items[index2];
            this.listViewForms.Items.RemoveAt(index2);
            this.listViewForms.Items.Insert(num2, gvItem);
            this.listViewForms.Items[num2].Selected = true;
          }
        }
        this.setDirtyFlag(true);
      }
    }

    private void defaultBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewForms.SelectedItems.Count == 0)
        return;
      this.setDefault(true);
    }

    private void notDefaultBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewForms.SelectedItems.Count == 0)
        return;
      this.setDefault(false);
    }

    private void setDefault(bool setToDefault)
    {
      for (int index1 = 0; index1 < this.listViewForms.SelectedItems.Count; ++index1)
      {
        int index2 = this.listViewForms.SelectedItems[index1].Index;
        ((InputFormInfo) this.listViewForms.Items[index2].Tag).IsDefault = setToDefault;
        this.listViewForms.Items[index2].SubItems[1].Text = setToDefault ? "Yes" : "No";
      }
      this.setTitle();
      this.setDirtyFlag(true);
    }

    private void listViewForms_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        this.stdIconBtnUp.Enabled = this.stdIconBtnDown.Enabled = this.defaultBtn.Enabled = this.notDefaultBtn.Enabled = false;
      else
        this.stdIconBtnUp.Enabled = this.stdIconBtnDown.Enabled = this.defaultBtn.Enabled = this.notDefaultBtn.Enabled = this.listViewForms.SelectedItems.Count > 0;
    }

    public string[] SelectedInputForms
    {
      get
      {
        return this.listViewForms.SelectedItems.Count == 0 ? (string[]) null : this.listViewForms.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => ((InputFormInfo) item.Tag).FormID)).ToArray<string>();
      }
    }

    public void SetSelectedInputForms(List<string> selectedInputForms)
    {
      for (int index = 0; index < selectedInputForms.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.listViewForms.Items.Count; ++nItemIndex)
        {
          if (((InputFormInfo) this.listViewForms.Items[nItemIndex].Tag).FormID == selectedInputForms[index])
          {
            this.listViewForms.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }
  }
}
