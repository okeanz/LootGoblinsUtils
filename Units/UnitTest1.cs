using System.Reflection;
using LootGoblinsUtils.Submods.Armor;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using NUnit.Framework.Internal;

namespace Units;

public class Tests
{
    private const string SubmodFolder = "../../../../LootGoblinsHeimUtils/Submods/Armor/Json";
    private const string ArmorConfigFileName = "armorFeatureConfig.model.json";
    
    private string ArmorConfigFilePath => GetPath(ArmorConfigFileName);

    private string GetPath(string fileName)
    {
        return Path.GetFullPath(Path.Combine(SubmodFolder, fileName));
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GenerateJsonSchemas()
    {
        var generator = new JSchemaGenerator
        {
            DefaultRequired = Required.Default,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        File.WriteAllText(ArmorConfigFilePath,
            generator.Generate(typeof(ArmorFeatureConfig)).ToString());

        Assert.Pass();
    }
}