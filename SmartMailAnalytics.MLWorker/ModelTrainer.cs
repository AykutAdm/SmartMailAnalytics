using Microsoft.ML;
using SmartMailAnalytics.MLWorker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.MLWorker
{
    public class ModelTrainer
    {
        public static void Train(string csvPath, string modelPath)
        {
            var mlContext = new MLContext();

            var data = mlContext.Data.LoadFromTextFile<MailModelInput>(
                csvPath, separatorChar: ',', hasHeader: true);

            var pipeline = mlContext.Transforms.Text
                .FeaturizeText("SubjectFeatures", nameof(MailModelInput.Subject))
                .Append(mlContext.Transforms.Text
                    .FeaturizeText("ContentFeatures", nameof(MailModelInput.Content)))
                .Append(mlContext.Transforms.Concatenate("Features", "SubjectFeatures", "ContentFeatures"))
                .Append(mlContext.BinaryClassification.Trainers.FastTree());

            Console.WriteLine("Model eğitiliyor...");
            var model = pipeline.Fit(data);

            mlContext.Model.Save(model, data.Schema, modelPath);
            Console.WriteLine($"Model kaydedildi: {modelPath}");
        }
    }
}
