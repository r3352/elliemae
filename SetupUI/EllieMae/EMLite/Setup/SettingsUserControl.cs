// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsUserControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsUserControl : UserControl
  {
    protected SetUpContainer setupContainer;
    private bool isDirty;
    private bool isValid = true;
    protected bool skipPromptForSave;
    protected SSFControl ssfControl = new SSFControl();
    private IContainer components;

    private SettingsUserControl()
    {
    }

    protected SettingsUserControl(SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.InitializeComponent();
    }

    public virtual void Save() => this.setDirtyFlag(false);

    public virtual bool SaveRtnBool()
    {
      this.Save();
      return true;
    }

    public virtual void Reset() => this.setDirtyFlag(false);

    public virtual bool IsDirty => this.isDirty;

    public virtual bool IsValid => this.isValid;

    protected virtual void setDirtyFlag(bool val)
    {
      if (this.setupContainer != null)
        this.setupContainer.ButtonSaveEnabled = this.setupContainer.ButtonResetEnabled = val;
      this.isDirty = val;
    }

    public TreeView TreeView { get; set; }

    public TreeNode Node { get; set; }

    public DialogResult PromptForSave()
    {
      bool flag = false;
      if (this.skipPromptForSave)
        return DialogResult.OK;
      SSFEventArgs<bool[]> eventArgs = new SSFEventArgs<bool[]>("module", "leave");
      if (this.ssfControl.RaiseEvent<bool[]>(eventArgs, 5000))
        flag = ((IEnumerable<bool>) eventArgs.EventFeedback).Last<bool>();
      return flag ? DialogResult.Cancel : DialogResult.OK;
    }

    protected void unloadHandler()
    {
      this.skipPromptForSave = true;
      if (!this.setupContainer.InvokeRequired)
        return;
      if (this.Node != null)
        this.setupContainer.Invoke((Delegate) (() => this.TreeView.SelectedNode = this.Node));
      else
        this.setupContainer.CloseForm();
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
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
