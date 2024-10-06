using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using BPI.Models;

namespace BPI
{
    public class Core
    {
        public void ScanBinary(string path, ConfigurationModel? configuration)
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<Core> logger = loggerFactory.CreateLogger<Core>();
            int bufferSize = FindBufferSize(configuration?.patterns);

            if (bufferSize == 0)
            {
                logger.LogWarning("No patterns found to scan.");
                return;
            }

            if (CheckFile(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        byte[] buffer = new byte[bufferSize];
                        while (reader.Read(buffer, 0, bufferSize) > 0)
                        {
                            bool found = ScanChunk(configuration?.patterns, buffer);
                            if (found)
                            {
                                logger.LogInformation($"Pattern matched in file: {path}");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                logger.LogError($"Binary file '{path}' not found");
            }
        }

        private int FindBufferSize(List<PatternModel> patterns)
        {
            int max = 0;

            if (patterns == null || patterns.Count == 0)
            {
                return max;
            }

            foreach (PatternModel pattern in patterns)
            {
                if (pattern?.bytes != null && pattern.bytes.Length > max)
                {
                    max = pattern.bytes.Length;
                }
            }


            return max;
        }

        private bool ValidatePattern(PatternModel pattern, byte[] chunk)
        {
            if (pattern == null || pattern.bytes == null || chunk.Length < pattern.bytes.Length)
            {
                return false;
            }

            for (int i = 0; i <= chunk.Length - pattern.bytes.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.bytes.Length; j++)
                {
                    if (pattern.bytes[j] != chunk[i + j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ScanChunk(List<PatternModel> patterns, byte[] chunk)
        {
            if (patterns == null || chunk == null || chunk.Length == 0)
            {
                return false;
            }

            foreach (PatternModel pattern in patterns)
            {
                if (ValidatePattern(pattern, chunk))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckFile(string path)
        {
            return File.Exists(path);
        }
    }
}
