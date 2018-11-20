using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            settings.DisableDirectStreaming();

            client = new ElasticClient(settings);
        }

        public ICatResponse<CatIndicesRecord> IndexList() {
            var request = new CatIndicesRequest();
            return client.CatIndices(request);
        }

        public IndexDefinition IndexDescription(string name) {
            var indexDef = new IndexDefinition() { Name = name };

            try {
                indexDef.Index = client.GetIndex(name);
                indexDef.Settings = indexDef.Index.Indices.FirstOrDefault().Value.Settings;
                indexDef.Mappings = indexDef.Index.Indices.FirstOrDefault().Value.Mappings;
            } catch (Exception) {
                indexDef.Name = $"Unable to retrieve {name}";
            }

            //try { indexDef.Index = client.GetIndex(name); } catch (Exception ex) { Debug.WriteLine(ex.Message); }   // ignore exception
            //try { indexDef.Settings = client.GetIndexSettings(i => i.Index(name)); } catch (Exception ex) { Debug.WriteLine(ex.Message); }    // ignore exception
            //try { indexDef.Mappings = client.GetMapping<object>(i => i.Index(name).AllTypes()); } catch (Exception ex) { Debug.WriteLine(ex.Message); }    // ignore exception

            return indexDef;
        }

        public IIndicesStatsResponse IndexStats(string name) {

            var indices = Nest.Indices.Index(name);
            var request = new IndicesStatsRequest(indices);
            return client.IndicesStats(request);
        }

        public class IndexDefinition {
            public string Name { get; set; }
            public IGetIndexResponse Index { get; set; }
            public IIndexSettings Settings { get; set; }
            public IMappings Mappings { get; set; }
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
