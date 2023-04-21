using umweltV1.Core.Interfaces;
using umweltV1.Data.MyDbContext;

namespace umweltV1.Core.Repositories
{
    public class AdminRepository : IAdminService
    {
        private readonly MyDb _db;
        public AdminRepository(MyDb db)
        {
            _db = db;
        }



        #region Role




        #endregion
    }
}
