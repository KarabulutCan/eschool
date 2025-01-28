using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class ParameterRepository : IParameterRepository
    {
        private readonly SchoolDbContext _parameterContext;

        public ParameterRepository(SchoolDbContext parameterContext)
        {
            _parameterContext = parameterContext;
        }
        IEnumerable<Parameter> IParameterRepository.GetParameterAll()
        {
            return _parameterContext.Parameter.ToArray();
        }
        IEnumerable<Parameter> IParameterRepository.GetParameterMain(string categoryLevel)
        {
            return _parameterContext.Parameter.Where(p => p.CategoryLevel == categoryLevel && p.IsProtected == false).OrderBy(c => c.SortOrder).ToArray();
        }

        IEnumerable<Parameter> IParameterRepository.GetParameterSubID(int categorySubID)
        {
            return _parameterContext.Parameter.Where(p => p.CategorySubID == categorySubID && p.IsActive == true).OrderBy(c => c.SortOrder).ToList();
        }
        IEnumerable<Parameter> IParameterRepository.GetParameterSubIDOnlyTrue(int categorySubID)
        {
            return _parameterContext.Parameter.Where(p => p.CategorySubID == categorySubID && p.IsActive == true && p.IsSelect == true).OrderBy(c => c.SortOrder).ToList();
        }
        IEnumerable<Parameter> IParameterRepository.GetParameterSubIDOnlyTrue2(int categorySubID)
        {
            return _parameterContext.Parameter.Where(p => p.CategorySubID == categorySubID && p.IsActive == true).OrderBy(c => c.SortOrder).ToList();
        }

        IEnumerable<Parameter> IParameterRepository.GetParameterDetail(int ID, string categoryLevel)
        {
            return _parameterContext.Parameter.Where(p => p.CategorySubID == ID && p.CategoryLevel == categoryLevel).OrderBy(c => c.SortOrder).ToArray();
        }

        public Parameter GetParameter(int ID)
        {
            return _parameterContext.Parameter.Where(p => p.CategoryID == ID).FirstOrDefault();
        }
        public Parameter GetParameterCategoryName(string name)
        {
            return _parameterContext.Parameter.Where(p => p.CategoryName == name).FirstOrDefault();
        }
        public Parameter GetParameterCategoryName2(int categorySubID, string name)
        {
            return _parameterContext.Parameter.Where(p => p.CategorySubID == categorySubID && p.CategoryName == name).FirstOrDefault();
        }

        public bool CreateParameter(Parameter parameter)
        {
            _parameterContext.Add(parameter);
            return Save();
        }

        public bool DeleteParameter(Parameter parameter)
        {
            _parameterContext.Remove(parameter);
            return Save();
        }
        public bool UpdateParameter(Parameter parameter)
        {
            _parameterContext.Update(parameter);
            return Save();
        }

        public bool ExistParameter(int categoryID)
        {
            return _parameterContext.Parameter.Any(c => c.CategoryID == categoryID);
        }

        public bool ExistCategoryName(string categoryName)
        {
            return _parameterContext.Parameter.Any(c => c.CategoryName == categoryName && categoryName != null);
        }
        public bool ExistCategoryNameSub(int categorySubID, string categoryName)
        {
            return _parameterContext.Parameter.Any(c => c.CategorySubID == categorySubID && c.CategoryName == categoryName);
        }

        public bool Save()
        {
            var saved = _parameterContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
