using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.MLWorker.DTOs
{
    public class MailModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool IsSpam { get; set; }
    }
}
