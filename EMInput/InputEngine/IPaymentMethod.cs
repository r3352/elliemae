// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IPaymentMethod
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public interface IPaymentMethod
  {
    void UpdateAmount(double amount);

    void GetFieldValues(Hashtable fieldTable);
  }
}
