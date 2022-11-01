using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace conversie_intre_baze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int b1, b2, intreg = 0, frac = 0, checkfrac = 0;
            string conv;
            char[] hex = new char[] { '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f'};
            char[] fracchar = new char[] { ',', '.' };
            Console.WriteLine("Introduceti baza din care vreti sa convertiti (b1): ");
            b1 = BaseCheck();
            Console.WriteLine("Introduceti baza in care vreti sa convertiti (b2): ");
            b2 = BaseCheck2();
            Console.WriteLine("Introduceti numarul pe care vreti sa il convertiti: ");
            conv = Console.ReadLine();
            Console.WriteLine($"\nNumarul {conv} din baza {b1} in baza {b2} este: \n");
            if (b1 > 10)
            {
                string convint, convfrac = "0";
                convint = conv;
                if (conv.Contains(',') || conv.Contains('.'))
                {
                    string[] nrc = conv.Split(fracchar);
                    convint = nrc[0];
                    convfrac = nrc[1];
                    if (nrc.Length > 2)
                    {
                        Console.WriteLine("nr introdus gresit!");
                        return;
                    }
                    checkfrac = 1;
                }
                char[] cifre = convint.ToCharArray();
                int[] cifreint = new int[cifre.Length];
                int countcif = 0;
                for (int i = 0; i < cifre.Length; i++)
                {
                    for (int j = 0; j < hex.Length; j++)
                    {
                        if (cifre[i] == hex[j])
                        {
                            cifreint[i] = j;
                        } 
                    }
                   // Console.WriteLine(cifreint[i]);
                }
                for (int i = cifre.Length - 1 ; i >= 0; i--)
                {
                    intreg = intreg + cifreint[i] * (int)Math.Pow(b1, countcif);
                    countcif++;
                }
                //// de aici partea fractiala
                if (checkfrac != 0)
                {
                    double fractie;
                    //Console.WriteLine(frac);                   
                    int cifref = 0, countfrac = -1;                   
                    char[] cifreb10pf = convfrac.ToCharArray();
                    int[] cifrefrac = new int[convfrac.Length];                   
                    for (int i = 0; i < cifreb10pf.Length; i++)
                    {
                        for (int j = 0; j < hex.Length; j++)
                        {
                            if (cifreb10pf[i] == hex[j])
                            {
                                cifrefrac[i] = j;
                            }
                        }
                        //Console.WriteLine(cifrefrac[i]);
                    }
                    fractie = 0;
                    for (int i = 0; i < cifrefrac.Length; i++)
                    {
                        fractie = fractie + cifrefrac[i] * Math.Pow(b1, countfrac);
                        //Console.WriteLine(fractie);
                        countfrac--;
                    }
                    // acuma avem fractia in baza 10
                    int countperiod = 0;
                    while (fractie != 0 && countperiod < 8)
                    {
                        cifref = 10 * cifref + (int)Math.Floor(fractie * 10);
                        fractie = (fractie * 10) - (int)Math.Floor(fractie * 10);
                        countperiod++;
                        //Console.WriteLine(cifref);
                    }
                    conv = string.Concat(Convert.ToString(intreg), ',', Convert.ToString(cifref)); // returneaza un nr in baza 10 cu virgula fixa
                }
                else
                {
                    conv = string.Concat(Convert.ToString(intreg)); // returneaza doar partea intreaga
                }
                b1 = 10; // nr initial s-a convertit in baza 10.
            }
            if (conv.Contains(',') || conv.Contains('.'))
            {
                string[] nrc = conv.Split(fracchar);
                intreg = int.Parse(nrc[0]);
                frac = int.Parse(nrc[1]);
                if (nrc.Length > 2)
                {
                    Console.WriteLine("nr introdus gresit!");
                    return;
                }
                checkfrac = 1;
            }
            else
            {
                intreg = int.Parse(conv);
            }
            int intb10 = 0;
            string convertit;
            int countb10 = 1;
            int intreglength = Convert.ToString(intreg).Length;
            if (b1 <= 10 && b2 <= 10) // partea intreaga din b1 in b2
            {
                //din b1 in baza 10
                int intregaux = intreg;
                int countcif = 0;
                int[] cifre = new int[intreglength];               
                for (int i = 0; i < cifre.Length; i++)
                {
                    cifre[i] = intregaux % 10;
                    //Console.WriteLine(cifre[i]);
                    intregaux = intregaux / 10;
                }
                for (int i = 0; i < cifre.Length; i++)
                {
                    intb10 = intb10 + cifre[i] * (int)Math.Pow(b1, countcif);
                    countcif++;
                }
                //Console.WriteLine($"b10 = {intb10}");
                intreg = intb10;
                // din b10 in b dorita.
                intregaux = intreg;
                while (intregaux > 0)
                {
                    // Console.WriteLine(intregaux);
                    intregaux = intregaux / b2;
                    countb10++;
                }
                int[] r = new int[countb10];
                int count2 = 0;
                while (intreg > 0)
                {
                    r[count2] = intreg % b2;
                    intreg = intreg / b2;
                    count2++;
                }               
                for (int i = r.Length - 2; i >= 0; i--)
                {
                    Console.Write(r[i]);
                }
                if (r.Length < 2)
                {
                    Console.Write("0");
                }
            }
            if (checkfrac == 1)
            {
                Console.Write(",");
                double fractie;
                //Console.WriteLine(frac);
                int fractieaux = frac;
                int cifref = 0, countfrac = -1;
                int[] cifreb1 = new int[Convert.ToString(frac).Length];
                for (int i = 0; i < Convert.ToString(frac).Length; i++)
                {
                    cifreb1[i] = fractieaux % 10;
                    fractieaux /= 10;
                   // Console.WriteLine(fractieaux);
                }
                fractie = 0;
                for (int i = cifreb1.Length - 1; i >= 0; i--)
                {
                    fractie = fractie + cifreb1[i] * Math.Pow(b1, countfrac);
                    countfrac--;
                }
                // acuma avem fractia in baza 10
                int countperiod = 0;
                while (fractie != 0 && countperiod < 8)
                {
                    cifref = 10 * cifref + (int)Math.Floor(fractie * b2);
                    fractie = (fractie * b2) - (int)Math.Floor(fractie * b2);
                    countperiod++;
                }
                Console.Write(cifref);                
            }
            if (b2 > 10)
            {
                int intregaux = intreg;
                if (b1 < 10)
                {                   
                    int countcif = 0;
                    int[] cifre = new int[intreglength];

                    for (int i = 0; i < cifre.Length; i++)
                    {
                        cifre[i] = intregaux % 10;
                        //Console.WriteLine(cifre[i]);
                        intregaux = intregaux / 10;
                    }
                    for (int i = 0; i < cifre.Length; i++)
                    {
                        intb10 = intb10 + cifre[i] * (int)Math.Pow(b1, countcif);
                        countcif++;
                    }
                    //Console.WriteLine($"b10 = {intb10}");
                    intreg = intb10;
                }
                intregaux = intreg;

                while (intregaux > 0)
                {
                    // Console.WriteLine(intregaux);
                    intregaux = intregaux / b2;
                    countb10++;
                }

                //Console.WriteLine(intreg);

                string[] r = new string[countb10];
                int count2 = 0;
                while (intreg > 0)
                {
                    r[count2] = Convert.ToString(intreg % b2);
                    intreg = intreg / b2;
                    count2++;
                }
                string[] hexb = new string[] { "0", "1","2", "3", "4", "5", "6", "7", "8", "9","a", "b", "c", "d", "e", "f" };
                for (int i = 0; i < r.Length; i++)
                {
                    for (int j = 0; j < hexb.Length; j++)
                    {
                        if (r[i] == Convert.ToString(j))
                        {
                            r[i] = hexb[j];
                        }
                    }
                    //Console.WriteLine(cifrefrac[i]);
                }
                for (int i = r.Length - 2; i >= 0; i--)
                {
                    Console.Write(r[i]);
                }
                if (r.Length < 2)
                {
                    Console.Write("0");
                }
                // de aici ii calculat partea intreaga.
                // partea fractionara este in baza 10
                if (checkfrac == 1)
                {
                    Console.Write(",");
                    double fractie;
                    //Console.WriteLine(frac);
                    int fractieaux = frac;
                    int countfrac = -1;
                    int[] cifreb1 = new int[Convert.ToString(frac).Length];
                    for (int i = 0; i < Convert.ToString(frac).Length; i++)
                    {
                        cifreb1[i] = fractieaux % 10;
                        fractieaux /= 10;
                        // Console.WriteLine(fractieaux);
                    }
                    fractie = 0;
                    for (int i = cifreb1.Length - 1; i >= 0; i--)
                    {
                        fractie = fractie + cifreb1[i] * Math.Pow(b1, countfrac);
                        countfrac--;
                    }
                    
                    // acuma avem fractia in baza 10

                }

            }
            Console.WriteLine();
            Console.ReadKey();
        }

        private static int BaseCheck2()
        {
            string basstring = Console.ReadLine();
            int b;
            while (!int.TryParse(basstring, out b))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b1 = ");
                basstring = Console.ReadLine();
            }
            while (b < 2 || b > 16)
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b2 = ");
                basstring = Console.ReadLine();
                while (!int.TryParse(basstring, out b))
                {
                    Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                    Console.Write("b2 = ");                   
                }
            }
            return b;
        }
        private static int BaseCheck()
        {
            string basstring = Console.ReadLine();
            int b;
            while (!int.TryParse(basstring, out b))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b1 = ");
                basstring = Console.ReadLine();
            }
            while (b < 2 || b > 16)
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b1 = ");
                basstring = Console.ReadLine();
                while (!int.TryParse(basstring, out b))
                {
                    Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                    Console.Write("b1 = ");
                }
            }
            return b;
        }
    }
}
