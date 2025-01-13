using Microsoft.ML;
using Microsoft.ML.Data;


namespace Application.AIML
{
    public class ProductPricePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer pricePredictionModel;

        public ProductPricePredictionModel()
        {
            mlContext = new MLContext();
        }

        public void TrainPriceModel(List<ProductData> trainingData, string modelPath)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(ProductData.Price))
                .Append(mlContext.Transforms.Text.FeaturizeText("NameFeaturized", nameof(ProductData.Name)))
                .Append(mlContext.Transforms.Text.FeaturizeText("DescriptionFeaturized", nameof(ProductData.Description)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("TypeEncoded", nameof(ProductData.Type)))
                .Append(mlContext.Transforms.Concatenate("Features", "NameFeaturized", "DescriptionFeaturized", "Review", "TypeEncoded"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 200));

            pricePredictionModel = pipeline.Fit(dataView);
            SaveModel(pricePredictionModel, modelPath);
        }

        public float PredictPrice(ProductData product, string modelPath)
        {
            if (pricePredictionModel == null)
            {
                LoadPriceModel(modelPath);
            }

            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductDataPrediction>(pricePredictionModel);
            var prediction = predictionEngine.Predict(product);
            return prediction.PredictedPrice;
        }

        public List<float[]> GetTransformedFeatures(List<ProductData> productData)
        {
            if (pricePredictionModel == null)
            {
                throw new InvalidOperationException("The model is not trained or loaded.");
            }

            var dataView = mlContext.Data.LoadFromEnumerable(productData);
            var transformedData = pricePredictionModel.Transform(dataView);

            var featuresColumn = mlContext.Data
                .CreateEnumerable<TransformedProductData>(transformedData, reuseRowObject: false)
                .Select(data => data.Features)
                .ToList();

            return featuresColumn;
        }

        public void LoadPriceModel(string modelPath)
        {
            pricePredictionModel = LoadModel(modelPath);
        }

        private void SaveModel(ITransformer model, string modelPath)
        {
            mlContext.Model.Save(model, null, modelPath);
        }

        private ITransformer LoadModel(string modelPath)
        {
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return mlContext.Model.Load(stream, out var _);
        }
    }

    public class TransformedProductData
    {
        [VectorType]
        public float[] Features { get; set; }
    }
}
