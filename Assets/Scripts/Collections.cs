using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.Networking;

public static class Collections
{
    public static int NetworkCheck()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return 0;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return 1;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return 2;
        }
        else 
        {
            return -1;
        }
    }

    private static readonly string PASSWORD = "000000000000000000000000000000"; //temp password
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    // Encryption
    public static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();

        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    // Decryption
    public static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }

    public static string EncodeUTF8(string data)
    {
        byte [] bytesForEncoding = Encoding.UTF8.GetBytes(data) ;
        string encodedString = Convert.ToBase64String(bytesForEncoding);
        return encodedString;
    }

    public static string DecodeUTF8(string encodedString)
    {
        byte[] decodedBytes = Convert.FromBase64String(encodedString);
        string decodedString = Encoding.UTF8.GetString(decodedBytes);
        return decodedString;
    }

    public static string WWWEncode(string data)
    {
        return UnityWebRequest.EscapeURL(data);
    }
}