using System.Text;


public static class DataFormatCsv
{
    /// <summary>
    /// Turn a string into a CSV cell output
    /// http://stackoverflow.com/questions/6377454/escaping-tricky-string-to-csv-format
    /// </summary>
    public static string EncodeStringForCSV(string str)
    {
        str = str.Replace("\n", "<br/>");
        bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
        if (mustQuote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char nextChar in str)
            {
                sb.Append(nextChar);
                if (nextChar == '"')
                    sb.Append("\"");
            }

            sb.Append("\"");
            return sb.ToString();
        }

        return str;
    }

    public static string EncodeObjectForCSV(object value)
    {
        if (value is DateTime || value is DateTime?)
        {
            return EncodeStringForCSV(((DateTime) value).ToString("O"));
        }

        return EncodeStringForCSV(value.ToString());
    }
}