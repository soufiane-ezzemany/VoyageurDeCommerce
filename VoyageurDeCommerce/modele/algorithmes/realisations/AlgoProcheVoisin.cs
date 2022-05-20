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
    class AlgoProcheVoisin : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();
        public override string Nom => "Plus proche voisin";
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            //Initialisation de la matrice de distances
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            //Liste des lieux à visiter
            List<Lieu> AVisiter = new List<Lieu>(listeLieux);

            //On part du premier lieu 
            Lieu depart = AVisiter[0];
            this.Tournee.Add(depart);
            tempsExe.Stop();
            this.NotifyPropertyChanged("Tournee");
            tempsExe.Start();
            AVisiter.Remove(depart);

            while (AVisiter.Count > 0)
            {
                int min = FloydWarshall.Distance(depart, AVisiter[0]);
                Lieu proche = AVisiter[0];
                //Trouver le plus proche
                for (int i = 1; i < AVisiter.Count; i++)
                {
                    if (min > FloydWarshall.Distance(depart, AVisiter[i]))
                    {
                        min = FloydWarshall.Distance(depart, AVisiter[i]);
                        proche = AVisiter[i];
                    }
                }

                //Ajouter le plus proche à la tournée
                this.Tournee.Add(proche);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
                tempsExe.Start();
                AVisiter.Remove(proche);

                //On part cette fois du voisin le plus proche
                depart = proche;
            }
            tempsExe.Stop();
            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Reset();
        }
    }
}
