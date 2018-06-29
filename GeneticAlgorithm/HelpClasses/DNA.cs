using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class DNA<T> : IEnumerable
    {
        private T[] Dna { get; set; }
        public int Length => Dna.Length;
        public T this[int i]
        {
            get
            {
                return Dna[i];
            }
            set
            {
                Dna[i] = value;
            }
        }
        public DNA(params T[] DnaElement)
        {
            Dna = new T[DnaElement.Length];
            Array.Copy(DnaElement, Dna, DnaElement.Length);
        }

        public IEnumerator GetEnumerator()
        {
            return Dna.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
