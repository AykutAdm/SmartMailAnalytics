using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.MLWorker.DTOs
{
    public class MailModelInput
    {
        [LoadColumn(0)]
        public string Subject { get; set; }

        [LoadColumn(1)]
        public string Content { get; set; }

        [LoadColumn(2)]
        [ColumnName("Label")]
        public bool Label { get; set; }
    }
}
