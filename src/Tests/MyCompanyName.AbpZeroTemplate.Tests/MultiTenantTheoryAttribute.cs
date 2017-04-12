using Xunit;

namespace MyCompanyName.AbpZeroTemplate.Tests
{
    public sealed class MultiTenantTheoryAttribute : TheoryAttribute
    {
        public MultiTenantTheoryAttribute()
        {
            var multiTenancyConfig = System.Configuration.ConfigurationManager.AppSettings["MultiTenancyEnabled"];
            if (multiTenancyConfig != null && multiTenancyConfig == "false")
            {
                Skip = "MultiTenancy is disabled.";
            }
        }
    }
}