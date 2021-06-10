using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Methods_for_controlling_integrity_of_delegated
{
    class Benaloh
    {
        public BigInteger[] primes = new BigInteger[] { 5, 7, 11, 13, 17, 19, 23, 29, 31 /*, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97*/};
        public BigInteger n;
        public BigInteger y;
        public BigInteger r;
        private int safetyParam;

        private BigInteger p;
        private BigInteger q;


        private Helpers helper = new Helpers();

        public void SetSafetyParam(int number)
        {
            safetyParam = number;
        }

        public void SetPrivateKey()
        {
            p = helper.GeneratePrime(safetyParam);
            q = helper.GeneratePrime(safetyParam);

            while (p.ToByteArray().Length > safetyParam || q.ToByteArray().Length > safetyParam)
            {
                p = helper.GeneratePrime(safetyParam);
                q = helper.GeneratePrime(safetyParam);
            }
            while (BigInteger.Compare(p, q) == 0)
            {
                q = helper.GeneratePrime(safetyParam);
            }

            n = p * q;

        }

        public void SetPublicKey()
        {
            r = p - BigInteger.One;
            while (BigInteger.GreatestCommonDivisor(q - BigInteger.One, r) != BigInteger.One)
            {
                r = BigInteger.Divide(r, BigInteger.GreatestCommonDivisor(q - BigInteger.One, r));
            }

            var len = new Random().Next(1, safetyParam - 1);
            y = helper.Generate(len);
            var exp = (p - BigInteger.One) * (q - BigInteger.One) / r;
            while (BigInteger.ModPow(y, exp, n) == BigInteger.One)
            {
                y += BigInteger.One;
            }
        }


        public Tuple<BigInteger, BigInteger, BigInteger> GetPublicKey()
            => Tuple.Create(y, r, n);
        

        public Tuple<BigInteger, BigInteger> GetPrivateKey()
            => Tuple.Create(p, q);
        

        public void CheckKeys()
        {
            Console.WriteLine("\n\n Перевірка чи задовільняє система умовам: ");
            BigInteger res1;
            BigInteger quotient1 = BigInteger.DivRem(p - BigInteger.One, r, out res1);
            Console.WriteLine("     \u2022 r ділить p-1                    :   {0};", res1 == 0);
            Console.WriteLine("     \u2022 НСД(r, p-1/r) = 1               :   {0};", BigInteger.GreatestCommonDivisor(r, (p - BigInteger.One) / r) == BigInteger.One);
            Console.WriteLine("     \u2022 НСД(r, q-1) = 1                 :   {0};", BigInteger.GreatestCommonDivisor(r, q - BigInteger.One) == BigInteger.One) ;
            Console.WriteLine("     \u2022 y^(p-1)(q-1)/r mod n != 1       :   {0};", BigInteger.ModPow(y, (p - BigInteger.One) * (q - BigInteger.One) / r, n) != BigInteger.One);
            Console.WriteLine("     \u2022 НСД(y^(p-1)(q-1)/r, n) = 1      :   {0};", BigInteger.GreatestCommonDivisor(BigInteger.ModPow(y, (p - BigInteger.One) * (q - BigInteger.One) / r, n), n) == BigInteger.One);


            Console.WriteLine("\n");
        }

        public void SetEntireSystem(BigInteger p, BigInteger q, BigInteger r, BigInteger y, BigInteger n)
        {
            this.p = p;
            this.q = q;
            this.r = r;
            this.y = y;
            this.n = n;

        }

        public BigInteger Encrypt(BigInteger message, BigInteger u)
            => (BigInteger.ModPow(y, message, n) * BigInteger.ModPow(u, r, n)) % n;
        

        public BigInteger Decrypt(BigInteger ciphertext)
        {
            var m = BigInteger.One;
            var x = BigInteger.ModPow(y, (p - BigInteger.One) * (q - BigInteger.One) / r, n);
            var a = BigInteger.ModPow(ciphertext, (p - BigInteger.One) * (q - BigInteger.One) / r, n);
            while (BigInteger.ModPow(x, m, n) != a)
            {
                m += BigInteger.One;
            }

            return m;
        }

        public List<Tuple<BigInteger, BigInteger, List<Tuple<BigInteger, List<BigInteger>>>>> GenAllSystemParams()
        {
            var pqList = new List<Tuple<BigInteger, BigInteger>>();
            var systemParams = new List<Tuple<BigInteger, BigInteger, List<Tuple<BigInteger, List<BigInteger>>>>>();
            foreach(var p in primes)
            {
                foreach(var q in primes)
                {
                    if(BigInteger.Compare(q,p)!=0)
                    {
                        pqList.Add(Tuple.Create(p, q));
                    }
                }
            }

            var result =
            pqList
            .Select(x => new[] { x.Item1, x.Item2 }.OrderBy(s => s).ToArray())
            .Select(x => Tuple.Create<BigInteger, BigInteger>(x[0], x[1]))
            .Distinct();


            foreach (var pq in result)
            {
                var rList = new List<Tuple<BigInteger, List<BigInteger>>>();
                var n = pq.Item1 * pq.Item2;
                var phi = (pq.Item1 - 1) * (pq.Item2 - 1);

                for (var i = 2; i < pq.Item1 - 1; i++)
                {
                    BigInteger res1;
                    BigInteger quotient1 = BigInteger.DivRem(pq.Item1 - BigInteger.One, i, out res1);
                    if (BigInteger.GreatestCommonDivisor(i, pq.Item2 - 1) == 1 && res1 == 0 && BigInteger.GreatestCommonDivisor(i, pq.Item1 - 1 / i) == 1)
                    {
                        var yList = new List<BigInteger>();
                        for (var j = 2; j < n; j++)
                        {
                            if (BigInteger.GreatestCommonDivisor(j, n) == 1 && BigInteger.ModPow(j, phi / i, n) != 1)
                            {
                                yList.Add(j);
                            }
                        }

                        rList.Add(Tuple.Create(new BigInteger(i), yList));
                    }
                    
                }
                if (rList.Count != 0)
                    systemParams.Add(Tuple.Create(pq.Item1, pq.Item2, rList));
            }
            return systemParams;    
        }

        public List<Tuple<string, BigInteger, BigInteger>> GenAllCiphertexts(List<Tuple<BigInteger, BigInteger, List<Tuple<BigInteger, List<BigInteger>>>>> system)
        {
            var list = new List<Tuple<string, BigInteger, BigInteger, BigInteger>>();
            var paramsAndM = new List<Tuple<string, BigInteger, BigInteger>>();
            foreach (var item in system) 
            {
                var n = item.Item1 * item.Item2;
                foreach (var r in item.Item3)
                {

                    foreach (var y in r.Item2)
                    {
                        var Encrypted = new List<BigInteger>();
                        for(var m = 1; m < r.Item1; m++)
                        {
                            for (var u = 1; u < n; u++)
                            {
                                if(BigInteger.GreatestCommonDivisor(u,n) == 1)
                                {
                                    var enc = (BigInteger.ModPow(y,m,n)*BigInteger.ModPow(u,r.Item1,n))% n;
                                    
                                    list.Add(Tuple.Create(item.Item1+", "+item.Item2+", "+ r.Item1+", "+ y, new BigInteger(m), new BigInteger(u), enc));
                                    paramsAndM.Add(Tuple.Create(item.Item1 + ", " + item.Item2 + ", " + r.Item1 + ", " + y, new BigInteger(m), enc));
                                }
                            }
                        }
                        
                    }
                }
            }

            Console.WriteLine(" Всього кортежiв <p, q, r, y, m, u, E(m)>:  " + list.Count);
           

            paramsAndM = paramsAndM
                .Distinct()
                .ToList();

            Console.WriteLine("\n Всього кортежiв <p, q, r, y, m, E(m)>:  " + paramsAndM.Count);
            return paramsAndM;
        }

        public Dictionary<string, List<BigInteger>> GroupByCiphertext(List<Tuple<string, BigInteger, BigInteger>> tuples)
        {
            var results = new Dictionary<string, List<BigInteger>>();
            foreach(var tuple in tuples)
            {
                if (results.ContainsKey(tuple.Item1 + ", " + tuple.Item3))
                {
                    results[tuple.Item1 + ", " + tuple.Item3].Add(tuple.Item2);
                }
                else
                {
                    results.Add(tuple.Item1 + ", " + tuple.Item3, new List<BigInteger>());
                    results[tuple.Item1 + ", " + tuple.Item3].Add(tuple.Item2);
                }
            }

            foreach (var tuple in results)
            {
                if(tuple.Value.Count < 2)
                {
                    results.Remove(tuple.Key);
                }
                
            }

            return results;

        }


        public Dictionary<string, int> GroupBySystemParams(Dictionary<string, List<BigInteger>> groups)
        {
            var results = new Dictionary<string, int>();
            foreach (var group in groups)
            {
                var param = group.Key.Split(", ");
                Array.Resize(ref param, 4);
                if (results.ContainsKey(String.Join(", ",  param) + "> CipherTexts :"))
                {
                    results[String.Join(", ", param) + "> CipherTexts :"]++;
                }
                else
                {
                    results.Add(String.Join(", ", param) + "> CipherTexts :", 1);
                }
            }

            return results;
        }

    }
}
