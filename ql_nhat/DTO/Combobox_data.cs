using ql_nhat.DAO;
using System;
using System.Windows.Forms;

namespace ql_nhat.DTO
{
    class Combobox_data
    {
        private static Combobox_data instance;

        public static Combobox_data Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Combobox_data();
                }
                return Combobox_data.instance;
            }
            private set { Combobox_data.instance = value; }
        }

        public void Combobox(ComboBox data, String query, String show,String id)
        {
            data.DataSource = null;
            data.AutoCompleteSource = AutoCompleteSource.ListItems;
            data.AutoCompleteMode = AutoCompleteMode.Suggest;
            data.DisplayMember = show;
            data.ValueMember = id;
            data.DataSource = DataProvider.Instance.ExecuteQuery(query);
            
        }
    }
}
