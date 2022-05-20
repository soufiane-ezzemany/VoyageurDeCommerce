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
    class TroisOpt : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();

        public override string Nom => "Trois OPT";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // On commence par la liste des lieux données et on cherche de l'améliorer
            //variable pour la gestion de la boucle
            bool amelioration = false;


            List<Lieu> listeTournee = new List<Lieu>(listeLieux);
            int n = listeLieux.Count;

            // tant que y a pas d'ameliorations 
            while (!amelioration)
            {
                int delta = 0;
                // itérer dans les lieux et inverser les lieux
                for (int i = 1; i < n; i++)
                {
                    for (int j = i + 2; j < n; j++)
                    {
                        for (int k = j + 2; k < n + 1; k++)
                        {   
                            //Calculer delta 
                            delta += reverse_segment_if_better(ref listeTournee, i,j,k);
                        }
                    }
                        
                }

                //Prendre en consideration la tournee et refaire le while
                if(delta < 0)
                {
                    this.Tournee.ListeLieux = listeTournee;
                    amelioration = true;
                }
                   

            }
            //Construction de la tournee
            //Faire une copie de la tournée juste pour l'affichage constructif dans le programme
            List<Lieu> Lieux = new List<Lieu>(this.Tournee.ListeLieux);
            //Vider la tournée
            this.Tournee.ListeLieux.Clear();
            foreach (Lieu l in Lieux)
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

        //Fonction cherhce a ameliorer une tourné pour trouver une distance plus petite
        private int reverse_segment_if_better(ref List<Lieu> tour, int i, int j, int k)
        {   
            //initialisations des lieux
            Lieu A = tour[i - 1];
            Lieu B = tour[i];
            Lieu C = tour[j - 1];
            Lieu D = tour[j];
            Lieu E = tour[k - 1];
            Lieu F = tour[k % tour.Count];

            // Initalisation des distances entre les lieux des indexes
            int d0 = (FloydWarshall.Distance(A,B) + FloydWarshall.Distance(C,D) + FloydWarshall.Distance(E,F));
            int d1 = (FloydWarshall.Distance(A,C) + FloydWarshall.Distance(B,D) + FloydWarshall.Distance(E,F));
            int d2 = (FloydWarshall.Distance(A,B) + FloydWarshall.Distance(C,E) + FloydWarshall.Distance(D,F));
            int d3 = (FloydWarshall.Distance(A,D) + FloydWarshall.Distance(E,B) + FloydWarshall.Distance(C,F));
            int d4 = (FloydWarshall.Distance(F,B) + FloydWarshall.Distance(C,D) + FloydWarshall.Distance(E,A));

            if (d0 > d1)
            {
                tour.Reverse(i, j-i);
                return -d0 + d1;
            }
            else if (d0 > d2)
            {
                tour.Reverse(j, k-j);
                return -d0 + d2;
            }
            else if (d0 > d4)
            {
                tour.Reverse(i, k-i);
                return -d0 + d4;
            }
            else if (d0 > d3)
            {
                List<Lieu> tmp = new List<Lieu>();
                for (int z = j; z < k; z++)
                    tmp.Add(tour[z]);
                for (int z = i; z < j; z++)
                    tmp.Add(tour[z]);
                int indx = 0;
                for (int f = i; f < k; f++)
                {
                    tour[f] = tmp[indx];
                    indx++;
                }
                    
                return -d0 + d3;
            }
            return 0;
        }
    }
}
