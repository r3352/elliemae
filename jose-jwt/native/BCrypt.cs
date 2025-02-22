﻿// Decompiled with JetBrains decompiler
// Type: Jose.native.BCrypt
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Jose.native
{
  public static class BCrypt
  {
    public const uint ERROR_SUCCESS = 0;
    public const uint BCRYPT_PAD_PSS = 8;
    public const uint BCRYPT_PAD_OAEP = 4;
    public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);
    public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";
    public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";
    public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";
    public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";
    public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";
    public static readonly string BCRYPT_AES_ALGORITHM = "AES";
    public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";
    public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;
    public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;
    public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434;

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptOpenAlgorithmProvider(
      out IntPtr phAlgorithm,
      [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId,
      [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation,
      uint dwFlags);

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptGetProperty(
      IntPtr hObject,
      [MarshalAs(UnmanagedType.LPWStr)] string pszProperty,
      byte[] pbOutput,
      int cbOutput,
      ref int pcbResult,
      uint flags);

    [DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
    internal static extern uint BCryptSetAlgorithmProperty(
      IntPtr hObject,
      [MarshalAs(UnmanagedType.LPWStr)] string pszProperty,
      byte[] pbInput,
      int cbInput,
      int dwFlags);

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptImportKey(
      IntPtr hAlgorithm,
      IntPtr hImportKey,
      [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType,
      out IntPtr phKey,
      IntPtr pbKeyObject,
      int cbKeyObject,
      byte[] pbInput,
      int cbInput,
      uint dwFlags);

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptDestroyKey(IntPtr hKey);

    [DllImport("bcrypt.dll")]
    public static extern uint BCryptEncrypt(
      IntPtr hKey,
      byte[] pbInput,
      int cbInput,
      ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo,
      byte[] pbIV,
      int cbIV,
      byte[] pbOutput,
      int cbOutput,
      ref int pcbResult,
      uint dwFlags);

    [DllImport("bcrypt.dll")]
    internal static extern uint BCryptDecrypt(
      IntPtr hKey,
      byte[] pbInput,
      int cbInput,
      ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo,
      byte[] pbIV,
      int cbIV,
      byte[] pbOutput,
      int cbOutput,
      ref int pcbResult,
      int dwFlags);

    public struct BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
    {
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pszAlgId = pszAlgId;
      public int cbSalt = cbSalt;
    }

    public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
    {
      public int cbSize;
      public int dwInfoVersion;
      public IntPtr pbNonce;
      public int cbNonce;
      public IntPtr pbAuthData;
      public int cbAuthData;
      public IntPtr pbTag;
      public int cbTag;
      public IntPtr pbMacContext;
      public int cbMacContext;
      public int cbAAD;
      public long cbData;
      public int dwFlags;

      public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
        : this()
      {
        this.dwInfoVersion = BCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
        this.cbSize = Marshal.SizeOf(typeof (BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
        if (iv != null)
        {
          this.cbNonce = iv.Length;
          this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
          Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
        }
        if (aad != null)
        {
          this.cbAuthData = aad.Length;
          this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
          Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
        }
        if (tag == null)
          return;
        this.cbTag = tag.Length;
        this.pbTag = Marshal.AllocHGlobal(this.cbTag);
        Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
        this.cbMacContext = tag.Length;
        this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
      }

      public void Dispose()
      {
        if (this.pbNonce != IntPtr.Zero)
          Marshal.FreeHGlobal(this.pbNonce);
        if (this.pbTag != IntPtr.Zero)
          Marshal.FreeHGlobal(this.pbTag);
        if (this.pbAuthData != IntPtr.Zero)
          Marshal.FreeHGlobal(this.pbAuthData);
        if (!(this.pbMacContext != IntPtr.Zero))
          return;
        Marshal.FreeHGlobal(this.pbMacContext);
      }
    }

    public struct BCRYPT_KEY_LENGTHS_STRUCT
    {
      public int dwMinLength;
      public int dwMaxLength;
      public int dwIncrement;
    }

    public struct BCRYPT_OAEP_PADDING_INFO(string alg)
    {
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pszAlgId = alg;
      public IntPtr pbLabel = IntPtr.Zero;
      public int cbLabel = 0;
    }
  }
}
