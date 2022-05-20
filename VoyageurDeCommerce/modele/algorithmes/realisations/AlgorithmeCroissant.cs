using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.parseur;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class AlgorithmeCroissant : Algorithme
    {
        private Stopwatch tempsExe = new Stopwatch();
        public override string Nom => "Tournee Croissante";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            tempsExe.Start();
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            foreach (Lieu l in listeLieux)
            {
                tempsExe.Start();
                this.Tournee.Add(l);
                tempsExe.Stop();
                this.NotifyPropertyChanged("Tournee");
            }
            tempsExe.Stop();
            this.TempsExecution = tempsExe.ElapsedMilliseconds;
            tempsExe.Reset();
        }
    }
}
