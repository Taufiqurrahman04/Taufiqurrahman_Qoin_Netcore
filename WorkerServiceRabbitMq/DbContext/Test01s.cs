using Taufiqurrahman_Test_Qoin.ServiceWorker.Models;

namespace Taufiqurrahman_Test_Qoin.EntityFramework
{
    public class Test01s 
    {
        private AppDbContext _ctx;
        public Test01s(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool CheckDataExist(int id)
        {
            return _ctx.Test01.Any(x => x.Id == id);
        }

        public bool CheckDataExist(string name)
        {
            if(string.IsNullOrEmpty(name))
                return false;
            return _ctx.Test01.Any(x => x.Nama.Trim() == name.Trim());
        }

        public bool DeleteTest01(string id)
        {
            Test01 data = _ctx.Test01.First(x => x.Nama.Trim() == id.Trim());
            _ctx.Test01.Remove(data);
            return _ctx.SaveChanges() > 0;
        }
        public bool CheckDbConnection()
        {
            return _ctx.Database.CanConnect();
        }
        public Test01 GetDetailsTest01(int id)
        {
            return _ctx.Test01.First(x => x.Id == id);
        }

      
        public bool InsertTest01(Test01 data)
        {
            _ctx.Test01.Add(new Test01
            {
                Nama = data.Nama,
                Status = data.Status,
                Created = DateTime.Now
            });
            return _ctx.SaveChanges() > 0;
        }

        public bool UpdateTest01(Test01 data)
        {
            Test01 update = _ctx.Test01.First(x=>x.Nama.Trim()==data.Nama.Trim());
           
              //  update.Nama = data.Nama;
                update.Status = data.Status;
                update.Updated =  DateTime.Now;
                return _ctx.SaveChanges() > 0;
           
        }
    }
}
