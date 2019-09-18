using System;

namespace FunApp.Services.MachineLearning
{
    public interface IJokesCategorizer
    {
        string Categorize(string modelFile, string Content);
    }
}
