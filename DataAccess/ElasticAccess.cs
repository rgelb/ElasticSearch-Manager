using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearchManager.DataAccess {
    public class ElasticAccess: IDisposable {

        private readonly ElasticClient client;

        public ElasticAccess(string connString) {

            var parts = connString.Split(';');
            var urls = parts[0];

            string userId = parts.Length == 3 ? parts[1].Split('=')[1] : string.Empty;
            string password = parts.Length == 3 ? parts[2].Split('=')[1] : string.Empty;

            var pool = new StaticConnectionPool(urls.Split(',').Select(uri => new Uri(uri)));
            var settings = new ConnectionSettings(pool);
            settings.DisableDirectStreaming();

            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(password)) {
                settings = settings.BasicAuthentication(userId, password);
            }

            client = new ElasticClient(settings);
        }

        public ICatResponse<CatIndicesRecord> IndexList() {
            var request = new CatIndicesRequest();
            return client.CatIndices(request);
        } 

        // same as IndexList, but with SegmentCount added
        public List<CatIndicesRecordExtended> IndexListExtended()
        {
            var requestParams = new CatIndicesRequestParameters
            {
                Format = "json",
                Headers = new string[]
                {
                    "health",
                    "status",
                    "index",
                    "creation.date.string",
                    "uuid",
                    "pri",
                    "rep",
                    "docs.count",
                    "docs.deleted",
                    "store.size",
                    "pri.store.size",
                    "segments.count"
                }
            };

            var response = client.LowLevel.CatIndices<StringResponse>(requestParams);
            return JsonConvert.DeserializeObject<List<CatIndicesRecordExtended>>(response.Body);
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

        public CatSegmentsDescriptor Segments()
        {
            var request = new CatSegmentsDescriptor();
            return request.AllIndices();
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

        public IListTasksResponse TaskList()
        {
            var request = new ListTasksRequest();
            var response = client.ListTasks(request);

            return response;
        }

    }

    public class CatIndicesRecordExtended //: CatIndicesRecord
    {
        [JsonProperty("docs.count")]
        public string DocsCount { get; set; }
        [JsonProperty("docs.deleted")]
        public string DocsDeleted { get; set; }
        [JsonProperty("health")]
        public string Health { get; set; }
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("pri")]
        public string Primary { get; set; }
        [JsonProperty("pri.store.size")]
        public string PrimaryStoreSize { get; set; }
        [JsonProperty("rep")]
        public string Replica { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("store.size")]
        public string StoreSize { get; set; }
        [JsonProperty("memory.total ")]
        public string TotalMemory { get; set; }
        [JsonProperty("creation.date.string")]
        public string CreationDate { get; set; }
        [JsonProperty("segments.count")]
        public int SegmentCount { get; set; }
    }
}
