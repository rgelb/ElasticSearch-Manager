using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearchManager.DataAccess {
    public class ElasticAccess: IDisposable {

        private readonly ElasticClient client;

        public ElasticAccess(string connString) {
            var pool = new StaticConnectionPool(connString.Split(',').Select(uri => new Uri(uri)));
            var settings = new ConnectionSettings(pool);

            client = new ElasticClient(settings);
        }

        public ICatResponse<CatIndicesRecord> IndexList() {
            var request = new CatIndicesRequest();
            return client.CatIndices(request);
        }

        public ICatResponse<CatAliasesRecord> AliasList() {
            var request = new CatAliasesRequest();
            return client.CatAliases(request);
        }

        public void Dispose() {

        }

        public bool DeleteIndex(CatIndicesRecord index) {
            IDeleteIndexResponse response = client.DeleteIndex(index.Index);
            return response.Acknowledged;
        }

        public bool DeleteAlias(CatAliasesRecord alias) {
            IBulkAliasResponse aliasResponse = client.Alias(a => a.Remove(r => r.Index(alias.Index).Alias(alias.Alias)));
            return aliasResponse.Acknowledged;
        }
    }
}
