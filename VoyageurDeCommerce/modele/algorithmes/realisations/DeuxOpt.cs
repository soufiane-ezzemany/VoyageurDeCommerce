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
    class DeuxOpt : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();
        public override string Nom => "Deux OPT";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // On commence par la liste des lieux données et on cherche de l'améliorer
            Tournee t = new Tournee();
            t.ListeLieux = listeLieux;
            //variable pour la gestion de la boucle
            bool amelioration = false;
            // Meilleure distance pour le moment
            int meilleurDistance = t.Distance;
            // copier

            while (!amelioration)
            {   
                // itérer dans les lieux et inverser les lieux
                for (int i = 0; i < t.ListeLieux.Count -1 ; i++)
                {
                    for(int j = i + 1; j < t.ListeLieux.Count; j++)
                    {
                        Tournee tourneDeTest = DeuxOptSwap(t, i, j);

                        //Si la nouvelle distance est mieux alors, on prend la nouvelle tournee
                        if(tourneDeTest.Distance < meilleurDistance)
                        {
                            t = tourneDeTest;
                            meilleurDistance = tourneDeTest.Distance;
                            //Pour recommecer la boucle
                            amelioration = true;
                        }
                    }
                }
                
            }

            //Construction de la tournee
            foreach (Lieu lieu in t.ListeLieux)
            {
                this.Tournee.Add(lieu);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
                tempsExe.Start();
            }

            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Stop();
            tempsExe.Reset();
        }

        //Inverser les lieux en utilisant la mainere de 2-opt
        private Tournee DeuxOptSwap(Tournee t, int i, int j)
        {
            Tournee res = new Tournee();

            // Prendre les lieux de debut jusqu'à i et les inserer
            for (int k = 0; k <= i - 1; k++)
            {
                res.Add(t.ListeLieux[k]);
            }

            // Prendre les lieux de i jusqu'à j en les inversent
            int inv = 0;
            for (int k = i; k <= j; k++)
            {
                res.Add(t.ListeLieux[j - inv]);
                inv++;
            }

            // insérer le restant de la tournee
            for (int k = j + 1; k < t.ListeLieux.Count; k++)
            {
                res.Add(t.ListeLieux[k]);
            }

            return res;
        }
    }
}
