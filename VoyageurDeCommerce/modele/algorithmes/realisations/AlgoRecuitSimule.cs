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
    class AlgoRecuitSimule : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();

        public override string Nom => "Recuit Simulé";

        private Random rnd = new Random();


        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            //Creation tourné base 
            List<Lieu> tourNow = listeLieux;
            List<Lieu> tourBest = tourNow;
            List<Lieu> tourNew = new List<Lieu>();

            int valueNow = GetCost(tourNow);
            int valueBest = valueNow;
            //the probability
            double proba;
            double alpha = 0.98;
            double temperature = 100;
            double epsilon = 1;
            double delta;

            while (temperature > epsilon)
            {
                for (int i = 0; i < listeLieux.Count * 2; i++)
                {
                    //get the next random permutation of distances 
                    tourNew = computeNext(tourNow);
                    int valueNew = GetCost(tourNew);
                    //compute the distance of the new permuted configuration
                    delta = valueNew - valueNow;
                    //if the new distance is better accept it and assign it
                    if (delta < 0)
                    {
                        if (valueNew < valueBest)
                        {
                            tourBest = tourNew;
                            valueBest = valueNew;

                            tourNow = tourNew;
                            valueNow = valueNew;

                        }
                    }
                    else
                    {
                        proba = rnd.NextDouble();

                        if (proba < Math.Exp(-delta / temperature))
                        {
                            tourNow = tourNew;
                            valueNow = valueNew;
                        }
                    }
                }

                tourNow = ShiftRight<Lieu>(tourNow, 2);
                //cooling process on every iteration
                temperature *= alpha;

            }

            //Construction de la tournee
            foreach (Lieu lieu in tourBest)
            {
                this.Tournee.Add(lieu);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
                tempsExe.Start();
            }

            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Stop();
            

        }

        private List<Lieu> computeNext(List<Lieu> c)
        {
            List<Lieu> res = new List<Lieu>(c);

            int i, j;
            i = (int)(rnd.Next(c.Count));

            do
            {
                j = (int)(rnd.Next(c.Count));
            } while (i == j);

            Lieu aux = res[i];
            res[i] = res[j];
            res[j] = aux;

            return res;
        }

        private int GetCost(List<Lieu> l)
        {
            Tournee res = new Tournee();
            res.ListeLieux = l;

            return res.Distance;
        }

        public static List<T> ShiftRight<T>(List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(list.Count - shiftBy, shiftBy);
            result.AddRange(list.GetRange(0, list.Count - shiftBy));
            return result;
        }

    }
}
