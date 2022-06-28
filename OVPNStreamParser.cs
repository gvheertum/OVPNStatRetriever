using System.Diagnostics;

public class OVPNStreamParser 
{
    public IEnumerable<OVPNConnection> GetConnectionsFromStream(string stream) 
    {
        Debug.WriteLine($"Got stream: {stream}");
        var relevantLines = GetRelevantLines(stream);
        
        // Parse and return the lines
        Debug.WriteLine($"Found {relevantLines.Count()} relevant lines");
        foreach(var l in relevantLines) { yield return ParseString(l); }
    }

    public List<string> GetRelevantLines(string stream) 
    {
        var lines = stream.Split("\n");
        Debug.WriteLine($"Processing {lines.Length} lines");
        int idxStart = -1;
        int idxEnd = -1;

        // Find the first occurence of the elements we are finding our data between
        for(int i = 0; i < lines.Length; i++) 
        {
            var currLine = lines[i];
            if(idxStart < 0 && currLine.IndexOf("CLIENT LIST") > -1) { idxStart = i; }
            if(idxEnd < 0 && currLine.IndexOf("ROUTING TABLE") > -1) { idxEnd = i; }
        }
        if(idxStart < 0 || idxEnd < 0) 
        {
            Debug.WriteLine("No end and/or start found for client list");
            return new List<string>();
        }
        return lines
            .Skip(idxStart) // Skip the first information until the lines we need
            .Take(idxEnd-idxStart) // Take the number of lines we want to have
            .Skip(3) // Skip the header indicator, timestamp and header of the table
            .ToList();
    }

    public OVPNConnection ParseString(string line) 
    {
        Debug.WriteLine($"Processing: {line}");
        var col = line.Split(",");
        return new OVPNConnection() 
        {
            CommonName = GetStringSafe(col, 0),
            RealAdress = GetStringSafe(col, 1),
            BytesReceived = GetStringSafe(col, 2),
            BytesSent = GetStringSafe(col, 3),
            ConnectedSince = GetStringSafe(col, 4)
        };
    }

    public string? GetStringSafe(string[] col, int desiredIndex, string? defaultValue = "UNKNOWN") 
    {
        return col.Length >= desiredIndex ? (col[desiredIndex] ?? defaultValue) : defaultValue;
    }
}
