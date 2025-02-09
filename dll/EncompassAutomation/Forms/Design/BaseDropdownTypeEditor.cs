// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.BaseDropdownTypeEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public abstract class BaseDropdownTypeEditor : UITypeEditor
  {
    private IWindowsFormsEditorService fEdSvc;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext pContext)
    {
      return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      object pValue)
    {
      if (pContext != null && pContext.Instance != null)
      {
        if (pProvider != null)
        {
          try
          {
            this.fEdSvc = (IWindowsFormsEditorService) pProvider.GetService(typeof (IWindowsFormsEditorService));
            ListBox pListBox = new ListBox();
            pListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            pListBox.Click += new EventHandler(this.List_Click);
            this.FillInList(pContext, pProvider, pListBox);
            pListBox.SelectedItem = pValue;
            this.fEdSvc.DropDownControl((System.Windows.Forms.Control) pListBox);
            return this.TranslateValue(pListBox.SelectedItem);
          }
          finally
          {
            this.fEdSvc = (IWindowsFormsEditorService) null;
          }
        }
      }
      return pValue;
    }

    internal void List_Click(object pSender, EventArgs pArgs) => this.fEdSvc.CloseDropDown();

    protected virtual object TranslateValue(object value) => value;

    internal abstract void FillInList(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      ListBox pListBox);
  }
}
