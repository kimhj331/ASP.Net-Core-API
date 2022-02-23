using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerLee.API.Helpers
{
    public static class LinqExtensions
    {
        public static IQueryable<T> If<T>(
            this IQueryable<T> source,
            bool condition,
            Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }

        /// <summary>
        /// Return item and all children recursively.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="item">The item to be traversed.</param>
        /// <param name="childSelector">Child property selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this T item, Func<T, T> childSelector)
        {
            Stack<T> stack = new Stack<T>(new T[] { item });

            while (stack.Any())
            {
                T next = stack.Pop();
                if (next != null)
                {
                    yield return next;
                    stack.Push(childSelector(next));
                }
            }
        }

        /// <summary>
        /// Return item and all children recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="childSelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this T item, Func<T, IEnumerable<T>> childSelector)
        {
            Stack<T> stack = new Stack<T>(new T[] { item });

            while (stack.Any())
            {
                T next = stack.Pop();
                //if(next != null)
                //{
                yield return next;
                foreach (T child in childSelector(next))
                {
                    stack.Push(child);
                }
                //}
            }
        }

        /// <summary>
        /// Return item and all children recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="childSelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items,
          Func<T, IEnumerable<T>> childSelector)
        {
            Stack<T> stack = new Stack<T>(items);
            while (stack.Any())
            {
                T next = stack.Pop();
                yield return next;
                foreach (T child in childSelector(next))
                    stack.Push(child);
            }
        }


    }
}
