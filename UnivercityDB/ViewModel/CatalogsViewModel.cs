using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{


    public class StringWrapper
    {
        [Display(Name = "Название")]
        public string Value { get; set; }

        public StringWrapper(string value)
        {
            Value = value;
        }

        public static ObservableCollection<StringWrapper> FromStringList(List<string> list)
        {
            var collection = new ObservableCollection<StringWrapper>();
            foreach (var item in list)
            {
                collection.Add(new StringWrapper(item));
            }
            return collection;
        }

    }
    class CatalogsViewModel : BaseViewModel
    {
        private Permission _permission;
        public string Text { get; set; } 

        public string SearchText { get; set; }


        public StringWrapper SelectedItem { get; set; }    
        public ICommand DeleteCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ObservableCollection<StringWrapper> Catalog
        {
            get; set;
        }

        private CatalogsModel _model;

        private string _catalogName;
        public CatalogsViewModel(string catalogName, Permission? permission)
        {
            _permission = permission;
            _model = new CatalogsModel();
            _catalogName = catalogName;
            Catalog = StringWrapper.FromStringList(_model.GetNamesFromTable(catalogName));
            AddCommand = new RelayCommand(Add, CanAdd);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            SearchCommand = new RelayCommand(Search);
        }

        public void Search()
        {
            try {
                Catalog = StringWrapper.FromStringList(_model.Search(SearchText, _catalogName));
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }


        public void Update()
        {
            try
            {
                _model.UpdateCatalog(SelectedItem.Value,Text,_catalogName);
                Catalog.Remove(SelectedItem);
                Catalog.Add(new StringWrapper(Text));
                SelectedItem = null;
                OnPropertyChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdate()
        {
            return Text != null && SelectedItem!=null && _permission.CanEdit;
        }

        private bool CanAdd()
        {
            return Text != null && _permission.CanWrite;
        }

        private bool CanDelete()
        {
            return Catalog.Count > 0 && Catalog!=null && SelectedItem!= null && _permission.CanDelete;
        }

        private void Delete()
        {
            try
            {
                _model.DeleteFromCatalog(SelectedItem.Value, _catalogName);
                Catalog.Remove(SelectedItem);
                OnPropertyChanged(nameof(Catalog));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add()
        {
            try
            {
                _model.AddToCatalog(Text, _catalogName);
                Catalog.Add(new StringWrapper(Text));
                OnPropertyChanged(nameof(Catalog));
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
