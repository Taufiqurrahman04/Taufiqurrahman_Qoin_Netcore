using Taufiqurrahman_Test_Qoin.EntityFramework.Models;
using Taufiqurrahman_Test_Qoin.Services;

namespace Taufiqurrahman_Test_Qoin.EntityFramework.Repositories
{
    public interface ITest01s
    {
        IEnumerable<Test01> GetTest01s(Test01Parameters param);
        Test01 GetDetailsTest01(int id);
        bool InsertTest01(Test01 data);
        bool UpdateTest01(Test01 data);
        bool DeleteTest01(int id);
        bool CheckDataExist(int id);
        bool CheckDataExist(string name);
        bool CheckDbConnection();
    }
}
