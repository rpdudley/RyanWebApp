using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RyanWebApp.Server.Services
{
    public class CipherService
    {
        private readonly int _transpositionKey;
        private readonly string _vigenereKeyword;

        public CipherService(int transpositionKey, string vigenereKeyword)
        {
            _transpositionKey = transpositionKey;
            _vigenereKeyword = vigenereKeyword;
        }

        // Transposition Encrypt
        public string TranspositionEncrypt(string text)
        {
            var encryptedText = new string[_transpositionKey];

            for (int i = 0; i < _transpositionKey; i++)
            {
                int currentIdx = i;
                while (currentIdx < text.Length)
                {
                    encryptedText[i] += text[currentIdx];
                    currentIdx += _transpositionKey;
                }
            }

            return string.Join("", encryptedText);
        }

        // Transposition Decrypt
        public string TranspositionDecrypt(string text)
        {
            int numColumns = (text.Length + _transpositionKey - 1) / _transpositionKey;
            int numRows = _transpositionKey;
            int numShadedBoxes = (numColumns * numRows) - text.Length;
            var plaintext = new string[numColumns];
            int col = 0;
            int row = 0;

            foreach (var symbol in text)
            {
                plaintext[col] += symbol;
                col++;
                if (col == numColumns || (col == numColumns - 1 && row >= numRows - numShadedBoxes))
                {
                    col = 0;
                    row++;
                }
            }

            return string.Join("", plaintext);
        }

        public string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[32];
                rng.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            }
        }
        // Vigenere Encrypt
        public string VigenereEncrypt(string plainText)
        {
            var keywordRepeated = RepeatKeyword(plainText, _vigenereKeyword);
            return new string(plainText.Zip(keywordRepeated, (p, k) =>
            {
                if (!char.IsLetter(p)) return p;
                int shift = k - 'A';
                return (char)((p - 'A' + shift) % 26 + 'A');
            }).ToArray());
        }

        // Vigenere Decrypt
        public string VigenereDecrypt(string cipherText)
        {
            var keywordRepeated = RepeatKeyword(cipherText, _vigenereKeyword);
            return new string(cipherText.Zip(keywordRepeated, (c, k) =>
            {
                if (!char.IsLetter(c)) return c;
                int shift = k - 'A';
                return (char)((c - 'A' - shift + 26) % 26 + 'A');
            }).ToArray());
        }

        // Helper method to repeat the Vigenere keyword to match the length of the text
        private string RepeatKeyword(string text, string keyword)
        {
            var repeatedKeyword = new StringBuilder(text.Length);
            int keywordIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    repeatedKeyword.Append(keyword[keywordIndex % keyword.Length]);
                    keywordIndex++;
                }
                else
                {
                    repeatedKeyword.Append(c);
                }
            }

            return repeatedKeyword.ToString();
        }
    }
}
