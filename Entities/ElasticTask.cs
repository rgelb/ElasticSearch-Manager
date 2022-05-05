using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchManager
{
    public class ElasticTask: TaskState
    {
        public string NodeIp { get; set; }
        public string NodeName { get; set; }
        public string TaskKey { get; set; }

        [Description("Started At")]
        public string StartedAt { 
            get {
                return Utilities.UnixTimeStampToDateTime((double) this.StartTimeInMilliseconds).ToString("HH:mm:ss.fff");
            }
        }

        public string Duration {
            get {
                double milliseconds = Utilities.NanosecondsToMilliseconds(this.RunningTimeInNanoSeconds);
                if (milliseconds < 1000) {
                    return string.Format("{0:.##}", milliseconds) + " ms"; 
                } else {
                    double seconds = milliseconds / 1000;
                    return string.Format("{0:.##}", seconds) + " s";
                }
            }
        }


        public static List<ElasticTask> ToList(IListTasksResponse tasks)
        {
            var lst = new List<ElasticTask>();

            foreach (var node in tasks.Nodes)
            {
                foreach (var task in node.Value.Tasks)
                {
                    ElasticTask et = AutoMapper.Mapper.Map<ElasticTask>(task.Value);

                    et.NodeIp = node.Value.Ip;
                    et.NodeName = node.Value.Name;
                    et.TaskKey = task.Key.FullyQualifiedId;

                    lst.Add(et);
                }
            }

            return lst;
        }

    }
}
