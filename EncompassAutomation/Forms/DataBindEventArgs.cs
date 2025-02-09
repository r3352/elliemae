// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DataBindEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Class representing the arguments for the DataBind event
  /// </summary>
  /// <remarks>The <see cref="E:EllieMae.Encompass.Forms.FieldControl.DataBind" /> event allows you to author custom code
  /// to manipulate the way in which loan data is represented within your form's controls. Use this
  /// event in conjunction with the <see cref="E:EllieMae.Encompass.Forms.FieldControl.DataCommit" /> event to perform
  /// completely custom data binding within Encompass.</remarks>
  public class DataBindEventArgs : EventArgs
  {
    private string value;
    private bool cancel;

    /// <summary>Constructor for the DataBindEventArgs class</summary>
    /// <param name="value">The value to be bound to the control.</param>
    public DataBindEventArgs(string value) => this.value = value;

    /// <summary>Gets or sets the value being bound into the control.</summary>
    /// <remarks>Your code may access this property to determine the value which is about
    /// to be bound to the <see cref="T:EllieMae.Encompass.Forms.FieldControl" />. You can override this value by setting
    /// the property to the new value you wish to have bound. Doing this will not cause the
    /// value in the underlying loan to be updated.
    /// </remarks>
    public string Value
    {
      get => this.value;
      set => this.value = value;
    }

    /// <summary>
    /// Gets or sets whether the default data binding behavior should be cancelled.
    /// </summary>
    /// <remarks>Set this property to <c>true</c> to completely override the default
    /// data binding behavior for a <see cref="T:EllieMae.Encompass.Forms.FieldControl" />. When this property is true
    /// the Encompass framework will not attempt to bind the <see cref="P:EllieMae.Encompass.Forms.DataBindEventArgs.Value" /> to the control.
    /// Instead, you should perform your own binding logic.
    /// <p>Generally, if you set this property to <c>true</c> you will often need to handle the
    /// control's <see cref="E:EllieMae.Encompass.Forms.FieldControl.DataCommit" /> event as well to include custom code
    /// for commiting the value back to the loan.</p>
    /// </remarks>
    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
