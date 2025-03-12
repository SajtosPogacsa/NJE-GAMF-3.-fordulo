//List<char[]> csomagolóhelyek = new();

//using (StreamReader sr = new("dobozok.txt"))
//{
//    while(!sr.EndOfStream)
//    {
//        char doboz = (char)sr.Read();
//        if(doboz == 'A')
//        {
//            char[] csh = new char[3];
//            csh[0] = doboz;
//            csomagolóhelyek.Add(csh);
//        }
//        else if(doboz == 'B')
//        {
//            try
//            {
//                csomagolóhelyek.Where(x => x[0] == 'A' && x[1] == 0).FirstOrDefault()[1] = doboz;
//            }
//            catch (Exception)
//            {
//                char[] csh = new char[3];
//                csh[0] = doboz;
//                csomagolóhelyek.Add(csh);
//            }
//        }
//        else if(doboz == 'C')
//        {
//            if (csomagolóhelyek.Where(x => x[0] == 'A' && x[1] == 'B' && x[2] != 'C').FirstOrDefault() != default)
//            {
//                csomagolóhelyek.FirstOrDefault(x => x[0] == 'A' && x[1] == 'B' && x[2] != 'C')[2] = doboz;
//            }
//            else if (csomagolóhelyek.Where(x => x[0] == 'A' && x[1] == 0).FirstOrDefault() != default)
//            {
//                csomagolóhelyek.FirstOrDefault(x => x[0] == 'A' && x[1] == 0)[1] = doboz;
//            }
//            else if (csomagolóhelyek.Where(x => x[0] == 'B' && x[1] == 0).FirstOrDefault() != default)
//            {
//                csomagolóhelyek.FirstOrDefault(x => x[0] == 'B' && x[1] == 0)[1] = doboz;
//            }
//            else
//            {
//                char[] csh = new char[3];
//                csh[0] = doboz;
//                csomagolóhelyek.Add(csh);
//            }

//        }
//    }
//}

//Console.WriteLine(csomagolóhelyek.Count());

//using(StreamWriter sr = new("adatok.txt"))
//{
//    foreach (var item in csomagolóhelyek)
//    {
//        foreach (var c in item)
//        {
//            if(c != 0) sr.Write(c);

//        }
//    }
//}


//class Program
//{
//    static Dictionary<long, long> cache = new Dictionary<long, long>();

//    static long CollatzLength(long n)
//    {
//        if (cache.ContainsKey(n))
//        {
//            return cache[n];
//        }

//        long length = 1;
//        long num = n;

//        while (num != 1)
//        {
//            if (num % 2 == 0)
//            {
//                num /= 2;
//            }
//            else
//            {
//                num = num * 3 + 1;
//            }
//            length++;

//            if (cache.ContainsKey(num))
//            {
//                length += cache[num] - 1;
//                break;
//            }
//        }

//        cache[n] = length;
//        return length;
//    }

//    static void Main(string[] args)
//    {
//        var szamok = new List<(long, long)>();
//        var lockObj = new object();

//        // Párhuzamos feldolgozás 1-től 1 millióig
//        Parallel.For(1, 1000000, i =>
//        {
//            long length = CollatzLength(i);
//            lock (lockObj)
//            {
//                szamok.Add((i, length));
//            }
//        });

//        // Maximális lépésszámú szám keresése
//        var maxSzam = szamok.OrderByDescending(x => x.Item2).First();
//        Console.WriteLine($"{maxSzam.Item1}|{maxSzam.Item2}");
//    }
//}


//asdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
using System.Collections.Generic;

Dictionary<string, List<string>> szamok = new();


for (int i = 1; i < 10; i++)
{
    for (int j = 1; j < 10; j++)
    {
        for (int k = 1; k < 10; k++)
        {
            for (int l = 1; l < 10; l++)
            {
                szamok.Add($"{i}{j}{k}{l}", new());
            }
        }
    }
}

foreach (var item in szamok)
{
    List<string> duplicate = new();
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            if (j == i) continue;
            for (int k = 0; k < 4; k++)
            {
                if (k == j || k == i) continue;
                for (int l = 0; l < 4; l++)
                {
                    if (l == k || l == j || l == i) continue;
                    string szam = $"{item.Key[i]}{item.Key[j]}{item.Key[k]}{item.Key[l]}";
                    if (!duplicate.Contains(szam))
                    {
                        szamok[item.Key].Add(szam);
                        duplicate.Add(szam);
                    }
                }
            }
        }
    }
}

static bool IsPrime(long number)
{
    // Kezeljük a speciális eseteket
    if (number <= 1)
        return false;
    if (number == 2 || number == 3)
        return true;
    if (number % 2 == 0 || number % 3 == 0)
        return false;

    // Csak a 6k ± 1 alakú számokat ellenőrizzük
    for (long i = 5; i * i <= number; i += 6)
    {
        if (number % i == 0 || number % (i + 2) == 0)
            return false;
    }

    return true;
}

Dictionary<string, List<string>> Primes = new();

int counter = 0;
foreach (var item in szamok)
{
    int primeCounter = 0;
    foreach (var num in item.Value)
    {
        if (IsPrime(int.Parse(num)))
        {
            if (!Primes.ContainsKey(item.Key)) Primes.Add(item.Key, new());
            Primes[item.Key].Add(num);
            primeCounter++;
        }
    }
    if (primeCounter > 5) counter++;
}


int index = 1;
foreach (var item in Primes)
{
    if (item.Value.Count < 6) continue;
    Console.Write($"{index}.{item.Key}->");
    foreach (var p in item.Value)
    {
        Console.Write(p + " ");
    }
    index++;
    Console.WriteLine();
}

Console.WriteLine(counter);

