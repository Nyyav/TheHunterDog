using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;
using System;
using SpriteFontPlus;

namespace MonogameExamples
{
     /// <summary>
    /// Управляет загрузкой и извлечением игровых ресурсов, включая текстуры, шрифты, тайловые карты и аудио.
    /// </summary>
    public class Loader
    {
        // Словари для хранения текстур, шрифтов и аудиоресурсов.
        private static Dictionary<Enum, Texture2D> textures = new Dictionary<Enum, Texture2D>();
        private static Dictionary<Enum, SoundEffect> songs = new Dictionary<Enum, SoundEffect>();
        private static Dictionary<Enum, SoundEffectInstance> soundEffectInstances = new Dictionary<Enum, SoundEffectInstance>();
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        // Свойство для управления tiled картами.
        public static TileHandler tiledHandler { get; private set; }

        // Текстура для отладки коллизий.
        public static Texture2D collisionBox;

        /// <summary>
        /// Загружает в память игровые ресурсы, такие как текстуры, шрифты и аудио.
        /// </summary>
        /// <param name="graphicsDevice">Графическая карта - используется для создания и управления графическими ресурсами.</param>
        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            // Загрузка шрифтов
            AddFont("GameFont", 16, graphicsDevice,  "Fonts", "DejaVuSerif");

            // Загрузка музыки
            //AddMusic(BackgroundMusic.Default, graphicsDevice, "Audio", "Background", "???");
            

            // Загрузка текстур
            // Игрок
            AddTexture(PlayerTexture.Idle, graphicsDevice, "Player", "Dog", "Idle");
            AddTexture(PlayerTexture.Walking, graphicsDevice, "Player", "Dog", "Walking");
            AddTexture(PlayerTexture.Jump, graphicsDevice, "Player", "Dog", "Jump");
            AddTexture(PlayerTexture.DoubleJump, graphicsDevice, "Player", "Dog", "Double Jump");
            AddTexture(PlayerTexture.Fall, graphicsDevice, "Player", "Dog", "Fall");
            AddTexture(PlayerTexture.Slide, graphicsDevice, "Player", "Dog", "Wall Jump");
            AddTexture(PlayerTexture.Hit, graphicsDevice, "Player", "Dog", "Hit");

            //Враг
            AddTexture(MaskedEnemyTexture.Idle, graphicsDevice, "Enemies", "Squirrel", "Idle");
            AddTexture(MaskedEnemyTexture.Walking, graphicsDevice, "Enemies", "Squirrel", "Run");
            AddTexture(MaskedEnemyTexture.Jump, graphicsDevice, "Enemies", "Squirrel", "Jump");
            AddTexture(MaskedEnemyTexture.DoubleJump, graphicsDevice, "Enemies", "Squirrel", "Double Jump");
            AddTexture(MaskedEnemyTexture.Fall, graphicsDevice, "Enemies", "Squirrel", "Fall");
            AddTexture(MaskedEnemyTexture.Slide, graphicsDevice, "Enemies", "Squirrel", "Wall Jump");
            AddTexture(MaskedEnemyTexture.Hit, graphicsDevice, "Enemies", "Squirrel", "Hit");

            //Предметы 
            AddTexture(FruitTexture.Apple, graphicsDevice, "Items", "Fruits", "Apple");
            AddTexture(FruitTexture.Orange, graphicsDevice, "Items", "Fruits", "Orange");
            AddTexture(FruitTexture.Collected, graphicsDevice, "Items", "Fruits", "Collected");


            // ... сюда еще текстуры добавить можно

            // Загрузка TiledMaps
            //Текстуры наборов тайлов, ключ должен совпадать с названием соответствующих наборов тайлов
            AddTexture(TiledTexture.Terrain, graphicsDevice, "TiledMap", "Textures", "Terrain");
            AddTexture(TiledTexture.UI, graphicsDevice, "TiledMap", "Textures", "UI");
            AddTexture(TiledTexture.BackgroundEnvironment, graphicsDevice, "TiledMap", "Textures", "BackgroundEnvironment");
            AddTexture(TiledTexture.Cloud, graphicsDevice, "TiledMap", "Textures", "Cloud");
            AddTexture(TiledTexture.AutumnForest, graphicsDevice, "TiledMap", "Textures", "AutumnForest");

            //TileMaps
            tiledHandler = new TileHandler();
            foreach (LevelID level in LevelID.GetValues(typeof(LevelID)))
            {
                string levelName = level.ToString();
                tiledHandler.Load(
                    Path.Combine("Content", "TiledMap", "Levels", $"{levelName}.tmx"),
                    Path.Combine("Content", "TiledMap", "Levels", " "),
                    levelName
                );

                // Сохранение поля столкновений для каждого уровня
                tiledHandler.GetLayersObstaclesInMap();
            }

            //Окно для отладки коллизий
            collisionBox = new Texture2D(graphicsDevice, 1, 1);
            collisionBox.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Загружает шрифт из файла .ttf и сохраняет его в словаре.
        /// </summary>
        /// <param name="fontKey">Ключ, используемый для идентификации загруженного шрифта.</param>
        /// <param name="fontSize">Размер шрифта, который необходимо сгенерировать.</param>
        /// <param name="graphicsDevice">Устройство рендеринга (GraphicsDevice), используемое для создания SpriteFont.</param>
        /// <param name="pathParts">Элементы пути, указывающие на файл .ttf.</param>
        private static void AddFont(string fontKey, int fontSize, GraphicsDevice graphicsDevice, params string[] pathParts)
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".ttf";
            using (var fileStream = File.OpenRead(path))
            {
                var ttfFontBakerResult = TtfFontBaker.Bake(fileStream,
                                                        fontSize,
                                                        1024,
                                                        1024,
                                                        new[] { CharacterRange.BasicLatin });

                var ttfFont = ttfFontBakerResult.CreateSpriteFont(graphicsDevice);

                if (ttfFont != null)
                {
                    fonts[fontKey] = ttfFont;
                }
            }
        }

       /// <summary>
        /// Загружает текстуру из файла и сохраняет её в словаре.
        /// </summary>
        /// <param name="textureKey">Ключ, который будет использоваться для доступа к загруженной текстуре.</param>
        /// <param name="graphicsDevice">Устройство рендеринга (GraphicsDevice), используемое для загрузки текстуры.</param>
        /// <param name="pathParts">Элементы пути, указывающие на файл текстуры.</param>
        private static void AddTexture<T>(T textureKey, GraphicsDevice graphicsDevice, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".png";

            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    if (texture != null)
                    {
                        textures[textureKey] = texture;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: The file at path {path} does not exist.");
            }
        }

        /// <summary>
        /// Загружает музыкальный файл и сохраняет его как SoundEffect в словаре.
        /// </summary>
        /// <param name="musicKey">Ключ, который будет использоваться для доступа к загруженному музыкальному файлу.</param>
        /// <param name="graphicsDevice">Устройство рендеринга (GraphicsDevice), используемое для загрузки музыкального файла.</param>
        /// <param name="pathParts">Элементы пути, указывающие на музыкальный файл.</param>
        private static void AddMusic<T>(T musicKey, GraphicsDevice graphicsDevice, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".wav";

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                SoundEffect soundEffect = SoundEffect.FromStream(fileStream);
                songs.Add(musicKey, soundEffect);
            }
        }


        /// <summary>
        /// Возвращает текстуру, связанную с указанным ключом.
        /// </summary>
        /// <param name="textureKey">Ключ текстуры, которую необходимо получить.</param>
        /// <returns>Объект Texture2D, соответствующий ключу, или null, если текстура не найдена.</returns>
        public static Texture2D GetTexture<T>(T textureKey) where T : Enum
        {
            if (textures.ContainsKey(textureKey))
            {
                return textures[textureKey];
            }
            return null;
        }

        /// <summary>
        /// Возвращает шрифт, связанный с указанным ключом.
        /// </summary>
        /// <param name="fontKey">Ключ шрифта, который необходимо получить.</param>
        /// <returns>Объект SpriteFont, соответствующий ключу, или null, если шрифт не найден.</returns>
        public static SpriteFont GetFont(string fontKey)
        {
            if (fonts.ContainsKey(fontKey))
            {
                return fonts[fontKey];
            }
            return null;
        }

        /// <summary>
        /// Возвращает музыкальный файл по его ключу.
        /// </summary>
        /// <param name="musicKey">Ключ музыкального файла, который необходимо получить.</param>
        /// <returns>Загруженный музыкальный файл (SoundEffect), или null, если файл не найден.</returns>
        public static SoundEffect GetMusic(Enum musicKey)
        {
            if (songs.ContainsKey(musicKey))
            {
                return songs[musicKey];
            }

            return null;
        }

        /// <summary>
        /// Воспроизводит музыкальный файл, связанный с указанным ключом.
        /// </summary>
        /// <param name="musicKey">Ключ музыкального файла для воспроизведения.</param>
        /// <param name="loop">Зацикливание музыки.</param>
        public static void PlayMusic(Enum musicKey, bool loop = false)
        {
            // Останавливает ранее запущенный экземпляр этого же звука.
            if (soundEffectInstances.ContainsKey(musicKey) && soundEffectInstances[musicKey].State == SoundState.Playing)
            {
                soundEffectInstances[musicKey].Stop();
            }

            if (songs.ContainsKey(musicKey))
            {
                SoundEffectInstance soundEffectInstance = songs[musicKey].CreateInstance();
                soundEffectInstance.IsLooped = loop;
                soundEffectInstance.Play();

                // Сохраняет экземпляр
                soundEffectInstances[musicKey] = soundEffectInstance;
            }
        }
    }
}
