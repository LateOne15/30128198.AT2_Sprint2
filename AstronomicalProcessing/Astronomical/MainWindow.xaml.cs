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
// Date: 25/09/25
// Version: 2.20
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
            tbtnBinary.IsChecked = true; // defaults search to binary
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
            while (min <= max) // the binary search itself
            {
                mid = (min + max) >> 1; // halves the integer using bitshift
                if (ints[mid] == x)
                {
                    return mid; // once a match is found, returns the index of the match and exits the function
                }

                if (ints[mid] < x) // shouldn't go lower, so sets minimum to one more than itself
                {
                    min = mid + 1;
                }
                else // shouldn't go higher, so sets max to one less than itself
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
            return -1; // error return; no index of -1 exists
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
                if (tbtnBinary.IsChecked == false && tbtnSequential.IsChecked == false) // in case of somehow entering method with neither search method checked
                {
                    DisplayError("No search method selected.", "Error");
                }
                else
                {
                    if (success != -1) // -1 is an error return, means no match found
                    {
                        lbxDataList.SelectedItem = lbxDataList.Items.GetItemAt(success);
                        DisplayMessage($"Match found at index {lbxDataList.SelectedIndex}!", "Found"); // tells index number starting at 0
                    }
                    else if (success == -1) // in case of no match
                    {
                        DisplayError("No match found in list.", "Unsuccessful");
                    }
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
                    RefreshStats(); // data has changed, remove statistics that are now out of date
                }
            }
            else // in case of bad or missing edit term
            {
                DisplayError("Please enter a valid integer.","Input Error");
            }
            tbEdit.Text = string.Empty; // removes the edit text
        }
    // these two methods swap the buttons depending on which one is pressed to ensure only one is enabled at a time
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
            lbxDataList.Items.Clear(); // removes old data first
            RefreshStats(); // new data means old stats are unusuable
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
            foreach (int i in lbxDataList.Items) // moves the datalist items into new array
            {
                ints.Add(i);
            }
            ints = SortList(ints); // sorts the duplicate list
            // Series of statistic methods. Mode, Range and Mid-Extreme require the list to be sorted, while Mode returns a list in case of multimodal data
            double mean = StatsMath.AverageMean(ints); 
            List<int> mode = StatsMath.AverageMode(ints);
            double range = StatsMath.AverageRange(ints);
            double midExtreme = StatsMath.AverageMidExtreme(ints);
            // statstic data display
            tbMean.Text = mean.ToString("F", CultureInfo.InvariantCulture); // decimal to two places
            tbMean.ToolTip = mean.ToString("G", CultureInfo.InvariantCulture); // displays a more accurate version in the tooltip
            if (mode.Count > 1) // if multimodal
            {
                string modeText = string.Empty;
                foreach (int i in mode)
                {
                    modeText += i.ToString("G",CultureInfo.InvariantCulture); // ignores decimals (which are zeroes) for space concerns
                    if (i != mode.Last())
                    {
                        modeText += ", "; // prints them in a comma separated list
                    }
                }
                tbModeNum.Text = "{"+mode.Count+"}"; // extra display of how many modes there are
                tbMode.ToolTip=modeText; // will display all modes, even ones that don't fit in the table, in the tooltip
                tbMode.Text = modeText; // actually prints the modes to the GUI
            }
            else // if single mode
            {
                tbMode.Text = mode.First().ToString("F", CultureInfo.InvariantCulture);
                tbMode.ToolTip = "Single Mode"; // an empty tooltip will still appear, so make it descriptive instead
                tbModeNum.Text = string.Empty; // removes the count display for single modes
            }
            tbRange.Text = range.ToString("F", CultureInfo.InvariantCulture);
            tbMidExtreme.Text = midExtreme.ToString("F", CultureInfo.InvariantCulture);
        }

        private void RefreshStats() // called for both data refresh and when data is edited, changing statistics
        {
            tbMean.Text = string.Empty;
            tbMean.ToolTip = string.Empty; // tooltips for empty text boxes shouldn't be visible but clear it anyway
            tbMode.Text = string.Empty;
            tbMode.ToolTip = string.Empty; // also removes the tooltip from mode, and the count display
            tbModeNum.Text = string.Empty;
            tbRange.Text = string.Empty;
            tbMidExtreme.Text = string.Empty;
        }
    }
}