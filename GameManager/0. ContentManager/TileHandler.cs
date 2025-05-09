using System.Collections.Generic;
using TiledCS;
using System;

namespace MonogameExamples
{
    //// <summary>
    /// Обрабатывает загрузку и рендеринг карт, построенных из тайлов.
    /// </summary>
    public class TileHandler
    {
        private Dictionary<string, TiledMap> _tiledMaps;
        private Dictionary<string, Dictionary<int, TiledTileset>> _tileSets;

        /// <summary>
        /// Словарь всех препятствий на каждой загруженной карте.
        /// </summary>
        public Dictionary<string, Dictionary<string, List<Rectangle>>> objects { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса TileHandler.
        /// </summary>
        public TileHandler()
        {
            _tiledMaps = new Dictionary<string, TiledMap>();
            _tileSets = new Dictionary<string, Dictionary<int, TiledTileset>>();
            objects = new Dictionary<string, Dictionary<string, List<Rectangle>>>();
        }

        /// <summary>
        /// Загружает карту и связанную с ней текстуру тайлсета.
        /// </summary>
        /// <param name="pathToMap">Путь к файлу карты Tiled.</param>
        /// <param name="pathToFolder">Путь к папке, содержащей текстуру тайлсета.</param>
        /// <param name="levelID">Имя, присваиваемое загруженной карте.</param>
        /// <param name="textureName">Имя, присваиваемое загруженной текстуре тайлсета.</param>
        public void Load(string pathToMap, string pathToFolder, string levelID)
        {
            try
            {
                TiledMap map = new TiledMap(pathToMap);
                var tilesets = map.GetTiledTilesets(pathToFolder);

                _tiledMaps[levelID] = map ?? throw new Exception("Файл карты не найден или не может быть загружен.");
                _tileSets[levelID] = tilesets ?? throw new Exception("Тайлсет не может быть загружен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке карты или ее тайлсетов: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Возвращает карту по ее LevelID.
        /// </summary>
        /// <param name="level">LevelID карты, которую необходимо получить.</param>
        /// <returns>Загруженная карта или null, если карта не была найдена.</returns>
        public TiledMap GetMap(LevelID level)
        {
            return _tiledMaps[level.ToString()];
        }

        /// <summary>
        /// Helper method for getting object representing obstacles in a map method below. 
        /// Creates rectangles representing the objects of a specified layer.
        /// </summary>
        /// <param name="layer">The layer to create bounds for.</param>
        /// <returns>A list of rectangles representing the layer's bounds.</returns>
        public List<Rectangle> GetLayerObjects(TiledLayer layer)
        {
            List<Rectangle> layerBounds = new List<Rectangle>();

            foreach (var obj in layer.objects)
            {
                int objX = (int)obj.x;
                int objY = (int)obj.y;
                int objWidth = (int)obj.width;
                int objHeight = (int)obj.height;
                layerBounds.Add(new Rectangle(objX, objY, objWidth, objHeight));
            }

            return layerBounds;
        }

        /// <summary>
        /// Creates rectangles that bound every obstacle object on every layer in every loaded Level. 
        /// Useful for collision detection with Rectangle.Intersects() method.
        /// </summary>
        public void GetLayersObstaclesInMap()
        {
            foreach (string mapName in _tiledMaps.Keys)
            {
                Dictionary<string, List<Rectangle>> layerBoundsMap = new Dictionary<string, List<Rectangle>>();
                foreach (var layer in _tiledMaps[mapName].Layers)
                {
                    string layerName = layer.name;
                    if (layer.type != TiledLayerType.ObjectLayer || !GameConstants.OBSTACLES.Contains(layer.name))
                    {
                        continue;
                    }
                    List<Rectangle> layerBounds = GetLayerObjects(layer);
                    if (!layerBoundsMap.ContainsKey(layerName))
                    {
                        layerBoundsMap[layerName] = new List<Rectangle>();
                    }
                    layerBoundsMap[layerName].AddRange(layerBounds);
                }
                objects[mapName] = layerBoundsMap;
            }
        }

        /// <summary>
        /// <summary>
        /// Отрисовывает все тайлы в слоях типа "TileLayer" указанной карты.
        /// </summary>
        /// <param name="LevelID">Имя карты для отрисовки.</param>
        /// <param name="spriteBatch">Объект SpriteBatch, используемый для рендеринга.</param>
        public void Draw(string LevelID, SpriteBatch spriteBatch)
        {
            TiledMap map = _tiledMaps[LevelID];
            var tilesets = _tileSets[LevelID];

            foreach (var layer in map.Layers)
            {
                if (layer.type != TiledLayerType.TileLayer)
                {
                    continue;
                }

                for (int y = 0; y < layer.height; y++)
                {
                    for (int x = 0; x < layer.width; x++)
                    {
                        // Предполагается, что используется порядок рендеринга по умолчанию, который идет справа вниз
                        var index = (y * layer.width) + x;
                        var gid = layer.data[index];
                        var tileX = x * map.TileWidth;
                        var tileY = y * map.TileHeight;
                        // Gid 0 используется для обозначения отсутствия тайла
                        if (gid == 0)
                        {
                            continue;
                        }

                        // Вспомогательный метод для получения правильного экземпляра TieldMapTileset.
                        // Это объект-связь, используемый Tiled для связывания правильного тайлсета со значением gid, используя свойство firstgid.
                        var mapTileset = map.GetTiledMapTileset(gid);

                        // Получаем фактический тайлсет на основе свойства firstgid объекта-связи, полученного ранее
                        var tileset = tilesets[mapTileset.firstgid];

                        // Используем объект-связь, а также тайлсет, чтобы определить исходный прямоугольник.
                        var rect = map.GetSourceRect(mapTileset, tileset, gid);
                        // Создаем прямоугольники назначения и источника
                        var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                        var destination = new Rectangle(tileX, tileY, map.TileWidth, map.TileHeight);
                        // Получаем текстуру, используемую в тайлсете
                        Enum.TryParse(tileset.Name, out TiledTexture textureName);
                        var tilesetTexture = Loader.GetTexture(textureName);
                        // Рисуем тайл
                        spriteBatch.Draw(tilesetTexture, destination, source, Color.White);
                    }
                }
            }
        }
    }
}
