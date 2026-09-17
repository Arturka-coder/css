using System;
using System.Collections.Generic;
using System.IO;

namespace GeneticSearch
{
    class Program
    {
        struct Protein
        {
            public string name;
            public string organism; 
            public string amino_acids;
        }

        struct Command
        {
            public string name;
            public string parameter1;
            public string parameter2;
        }

        //новая
        static List<T> ReadFile<T>(string filename, Func<string[], T> parseLine)
        {
            List<T> result = new List<T>();

            if (!File.Exists(filename)) return result;

            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');
                    T item = parseLine(parts);
                    result.Add(item);
                }
            }

            return result;
        }

        static Command ParseCommand(string[] parts)
        {
            return new Command
            {
                name = parts.Length > 0 ? parts[0] : string.Empty,
                parameter1 = parts.Length > 1 ? parts[1] : string.Empty,
                parameter2 = parts.Length > 2 ? parts[2] : string.Empty
            };
        }
        static Protein ParseProtein(string[] parts)
        {
            return new Protein
            {
                name = parts.Length > 0 ? parts[0] : string.Empty,
                organism = parts.Length > 1 ? parts[1] : string.Empty,
                amino_acids = parts.Length > 2 ? Decoding(parts[2]) : string.Empty
            };
        }

        static string Encoding(string amino_acids)
        {
            string encoded = string.Empty;
            for (int i = 0; i < amino_acids.Length; i++)
            {
                char ch = amino_acids[i];
                int count = 1;
                while (i < amino_acids.Length - 1 && amino_acids[i + 1] == ch)
                {
                    count++;
                    i++;
                }
                if (count > 2) encoded += count.ToString() + ch;
                else if (count == 1) encoded += ch;
                else if (count == 2) encoded += ch.ToString() + ch;
            }
            return encoded;
        }

        static string Decoding(string amino_acids)
        {
            string decoded = string.Empty;
            for (int i = 0; i < amino_acids.Length; i++)
            {
                char ch = amino_acids[i];
                if (char.IsDigit(ch))
                {
                    if (i + 1 < amino_acids.Length)
                    {
                        char letter = amino_acids[i + 1];
                        int count = ch - '0';
                        for (int j = 0; j < count; j++)
                        {
                            decoded += letter;
                        }
                        i++;
                    }
                }
                else
                {
                    decoded += ch;
                }
            }
            return decoded;
        }

        static void HandleSearch(List<Protein> proteins, string searchSequence, StreamWriter writer)
        {
            string decodedSearch = Decoding(searchSequence);
            bool found = false;

            writer.WriteLine("organism\t\t\t\tprotein");

            for (int i = 0; i < proteins.Count; i++)
            {
                if (proteins[i].amino_acids.Contains(decodedSearch))
                {
                    writer.WriteLine(proteins[i].organism + "\t\t" + proteins[i].name);
                    found = true;
                }
            }

            if (!found)
            {
                writer.WriteLine("NOT FOUND");
            }
        }

        static void HandleDiff(List<Protein> proteins, string protein1Name, string protein2Name, StreamWriter writer)
        {
            Protein? p1 = null;
            Protein? p2 = null;

            for (int i = 0; i < proteins.Count; i++)
            {
                if (proteins[i].name == protein1Name) p1 = proteins[i];
                if (proteins[i].name == protein2Name) p2 = proteins[i];
            }

            writer.WriteLine("amino-acids difference:");

            if (!p1.HasValue || !p2.HasValue)
            {
                string missing = "";
                if (!p1.HasValue) missing += protein1Name;
                if (!p2.HasValue)
                {
                    if (missing != "") missing += ", ";
                    missing += protein2Name;
                }
                writer.WriteLine("MISSING: " + missing);
                return;
            }

            string seq1 = p1.Value.amino_acids;
            string seq2 = p2.Value.amino_acids;
            int maxLength = Math.Max(seq1.Length, seq2.Length);
            int differences = 0;

            for (int i = 0; i < maxLength; i++)
            {
                char c1 = i < seq1.Length ? seq1[i] : '\0';
                char c2 = i < seq2.Length ? seq2[i] : '\0';
                if (c1 != c2) differences++;
            }

            writer.WriteLine(differences.ToString());
        }

        static void HandleMode(List<Protein> proteins, string proteinName, StreamWriter writer)
        {
            Protein? foundProtein = null;

            for (int i = 0; i < proteins.Count; i++)
            {
                if (proteins[i].name == proteinName)
                {
                    foundProtein = proteins[i];
                    break;
                }
            }

            writer.WriteLine("amino-acid occurs:");

            if (!foundProtein.HasValue)
            {
                writer.WriteLine("MISSING: " + proteinName);
                return;
            }

            string sequence = foundProtein.Value.amino_acids;
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            for (int i = 0; i < sequence.Length; i++)
            {
                char c = sequence[i];
                if (frequency.ContainsKey(c))
                    frequency[c]++;
                else
                    frequency[c] = 1;
            }

            char mostFrequent = '\0';
            int maxCount = 0;

            List<char> sortedChars = new List<char>(frequency.Keys);
            sortedChars.Sort();

            for (int i = 0; i < sortedChars.Count; i++)
            {
                if (frequency[sortedChars[i]] > maxCount)
                {
                    maxCount = frequency[sortedChars[i]];
                    mostFrequent = sortedChars[i];
                }
            }

            writer.WriteLine(mostFrequent + "          " + maxCount);
        }

        static void CommandHandler(List<Protein> proteins, List<Command> commands, string outputFilename, string authorName)
        {
            using (StreamWriter writer = new StreamWriter(outputFilename))
            {
                writer.WriteLine(authorName);
                writer.WriteLine("Genetic Searching");
                writer.WriteLine("--------------------------------------------------------------------------");

                int commandNumber = 1;
                for (int i = 0; i < commands.Count; i++)
                {
                    Command cmd = commands[i];
                    string number = commandNumber.ToString("D3");
                    string param1 = cmd.name == "search" ? Decoding(cmd.parameter1) : cmd.parameter1;
                    string commandLine = number + "   " + cmd.name + "   " + param1;
                    if (!string.IsNullOrEmpty(cmd.parameter2))
                        commandLine += "   " + cmd.parameter2;

                    if (cmd.name == "diff") commandLine += " ";
                    if (cmd.name == "mode") commandLine += "  ";

                    writer.WriteLine(commandLine);

                    if (cmd.name == "search")
                    {
                        HandleSearch(proteins, cmd.parameter1, writer);
                    }
                    else if (cmd.name == "diff")
                    {
                        HandleDiff(proteins, cmd.parameter1, cmd.parameter2, writer);
                    }
                    else if (cmd.name == "mode")
                    {
                        HandleMode(proteins, cmd.parameter1, writer);
                    }

                    writer.WriteLine("--------------------------------------------------------------------------");
                    commandNumber++;
                }
            }
        }

        static void PrintStartupInfo(string sequencesFile, string commandsFile, string outputFile, int proteinsCount, int commandsCount)
        {
            Console.WriteLine("=== GENETIC SEARCH ===");
            Console.WriteLine($"Input sequences: {sequencesFile}");
            Console.WriteLine($"Input commands: {commandsFile}");
            Console.WriteLine($"Output file: {outputFile}");
            Console.WriteLine();
            Console.WriteLine($"Loaded {proteinsCount} proteins");
            Console.WriteLine($"Loaded {commandsCount} commands");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string sequencesFile = "sequences.0.txt";
            string commandsFile = "commands.0.txt";
            string outputFile = "genedata.txt";
            string authorName = "Артур";

            if (args.Length >= 1) sequencesFile = args[0];
            if (args.Length >= 2) commandsFile = args[1];
            if (args.Length >= 3) outputFile = args[2];
            if (args.Length >= 4) authorName = args[3];

            List<Protein> data = ReadFile(sequencesFile, ParseProtein);
            List<Command> commands = ReadFile(commandsFile, ParseCommand);

            PrintStartupInfo(sequencesFile, commandsFile, outputFile, data.Count, commands.Count);

            CommandHandler(data, commands, outputFile, authorName);

            Console.WriteLine($"Done! Output written to {outputFile}");
        }
    }
}
