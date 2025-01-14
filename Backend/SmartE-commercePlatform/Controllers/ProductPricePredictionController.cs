using Application.AIML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/product-price-prediction")]
    [ApiController]
    public class ProductPricePredictionController : ControllerBase
    {
        private readonly ProductPricePredictionModel _predictionModel;

        public ProductPricePredictionController()
        {
            _predictionModel = new ProductPricePredictionModel();
            var trainingData = ProductDataGenerator.GetProducts();

            var modelPath = "path_to_saved_model.zip";
            if (System.IO.File.Exists(modelPath))
            {
                _predictionModel.LoadPriceModel(modelPath);
            }
            else
            {
                _predictionModel.TrainPriceModel(trainingData, modelPath);
            }
        }

        [AllowAnonymous]
        [HttpPost("predict")]
        public ActionResult Predict([FromBody] ProductData product)
        {
            try
            {
                Console.WriteLine($"Received product for prediction: {product.Type}, {product.Name}, {product.Description}, {product.Review}");

                var transformedFeatures = _predictionModel.GetTransformedFeatures(new List<ProductData> { product });
                foreach (var features in transformedFeatures)
                {
                    Console.WriteLine($"Transformed features: {string.Join(", ", features)}");
                }

                var predictedPrice = _predictionModel.PredictPrice(product, "path_to_saved_model.zip");
                return Ok(new { predictedPrice });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "Model is not trained or loaded.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }
    }
}
