using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CompTemplate
{
    public partial class Q1Form : Form
    {
        // Variable representing the path of the data file chosen by user
        public string path;

        // Neandrethal words, divided into 2 lists for less items to iterate through
        string[] o_words = { "ook", "ookook", "oog", "ooga", "oogam", "oogum" };
        string[] um_words = { "mook", "mookmook", "ug", "ugug" };

        // Hashtable to store all previously calculated strings
        Hashtable previouslyCalculatedStrings = new Hashtable();

        public Q1Form()
        {
            InitializeComponent();
        }

        // Start button pressed, starting to solve the problem
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartSolution();
        }

        // Open FileDialog to allow user to choose their input file
        private void mniOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog datafileSelect = new OpenFileDialog();

            if (datafileSelect.ShowDialog() == DialogResult.OK)
            {
                path = datafileSelect.FileName;
            }

            btnStart.Enabled = true;
            lblStatus.Text = "Data file loaded. Press start...";
        }

        // Function to output on the screen. If you set nl to false, it will not add a new line after the thing you're printing.
        public void write(string text, bool nl = true)
        {
            tbxOutput.AppendText(text + (nl ? Environment.NewLine : ""));
        }

        // Main recursive function. Takes a string s to be searched.
        int neanNumbers(string s)
        {
            // Variable that tells you how many possible numbers can be
            // created with the current substring s
            int poss = 0;

            // Base Case: no characters left means a successful collection of numbers
            if (s.Length == 0)
            {
                return 1;               
            }

            // Dynamic programming: Check if this string has already been calculated,
            // in which case set poss to be the previously calculated value
            else if (previouslyCalculatedStrings.Contains(s))
            {
                return (int) previouslyCalculatedStrings[s];
            }

            // Not base case or previously calculated
            else
            {
                // Contains a list where only the relevant words exist
                string[] wordlist;

                // If the first letter is o, only search through words that start with o
                if (s[0] == 'o')
                {
                    wordlist = o_words;
                }
                // Otherwise, search all other words
                else
                {
                    wordlist = um_words;
                }
        
                // For every word in wordlist, see if that word is a possible new path
                foreach (string word in wordlist)
                {
                    bool equal = true;

                    // Making sure that the current word isn't longer than the current
                    // string. If it is, then obviously it isn't a possible path
                    if (word.Length <= s.Length)
                    {
                        // Iterating through each character in word and string
                        // To see if they are the same
                        for (int i = 0; i < word.Length; i++)
                        {  
                            if (s[i] != word[i])
                            {
                                // Word doesn't equal the string. set equal to false and break from the loop.
                                equal = false;
                                break;
                            }
                        }

                        // If the string and word are equal, figure out how many possibilities there are
                        // In a substring that doesn't contain the current word.
                        if (equal)
                        {
                            poss += neanNumbers(s.Substring(word.Length));                           
                        }
                    }    
                }
            }
            
            // We have calculated the value of this string, so we can add it to the
            // hastable of previously calculated strings for future reference
            previouslyCalculatedStrings.Add(s, poss);
            
            // Return the number of possibilities in the current substring.
            return poss;
        }

        // Function to be called when it is time to solve the problem
        public void StartSolution()
        {
            // Read all of the input data and store it in the list data
            List<string> data = new List<string>(System.IO.File.ReadAllLines(path));

            // Loop for iterating through cases
            for (int CaseNum = 0; CaseNum < 10; CaseNum++)
            {
                // Problem contains the currrent neandrethal number to be calculated
                string problem = data[CaseNum];

                // Calculate and write how many possibilities there are in the current problem
                int possibilities = neanNumbers(problem);
                write(Convert.ToString(possibilities));
            }       
        }
    }
}
