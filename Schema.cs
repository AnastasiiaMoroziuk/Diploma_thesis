using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Methods_for_controlling_integrity_of_delegated
{
    class Schema
    {
        
        public Helpers helper = new Helpers();
        private BigInteger q;
        private BigInteger p;
        private int safetyParam;
        private List<BigInteger> usedStates = new List<BigInteger>();
        public Dictionary<string, BigInteger> publicKey = new Dictionary<string, BigInteger>()
        {
            { "n", BigInteger.Zero }
        };

        private Dictionary<string, BigInteger> secretKey = new Dictionary<string, BigInteger>()
        {
            {"g1", BigInteger.Zero },
            {"g2", BigInteger.Zero },
            {"r", BigInteger.Zero },
            {"c", BigInteger.Zero }
        };

        public BigInteger phi;

        public void setSafetyParam(int byteLength)
        {
            safetyParam = byteLength;
        }

        public void GenerateKey()
        {
            int k = new Random().Next(2,4);

            p = helper.Generate(safetyParam);
            q = helper.Generate(safetyParam);
            secretKey["r"] = helper.GeneratePrime(safetyParam);

            while (!helper.MillerRabinTest(p))
            {
                p = k*secretKey["r"] + BigInteger.One;
                k++;
            }

            while (!(BigInteger.GreatestCommonDivisor(q - BigInteger.One, secretKey["r"]) == BigInteger.One && BigInteger.Compare(p,q)!=0 && helper.MillerRabinTest(q)==true))
            {
                var pByteLength = p.ToByteArray().Length;
                q = helper.Generate(pByteLength);
            }
            publicKey["n"] = p * q;
            secretKey["c"] = (q - BigInteger.One) * (p - BigInteger.One) / secretKey["r"];

            secretKey["g1"] = helper.GenerateG(secretKey["c"], publicKey["n"]);
            secretKey["g2"] = helper.GenerateG(secretKey["c"], publicKey["n"]);

            while (BigInteger.Compare(secretKey["g1"], secretKey["g2"]) == 0)
            {
                secretKey["g2"] = helper.GenerateG(secretKey["c"], publicKey["n"]);
            }
            phi = (p - 1) * (q - 1);
        }

        public void LogKSecretKey()
        {
            Console.WriteLine("\n Секретний ключ:\n    r = {0}, \n    c = {1}, \n    g\u2081 = {2}, \n    g\u2082 = {3}", secretKey["r"], secretKey["c"], secretKey["g1"], secretKey["g2"]);
        }

        public void LogPublicKey()
        {
            Console.WriteLine("\n Публічний ключ:\n    n = {0}", publicKey["n"]);
        }

        public Dictionary<string, BigInteger> getSecretKey()
        {
            return secretKey;
        }

        public void SetEntireSystem(BigInteger p, BigInteger q, BigInteger r, BigInteger g1, BigInteger g2)
        {
            this.p = p;
            this.q = q;
            publicKey["n"] = p * q;
            secretKey["r"] = r;
            secretKey["c"] = (q - BigInteger.One) * (p - BigInteger.One) / secretKey["r"];
            secretKey["g1"] = g1;
            secretKey["g2"] = g2;
        }

        public bool CheckSystem(bool logResults = false)
        {
            BigInteger res1;
            BigInteger quotient1 = BigInteger.DivRem(p - BigInteger.One, secretKey["r"], out res1);
            bool Condition1 = BigInteger.GreatestCommonDivisor(secretKey["r"], (p - BigInteger.One) / secretKey["r"]) == BigInteger.One;
            bool Condition2 = BigInteger.GreatestCommonDivisor(secretKey["r"], q - BigInteger.One) == BigInteger.One;
            bool Condition3 = BigInteger.Compare(p, q) == 0;
            bool Condition4 = BigInteger.Compare(secretKey["g1"], secretKey["g2"]) == 0;
            bool Condition5 = BigInteger.ModPow(secretKey["g1"], secretKey["c"], publicKey["n"]) != BigInteger.One;
            bool Condition6 = BigInteger.GreatestCommonDivisor(secretKey["g1"], publicKey["n"]) == BigInteger.One;
            bool Condition7 = BigInteger.ModPow(secretKey["g2"], secretKey["c"], publicKey["n"]) != BigInteger.One;
            bool Condition8 = BigInteger.GreatestCommonDivisor(secretKey["g2"], publicKey["n"]) == BigInteger.One;
            if (logResults)
            {

                Console.WriteLine("\n\n Перевірка : ");
                Console.WriteLine("     \u2022 r ділить p-1                  :   {0}", res1 == 0);
                Console.WriteLine("     \u2022 НСД(r, p-1/r)=1               :   {0}", Condition1);
                Console.WriteLine("     \u2022 НСД(r, q-1)=1                 :   {0}", Condition2);
                Console.WriteLine("     \u2022 p != q                        :   {0}", !Condition3);
                Console.WriteLine("     \u2022 g1 != g2                      :   {0}", !Condition4);
                Console.WriteLine("     \u2022 НСД(g1,n)=1                   :   {0}", Condition5);
                Console.WriteLine("     \u2022 g1^c mod n !=1                :   {0}", Condition6);
                Console.WriteLine("     \u2022 НСД(g2,n)=1                   :   {0}", Condition7);
                Console.WriteLine("     \u2022 g2^c mod n !=1                :   {0}", Condition8);
            }
            
            return Condition1 && Condition2 && Condition3 && Condition4 && Condition5 && Condition6 && Condition7 && Condition8;
        } 


        public BigInteger F(BigInteger b, BigInteger x)
             => (BigInteger.ModPow(secretKey["g1"], x, publicKey["n"]) * BigInteger.ModPow(b, secretKey["r"], publicKey["n"]) ) % publicKey["n"]; // вернуть обратный по модулю если минус 

        public List<BigInteger> GetUsedState()
            => usedStates;

        public BigInteger Id(BigInteger a_x, BigInteger u)
            => (BigInteger.ModPow(secretKey["g2"], a_x, publicKey["n"]) * BigInteger.ModPow(u, secretKey["r"], publicKey["n"])) % publicKey["n"];

        public BigInteger GenerateStates()
        {
            var a = helper.GenerateBigInteger(secretKey["r"]);

            while(usedStates.Contains(a))
            {
                a = helper.GenerateBigInteger(secretKey["r"]);
                if(usedStates.Count == secretKey["r"] - 2)
                {
                    Console.WriteLine("No states left!");
                    break;
                }
            }

            usedStates.Add(a);

            return a;
        }

        public BigInteger GenerateImitInsertion(BigInteger b, BigInteger u, BigInteger x, BigInteger a_x)
            => (F(b, x) * Id(a_x, u)) % publicKey["n"];
            //=>((BigInteger.ModPow(secretKey["g1"], x, publicKey["n"])* BigInteger.ModPow(b, secretKey["r"], publicKey["n"])) % publicKey["n"] *
            //                            (BigInteger.ModPow(secretKey["g2"], a_x, publicKey["n"]) * BigInteger.ModPow(u, secretKey["r"], publicKey["n"]) % publicKey["n"])) % publicKey["n"];

        public bool Verify(BigInteger x,  BigInteger e_x, BigInteger a_x)
        {
            var x1 = x % secretKey["r"];
            var a1 = a_x % secretKey["r"];
            var L = BigInteger.ModPow(e_x, secretKey["c"], publicKey["n"]);
            var R = (BigInteger.ModPow(secretKey["g1"], (secretKey["c"]*x1) % publicKey["n"], publicKey["n"])*
                     BigInteger.ModPow(secretKey["g2"], (secretKey["c"]*a1) % publicKey["n"], publicKey["n"])
                     )%publicKey["n"];
            return L == R;
        }


        public List<Tuple<BigInteger, BigInteger, BigInteger, BigInteger, BigInteger, BigInteger>> GenerateAllSystemParams()
        {
            var systemParams = new List<Tuple<BigInteger, BigInteger, BigInteger, BigInteger, BigInteger, BigInteger>>();
            var pqList = new List<Tuple<BigInteger, BigInteger>>();
            var pq = Tuple.Create(new BigInteger(19), new BigInteger(23));
            var r = new BigInteger(9);
            
            var rList = new List<Tuple<BigInteger, List<BigInteger>>>();
            var n = pq.Item1 * pq.Item2;
            var phi = (pq.Item1 - 1) * (pq.Item2 - 1);

            var zn = new List<BigInteger>();
            for (var j = 2; j < n; j++)
            {
                if (BigInteger.GreatestCommonDivisor(j, n) == 1 && BigInteger.ModPow(j, phi / r, n) != 1)
                {
                    zn.Add(j);
                }
            }

            foreach (var g1 in zn)
            {
                foreach (var g2 in zn)
                {
                    if (BigInteger.Compare(g1, g2) != 0)
                    {
                        systemParams.Add(Tuple.Create(pq.Item1, pq.Item2, r, ((pq.Item1 - BigInteger.One) * (pq.Item2 - BigInteger.One)) / r, g1, g2)); // p q r c
                    }
                }
            }

            return systemParams;//p q r c g1 g2 
        }


        public List<IGrouping<BigInteger, Tuple<string, BigInteger,BigInteger,BigInteger>>> FindAllTuples(List<Tuple<BigInteger, BigInteger, BigInteger, BigInteger, BigInteger, BigInteger>> systemParams)
        {
            var resultTuples = new List<Tuple<string, BigInteger, BigInteger, BigInteger>>();
            var tempResults = new List<Tuple<string, BigInteger, BigInteger, BigInteger>>();
            var n = systemParams[0].Item1 * systemParams[0].Item2;

            for (var a_x = 2; a_x < systemParams[0].Item3; a_x++)
            {
                for (var m = 1; m < 50; m++)
                {
                    for (var u = 1; u < n; u++)
                    {
                        for (var b = 1; b < n; b++)
                        {
                            if (BigInteger.GreatestCommonDivisor(u, n) == 1 && BigInteger.GreatestCommonDivisor(b, n) == 1)
                            {
                                var e_x = ((BigInteger.ModPow(systemParams[0].Item5, m, n) * BigInteger.ModPow(b, systemParams[0].Item3, n)) % n *
                                    (BigInteger.ModPow(systemParams[0].Item6, a_x, n) * BigInteger.ModPow(u, systemParams[0].Item3, n) % n)) % n;

                                tempResults.Add(Tuple.Create("|  " + systemParams[0].Item1.ToString().PadRight(4)
                                    + "|  " + systemParams[0].Item2/*.ToString().PadRight(4)*/ 
                                    + "|  " + systemParams[0].Item3/*.ToString().PadRight(4)*/
                                    + "|  " + systemParams[0].Item4/*.ToString().PadRight(4)*/
                                    + "|  " + systemParams[0].Item5/*.ToString().PadRight(4)*/
                                    + "|  " + systemParams[0].Item6/*.ToString().PadRight(4)*/, new BigInteger(m), e_x, new BigInteger(a_x)));
                            }

                        }
                    }
                }
            }

            Console.WriteLine(" Кiлькiсть кортежiв виду < p, q, r, c, g\u2081, g\u2082,  x, E\u2093, a\u2093, b, u >  : {0}\n", tempResults.Count);

            tempResults = tempResults.Distinct().ToList();

            Console.WriteLine(" Кiлькiсть кортежiв виду < p, q, r, c, g\u2081, g\u2082, x, E\u2093, a\u2093 >  : {0}\n", tempResults.Count);
            var groups = tempResults.GroupBy(x => x.Item3)
                                   .ToList();

            //var groups = new Dictionary<BigInteger, List<BigInteger>>();

            Console.WriteLine(" Кiлькiсть iмiтовставок E\u2093, якi можуть бути отриманні за допомогою різних повiдомлень x,\n для параметрiв < p, q, r, c, g\u2081, g\u2082 >  : {0}", groups.Count);
            return groups;
        }


    }
}
