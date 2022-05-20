using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class AlgoMeilleurePermutation : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();
        public override string Nom => "Meilleure Permutation";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            //Creation d'une dictionnare avec des tournées(liste des lieux)
            //et comme valeur sa distance
            Dictionary<List<Lieu>, int> listeTournees = Permute(listeLieux);

            //On trouve la Tournee avec la plus petite distance
            List<Lieu> meilleure = listeTournees.OrderBy(x => x.Value).First().Key;

            //Afficher la Tournee
            foreach (Lieu l in meilleure)
            {
                this.Tournee.Add(l);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
                tempsExe.Start();
            }
            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Stop();
            tempsExe.Reset();
        }

        /// <summary> Calcule du factoriel </summary>
        private int factorial(int number)
        {
            if (number == 1)
                return 1;
            else
                return number * factorial(number - 1);
        }

        /// <summary>  
        /// Realiser plusiers permutations en limitant le nombre du permutation pour les liste les plus grands 
        /// </summary>
        private Dictionary<List<Lieu>, int> Permute(List<Lieu> liste)
        {
            Dictionary<List<Lieu>, int> resultat = new Dictionary<List<Lieu>, int>();

            int len = liste.Count;
            //Vue que la permutation est factioriel je limite le permutation à 2000
            int total = 50000;

            //Dans le cas où il y a moins de 9 element (9! = 362880) 
            //On prend en consideration toutes les permutation possibles (max 8! = 5040)
            if (len < 9)
                total = factorial(len);

            for (int number = 0; number < total; number++)
            {
                List<Lieu> local = new List<Lieu>(liste);
                List<Lieu> res = new List<Lieu>();
                int temp = number;

                //Composition de la permutation
                for (int divisor = len; divisor >= 1; divisor--)
                {
                    int q = temp / divisor;
                    int r = temp % divisor;

                    res.Add(local[r]);
                    local.RemoveAt(r);
                    temp = q;
                }

                resultat.Add(res, Distance(res));

            }

            return resultat;
        }

        /// <summary>Donner la distance d'apres une liste des lieux</summary>
        private int Distance(List<Lieu> liste)
        {
            Tournee temp = new Tournee();
            temp.ListeLieux = liste;

            return temp.Distance;
        }
    }

   
}
