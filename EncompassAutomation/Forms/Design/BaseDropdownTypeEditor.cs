// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.BaseDropdownTypeEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides a type editor which appears as a dropdown list in the property grid.
  /// </summary>
  /// <exclude />
  public abstract class BaseDropdownTypeEditor : UITypeEditor
  {
    private IWindowsFormsEditorService fEdSvc;

    /// <summary>Overrides the editor style to be dropdown.</summary>
    /// <param name="pContext">The current contol context.</param>
    /// <returns>Always returns UITypeEditorEditStyle.DropDown.</returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext pContext)
    {
      return UITypeEditorEditStyle.DropDown;
    }

    /// <summary>
    /// Method which draws the dropdown box on the property grid.
    /// </summary>
    /// <param name="pContext">The current editing context.</param>
    /// <param name="pProvider">The current host service propvider.</param>
    /// <param name="pValue">The value being edited.</param>
    /// <returns></returns>
    public override object EditValue(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
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

    /// <summary>
    /// Translates a value from an interval value to a user-visible value.
    /// </summary>
    /// <param name="value">The value to be transalted</param>
    /// <returns>Returns the transation of the value.</returns>
    protected virtual object TranslateValue(object value) => value;

    internal abstract void FillInList(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
      ListBox pListBox);
  }
}
