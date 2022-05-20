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
    /// <summary> Algorithme de Recherche locale</summary>
    class AlgorithmeRecherche : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();
        public override string Nom => "Recherche Locale";
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            //On part de la tournée obtenue avec l'algorithme du plus proche voisin
            Algorithme tourneeBase = new AlgoProcheVoisin();
            tourneeBase.Executer(listeLieux, listeRoute);
            
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            this.Tournee = tourneeBase.Tournee;
            int min = this.Tournee.Distance;

            bool continuer = true;

            while (continuer == true)
            {
                //Permet de stocker toutes les tournées voisines obtenues
                List<Tournee> tourneesVoisines = new List<Tournee>();
                tourneesVoisines.Add(this.Tournee);

                //On parcourt les lieux de la tournée et on en inverse 2 dans une variable temporaire que l'on ajoute dans la liste
                //des tournées voisines
                for (int i = 0; i < this.Tournee.ListeLieux.Count() - 2; i++)
                {
                    Tournee temp = new Tournee(this.Tournee);
                    temp.ListeLieux.Reverse(i, 2);
                    tourneesVoisines.Add(temp);
                }
                //On compare les tournées voisines avec la tournée qui a la plus petite distance
                foreach (Tournee t in tourneesVoisines)
                {
                    if (t.Distance <= min)
                    {
                        min = t.Distance;
                        this.Tournee = t;
                        continuer = true;
                    }
                    else
                    {
                        continuer = false;
                    }
                }
            }
            
            //Faire une copie de la tournée juste pour l'affichage constructif dans le programme
            List<Lieu> Lieux = new List<Lieu>(this.Tournee.ListeLieux);
            //Vider la tournée
            this.Tournee.ListeLieux.Clear();
            foreach(Lieu l in Lieux)
            {   
                this.Tournee.Add(l);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
                tempsExe.Start();
            }
            
            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Stop();

        }
    }
}
