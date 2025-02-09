// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpBusinessRuleFindFieldDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.InputEngine;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpBusinessRuleFindFieldDialog : 
    OperationBase,
    IOpBusinessRuleFindFieldDialog,
    IOperation,
    IDisposable
  {
    public OpFieldDialogResponse ShowDialog(
      OpBusinessRuleFindFieldDialogShowDialogRequest request)
    {
      OpFieldDialogResponse fieldDialogResponse = new OpFieldDialogResponse();
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(request.CommandContext.Session, (string[]) null, request.HideAccessRight, request.HelpTag, request.IsSingleSelection, request.EnableButtonSelection))
      {
        if (ruleFindFieldDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          fieldDialogResponse.DialogResult = ruleFindFieldDialog.DialogResult.ToString();
          if (ruleFindFieldDialog.SelectedRequiredFields.Length != 0)
            fieldDialogResponse.FieldId = ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1];
        }
      }
      return fieldDialogResponse;
    }
  }
}
