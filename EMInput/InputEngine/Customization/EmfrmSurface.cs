// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Customization.EmfrmSurface
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Customization
{
  public class EmfrmSurface : EmfrmControl, IControlSurface, ISurfaceElement
  {
    private ContainerControl controlContainer;

    public EmfrmSurface(ContainerControl controlContainer)
      : base((Control) controlContainer)
    {
      this.controlContainer = controlContainer;
    }

    public ISurfaceElement CreateLabel(Rectangle bounds, string text)
    {
      Label label = new Label();
      this.controlContainer.Controls.Insert((Control) label, bounds.Location);
      label.Text = text;
      int height = label.Size.Height;
      label.Overflow = Overflow.Ellipses;
      label.Size = new Size(bounds.Width, height);
      int num = (bounds.Height - label.Size.Height) / 2;
      if (num > 0)
        label.Top += num;
      return (ISurfaceElement) new EmfrmControl((Control) label);
    }

    public ISurfaceElement CreateField(Rectangle bounds, FieldDescriptor field)
    {
      FieldControl fieldControl = field.Format == LoanFieldFormat.X || field.Format == LoanFieldFormat.YN ? (FieldControl) new CheckBox() : (!field.Options.RequireValueFromList ? (field.Options.Count <= 0 ? (FieldControl) new TextBox() : (FieldControl) new DropdownEditBox()) : (FieldControl) new DropdownBox());
      this.controlContainer.Controls.Insert((Control) fieldControl, bounds.Location);
      fieldControl.Size = bounds.Size;
      if (field != null)
        fieldControl.Field = field;
      if (fieldControl is CheckBox)
        ((CheckBox) fieldControl).Text = "";
      return (ISurfaceElement) new EmfrmControl((Control) fieldControl);
    }

    public IControlSurface CreateSurface(Rectangle bounds)
    {
      Panel controlContainer = new Panel();
      this.controlContainer.Controls.Insert((Control) controlContainer, bounds.Location);
      controlContainer.Size = bounds.Size;
      controlContainer.BorderStyle = BorderStyle.None;
      return (IControlSurface) new EmfrmSurface((ContainerControl) controlContainer);
    }
  }
}
