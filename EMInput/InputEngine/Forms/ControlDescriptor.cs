// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.ControlDescriptor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;
using System;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class ControlDescriptor
  {
    private string controlId;
    private string controlTypeName;

    public ControlDescriptor(string controlId, string controlTypeName)
    {
      this.controlId = controlId ?? "";
      this.controlTypeName = controlTypeName ?? "";
      if (this.controlId == "")
        throw new ArgumentException("Invalid control ID specified");
      if (this.controlTypeName == "")
        throw new ArgumentException("Invalid control type specified");
    }

    public string ControlID => this.controlId;

    public string ControlTypeName => this.controlTypeName;

    public Type GetControlType()
    {
      return Assembly.GetAssembly(typeof (Form)).GetType(this.controlTypeName);
    }
  }
}
