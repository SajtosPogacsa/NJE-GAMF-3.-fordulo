using System;
using System.Collections.Concurrent;

ConcurrentDictionary<long, long> cache = new ConcurrentDictionary<long, long>();

static long CollatzLength(long n, ConcurrentDictionary<long, long> cache)
{
    if (cache.TryGetValue(n, out long cachedLength))
    {
        return cachedLength;
    }

    long length = 1;
    long num = n;

    while (num != 1)
    {
        if (num % 2 == 0)
        {
            num /= 2;
        }
        else
        {
            num = num * 3 + 1;
        }
        length++;

        if (cache.TryGetValue(num, out cachedLength))
        {
            length += cachedLength - 1;
            break;
        }
    }

    cache[n] = length;
    return length;
}

var szamok2 = new List<(long, long)>();
var lockObj = new object();

// Párhuzamos feldolgozás 1-től 1 millióig
Parallel.For(1, 1000000, i =>
{
    long length = CollatzLength(i, cache);
    lock (lockObj)
    {
        szamok2.Add((i, length));
    }
});

// Maximális lépésszámú szám keresése
var maxSzam = szamok2.OrderByDescending(x => x.Item2).First();
Console.WriteLine($"{maxSzam.Item1}|{maxSzam.Item2}");


// 3. feladat
Dictionary<string, List<string>> szamokRaw = new();

for (int i = 1000; i <= 9999; i++)
{
    szamokRaw.Add(i.ToString(), new());
}


//permutációk
foreach (var item in szamokRaw)
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
                        szamokRaw[item.Key].Add(szam);
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

Dictionary<string, List<string>> szamok = new();
List<List<string>> vótmámore = new();
//számnégyes duplikációk szűrése
foreach (var negyes in szamokRaw)
{
    bool dupl = false;
    List<string> value = negyes.Value.OrderBy(x => x).ToList();
    if (vótmámore.Count == 0)
    {
        vótmámore.Add(value);
        szamok.Add(negyes.Key, value);
        continue;
    }
    foreach (var vót in vótmámore)
    {
        var vótOrd = vót.OrderBy(x => x).ToList();
        if (value.SequenceEqual(vótOrd))
        {
            dupl = true;
            break;
        }
    }

    if (!dupl)
    {
        vótmámore.Add(value);
        szamok.Add(negyes.Key, value);
    }
}


Dictionary<string, List<string>> Primes = new();

//számlálás
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
Console.WriteLine(counter);


foreach (var item in Primes)
{
    bool found = false;
    if (item.Value.Count < 3) continue;
    for (int i = 0; i < item.Value.Count - 2; i++)
    {
        int fnum = int.Parse(item.Value[i]);
        int snum = int.Parse(item.Value[i + 1]);
        int tnum = int.Parse(item.Value[i + 2]);
        int dif = snum - fnum;
        if (fnum + dif == snum && snum + dif == tnum)
        {
            Console.WriteLine($"{fnum} {snum} {tnum}");
            found = true;
            break;
        }
    }
    if (found) break;
}

// 4. feladat
List<Papi> papik = new();

for (int i = 1; i < 300; i++)
{
    papik.Add(new Papi(i));
    if (i == 2) papik.Find(x => x.Id == 2).GetInfected(1);
}
using (StreamReader sr = new("elek.txt"))
{
    while (!sr.EndOfStream)
    {
        string[] line = sr.ReadLine().Split(' ');
        int id = int.Parse(line[0]);
        int contact = int.Parse(line[1]);
        papik.Find(x => x.Id == id).Contacts.Add(contact);
        if (!papik.Find(x => x.Id == contact).Contacts.Contains(id))
            papik.Find(x => x.Id == contact).Contacts.Add(id);
    }
}

int counterV = 1;
int step = 1;
while (papik.Count(x => x.Infected) != 0)
{
    foreach (Papi p in papik)
    {
        p.InfTick();
    }
    using (StreamReader sr = new("elek.txt"))
    {
        while (!sr.EndOfStream)
        {
            string[] talalkozas = sr.ReadLine().Split(' ');
            int elso = int.Parse(talalkozas[0]);
            int masodik = int.Parse(talalkozas[1]);
            papik[elso - 1].Infect(papik[masodik - 1], step);
        }
    }
    if(counterV == 5)
    {
        Console.WriteLine($"Az 5. lépésben a fertőzöttek száma: {papik.Count(x => x.Infected)}");
    }
    if(counterV == 11)
    {
        Console.WriteLine($"A 11. lépésben a fertőzöttek száma: {papik.Count(x => x.Infected)}");
    }
    if (counterV == 13)
    {
        Console.WriteLine($"A 13. lépésben a fertőzöttek száma: {papik.Count(x => x.Infected)}");
    }
    if (papik.Count(x => x.Infected) == 0)
    {
        Console.WriteLine($"A {counterV}. lépésben lesz 0 a fertőzöttek száma!");
    }
    counterV++;
    step++;
}

