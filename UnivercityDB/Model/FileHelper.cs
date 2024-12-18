using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivercityDB.Model
{
    public class FileHelper
    {
        // Метод для сохранения DataTable в CSV файл
        public void SaveDataTableToCsv(DataTable dataTable, string filePath)
        {
            try
            {
                // Создаем StringBuilder для записи в файл
                var csvContent = new StringBuilder();

                // Записываем заголовки столбцов
                foreach (DataColumn column in dataTable.Columns)
                {
                    csvContent.Append(column.ColumnName + ",");
                }

                // Убираем последнюю запятую и добавляем новую строку
                csvContent.Length--;
                csvContent.AppendLine();

                // Записываем строки данных
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        csvContent.Append(item.ToString() + ",");
                    }

                    // Убираем последнюю запятую и добавляем новую строку
                    csvContent.Length--;
                    csvContent.AppendLine();
                }

                // Сохраняем содержимое в файл
                File.WriteAllText(filePath, csvContent.ToString());

                Console.WriteLine($"DataTable saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving DataTable to CSV: {ex.Message}");
            }
        }
    }
}
