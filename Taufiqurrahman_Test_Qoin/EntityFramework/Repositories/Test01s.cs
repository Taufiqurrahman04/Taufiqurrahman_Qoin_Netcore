using Taufiqurrahman_Test_Qoin.EntityFramework.Models;
using Taufiqurrahman_Test_Qoin.Services;

namespace Taufiqurrahman_Test_Qoin.EntityFramework.Repositories
{
    public class Test01s : ITest01s
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
        public bool CheckDbConnection()
        {
            return _ctx.Database.CanConnect();
        }
        public bool CheckDataExist(string name)
        {
            if(string.IsNullOrEmpty(name))
                return false;
            return _ctx.Test01.Any(x => x.Nama.Trim() == name.Trim());
        }

        public bool DeleteTest01(int id)
        {
            Test01 data = _ctx.Test01.First(x => x.Id == id);
            _ctx.Test01.Remove(data);
            return _ctx.SaveChanges() > 0;
        }
        
        public Test01 GetDetailsTest01(int id)
        {
            return _ctx.Test01.First(x => x.Id == id);
        }

        public IEnumerable<Test01> GetTest01s(Test01Parameters param)
        {
            return _ctx.Test01.ToList().OrderBy(x=>x.Id).Skip((param.pageNumber-1)*param.pageSize).Take(param.pageSize).AsEnumerable();
        }

        public bool InsertTest01(Test01 data)
        {
            _ctx.Test01.Add(new Test01
            {
                Nama = data.Nama,
                Status = data.Status,
                Created = data.Created
            });
            return _ctx.SaveChanges() > 0;
        }

        public bool UpdateTest01(Test01 data)
        {
            Test01 update = _ctx.Test01.First(x=>x.Id==data.Id);
           
                update.Nama = data.Nama;
                update.Status = data.Status;
                update.Updated = (data.Updated != null) ? data.Updated.Value : DateTime.Now;
                return _ctx.SaveChanges() > 0;
           
        }
    }
}
