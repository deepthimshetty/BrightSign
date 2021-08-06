using System;

namespace OnTrack.Security.Cryptography
{
	public class CryptoService
	{
		private string mPassword = "";
		private byte[] mSalt = null;

		public CryptoService()
		{
			mSalt = Crypto.CreateSalt(16);
			mPassword = "password";
		}

		public byte[] EncryptAes(string data)
		{
			return Crypto.EncryptAes (data, mPassword, mSalt);
		}

		public string DecryptAes(byte[] data)
		{
			return Crypto.DecryptAes (data, mPassword, mSalt);
		}
	}
}

