using Microsoft.ML.Data;
using System;
using System.Collections.Generic;


namespace Application.AIML
{
    public class ProductData
    {
        
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }    
        public float Price { get; set; }  
        public float Review { get; set; }
    }
}