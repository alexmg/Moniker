using PublicApiGenerator;
using Shouldly;
using Xunit;

namespace Moniker.ApprovalTests;

public class ApiApprovalTests
{
    [Fact]
    public void PublicApiShouldNotChangeUnintentionally()
    {
        var assembly = typeof(NameGenerator).Assembly;
        var publicApi = assembly.GeneratePublicApi(
            new ApiGeneratorOptions
            {
                IncludeAssemblyAttributes = false,
                ExcludeAttributes = ["System.Diagnostics.DebuggerDisplayAttribute"],
            });

        publicApi.ShouldMatchApproved(options =>
            options.WithFilenameGenerator((_, _, fileType, fileExtension) =>
                $"{assembly.GetName().Name!}.{fileType}.{fileExtension}"));
    }
}