using System;
using System.Collections;

namespace Algorithms
{
    /// <summary>
    /// The Bresenham algorithm 
    /// 
    /// Jason Morley's C# Version
    /// http://www.morleydev.co.uk/blog/2010/11/18/generic-bresenhams-line-algorithm-in-visual-basic-net/
    /// 
    /// </summary>
    public static class Bresenham
    {
        private static void Swap<T> (ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }            
            
        /// <summary>
        /// Plot the line from (x0, y0) to (x1, y10
        /// </summary>
        /// <param name="x0">The start x</param>
        /// <param name="y0">The start y</param>
        /// <param name="x1">The end x</param>
        /// <param name="y1">The end y</param>
        public static void Line (int x0, int y0, int x1, int y1, Func<int, int, bool> pointFunc)
        {
            bool steep = Math.Abs (y1 - y0) > Math.Abs (x1 - x0);
            if (steep) {
                Swap<int> (ref x0, ref y0);
                Swap<int> (ref x1, ref y1);
            }
            if (x0 > x1) {
                Swap<int> (ref x0, ref x1);
                Swap<int> (ref y0, ref y1);
            }
            int dX = (x1 - x0), dY = Math.Abs (y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;
                
            for (int x = x0; x <= x1; ++x) {
                if (! (steep ? pointFunc (y, x) : pointFunc (x, y)))
                    return;
                err = err - dY;
                if (err < 0) {
                    y += ystep;
                    err += dX;
                }
            }
        }
    }
}    