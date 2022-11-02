using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace conversie_intre_baze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseCalc();
        }
        private static void BaseCalc()
        {
            int b1, b2, intreg = 0, frac = 0, fraccifre0 = 0;
            bool checkfrac = false;
            string conv, cifrefstring;
            bool perioada = false;
            char[] hex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            char[] fracchar = new char[] { ',', '.' };
            Console.WriteLine("Introduceti baza din care vreti sa convertiti (b1): ");
            b1 = BaseCheck();
            Console.WriteLine("Introduceti baza in care vreti sa convertiti (b2): ");
            b2 = BaseCheck2();
            Console.WriteLine("Introduceti numarul pe care vreti sa il convertiti: ");
            conv = CONV(ref b1, ref hex);
            Console.WriteLine($"\nNumarul {conv} din baza {b1} in baza {b2} este: \n");
            if (conv.Contains('-')) // in caz de nr negativ, folosim semnul negativ in fata nr convertit
            {
                conv = conv.Replace("-", "");
                Console.Write("-");
            }
            try
            {
                checked // cazul in care se fac calcule cu nr foarte mari, sa nu returneze o valoare corupta dintr-un overflow
                {     
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
                                Console.WriteLine("Nr introdus gresit!!!!");
                                return;
                            }
                            checkfrac = true;
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
                        for (int i = cifre.Length - 1; i >= 0; i--)
                        {
                            intreg = intreg + cifreint[i] * (int)Math.Pow(b1, countcif);
                            countcif++;
                        }
                        //// de aici partea fractiala
                        if (checkfrac)
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
                            while (fractie != 0 && countperiod < 9) 
                            {
                                cifref = 10 * cifref + (int)Math.Floor(fractie * 10);
                                fractie = (fractie * 10) - (int)Math.Floor(fractie * 10);
                                countperiod++;
                                //Console.WriteLine(countperiod);
                            }
                            cifrefstring = Convert.ToString(cifref); // necesar ca am calculat in double si se pierd 0-uri dupa virgula. 
                            while (countperiod != Convert.ToString(cifref).Length)
                            {
                                fraccifre0++;
                                countperiod--;
                            }
                            while (fraccifre0 != 0)
                            {
                                cifrefstring = string.Concat("0",Convert.ToString(cifref));
                                fraccifre0--;
                            }
                            conv = string.Concat(Convert.ToString(intreg), ',', cifrefstring); // returneaza un nr in baza 10 cu virgula fixa
                        }
                        else
                        {
                            conv = Convert.ToString(intreg); // returneaza doar partea intreaga
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
                        checkfrac = true;
                    }
                    else
                    {
                        intreg = int.Parse(conv);
                    }
                    int intb10 = 0;
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

                        if (checkfrac)
                        {
                            Console.Write(",");
                            decimal fractie;
                            //Console.WriteLine(conv.Split(fracchar)[1]);               
                            int cifref = 0, countfrac = -1;
                            char[] cifrefrac = conv.Split(fracchar)[1].ToCharArray();
                            int[] cifreb1 = new int[cifrefrac.Length];
                            for (int i = 0; i < cifrefrac.Length; i++)
                            {
                                cifreb1[i] = int.Parse(Convert.ToString(cifrefrac[i]));
                                //Console.WriteLine(cifreb1[i]);
                            }
                            fractie = 0;
                            for (int i = 0; i < cifreb1.Length; i++)
                            {
                                fractie = fractie + cifreb1[i] * (decimal)Math.Pow(b1, countfrac);
                                //Console.WriteLine(fractie);
                                countfrac--;
                            }
                            // acuma avem fractia in baza 10
                            int countperiod = 0;
                            decimal[] fractierest = new decimal[15];
                            while (fractie != 0 && countperiod < 15)
                            {
                                cifref = 10 * cifref + (int)Math.Floor(fractie * b2);
                                fractie = (fractie * b2) - (int)Math.Floor(fractie * b2);
                                fractierest[countperiod] = fractie;
                                for (int i = 0; i <= countperiod - 1; i++)
                                {
                                    //Console.WriteLine(fractierest[0]);
                                    //Console.WriteLine(fractie);
                                    if (Decimal.Compare(fractierest[i], fractie) == 0) // de ce nu functioneaza?????
                                    {
                                        //Console.Write("da");
                                        perioada = true;
                                        break;
                                    }
                                }
                                if (perioada) break;
                                countperiod++;
                            }
                            cifrefstring = Convert.ToString(cifref);  // necesar ca am calculat in int si se pierd 0-uri dupa virgula
                            while (countperiod != Convert.ToString(cifref).Length)
                            {
                                fraccifre0++;
                                countperiod--;
                            }
                            while (fraccifre0 != 0)
                            {
                                cifrefstring = string.Concat("0", Convert.ToString(cifref));
                                fraccifre0--;
                            }
                            Console.Write(cifrefstring);
                        }
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
                        string[] hexb = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

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
                        // de aici ii de calculat partea intreaga.
                        if (checkfrac)
                        {
                            Console.Write(",");
                            decimal fractie;
                            //Console.WriteLine(conv.Split(fracchar)[1]);               
                            int countfrac = -1;
                            char[] cifrefrac = conv.Split(fracchar)[1].ToCharArray(); // se stocheaza toate cifrele dupa , intr-un tablou ( sa nu se piarda 0-ul de la inceput daca este )
                            int[] cifreb1 = new int[cifrefrac.Length];
                            for (int i = 0; i < cifrefrac.Length; i++)
                            {
                                cifreb1[i] = int.Parse(Convert.ToString(cifrefrac[i]));
                                //Console.WriteLine(cifreb1[i]);
                            }
                            fractie = 0;
                            for (int i = 0; i < cifreb1.Length; i++)
                            {
                                fractie = fractie + cifreb1[i] * (decimal)Math.Pow(b1, countfrac);
                                //Console.WriteLine(fractie);
                                countfrac--;
                            }
                            // de aici avem fracie in format 0.XXXXX in baza 10
                            int countperiod = 0;
                            string[] cifresuperior = new string[15];
                            decimal[] fractierest = new decimal[15]; // sa calculeze o perioada pana la 15 cifre.
                            while (fractie != 0 && countperiod < 15)
                            {
                                // Console.WriteLine("f" + fractie);

                                cifresuperior[countperiod] = Convert.ToString((int)Math.Floor(fractie * b2));
                                fractie = (fractie * b2) - (int)Math.Floor(fractie * b2);
                                fractierest[countperiod] = fractie;
                                for (int i = 0; i <= countperiod - 1; i++)
                                {
                                    //Console.WriteLine(fractierest[0]);
                                    //Console.WriteLine(fractie);
                                    if (Decimal.Compare(fractierest[i], fractie) == 0)
                                    {
                                        //Console.Write("da");
                                        perioada = true;
                                        break;
                                    }
                                }
                                if (perioada) break;
                                // Console.WriteLine("d" + fractierest[countperiod]);
                                countperiod++;
                            }

                            for (int i = 0; i < cifresuperior.Length; i++)
                            {
                                for (int j = 0; j < hexb.Length; j++)
                                {
                                    if (cifresuperior[i] == Convert.ToString(j))
                                    {
                                        cifresuperior[i] = hexb[j];
                                    }
                                }
                                //Console.WriteLine(cifrefrac[i]);
                            }
                            for (int i = 0; i < cifresuperior.Length; i++)
                            {
                                Console.Write(cifresuperior[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Convertorul nu functioneaza pt numere introduse mai mari de " + Int32.MaxValue + ".");
            }
            if (perioada)
            {
                Console.Write("... numarul contine o perioada. Perioada incepe cu ultima cifra.");
            }
            Console.WriteLine("\nPentru a iesi din aplicatie apasati orice buton.");
            Console.ReadKey();
        }
        private static string CONV(ref int b1, ref char[] hex) 
        {
            tryagain:           
            string conv = Console.ReadLine();
            string convaux = conv;
            char[] sep = new char[] { ',', '.' };
            bool fractie = false;
            if (conv.Contains(',') || conv.Contains('.'))
            {
                convaux = conv.Split(sep)[0];
                fractie = true;
            }
            testbase:
            int basecount = 0;
            char[] cifre = convaux.ToCharArray();
            for (int j = 0; j < cifre.Length; j++)
            {            
                for (int i = 0; i < b1; i++)
                {                   
                    if (cifre[j] == hex[i])
                    {
                        basecount++;
                    }
                }
            }
            if (basecount != cifre.Length || convaux == "")
            {
                Console.WriteLine("Numarul introdus contine cifre necorespunzatoare bazei alese! Incearca din nou!");
                goto tryagain;
            }
            if (fractie)
            {
                convaux = conv.Split(sep)[1];
                fractie = false; // s-a rulat odata sa nu se mai ruleze.
                goto testbase; // nu-mi place sa folosesc goto dar nu stiu cum sa revin la testul de inainte fara sau scriu tot aici sub if
            }
            return conv;
                
        }
        private static int BaseCheck2()
        {
            string basstring = Console.ReadLine();
            int b;
            tryagain:
            while (!int.TryParse(basstring, out b))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b2 = ");
                basstring = Console.ReadLine();
            }
            if (b < 2 || b > 16)
            {
                basstring = "wrong";
                goto tryagain; 
            }
            return b;
        }
        private static int BaseCheck()
        {
            string basstring = Console.ReadLine();
            int b;
            tryagain:
            while (!int.TryParse(basstring, out b ))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b1 = ");
                basstring = Console.ReadLine();
            }
            if (b < 2 || b > 16)
            {
                basstring = "wrong";
                goto tryagain; // necesar sa obtinem o baza corespunzatoare, altfel se produc erori de calcul grave.
            }
            return b;
        }
    }
}
