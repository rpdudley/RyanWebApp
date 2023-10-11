using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace RyanWebApp.Server.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CipherController : Controller
    {
        private readonly CipherService _cipherService;

        public CipherController()
        {
            _cipherService = new CipherService(4, "KEY"); // Example values for transposition_key and vigenere_keyword
        }

        [HttpGet("generate-token")]
        public ActionResult<string> GenerateToken()
        {
            return Ok(_cipherService.GenerateToken());
        }

        [HttpPost("encrypt")]
        public ActionResult<string> Encrypt([FromBody] string message)
        {
            var encryptedMessage = _cipherService.Encrypt(message);
            return Ok(encryptedMessage);
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt(string encryptedMessage, string token)
        {
            try
            {
                var decryptedMessage = _cipherService.Decrypt(encryptedMessage, token);
                return Ok(decryptedMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class CipherService
    {
        private readonly int _transpositionKey;
        private readonly string _vigenereKeyword;

        public CipherService(int transpositionKey, string vigenereKeyword)
        {
            _transpositionKey = transpositionKey;
            _vigenereKeyword = vigenereKeyword;
        }

        public string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[16];
                rng.GetBytes(randomBytes);
                string token = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
                return token;
            }
        }

        public string Encrypt(string message)
        {
            // TODO: Implement transposition and Vigenere encryption here
            // This is a placeholder
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        public string Decrypt(string encryptedMessage, string token)
        {
            // TODO: Implement transposition and Vigenere decryption here using the token
            // This is a placeholder
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedMessage));
        }

        
    }

}
