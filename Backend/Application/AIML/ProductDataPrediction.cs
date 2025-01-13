using Microsoft.ML.Data;

public class ProductDataPrediction
{
    [ColumnName("Score")]
    public float PredictedPrice { get; set; }
}