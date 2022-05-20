using System.Collections.Generic;
using VoyageurDeCommerce.exception.realisations;
using VoyageurDeCommerce.modele.algorithmes.realisations;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes
{
    /// <summary> Fabrique des algorithmes </summary>
    public class FabriqueAlgorithme
    {
        /// <summary>
        /// Méthode de fabrication
        /// </summary>
        /// <param name="type">Type de l'algorithme à construire</param>
        /// <param name="listeLieux">Liste des lieux</param>
        /// <returns>L'algorithme créé</returns>
        public static Algorithme Creer(TypeAlgorithme type)
        {
            Algorithme algo;
            switch (type)
            {
                case TypeAlgorithme.CROISSANT: algo = new AlgorithmeCroissant(); break;
                case TypeAlgorithme.PROCHEVOISIN: algo = new AlgoProcheVoisin(); break;
                case TypeAlgorithme.VOISINAGE: algo = new AlgorithmeRecherche(); break;
                case TypeAlgorithme.INSERTIONLOIN: algo = new AlgoInsertionLoin(); break;
                case TypeAlgorithme.INSERTIONPROCHE: algo = new AlgoInsertionProche(); break;
                case TypeAlgorithme.MEILLEUREPERMUTATION: algo = new AlgoMeilleurePermutation(); break;
                case TypeAlgorithme.RECUITSIMULE: algo = new AlgoRecuitSimule(); break;
                case TypeAlgorithme.DEUXOPT: algo = new DeuxOpt(); break;

                default: throw new ExceptionAlgorithme("Vous n'avez pas modifié la fabrique des algorithmes !");
            }

            return algo;
        }
    }
}
