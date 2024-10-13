using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectTemplate.Pipes.Telegram
{
    public class TelegramApi
    {
        private static TelegramBotClient _botClient = new TelegramBotClient("7762548493:AAE17Et6VWcnwIpKUIcJ2Q3CSLOANydubv8");
        private static long chatId = -1002278552269;

        public static async Task SendMessageAsync(string message)
        {
            try
            {
                await _botClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }


        public static async Task SendImageAsync(string imagePath, string caption)
        {
            try
            {
                if (!System.IO.File.Exists(imagePath))
                {
                    Console.WriteLine($"Error: Image file '{imagePath}' does not exist.");
                }

                await using (Stream stream = System.IO.File.OpenRead(imagePath))
                {
                    await _botClient.SendPhotoAsync(chatId, photo: InputFile.FromStream(stream, Path.GetFileName(imagePath)), caption: caption);
                    Console.WriteLine($"Image '{Path.GetFileName(imagePath)}' sent successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending image: {ex.Message}");
            }
        }


        public static async Task SendLogText(string message, string tipo)
        {
            var filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}LogTxt\Relatorio.txt";

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.Write(message);
                writer.Close();
            }

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {

                var fileName = $"Relatorio_{tipo}.txt";
                await _botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: InputFile.FromStream(stream: fs, fileName: fileName),
                    caption: $"Informações atualizadas sobre {tipo}."
                );
            };

        }
        public static async Task SendLogArchive(string caminho, string name)
        {
            try
            {
                using (FileStream fs = new FileStream(caminho, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var fileName = name;
                    await _botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: InputFile.FromStream(stream: fs, fileName: fileName),
                        caption: $"Informações atualizadas."
                    );
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO :: " + ex.ToString());
            }

        }
    }
}
