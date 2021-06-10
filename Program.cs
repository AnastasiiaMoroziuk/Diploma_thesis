using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace Methods_for_controlling_integrity_of_delegated
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;


            ////-------------------------------------------------------------------------------------------  К Р И П Т О С И С Т Е М А     Б Е Н А Л О    ----------------------------------------------------------------------------------------------

            /*var benaloSystem = new Benaloh();
            var helper = new Helpers();

            benaloSystem.SetSafetyParam(64);
            benaloSystem.SetPrivateKey();
            benaloSystem.SetPublicKey();

            Console.WriteLine($" Параметр безпеки:  64 байти");
            Console.WriteLine($" Згенерована система: ");
            Console.WriteLine($"\n    Публічний ключ: \n    y = {benaloSystem.GetPublicKey().Item1}\n    r = {benaloSystem.GetPublicKey().Item2}\n    n = {benaloSystem.GetPublicKey().Item3}");
            Console.WriteLine($"\n    Секретний ключ: \n    p = {benaloSystem.GetPrivateKey().Item1}\n    q = {benaloSystem.GetPrivateKey().Item2}\n   ");



           // benaloSystem.CheckKeys();



            var message = BigInteger.Parse("608001");
            Console.WriteLine($" Повідомлення message = {message}\n");
            var ciphertext = benaloSystem.Encrypt(message, helper.GenerateFromMultipicativeGroup(benaloSystem.GetPublicKey().Item3));

            Console.WriteLine($" Повідомлення message = {message}\n");


            Console.WriteLine("\n Зашифрування повідомлення message: ");
            Console.WriteLine($"    Шифротекст ciphetext = {ciphertext}");

            Console.WriteLine("\n Розшифрування шифротекста ciphetext:");
            Console.WriteLine($"    Повідомлення message = {benaloSystem.Decrypt(ciphertext)}");
            */

            /*

            Console.WriteLine($"\n\n Гомоморфність в утворенній криптосистемі :");

            var m1 = new BigInteger(1092);
            var m2 = new BigInteger(1379);

            var u1 = helper.GenerateFromMultipicativeGroup(benaloSystem.GetPublicKey().Item3);
            var u2 = helper.GenerateFromMultipicativeGroup(benaloSystem.GetPublicKey().Item3);

            Console.WriteLine($"\n m\u2081 = {m1}\n m\u2082 = {m2}");

            Console.WriteLine($"\n E(m\u2081 + m\u2082) = { benaloSystem.Encrypt(m1 + m2, (u1 * u2) % benaloSystem.GetPublicKey().Item3) }");
            Console.WriteLine($" E(m\u2081) * E(m\u2082) = { (benaloSystem.Encrypt(m1, u1) * benaloSystem.Encrypt(m2, u2)) % benaloSystem.GetPublicKey().Item3 }");

            Console.WriteLine(" E(m\u2081 + m\u2082) = E(m\u2081) * E(m\u2082)");

            Console.WriteLine($"\n\n D(E(m\u2081) + E(m\u2082)) = {benaloSystem.Decrypt((benaloSystem.Encrypt(m1, u1) + benaloSystem.Encrypt(m2, u2)) % benaloSystem.GetPublicKey().Item3)}");
            Console.WriteLine($" m\u2081 + m\u2082 = {m1 + m2}");
            Console.WriteLine(" D(E(m\u2081) + E(m\u2082)) = m\u2081 + m\u2082 ");*/



            //var n = 19 * 23;
            //var p = 19;
            //var q = 23;
            //var r = 9;
            //var yyy = new List<BigInteger>();
            //for(var i = 1; i <n ; i++)
            //{

            //    var exp = (p - BigInteger.One) * (q - BigInteger.One) / r;
            //    if (BigInteger.ModPow(i, exp, n) != BigInteger.One)
            //    {
            //        yyy.Add(i);
            //    }
            //}
            //Console.WriteLine($" Загальна кількість можливих y : {yyy.Count}");
            //Console.WriteLine($" Кількість отриманих \"поганих\" y : {89}");
            //--------------------------------------------------------------------------------    П О И С К     П Л О Х И Х     И Г Р И К О В   ----------------------------------------------------------------------------------------------------------

            /*var benaloh = new Benaloh();
            var benalohSystem = benaloh.GenAllSystemParams();
            var benalohTuples = benaloh.GenAllCiphertexts(benalohSystem);

            string benalohOutput = @"C:/Users/Anastasiia/Desktop/диплом/benalohOutput.txt";

            var groups = benaloh.GroupByCiphertext(benalohTuples);

            File.WriteAllText(benalohOutput, "");
            Console.WriteLine("\n Кiлькiсть кортежiв <p,q,r,y,E(m), М> :  " + groups.Count() + ",\n   де М - множина вiдкритих текстiв, якi при зашифруваннi дають однаковий шифротекст");


            var results = benaloh.GroupBySystemParams(groups);

            File.AppendAllText(benalohOutput, $"\nКiлькiсть кортежiв <p,q,r,y, |C|> :  {results.Count()}\n\n");

            File.AppendAllText(benalohOutput, $"\nКортежi:\n");
            File.AppendAllText(benalohOutput, $"< p,  q,  r,  y,  |C| >");

            Console.WriteLine("\n Кiлькiсть кортежiв <p,q,r,y, |C|> :  " + results.Count());
            foreach (var r in results)
            {
                File.AppendAllText(benalohOutput, $"\n< {r.Key}  {r.Value}");
                Console.WriteLine(" <" + r.Key + "  " + r.Value);
            }*/



            //---------------------------------------------------------------------    П Р О В Е Р К А     С К О Р О С Т И    Р А С Ш И Ф Р О В А Н И Я   ------------------------------------------------------------------------------------------------

            //var helper = new Helpers();

            /* var system = new Benaloh();
             system.SetSafetyParam(6); // 6 bytes - 48 bits

             //system.SetPrivateKey();
             //system.SetPublicKey();
             system.SetEntireSystem(BigInteger.Parse("85977630100367"),
                 BigInteger.Parse("47227789557161"),
                 BigInteger.Parse("42988815050183"),
                 BigInteger.Parse("3382385"),
                 BigInteger.Parse("4060533421003563862913578087"));

             system.CheckKeys();


             Console.WriteLine($" Параметр безпеки:  48 біт");
             Console.WriteLine($" Згенерована система: ");
             Console.WriteLine($"\n    Публічний ключ: \n    y = {system.GetPublicKey().Item1}\n    r = {system.GetPublicKey().Item2}\n    n = {system.GetPublicKey().Item3}");
             Console.WriteLine($"\n    Секретний ключ: \n    p = {system.GetPrivateKey().Item1}\n    q = {system.GetPrivateKey().Item2}\n   ");


             for (int i = 5; i <= 5; i++)
             {
                 Console.WriteLine($"\n\n Розмір повідомлення: {i * 8} біт");
                 for (int j = 1; j <= 5; j++)
                 {

                     var m = BigInteger.Parse("3120407298");
                     Console.WriteLine(m.ToByteArray().Length);
                     var c = system.Encrypt(m, helper.GenerateFromMultipicativeGroup(system.GetPublicKey().Item3));
                     Console.WriteLine($"\n Повідомлення m = {m},");
                     var m_start = DateTime.Now;
                     var m_dec = system.Decrypt(c);
                     var m_end = DateTime.Now;
                     Console.WriteLine($" Шифротекст = {c}");
                     Console.WriteLine($" Результат розшифрування: m = {m_dec}");
                     Console.WriteLine($" Час виконання розшифрування : {m_end - m_start}");

                     var m1 = BigInteger.Parse("4037103298");
                     Console.WriteLine(m1.ToByteArray().Length);
                     var c1 = system.Encrypt(m1, helper.GenerateFromMultipicativeGroup(system.GetPublicKey().Item3));
                     Console.WriteLine($"\n Повідомлення m = {m1},");
                     var m1_start = DateTime.Now;
                     var m1_dec = system.Decrypt(c1);
                     var m1_end = DateTime.Now;
                     Console.WriteLine($" Шифротекст = {c1}");
                     Console.WriteLine($" Результат розшифрування: m = {m1_dec}");
                     Console.WriteLine($" Час виконання розшифрування : {m1_end - m1_start}");
                 }
             }*/


            //---------------------------------------------------------------------П Р О В Е Р К А Ц Е Л О С Н О С Т И С Л О Ж Е Н И Я--------------------------------------------------------------------------------------------------

            //var schema = new Schema();
            //var helper = new Helpers();

            //schema.setSafetyParam(32);
            //schema.GenerateKey();


            //Console.WriteLine($"\n Параметр безпеки:  32 байти");
            //Console.WriteLine($" Згенерована система: ");
            //schema.LogPublicKey();
            //schema.LogKSecretKey();

            //schema.CheckSystem(true);

            //var m1 = helper.Generate(32);
            //var m2 = helper.Generate(32);



            //var a_m1 = schema.GenerateStates();
            //var a_m2 = schema.GenerateStates();

            //var b1 = helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]);
            //var u1 = helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]);
            //var b2 = helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]);
            //var u2 = helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]);


            //var E_m1 = schema.GenerateImitInsertion(b1, u1, m1, a_m1);
            //var E_m2 = schema.GenerateImitInsertion(b2, u2, m2, a_m2);

            //var E_m1_plus_m2 = (E_m1 * E_m2) % schema.publicKey["n"];

            //Console.WriteLine("\n Повідомлення x\u2081 = {0} \n Імітовставка E\u2093\u2081: {1}", m1, E_m1);
            //Console.WriteLine("\n Повідомлення x\u2082 = {0} \n Імітовставка E\u2093\u2082: {1}", m2, E_m2);


            //Console.WriteLine($"\n\n Результат перевірки цілісності повідомлення x\u2081: {schema.Verify(m1, E_m1, a_m1)}");
            //Console.WriteLine($" Результат перевірки цілісності повідомлення x\u2082: {schema.Verify(m2, E_m2, a_m2)}");

            //Console.WriteLine($"\n\n Результат перевірки цілісності повідомлення x\u2081 при заміні на повідомлення x\u2082: {schema.Verify(m2, E_m1, a_m1)}");

            //Console.WriteLine("\n\n Сума повідомлень x\u2081 + x\u2082: {0}\n Імітовставка суми E\u2093\u2081\u208a\u2093\u2082:  {1}\n", m1 + m2, E_m1_plus_m2);
            //Console.WriteLine(" Результат перевірки цілісності суми повідомлень x\u2081 та x\u2082:  {0}", schema.Verify((m1+m2) % schema.publicKey["n"], E_m1_plus_m2, (a_m1 + a_m2) % schema.publicKey["n"]));

            //// ---------------------------------------------------------------------------------    П О Д Д Е Л К А Ц Е Л О С Н О С Т И    ---------------------------------------------------------------------------------------------------------------


            ////string integrityOutput = @"C:/Users/Anastasiia/Desktop/диплом/integrityOutput.txt";
            //string integrityOutput = @"C:/Users/Anastasiia/Desktop/диплом/groupedByStates.txt";
            //var schema = new Schema();
            //var start = DateTime.Now;
            //var parameters = schema.GenerateAllSystemParams();
            //Console.WriteLine(" Кiлькiсть всiх можливих кортежiв виду < p, q, r, c, g\x2081, g\u2082> при фiксованих <p, q, r>  : {0}\n", parameters.Count);
            //var tuples = schema.FindAllTuples(parameters);
            //var end = DateTime.Now;
            //Console.WriteLine("\ntime:  {0}", end - start);

            //foreach(var item in tuples)
            //{
            //    var list = new List<BigInteger>();
            //    Console.Write(item.Key);
            //    foreach(var tuple in item)
            //    {
            //        list.Add(tuple.Item4);
            //    }
            //    list = list.Distinct().ToList();
            //    Console.WriteLine(list.Count());
            //}


            //File.WriteAllText(integrityOutput, "");
            //foreach (var item in tuples)
            //{
            //    //File.AppendAllText(integrityOutput, $"\n\nІмітовставка : {item.Key}");

            //    File.AppendAllText(integrityOutput, $"\nState : {item.Key}");
            //    //File.AppendAllText(integrityOutput, $"\n_________________________________________________________________ ");
            //    //File.AppendAllText(integrityOutput, $"\n|      |      |	     |	    |	   |	  |      |        |      | ");
            //    //File.AppendAllText(integrityOutput, $"\n|   p  |   q  |   r  |   c  |  g\u2081  |  g\u2082  |   x  |   E\u2093   |  a\u2093  |");
            //    //File.AppendAllText(integrityOutput, $"\n|______|______|______|______|______|______|______|________|______| ");
            //    foreach (var cc in item)
            //    {

            //        File.AppendAllText(integrityOutput, $"\n{cc.Item1}|  {cc.Item2.ToString().PadRight(4)}|  {cc.Item3.ToString().PadRight(6)}|  {cc.Item4.ToString().PadRight(3)} |");
            //        //File.AppendAllText(integrityOutput, $"\n|______|______|______|______|______|______|______|________|______|");
            //    }
            //}


            //Console.WriteLine("done");

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //schema.SetEntireSystem(19, 23, 9, 2, 3);
            ////schema.LogKSecretKey();
            ////schema.LogPublicKey();
            ////////////schema.CheckSystem(true);

            //var m1 = new BigInteger(31);
            //var a_m1 = new BigInteger(2);
            //var h = new List<Tuple<BigInteger, BigInteger, BigInteger>>();
            //var E_m1 = schema.GenerateImitInsertion(helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), m1, a_m1);
            //while (E_m1 != new BigInteger(274))
            //{
            //    E_m1 = schema.GenerateImitInsertion(helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), m1, a_m1);
            //}



            //var m2 = new BigInteger(49);
            //var a_m2 = new BigInteger(2);
            //var E_m2 = schema.GenerateImitInsertion(helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), m2, a_m2);
            //while (E_m2 != E_m1)
            //{
            //    E_m2 = schema.GenerateImitInsertion(helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), m2, a_m2);
            //}



            //var m3 = new BigInteger(65);
            //var a_m3 = schema.GenerateStates();
            //var E_m3 = schema.GenerateImitInsertion(helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), helper.GenerateFromMultipicativeGroup(schema.publicKey["n"]), m3, a_m3);


            //Console.WriteLine($"\nx\u2081: {m1}, a\u2093\u2081: {a_m1},  E\u2093\u2081: {E_m1}");
            //Console.WriteLine($"Verify(x\u2081, a\u2093\u2081, E\u2093\u2081) : { schema.Verify(m1, E_m1, a_m1)}\n\n");

            //Console.WriteLine($"x\u2082: {m2}, a E\u2093\u2082: {a_m2}, E\u2093\u2082 {E_m2}");
            //Console.WriteLine($"Verify(x\u2082, a\u2093\u2081, E\u2093\u2081) : { schema.Verify(m2, E_m1, a_m1)}\n\n");


            ////Console.WriteLine($"m3: {m3}, a_m3: {a_m3}, e_m3: {E_m3}");
            //Console.WriteLine($"\nx\u2083: {m3}, a E\u2093\u2083: {a_m3}, E\u2093\u2083 {E_m3}\n");


            ////Console.WriteLine($"Verify(m2, e_m1, a_m1) : { schema.Verify(m2, E_m1, a_m1)}\n");

            //Console.WriteLine("Verification of  x\u2081 + x\u2083 : ");


            //var E_m2_plus_m3 = (E_m1 * E_m3) % schema.publicKey["n"];



            //Console.WriteLine(" x\u2081 + x\u2083: {0}", m3 + m1, E_m2_plus_m3);

            //Console.WriteLine("Is verified:  {0}\n", schema.Verify((m1 + m3) % schema.publicKey["n"], E_m2_plus_m3, (a_m2 + a_m3) % schema.publicKey["n"]));

            //Console.WriteLine("Verification of  x\u2082 + x\u2083 (imitinsertions and states do not change) : ");

            //Console.WriteLine(" x\u2082 + x\u2083: {0}", m3 + m2);
            //Console.WriteLine("Is verified:  {0}", schema.Verify((m2 + m3) % schema.publicKey["n"], E_m2_plus_m3, (a_m2 + a_m3) % schema.publicKey["n"]));

            Console.Read();
        }
    }
    
}
