using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astronomical
{
    public class StatsMath
    {
        public static double AverageMean(List<int> ints) // mean calculation
        {
            double sum = 0;
            foreach (int i in ints) // sum all elements
            {
                sum += i;
            }
            double mean = sum / ints.Count; // divide by count
            return mean;
        }

        public static List<int> AverageMode(List<int> ints) // mode calculation
        {
            List<int> mode = new List<int>();
            mode.Add(1); // ensures the mode has something in it, just in case
            int mostFrequent = 0; // stores the highest reccurences so far. If this number increases, it removes the existing modes
            int[] frequency = new int[ints.Last() + 1]; // creates array large enough to store all possible values as frequency table
            for (int i = 0; i < frequency.Length; i++) // initialises the array
            {
                frequency[i] = 0;
            }
            foreach (int i in ints) // sets each element to the frequency of its occurence in the data list. this means this function must have a sorted list
            {
                frequency[i]++;
            }
            for (int i = 0; i < frequency.Length; i++) // mode finding
            {
                if (frequency[i] > mostFrequent) // in case of more reoccuring number
                {
                    mostFrequent = frequency[i]; // new highest occurence
                    mode.Clear(); // remove existing modes
                    mode.Add(i); // add new one
                }
                else if (frequency[i] == mostFrequent) // in case of equally occuring number
                {
                    mode.Add(i); // just add to the list
                }
            }
            return mode;
        }

        public static double AverageRange(List<int> ints)
        {
            double range = ints.Last() - ints.First(); // needs to be passed a sorted list
            return range;
        }

        public static double AverageMidExtreme(List<int> ints)
        {
            double midExtreme = (ints.Last() + ints.First()) / 2.0; // needs to be passed a sorted list. without the .0, will only return whole numbers
            return midExtreme;
        }
    }
}
