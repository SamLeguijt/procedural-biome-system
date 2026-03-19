using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayout 
{
    public Map<EBiome> BiomeMap { get; private set; }
    public Map<float> ElevationMap { get; private set; }
    public Map<float> ErosionMap { get; private set; }
    public Map<float> HumidityMap { get; private set; }

    public List<WorldChunk> WorldChunks { get; private set; }
    
    private WorldLayout(Map<EBiome> biomeMap, Map<float> elevation, Map<float> erosion, Map<float> humidity, List<WorldChunk> chunks)
    {
        BiomeMap = biomeMap;
        ElevationMap = elevation;
        HumidityMap = humidity;
        WorldChunks = chunks;
    }

    public WorldLayout(List<WorldChunk> chunks)
    {
        WorldChunks = chunks;
    }

    public class LayoutBuilder
    {
        private Map<EBiome> biomeMap = new Map<EBiome>(0, 0);
        private Map<float> elevationMap = new Map<float>(0, 0);
        private Map<float> erosionMap = new Map<float>(0, 0);
        private Map<float> humidityMap = new Map<float>(0, 0);
        private List<WorldChunk> chunks = new List<WorldChunk>();

        public LayoutBuilder WithChunks(List<WorldChunk> chunkList)
        {
            chunks = chunkList;
            return this;
        }

        public LayoutBuilder WithBiomeMap(Map<EBiome> map)
        {
            biomeMap = map;
            return this;
        }

        public LayoutBuilder WithElevationMap(Map<float> map)
        {
            elevationMap = map;
            return this;
        }

        public LayoutBuilder WithErosionMap(Map<float> map)
        {
            erosionMap = map;
            return this;
        }

        public LayoutBuilder WithHumidityMap(Map<float> map)
        {
            humidityMap = map;
            return this;
        }

        public WorldLayout Build()
        {
            return new WorldLayout(biomeMap, elevationMap, erosionMap, humidityMap, chunks);
        }
    }
}
