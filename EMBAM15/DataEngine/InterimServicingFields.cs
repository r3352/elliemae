// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicingFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class InterimServicingFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static InterimServicingFields()
    {
      string empty = string.Empty;
      for (int payOrder = 0; payOrder <= 4; ++payOrder)
      {
        string str = payOrder == 0 ? "Most Recent" : "Previous " + (object) payOrder;
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "SDATE", "Interim Servicing - Statement Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "DUEDATE", "Interim Servicing - Due Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "LATEDATE", "Interim Servicing - Late Payment Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "RDATE", "Interim Servicing - Received Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "DEPODATE", "Interim Servicing - Deposited Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "INDEXRATE", "Interim Servicing - Index Rate (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_5));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "INTRATE", "Interim Servicing - Interest Rate (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_3));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "AMTDUE", "Interim Servicing - Total Amount Due (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "AMTRECE", "Interim Servicing - Total Amount Received (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "PRINCIPAL", "Interim Servicing - Principal Payment(" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "INTEREST", "Interim Servicing - Interest Payment (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ESCROW", "Interim Servicing - Escrow Payment (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "LATEFEE", "Interim Servicing - Late Fee (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "MISCFEE", "Interim Servicing - Misc. Fee (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ADDPRINCIPAL", "Interim Servicing - Additional Principal Payment (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ADDESCROW", "Interim Servicing - Additional Escrow Payment (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "PAYMETHOD", "Interim Servicing - Payment Method (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "INTNAME", "Interim Servicing - Institution Name (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ROUTINE", "Interim Servicing - Institution Routine (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ACCTNO", "Interim Servicing - Account Number (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "ACCTHOLDER", "Interim Servicing - Account Holder (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "AMOUNT", "Interim Servicing - Check/Transaction/Lock Box/Wire Amount (" + str + " Mortgage Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "CHKNO", "Interim Servicing - Check Number/Reference/ (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "CHKDATE", "Interim Servicing - Check/Transaction/Credited/Wired Date (" + str + " Mortgage Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Payment, payOrder, "COMMENTS", "Interim Servicing - Comments (" + str + " Mortgage Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "ESCROWTYPE", "Interim Servicing - Escrow Type (" + str + " Escrow Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "INTNAME", "Interim Servicing - Institution Name (" + str + " Escrow Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "DUEDATE", "Interim Servicing - Due Date (" + str + " Escrow Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "DATE", "Interim Servicing - Disbursement Date (" + str + " Escrow Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "AMT", "Interim Servicing - Disbursement Amount (" + str + " Escrow Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Escrow, payOrder, "COMMENTS", "Interim Servicing - Comments (" + str + " Escrow Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Interest, payOrder, "DATE", "Interim Servicing - Interest Incurred Date (" + str + " Escrow Interest Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Interest, payOrder, "AMT", "Interim Servicing - Interest Amount (" + str + " Escrow Interest Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Interest, payOrder, "COMMENTS", "Interim Servicing - Comments (" + str + " Escrow Interest Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "DATE", "Interim Servicing - Date (" + str + " Other Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "AMT", "Interim Servicing - Amount (" + str + " Other Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "INTNAME", "Interim Servicing - Institution Name (" + str + " Other Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "ROUTINE", "Interim Servicing - Institution Routine (" + str + " Other Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "ACCTNO", "Interim Servicing - Account Number (" + str + " Other Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "REFNO", "Interim Servicing - Reference Number (" + str + " Other Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, payOrder, "COMMENTS", "Interim Servicing - Comments (" + str + " Other Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "TYPE", "Interim Servicing - Type (" + str + " Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "DATE", "Interim Servicing - Date (" + str + " Payment)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "AMT", "Interim Servicing - Amount (" + str + " Payment)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "CREATEDBY", "Interim Servicing - Created By (" + str + " Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "CREATEDDATETIME", "Interim Servicing - Created Date Time (" + str + " Payment)", FieldFormat.DATETIME));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "MODIFIEDBY", "Interim Servicing - Modified By (" + str + " Payment)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.All, payOrder, "MODIFIEDDATETIME", "Interim Servicing - Modified Date Time (" + str + " Payment)", FieldFormat.DATETIME));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Principal, payOrder, "AMT", "Interim Servicing - Principal Disbursement Amount (" + str + " Principal Disbursement)", FieldFormat.DECIMAL_2));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Principal, payOrder, "COMMENTS", "Interim Servicing - Principal Disbursement Comments (" + str + " Principal Disbursement)", FieldFormat.STRING));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Principal, payOrder, "DATE", "Interim Servicing - Principal Disbursement Date (" + str + " Principal Disbursement)", FieldFormat.DATE));
        InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Principal, payOrder, "INTNAME", "Interim Servicing - Institution Name (" + str + " Principal Disbursement)", FieldFormat.STRING));
      }
      InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, "NEXTDISBURSEMENTDATE", "Interim Servicing - Next Disbursement Date", FieldFormat.DATE));
      InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, "NEXTDISBURSEMENTTYPE", "Interim Servicing - Next Disbursement Type", FieldFormat.STRING));
      InterimServicingFields.All.Add((FieldDefinition) new InterimServicingField(InterimServicingProperty.Other, "NEXTDISBURSEMENTAMOUNT", "Interim Servicing - Next Disbursement Amount", FieldFormat.DECIMAL_2));
    }
  }
}
