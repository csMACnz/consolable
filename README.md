# Consolable
When you get it wrong, we are here to help. Command-Line parsing for C# that is more helpful when the arguments are wrong.

## Elevator Pitch ##

Don't you hate using an application that just prints USAGE when you type something wrong? Isn't is painful when you have to read an entire string of command-line arguments to find your mistake? As a developer, do you get sick of parsing arguments to give users better feedback?  Well, do I have a solution for you!

The goals of this library are to:
* Make it clear which argument is ill-formatted
  * Easily print out which argument(s) is/are missing values
  * Easily print out which arguments don't exist
  * Easily print out full USAGE based on configuration
* In code, It should get the job done and get out of your way
* Support a flexible choice between how users want to format their arguments
  * flags support 0, 1, or many argument values
  * formats should support space, colon and equals delimiters
  * flags should work with either - or / for short, and -- for long arguments
  * eg -d:<value> /d:<value> -d=<value> /d=<value> /d value -d <value> -abcd <value> --dflag <value> --dflag=<value>
* Support optional leading mode-name argument, as well as trailing argument(s) (where applicable)

###Stretch Goals: ###

* Make Porting easy
  * Build an adapter for docopt usage.txt files
  * Build a fluent adapter similar to some cli frameworks
  * Build an attributes-driven adapter similar to some cli frameworks
* Meet as much of the [syntax standards](https://en.wikipedia.org/wiki/Command-line_interface) as possible out of the box, at the same time
* Support app modes is a convenient way, with seperate argument configurations.
