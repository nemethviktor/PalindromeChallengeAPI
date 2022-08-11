using System.Data;

/// <summary>
/// [TASK]: 
/// "Write an application that finds the 3 longest unique palindromes in a supplied string. For the 3 longest
/// ... palindromes, report the palindrome, start index and length, in descending order of length"
/// ... Given the input string: sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop , the output should be:
/// Text: hijkllkjih, Index: 23, Length: 10
/// Text: defggfed, Index: 13, Length: 8
/// Text: abccba, Index: 5 Length: 6
/// [TASK DESCRIPTION ENDS]
/// See readme.md for details & assumptions
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        string usageWarning = "Usage is /palindrome/yourtexttxetruoy -- needs to be between 2 and 160 chars. Spaces allowed.";
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        //default
        app.MapGet("/", () => usageWarning).Produces(StatusCodes.Status418ImATeapot);

        app.MapGet("/palindrome/{input:length(2,160)}", (string input) =>
        {
            // replace spaces (%20) with spaces
            return findPalindrome.findPalindromeString(input.Replace("%20", " "));
        }).Produces(StatusCodes.Status200OK);

        // everything else - identical to default.
        app.MapGet("/{*rest}", (string rest) => usageWarning).Produces(StatusCodes.Status418ImATeapot);

        app.Run();
    }
    public class findPalindrome
    {
        public static string findPalindromeString(string strPalindromeToCheck)
        {
            DataTable dtAllPalindromes = new DataTable();
            dtAllPalindromes.Columns.Add("Palindrome", typeof(string));
            dtAllPalindromes.Columns.Add("Index", typeof(int));
            dtAllPalindromes.Columns.Add("Length", typeof(int));
            string returnString = "";
            int strLength = strPalindromeToCheck.Length;
            // for each character
            for (int thisLocation = 0; thisLocation < strLength; thisLocation++)
            {
                // starting on the left and using it as center see if the char one farther to each direction is the same
                int expandedLocationLeft = thisLocation;
                int expandedLocationRight = thisLocation + 1;
                while (expandedLocationRight < strLength && expandedLocationLeft >= 0 && strPalindromeToCheck.Substring(expandedLocationLeft, 1) == strPalindromeToCheck.Substring(expandedLocationRight, 1))
                {
                    DataRow drPalindromeRow = dtAllPalindromes.NewRow();
                    drPalindromeRow["Palindrome"] = strPalindromeToCheck.Substring(expandedLocationLeft, expandedLocationRight - expandedLocationLeft + 1);
                    drPalindromeRow["Index"] = expandedLocationLeft;
                    drPalindromeRow["Length"] = expandedLocationRight - expandedLocationLeft + 1;
                    dtAllPalindromes.Rows.Add(drPalindromeRow);

                    expandedLocationLeft--;
                    expandedLocationRight++;
                }
            }

            if (dtAllPalindromes.Rows.Count > 0)
            {
                // get distinct values
                // https://stackoverflow.com/a/1199956/3968494 (actually in the comments, not the solution itself)
                // "First()" is just what it is, as data is stored in the table, the First relevant value will be extracted. See my comment in the Assumptions block.
                DataTable dtAllPalindromesUnique = dtAllPalindromes.AsEnumerable().GroupBy(row => row.Field<string>("Palindrome")).Select(group => group.First()).CopyToDataTable();
                DataView dvAllPalindromesUniqueSorted = new DataView(dtAllPalindromesUnique);
                dvAllPalindromesUniqueSorted.Sort = "Length DESC";

                // this is a simple counter to see how many items we've exported
                int exportedRowCount = 0;

                // a list of palindromes that have been exported.
                List<string> exportedPalindromes = new List<string>();

                if (dvAllPalindromesUniqueSorted.Count > 0)
                {
                    foreach (DataRowView drPalindromeRow in dvAllPalindromesUniqueSorted)
                    {
                        // only export top 3
                        if (exportedRowCount < 3)
                        {
                            bool existsAlready = false;
                            // 'tis a bit silly but realistically tho' CPU exhaustive the list can't be particularly long at this point
                            foreach (string exportedPalindrome in exportedPalindromes)
                            {
                                // logic: if "abba" contains "bb" then mark "bb" as existing and ultimately ignore
                                if (exportedPalindrome.Contains(drPalindromeRow["Palindrome"].ToString()))
                                {
                                    existsAlready = true;
                                }
                            }
                            if (!existsAlready)
                            {
                                exportedPalindromes.Add(drPalindromeRow["Palindrome"].ToString());
                                exportedRowCount++;
                                returnString += "Text: " + drPalindromeRow["Palindrome"] + ", ";
                                returnString += "Index: " + drPalindromeRow["Index"].ToString() + ", ";
                                returnString += "Length: " + drPalindromeRow["Length"].ToString() + Environment.NewLine;
                            }
                        }
                    }
                }
            }

            else
            {
                returnString = "No palindromes found.";
            }
            return returnString;
        }
    }
}
