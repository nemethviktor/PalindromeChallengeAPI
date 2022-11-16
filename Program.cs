using System.Data;

/// <summary>
///     [TASK]:
///     "Write an application that finds the 3 longest unique palindromes in a supplied string. For the 3 longest
///     ... palindromes, report the palindrome, start index and length, in descending order of length"
///     ... Given the input string: sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop , the output should be:
///     Text: hijkllkjih, Index: 23, Length: 10
///     Text: defggfed, Index: 13, Length: 8
///     Text: abccba, Index: 5 Length: 6
///     [TASK DESCRIPTION ENDS]
///     See readme.md for details & assumptions
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        string usageWarning =
            "Usage is /palindrome/yourtexttxetruoy -- needs to be between 2 and 160 chars. Spaces allowed.";
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);
        WebApplication app = builder.Build();

        //default
        app.MapGet(pattern: "/", handler: () => usageWarning)
            .Produces(statusCode: StatusCodes.Status418ImATeapot);

        app.MapGet(pattern: "/palindrome/{input:length(2,160)}", handler: (string input) =>
            {
                // replace spaces (%20) with spaces
                return findPalindrome.findPalindromeString(strPalindromeToCheck: input.Replace(oldValue: "%20", newValue: " "));
            })
            .Produces(statusCode: StatusCodes.Status200OK);

        // everything else - identical to default.
        app.MapGet(pattern: "/{*rest}", handler: (string rest) => usageWarning)
            .Produces(statusCode: StatusCodes.Status418ImATeapot);

        app.Run();
    }

    public class findPalindrome
    {
        public static string findPalindromeString(string strPalindromeToCheck)
        {
            DataTable dtAllPalindromes = new DataTable();
            dtAllPalindromes.Columns.Add(columnName: "Palindrome", type: typeof(string));
            dtAllPalindromes.Columns.Add(columnName: "Index", type: typeof(int));
            dtAllPalindromes.Columns.Add(columnName: "Length", type: typeof(int));
            string returnString = "";
            int strLength = strPalindromeToCheck.Length;
            // for each character
            for (int thisLocation = 0; thisLocation < strLength; thisLocation++)
            {
                // starting on the left and using it as center see if the char one farther to each direction is the same
                int expandedLocationLeft = thisLocation;
                int expandedLocationRight = thisLocation + 1;
                while (expandedLocationRight < strLength &&
                       expandedLocationLeft >= 0 &&
                       strPalindromeToCheck.Substring(startIndex: expandedLocationLeft, length: 1) ==
                       strPalindromeToCheck.Substring(startIndex: expandedLocationRight, length: 1))
                {
                    DataRow drPalindromeRow = dtAllPalindromes.NewRow();
                    drPalindromeRow[columnName: "Palindrome"] = strPalindromeToCheck.Substring(startIndex: expandedLocationLeft,
                                                                                               length: expandedLocationRight - expandedLocationLeft + 1);
                    drPalindromeRow[columnName: "Index"] = expandedLocationLeft;
                    drPalindromeRow[columnName: "Length"] = expandedLocationRight - expandedLocationLeft + 1;
                    dtAllPalindromes.Rows.Add(row: drPalindromeRow);

                    expandedLocationLeft--;
                    expandedLocationRight++;
                }
            }

            if (dtAllPalindromes.Rows.Count > 0)
            {
                // get distinct values
                // https://stackoverflow.com/a/1199956/3968494 (actually in the comments, not the solution itself)
                // "First()" is just what it is, as data is stored in the table, the First relevant value will be extracted. See my comment in the Assumptions block.
                DataTable dtAllPalindromesUnique = dtAllPalindromes.AsEnumerable()
                    .GroupBy(keySelector: row => row.Field<string>(columnName: "Palindrome"))
                    .Select(selector: group => group.First())
                    .CopyToDataTable();
                DataView dvAllPalindromesUniqueSorted = new DataView(table: dtAllPalindromesUnique);
                dvAllPalindromesUniqueSorted.Sort = "Length DESC";

                // this is a simple counter to see how many items we've exported
                int exportedRowCount = 0;

                // a list of palindromes that have been exported.
                List<string> exportedPalindromes = new();

                if (dvAllPalindromesUniqueSorted.Count > 0)
                {
                    foreach (DataRowView drPalindromeRow in dvAllPalindromesUniqueSorted)
                        // only export top 3
                    {
                        if (exportedRowCount < 3)
                        {
                            bool existsAlready = false;
                            // 'tis a bit silly but realistically tho' CPU exhaustive the list can't be particularly long at this point
                            foreach (string exportedPalindrome in exportedPalindromes)
                                // logic: if "abba" contains "bb" then mark "bb" as existing and ultimately ignore
                            {
                                if (exportedPalindrome.Contains(value: drPalindromeRow[property: "Palindrome"]
                                                                    .ToString()))
                                {
                                    existsAlready = true;
                                }
                            }

                            if (!existsAlready)
                            {
                                exportedPalindromes.Add(item: drPalindromeRow[property: "Palindrome"]
                                                            .ToString());
                                exportedRowCount++;
                                returnString += "Text: " + drPalindromeRow[property: "Palindrome"] + ", ";
                                returnString += "Index: " + drPalindromeRow[property: "Index"] + ", ";
                                returnString += "Length: " + drPalindromeRow[property: "Length"] + Environment.NewLine;
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