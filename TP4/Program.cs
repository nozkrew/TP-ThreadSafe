using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class;
using System.Threading;

namespace TP4
{
    class Program
    {
        static Thread th1;
        static Thread th2;
        static FileThreadUnsafe<int> f;
        static void Main(string[] args)
        {
            f = new FileThreadUnsafe<int>(10);

            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    f.Enfiler(i);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur : " + e.Message);
                }
            }
            AfficherNBElement(f.NbElements());

            th1 = new Thread(Lancement1);
            th2 = new Thread(Lancement2);

            th1.Start();
            th2.Start();

            th1.Join();
            th2.Join();

            Console.ReadLine();
        }

        public static void Lancement1()
        {
            Console.WriteLine("Lancement 1");
            if (!f.Vide())
            {
                try
                {
                    f.Defiler();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Erreur " + e.Message);
                }
            }
            AfficherNBElement(f.NbElements());
        }

        public static void Lancement2()
        {
            Console.WriteLine("Lancement 2");
            while (!f.Vide())
            {
                try
                {
                    f.Defiler();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Erreur " + e.Message);
                }

            }
            AfficherNBElement(f.NbElements());
        }

        public static void AfficherNBElement(int nb)
        {
            Console.WriteLine("Nb elements : " + nb);
        }
    }
}
