// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LockExtensionPriceAdjustment : IComparable
  {
    private string extensionNumber;
    private int daysToExtend;
    private Decimal priceAdjustment;

    public LockExtensionPriceAdjustment(
      string extensionNumber,
      int daysToExtend,
      Decimal priceAdjustment)
    {
      this.extensionNumber = extensionNumber;
      this.daysToExtend = daysToExtend;
      this.priceAdjustment = priceAdjustment;
    }

    public LockExtensionPriceAdjustment(int daysToExtend, Decimal priceAdjustment)
    {
      this.daysToExtend = daysToExtend;
      this.priceAdjustment = priceAdjustment;
    }

    public string ExtensionNumber
    {
      get => this.extensionNumber;
      set => this.extensionNumber = value;
    }

    public int DaysToExtend
    {
      get => this.daysToExtend;
      set => this.daysToExtend = value;
    }

    public Decimal PriceAdjustment
    {
      get => this.priceAdjustment;
      set => this.priceAdjustment = value;
    }

    public bool IsByExtOccurrence => !string.IsNullOrEmpty(this.extensionNumber);

    public override bool Equals(object obj)
    {
      if (!(obj is LockExtensionPriceAdjustment))
        return false;
      LockExtensionPriceAdjustment extensionPriceAdjustment = (LockExtensionPriceAdjustment) obj;
      return extensionPriceAdjustment.IsByExtOccurrence && this.IsByExtOccurrence && extensionPriceAdjustment.extensionNumber.Equals(this.extensionNumber, StringComparison.OrdinalIgnoreCase) || !extensionPriceAdjustment.IsByExtOccurrence && !this.IsByExtOccurrence && extensionPriceAdjustment.DaysToExtend == this.DaysToExtend;
    }

    public override int GetHashCode()
    {
      return this.IsByExtOccurrence ? this.GetExtensionNumber(this.extensionNumber) : this.daysToExtend;
    }

    public int CompareTo(object obj)
    {
      if (!(obj is LockExtensionPriceAdjustment))
        return 0;
      LockExtensionPriceAdjustment extensionPriceAdjustment = (LockExtensionPriceAdjustment) obj;
      if (extensionPriceAdjustment.IsByExtOccurrence && this.IsByExtOccurrence)
      {
        int extensionNumber1 = this.GetExtensionNumber(extensionPriceAdjustment.ExtensionNumber);
        int extensionNumber2 = this.GetExtensionNumber(this.ExtensionNumber);
        if (extensionNumber1 > extensionNumber2)
          return -1;
        return extensionNumber1 == extensionNumber2 ? 0 : 1;
      }
      if (extensionPriceAdjustment.DaysToExtend < this.DaysToExtend)
        return 2;
      return extensionPriceAdjustment.DaysToExtend == this.DaysToExtend ? 1 : -1;
    }

    public int GetExtensionNumber(string strExtNumber)
    {
      int num1 = strExtNumber.IndexOf("#");
      int num2;
      return int.Parse(strExtNumber.Substring(num2 = num1 + 1));
    }
  }
}
