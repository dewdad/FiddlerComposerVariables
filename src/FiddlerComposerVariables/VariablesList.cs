using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace FiddlerComposerVariables
{
    public class VariablesList : BindingList<Variable>
    {
        public VariablesList(string savesPath, string saveName)
        {
            SaveName = saveName;
            _savesPath = savesPath;

            Load();
        }

        private readonly string _savesPath;

        public string SaveName { get; set; }

        private string SavePath => string.Format(_savesPath + "/{0}.csv", SaveName);

        public bool IsModified { get; set; }

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                IsModified = true;
                return;
            }

            try
            {
                var stream = File.OpenRead(SavePath);
                var streamReader = new StreamReader(stream);
                var csvReader = new CsvFactory().CreateReader(streamReader);
                foreach (var variable in csvReader.GetRecords<Variable>())
                    Add(variable);
                streamReader.Close();
            }
            catch
            {
                Utils.Alert("Error", "An error has occured reading the variables from \"" + SavePath + "\".");
                return;
            }
        }

        public bool Save(string newName = null)
        {
            if (!string.IsNullOrEmpty(newName)) SaveName = newName;
            if (string.IsNullOrEmpty(SaveName))
            {
                Utils.Alert("Error", "An error occured saving the variables. No save name was provided.");
                return false;
            }

            try
            {
                var stream = File.Create(SavePath);
                var streamWriter = new StreamWriter(stream);
                var csvWriter = new CsvFactory().CreateWriter(streamWriter);
                csvWriter.WriteHeader<Variable>();
                csvWriter.WriteRecords(Items);
                streamWriter.Flush();
                streamWriter.Close();
                IsModified = false;
                return true;
            }
            catch
            {
                Utils.Alert("Error", "An error has occured saving the variables to \"" + SavePath + "\".");
                return false;
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                case ListChangedType.ItemChanged:
                case ListChangedType.ItemDeleted:
                case ListChangedType.ItemMoved:
                    IsModified = true;
                    break;
            }
        }
    }
}