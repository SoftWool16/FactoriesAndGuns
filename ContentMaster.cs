using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Factories_And_Guns
{
    internal class ContentMaster
    {
        public static Dictionary<string, Dictionary<string, Texture2D>> Textures = [];

        public static void LoadTexture(ContentManager content)
        {
            string contentRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, content.RootDirectory); // Путь к папке, где находятся уже скомпилированные .xnb‑файлы.
            // Где: AppDomain.CurrentDomain.BaseDirectory - путь к папке с проектом,
            //      а Content.RootDirectory - путь к папке, в которой подпапки с .xnb ( относительно проекта, например: bin/Debug/net6.0/Content/player.xnb ).

            string[] xnbFiles = Directory.GetFiles(contentRoot, "*.xnb", SearchOption.AllDirectories); // Получаем все файлы .xnb в любой вложенной папке.

            foreach (string fullPath in xnbFiles)
            {
                string relativePath = Path.GetRelativePath(contentRoot, fullPath);  // 1️. Приводим путь к относительному относительно корня Content.

                string assetName = Path.ChangeExtension(relativePath, null);        // 2️. Убираем расширение .xnb.

                assetName = assetName.Replace('\\', '/');                           // 3️. Меняем обратные слеши на прямые – так требует ContentManager.

                string path = Path.GetDirectoryName(assetName)?.Replace('\\', '/') ?? ""; // Path.GetDirectoryName вернет папку (например, "Characters/Enemies") или пустую строку для корня

                if (!Textures.ContainsKey(path))
                {
                    Textures[path] = [];
                }

                // 4️. Загружаем текстуру и сохраняем по названию.
                try
                {
                    Textures[path][Path.GetFileName(assetName)] = content.Load<Texture2D>(assetName);
                }
                catch (ContentLoadException)
                {
                    // Если какой‑то файл не является текстурой ( например, .xnb шрифта ), просто игнорируем.
                }
            }
        }
    }
}
