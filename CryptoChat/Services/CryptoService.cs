namespace CryptoChat.Services
{
    using System.Security.Cryptography;
    using System.Text;

    public class CryptoService
    {
        private readonly byte[] _key; // Pre-shared key (32 bytes for AES-256)
        private readonly byte[] _iv;  // Initialization vector (16 bytes)

        public CryptoService(string key)
        {
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(key)); // Hash the input to ensure 32 bytes
            _iv = _key.Take(16).ToArray(); // Take first 16 bytes for IV
        }

        public (string encrypted, string hmac) Encrypt(string message)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(message);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }
                encryptedBytes = ms.ToArray();
            }

            string encrypted = Convert.ToBase64String(encryptedBytes);
            string hmac = ComputeHmac(encrypted);
            return (encrypted, hmac);
        }

        public string Decrypt(string encrypted, string hmac)
        {
            if (ComputeHmac(encrypted) != hmac)
                throw new Exception("Message integrity check failed");

            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private string ComputeHmac(string data)
        {
            using var hmac = new HMACSHA256(_key);
            byte[] hmacBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hmacBytes);
        }
    }
}
