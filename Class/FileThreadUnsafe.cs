using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class
{
    public class FileThreadUnsafe<T>
    {
        private T[] tab;
        private int tete, queue;
        static readonly object verrou = new object();
        public FileThreadUnsafe(int taille)
        {
            tab = new T[taille];
            Init();
        }
        private int Suivant(int i)
        {
            lock (verrou)
                return (i + 1) % tab.Length;
        }
        private void Init()
        {
            lock (verrou)
                tete = queue = -1;
        }
        public void Enfiler(T element)
        {
            if (Pleine())
                throw new ExceptionFilePleine();
            else if (Vide())
                tab[queue = tete = 0] = element;
            else
                tab[queue = Suivant(queue)] = element;
        }
        public void Defiler()
        {
            lock (verrou)
            {
                if (Vide())
                    throw new ExceptionFileVide();
                else if (NbElements() == 1)
                    Init();
                else
                    tete = Suivant(tete);
            }
        }

        public bool Vide()
        {
            return (tete == -1) && (queue == -1);
        }
        public bool Pleine()
        {
            return Suivant(queue) == tete;
        }

        public int NbElements()
        {
            lock (verrou)
            {
                if (Vide())
                    return 0;
                else if (tete <= queue)
                    return queue - tete + 1;
                else
                    return tab.Length + queue - tete + 1;
            }
        }

        public T Premier() {
            lock (verrou)
            {
                if (Vide())
                    throw new ExceptionFileVide();
                else
                    return tab[tete];
            }
        }
    }
    public class ExceptionFileVide : Exception { }
    public class ExceptionFilePleine : Exception { }
}