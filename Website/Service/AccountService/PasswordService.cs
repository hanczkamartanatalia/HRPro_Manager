﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using System.Text;

namespace Website.Service.AccountService
{
    public static class PasswordService
    {
        public static string HashPassword(string _password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static void isCorrect(string _inputPass, string _dbPass)
        {
            string _inputPassHash = HashPassword(_inputPass);
            if(_dbPass != _inputPassHash) throw new Exception("Incorrect password.");
        }

       
    }
}
