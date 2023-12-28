﻿using System.Text.RegularExpressions;
using NAudio.Wave;

namespace Waver;

public abstract class ResampleBase
{
    private const string Wildcard = "*";
    private const string WildcardRegEx = @"[\*]";
    
    public void ProcessFiles(CommonOptions commonOptions, string defaultExtension)
    {
        foreach (var inputFileName in GetInputFileNames(commonOptions.InputFileMask))
        {
            ProcessFile(inputFileName, GetOutputFileName(inputFileName, commonOptions.OutputFileMask, defaultExtension), 
                commonOptions);
        }
    }

    protected abstract void ProcessFile(string inputFileName, string outputFileName, CommonOptions resampleOptions);
    
    public static IEnumerable<string> GetInputFileNames(string inputFileMask)
    {
        if (string.IsNullOrEmpty(inputFileMask))
        {
            throw new ArgumentException("Input file mask is null or empty.");
        }
        
        if (File.Exists(inputFileMask) && !Directory.Exists(inputFileMask))
        {
            return new List<string>() { inputFileMask };
        }

        string directory;
        var searchPattern = Wildcard;

        if (Directory.Exists(inputFileMask))
        {
            directory = inputFileMask;
        }
        else
        {
            directory = Path.GetDirectoryName(inputFileMask) ?? Directory.GetCurrentDirectory();
        
            searchPattern = Path.GetFileName(inputFileMask);
        }

        var fileList = Directory.EnumerateFiles(directory, searchPattern).ToList();

        if (fileList == null || !fileList.Any()) throw new FileNotFoundException("No input file found.", inputFileMask) ;
        
        return fileList;
    }

    public static string GetOutputFileName(string inputFileName, string outputFileMask, string defaultExtension)
    {
        if (string.IsNullOrEmpty(inputFileName))
        {
            throw new ArgumentException("Input file name is null or empty");
        }
        
        if (string.IsNullOrEmpty(defaultExtension))
        {
            defaultExtension = ".wav";
        }

        if (string.IsNullOrEmpty(outputFileMask))
        {
            outputFileMask = $"*{defaultExtension}";
        }

        if (!File.Exists(inputFileName))
        {
            throw new FileNotFoundException($"Input file not found.", inputFileName);
        }
        
        if (Directory.Exists(outputFileMask))
        {
            return Path.Combine(outputFileMask, $"{Path.GetFileNameWithoutExtension(inputFileName)}{defaultExtension}");
        }

        if (!ContainsWildcard(outputFileMask))
        {
            var directory = Path.GetDirectoryName(outputFileMask);
            var filename = $"{Path.GetFileNameWithoutExtension(outputFileMask)}{defaultExtension}";

            return string.IsNullOrEmpty(directory) ? filename : Path.Combine(directory, filename);
            
        }

        return outputFileMask.Replace(Wildcard, Path.GetFileNameWithoutExtension(inputFileName));
    }

    public static bool ContainsWildcard(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        
        var matchCount = Regex.Matches(input, WildcardRegEx).Count;

        if (matchCount > 1) throw new ArgumentException($"{input} contains multiple wildcards");

        return matchCount == 1;
    }

    protected void PrintWaveFileFormat(string fileName)
    {
        var waveReader = new WaveFileReader(fileName);
        var waveFormat = waveReader.WaveFormat;
        Console.WriteLine($"{fileName}: {waveFormat.Channels} channel(s), {waveFormat.SampleRate} Hz, {waveFormat.BitsPerSample} bit/sample, {waveFormat.Encoding}");
        
    }
}