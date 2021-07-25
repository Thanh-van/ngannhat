using ql_nhat.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ql_nhat.DAO
{

    class DataProvider
    {
        private String ConnectionSTR = "Data Source=MLC;Initial Catalog=QLBH;Integrated Security=True";
        
        private static DataProvider instance;

        public static DataProvider Instance {
            get
            {
                if (instance == null)
                {
                    instance = new DataProvider();
                }
                return  DataProvider.instance;
            }
            private set { DataProvider.instance = value; }
        }
        private DataProvider()
        { }
        

        public DataTable ExecuteQuery(String query,object[] parament =null)
        {
            DataTable table = new DataTable();
            using (SqlConnection Connect = new SqlConnection(ConnectionSTR))
            {
                Connect.Open();

                SqlCommand cmd = new SqlCommand(query, Connect);
              
                if(parament !=null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach(String item in listPara)
                    {
                        if(item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parament[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(table);
                Connect.Close();

            };
            return table;
        }

        public int ExecuteNonQuery(String query, object[] parament = null)
        {
            try
            {
                int data = 0;

                using (SqlConnection Connect = new SqlConnection(ConnectionSTR))
                {
                    Connect.Open();

                    SqlCommand cmd = new SqlCommand(query, Connect);

                    if (parament != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (String item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parament[i]);
                                i++;
                            }
                        }
                    }
                    data = cmd.ExecuteNonQuery();
                    Connect.Close();

                };
                return data;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        public object ExecuteScalar(String query, object[] parament = null)
        {
            object data = 0;

            using (SqlConnection Connect = new SqlConnection(ConnectionSTR))
            {
                Connect.Open();

                SqlCommand cmd = new SqlCommand(query, Connect);

                if (parament != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (String item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parament[i]);
                            i++;
                        }
                    }
                }
                data = cmd.ExecuteScalar();
                Connect.Close();

            };
            return data;
        }

        public String[] SqldataReader(String query)
        {
            String[] data=new string[3];

            using (SqlConnection Connect = new SqlConnection(ConnectionSTR))
            {
                Connect.Open();

                SqlCommand cmd = new SqlCommand(query, Connect);

                SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data[0]=(reader.GetValue(4).ToString());
                        data[1]= (reader.GetValue(12).ToString());
                        data[2]= (reader.GetValue(5).ToString());
                }
                
                Connect.Close();

            };
            return data;
        }

    }
}
