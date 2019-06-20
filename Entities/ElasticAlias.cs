using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchManager {
    public class ElasticAlias: CatAliasesRecord {

        public static List<ElasticAlias> ToList(ICatResponse<CatAliasesRecord> aliasesRecords) {
            var lst = new List<ElasticAlias>();

            foreach (var item in aliasesRecords.Records) {

                ElasticAlias ea = AutoMapper.Mapper.Map<ElasticAlias>(item);
                lst.Add(ea);
            }

            return lst;
        }
    }
}
