using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services
{
    internal class GenerateCapthcaText
    {
        private static readonly Random random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string Generate_CapthcaText(int Lenght)
        {
            if (Lenght <= 0)
            {
                throw new ArgumentException("Длина текса капчи должна быть больше нуля");
            }
            StringBuilder sb = new StringBuilder(Lenght);
            for (int i = 0; i < Lenght; i++) 
            {
                int index = random.Next(Characters.Length);
                sb.Append(Characters[index]);
            }
            return sb.ToString();
        }
    }
}
