using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ql_nhat.DTO
{
    class list_product
    {
        public list_product(String name,String dvt,int dg,int sl)
        {
            
            this.Name = name;
            this.dvt = dvt;
            this.Dongia = dg;
            this.Sl = sl;
        }
        private String dvt;
        private String name;
        private int dongia;
        private int sl;

        
        public string Name { get => name; set => name = value; }
        public int Dongia { get => dongia; set => dongia = value; }
        public int Sl { get => sl; set => sl = value; }
        public string Dvt { get => dvt; set => dvt = value; }
    }
}
