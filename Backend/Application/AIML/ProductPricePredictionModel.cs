using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.AIML
{
    public class ProductPricePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer model;

        public ProductPricePredictionModel()
        {
            mlContext = new MLContext();
        }

        public void Train(List<ProductData> trainingData)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            Console.WriteLine("Training data:");
            foreach (var product in trainingData)
            {
                Console.WriteLine($"{product.Type}, {product.Name}, {product.Description}, {product.Price}, {product.Review}");
            }

            var pipeline = mlContext.Transforms.Text.FeaturizeText("TypeFeaturized", nameof(ProductData.Type))
                .Append(mlContext.Transforms.Text.FeaturizeText("NameFeaturized", nameof(ProductData.Name)))
                .Append(mlContext.Transforms.Text.FeaturizeText("DescriptionFeaturized", nameof(ProductData.Description)))
                .Append(mlContext.Transforms.Conversion.ConvertType("Review", outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features", "TypeFeaturized", "NameFeaturized", "DescriptionFeaturized", nameof(ProductData.Review)))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(ProductData.Price), maximumNumberOfIterations: 100));

            model = pipeline.Fit(dataView);

            ///debugging
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: nameof(ProductData.Price));
            Console.WriteLine($"R^2: {metrics.RSquared}");
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");
        }

        public float Predict(ProductData product)
        {
            Console.WriteLine($"Product: Type={product.Type}, Name={product.Name}, Description={product.Description}, Review={product.Review}");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductDataPrediction>(model);
            var prediction = predictionEngine.Predict(product);
            return prediction.PredictedPrice;
        }

        public List<float[]> GetTransformedFeatures(List<ProductData> productData)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(productData);
            var transformedData = model.Transform(dataView);

            var transformedFeatures = mlContext.Data.CreateEnumerable<ProductDataWithFeatures>(transformedData, reuseRowObject: false).ToList();

            return transformedFeatures.Select(f => f.Features).ToList();
        }
    }

    public class ProductDataWithFeatures
    {
        public float[] Features { get; set; }
    }
}
