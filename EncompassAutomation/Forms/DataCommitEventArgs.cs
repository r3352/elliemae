// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DataCommitEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Class representing the arguments for the DataCommit event
  /// </summary>
  /// <remarks>The <see cref="E:EllieMae.Encompass.Forms.FieldControl.DataCommit" /> event allows you to author custom code
  /// to manipulate the way in which data is saved from the control into the loan. Use this
  /// event in conjunction with the <see cref="E:EllieMae.Encompass.Forms.FieldControl.DataBind" /> event to perform
  /// completely custom data binding within Encompass.</remarks>
  public class DataCommitEventArgs : EventArgs
  {
    private string value = "";
    private bool cancel;

    /// <summary>Constructor for the DataCommitEventArgs object</summary>
    /// <param name="value">The value to be committed to the loan.</param>
    public DataCommitEventArgs(string value) => this.value = value;

    /// <summary>Gets or sets the value to be commited to the loan.</summary>
    /// <remarks>When the event is triggered, the Value property will be set to the value to
    /// be stored in the underlying loan using the <see cref="P:EllieMae.Encompass.Forms.FieldControl.Field" /> specified
    /// for the control. You can modify the value being saved by setting this property to
    /// a different value.
    /// <p>Alternatively, you could directly update the loan file and set the <see cref="P:EllieMae.Encompass.Forms.DataCommitEventArgs.Cancel" />
    /// property to <c>true</c>, essentially preventing the Encompass framework from saving any value
    /// into the loan for this field. This technique allows for completely customized data binding
    /// behavior.</p>
    /// </remarks>
    public string Value
    {
      get => this.value;
      set => this.value = value;
    }

    /// <summary>
    /// Gets or sets whether Encompass will commit the <see cref="P:EllieMae.Encompass.Forms.DataCommitEventArgs.Value" /> to the loan.
    /// </summary>
    /// <remarks>Set this property to <c>true</c> to prevent Encompass from saving the
    /// <see cref="P:EllieMae.Encompass.Forms.DataCommitEventArgs.Value" /> to the loan. This can be done if your field control's display
    /// does not correspond to the actual underlying field value and you want to completely
    /// control the manner in which data flows between the control and the loan file.</remarks>
    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
