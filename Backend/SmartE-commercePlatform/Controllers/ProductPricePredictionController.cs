using Application.AIML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            _predictionModel.Train(trainingData);
        }

        // POST: api/v1/product-price-prediction/predict
        [AllowAnonymous]
        [HttpPost("predict")]
        public ActionResult<float> Predict([FromBody] ProductData product)
        {
            try
            {
                ///debugging
                Console.WriteLine($"Received product for prediction: {product.Type}, {product.Name}, {product.Description}, {product.Review}");

                ///NU INTELEG
                var transformedFeatures = _predictionModel.GetTransformedFeatures(new List<ProductData> { product });

                foreach (var features in transformedFeatures)
                {
                    Console.WriteLine($"Transformed features: {string.Join(", ", features)}");
                }

                Console.WriteLine("End product features \n");

                
                var predictedPrice = _predictionModel.Predict(product);

                return Ok(new { predictedPrice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Prediction failed", error = ex.Message });
            }
        }
    }
}
