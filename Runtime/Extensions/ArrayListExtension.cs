using UnityEngine;
using System.Collections;

namespace UnityEssentials
{
    public static class ArrayListExtension
    {
        /*
         * ArrayList class extension method to add values only they are not null
         */
        public static void AddNotNull(this ArrayList collection, object entity)
        {
            if (entity != null)
            {
                collection.Add(entity);
            }
        }
        /*
         * Removes from the ArrayList those elements that exist in a given ArrayList
         */
        public static void Exclusive(this ArrayList collection, ArrayList sourceCollection)
        {
            ArrayList exclusives = new ArrayList();
            foreach (object entity in collection)
            {
                if (!sourceCollection.Contains(entity))
                {
                    exclusives.Add(entity);
                }
            }
            collection.RemoveRange(0, collection.Count);
            collection.AddRange(exclusives);
        }
        /* Removes all duplications */
        public static ArrayList Distinct(this ArrayList collection)
        {
            ArrayList result = new ArrayList();
            foreach (object entityityE in collection)
            {
                if (!result.Contains(entity))
                {
                    result.Add(entity);
                }
            }
            return result;
        }
    }
}