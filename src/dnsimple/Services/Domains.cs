using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using dnsimple.Services.ListOptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using static dnsimple.Services.Paths;

namespace dnsimple.Services
{
    /// <inheritdoc cref="Service"/>
    /// <see>https://developer.dnsimple.com/v2/domains/</see>
    public partial class DomainsService : Service
    {
        /// <inheritdoc cref="Service"/>
        public DomainsService(IClient client) : base(client)
        {
        }

        /// <summary>
        /// Retrieves the details of an existing domain.
        /// </summary>
        /// <param name="accountId">The account ID</param>
        /// <param name="domainIdentifier">The domain name or ID</param>
        /// <returns>The domain requested.</returns>
        /// <see>https://developer.dnsimple.com/v2/domains/#getDomain</see>
        public SimpleDnsimpleResponse<Domain> GetDomain(long accountId,
            string domainIdentifier)
        {
            return new SimpleDnsimpleResponse<Domain>(Execute(
                BuildRequestForPath(DomainPath(accountId, domainIdentifier))
                    .Request));
        }

        /// <summary>
        /// Lists the domains in the account.
        /// </summary>
        /// <param name="accountId">The account ID</param>
        /// <param name="options">Options passed to the list (sorting, filtering, pagination)</param>
        /// <returns>A <c>DomainResponse</c> containing a list of domains.</returns>
        /// <see cref="DomainListOptions"/>
        /// <see>https://developer.dnsimple.com/v2/domains/#listDomains</see>
        public PaginatedDnsimpleResponse<Domain> ListDomains(long accountId,
            ListOptionsWithFiltering options = null)
        {
            var builder = BuildRequestForPath(DomainsPath(accountId));

            AddListOptionsToRequest(options, ref builder);

            return new PaginatedDnsimpleResponse<Domain>(
                Execute(builder.Request));
        }

        /// <summary>
        /// Adds a domain to the account.
        /// </summary>
        /// <param name="accountId">The account ID</param>
        /// <param name="domainName">The name of the domain to be created</param>
        /// <returns>A <c>DomainResponse</c> containing the data of the newly
        /// created domain.</returns>
        /// <see>https://developer.dnsimple.com/v2/domains/#createDomain</see>
        public SimpleDnsimpleResponse<Domain> CreateDomain(long accountId,
            string domainName)
        {
            var builder = BuildRequestForPath(DomainsPath(accountId));
            builder.Method(Method.POST);

            var parameters = new Collection<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", domainName)
            };
            builder.AddParameters(parameters);

            return new SimpleDnsimpleResponse<Domain>(Execute(builder.Request));
        }

        /// <summary>
        /// Permanently deletes a domain from the account.
        /// </summary>
        /// <remarks>It cannot be undone.</remarks>
        /// <param name="accountId">The account ID</param>
        /// <param name="domainIdentifier">The domain name or id</param>
        /// <see>https://developer.dnsimple.com/v2/domains/#deleteDomain</see>
        public EmptyDnsimpleResponse DeleteDomain(long accountId,
            string domainIdentifier)
        {
            var builder = BuildRequestForPath(DeleteDomainPath(accountId,
                    domainIdentifier));
            builder.Method(Method.DELETE);

            return new EmptyDnsimpleResponse(Execute(builder.Request));
        }
    }

    /// <summary>
    /// Represents a <c>Domain</c>
    /// </summary>
    /// <see>https://developer.dnsimple.com/v2/domains/</see>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public struct Domain
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long? RegistrantId { get; set; }
        public string Name { get; set; }
        public string UnicodeName { get; set; }
        public string State { get; set; }
        public bool? AutoRenew { get; set; }
        public bool? PrivateWhois { get; set; }
        public string ExpiresOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}