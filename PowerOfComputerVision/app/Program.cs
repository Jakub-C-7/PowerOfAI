// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;


namespace PowerOfComputerVision
{
    class Program
    {
        static string subscriptionKey = "YOURKEY";
        static string endpoint = "YOURURL";
        private const string image_url = "https://images.unsplash.com/photo-1532635241-17e820acc59f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=815&q=80";

        static void Main(string[] args)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            AnalyseImageUrl(client, image_url).Wait();

        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint };
            return client;
        }

        public static async Task AnalyseImageUrl(ComputerVisionClient client, string imageUrl)
        {
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories,VisualFeatureTypes.Description,VisualFeatureTypes.Faces,VisualFeatureTypes.ImageType, VisualFeatureTypes.Tags, VisualFeatureTypes.Adult, VisualFeatureTypes.Color, VisualFeatureTypes.Brands, VisualFeatureTypes.Objects
            };

            Console.WriteLine($"Analysing the image{Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            //Perform image analysis
            ImageAnalysis results = await client.AnalyzeImageAsync(image_url, visualFeatures: features);


            Console.WriteLine("Summary: ");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");

            }
            Console.WriteLine();

            Console.WriteLine("Categories: ");
            foreach (var category in results.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");

            }
            Console.WriteLine();

            Console.WriteLine("Tags: ");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} with confidence {tag.Confidence}");

            }
            Console.WriteLine();

            Console.WriteLine("Objects: ");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");

            }
            Console.WriteLine();

            Console.WriteLine("Faces: ");
            foreach (var face in results.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Width}, {face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, {face.FaceRectangle.Top + face.FaceRectangle.Height}");

            }
            Console.WriteLine();

            Console.WriteLine("Adult: ");
            Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
            Console.WriteLine($"Has gory content: {results.Adult.IsGoryContent} with confidence {results.Adult.GoreScore}");

            Console.WriteLine();

            Console.WriteLine("Brands: ");
            foreach (var brand in results.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");

            }
            Console.WriteLine();

            Console.WriteLine("Celebrities: ");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celebrity in category.Detail.Celebrities)
                    {
                        Console.WriteLine($"{celebrity.Name} with confidence {celebrity.Confidence} at location {celebrity.FaceRectangle.Width}, {celebrity.FaceRectangle.Left}, {celebrity.FaceRectangle.Top + celebrity.FaceRectangle.Width}, {celebrity.FaceRectangle.Top + celebrity.FaceRectangle.Height}");

                    }

                }

            }
            Console.WriteLine();


            Console.WriteLine("Landmarks: ");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmk in category.Detail.Landmarks)
                    {
                        Console.WriteLine($"{landmk.Name} with confidence {landmk.Confidence}");

                    }

                }

            }
            Console.WriteLine();

            Console.WriteLine("Color Scheme: ");
            Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
            Console.WriteLine("Accent colour: " + results.Color.AccentColor);
            Console.WriteLine("Dominant background colour: " + results.Color.DominantColorBackground);
            Console.WriteLine("Dominant foreground colour: " + results.Color.DominantColorForeground);
            Console.WriteLine("Dominant colours: " + string.Join(",", results.Color.DominantColors));

            Console.WriteLine("DONE");

        }

    }

}