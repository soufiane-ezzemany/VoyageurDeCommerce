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
    class AlgoInsertionProche : Algorithme
    {
        public override string Nom => "Insertion proche";

        private Stopwatch sw = new Stopwatch();


        /// <summary>
        /// Foction pour calculer la distance entre un lieu et un couple
        /// </summary>
        /// <param name="lieu">Le lieu à calculer la distance</param>
        /// <param name="a">elem du couple</param>
        /// <param name="b">elem du couple</param>
        /// <returns></returns>
        private int DistanceLieuFromCouple(Lieu lieu, Lieu a, Lieu b)
        {
            return FloydWarshall.Distance(lieu, a) + FloydWarshall.Distance(lieu, b) - FloydWarshall.Distance(a, b);
        }

        /// <summary>
        /// Fonction pour calculer la distance entre un lieu et une tournée
        /// </summary>
        /// <param name="l">le lieu</param>
        /// <param name="listeTournee">la tournée</param>
        /// <param name="pos"> parametre passé en reference pour prendre en consideration sa position dans la tournée</param>
        /// <returns>La distance minimale</returns>
        private int DistanceLieuFromTournee(Lieu l, List<Lieu> listeTournee)
        {
            //Pour initialiser
            int min = DistanceLieuFromCouple(l, listeTournee[0], listeTournee[1]);

            for (int i = 1; i <= listeTournee.Count - 1; i++)
            {
                if (min > DistanceLieuFromCouple(l, listeTournee[i], listeTournee[(i + 1) % listeTournee.Count]))
                {
                    min = DistanceLieuFromCouple(l, listeTournee[i], listeTournee[(i + 1) % listeTournee.Count]);
                }
            }

            return min;
        }

        /// <summary>
        /// Fonction pour trouver la meilleur position pour plaser un lieu dans une tournée
        /// </summary>
        /// <param name="l"></param>
        /// <param name="listeTournee"></param>
        /// <returns></returns>
        private Lieu TrouverPosition(Lieu l, List<Lieu> listeTournee)
        {
            int min = DistanceLieuFromCouple(l, listeTournee[0], listeTournee[1]);
            Lieu pos = listeTournee[1];

            for (int i = 1; i <= listeTournee.Count - 1; i++)
            {
                if (min > DistanceLieuFromCouple(l, listeTournee[i], listeTournee[(i + 1) % listeTournee.Count]))
                {
                    min = DistanceLieuFromCouple(l, listeTournee[i], listeTournee[(i + 1) % listeTournee.Count]);
                    pos = listeTournee[(i + 1) % listeTournee.Count];
                }
            }

            return pos;
        }



        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            sw.Start();

            //Liste des lieux à visiter
            List<Lieu> AVister = new List<Lieu>(listeLieux);

            //Liste des lieux pour remplire la tournée aprés
            List<Lieu> listeTournee = new List<Lieu>();

            //Trouver les deux lieux les plus loin
            Lieu start = listeLieux[0];
            Lieu end = listeLieux[1];
            int max = FloydWarshall.Distance(start, end);
            foreach (Lieu i in listeLieux)
            {
                foreach (Lieu j in listeLieux)
                {
                    if (max < FloydWarshall.Distance(i, j))
                    {
                        max = FloydWarshall.Distance(i, j);
                        start = i;
                        end = j;
                    }
                }
            }
            //Les enlever de la liste a visiter
            AVister.Remove(start);
            AVister.Remove(end);

            //Creer une tournée à partir des deux
            this.Tournee.Add(start);
            this.Tournee.Add(end);

            sw.Stop();
            this.NotifyPropertyChanged("Tournee");
            sw.Start();

            listeTournee.Add(start);
            listeTournee.Add(end);


            //Tant que la liste n'est pas vide
            while (AVister.Count > 0)
            {
                //Initialisation pour trouver le lieu le plus proche de la tournée
                Lieu trouve = AVister[0];
                int min = DistanceLieuFromTournee(trouve, listeTournee);

                //Trouver le lieu le plus proche de la tournée
                for (int i = 1; i < AVister.Count; i++)
                {
                    if (min > DistanceLieuFromTournee(AVister[i], listeTournee))
                    {
                        min = DistanceLieuFromTournee(AVister[i], listeTournee);
                        trouve = AVister[i];
                    }
                }
                //Meilleure position pour placer le lieu proche
                Lieu pos = TrouverPosition(trouve, listeTournee);
                //L'inserer dans la position adequate
                int index = listeTournee.IndexOf(pos);
                listeTournee.Insert(index, trouve);
                this.Tournee.ListeLieux = listeTournee;
                sw.Stop();
                this.NotifyPropertyChanged("Tournee");
                sw.Start();

                AVister.Remove(trouve);

            }

            sw.Stop();
            this.TempsExecution = sw.ElapsedMilliseconds;

        }
    }
}
