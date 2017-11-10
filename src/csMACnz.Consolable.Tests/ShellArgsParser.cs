using System.Collections.Generic;
using System.Text;

namespace csMACnz.Consolable.Tests
{
    public static class ShellArgsParser
    {
        public static string[] Parse(string rawArgs)
        {
            var result = new List<string>();

            var inquote = false;
            var currentIndex = 0;

            while (true)
            {
                if (currentIndex != rawArgs.Length)
                {
                    while (currentIndex != rawArgs.Length && (rawArgs[currentIndex] == ' ' || rawArgs[currentIndex] == '\t'))
                    {
                        currentIndex++;
                    }
                }

                if (currentIndex == rawArgs.Length)
                {
                    break;
                }

                var currentResult = new StringBuilder();

                while (true)
                {
                    var copychar = true;
                    var numslash = 0;

                    while (currentIndex != rawArgs.Length && rawArgs[currentIndex] == '\\')
                    {
                        currentIndex++;
                        ++numslash;
                    }

                    if (currentIndex != rawArgs.Length && rawArgs[currentIndex] == '"')
                    {
                        if (numslash % 2 == 0)
                        {
                            if (inquote && rawArgs.Length != currentIndex + 1 && rawArgs[currentIndex + 1] == '"')
                            {
                                currentIndex++;
                            }
                            else
                            {
                                copychar = false;
                                inquote = !inquote;
                            }
                        }

                        numslash /= 2;
                    }

                    while (numslash != 0)
                    {
                        numslash--;
                        currentResult.Append('\\');
                    }

                    if (currentIndex == rawArgs.Length || (!inquote && (rawArgs[currentIndex] == ' ' || rawArgs[currentIndex] == '\t')))
                    {
                        break;
                    }

                    if (copychar)
                    {
                        currentResult.Append(rawArgs[currentIndex]);
                    }

                    currentIndex++;
                }

                result.Add(currentResult.ToString());
            }

            return result.ToArray();
        }
    }
}
