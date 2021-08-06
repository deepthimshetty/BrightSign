using System;

namespace OTEncrypt
{
	public class CryptoService
	{
		private string mPassword = "";
		private byte[] mSalt = null;

		public CryptoService ()
		{
			mSalt = GetBytes ("k\\xx2\\xx2\\xx2\\xx2G\\xx2\\xx2~\\xx2{\\xx"); // Crypto.CreateSalt(16);
			mPassword = "password";
		}

		static byte[] GetBytes (string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public byte[] EncryptAes (string data)
		{
			return Crypto.EncryptAes (data, mPassword, mSalt);
		}

		public string DecryptAes (byte[] data)
		{
			return Crypto.DecryptAes (data, mPassword, mSalt);
		}
	}
}

