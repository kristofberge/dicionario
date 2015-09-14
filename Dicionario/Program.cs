using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicionario.Exceptions;

namespace Dicionario
{
    class Program
    {
        static void Main(string[] args)
        {
            WordSearcher searcher = new WordSearcher();
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine("Entra uma palavra:");
                string word = Console.ReadLine();

                if (!string.IsNullOrEmpty(word))
                {
                    try
                    {
                        Console.WriteLine("Buscando a palavra...");
                        int position = searcher.GetPositionOfWord(word.ToUpper());
                        Console.WriteLine(string.Format("A palavra {0} foi encontrada em posição {1}.", word, position));
                    }
                    catch (WordNotFoundException)
                    {
                        Console.WriteLine(string.Format("A palavra {0} NÃO foi encontrada no dicionário.", word));
                    }
                    Console.WriteLine(string.Format("Gatinhos mortes: {0} :(", searcher.deadKittens));
                }

                char wantToContinue = ' ';
                while(Char.ToUpper(wantToContinue)!='N' && Char.ToUpper(wantToContinue) != 'Y')
                {
                    Console.WriteLine("Você quer buscar mais uma palavra? y/n:");
                    wantToContinue = (char) Console.Read();
                    Console.ReadLine();
                }
                continueLoop = Char.ToUpper(wantToContinue) == 'Y';
                Console.WriteLine();
            }
        }
    }
}
