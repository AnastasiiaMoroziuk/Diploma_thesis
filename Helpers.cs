using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Methods_for_controlling_integrity_of_delegated
{
    class Helpers
    {
        public BigInteger TWO = new BigInteger(2);
        public BigInteger THREE = new BigInteger(3);
        public BigInteger Generate(int byteLength)
        {
            Random rnd = new Random();
            byte[] seed = new byte[byteLength];
            byte[] zeros = new byte[byteLength];
            Array.Fill(zeros, (byte)0);
            rnd.NextBytes(seed);
            if (seed.SequenceEqual(zeros))
            {
                seed[seed.Length - 1] = (byte)1;
            }

            return BigInteger.Abs(new BigInteger(seed));
        }


        public BigInteger GenerateBigInteger(BigInteger max)
        {
            Random rnd = new Random();
            byte[] maxBytes = max.ToByteArray(true, false);
            byte[] seedBytes = new byte[maxBytes.Length];

            rnd.NextBytes(seedBytes);
            seedBytes[seedBytes.Length - 1] &= (byte)0x7F;
            var seed = new BigInteger(seedBytes);

            while (seed >= max || seed < TWO)
            {
                rnd.NextBytes(seedBytes);
                seedBytes[seedBytes.Length - 1] &= (byte)0x7F;
                seed = new BigInteger(seedBytes);
            }

            return seed;
        }

        public bool MillerRabinTest(BigInteger num, int k = 30) //робили по псевдокоду з вікіпедії
        {
            if (num == TWO || num == THREE)
            {
                return true;
            }
            if (num < TWO || num % TWO == BigInteger.Zero)
            {
                return false;
            }

            BigInteger d = num - BigInteger.One;
            int s = 0;

            while (d % TWO == BigInteger.Zero)
            {
                d /= TWO;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                var a = GenerateBigInteger(num - TWO);
                var x = BigInteger.ModPow(a, d, num);
                if (x == BigInteger.One || x == num - BigInteger.One)
                {
                    continue;
                }
                for (int j = 0; j < s; j++)
                {
                    x = BigInteger.ModPow(x, TWO, num);
                    if (x == BigInteger.One)
                    {
                        return false;
                    }
                    if (x == num - BigInteger.One)
                    {
                        break;
                    }
                }
                if (x != num - BigInteger.One)
                {
                    return false;
                }
            }

            return true;
        }

        /* Prime numbers generator */
        public byte[] GenerateRandomByteSeed(int size)
        {
            Random rnd = new Random();
            byte[] seed = new byte[size];
            byte[] zeros = new byte[size];
            Array.Fill(zeros, (byte)0);
            rnd.NextBytes(seed);
            if (seed.SequenceEqual(zeros))
            {
                seed[seed.Length - 1] = (byte)1;
            }
            return seed;
        }

        public BigInteger GeneratePrime(int byteLength)
        {
            var bytes = GenerateRandomByteSeed(byteLength);
            var num = BigInteger.Abs(new BigInteger(bytes));
            while (!MillerRabinTest(num))
            {
                bytes = GenerateRandomByteSeed(byteLength);
                num = new BigInteger(bytes);
            }

            return num;
        }

        public BigInteger GenerateFromMultipicativeGroup(BigInteger n)
        {
            var num = GeneratePrime(n.ToByteArray().Length);
            while (BigInteger.GreatestCommonDivisor(num, n) != 1 || num>=n)
            {
                num = GeneratePrime(n.ToByteArray().Length);
            }

            //while (BigInteger.ModPow(num, phi, n) != 1)
            //{
            //    num = GenerateBigInteger(n);
            //}

            return num;
        }
        public BigInteger GenerateG(BigInteger exp, BigInteger n)
        {
            var len = new Random().Next(1, n.ToByteArray().Length - 1);
            var g = this.Generate(len);
            while (!(BigInteger.ModPow(g, exp, n) != BigInteger.One && BigInteger.GreatestCommonDivisor(g, n)==BigInteger.One))
            {
                g = this.Generate(len);
            }

            return g;
        }

        static BigInteger Inverse(BigInteger num, BigInteger mod)
        {
            BigInteger q, r, t, u1 = BigInteger.One, u2 = BigInteger.Zero, v1 = BigInteger.Zero, v2 = BigInteger.One,
                        a = num, b = mod;
            while (b != BigInteger.Zero)
            {
                q = a / b;
                r = a % b;
                a = b; b = r;
                t = u2;
                u2 = u1 - q * u2;
                u1 = t;
                t = v2;
                v2 = v1 - q * v2;
                v1 = t;
            }
            if (u1 < BigInteger.Zero)
            {
                u1 += mod;
            }
            return u1;
        }

    }
}
