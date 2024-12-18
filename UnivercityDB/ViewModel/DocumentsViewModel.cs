using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;
using UnivercityDB.Model.UnivercityDB.Helpers;

namespace UnivercityDB.ViewModel
{
    internal class DocumentsViewModel : BaseViewModel
    {
        public string ?QueryText { get; set; }

        private DatabaseExecutor _model;
        public List<string> QueryTemlatesNames { get; set; }

        public string ?SelectedQueryTemlateName { get; set; }
        public Dictionary<string,string> QueryTemlates { get; private set; }

        public DataTable QueryResults { get; set; }
        public ICommand ExecuteQueryCommand { get; set; }
        public ICommand SaveQueryCommand { get; set; }

        public DocumentsViewModel()
        {
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery, CanExecute);
            SaveQueryCommand = new RelayCommand(SaveToFile);
            _model = new DatabaseExecutor();
            QueryTemlates = _model.GenerateQueryTemplates();
            QueryTemlatesNames = _model.GenerateQueryTemplates().Keys.ToList();
        }

        private bool CanExecute()
        {
            return QueryText != null || SelectedQueryTemlateName != null;
        }



        public void SaveToFile()
        {
            FileHelper fileHelper = new FileHelper();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Сохранить файл как",
                    Filter = "Текстовые файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                    DefaultExt = "csv",
                    AddExtension = true
                };

                // Показываем диалоговое окно и проверяем результат
                if ((bool)saveFileDialog.ShowDialog())
                {
                    string filePath = saveFileDialog.FileName;
                    fileHelper.SaveDataTableToCsv(QueryResults, filePath);
                }
                    
            }catch(Exception ex)
            {
                MessageBox.Show($"Ошибка запсии в файл {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void ExecuteQuery() {

            try
            {
                if (SelectedQueryTemlateName != null)
                {
                    //QueryText = QueryTemlates[SelectedQueryTemlateName];
                    QueryResults = await _model.ExecuteSelectQueryAsync(QueryTemlates[SelectedQueryTemlateName]);
                    SelectedQueryTemlateName = null;
                    OnPropertyChanged(nameof(QueryResults));
                }
                else
                {
                    if (QueryText.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        QueryResults = await _model.ExecuteSelectQueryAsync(QueryText);
                        OnPropertyChanged(nameof(QueryResults));
                    }
                    else
                    {
                        var result = _model.ExecuteNonQuery(QueryText);
                        MessageBox.Show(result, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"Ошибка запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
