using ClosedXML.Excel;

namespace Application.AIML
{
    public static class ProductDataGenerator
    {
        public static List<ProductData> GetProducts()
        {
            var productList = new List<ProductData>();
            string filePath = "C:\\Users\\Luki\\Desktop\\.net final project\\SmartE-CommercePlatform\\Backend\\Application\\AIML\\training_data.xlsx";

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Fi?ierul nu a fost g?sit: {filePath}");

          
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); 
                var rows = worksheet.RangeUsed().RowsUsed();

                foreach (var row in rows.Skip(1)) 
                {
                    productList.Add(new ProductData
                    {
                        Type = row.Cell(1).GetValue<string>(),
                        Name = row.Cell(2).GetValue<string>(),
                        Description = row.Cell(3).GetValue<string>(),
                        Price = row.Cell(4).GetValue<float>(),
                        Review = row.Cell(5).GetValue<int>()
                    });
                }
            }

            return productList;
        }
    }
}
