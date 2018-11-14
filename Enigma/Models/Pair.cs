using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Models
{
    class Pair
    {
        public char FirstLetter { get; set; }
        public char SecondLetter { get; set; }

        public static void SetPair(ref List<Pair> pairs, char input, char output)
        {
            if (input == output)
                throw new Exception("Input and output char must be different!");

            if (pairs.Count == 10)
                throw new Exception("Can't set more then 10 paris!");

            if (IsPairExist(pairs, input, output))
                throw new Exception($"The char {input} and/or the char {output} already associated with other pair. You need to remove the pair first!");

            pairs.Add(new Pair()
            {
                FirstLetter = input,
                SecondLetter = output
            });
        }

        public static void RemovePair(ref List<Pair> pairs, char input, char output)
        {
            pairs.Remove(pairs.FirstOrDefault(x => x.FirstLetter == input && x.SecondLetter == output));
        }

        public static bool IsPairExist(List<Pair> pairs, char input, char output)
        {
            Pair pair = new Pair()
            {
                FirstLetter = input,
                SecondLetter = output
            };

            if (pairs.Contains(pair))
                return true;

            foreach (Pair p in pairs)
                if (p.FirstLetter == input || p.SecondLetter == input || p.FirstLetter == output || p.SecondLetter == output)
                    return true;

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", FirstLetter, SecondLetter);
        }
    }
}
