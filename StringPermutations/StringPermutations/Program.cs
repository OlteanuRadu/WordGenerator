using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringPermutations
{
    class Program
    {
        static void Main(string[] args)
        {
            var foundWords = new HashSet<string>();

            Enumerable
                .Range(1, "amdaris".ToCharArray().Length)
                .SelectMany(_ => Enumerable.Repeat("amdaris".ToCharArray(), _)
                .CartesianProduct())
                .Distinct()
                .Select(combination => new string(combination.ToArray()))
                .Where(_ => _.Length == 4)
                .ForEach(_ => _.CheckWordExistence(foundWords));



            Console.WriteLine($"Done...Found {foundWords.Count()} words");
            Console.ReadLine();
        }
    }

    public static class Utils
    {

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<string> GenerateStrings(this IEnumerable<char> characters, int length)
        {
            if (length > 0)
            {
                foreach (char c in characters)
                {
                    foreach (string suffix in GenerateStrings(characters, length - 1))
                    {
                        yield return c + suffix;
                    }
                }
            }
            else
            {
                yield return string.Empty;
            }
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };

            sequences.ForEach(sequence =>
            {
                result = result
                            .SelectMany(_ => sequence, (left, right) => left.Concat(new[] { right }));
            });

            return result;
        }

        public static void CheckWordExistence(this string wordToCheck, HashSet<string> items)
        {

            WordDictionary oDict = new WordDictionary();

            oDict.DictionaryFile = "en-US.dic";
            oDict.Initialize();

            NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();

            oSpell.Dictionary = oDict;
            if (oSpell.TestWord(wordToCheck))
            {
                if (wordToCheck.Length == 3) { Console.ForegroundColor = ConsoleColor.Magenta; }
                if (wordToCheck.Length == 4) { Console.ForegroundColor = ConsoleColor.Cyan; }
                if (wordToCheck.Length == 5) { Console.ForegroundColor = ConsoleColor.Green; }
                if (wordToCheck.Length == 6) { Console.ForegroundColor = ConsoleColor.Yellow; }
                if (wordToCheck.Length == 7) { Console.ForegroundColor = ConsoleColor.DarkMagenta; }

                Console.WriteLine($"{wordToCheck} was found in the dictionary");
                items.Add(wordToCheck);
            }
        }

    }
}
