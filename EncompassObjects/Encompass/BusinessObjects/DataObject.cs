// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.DataObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  /// <summary>Represents a generic data object.</summary>
  /// <remarks>This object can be a binary or text data object.</remarks>
  public class DataObject : IDataObject, IDisposable
  {
    private BinaryObject data;
    private byte[] byteData;

    internal DataObject(BinaryObject data) => this.data = data;

    /// <summary>Creates an empty DataObject.</summary>
    public DataObject() => this.data = new BinaryObject(new byte[0]);

    /// <summary>Constructor to create a DataObject from a byte array.</summary>
    /// <param name="data"></param>
    public DataObject(byte[] data) => this.data = new BinaryObject(data);

    /// <summary>Creates a new DataObject from a file on disk.</summary>
    /// <param name="filePath"></param>
    public DataObject(string filePath) => this.data = new BinaryObject(filePath);

    /// <summary>
    /// Creates a new DataObject by reading in the data from a Stream.
    /// </summary>
    /// <param name="stream">The stream to be read. The data will be read until the end
    /// of the stream is reached.</param>
    public DataObject(Stream stream) => this.data = new BinaryObject(stream, true);

    /// <summary>Gets the size of the object in bytes.</summary>
    public int Size => (int) this.data.Length;

    /// <summary>Gets the udnerlying byte data from the object.</summary>
    public byte[] Data
    {
      get
      {
        if (this.byteData == null)
          this.byteData = this.data.GetBytes();
        return this.byteData;
      }
    }

    /// <summary>
    /// Converts the data object to a string using the specified character encoding.
    /// </summary>
    /// <param name="encoding">The character encoding to be used for converting the binary object to text.</param>
    /// <returns></returns>
    public string ToString(Encoding encoding) => this.data.ToString(encoding);

    /// <summary>Returns an open stream for the data object.</summary>
    /// <returns>An open stream set to the first byte of the object.</returns>
    public Stream OpenStream() => this.data.AsStream();

    /// <summary>Saves the object to the specified file location.</summary>
    /// <param name="filePath">The path of the file to be written.</param>
    public void SaveToDisk(string filePath) => this.data.Write(filePath);

    /// <summary>Loads a new data array into the object.</summary>
    /// <param name="data">The byte data to load.</param>
    public void Load(byte[] data) => this.data = new BinaryObject(data);

    /// <summary>Loads the object from a file on disk.</summary>
    /// <param name="filePath">The path of the source file.</param>
    public void LoadFile(string filePath) => this.data = new BinaryObject(filePath);

    internal BinaryObject Unwrap() => this.data;

    /// <summary>Disposes the data object</summary>
    public void Dispose()
    {
      if (this.data == null)
        return;
      this.data.Dispose();
      this.data = (BinaryObject) null;
    }
  }
}
