using ElasticSearchManager.DataAccess;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchManager {
    public class ElasticIndex : CatIndicesRecord {

        public string Alias { get; set; }
        public int SegmentCount { get; set; }

        public static List<ElasticIndex> ToList(ICatResponse<CatIndicesRecord> indexRecords) {
            var lst = new List<ElasticIndex>();

            foreach (var item in indexRecords.Records.Where(r => !r.Index.StartsWith("."))) {

                ElasticIndex ea = AutoMapper.Mapper.Map<ElasticIndex>(item);
                lst.Add(ea);
            }

            return lst;
        }

        public static List<ElasticIndex> ToList(List<CatIndicesRecordExtended> indexRecords)
        {
            var lst = new List<ElasticIndex>();

            foreach (var item in indexRecords.Where(r => !r.Index.StartsWith(".")))
            {

                ElasticIndex ea = AutoMapper.Mapper.Map<ElasticIndex>(item);
                lst.Add(ea);
            }

            return lst;
        }

    }
}
