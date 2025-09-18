using System.CodeDom;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Lleyton Eggins, Sprint 2
// Date: 18/09/25
// Version: 1.01
// Astronomical Processing
// Creates and displays a list of simulated neutrino data,
// which can be sorted, searched and edited using textboxes and buttons

namespace Astronomical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tbtnBinary.IsChecked = true;
            int[] ints = new int[24];
            Random rnd = new Random();
            for (int i = 0; i < 24; i++) // randomly assigns 24 values to an array with min 10 and max 90
            {
                ints[i] = rnd.Next(10,91);
            }
            foreach(int i in ints) //sets the list to those 24 items
            {
                lbxDataList.Items.Add(i);
            }
        }

        #region Messages
        private void DisplayMessage(string msg, string caption) // display non-error message
        {
            MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DisplayError(string msg, string caption) // display error message
        {
            MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        //private void DisplayStatus(string msg) // display status bar text (unused)
        //{
        //    txtStatus.Text = msg;
        //}
        #endregion

        #region SortSearch
        private List<int> SortList(List<int> ints) // Bubble Sort
        {
            int iterations = ints.Count; 
            for (int i = 0; i < iterations - 1; i++) // occurs one less than the array length
            {
                for (int j = 0; j < iterations - i - 1; j++) // occurs less the more items have been sorted
                {
                    if (ints[j] > ints[j + 1]) // is the next number less than the current number?
                    {
                        int temp = ints[j + 1]; // if so, swaps them using a temp variable
                        ints[j + 1] = ints[j];
                        ints[j] = temp;
                    }
                }
            }
            return ints;

            // Sort directly through C# implementations instead of Bubble Sort
            //List<int> ints = new List<int>();
            //foreach (int i in lbxDataList.Items)
            //{
            //    ints.Add(i);
            //}
            //ints.Sort();
            //lbxDataList.Items.Clear();
            //foreach (int i in ints)
            //{
            //    lbxDataList.Items.Add(i);
            //}
        }

        private int BinarySearch(int x)
        {
            List<int> ints = new List<int>();
            foreach (int i in lbxDataList.Items) //moves the datalist items into new array
            {
                ints.Add(i);
            }
            ints = SortList(ints); // always needs to sort before binary search
            lbxDataList.Items.Clear(); // remove the existing list order, then repopulate with the new ordered list
            foreach (int i in ints)
            {
                lbxDataList.Items.Add(i);
            }

            int min = 0, max = ints.Count - 1, mid;
            while (min <= max)
            {
                mid = (min + max) >> 1; // halves the integer using bitshift
                if (ints[mid] == x)
                {
                    return mid; // once a match is found, returns the index of the match and exits the function
                }

                if (ints[mid] < x)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return -1; // if btnSearch_Click sees this number, otherwise unavailable within the array bounds, it knows the search failed
            
            // Search directly through C# implementations instead of Binary Seach
            //int temp = int.Parse(tbSearch.Text);
            //bool found = lbxDataList.Items.Contains(temp);
            //if (found)
            //{
            //    lbxDataList.SelectedItem=temp;
            //    DisplayMessage("Match found!", "Found");
            //}
            //else
            //{
            //    DisplayError("No match found in list.", "Unsuccessful");
            //}
            //tbSearch.Text = string.Empty;
        }

        private int SequentialSearch (int x)
        {
            List<int> ints = new List<int>();
            foreach (int i in lbxDataList.Items) //moves the datalist items into new array
            {
                ints.Add(i);
            }
            for (int i = 0; i < ints.Count; i++) // iterates through every member of the unsorted array
            {
                if (ints[i] == x)
                {
                    return i; // if a match is found, return the index
                }
            }
            return -1;
        }

        #endregion

        #region Math
        private double AverageMean(List<int> ints) // mean calculation
        {
            double sum = 0;
            foreach (int i in ints) // sum all elements
            {
                sum += i;
            }
            double mean = sum / ints.Count; // divide by count
            return mean;
        }

        private List<int> AverageMode(List<int> ints)
        {
            List<int> mode = new List<int>();
            mode.Add(1);
            int duplicates = 1, mostFrequent = 0;
            int[] frequency = new int[ints.Last() + 1];
            for (int i=0;i<frequency.Length;i++)
            {
                frequency[i] = 0;
            }
            foreach (int i in ints)
            {
                frequency[i]++;
            }
            for (int i=0;i<frequency.Length;i++)
            {
                if (frequency[i] > mostFrequent)
                {
                    mostFrequent = frequency[i];
                    mode.Clear();
                    mode.Add(i);
                }
                else if (frequency[i] == mostFrequent)
                {
                    mode.Add(i);
                }
            }
            return mode;
        }

        private double AverageRange (List<int> ints)
        {
            double range = ints.Last() - ints.First();
            return range;
        }

        private double AverageMidExtreme (List<int> ints)
        {
            double midExtreme = (ints.Last() + ints.First()) / 2;
            return midExtreme;
        }

        #endregion
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = tbSearch.Text;
            if (int.TryParse(searchTerm, out int searchInt)) // checks if the input is valid first
            {
                int success = -2;
                if (tbtnBinary.IsChecked == true)
                {
                    success = BinarySearch(searchInt); // method for binary searching
                }
                else if (tbtnSequential.IsChecked == true)
                {
                    success = SequentialSearch(searchInt); // method for sequential searching
                }
                if (success != -1) // -1 is an error return, means no match found
                {
                    lbxDataList.SelectedItem = lbxDataList.Items.GetItemAt(success);
                    DisplayMessage($"Match found at index {lbxDataList.SelectedIndex}!", "Found"); // tells index number starting at 0
                }
                else if (success == -1) // in case of no match
                {
                    DisplayError("No match found in list.", "Unsuccessful");
                }
                else // in case of somehow entering method with neither search method checked
                {
                    DisplayError("No search method selected.", "Error");
                }
            }
            else // input error display
            {
                DisplayError("Please enter a valid integer.", "Input Error");
            }
            tbSearch.Text = string.Empty; // removes the search text
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            List<int> ints = new List<int>();
            foreach (int i in lbxDataList.Items) //moves the datalist items into new array
            {
                ints.Add(i);
            }
            ints = SortList(ints); // bubble sort method
            lbxDataList.Items.Clear(); // remove the existing list order, then repopulate with the new ordered list
            foreach (int i in ints)
            {
                lbxDataList.Items.Add(i);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string temp = tbEdit.Text;
            if (lbxDataList.SelectedItem == null) // makes sure a selection currently exists
            {
                DisplayError("Error: Selection for edit cannot be null.\nPlease select an item.", "Selection Error");
            }
            else if (int.TryParse(temp, out int value))
            {
                if (value < 10) // ensures values are within accepted bounds
                {
                    DisplayError("Error: Edit term is below accepted bounds", "Input error");
                }
                else if (value > 90)
                {
                    DisplayError("Error: Edit term is above accepted bounds", "Input error");
                }
                else // if all terms are met
                {
                    int selector = lbxDataList.SelectedIndex; // grabs the index
                    lbxDataList.Items.Insert(lbxDataList.SelectedIndex, value); // places the edited item where the old one currently is, moving it forward
                    lbxDataList.Items.Remove(lbxDataList.SelectedItem); // unedited item is still selected, now removes it
                    lbxDataList.SelectedIndex = selector; // moves the selection to the new item
                }
            }
            else // in case of bad or missing edit term
            {
                DisplayError("Please enter a valid integer.","Input Error");
            }
            tbEdit.Text = string.Empty; // removes the edit text
        }

        private void tbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == tbtnBinary)
            {
                tbtnSequential.IsChecked = false;
            }
            else if (sender == tbtnSequential)
            {
                tbtnBinary.IsChecked = false;
            }
        }

        private void tbtn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender == tbtnBinary)
            {
                tbtnSequential.IsChecked = true;
            }
            else if (sender == tbtnSequential)
            {
                tbtnBinary.IsChecked = true;
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lbxDataList.Items.Clear();
            int[] ints = new int[24];
            Random rnd = new Random();
            for (int i = 0; i < 24; i++) // randomly assigns 24 values to an array with min 10 and max 90
            {
                ints[i] = rnd.Next(10, 91);
            }
            foreach (int i in ints) //sets the list to those 24 items
            {
                lbxDataList.Items.Add(i);
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            List<int> ints = new List<int>();
            foreach (int i in lbxDataList.Items) //moves the datalist items into new array
            {
                ints.Add(i);
            }
            ints = SortList(ints);
            double mean = AverageMean(ints);
            List<int> mode = AverageMode(ints);
            double range = AverageRange(ints);
            double midExtreme = AverageMidExtreme(ints);

            tbMean.Text = mean.ToString("F", CultureInfo.InvariantCulture);
            if (mode.Count > 1)
            {
                string modeText = string.Empty;
                foreach (int i in mode)
                {
                    modeText += i.ToString("F",CultureInfo.InvariantCulture);
                    if (i != mode.Last())
                    {
                        modeText += ", ";
                    }
                }
                tbMode.Text = modeText;
            }
            else
            {
                tbMode.Text = mode.First().ToString("F", CultureInfo.InvariantCulture);
            }
            tbRange.Text = range.ToString("F", CultureInfo.InvariantCulture);
            tbMidExtreme.Text = midExtreme.ToString("F", CultureInfo.InvariantCulture);
        }
    }
}