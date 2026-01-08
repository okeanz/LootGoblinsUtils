using LootGoblinsUtils.Submods.Armor;
using LootGoblinsUtils.Submods.ReactiveGear.GearSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace Units;

public class Tests
{
    private const string SubmodPath = "../../../../LootGoblinsHeimUtils/Submods";

    private const string ArmorSubmodFolder = $"{SubmodPath}/Armor/Json";
    private const string ArmorConfigFileName = "armorFeatureConfig.model.json";

    private const string GearSetsSubmodFolder = $"{SubmodPath}/ReactiveGear/Json";
    private const string GearSetsConfigFileName = "gearSets.model.json";

    private string ArmorConfigFilePath => GetPath(ArmorConfigFileName, ArmorSubmodFolder);
    private string GearSetsConfigFilePath => GetPath(GearSetsConfigFileName, GearSetsSubmodFolder);

    private string GetPath(string fileName, string folder) => Path.GetFullPath(Path.Combine(folder, fileName));

    private void GenerateJsonSchema<T>(string path)
    {
        var generator = new JSchemaGenerator
        {
            DefaultRequired = Required.Default,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            GenerationProviders = { new StringEnumGenerationProvider() }
        };

        File.WriteAllText(path,
            generator.Generate(typeof(T)).ToString());

        Assert.Pass();
    }

    [Test]
    public void GenerateJsonSchemas()
    {
        GenerateJsonSchema<ArmorFeatureConfig>(ArmorConfigFilePath);
    }

    [Test]
    public void GenerateGearSetsJsonSchema()
    {
        GenerateJsonSchema<GearSetList>(GearSetsConfigFilePath);
    }
}