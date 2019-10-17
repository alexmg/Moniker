using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;

namespace Moniker.Cli
{
    [Command(Name = "moniker", FullName = "moniker", Description = "Moniker command line interface")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    internal class Program
    {
        private readonly IConsole _console;

        public Program(IConsole console) => _console = console;

        [Option("-s|--style <STYLE>",
            "The style of moniker to generate (Moniker or Moby)",
            CommandOptionType.SingleValue)]
        [UsedImplicitly]
        public MonikerStyle MonikerStyle { get; } = MonikerStyle.Moniker;

        [Option("-d|--delimiter <DELIMITER>",
            "The delimiter to use between name parts such as adjective and noun",
            CommandOptionType.SingleValue)]
        [UsedImplicitly]
        public string Delimiter { get; } = NameGenerator.DefaultDelimiter;

        [UsedImplicitly]
        public int OnExecute()
        {
            var moniker = NameGenerator.Generate(MonikerStyle, Delimiter);
            _console.Write(moniker);
            return 0;
        }

        private static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        private static string GetVersion() =>
            (string)typeof(Program).Assembly
                .GetType(nameof(GitVersionInformation))
                .GetField(nameof(GitVersionInformation.LegacySemVer))
                .GetValue(null);
    }
}
