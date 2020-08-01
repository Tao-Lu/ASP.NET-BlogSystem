using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IDAL
{
    public interface ICategoryDAL: IBaseDAL<Category>
    {
        Category GetCategoryById(Guid categoryId);
        Category GetCategoryByName(string name);
    }
}
