using Dicionario.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dicionario
{
    public class WordSearcher
    {
        protected DicionarioClient Dicionario;
        public int deadKittens;

        private string _wordToSearch;

        public WordSearcher()
        {
            Dicionario = new DicionarioClient();
        }

        public int GetPositionOfWord(string word)
        {
            _wordToSearch = word;
            deadKittens = 0;

            if (!IsDicionarioEmpty())
            {
                int min = 0;
                int max = FindMax();

                try
                {
                    return DoBinarySearchOnDicionario(min, max);
                }
                catch (WordNotFoundException)
                {
                    throw;
                }
            }
            else
            {
                throw new DicionarioEmptyException();
            }
        }

        private bool IsDicionarioEmpty()
        {
            try
            {
                deadKittens++;
                Dicionario.GetWordAtPosition(0);
                return false;
            }
            catch (NoWordAtPositionException)
            {
                return true;
            }
        }

        protected virtual int FindMax(int start = 50000, int increment = 50000)
        {
            // Precisamos achar um max que aproxima o tamanho do dicionário.
            // Começamos com um valor inicial e incrementamos cada vez até sair do dicionário.
            //
            // Para escolher os valores assumi que:
            //  - um dicionário é grande: tem pelo menos alguns 10 milhares de palavras. 
            //    Por isso começamos com 50.000.
            //  - um dicionário não é MUITO grande: Na língua portuguesa existem mais ou menos 400.000 palavras.  
            //    Por isso incrementamos com 50.000.

            int max = start;
            while (true)
            {
                try
                {
                    deadKittens++;
                    Dicionario.GetWordAtPosition(max);
                    max += increment;
                }
                catch (NoWordAtPositionException)
                {
                    return max;
                }
            }
        }

        protected virtual int DoBinarySearchOnDicionario(int min, int max)
        {
            int middle = CalculateMiddle(min, max);

            try
            {
                deadKittens++;
                string wordAtMiddle = Dicionario.GetWordAtPosition(middle);
                int compare = string.Compare(wordAtMiddle, _wordToSearch);

                if (compare == 0)
                {
                    return middle;
                }
                else if (min == max || middle == 0)
                {
                    throw new WordNotFoundException();
                }
                else if (compare < 0)
                {
                    return DoBinarySearchOnDicionario(middle + 1, max);
                }
                else
                {
                    return DoBinarySearchOnDicionario(min, middle);
                }
            }
            catch (NoWordAtPositionException)
            {
                // Nosso middle é fora do dicionario. Precisamos usar um menor.
                 return DoBinarySearchOnDicionario(min, middle-1);
            }
        }

        private int CalculateMiddle(int min, int max)
        {
            return (min + ((max - min) >> 1));
        }
    }
}
