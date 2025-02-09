// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.StateNameValidator
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class StateNameValidator
  {
    private static Dictionary<string, string> stateFieldsAllowedForeignAddress = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase)
    {
      {
        "FR0107",
        "FR0129"
      },
      {
        "FR0207",
        "FR0229"
      },
      {
        "FR0307",
        "FR0329"
      },
      {
        "FR0407",
        "FR0429"
      },
      {
        "1418",
        "URLA.X267"
      },
      {
        "1521",
        "URLA.X268"
      }
    };

    public static bool ValidateState(string id, string val, InputHandlerBase input)
    {
      if (StateNameValidator.stateFieldsAllowedForeignAddress.ContainsKey(id) && input.GetField(StateNameValidator.stateFieldsAllowedForeignAddress[id]) == "Y")
        return true;
      if (val.Length < 2)
        return false;
      foreach (string state in Utils.GetStates())
      {
        if (val.ToUpper() == state)
          return true;
      }
      return false;
    }
  }
}
